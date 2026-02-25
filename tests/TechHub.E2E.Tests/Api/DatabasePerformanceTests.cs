using System.Data;
using System.Diagnostics;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Npgsql;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Infrastructure.Data;
using TechHub.Infrastructure.Repositories;
using TechHub.TestUtilities;

namespace TechHub.E2E.Tests.Api;

/// <summary>
/// Performance tests for database queries against the live PostgreSQL database.
/// Requires PostgreSQL running (started automatically by the runner).
/// Uses a mock cache that NEVER returns cached values so every call hits the database directly.
///
/// Purpose: Validate that all database queries execute within acceptable thresholds against
/// a database with real data (~4000+ content items).
/// By testing at the repository level (no HTTP, no caching), we get accurate, stable
/// measurements of actual database query performance without environmental noise.
///
/// Coverage:
/// - Tag cloud queries (GetTagCountsAsync) — most complex GROUP BY + HAVING queries
/// - Search queries (SearchAsync) — FTS, tag filtering, date ranges, pagination
/// - Content detail (GetBySlugAsync) — single item lookups
/// </summary>
public class DatabasePerformanceTests : IDisposable
{
    private const string PostgresConnectionString =
        "Host=localhost;Database=techhub;Username=techhub;Password=localdev";

    // Thresholds for PostgreSQL — warm-cache steady-state (matches production behavior).
    // Each test runs one unmeasured warmup query to load data pages into shared_buffers/OS cache,
    // then measures the second call. This reflects real production where PostgreSQL caches persist
    // across application deployments (only the app restarts, not the database).
    //
    // CI multiplier: GitHub Actions runners have shared, slower I/O compared to dedicated hardware.
    // Apply a 3x multiplier when CI=true to avoid flaky failures from infrastructure variance.
    private static readonly bool IsCI = Environment.GetEnvironmentVariable("CI") == "true";
    private static readonly int CIMultiplier = IsCI ? 3 : 1;
    private static readonly int MaxAcceptableMs = 100 * CIMultiplier;
    private static readonly int MaxFtsMs = 200 * CIMultiplier;
    private static readonly int MaxTagsToCountMs = 250 * CIMultiplier;

    private readonly ContentRepository? _repository;
    private readonly IDbConnection? _connection;
    private readonly ILogger<ContentRepository> _logger;
    private readonly bool _databaseAvailable;

    public DatabasePerformanceTests()
    {
        _logger = LoggerFactory
            .Create(b => b.AddConsole().SetMinimumLevel(LogLevel.Debug))
            .CreateLogger<ContentRepository>();

        try
        {
            _connection = new NpgsqlConnection(PostgresConnectionString);
            _connection.Open();
            _databaseAvailable = true;
        }
        catch
        {
            _databaseAvailable = false;
            return;
        }

        // Mock cache that NEVER returns cached values — forces every call to hit the database
        var mockCache = new Mock<IMemoryCache>();
        mockCache
            .Setup(m => m.TryGetValue(It.IsAny<object>(), out It.Ref<object?>.IsAny))
            .Returns(false);
        mockCache
            .Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns(Mock.Of<ICacheEntry>());

        var mockMarkdownService = new Mock<IMarkdownService>();
        mockMarkdownService.Setup(m => m.RenderToHtml(It.IsAny<string>()))
            .Returns<string>(content => $"<p>{content}</p>");
        mockMarkdownService.Setup(m => m.ProcessYouTubeEmbeds(It.IsAny<string>()))
            .Returns<string>(content => content);
        mockMarkdownService.Setup(m => m.ExtractExcerpt(It.IsAny<string>(), It.IsAny<int>()))
            .Returns<string, int>((content, _) => content.Length > 100 ? content[..100] : content);

        // Load real AppSettings (section configuration needed for exclude-tag logic)
        var appSettings = ConfigurationHelper.LoadAppSettings();

        // Enable query logging so we get timing + EXPLAIN for slow queries
        var dbOptions = Options.Create(new DatabaseOptions
        {
            Provider = "PostgreSQL",
            ConnectionString = PostgresConnectionString,
            EnableQueryLogging = false
        });

        _repository = new ContentRepository(
            _connection,
            new PostgresDialect(),
            mockCache.Object,
            mockMarkdownService.Object,
            Options.Create(appSettings),
            _logger,
            dbOptions);
    }

    public void Dispose()
    {
        _connection?.Dispose();
        GC.SuppressFinalize(this);
    }

    private void SkipIfNoDatabase()
    {
        Assert.SkipWhen(!_databaseAvailable,
            "PostgreSQL not available. Run 'Run' to start PostgreSQL and the application.");
    }

    // ==================== Measurement Helpers ====================

    private async Task<long> MeasureTagCountsAsync(TagCountsRequest request)
    {
        // Warmup: run the exact query once to load relevant data pages into PostgreSQL/OS cache.
        // This simulates production where the DB stays warm across app deployments.
        await _repository!.GetTagCountsAsync(request, TestContext.Current.CancellationToken);

        var sw = Stopwatch.StartNew();
        var result = await _repository!.GetTagCountsAsync(request, TestContext.Current.CancellationToken);
        sw.Stop();

        _logger.LogInformation(
            "[PostgreSQL] TagCounts query returned {Count} tags in {ElapsedMs}ms (section={Section}, collection={Collection}, tags={Tags}, tagsToCount={TagsToCount}, search={Search})",
            result.Count,
            sw.ElapsedMilliseconds,
            request.SectionName,
            request.CollectionName,
            request.Tags != null ? string.Join(",", request.Tags) : "(none)",
            request.TagsToCount != null ? string.Join(",", request.TagsToCount) : "(none)",
            request.SearchQuery ?? "(none)");

        return sw.ElapsedMilliseconds;
    }

    private async Task<(long ElapsedMs, SearchResults<ContentItem> Results)> MeasureSearchAsync(SearchRequest request)
    {
        // Warmup: run the exact query once to load relevant data pages into PostgreSQL/OS cache.
        await _repository!.SearchAsync(request, TestContext.Current.CancellationToken);

        var sw = Stopwatch.StartNew();
        var result = await _repository!.SearchAsync(request, TestContext.Current.CancellationToken);
        sw.Stop();

        _logger.LogInformation(
            "[PostgreSQL] Search query returned {Count}/{Total} items in {ElapsedMs}ms (sections={Sections}, collections={Collections}, tags={Tags}, query={Query})",
            result.Items.Count,
            result.TotalCount,
            sw.ElapsedMilliseconds,
            string.Join(",", request.Sections),
            string.Join(",", request.Collections),
            request.Tags.Count > 0 ? string.Join(",", request.Tags) : "(none)",
            request.Query ?? "(none)");

        return (sw.ElapsedMilliseconds, result);
    }

    private async Task<(long ElapsedMs, ContentItemDetail? Result)> MeasureGetBySlugAsync(
        string collectionName, string slug)
    {
        // Warmup: run the exact query once to load relevant data pages into PostgreSQL/OS cache.
        await _repository!.GetBySlugAsync(collectionName, slug, ct: TestContext.Current.CancellationToken);

        var sw = Stopwatch.StartNew();
        var result = await _repository!.GetBySlugAsync(collectionName, slug, ct: TestContext.Current.CancellationToken);
        sw.Stop();

        _logger.LogInformation(
            "[PostgreSQL] GetBySlug returned {Found} in {ElapsedMs}ms (collection={Collection}, slug={Slug})",
            result != null ? "found" : "not found",
            sw.ElapsedMilliseconds,
            collectionName,
            slug);

        return (sw.ElapsedMilliseconds, result);
    }

    private void AssertPerformance(long elapsedMs, string operationName, int? overrideThresholdMs = null)
    {
        var threshold = overrideThresholdMs ?? MaxAcceptableMs;
        elapsedMs.Should().BeLessThan(threshold,
            $"{operationName} should complete within {threshold}ms on PostgreSQL (actual: {elapsedMs}ms)");
    }

    // ==================== Tag Cloud: TopN Queries ====================

    [Fact]
    public async Task TopTags_AllSections_AllCollections_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            maxTags: 20,
            minUses: 2);

        var elapsedMs = await MeasureTagCountsAsync(request);

        AssertPerformance(elapsedMs, "Homepage tag cloud (all/all)");
    }

    [Theory]
    [InlineData("github-copilot", "all")]
    [InlineData("ai", "all")]
    [InlineData("azure", "all")]
    [InlineData("devops", "all")]
    [InlineData("security", "all")]
    [InlineData("ml", "all")]
    public async Task TopTags_SpecificSection_AllCollections_PerformsWithinThreshold(
        string sectionName, string collectionName)
    {
        SkipIfNoDatabase();

        var request = new TagCountsRequest(
            sectionName: sectionName,
            collectionName: collectionName,
            maxTags: 20,
            minUses: 2);

        var elapsedMs = await MeasureTagCountsAsync(request);

        AssertPerformance(elapsedMs, $"Tag cloud for section '{sectionName}'");
    }

    [Theory]
    [InlineData("github-copilot", "blogs")]
    [InlineData("github-copilot", "videos")]
    [InlineData("ai", "blogs")]
    [InlineData("all", "blogs")]
    public async Task TopTags_SectionAndCollection_PerformsWithinThreshold(
        string sectionName, string collectionName)
    {
        SkipIfNoDatabase();

        var request = new TagCountsRequest(
            sectionName: sectionName,
            collectionName: collectionName,
            maxTags: 20,
            minUses: 1);

        var elapsedMs = await MeasureTagCountsAsync(request);

        AssertPerformance(elapsedMs, $"Tag cloud for '{sectionName}/{collectionName}'");
    }

    [Fact]
    public async Task TopTags_MaxTags50_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            maxTags: 50,
            minUses: 1);

        var elapsedMs = await MeasureTagCountsAsync(request);

        AssertPerformance(elapsedMs, "Tag cloud with maxTags=50");
    }

    [Fact]
    public async Task TopTags_HighMinUses_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            maxTags: 20,
            minUses: 10);

        var elapsedMs = await MeasureTagCountsAsync(request);

        AssertPerformance(elapsedMs, "Tag cloud with high minUses=10");
    }

    // ==================== Tag Cloud: TagsToCount (Dynamic Counts) ====================

    [Fact]
    public async Task TagsToCount_WithSelectedTags_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        var request = new TagCountsRequest(
            sectionName: "github-copilot",
            collectionName: "all",
            maxTags: 20,
            tags: ["vs code", "copilot coding agent"],
            tagsToCount: ["agent mode", "pull requests", "automation", "chat", "edits"]);

        var elapsedMs = await MeasureTagCountsAsync(request);

        AssertPerformance(elapsedMs, "TagsToCount with selected tags", MaxTagsToCountMs);
    }

    [Fact]
    public async Task TagsToCount_HomepageWithSelectedTag_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            maxTags: 20,
            tags: ["vs code"],
            tagsToCount: ["agent mode", "pull requests", "automation", "chat", "edits",
                          "copilot coding agent", "productivity", "testing", "security"]);

        var elapsedMs = await MeasureTagCountsAsync(request);

        AssertPerformance(elapsedMs, "TagsToCount on homepage with tag filter", MaxTagsToCountMs);
    }

    [Fact]
    public async Task TagsToCount_WithMultipleTags_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        var request = new TagCountsRequest(
            sectionName: "github-copilot",
            collectionName: "blogs",
            maxTags: 20,
            tags: ["vs code", "copilot coding agent", "agent mode"],
            tagsToCount: ["pull requests", "automation", "chat", "edits", "productivity"]);

        var elapsedMs = await MeasureTagCountsAsync(request);

        AssertPerformance(elapsedMs, "TagsToCount with 3 selected tags", MaxTagsToCountMs);
    }

    [Fact]
    public async Task TagsToCount_LargeTagSet_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        var request = new TagCountsRequest(
            sectionName: "github-copilot",
            collectionName: "all",
            maxTags: 50,
            tagsToCount: [
                "vs code", "agent mode", "pull requests", "automation",
                "chat", "edits", "productivity", "testing", "security",
                "copilot coding agent", "code review", "suggestions",
                "extensions", "workspace", "terminal"
            ]);

        var elapsedMs = await MeasureTagCountsAsync(request);

        AssertPerformance(elapsedMs, "TagsToCount with 15 specific tags", MaxTagsToCountMs);
    }

    // ==================== Tag Cloud: With Date Range ====================

    [Fact]
    public async Task TopTags_WithDateRange_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        var request = new TagCountsRequest(
            sectionName: "github-copilot",
            collectionName: "all",
            maxTags: 20,
            minUses: 1,
            dateFrom: DateTimeOffset.UtcNow.AddDays(-90),
            dateTo: DateTimeOffset.UtcNow);

        var elapsedMs = await MeasureTagCountsAsync(request);

        AssertPerformance(elapsedMs, "Tag cloud with 90-day date range", MaxTagsToCountMs);
    }

    [Fact]
    public async Task TagsToCount_WithDateRangeAndTags_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            maxTags: 20,
            dateFrom: DateTimeOffset.UtcNow.AddDays(-365),
            dateTo: DateTimeOffset.UtcNow,
            tags: ["vs code"],
            tagsToCount: ["agent mode", "pull requests", "automation", "chat"]);

        var elapsedMs = await MeasureTagCountsAsync(request);

        AssertPerformance(elapsedMs, "TagsToCount with date range and tag filter", MaxTagsToCountMs);
    }

    // ==================== Tag Cloud: With Search Query ====================

    [Fact]
    public async Task TopTags_WithSearchQuery_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            maxTags: 20,
            minUses: 1,
            searchQuery: "copilot");

        var elapsedMs = await MeasureTagCountsAsync(request);

        AssertPerformance(elapsedMs, "Tag cloud with FTS search query", MaxFtsMs);
    }

    // ==================== Search: Basic Queries ====================

    [Theory]
    [InlineData("ai", "blogs")]
    [InlineData("github-copilot", "videos")]
    [InlineData("azure", "blogs")]
    [InlineData("devops", "blogs")]
    [InlineData("security", "blogs")]
    public async Task Search_SpecificSectionAndCollection_PerformsWithinThreshold(
        string sectionName, string collectionName)
    {
        SkipIfNoDatabase();

        var request = new SearchRequest(
            take: 20,
            sections: [sectionName],
            collections: [collectionName],
            tags: []);

        var (elapsedMs, result) = await MeasureSearchAsync(request);

        result.Items.Should().NotBeEmpty();
        AssertPerformance(elapsedMs, $"Search {sectionName}/{collectionName}");
    }

    [Fact]
    public async Task Search_AllInSection_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        var request = new SearchRequest(
            take: 20,
            sections: ["ai"],
            collections: ["all"],
            tags: []);

        var (elapsedMs, result) = await MeasureSearchAsync(request);

        result.Items.Should().NotBeEmpty();
        AssertPerformance(elapsedMs, "Search all collections in section");
    }

    // ==================== Search: Tag Filtering ====================

    [Fact]
    public async Task Search_WithSingleTag_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        var request = new SearchRequest(
            take: 20,
            sections: ["ai"],
            collections: ["all"],
            tags: ["AI"]);

        var (elapsedMs, result) = await MeasureSearchAsync(request);

        result.Items.Should().NotBeEmpty();
        AssertPerformance(elapsedMs, "Search with single tag filter");
    }

    [Fact]
    public async Task Search_WithMultipleTags_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        var request = new SearchRequest(
            take: 20,
            sections: ["ai"],
            collections: ["all"],
            tags: ["AI", "GitHub"]);

        var (elapsedMs, _) = await MeasureSearchAsync(request);

        AssertPerformance(elapsedMs, "Search with multiple tags (AND logic)");
    }

    // ==================== Search: Full-Text Search ====================

    [Fact]
    public async Task Search_WithFtsQuery_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        var request = new SearchRequest(
            take: 20,
            sections: ["ai"],
            collections: ["all"],
            tags: [],
            query: "copilot");

        var (elapsedMs, result) = await MeasureSearchAsync(request);

        result.Items.Should().NotBeEmpty();
        AssertPerformance(elapsedMs, "Search with FTS query", MaxFtsMs);
    }

    [Fact]
    public async Task Search_WithFtsAndTagFilter_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        var request = new SearchRequest(
            take: 20,
            sections: ["ai"],
            collections: ["all"],
            tags: ["AI"],
            query: "copilot");

        var (elapsedMs, _) = await MeasureSearchAsync(request);

        AssertPerformance(elapsedMs, "Search with FTS + tag filter", MaxFtsMs);
    }

    [Fact]
    public async Task Search_WithFtsAndDateAndTags_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        var request = new SearchRequest(
            take: 20,
            sections: ["ai"],
            collections: ["all"],
            tags: ["AI"],
            query: "copilot",
            dateFrom: DateTimeOffset.UtcNow.AddDays(-90),
            dateTo: DateTimeOffset.UtcNow);

        var (elapsedMs, _) = await MeasureSearchAsync(request);

        AssertPerformance(elapsedMs, "Search with FTS + tags + date filter", MaxFtsMs);
    }

    // ==================== Search: Date Filtering ====================

    [Fact]
    public async Task Search_WithDateRange_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        var request = new SearchRequest(
            take: 20,
            sections: ["ai"],
            collections: ["all"],
            tags: [],
            dateFrom: DateTimeOffset.UtcNow.AddDays(-30),
            dateTo: DateTimeOffset.UtcNow);

        var (elapsedMs, _) = await MeasureSearchAsync(request);

        AssertPerformance(elapsedMs, "Search with 30-day date range");
    }

    // ==================== Search: Subcollection Filtering ====================

    [Fact]
    public async Task Search_WithSubcollection_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        var request = new SearchRequest(
            take: 20,
            sections: ["github-copilot"],
            collections: ["videos"],
            tags: [],
            subcollection: "ghc-features");

        var (elapsedMs, _) = await MeasureSearchAsync(request);

        AssertPerformance(elapsedMs, "Search with subcollection filter");
    }

    // ==================== Search: Pagination ====================

    [Theory]
    [InlineData(0)]
    [InlineData(20)]
    [InlineData(40)]
    public async Task Search_DifferentOffsets_MaintainsPerformance(int skip)
    {
        SkipIfNoDatabase();

        var request = new SearchRequest(
            take: 20,
            sections: ["ai"],
            collections: ["all"],
            tags: [],
            skip: skip);

        var (elapsedMs, _) = await MeasureSearchAsync(request);

        AssertPerformance(elapsedMs, $"Search with skip={skip}");
    }

    // ==================== Content Detail ====================

    [Fact]
    public async Task GetBySlug_ValidItem_PerformsWithinThreshold()
    {
        SkipIfNoDatabase();

        // First find a valid slug to look up
        var searchResult = await _repository!.SearchAsync(
            new SearchRequest(take: 1, sections: ["all"], collections: ["roundups"], tags: []),
            TestContext.Current.CancellationToken);
        searchResult.Items.Should().NotBeEmpty("need at least one roundup item to test");

        var item = searchResult.Items[0];

        // Now measure the actual GetBySlug call
        var (elapsedMs, result) = await MeasureGetBySlugAsync(item.CollectionName, item.Slug);

        result.Should().NotBeNull();
        AssertPerformance(elapsedMs, "GetBySlug single item lookup");
    }
}
