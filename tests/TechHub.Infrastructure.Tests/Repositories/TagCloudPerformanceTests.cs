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

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Performance tests for tag cloud queries against the live PostgreSQL database (docker-compose).
/// Uses a mock cache that never returns cached values so every call hits the database.
///
/// These tests require PostgreSQL to be running on localhost:5432.
/// PostgreSQL is started automatically by the runner via docker-compose.
/// Tests are automatically skipped if PostgreSQL is not reachable.
///
/// Purpose: Validate that tag cloud queries, which are the most complex queries in the system,
/// execute within 1 second for all parameter combinations against the production-like PostgreSQL
/// database with real data (~4000+ content items).
/// </summary>
public class TagCloudPerformanceTests : IDisposable
{
    private const string PostgresConnectionString =
        "Host=localhost;Database=techhub;Username=techhub;Password=localdev";

    private const int MaxAcceptableMs = 500;

    private readonly ContentRepository? _repository;
    private readonly IDbConnection? _connection;
    private readonly ILogger<ContentRepository> _logger;
    private readonly bool _postgresAvailable;

    public TagCloudPerformanceTests()
    {
        _logger = LoggerFactory
            .Create(b => b.AddConsole().SetMinimumLevel(LogLevel.Debug))
            .CreateLogger<ContentRepository>();

        // Check PostgreSQL availability before setting up
        _postgresAvailable = IsPostgresAvailable();
        if (!_postgresAvailable)
        {
            return;
        }

        // Open a live connection to the Docker PostgreSQL instance
        _connection = new NpgsqlConnection(PostgresConnectionString);
        _connection.Open();

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
            EnableQueryLogging = false, // Set to true to log all queries with EXPLAIN for any that exceed the MaxAcceptableMs threshold
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

    private static bool IsPostgresAvailable()
    {
        try
        {
            using var conn = new NpgsqlConnection(PostgresConnectionString);
            conn.Open();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void SkipIfNoPostgres()
    {
        Assert.SkipWhen(!_postgresAvailable,
            "PostgreSQL not available on localhost:5432. Run 'Run' to start servers (PostgreSQL starts automatically).");
    }

    private async Task<long> MeasureTagCountsAsync(TagCountsRequest request)
    {
        var sw = Stopwatch.StartNew();
        var result = await _repository!.GetTagCountsAsync(request, TestContext.Current.CancellationToken);
        sw.Stop();

        _logger.LogInformation(
            "TagCounts query returned {Count} tags in {ElapsedMs}ms (section={Section}, collection={Collection}, tags={Tags}, tagsToCount={TagsToCount}, search={Search})",
            result.Count,
            sw.ElapsedMilliseconds,
            request.SectionName,
            request.CollectionName,
            request.Tags != null ? string.Join(",", request.Tags) : "(none)",
            request.TagsToCount != null ? string.Join(",", request.TagsToCount) : "(none)",
            request.SearchQuery ?? "(none)");

        return sw.ElapsedMilliseconds;
    }

    // ==================== TopN Tag Cloud Queries ====================

    [Fact]
    public async Task TopTags_AllSections_AllCollections_PerformsUnder1Second()
    {
        SkipIfNoPostgres();

        // Arrange - Homepage tag cloud (no filters)
        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            maxTags: 20,
            minUses: 2);

        // Act
        var elapsedMs = await MeasureTagCountsAsync(request);

        // Assert
        elapsedMs.Should().BeLessThan(MaxAcceptableMs,
            "Homepage tag cloud (all/all) should complete within 1 second on PostgreSQL");
    }

    [Theory]
    [InlineData("github-copilot", "all")]
    [InlineData("ai", "all")]
    [InlineData("azure", "all")]
    [InlineData("devops", "all")]
    [InlineData("security", "all")]
    [InlineData("ml", "all")]
    public async Task TopTags_SpecificSection_AllCollections_PerformsUnder1Second(
        string sectionName, string collectionName)
    {
        SkipIfNoPostgres();

        // Arrange - Section page tag cloud
        var request = new TagCountsRequest(
            sectionName: sectionName,
            collectionName: collectionName,
            maxTags: 20,
            minUses: 2);

        // Act
        var elapsedMs = await MeasureTagCountsAsync(request);

        // Assert
        elapsedMs.Should().BeLessThan(MaxAcceptableMs,
            $"Tag cloud for section '{sectionName}' should complete within 1 second on PostgreSQL");
    }

    [Theory]
    [InlineData("github-copilot", "blogs")]
    [InlineData("github-copilot", "videos")]
    [InlineData("ai", "blogs")]
    [InlineData("all", "blogs")]
    public async Task TopTags_SectionAndCollection_PerformsUnder1Second(
        string sectionName, string collectionName)
    {
        SkipIfNoPostgres();

        // Arrange - Collection page tag cloud
        var request = new TagCountsRequest(
            sectionName: sectionName,
            collectionName: collectionName,
            maxTags: 20,
            minUses: 1);

        // Act
        var elapsedMs = await MeasureTagCountsAsync(request);

        // Assert
        elapsedMs.Should().BeLessThan(MaxAcceptableMs,
            $"Tag cloud for '{sectionName}/{collectionName}' should complete within 1 second on PostgreSQL");
    }

    // ==================== TagsToCount Queries (Dynamic Counts) ====================

    [Fact]
    public async Task TagsToCount_WithSelectedTags_PerformsUnder1Second()
    {
        SkipIfNoPostgres();

        // Arrange - Dynamic tag counts after user selects tags (intersection query)
        // tagsToCount must be real full tags (not word fragments) since the query returns MAX(tag_display)
        var request = new TagCountsRequest(
            sectionName: "github-copilot",
            collectionName: "all",
            maxTags: 20,
            tags: ["vs code", "copilot coding agent"],
            tagsToCount: ["agent mode", "pull requests", "code review", "collaboration", "developer tools"]);

        // Act
        var elapsedMs = await MeasureTagCountsAsync(request);

        // Assert
        elapsedMs.Should().BeLessThan(MaxAcceptableMs,
            "TagsToCount with selected tags should complete within 1 second on PostgreSQL");
    }

    [Fact]
    public async Task TagsToCount_HomepageWithSelectedTag_PerformsUnder1Second()
    {
        SkipIfNoPostgres();

        // Arrange - Homepage with one tag selected
        // tagsToCount must be real full tags (not word fragments) since the query returns MAX(tag_display)
        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            maxTags: 20,
            tags: ["vs code"],
            tagsToCount: ["agent mode", "pull requests", "code review",
                          "copilot coding agent", "developer tools", "security", "collaboration", "cloud computing", "test tag"]);

        // Act
        var elapsedMs = await MeasureTagCountsAsync(request);

        // Assert
        elapsedMs.Should().BeLessThan(MaxAcceptableMs,
            "TagsToCount on homepage with tag filter should complete within 1 second");
    }

    [Fact]
    public async Task TagsToCount_WithMultipleTags_PerformsUnder1Second()
    {
        SkipIfNoPostgres();

        // Arrange - Multiple tags selected (deeper intersection query)
        // tagsToCount must be real full tags (not word fragments) since the query returns MAX(tag_display)
        var request = new TagCountsRequest(
            sectionName: "github-copilot",
            collectionName: "blogs",
            maxTags: 20,
            tags: ["vs code", "copilot coding agent", "agent mode"],
            tagsToCount: ["pull requests", "code review", "collaboration", "developer tools", "developer productivity"]);

        // Act
        var elapsedMs = await MeasureTagCountsAsync(request);

        // Assert
        elapsedMs.Should().BeLessThan(MaxAcceptableMs,
            "TagsToCount with 3 selected tags should complete within 1 second on PostgreSQL");
    }

    // ==================== With Date Range Filter ====================

    [Fact]
    public async Task TopTags_WithDateRange_PerformsUnder1Second()
    {
        SkipIfNoPostgres();

        // Arrange - Tag cloud with 90-day date range filter
        var request = new TagCountsRequest(
            sectionName: "github-copilot",
            collectionName: "all",
            maxTags: 20,
            minUses: 1,
            dateFrom: DateTimeOffset.UtcNow.AddDays(-90),
            dateTo: DateTimeOffset.UtcNow);

        // Act
        var elapsedMs = await MeasureTagCountsAsync(request);

        // Assert
        elapsedMs.Should().BeLessThan(MaxAcceptableMs,
            "Tag cloud with 90-day date range should complete within 1 second on PostgreSQL");
    }

    [Fact]
    public async Task TagsToCount_WithDateRangeAndTags_PerformsUnder1Second()
    {
        SkipIfNoPostgres();

        // Arrange - Dynamic counts with date range + tag filter (worst case combo)
        // tagsToCount must be real full tags (not word fragments) since the query returns MAX(tag_display)
        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            maxTags: 20,
            dateFrom: DateTimeOffset.UtcNow.AddDays(-365),
            dateTo: DateTimeOffset.UtcNow,
            tags: ["vs code"],
            tagsToCount: ["agent mode", "pull requests", "code review", "collaboration"]);

        // Act
        var elapsedMs = await MeasureTagCountsAsync(request);

        // Assert
        elapsedMs.Should().BeLessThan(MaxAcceptableMs,
            "TagsToCount with date range and tag filter should complete within 1 second");
    }

    // ==================== With Search Query ====================

    [Fact]
    public async Task TopTags_WithSearchQuery_PerformsUnder1Second()
    {
        SkipIfNoPostgres();

        // Arrange - Tag cloud filtered by full-text search
        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            maxTags: 20,
            minUses: 1,
            searchQuery: "copilot");

        // Act
        var elapsedMs = await MeasureTagCountsAsync(request);

        // Assert
        elapsedMs.Should().BeLessThan(MaxAcceptableMs,
            "Tag cloud with search query should complete within 1 second on PostgreSQL");
    }

    // ==================== Edge Cases ====================

    [Fact]
    public async Task TopTags_MaxTags50_PerformsUnder1Second()
    {
        SkipIfNoPostgres();

        // Arrange - Maximum allowed tags (largest result set)
        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            maxTags: 50,
            minUses: 1);

        // Act
        var elapsedMs = await MeasureTagCountsAsync(request);

        // Assert
        elapsedMs.Should().BeLessThan(MaxAcceptableMs,
            "Tag cloud with maxTags=50 should complete within 1 second on PostgreSQL");
    }

    [Fact]
    public async Task TopTags_HighMinUses_PerformsUnder1Second()
    {
        SkipIfNoPostgres();

        // Arrange - High minUses filter (tests HAVING clause efficiency)
        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            maxTags: 20,
            minUses: 10);

        // Act
        var elapsedMs = await MeasureTagCountsAsync(request);

        // Assert
        elapsedMs.Should().BeLessThan(MaxAcceptableMs,
            "Tag cloud with high minUses should complete within 1 second on PostgreSQL");
    }

    [Fact]
    public async Task TagsToCount_LargeTagSet_PerformsUnder1Second()
    {
        SkipIfNoPostgres();

        // Arrange - Count a large number of specific tags (tests IN clause scaling)
        // tagsToCount must be real full tags (not word fragments) since the query returns MAX(tag_display)
        var request = new TagCountsRequest(
            sectionName: "github-copilot",
            collectionName: "all",
            maxTags: 50,
            tagsToCount: [
                "vs code", "agent mode", "pull requests", "code review",
                "collaboration", "developer tools", "security", "test tag",
                "copilot coding agent", "cloud computing", "developer productivity",
                ".net", "copilot", "devops", "azure"
            ]);

        // Act
        var elapsedMs = await MeasureTagCountsAsync(request);

        // Assert
        elapsedMs.Should().BeLessThan(MaxAcceptableMs,
            "TagsToCount with 15 tags should complete within 1 second on PostgreSQL");
    }
}
