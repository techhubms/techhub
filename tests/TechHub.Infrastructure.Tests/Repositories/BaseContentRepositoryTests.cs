using FluentAssertions;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Base class for content repository integration tests.
/// Contains the superset of assertions that ALL repository types (FileBased, SQLite, PostgreSQL) must support.
/// Derived classes must implement CreateRepository() to provide their specific repository implementation.
/// All test data comes from TestCollections directory - NO manual insertions allowed.
/// </summary>
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
        var results = await Repository.GetAllAsync();

        // Assert
        results.Should().NotBeEmpty();
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
        var results = await Repository.GetByCollectionAsync("blogs");

        // Assert
        results.Should().NotBeEmpty("TestCollections should contain blog posts");
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
        var results = await Repository.GetByCollectionAsync("all");

        // Assert
        results.Should().NotBeEmpty("TestCollections should contain multiple items");
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
        var results = await Repository.GetByCollectionAsync("blogs", includeDraft: false);

        // Assert
        results.Should().NotContain(item => item.Slug == "2024-01-02-draft-article", 
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
        var results = await Repository.GetByCollectionAsync("blogs", includeDraft: true);

        // Assert
        results.Should().Contain(item => item.Slug == "2024-01-02-draft-article" && item.Draft, 
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
        // Expected: _ghc-features/*.md and _vscode-updates/*.md exist

        // Act
        var results = await Repository.GetByCollectionAsync("videos");

        // Assert
        results.Should().NotBeEmpty("TestCollections should contain video subcollections");
        results.Should().OnlyContain(item => 
            item.CollectionName == "ghc-features" || 
            item.CollectionName == "vscode-updates",
            "videos collection should map to ghc-features and vscode-updates subcollections");
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
        var results = await Repository.GetBySectionAsync("ai");

        // Assert
        results.Should().NotBeEmpty("TestCollections should contain AI section content");
        results.Should().OnlyContain(item => item.SectionNames.Contains("ai"), 
            "Should only return items with 'ai' in section_names");
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
        var results = await Repository.GetBySectionAsync("all");

        // Assert
        results.Should().NotBeEmpty("TestCollections should contain multiple items");
        results.Should().NotContain(item => item.Draft, "Should exclude drafts by default");
        // Should contain items from different sections
        var sections = results.SelectMany(r => r.SectionNames).Distinct().ToList();
        sections.Should().HaveCountGreaterThan(1, "all section should aggregate multiple section types");
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
        var results = await Repository.GetBySectionAsync("ai", includeDraft: false);

        // Assert
        results.Should().NotContain(item => item.Slug == "2024-01-02-draft-article", 
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
        var results = await Repository.GetBySectionAsync("ai", includeDraft: true);

        // Assert
        results.Should().Contain(item => item.Slug == "2024-01-02-draft-article" && item.Draft, 
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
        var result = await Repository.GetBySlugAsync("blogs", "2024-01-01-test-article");

        // Assert
        result.Should().NotBeNull("TestCollections should contain the test article");
        result!.Slug.Should().Be("2024-01-01-test-article");
        result.Title.Should().Be("Test Article with AI and Azure Tags");
        result.Tags.Should().BeEquivalentTo(new[] { "AI", "Azure" });
        result.SectionNames.Should().Contain(new[] { "ai", "cloud" });
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
        var result = await Repository.GetBySlugAsync("blogs", "2024-01-02-draft-article", includeDraft: false);

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
        var result = await Repository.GetBySlugAsync("blogs", "2024-01-02-draft-article", includeDraft: true);

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
        var results = await Repository.GetByCollectionAsync("videos");

        // Assert: Find the vscode-updates item
        var vscodeItem = results.FirstOrDefault(v => v.SubcollectionName == "vscode-updates");
        vscodeItem.Should().NotBeNull("TestCollections should contain a vscode-updates item");
        vscodeItem!.CollectionName.Should().Be("videos", "Subcollection items should have collection name 'videos'");
        vscodeItem.Url.Should().Contain("/videos/", "URL should use collection name in path");
        vscodeItem.Url.Should().NotContain("/vscode-updates/", "URL should not contain subcollection name (used for filtering only)");
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
        var results = await Repository.GetByCollectionAsync("videos");

        // Assert: Find a ghc-features item
        var ghcFeatureItem = results.FirstOrDefault(v => v.SubcollectionName == "ghc-features");
        ghcFeatureItem.Should().NotBeNull("TestCollections should contain a ghc-features item");
        ghcFeatureItem!.CollectionName.Should().Be("videos", "Subcollection items should have collection name 'videos'");
        ghcFeatureItem.Url.Should().Contain("/videos/", "URL should use collection name in path");
        ghcFeatureItem.Url.Should().NotContain("/ghc-features/", "URL should not contain subcollection name (used for filtering only)");
    }

    #endregion
}
