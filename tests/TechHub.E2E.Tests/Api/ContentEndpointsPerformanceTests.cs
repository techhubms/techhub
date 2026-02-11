using System.Diagnostics;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Models;
using TechHub.TestUtilities;

namespace TechHub.E2E.Tests.Repositories;

/// <summary>
/// E2E Performance tests using HTTP API calls for REALISTIC production measurements.
/// Tests measure full request/response cycle including middleware, serialization, database queries.
/// 
/// Uses:
/// - TechHubE2ETestApiFactory which runs both API and database
/// - REAL HTTP calls via HttpClient (not direct repository calls)
/// - Production data: 4000+ actual content items from markdown files
/// - Tests work with both SQLite (default) and PostgreSQL (Run -Docker)
/// 
/// Performance thresholds:
/// - PostgreSQL: 200ms standard, 800ms FTS, 400ms tag filtering (stricter due to superior performance)
/// - SQLite: 300ms standard, 1200ms FTS, 600ms tag filtering (relaxed due to temp B-tree overhead)
/// 
/// Why HTTP-based tests?
/// - Measures real-world performance including all layers (HTTP, middleware, serialization, DB)
/// - Validates caching, compression, and other HTTP-level optimizations
/// - Works identically whether using SQLite or PostgreSQL
/// - When Run -Docker is used, automatically tests against PostgreSQL
/// </summary>
public class ContentEndpointsPerformanceTests : IClassFixture<TechHubE2ETestApiFactory>
{
    private readonly HttpClient _client;
    private readonly bool _isPostgreSQL;

    // PostgreSQL thresholds (stricter - in-memory hash aggregation)
    private const int PostgresMaxResponseTimeMs = 200;
    private const int PostgresMaxFtsResponseTimeMs = 500;
    private const int PostgresMaxTagFilterResponseTimeMs = 300;

    // SQLite thresholds (relaxed - uses temp B-trees for GROUP BY/ORDER BY)
    private const int SqliteMaxResponseTimeMs = 300;
    private const int SqliteMaxFtsResponseTimeMs = 1000;
    private const int SqliteMaxTagFilterResponseTimeMs = 500;

    public ContentEndpointsPerformanceTests(TechHubE2ETestApiFactory factory)
    {
        _client = factory.CreateClient();

        // Detect database provider from configuration
        var config = factory.Services.GetRequiredService<IConfiguration>();
        var provider = config["Database:Provider"] ?? "SQLite";
        _isPostgreSQL = provider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase);

        // Note: Database is seeded once on factory startup via ContentSyncService
    }

    #region Helper Methods

    private static async Task<long> MeasureHttpGetAsync<T>(HttpClient client, string url)
    {
        var sw = Stopwatch.StartNew();
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        _ = await response.Content.ReadFromJsonAsync<T>(TestContext.Current.CancellationToken);
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }

    private void AssertPerformance(long elapsedMs, string operationName, int? overrideThresholdMs = null)
    {
        // Use override threshold if provided, otherwise use database-specific threshold
        var threshold = overrideThresholdMs ?? (_isPostgreSQL ? PostgresMaxResponseTimeMs : SqliteMaxResponseTimeMs);
        var dbType = _isPostgreSQL ? "PostgreSQL" : "SQLite";

        if (elapsedMs > threshold)
        {
            throw new Exception(
                $"Performance degradation detected in {operationName} ({dbType}): " +
                $"{elapsedMs}ms > {threshold}ms threshold. " +
                $"Check database indexes, query plans, and HTTP middleware overhead.");
        }
    }

    private void AssertFtsPerformance(long elapsedMs, string operationName)
    {
        var threshold = _isPostgreSQL ? PostgresMaxFtsResponseTimeMs : SqliteMaxFtsResponseTimeMs;
        AssertPerformance(elapsedMs, operationName, threshold);
    }

    private void AssertTagFilterPerformance(long elapsedMs, string operationName)
    {
        var threshold = _isPostgreSQL ? PostgresMaxTagFilterResponseTimeMs : SqliteMaxTagFilterResponseTimeMs;
        AssertPerformance(elapsedMs, operationName, threshold);
    }

    #endregion

    #region Single Item Detail Tests

    [Fact]
    public async Task GetContentDetail_ValidSlug_ReturnsItem_WithinPerformanceThreshold()
    {
        // Arrange - using roundups collection which links internally (blogs/news/community link externally and return 404)
        var url = "/api/sections/all/collections/roundups/weekly-ai-and-tech-news-roundup";

        // Act - measure HTTP request/response cycle
        var elapsed = await MeasureHttpGetAsync<ContentItemDetail>(_client, url);

        // Assert performance - single item by slug should be very fast
        AssertPerformance(elapsed, "GET content detail (single item by slug)");
    }

    #endregion

    #region Collection Items Tests

    [Fact]
    public async Task GetCollectionItems_Blogs_ReturnsItems_WithinPerformanceThreshold()
    {
        // Arrange
        var url = "/api/sections/ai/collections/blogs/items?take=20";

        // Act - measure paginated collection query
        var sw = Stopwatch.StartNew();
        var response = await _client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var items = await response.Content.ReadFromJsonAsync<IEnumerable<ContentItem>>(TestContext.Current.CancellationToken);
        sw.Stop();

        // Assert
        items.Should().NotBeNull();
        items!.Should().AllSatisfy(item => item.CollectionName.Should().Be("blogs"));

        AssertPerformance(sw.ElapsedMilliseconds, "GET /items (blogs collection - 20 items)");
    }

    [Fact]
    public async Task GetCollectionItems_Videos_ReturnsItems_WithinPerformanceThreshold()
    {
        // Arrange
        var url = "/api/sections/github-copilot/collections/videos/items?take=20";

        // Act
        var elapsed = await MeasureHttpGetAsync<IEnumerable<ContentItem>>(_client, url);

        // Assert performance
        AssertPerformance(elapsed, "GET /items (videos collection - 20 items)");
    }

    [Fact]
    public async Task GetCollectionItems_AllInSection_ReturnsItems_WithinPerformanceThreshold()
    {
        // Arrange - "all" is virtual collection showing all items in section
        var url = "/api/sections/ai/collections/all/items?take=20";

        // Act
        var elapsed = await MeasureHttpGetAsync<IEnumerable<ContentItem>>(_client, url);

        // Assert performance - section-level query
        AssertPerformance(elapsed, "GET /items (all collections in section - 20 items)");
    }

    #endregion

    #region Tag Cloud Performance Tests

    [Fact]
    public async Task GetCollectionTags_Homepage_ReturnsTopTags_WithinPerformanceThreshold()
    {
        // Arrange - getting tag cloud for entire site (most expensive query)
        var url = "/api/sections/ai/collections/all/tags?maxTags=50";

        // Act - CRITICAL PERFORMANCE TEST: homepage tag cloud
        var sw = Stopwatch.StartNew();
        var response = await _client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var tags = await response.Content.ReadFromJsonAsync<IReadOnlyList<TagCloudItem>>(TestContext.Current.CancellationToken);
        sw.Stop();

        // Assert
        tags.Should().NotBeNull();
        tags!.Count.Should().BeLessThanOrEqualTo(50);
        tags.Should().BeInDescendingOrder(t => t.Count);

        AssertPerformance(sw.ElapsedMilliseconds, "GET /tags (section tag cloud - top 50)");
    }

    [Theory]
    [InlineData("ai")]
    [InlineData("github-copilot")]
    [InlineData("azure")]
    public async Task GetCollectionTags_SpecificSection_ReturnsTopTags_WithinPerformanceThreshold(
        string sectionName)
    {
        // Arrange
        var url = $"/api/sections/{sectionName}/collections/all/tags?maxTags=50";

        // Act - section-specific tag cloud
        var elapsed = await MeasureHttpGetAsync<IReadOnlyList<TagCloudItem>>(_client, url);

        // Assert performance
        AssertPerformance(elapsed, $"GET /tags ({sectionName} section - top 50)");
    }

    [Theory]
    [InlineData("ai", "blogs")]
    [InlineData("github-copilot", "videos")]
    public async Task GetCollectionTags_SectionAndCollection_ReturnsTopTags_WithinPerformanceThreshold(
        string sectionName, string collectionName)
    {
        // Arrange
        var url = $"/api/sections/{sectionName}/collections/{collectionName}/tags?maxTags=30";

        // Act - most specific tag cloud (section + collection)
        var elapsed = await MeasureHttpGetAsync<IReadOnlyList<TagCloudItem>>(_client, url);

        // Assert performance
        AssertPerformance(elapsed, $"GET /tags ({sectionName}/{collectionName} - top 30)");
    }

    [Fact]
    public async Task GetCollectionTags_WithTagsToCount_ReturnsSpecificTags_WithinPerformanceThreshold()
    {
        // Arrange - First get baseline tags to know which tags to request
        var baselineUrl = "/api/sections/github-copilot/collections/all/tags?maxTags=20";
        var baselineResponse = await _client.GetAsync(baselineUrl, TestContext.Current.CancellationToken);
        baselineResponse.EnsureSuccessStatusCode();
        var baselineTags = await baselineResponse.Content.ReadFromJsonAsync<IReadOnlyList<TagCloudItem>>(TestContext.Current.CancellationToken);
        baselineTags.Should().NotBeNull();
        baselineTags!.Count.Should().BeGreaterThan(5, "Need baseline tags for test");

        // Build tagsToCount parameter with all baseline tag names
        var tagsToCountParam = string.Join(",", baselineTags!.Select(t => Uri.EscapeDataString(t.Tag)));
        var url = $"/api/sections/github-copilot/collections/all/tags?tagsToCount={tagsToCountParam}";

        // Act - PERFORMANCE TEST: tagsToCount with ~20 specific tags
        var sw = Stopwatch.StartNew();
        var response = await _client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var tags = await response.Content.ReadFromJsonAsync<IReadOnlyList<TagCloudItem>>(TestContext.Current.CancellationToken);
        sw.Stop();

        // Assert
        tags.Should().NotBeNull();
        // Should return all requested tags (tagsToCount bypasses maxTags)
        tags!.Count.Should().BeGreaterThan(0);

        // Performance should be similar to normal tag cloud query
        AssertPerformance(sw.ElapsedMilliseconds, "GET /tags with tagsToCount (20 specific tags)");
    }

    [Fact]
    public async Task GetCollectionTags_WithTagsToCount_AndTagFilter_ReturnsFilteredCounts_WithinPerformanceThreshold()
    {
        // Arrange - Get baseline tags first
        var baselineUrl = "/api/sections/github-copilot/collections/all/tags?maxTags=20";
        var baselineResponse = await _client.GetAsync(baselineUrl, TestContext.Current.CancellationToken);
        baselineResponse.EnsureSuccessStatusCode();
        var baselineTags = await baselineResponse.Content.ReadFromJsonAsync<IReadOnlyList<TagCloudItem>>(TestContext.Current.CancellationToken);
        baselineTags.Should().NotBeNull();
        baselineTags!.Count.Should().BeGreaterThan(2, "Need baseline tags for test");

        // Use first tag as filter, request counts for remaining tags
        var filterTag = Uri.EscapeDataString(baselineTags![0].Tag);
        var tagsToCountParam = string.Join(",", baselineTags!.Skip(1).Select(t => Uri.EscapeDataString(t.Tag)));
        var url = $"/api/sections/github-copilot/collections/all/tags?tags={filterTag}&tagsToCount={tagsToCountParam}";

        // Act - CRITICAL PERFORMANCE TEST: This is what happens when filtering tags in UI
        var sw = Stopwatch.StartNew();
        var response = await _client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var tags = await response.Content.ReadFromJsonAsync<IReadOnlyList<TagCloudItem>>(TestContext.Current.CancellationToken);
        sw.Stop();

        // Assert
        tags.Should().NotBeNull();

        // Tag filtering with tagsToCount uses intersection counting (more complex query)
        AssertTagFilterPerformance(sw.ElapsedMilliseconds, "GET /tags with tagsToCount + tag filter (intersection)");
    }

    [Fact]
    public async Task GetCollectionTags_TagsToCount_VsNormal_SimilarPerformance()
    {
        // This test ensures tagsToCount doesn't introduce significant performance regression

        // Arrange - Get baseline tags
        var baselineUrl = "/api/sections/github-copilot/collections/all/tags?maxTags=20";
        var baselineResponse = await _client.GetAsync(baselineUrl, TestContext.Current.CancellationToken);
        baselineResponse.EnsureSuccessStatusCode();
        var baselineTags = await baselineResponse.Content.ReadFromJsonAsync<IReadOnlyList<TagCloudItem>>(TestContext.Current.CancellationToken);
        baselineTags!.Count.Should().BeGreaterThan(5, "Need baseline tags for test");

        var tagsToCountParam = string.Join(",", baselineTags!.Select(t => Uri.EscapeDataString(t.Tag)));

        // Act - Compare normal query vs tagsToCount query
        var normalUrl = "/api/sections/github-copilot/collections/all/tags?maxTags=20";
        var tagsToCountUrl = $"/api/sections/github-copilot/collections/all/tags?tagsToCount={tagsToCountParam}";

        var normalElapsed = await MeasureHttpGetAsync<IReadOnlyList<TagCloudItem>>(_client, normalUrl);
        var tagsToCountElapsed = await MeasureHttpGetAsync<IReadOnlyList<TagCloudItem>>(_client, tagsToCountUrl);

        // Assert - tagsToCount should not be significantly slower than normal query
        // Allow 50% overhead for the additional filtering logic
        var maxAllowedTime = normalElapsed * 1.5 + 50; // 50% overhead + 50ms buffer for variance
        tagsToCountElapsed.Should().BeLessThan((long)maxAllowedTime,
            $"tagsToCount query ({tagsToCountElapsed}ms) should not be significantly slower than normal query ({normalElapsed}ms)");
    }

    #endregion

    #region Filtered Items Tests

    [Fact]
    public async Task GetCollectionItems_WithTagFilter_ReturnsFiltered_WithinPerformanceThreshold()
    {
        // Arrange
        var url = "/api/sections/ai/collections/all/items?tags=AI&take=20";

        // Act - tag filtering (critical for filtering UI)
        var sw = Stopwatch.StartNew();
        var response = await _client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var items = await response.Content.ReadFromJsonAsync<IEnumerable<ContentItem>>(TestContext.Current.CancellationToken);
        sw.Stop();

        // Assert
        items.Should().NotBeNull();

        // Tag filtering uses complex GROUP BY + HAVING subquery, so uses higher threshold
        AssertTagFilterPerformance(sw.ElapsedMilliseconds, "GET /items with tag filter");
    }

    [Fact]
    public async Task GetCollectionItems_WithMultipleTags_ReturnsFiltered_WithinPerformanceThreshold()
    {
        // Arrange - multiple tags use AND logic
        var url = "/api/sections/ai/collections/all/items?tags=AI,GitHub&take=20";

        // Act
        var elapsed = await MeasureHttpGetAsync<IEnumerable<ContentItem>>(_client, url);

        // Assert performance - multiple tag filter with AND logic (complex GROUP BY + HAVING)
        AssertTagFilterPerformance(elapsed, "GET /items with multiple tags (AND logic)");
    }

    [Fact]
    public async Task GetCollectionItems_WithDateFilter_ReturnsRecent_WithinPerformanceThreshold()
    {
        // Arrange
        var url = "/api/sections/ai/collections/all/items?lastDays=30&take=20";

        // Act
        var elapsed = await MeasureHttpGetAsync<IEnumerable<ContentItem>>(_client, url);

        // Assert performance - date range filtering
        AssertPerformance(elapsed, "GET /items with date filter (last 30 days)");
    }

    [Fact]
    public async Task GetCollectionItems_WithSearchQuery_ReturnsResults_WithinPerformanceThreshold()
    {
        // Arrange
        var url = "/api/sections/ai/collections/all/items?q=copilot&take=20";

        // Act - full-text search query
        var elapsed = await MeasureHttpGetAsync<IEnumerable<ContentItem>>(_client, url);

        // Assert performance - FTS uses higher threshold
        AssertFtsPerformance(elapsed, "GET /items with search query (FTS)");
    }

    [Fact]
    public async Task GetCollectionItems_WithSearchAndFilters_ReturnsResults_WithinPerformanceThreshold()
    {
        // Arrange - combined FTS + tag + date filters
        var url = "/api/sections/ai/collections/all/items?q=copilot&tags=AI&lastDays=90&take=20";

        // Act
        var elapsed = await MeasureHttpGetAsync<IEnumerable<ContentItem>>(_client, url);

        // Assert performance - FTS with filters uses higher threshold
        AssertFtsPerformance(elapsed, "GET /items with search + filters (FTS + tags + date)");
    }

    [Fact]
    public async Task GetCollectionItems_WithSubcollectionFilter_ReturnsFiltered_WithinPerformanceThreshold()
    {
        // Arrange
        var url = "/api/sections/github-copilot/collections/videos/items?subcollection=ghc-features&take=20";

        // Act
        var elapsed = await MeasureHttpGetAsync<IEnumerable<ContentItem>>(_client, url);

        // Assert performance - subcollection filtering
        AssertPerformance(elapsed, "GET /items with subcollection filter");
    }

    #endregion

    #region Pagination Performance Tests

    [Theory]
    [InlineData(0)]
    [InlineData(20)]
    [InlineData(40)]
    public async Task GetCollectionItems_DifferentOffsets_MaintainsPerformance(int skip)
    {
        // Arrange
        var url = $"/api/sections/ai/collections/all/items?take=20&skip={skip}";

        // Act - test pagination at different offsets
        var elapsed = await MeasureHttpGetAsync<IEnumerable<ContentItem>>(_client, url);

        // Assert performance - should be consistent across pages
        AssertPerformance(elapsed, $"GET /items with skip={skip}");
    }

    #endregion

    #region Cross-Section Performance Tests

    [Theory]
    [InlineData("ai", "blogs")]
    [InlineData("github-copilot", "blogs")]
    [InlineData("azure", "blogs")]
    [InlineData("devops", "blogs")]
    [InlineData("security", "blogs")]
    public async Task GetCollectionItems_DifferentSections_MaintainsPerformance(
        string sectionName, string collectionName)
    {
        // Arrange
        var url = $"/api/sections/{sectionName}/collections/{collectionName}/items?take=20";

        // Act - verify performance across different sections
        var elapsed = await MeasureHttpGetAsync<IEnumerable<ContentItem>>(_client, url);

        // Assert performance - should be consistent across sections
        AssertPerformance(elapsed, $"GET /items ({sectionName}/{collectionName})");
    }

    #endregion

    #region Metadata Endpoints Performance Tests

    [Fact]
    public async Task GetAllSections_ReturnsAllSections_WithinPerformanceThreshold()
    {
        // Arrange
        var url = "/api/sections/";

        // Act
        var elapsed = await MeasureHttpGetAsync<IEnumerable<Section>>(_client, url);

        // Assert performance - metadata endpoint (no DB query, config-based)
        AssertPerformance(elapsed, "GET /sections (all sections metadata)", 100);
    }

    [Theory]
    [InlineData("ai")]
    [InlineData("github-copilot")]
    [InlineData("azure")]
    public async Task GetSectionByName_ReturnsSection_WithinPerformanceThreshold(string sectionName)
    {
        // Arrange
        var url = $"/api/sections/{sectionName}";

        // Act
        var elapsed = await MeasureHttpGetAsync<Section>(_client, url);

        // Assert performance - metadata endpoint (no DB query, config-based)
        AssertPerformance(elapsed, $"GET /sections/{sectionName} (section metadata)", 100);
    }

    [Theory]
    [InlineData("ai")]
    [InlineData("github-copilot")]
    public async Task GetSectionCollections_ReturnsCollections_WithinPerformanceThreshold(string sectionName)
    {
        // Arrange
        var url = $"/api/sections/{sectionName}/collections";

        // Act
        var elapsed = await MeasureHttpGetAsync<IEnumerable<Collection>>(_client, url);

        // Assert performance - metadata endpoint (no DB query, config-based)
        AssertPerformance(elapsed, $"GET /sections/{sectionName}/collections (collections metadata)", 100);
    }

    [Theory]
    [InlineData("ai", "blogs")]
    [InlineData("github-copilot", "videos")]
    public async Task GetSectionCollection_ReturnsCollection_WithinPerformanceThreshold(
        string sectionName, string collectionName)
    {
        // Arrange
        var url = $"/api/sections/{sectionName}/collections/{collectionName}";

        // Act
        var elapsed = await MeasureHttpGetAsync<Collection>(_client, url);

        // Assert performance - metadata endpoint (no DB query, config-based)
        AssertPerformance(elapsed, $"GET /sections/{sectionName}/collections/{collectionName} (collection metadata)", 100);
    }

    #endregion

    #region RSS Feed Performance Tests

    [Fact]
    public async Task GetAllContentFeed_ReturnsRssFeed_WithinPerformanceThreshold()
    {
        // Arrange
        var url = "/api/rss/all";

        // Act - RSS feed generation (XML serialization)
        var sw = Stopwatch.StartNew();
        var response = await _client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        _ = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        sw.Stop();

        // Assert performance - RSS feed generation includes XML formatting
        AssertPerformance(sw.ElapsedMilliseconds, "GET /rss/all (all content RSS feed)");
    }

    [Theory]
    [InlineData("ai")]
    [InlineData("github-copilot")]
    [InlineData("azure")]
    public async Task GetSectionFeed_ReturnsRssFeed_WithinPerformanceThreshold(string sectionName)
    {
        // Arrange
        var url = $"/api/rss/{sectionName}";

        // Act
        var sw = Stopwatch.StartNew();
        var response = await _client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        _ = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        sw.Stop();

        // Assert performance
        AssertPerformance(sw.ElapsedMilliseconds, $"GET /rss/{sectionName} (section RSS feed)");
    }

    [Theory]
    [InlineData("roundups")]
    [InlineData("videos")]
    public async Task GetCollectionFeed_ReturnsRssFeed_WithinPerformanceThreshold(string collectionName)
    {
        // Arrange
        var url = $"/api/rss/all/{collectionName}";

        // Act
        var sw = Stopwatch.StartNew();
        var response = await _client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        _ = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        sw.Stop();

        // Assert performance
        AssertPerformance(sw.ElapsedMilliseconds, $"GET /rss/collection/{collectionName} (collection RSS feed)");
    }

    #endregion

    #region Custom Pages Performance Tests

    [Fact]
    public async Task GetDXSpaceData_ReturnsCustomPage_WithinPerformanceThreshold()
    {
        // Arrange
        var url = "/api/custom-pages/dx-space";

        // Act
        var elapsed = await MeasureHttpGetAsync<object>(_client, url);

        // Assert performance - custom page data
        AssertPerformance(elapsed, "GET /custom/dx-space");
    }

    [Fact]
    public async Task GetHandbookData_ReturnsCustomPage_WithinPerformanceThreshold()
    {
        // Arrange
        var url = "/api/custom-pages/handbook";

        // Act
        var elapsed = await MeasureHttpGetAsync<object>(_client, url);

        // Assert performance
        AssertPerformance(elapsed, "GET /custom/handbook");
    }

    [Fact]
    public async Task GetLevelsData_ReturnsCustomPage_WithinPerformanceThreshold()
    {
        // Arrange
        var url = "/api/custom-pages/levels";

        // Act
        var elapsed = await MeasureHttpGetAsync<object>(_client, url);

        // Assert performance
        AssertPerformance(elapsed, "GET /custom/levels");
    }

    [Fact]
    public async Task GetFeaturesData_ReturnsCustomPage_WithinPerformanceThreshold()
    {
        // Arrange
        var url = "/api/custom-pages/features";

        // Act
        var elapsed = await MeasureHttpGetAsync<object>(_client, url);

        // Assert performance
        AssertPerformance(elapsed, "GET /custom/features");
    }

    [Fact]
    public async Task GetGenAIBasicsData_ReturnsCustomPage_WithinPerformanceThreshold()
    {
        // Arrange
        var url = "/api/custom-pages/genai-basics";

        // Act
        var elapsed = await MeasureHttpGetAsync<object>(_client, url);

        // Assert performance
        AssertPerformance(elapsed, "GET /custom/genai-basics");
    }

    [Fact]
    public async Task GetGenAIAdvancedData_ReturnsCustomPage_WithinPerformanceThreshold()
    {
        // Arrange
        var url = "/api/custom-pages/genai-advanced";

        // Act
        var elapsed = await MeasureHttpGetAsync<object>(_client, url);

        // Assert performance
        AssertPerformance(elapsed, "GET /custom/genai-advanced");
    }

    [Fact]
    public async Task GetGenAIAppliedData_ReturnsCustomPage_WithinPerformanceThreshold()
    {
        // Arrange
        var url = "/api/custom-pages/genai-applied";

        // Act
        var elapsed = await MeasureHttpGetAsync<object>(_client, url);

        // Assert performance
        AssertPerformance(elapsed, "GET /custom/genai-applied");
    }

    [Fact]
    public async Task GetSDLCData_ReturnsCustomPage_WithinPerformanceThreshold()
    {
        // Arrange
        var url = "/api/custom-pages/sdlc";

        // Act
        var elapsed = await MeasureHttpGetAsync<object>(_client, url);

        // Assert performance
        AssertPerformance(elapsed, "GET /custom/sdlc");
    }

    #endregion
}
