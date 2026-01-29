using FluentAssertions;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using static TechHub.TestUtilities.TestDataConstants;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Base class for content repository integration tests.
/// Contains the superset of assertions that ALL repository types (FileBased, SQLite, PostgreSQL) must support.
/// Derived classes must implement CreateRepository() to provide their specific repository implementation.
/// All test data comes from TestCollections directory - NO manual insertions allowed.
/// </summary>
/// <remarks>
/// Expected counts are defined in TestDataConstants.cs in TestUtilities project.
/// Update TestDataConstants when test data changes.
/// </remarks>
public abstract class BaseContentRepositoryTests : IDisposable
{

    protected abstract IContentRepository Repository { get; }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    #region GetAllAsync Tests

    /// <summary>
    /// Test: GetAllAsync returns all non-draft items
    /// Why: Core functionality - repository must load all published content
    /// </summary>
    [Fact]
    public virtual async Task GetAllAsync_ReturnsAllNonDraftItems()
    {
        // Arrange - data already seeded from TestCollections

        // Act
        var results = await Repository.GetAllAsync(limit: int.MaxValue);

        // Assert
        results.Should().HaveCount(TotalPublishedItems, "Should return exactly 32 published items from TestCollections");
        results.Should().OnlyContain(item => !item.Draft, "GetAllAsync should exclude drafts by default");
        results.Should().BeInDescendingOrder(item => item.DateEpoch, "Items should be sorted by date descending");
    }

    #endregion

    #region GetByCollectionAsync Tests

    /// <summary>
    /// Test: GetByCollectionAsync filters content by collection name
    /// Why: Users need to view content from specific collections (news, videos, blogs, etc.)
    /// </summary>
    [Fact]
    public virtual async Task GetByCollectionAsync_FiltersByCollection()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _blogs/*.md files exist

        // Act
        var results = await Repository.GetByCollectionAsync("blogs", limit: int.MaxValue);

        // Assert
        results.Should().HaveCount(BlogsCount, "Should return exactly 18 blog posts from TestCollections");
        results.Should().OnlyContain(item => item.CollectionName == "blogs", "Should only return blogs collection items");
        results.Should().NotContain(item => item.Draft, "Should exclude drafts by default");
    }

    /// <summary>
    /// Test: GetByCollectionAsync with "all" returns all content
    /// Why: The /all/all collection page should show all content across all collections
    ///      "all" is a virtual collection that returns all content from all collections
    /// </summary>
    [Fact]
    public virtual async Task GetByCollectionAsync_AllCollection_ReturnsAllContent()
    {
        // Arrange - data already seeded from TestCollections

        // Act
        var results = await Repository.GetByCollectionAsync("all", limit: int.MaxValue);

        // Assert
        results.Should().HaveCount(TotalPublishedItems, "Should return all 32 published items from TestCollections");
        results.Should().NotContain(item => item.Draft, "Should exclude drafts by default");
        // Should contain items from different collections
        var collections = results.Select(r => r.CollectionName).Distinct().ToList();
        collections.Should().HaveCountGreaterThan(1, "all collection should aggregate multiple collection types");
    }

    /// <summary>
    /// Test: GetByCollectionAsync excludes draft items by default
    /// Why: Draft items should NOT appear by default, only when explicitly requested
    /// </summary>
    [Fact]
    public virtual async Task GetByCollectionAsync_ExcludesDraftItemsByDefault()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _blogs/2024-01-02-draft-article.md exists with draft: true

        // Act
        var results = await Repository.GetByCollectionAsync("blogs", includeDraft: false, limit: int.MaxValue);

        // Assert
        results.Should().NotContain(item => item.Slug == "draft-article",
            "Draft articles should not appear when includeDraft=false");
        results.Should().OnlyContain(item => !item.Draft, "All returned items should have Draft=false");
    }

    /// <summary>
    /// Test: GetByCollectionAsync includes draft items when includeDraft=true
    /// Why: When explicitly requesting drafts, they should be included (e.g., for preview/admin)
    /// </summary>
    [Fact]
    public virtual async Task GetByCollectionAsync_IncludesDraftItemsWhenRequested()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _blogs/2024-01-02-draft-article.md exists with draft: true

        // Act
        var results = await Repository.GetByCollectionAsync("blogs", includeDraft: true, limit: int.MaxValue);

        // Assert
        results.Should().Contain(item => item.Slug == "draft-article" && item.Draft,
            "Draft articles should appear when includeDraft=true");
    }

    /// <summary>
    /// Test: GetByCollectionAsync for videos includes subcollection items
    /// Why: When querying "videos", should include both root videos and subcollections (ghc-features, vscode-updates)
    ///      Subcollections are part of the collection hierarchy
    /// </summary>
    [Fact]
    public virtual async Task GetByCollectionAsync_VideosCollection_IncludesSubcollections()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _ghc-features/*.md and _vscode-updates/*.md exist, and optionally _videos/*.md

        // Act
        var results = await Repository.GetByCollectionAsync("videos", limit: int.MaxValue);

        // Assert
        results.Should().NotBeEmpty("TestCollections should contain video content");
        results.Should().OnlyContain(item =>
            item.CollectionName == "videos" ||
            item.CollectionName == "ghc-features" ||
            item.CollectionName == "vscode-updates",
            "videos collection should include videos, ghc-features, and vscode-updates");
    }

    #endregion

    #region GetBySectionAsync Tests

    /// <summary>
    /// Test: GetBySectionAsync filters content by section name
    /// Why: Sections display content filtered by section name (ai, github-copilot, etc.)
    /// </summary>
    [Fact]
    public virtual async Task GetBySectionAsync_FiltersBySection()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: Files with section_names: [ai] exist

        // Act
        var results = await Repository.GetBySectionAsync("ai", limit: int.MaxValue);

        // Assert
        results.Should().NotBeEmpty("TestCollections should contain AI section content");
    }

    /// <summary>
    /// Test: GetBySectionAsync with "all" returns all content
    /// Why: The /all section page should show all content across all sections
    ///      "all" is a virtual section that returns all content from all sections
    /// </summary>
    [Fact]
    public virtual async Task GetBySectionAsync_AllSection_ReturnsAllContent()
    {
        // Arrange - data already seeded from TestCollections

        // Act
        var results = await Repository.GetBySectionAsync("all", limit: int.MaxValue);

        // Assert
        results.Should().NotBeEmpty("TestCollections should contain multiple items");
        results.Should().NotContain(item => item.Draft, "Should exclude drafts by default");
        // Should contain items from different sections
        var primarySections = results.Select(r => r.PrimarySectionName).Distinct().ToList();
        primarySections.Should().HaveCountGreaterThan(1, "all section should aggregate multiple section types");
    }

    /// <summary>
    /// Test: GetBySectionAsync excludes draft items by default
    /// Why: When viewing a section, draft items should NOT appear unless explicitly requested
    /// </summary>
    [Fact]
    public virtual async Task GetBySectionAsync_ExcludesDraftItemsByDefault()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _blogs/2024-01-02-draft-article.md exists with draft: true and section_names: [ai]

        // Act
        var results = await Repository.GetBySectionAsync("ai", limit: int.MaxValue, includeDraft: false);

        // Assert
        results.Should().NotContain(item => item.Slug == "draft-article",
            "Draft articles should not appear when includeDraft=false");
        results.Should().OnlyContain(item => !item.Draft, "All returned items should have Draft=false");
    }

    /// <summary>
    /// Test: GetBySectionAsync includes draft items when includeDraft=true
    /// Why: When explicitly requesting drafts (e.g., preview mode), both published AND draft items returned
    /// </summary>
    [Fact]
    public virtual async Task GetBySectionAsync_IncludesDraftItemsWhenRequested()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _blogs/2024-01-02-draft-article.md exists with draft: true and section_names: [ai]

        // Act
        var results = await Repository.GetBySectionAsync("ai", limit: int.MaxValue, includeDraft: true);

        // Assert
        results.Should().Contain(item => item.Slug == "draft-article" && item.Draft,
            "Draft articles should appear when includeDraft=true");
    }

    #endregion

    #region GetBySlugAsync Tests

    /// <summary>
    /// Test: GetBySlugAsync retrieves single content item by slug
    /// Why: Display individual content detail pages by URL slug
    /// </summary>
    [Fact]
    public virtual async Task GetBySlugAsync_ExistingItem_ReturnsItem()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _blogs/2024-01-01-test-article.md exists

        // Act
        var result = await Repository.GetBySlugAsync("blogs", "test-article");

        // Assert
        result.Should().NotBeNull("TestCollections should contain the test article");
        result!.Slug.Should().Be("test-article");
        result.Title.Should().Be("Test Article with AI and Azure Tags");
        result.Tags.Should().BeEquivalentTo(["AI", "Azure", "Cloud"]);
        result.PrimarySectionName.Should().BeOneOf("ai", "cloud");
    }

    /// <summary>
    /// Test: GetBySlugAsync returns null for non-existent item
    /// Why: Graceful handling of missing content (404 pages)
    /// </summary>
    [Fact]
    public virtual async Task GetBySlugAsync_NonExistentItem_ReturnsNull()
    {
        // Arrange - data already seeded from TestCollections

        // Act
        var result = await Repository.GetBySlugAsync("blogs", "non-existent-slug");

        // Assert
        result.Should().BeNull("Non-existent slug should return null");
    }

    /// <summary>
    /// Test: GetBySlugAsync excludes draft items by default
    /// Why: Draft items should NOT be accessible by slug unless explicitly requested
    /// </summary>
    [Fact]
    public virtual async Task GetBySlugAsync_DraftItem_ExcludedByDefault()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _blogs/2024-01-02-draft-article.md exists with draft: true

        // Act
        var result = await Repository.GetBySlugAsync("blogs", "draft-article", includeDraft: false);

        // Assert
        result.Should().BeNull("Draft items should not be returned when includeDraft=false");
    }

    /// <summary>
    /// Test: GetBySlugAsync includes draft items when includeDraft=true
    /// Why: When explicitly requesting drafts (e.g., preview mode), they should be accessible
    /// </summary>
    [Fact]
    public virtual async Task GetBySlugAsync_DraftItem_IncludedWhenRequested()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _blogs/2024-01-02-draft-article.md exists with draft: true

        // Act
        var result = await Repository.GetBySlugAsync("blogs", "draft-article", includeDraft: true);

        // Assert
        result.Should().NotBeNull("Draft items should be returned when includeDraft=true");
        result!.Draft.Should().BeTrue("Item should be marked as draft");
    }

    #endregion

    #region Subcollection Tests

    /// <summary>
    /// Test: Subcollection items have correct URL with collection name in path
    /// Why: URLs always use collection name, not subcollection (subcollections are for filtering only)
    ///      e.g., /github-copilot/videos/slug (not /github-copilot/vscode-updates/slug)
    /// </summary>
    [Fact]
    public virtual async Task SubcollectionItems_HaveCorrectUrlWithCollectionInPath()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _vscode-updates/2025-01-10-vscode-update.md exists

        // Act
        var results = await Repository.GetByCollectionAsync("videos", limit: int.MaxValue);

        // Assert: Find the vscode-updates item
        var vscodeItem = results.FirstOrDefault(v => v.SubcollectionName == "vscode-updates");
        vscodeItem.Should().NotBeNull("TestCollections should contain a vscode-updates item");
        vscodeItem!.CollectionName.Should().Be("videos", "Subcollection items should have collection name 'videos'");
        vscodeItem.GetHref().Should().Contain("/videos/", "URL should use collection name in path");
        vscodeItem.GetHref().Should().NotContain("/vscode-updates/", "URL should not contain subcollection name (used for filtering only)");
    }

    /// <summary>
    /// Test: ghc-features subcollection items have correct URL with collection name in path
    /// Why: URLs always use collection name, not subcollection (subcollections are for filtering only)
    ///      e.g., /github-copilot/videos/slug (not /github-copilot/ghc-features/slug)
    /// </summary>
    [Fact]
    public virtual async Task GhcFeaturesSubcollectionItems_HaveCorrectUrlWithCollectionInPath()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _ghc-features/*.md exists

        // Act
        var results = await Repository.GetByCollectionAsync("videos", limit: int.MaxValue);

        // Assert: Find a ghc-features item
        var ghcFeatureItem = results.FirstOrDefault(v => v.SubcollectionName == "ghc-features");
        ghcFeatureItem.Should().NotBeNull("TestCollections should contain a ghc-features item");
        ghcFeatureItem!.CollectionName.Should().Be("videos", "Subcollection items should have collection name 'videos'");
        ghcFeatureItem.GetHref().Should().Contain("/videos/", "URL should use collection name in path");
        ghcFeatureItem.GetHref().Should().NotContain("/ghc-features/", "URL should not contain subcollection name (used for filtering only)");
    }

    #endregion

    #region SearchAsync Tests

    /// <summary>
    /// Test: SearchAsync with tag filter returns matching items
    /// Why: Users need to filter content by tags across all collections
    /// </summary>
    [Fact]
    public virtual async Task SearchAsync_TagFilter_FiltersCorrectly()
    {
        // Arrange - data already seeded from TestCollections
        var request = new SearchRequest { Tags = ["AI"], Take = 1000 };

        // Act
        var results = await Repository.SearchAsync(request);

        // Assert - verify exact count and filtering logic
        var actualCount = results.Items.Count;
        var expectedCount = AiTagCount;
        actualCount.Should().Be(expectedCount, $"Should return exactly {expectedCount} items with AI tag (actual: {actualCount})");
        results.TotalCount.Should().Be(AiTagCount, $"TotalCount should be {AiTagCount} for AI tag filter");
        results.Items.Should().OnlyContain(item =>
            item.Tags.Any(t => t.Contains("AI", StringComparison.OrdinalIgnoreCase)),
            "All returned items should have AI tag");
        // Note: "devops-only" is included because ContentFixer added "AI" tag based on section_names
    }

    /// <summary>
    /// Test: SearchAsync with section filter returns matching items
    /// Why: Users need to filter content by section
    /// </summary>
    [Fact]
    public virtual async Task SearchAsync_SectionFilter_FiltersCorrectly()
    {
        // Arrange - data already seeded from TestCollections
        // "test-article" has sections: ["ai", "cloud"]
        // "devops-section" has sections: ["devops"]
        var request = new SearchRequest { Sections = ["ai"], Take = 1000 };

        // Act
        var results = await Repository.SearchAsync(request);

        // Assert - verify positive and negative cases
        results.Items.Should().NotBeEmpty("TestCollections should contain items in ai section");
        results.Items.Should().NotContain(item => item.Slug == "devops-section",
            "Items only in devops section should be excluded");
        results.TotalCount.Should().Be(results.Items.Count,
            "TotalCount should match returned items count");
    }

    /// <summary>
    /// Test: SearchAsync with collection filter returns matching items
    /// Why: Users need to filter content by collection type
    /// </summary>
    [Fact]
    public virtual async Task SearchAsync_CollectionFilter_FiltersCorrectly()
    {
        // Arrange - data already seeded from TestCollections
        // _blogs/ and _news/ directories exist with different content
        var request = new SearchRequest { Collections = ["blogs"], Take = 1000 };

        // Act
        var results = await Repository.SearchAsync(request);

        // Assert - verify exact count and filtering logic
        results.Items.Should().HaveCount(BlogsCount, "Should return exactly 18 blog items");
        results.TotalCount.Should().Be(BlogsCount, "TotalCount should be 18 for blogs collection");
        results.Items.Should().OnlyContain(item =>
            item.CollectionName.Equals("blogs", StringComparison.OrdinalIgnoreCase),
            "All returned items should be from blogs collection");
        results.Items.Should().NotContain(item =>
            item.CollectionName.Equals("news", StringComparison.OrdinalIgnoreCase),
            "News items should be excluded when filtering by blogs");
    }

    /// <summary>
    /// Test: SearchAsync with date range filter returns matching items
    /// Why: Users need to filter content by date range (e.g., last 30 days)
    /// </summary>
    [Fact]
    public virtual async Task SearchAsync_DateRangeFilter_FiltersCorrectly()
    {
        // Arrange - data already seeded from TestCollections
        // 2024 items exist, 2023 items exist (old-post), 2025+ items exist
        var request = new SearchRequest
        {
            DateFrom = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            DateTo = new DateTimeOffset(2024, 12, 31, 23, 59, 59, TimeSpan.Zero),
            Take = 1000
        };

        // Act
        var results = await Repository.SearchAsync(request);
        var fromEpoch = request.DateFrom.Value.ToUnixTimeSeconds();
        var toEpoch = request.DateTo.Value.ToUnixTimeSeconds();

        // Assert - verify exact count and date range logic
        results.Items.Should().HaveCount(Items2024Count, "Should return exactly 17 items from 2024");
        results.TotalCount.Should().Be(Items2024Count, "TotalCount should be 17 for 2024 date range");
        results.Items.Should().OnlyContain(item =>
            item.DateEpoch >= fromEpoch && item.DateEpoch <= toEpoch,
            "All returned items should be within date range");
        results.Items.Should().NotContain(item => item.Slug == "old-post",
            "2023 items should be excluded from 2024 date range");
        results.Items.Should().NotContain(item => item.Slug == "recent-post",
            "2025 items should be excluded from 2024 date range");
    }

    /// <summary>
    /// Test: SearchAsync pagination returns correct count
    /// Why: Pagination must work correctly for infinite scroll
    /// </summary>
    [Fact]
    public virtual async Task SearchAsync_Pagination_ReturnsCorrectCount()
    {
        // Arrange - data already seeded from TestCollections
        var request = new SearchRequest { Take = 3 };

        // Act
        var results = await Repository.SearchAsync(request);

        // Assert - verify pagination works correctly with known total
        results.Items.Should().HaveCount(3, "Should return exactly 3 items when Take=3");
        results.TotalCount.Should().Be(TotalPublishedItems,
            "TotalCount should be 32 (all published items), not just the page size");
    }

    /// <summary>
    /// Test: SearchAsync excludes draft items
    /// Why: Draft items should never appear in search results
    /// </summary>
    [Fact]
    public virtual async Task SearchAsync_ExcludesDrafts()
    {
        // Arrange - data already seeded from TestCollections
        var request = new SearchRequest { Take = 1000 };

        // Act
        var results = await Repository.SearchAsync(request);

        // Assert
        results.Items.Should().NotContain(item => item.Draft,
            "Search results should never include drafts");
    }

    #endregion

    #region GetFacetsAsync Tests

    /// <summary>
    /// Test: GetFacetsAsync returns tag counts
    /// Why: Faceted navigation requires accurate tag counts
    /// </summary>
    [Fact]
    public virtual async Task GetFacetsAsync_ReturnsTagCounts()
    {
        // Arrange - data already seeded from TestCollections
        var request = new FacetRequest { FacetFields = ["tags"] };

        // Act
        var results = await Repository.GetFacetsAsync(request);

        // Assert - verify facet structure and accuracy
        results.Facets.Should().ContainKey("tags", "Should return tag facets when requested");
        results.Facets["tags"].Should().NotBeEmpty("TestCollections should have tagged content");
        results.Facets["tags"].Should().OnlyContain(f => f.Count > 0, "All facets should have positive counts");

        // Verify AI tag count matches actual PUBLISHED items (using substring matching)
        var aiTagFacet = results.Facets["tags"].FirstOrDefault(f =>
            f.Value.Equals("AI", StringComparison.OrdinalIgnoreCase));
        if (aiTagFacet != null)
        {
            var allItems = await Repository.GetAllAsync(limit: int.MaxValue);
            // Count PUBLISHED items that have tags containing "ai" as a word (substring match)
            var actualAiCount = allItems.Where(i => !i.Draft).Count(i =>
                i.Tags.Any(t => t.Split(new[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries)
                    .Any(word => word.Equals("AI", StringComparison.OrdinalIgnoreCase))));
            aiTagFacet.Count.Should().Be(actualAiCount,
                "AI tag facet count should match PUBLISHED items with 'AI' as a word in any tag (substring match)");
        }
    }

    /// <summary>
    /// Test: GetFacetsAsync returns collection counts
    /// Why: Faceted navigation requires accurate collection counts
    /// </summary>
    [Fact]
    public virtual async Task GetFacetsAsync_ReturnsCollectionCounts()
    {
        // Arrange - data already seeded from TestCollections
        var request = new FacetRequest { FacetFields = ["collections"] };

        // Act
        var results = await Repository.GetFacetsAsync(request);

        // Assert - verify facet structure and hardcoded expected counts
        results.Facets.Should().ContainKey("collections", "Should return collection facets when requested");
        results.Facets["collections"].Should().NotBeEmpty("TestCollections should have multiple collections");

        // Verify blogs collection count matches expected
        var blogsFacet = results.Facets["collections"].FirstOrDefault(f =>
            f.Value.Equals("blogs", StringComparison.OrdinalIgnoreCase));
        blogsFacet.Should().NotBeNull("Should have blogs collection facet");
        blogsFacet!.Count.Should().Be(BlogsCount,
            "Blogs facet count should be 18");

        // Verify news collection count matches expected
        var newsFacet = results.Facets["collections"].FirstOrDefault(f =>
            f.Value.Equals("news", StringComparison.OrdinalIgnoreCase));
        newsFacet.Should().NotBeNull("Should have news collection facet");
        newsFacet!.Count.Should().Be(NewsCount,
            "News facet count should be 7");
    }

    /// <summary>
    /// Test: GetFacetsAsync returns section counts
    /// Why: Faceted navigation requires accurate section counts
    /// </summary>
    [Fact]
    public virtual async Task GetFacetsAsync_ReturnsSectionCounts()
    {
        // Arrange - data already seeded from TestCollections
        var request = new FacetRequest { FacetFields = ["sections"] };

        // Act
        var results = await Repository.GetFacetsAsync(request);

        // Assert - verify facet structure
        results.Facets.Should().ContainKey("sections", "Should return section facets when requested");
        results.Facets["sections"].Should().NotBeEmpty("TestCollections should have multiple sections");

        // Verify facets are returned
        results.Facets["sections"].Should().NotBeEmpty("Should have section facets");
        results.Facets["sections"].Should().Contain(f => f.Value == "ai" || f.Value == "cloud",
            "Should contain expected sections from test data");
    }

    /// <summary>
    /// Test: GetFacetsAsync returns total count
    /// Why: Total count is needed for pagination and UI display
    /// </summary>
    [Fact]
    public virtual async Task GetFacetsAsync_ReturnsTotalCount()
    {
        // Arrange - data already seeded from TestCollections
        var request = new FacetRequest { FacetFields = ["tags"] };

        // Act
        var results = await Repository.GetFacetsAsync(request);

        // Assert - verify total count matches known expected value
        results.TotalCount.Should().Be(TotalPublishedItems,
            "TotalCount should equal 32 (the actual number of non-draft items in TestCollections)");
    }

    /// <summary>
    /// Test: GetFacetsAsync with tag filter shows intersection counts
    /// Why: When filtering by a tag, other facet counts should reflect the filtered scope
    /// </summary>
    [Fact]
    public virtual async Task GetFacetsAsync_FilteredByTags_ShowsIntersectionCounts()
    {
        // Arrange - data already seeded from TestCollections
        // Filter by AI tag and check that counts are for AI-filtered content only
        var filteredRequest = new FacetRequest { Tags = ["AI"], FacetFields = ["tags"] };

        // Act
        var filteredResults = await Repository.GetFacetsAsync(filteredRequest);

        // Assert - verify filtered counts reflect reduced scope
        filteredResults.TotalCount.Should().Be(AiTagCount,
            $"Filtered count should be {AiTagCount} (the number of AI-tagged items)");
        filteredResults.TotalCount.Should().BeLessThan(TotalPublishedItems,
            $"Filtered count ({AiTagCount}) should be less than total ({TotalPublishedItems}) - not all items have AI tag");
    }

    #endregion

    #region GetRelatedAsync Tests

    /// <summary>
    /// Test: GetRelatedAsync returns items with tag overlap
    /// Why: Related articles feature uses tag overlap to find similar content
    /// </summary>
    [Fact]
    public virtual async Task GetRelatedAsync_BasedOnTagOverlap_ReturnsRelatedItems()
    {
        // Arrange - data already seeded from TestCollections
        // "related1" has tags: ["TestRelated", "TestTagA", "TestTagB"]
        // "related2" has tags: ["TestRelated", "TestTagC"] - should match on TestRelated
        // "related3" has tags: ["TestTagA", "TestTagB"] - should match on TestTagA, TestTagB
        var sourceTags = new[] { "TestRelated", "TestTagA", "TestTagB" };
        var excludeSlug = "related1";

        // Act
        var results = await Repository.GetRelatedAsync(sourceTags, excludeSlug, count: 5);

        // Assert - should find related items based on tag overlap
        results.Should().NotBeEmpty("Should find related items via tag overlap");
        results.Should().NotContain(item => item.Slug == excludeSlug,
            "Should not include the source article itself");

        // Verify results have tag overlap with source
        // related2 shares "TestRelated", related3 shares "TestTagA" and "TestTagB"
        var resultSlugs = results.Select(r => r.Slug).ToList();
        resultSlugs.Should().Contain(s => s == "related2" || s == "related3",
            "Should return items that share tags with the source article");
    }

    /// <summary>
    /// Test: GetRelatedAsync excludes the source item
    /// Why: The source article should never appear in its own related list
    /// </summary>
    [Fact]
    public virtual async Task GetRelatedAsync_ExcludesSourceItem()
    {
        // Arrange - data already seeded from TestCollections
        var sourceTags = new[] { "TestRelated", "TestTagA", "TestTagB" };
        var excludeSlug = "related1";

        // Act
        var results = await Repository.GetRelatedAsync(sourceTags, excludeSlug, count: 10);

        // Assert
        results.Should().NotContain(item => item.Slug == excludeSlug,
            "Related items should never include the source article");
    }

    /// <summary>
    /// Test: GetRelatedAsync excludes draft items
    /// Why: Draft items should never appear in related articles
    /// </summary>
    [Fact]
    public virtual async Task GetRelatedAsync_ExcludesDrafts()
    {
        // Arrange - data already seeded from TestCollections
        // "related1" has tags: ["TestRelated", "TestTagA", "TestTagB"]
        // "draft-related" has tags: ["AI"] and draft: true
        var sourceTags = new[] { "TestRelated", "TestTagA", "TestTagB" };
        var excludeSlug = "related1";

        // Act
        var results = await Repository.GetRelatedAsync(sourceTags, excludeSlug, count: 10);

        // Assert
        results.Should().NotContain(item => item.Draft,
            "Related items should never include drafts");
        results.Should().NotContain(item => item.Slug == "draft-related",
            "Should not include draft-related article");
    }

    /// <summary>
    /// Test: GetRelatedAsync respects count limit
    /// Why: UI may only want a limited number of related items
    /// </summary>
    [Fact]
    public virtual async Task GetRelatedAsync_RespectsCountLimit()
    {
        // Arrange - data already seeded from TestCollections
        var sourceTags = new[] { "TestRelated", "TestTagA", "TestTagB" };
        var excludeSlug = "related1";
        var count = 2;

        // Act
        var results = await Repository.GetRelatedAsync(sourceTags, excludeSlug, count: count);

        // Assert
        results.Should().HaveCountLessThanOrEqualTo(count,
            "Should respect the count limit");
    }

    /// <summary>
    /// Test: GetRelatedAsync with no shared tags returns empty
    /// Why: Without tag overlap, no related content can be found
    /// </summary>
    [Fact]
    public virtual async Task GetRelatedAsync_NoSharedTags_ReturnsEmpty()
    {
        // Arrange - use tags that don't match any other content
        var sourceTags = new[] { "UniqueTagThatMatchesNothing" };
        var excludeSlug = "unrelated";

        // Act
        var results = await Repository.GetRelatedAsync(sourceTags, excludeSlug, count: 5);

        // Assert - no fallback, just return empty if no shared tags
        results.Should().BeEmpty("Should return empty when no items share tags");
    }

    #endregion

    #region Property Mapping Tests

    /// <summary>
    /// Test: ExternalUrl property is correctly loaded from database/file
    /// Why: News, blogs, and community items need external URLs to redirect to original sources
    /// </summary>
    [Fact]
    public virtual async Task PropertyMapping_ExternalUrl_IsLoadedCorrectly()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _news/2025-12-08-NET-10-Networking-Improvements.md has external_url set
        // Note: Slugs are normalized to lowercase

        // Act
        var result = await Repository.GetBySlugAsync("news", "net-10-networking-improvements");

        // Assert
        result.Should().NotBeNull("TestCollections should contain the .NET 10 news item");
        result!.ExternalUrl.Should().Be("https://devblogs.microsoft.com/dotnet/dotnet-10-networking-improvements/",
            "ExternalUrl should be loaded correctly from external_url frontmatter/database field");
    }

    /// <summary>
    /// Test: Author property is correctly loaded from database/file
    /// Why: Content attribution requires correct author information
    /// </summary>
    [Fact]
    public virtual async Task PropertyMapping_Author_IsLoadedCorrectly()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _news/2025-12-08-NET-10-Networking-Improvements.md has author: Máňa

        // Act
        var result = await Repository.GetBySlugAsync("news", "net-10-networking-improvements");

        // Assert
        result.Should().NotBeNull();
        result!.Author.Should().Be("Máňa",
            "Author should be loaded correctly including Unicode characters");
    }

    /// <summary>
    /// Test: FeedName property is correctly loaded from database/file
    /// Why: RSS feeds and source attribution require feed_name
    /// </summary>
    [Fact]
    public virtual async Task PropertyMapping_FeedName_IsLoadedCorrectly()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _news/2025-12-08-NET-10-Networking-Improvements.md has feed_name: Microsoft .NET Blog

        // Act
        var result = await Repository.GetBySlugAsync("news", "net-10-networking-improvements");

        // Assert
        result.Should().NotBeNull();
        result!.FeedName.Should().Be("Microsoft .NET Blog",
            "FeedName should be loaded correctly from feed_name frontmatter/database field");
    }

    /// <summary>
    /// Test: Tags array is correctly loaded from database/file
    /// Why: Tags are used for filtering, related content, and tag clouds
    /// </summary>
    [Fact]
    public virtual async Task PropertyMapping_Tags_AreLoadedCorrectly()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _news/2025-12-08-NET-10-Networking-Improvements.md has multiple tags

        // Act
        var result = await Repository.GetBySlugAsync("news", "net-10-networking-improvements");

        // Assert
        result.Should().NotBeNull();
        result!.Tags.Should().NotBeEmpty("Tags should be loaded");
        result.Tags.Should().Contain(".NET", "Should contain .NET tag");
        result.Tags.Should().Contain(".NET 10", "Should contain .NET 10 tag");
        result.Tags.Should().Contain("HTTP", "Should contain HTTP tag");
        result.Tags.Should().Contain("Web Sockets", "Should contain Web Sockets tag");
    }

    /// <summary>
    /// Test: SectionNames array is correctly loaded from database/file
    /// Why: Sections determine where content appears in the navigation
    /// </summary>
    [Fact]
    public virtual async Task PropertyMapping_SectionNames_AreLoadedCorrectly()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _news/2025-12-08-NET-10-Networking-Improvements.md has section_names: [coding, security]

        // Act
        var result = await Repository.GetBySlugAsync("news", "net-10-networking-improvements");

        // Assert
        result.Should().NotBeNull();
        result!.PrimarySectionName.Should().BeOneOf("coding", "security", "Primary section should be one of the assigned sections");
    }

    /// <summary>
    /// Test: PrimarySectionName is correctly computed from SectionNames
    /// Why: Primary section determines the item's main categorization and priority order
    /// </summary>
    [Fact]
    public virtual async Task PropertyMapping_PrimarySectionName_IsComputedCorrectly()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _news/2025-12-08-NET-10-Networking-Improvements.md has primary_section: coding

        // Act
        var result = await Repository.GetBySlugAsync("news", "net-10-networking-improvements");

        // Assert
        result.Should().NotBeNull();
        result!.PrimarySectionName.Should().Be("coding",
            "PrimarySectionName should be computed from section_names");
    }

    /// <summary>
    /// Test: DateEpoch is correctly loaded and represents valid Unix timestamp
    /// Why: Date sorting and filtering depends on correct DateEpoch values
    /// </summary>
    [Fact]
    public virtual async Task PropertyMapping_DateEpoch_IsLoadedCorrectly()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _news/2025-12-08-NET-10-Networking-Improvements.md has date: 2025-12-08 18:05:00 +00:00

        // Act
        var result = await Repository.GetBySlugAsync("news", "net-10-networking-improvements");

        // Assert
        result.Should().NotBeNull();
        result!.DateEpoch.Should().BeGreaterThan(0, "DateEpoch should be a valid Unix timestamp");

        // Convert to DateTimeOffset and verify date
        var date = DateTimeOffset.FromUnixTimeSeconds(result.DateEpoch);
        date.Year.Should().Be(2025, "Year should be 2025");
        date.Month.Should().Be(12, "Month should be December");
        date.Day.Should().Be(8, "Day should be 8");
    }

    /// <summary>
    /// Test: CollectionName is correctly derived from folder structure
    /// Why: Collection routing and filtering depends on correct CollectionName
    /// </summary>
    [Fact]
    public virtual async Task PropertyMapping_CollectionName_IsLoadedCorrectly()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _news/*.md files should have CollectionName = "news"

        // Act
        var result = await Repository.GetBySlugAsync("news", "net-10-networking-improvements");

        // Assert
        result.Should().NotBeNull();
        result!.CollectionName.Should().Be("news",
            "CollectionName should be derived from folder name without underscore prefix");
    }

    /// <summary>
    /// Test: Slug is correctly derived from filename (lowercased)
    /// Why: URL routing depends on correct slug values
    /// </summary>
    [Fact]
    public virtual async Task PropertyMapping_Slug_IsDerivedFromFilename()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: 2025-12-08-NET-10-Networking-Improvements.md -> slug: net-10-networking-improvements (lowercased)

        // Act
        var result = await Repository.GetBySlugAsync("news", "net-10-networking-improvements");

        // Assert
        result.Should().NotBeNull();
        result!.Slug.Should().Be("net-10-networking-improvements",
            "Slug should be derived from filename with date prefix removed and lowercased");
    }

    /// <summary>
    /// Test: Title is correctly loaded from database/file
    /// Why: Title is displayed in content cards and detail pages
    /// </summary>
    [Fact]
    public virtual async Task PropertyMapping_Title_IsLoadedCorrectly()
    {
        // Arrange - data already seeded from TestCollections

        // Act
        var result = await Repository.GetBySlugAsync("news", "net-10-networking-improvements");

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be(".NET 10 Networking Improvements",
            "Title should be loaded correctly from frontmatter/database");
    }

    /// <summary>
    /// Test: Excerpt is correctly extracted from content
    /// Why: Excerpt is shown in content cards and RSS feeds
    /// </summary>
    [Fact]
    public virtual async Task PropertyMapping_Excerpt_IsLoadedCorrectly()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: Content before <!--excerpt_end--> marker

        // Act
        var result = await Repository.GetBySlugAsync("news", "net-10-networking-improvements");

        // Assert
        result.Should().NotBeNull();
        result!.Excerpt.Should().NotBeNullOrEmpty("Excerpt should be populated");
        result.Excerpt.Should().Contain("networking", "Excerpt should describe the content topic");
    }

    /// <summary>
    /// Test: Draft property correctly identifies draft content
    /// Why: Draft content should be excluded from public views
    /// </summary>
    [Fact]
    public virtual async Task PropertyMapping_Draft_IsLoadedCorrectly()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _blogs/2024-01-02-draft-article.md has draft: true

        // Act
        var draftResult = await Repository.GetBySlugAsync("blogs", "draft-article", includeDraft: true);
        var publishedResult = await Repository.GetBySlugAsync("blogs", "test-article");

        // Assert
        draftResult.Should().NotBeNull("Draft article should exist");
        draftResult!.Draft.Should().BeTrue("Draft article should have Draft=true");

        publishedResult.Should().NotBeNull("Published article should exist");
        publishedResult!.Draft.Should().BeFalse("Published article should have Draft=false");
    }

    /// <summary>
    /// Test: Url property is correctly computed for internal linking
    /// Why: Internal navigation uses the Url property
    /// </summary>
    [Fact]
    public virtual async Task PropertyMapping_Url_IsComputedCorrectly()
    {
        // Arrange - data already seeded from TestCollections

        // Act
        var result = await Repository.GetBySlugAsync("news", "net-10-networking-improvements");

        // Assert
        result.Should().NotBeNull();
        result!.GetHref().Should().NotBeNullOrEmpty("Url should be populated");
        result.GetHref().Should().Contain("net-10-networking-improvements", "Url should contain the lowercase slug");
    }

    /// <summary>
    /// Test: Content with YouTube video has external_url correctly loaded
    /// Why: Video collection items need external URLs for YouTube embedding
    /// </summary>
    [Fact]
    public virtual async Task PropertyMapping_ExternalUrl_YouTubeVideo_IsLoadedCorrectly()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _videos/2026-01-05-Hands-On-Lab-The-Power-of-GitHub-Copilot-in-Visual-Studio-Code.md
        //           has external_url: https://www.youtube.com/watch?v=dxDqelvVc2U

        // Act
        var result = await Repository.GetBySlugAsync("videos", "hands-on-lab-the-power-of-github-copilot-in-visual-studio-code");

        // Assert
        result.Should().NotBeNull("TestCollections should contain the video item");
        result!.ExternalUrl.Should().Be("https://www.youtube.com/watch?v=dxDqelvVc2U",
            "ExternalUrl should be loaded correctly for YouTube videos");
    }

    /// <summary>
    /// Test: Blog post with external_url correctly loads all properties
    /// Why: Blogs redirect to external sources and need all properties populated
    /// </summary>
    [Fact]
    public virtual async Task PropertyMapping_BlogWithExternalUrl_AllPropertiesLoaded()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _blogs/2026-01-02-From-Tool-to-Teammate-Using-GitHub-Copilot-as-a-Collaborative-Partner.md

        // Act
        var result = await Repository.GetBySlugAsync("blogs", "from-tool-to-teammate-using-github-copilot-as-a-collaborative-partner");

        // Assert
        result.Should().NotBeNull("TestCollections should contain the blog post");

        // Verify all key properties are loaded (slugs are lowercased)
        result!.Slug.Should().Be("from-tool-to-teammate-using-github-copilot-as-a-collaborative-partner");
        result.Title.Should().Be("From Tool to Teammate: Using GitHub Copilot as a Collaborative Partner");
        result.Author.Should().Contain("Randy Pagels", "Author should contain Randy Pagels");
        result.ExternalUrl.Should().Be("https://www.cooknwithcopilot.com/blog/from-tool-to-teammate.html");
        result.FeedName.Should().Be("Randy Pagels");
        result.CollectionName.Should().Be("blogs");
        result.PrimarySectionName.Should().BeOneOf("ai", "github-copilot");
        result.Tags.Should().Contain("Code Review");
        result.Tags.Should().Contain("Developer Tools");
        result.Draft.Should().BeFalse();
    }

    #endregion
}
