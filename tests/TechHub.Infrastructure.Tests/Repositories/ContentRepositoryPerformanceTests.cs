using System.Diagnostics;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.TestUtilities;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Performance tests for ContentRepository with REAL production data.
/// Tests measure query execution time to validate indexes and query optimization.
/// 
/// Uses:
/// - E2E test factory (TechHubE2ETestApiFactory) which loads REAL collections folder
/// - 4000+ actual content items from production markdown files
/// - Real MarkdownService and ContentSyncService
/// - Cache ENABLED (first call populates cache, measures real-world cold-cache performance)
/// 
/// Performance threshold: 250ms per query (allows for FTS + section filter overhead).
/// These tests validate that covering indexes and query optimization work with real data volumes.
/// </summary>
public class ContentRepositoryPerformanceTests : IClassFixture<TechHubE2ETestApiFactory>
{
    private readonly TechHubE2ETestApiFactory _factory;
    private const int MaxResponseTimeMs = 200;  // Aggressive threshold for non-FTS queries
    private const int MaxFtsResponseTimeMs = 500;  // FTS queries with bm25() + section filter are inherently slower

    public ContentRepositoryPerformanceTests(TechHubE2ETestApiFactory factory)
    {
        _factory = factory;
        // Note: Database is seeded once on factory startup via ContentSyncService
        // Each test creates its own scope and gets a fresh repository instance
    }

    /// <summary>
    /// Get a fresh repository instance for each test.
    /// Creates a new scope to ensure proper service lifetime management.
    /// </summary>
    private IContentRepository GetRepository()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IContentRepository>();
    }

    #region Helper Methods

    private static async Task<long> MeasureExecutionAsync(Func<Task> action)
    {
        var sw = Stopwatch.StartNew();
        await action();
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }

    private static void AssertPerformance(long elapsedMs, string operationName, int? thresholdMs = null)
    {
        var threshold = thresholdMs ?? MaxResponseTimeMs;
        if (elapsedMs > threshold)
        {
            throw new Exception(
                $"Performance degradation detected in {operationName}: " +
                $"{elapsedMs}ms > {threshold}ms threshold. " +
                $"This indicates missing indexes, inefficient queries, or suboptimal query plans. " +
                $"Review EXPLAIN QUERY PLAN output and add appropriate indexes.");
        }
    }

    #endregion

    #region GetBySlugAsync Performance Tests

    [Fact]
    public async Task GetBySlugAsync_WithValidSlug_ReturnsCompleteItem_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure query performance
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var item = await repository.GetBySlugAsync("blogs", "github-copilot-levels-of-enlightenment");

            // Assert completeness
            item.Should().NotBeNull();
            item!.Slug.Should().Be("github-copilot-levels-of-enlightenment");
            item.Title.Should().NotBeNullOrEmpty();
            item.Tags.Should().NotBeEmpty();
        });

        // Assert performance
        AssertPerformance(elapsed, "GetBySlugAsync (single item by PK)");
    }

    #endregion

    #region GetAllAsync Performance Tests

    [Fact]
    public async Task GetAllAsync_ReturnsItems_WithinPerformanceThreshold()
    {
        var scope = _factory.Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IContentRepository>();

        // Measure query execution time
        var sw = Stopwatch.StartNew();
        var items = await repository.GetAllAsync(limit: 20, offset: 0);
        sw.Stop();

        // Validate results
        items.Should().NotBeEmpty();
        items.Count.Should().BeLessThanOrEqualTo(20);
        items.Should().AllSatisfy(item =>
        {
            item.Slug.Should().NotBeNullOrEmpty();
            item.Title.Should().NotBeNullOrEmpty();
            item.CollectionName.Should().NotBeNullOrEmpty();
        });

        // Assert performance
        AssertPerformance(sw.ElapsedMilliseconds, "GetAllAsync (paginated - 20 items)");
    }

    #endregion

    #region GetByCollectionAsync Performance Tests

    [Fact]
    public async Task GetByCollectionAsync_Blogs_ReturnsItems_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure query performance
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var items = await repository.GetByCollectionAsync("blogs", limit: 20, offset: 0);

            // Assert completeness
            items.Should().NotBeEmpty();
            items.Count.Should().BeLessThanOrEqualTo(20);
            items.Should().AllSatisfy(item => item.CollectionName.Should().Be("blogs"));
        });

        // Assert performance
        AssertPerformance(elapsed, "GetByCollectionAsync (blogs - 20 items)");
    }

    [Fact]
    public async Task GetByCollectionAsync_Videos_ReturnsItems_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure query performance for different collection
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var items = await repository.GetByCollectionAsync("videos", limit: 20, offset: 0);

            // Assert completeness
            items.Should().NotBeEmpty();
            items.Count.Should().BeLessThanOrEqualTo(20);
            items.Should().AllSatisfy(item => item.CollectionName.Should().Be("videos"));
        });

        // Assert performance
        AssertPerformance(elapsed, "GetByCollectionAsync (videos - 4 items)");
    }

    #endregion

    #region GetBySectionAsync Performance Tests

    [Theory]
    [InlineData("ai")]
    [InlineData("github-copilot")]
    [InlineData("azure")]
    [InlineData("devops")]
    [InlineData("security")]
    [InlineData("coding")]
    [InlineData("ml")]
    public async Task GetBySectionAsync_ReturnsItemsForSection_WithinPerformanceThreshold(
        string sectionName)
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure query performance for section (CRITICAL for tag cloud performance)
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var items = await repository.GetBySectionAsync(sectionName, limit: 20, offset: 0);

            // Assert completeness (some sections may be empty in test data)
            items.Count.Should().BeLessThanOrEqualTo(20);
        });

        // Assert performance - uses partial indexes with is_{section} = 1
        AssertPerformance(elapsed, $"GetBySectionAsync ({sectionName})");
    }

    #endregion

    #region GetTagCountsAsync Performance Tests

    [Fact]
    public async Task GetTagCountsAsync_AllSections_ReturnsTopTags_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure tag cloud query performance (homepage) - MOST CRITICAL PERFORMANCE TEST
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var tags = await repository.GetTagCountsAsync(maxTags: 100);

            // Assert completeness
            tags.Should().NotBeEmpty();
            tags.Count.Should().BeLessThanOrEqualTo(100);
            tags.Should().AllSatisfy(tag =>
            {
                tag.Tag.Should().NotBeNullOrEmpty();
                tag.Count.Should().BeGreaterThan(0);
            });
            // Should be sorted by count DESC
            tags.Should().BeInDescendingOrder(t => t.Count);
        });

        // Assert performance - CRITICAL: tag cloud must be fast (uses covering index with tags_csv)
        AssertPerformance(elapsed, "GetTagCountsAsync (homepage - all sections - top 100)");
    }

    [Theory]
    [InlineData("ai")]
    [InlineData("github-copilot")]
    [InlineData("azure")]
    [InlineData("devops")]
    public async Task GetTagCountsAsync_SpecificSection_ReturnsTopTags_WithinPerformanceThreshold(
        string sectionName)
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure tag cloud query performance for specific section - CRITICAL PERFORMANCE TEST
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var tags = await repository.GetTagCountsAsync(sectionName: sectionName, maxTags: 50);

            // Assert completeness
            tags.Should().NotBeEmpty($"{sectionName} section should have tags");
            tags.Count.Should().BeLessThanOrEqualTo(50);
            tags.Should().BeInDescendingOrder(t => t.Count);
        });

        // Assert performance - CRITICAL: section tag clouds (uses section partial indexes with tags_csv)
        AssertPerformance(elapsed, $"GetTagCountsAsync ({sectionName} - top 50)");
    }

    #endregion

    #region SearchAsync Performance Tests

    [Fact]
    public async Task SearchAsync_SimpleQuery_ReturnsResults_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure search query performance
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var request = new SearchRequest { Query = "test", Take = 20 };
            var results = await repository.SearchAsync(request);

            // Assert completeness - may be empty if no matches
            results.Items.Count.Should().BeLessThanOrEqualTo(20);
        });

        // Assert performance - FTS with bm25() ranking uses higher threshold
        AssertPerformance(elapsed, "SearchAsync (simple query)", MaxFtsResponseTimeMs);
    }

    [Fact]
    public async Task SearchAsync_WithSectionFilter_ReturnsFilteredResults_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure search with section filter
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var request = new SearchRequest
            {
                Query = "test",
                Sections = ["ai"],
                Take = 20
            };
            var results = await repository.SearchAsync(request);

            // Assert completeness - may be empty if no matches
            results.Items.Count.Should().BeLessThanOrEqualTo(20);
        });

        // Assert performance - FTS with bm25() ranking uses higher threshold
        AssertPerformance(elapsed, "SearchAsync (with section filter)", MaxFtsResponseTimeMs);
    }

    [Fact]
    public async Task SearchAsync_FtsWithSectionAndCollection_ReturnsFilteredResults_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure FTS search with section + collection filters
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var request = new SearchRequest
            {
                Query = "test",
                Sections = ["ai"],
                Collections = ["blogs"],
                Take = 20
            };
            var results = await repository.SearchAsync(request);

            // Assert completeness - may be empty if no matches
            results.Items.Count.Should().BeLessThanOrEqualTo(20);
        });

        // Assert performance - FTS with bm25() ranking uses higher threshold
        AssertPerformance(elapsed, "SearchAsync (FTS + section + collection)", MaxFtsResponseTimeMs);
    }

    [Fact]
    public async Task SearchAsync_FtsWithSectionAndTags_ReturnsFilteredResults_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure FTS search with section + tags filters
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var request = new SearchRequest
            {
                Query = "test",
                Sections = ["ai"],
                Tags = ["Copilot"],
                Take = 20
            };
            var results = await repository.SearchAsync(request);

            // Assert completeness - may be empty if no matches
            results.Items.Count.Should().BeLessThanOrEqualTo(20);
        });

        // Assert performance - FTS with bm25() ranking uses higher threshold
        AssertPerformance(elapsed, "SearchAsync (FTS + section + tags)", MaxFtsResponseTimeMs);
    }

    [Fact]
    public async Task SearchAsync_FtsWithSectionCollectionAndTags_ReturnsFilteredResults_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure FTS search with section + collection + tags filters (all filters)
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var request = new SearchRequest
            {
                Query = "test",
                Sections = ["ai"],
                Collections = ["blogs"],
                Tags = ["Copilot"],
                Take = 20
            };
            var results = await repository.SearchAsync(request);

            // Assert completeness - may be empty if no matches
            results.Items.Count.Should().BeLessThanOrEqualTo(20);
        });

        // Assert performance - FTS with bm25() ranking uses higher threshold
        AssertPerformance(elapsed, "SearchAsync (FTS + section + collection + tags)", MaxFtsResponseTimeMs);
    }

    [Fact]
    public async Task SearchAsync_WithTagsFilter_ReturnsFilteredResults_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure search with tags filter
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var request = new SearchRequest
            {
                Tags = ["AI"],
                Take = 20
            };
            var results = await repository.SearchAsync(request);

            // Assert completeness - should have AI-tagged items
            results.Items.Count.Should().BeLessThanOrEqualTo(20);
        });

        // Assert performance
        AssertPerformance(elapsed, "SearchAsync (with tags filter)");
    }

    [Fact]
    public async Task SearchAsync_WithDateFilter_ReturnsRecentResults_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure search with date filter
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var request = new SearchRequest
            {
                DateFrom = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Take = 20
            };
            var results = await repository.SearchAsync(request);

            // Assert completeness - should have 2024+ items
            results.Items.Count.Should().BeLessThanOrEqualTo(20);
        });

        // Assert performance
        AssertPerformance(elapsed, "SearchAsync (date filter)");
    }

    [Fact]
    public async Task SearchAsync_WithMultipleTags_ReturnsFilteredResults_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure search with multiple tags (AND logic)
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var request = new SearchRequest
            {
                Tags = ["AI", "GitHub"],
                Take = 20
            };
            var results = await repository.SearchAsync(request);

            // Assert completeness
            results.Items.Count.Should().BeLessThanOrEqualTo(20);
        });

        // Assert performance
        AssertPerformance(elapsed, "SearchAsync (multiple tags - AND logic)");
    }

    [Fact]
    public async Task SearchAsync_WithTagsAndSection_ReturnsFilteredResults_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure search with tags + section (uses partial index)
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var request = new SearchRequest
            {
                Tags = ["Copilot"],
                Sections = ["ai"],
                Take = 20
            };
            var results = await repository.SearchAsync(request);

            // Assert completeness
            results.Items.Count.Should().BeLessThanOrEqualTo(20);
        });

        // Assert performance - CRITICAL: uses idx_tags_section_ai partial index
        AssertPerformance(elapsed, "SearchAsync (tags + section)");
    }

    [Fact]
    public async Task SearchAsync_WithTagsAndCollection_ReturnsFilteredResults_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure search with tags + collection
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var request = new SearchRequest
            {
                Tags = ["Copilot"],
                Collections = ["blogs"],
                Take = 20
            };
            var results = await repository.SearchAsync(request);

            // Assert completeness
            results.Items.Count.Should().BeLessThanOrEqualTo(20);
        });

        // Assert performance - should use idx_tags_collection index
        AssertPerformance(elapsed, "SearchAsync (tags + collection)");
    }

    [Fact]
    public async Task SearchAsync_WithTagsSectionAndCollection_ReturnsFilteredResults_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure search with tags + section + collection (most specific filter)
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var request = new SearchRequest
            {
                Tags = ["Copilot"],
                Sections = ["ai"],
                Collections = ["blogs"],
                Take = 20
            };
            var results = await repository.SearchAsync(request);

            // Assert completeness
            results.Items.Count.Should().BeLessThanOrEqualTo(20);
        });

        // Assert performance - should use idx_tags_section_collection index
        AssertPerformance(elapsed, "SearchAsync (tags + section + collection)");
    }

    [Fact]
    public async Task SearchAsync_WithMultipleSections_ReturnsFilteredResults_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure search with multiple sections
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var request = new SearchRequest
            {
                Sections = ["ai", "github-copilot"],
                Take = 20
            };
            var results = await repository.SearchAsync(request);

            // Assert completeness
            results.Items.Count.Should().BeLessThanOrEqualTo(20);
        });

        // Assert performance
        AssertPerformance(elapsed, "SearchAsync (multiple sections)");
    }

    [Fact]
    public async Task SearchAsync_WithSectionAndCollection_ReturnsFilteredResults_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure search with section + collection (no tags, no query)
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var request = new SearchRequest
            {
                Sections = ["ai"],
                Collections = ["blogs"],
                Take = 20
            };
            var results = await repository.SearchAsync(request);

            // Assert completeness
            results.Items.Count.Should().BeLessThanOrEqualTo(20);
        });

        // Assert performance - should use idx_section_collection index
        AssertPerformance(elapsed, "SearchAsync (section + collection - no tags)");
    }

    [Fact]
    public async Task SearchAsync_NoFilters_ReturnsTopItems_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure getting top X items (no search, no filters)
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var request = new SearchRequest { Take = 20 };
            var results = await repository.SearchAsync(request);

            // Assert completeness
            results.Items.Count.Should().BeLessThanOrEqualTo(20);
            // Should be sorted by date DESC
            results.Items.Should().BeInDescendingOrder(i => i.DateEpoch);
        });

        // Assert performance - should use idx_draft_date or similar
        AssertPerformance(elapsed, "SearchAsync (no filters - top 20 items)");
    }

    [Fact]
    public async Task SearchAsync_CollectionOnly_ReturnsFilteredItems_WithinPerformanceThreshold()
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure getting items from collection (no search, no section)
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var request = new SearchRequest
            {
                Collections = ["blogs"],
                Take = 20
            };
            var results = await repository.SearchAsync(request);

            // Assert completeness
            results.Items.Count.Should().BeLessThanOrEqualTo(20);
            results.Items.Should().AllSatisfy(i => i.CollectionName.Should().Be("blogs"));
        });

        // Assert performance - should use idx_collection_date
        AssertPerformance(elapsed, "SearchAsync (collection only - no section)");
    }

    #endregion

    #region GetTagCountsAsync Extended Performance Tests

    [Theory]
    [InlineData("blogs")]
    [InlineData("videos")]
    public async Task GetTagCountsAsync_SpecificCollection_ReturnsTopTags_WithinPerformanceThreshold(
        string collectionName)
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure tag cloud for collection (no section filter)
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var tags = await repository.GetTagCountsAsync(
                collectionName: collectionName,
                maxTags: 50);

            // Assert completeness
            tags.Count.Should().BeLessThanOrEqualTo(50);
            tags.Should().BeInDescendingOrder(t => t.Count);
        });

        // Assert performance - should use tags_csv covering index
        AssertPerformance(elapsed, $"GetTagCountsAsync ({collectionName} - no section)");
    }

    [Theory]
    [InlineData("ai", "blogs")]
    [InlineData("github-copilot", "blogs")]
    [InlineData("ai", "videos")]
    public async Task GetTagCountsAsync_SectionAndCollection_ReturnsTopTags_WithinPerformanceThreshold(
        string sectionName, string collectionName)
    {
        // Arrange
        var repository = GetRepository();

        // Act - measure tag cloud for section + collection (most specific)
        var elapsed = await MeasureExecutionAsync(async () =>
        {
            var tags = await repository.GetTagCountsAsync(
                sectionName: sectionName,
                collectionName: collectionName,
                maxTags: 30);

            // Assert completeness
            tags.Count.Should().BeLessThanOrEqualTo(30);
            tags.Should().BeInDescendingOrder(t => t.Count);
        });

        // Assert performance - CRITICAL: uses section partial index with collection filter
        AssertPerformance(elapsed, $"GetTagCountsAsync ({sectionName}/{collectionName})");
    }

    #endregion

}
