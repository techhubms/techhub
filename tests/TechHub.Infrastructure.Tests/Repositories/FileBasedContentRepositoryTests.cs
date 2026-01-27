using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Infrastructure.Repositories;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Tests for FileBasedContentRepository - loading and querying content from markdown files
/// </summary>
public class FileBasedContentRepositoryTests : IDisposable
{
    private readonly string _tempDirectory;
    private readonly string _collectionsPath;
    private readonly IContentRepository _repository;
    private readonly IMarkdownService _markdownService;
    private readonly AppSettings _settings;

    public FileBasedContentRepositoryTests()
    {
        // Setup: Create temporary directory for test files
        _tempDirectory = Path.Combine(Path.GetTempPath(), $"techhub_content_tests_{Guid.NewGuid()}");
        _collectionsPath = Path.Combine(_tempDirectory, "collections");
        Directory.CreateDirectory(_collectionsPath);

        // Setup: Configure AppSettings with all required sections
        _settings = new AppSettings
        {
            Content = new ContentSettings
            {
                CollectionsPath = _collectionsPath,
                Sections = [] // Empty for content tests
            }
        };

        // Setup: Create mock IHostEnvironment
        var mockEnvironment = new Mock<IHostEnvironment>();
        mockEnvironment.Setup(e => e.ContentRootPath).Returns(_tempDirectory);

        // Setup: Create real TagMatchingService for accurate tag filtering in tests
        var tagMatchingService = new TagMatchingService();

        // Setup: Create MemoryCache for caching
        var cache = new MemoryCache(new MemoryCacheOptions());

        // Setup: Create dependencies
        _markdownService = new MarkdownService();
        _repository = new FileBasedContentRepository(
            Options.Create(_settings),
            _markdownService,
            tagMatchingService,
            mockEnvironment.Object,
            cache
        );
    }

    public void Dispose()
    {
        // Cleanup: Delete temporary test directory
        if (Directory.Exists(_tempDirectory))
        {
            Directory.Delete(_tempDirectory, true);
        }
    }

    /// <summary>
    /// Test: GetAllAsync loads content from all collection directories
    /// Why: Core functionality - repository must discover and load all markdown files
    /// </summary>
    [Fact]
    public async Task GetAllAsync_MultipleCollections_LoadsAllContent()
    {
        // Arrange: Create content in _news and _videos directories
        var newsDir = Path.Combine(_collectionsPath, "_news");
        var videosDir = Path.Combine(_collectionsPath, "_videos");
        Directory.CreateDirectory(newsDir);
        Directory.CreateDirectory(videosDir);

        var newsFile = Path.Combine(newsDir, "2025-01-15-ai-announcement.md");
        await File.WriteAllTextAsync(newsFile, """
            ---
            title: AI Product Launch
            date: 2025-01-15
            author: Tech Team
            section_names: [AI]
            tags: [Announcement, AI]
            ---
            
            Full details about the AI product launch.
            """);

        var videoFile = Path.Combine(videosDir, "2025-01-10-tutorial.md");
        await File.WriteAllTextAsync(videoFile, """
            ---
            title: Getting Started Tutorial
            date: 2025-01-10
            author: Educator
            section_names: [GitHub Copilot]
            tags: [GitHub Copilot, Videos, Tutorial, Beginner]
            videoUrl: https://www.youtube.com/watch?v=ABC123
            ---
            
            This tutorial covers the fundamentals.
            """);

        // Act: Load all content
        var content = await _repository.GetAllAsync();

        // Assert: Both items loaded from different collections
        content.Count.Should().Be(2);
        content.Should().Contain(c => c.Title == "AI Product Launch");
        content.Should().Contain(c => c.Title == "Getting Started Tutorial");
    }

    /// <summary>
    /// Test: GetByCollectionAsync filters content by collection name
    /// Why: Users need to view content from specific collections (news, videos, etc.)
    /// </summary>
    [Fact]
    public async Task GetByCollectionAsync_SpecificCollection_ReturnsOnlyMatchingContent()
    {
        // Arrange: Create content in news and blogs collections
        var newsDir = Path.Combine(_collectionsPath, "_news");
        var blogsDir = Path.Combine(_collectionsPath, "_blogs");
        Directory.CreateDirectory(newsDir);
        Directory.CreateDirectory(blogsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "news1.md"), """
            ---
            title: News Item 1
            date: 2025-01-01
            section_names: [AI]
            tags: [News]
            ---
            News content
            """);

        await File.WriteAllTextAsync(Path.Combine(blogsDir, "blog1.md"), """
            ---
            title: Blog Post 1
            date: 2025-01-02
            section_names: [AI]
            tags: [Blog]
            ---
            Blog content
            """);

        // Act: Get only news collection
        var newsContent = await _repository.GetByCollectionAsync("news");

        // Assert: Only news items returned, not blogs
        newsContent.Should().ContainSingle();
        newsContent[0].Title.Should().Be("News Item 1");
    }

    /// <summary>
    /// Test: GetByCollectionAsync with "all" returns all content
    /// Why: The /all/all collection page should show all content across all collections
    ///      "all" is a virtual collection that returns all content from all collections
    /// </summary>
    [Fact]
    public async Task GetByCollectionAsync_AllCollection_ReturnsAllContent()
    {
        // Arrange: Create content in different collections
        var newsDir = Path.Combine(_collectionsPath, "_news");
        var blogsDir = Path.Combine(_collectionsPath, "_blogs");
        Directory.CreateDirectory(newsDir);
        Directory.CreateDirectory(blogsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "news1.md"), """
            ---
            title: News Item 1
            date: 2025-01-01
            section_names: [ai]
            tags: [News]
            ---
            News content
            """);

        await File.WriteAllTextAsync(Path.Combine(blogsDir, "blog1.md"), """
            ---
            title: Blog Post 1
            date: 2025-01-02
            section_names: [ai]
            tags: [Blog]
            ---
            Blog content
            """);

        // Act: Get "all" collection (virtual collection)
        var allContent = await _repository.GetByCollectionAsync("all");

        // Assert: Returns all content from all collections
        allContent.Should().HaveCount(2);
        allContent.Should().Contain(item => item.Title == "News Item 1");
        allContent.Should().Contain(item => item.Title == "Blog Post 1");
    }

    /// <summary>
    /// Test: GetByCollectionAsync excludes draft items by default
    /// Why: Draft videos in ghc-features and other collections should NOT appear by default
    ///      Draft items should only be visible when explicitly requested with includeDraft=true
    /// </summary>
    [Fact]
    public async Task GetByCollectionAsync_ExcludesDraftItemsByDefault()
    {
        // Arrange: Create videos in root and ghc-features subdirectory
        var videosDir = Path.Combine(_collectionsPath, "_videos");
        var ghcFeaturesDir = Path.Combine(videosDir, "ghc-features");
        Directory.CreateDirectory(videosDir);
        Directory.CreateDirectory(ghcFeaturesDir);

        // Root video (published, should appear)
        await File.WriteAllTextAsync(Path.Combine(videosDir, "2025-01-15-published-video.md"), """
            ---
            title: Published Video
            date: 2025-01-15
            section_names: [github-copilot]
            tags: [Video]
            draft: false
            ---
            This is a published video in the root videos collection.
            """);

        // ghc-features video (draft, should NOT appear without includeDraft=true)
        await File.WriteAllTextAsync(Path.Combine(ghcFeaturesDir, "2026-08-29-draft-feature.md"), """
            ---
            title: Draft Feature Video
            date: 2026-08-29
            section_names: [github-copilot]
            tags: [Video]
            draft: true
            ---
            This is a draft video in ghc-features subcollection.
            """);

        // ghc-features video (published, SHOULD appear - subcollections are included)
        await File.WriteAllTextAsync(Path.Combine(ghcFeaturesDir, "2025-01-10-published-feature.md"), """
            ---
            title: Published Feature Video
            date: 2025-01-10
            section_names: [github-copilot]
            tags: [Video]
            draft: false
            ---
            This is a published video in ghc-features subcollection.
            """);

        // Act: Get videos collection (should exclude drafts but include subcollections)
        var videosContent = await _repository.GetByCollectionAsync("videos", includeDraft: false);

        // Assert: Published items from root AND subcollections returned, but NOT drafts
        videosContent.Should().HaveCount(2);
        videosContent.Should().Contain(v => v.Title == "Published Video");
        videosContent.Should().Contain(v => v.Title == "Published Feature Video");
        videosContent.Should().NotContain(v => v.Title == "Draft Feature Video");
        videosContent.Should().AllSatisfy(v => v.Draft.Should().BeFalse());
    }

    /// <summary>
    /// Test: GetByCollectionAsync includes draft items when includeDraft=true
    /// Why: When explicitly requesting drafts, they should be included (e.g., for features showcase)
    /// </summary>
    [Fact]
    public async Task GetByCollectionAsync_IncludesDraftItemsWhenRequested()
    {
        // Arrange: Create videos with draft flag
        var videosDir = Path.Combine(_collectionsPath, "_videos");
        var ghcFeaturesDir = Path.Combine(videosDir, "ghc-features");
        Directory.CreateDirectory(videosDir);
        Directory.CreateDirectory(ghcFeaturesDir);

        // Published video
        await File.WriteAllTextAsync(Path.Combine(videosDir, "2025-01-15-published.md"), """
            ---
            title: Published Video
            date: 2025-01-15
            section_names: [github-copilot]
            tags: [Video]
            draft: false
            ---
            Published content
            """);

        // Draft video in ghc-features
        await File.WriteAllTextAsync(Path.Combine(ghcFeaturesDir, "2026-08-29-draft.md"), """
            ---
            title: Draft Video
            date: 2026-08-29
            section_names: [github-copilot]
            tags: [Video]
            draft: true
            ---
            Draft content
            """);

        // Act: Get videos collection WITH drafts
        var videosContent = await _repository.GetByCollectionAsync("videos", includeDraft: true);

        // Assert: Both published AND draft items returned
        videosContent.Should().HaveCount(2);
        videosContent.Should().Contain(v => v.Title == "Published Video" && !v.Draft);
        videosContent.Should().Contain(v => v.Title == "Draft Video" && v.Draft);
    }

    /// <summary>
    /// Test: GetBySectionAsync excludes draft items by default
    /// Why: When viewing a section (e.g., /ai/videos), draft items should NOT appear
    ///      Draft items are only shown when explicitly requested (e.g., ghcFeature=true)
    /// </summary>
    [Fact]
    public async Task GetBySectionAsync_ExcludesDraftItemsByDefault()
    {
        // Arrange: Create published and draft items in same section
        var videosDir = Path.Combine(_collectionsPath, "_videos");
        Directory.CreateDirectory(videosDir);

        // Published video
        await File.WriteAllTextAsync(Path.Combine(videosDir, "2025-01-01-published-video.md"), """
            ---
            title: Published Video
            date: 2025-01-01
            section_names: [ai]
            tags: [AI, Video]
            draft: false
            ---
            Published content
            """);

        // Draft video (future date + draft: true)
        await File.WriteAllTextAsync(Path.Combine(videosDir, "2026-08-29-draft-video.md"), """
            ---
            title: Draft Video
            date: 2026-08-29
            section_names: [ai]
            tags: [AI, Video]
            draft: true
            ---
            Draft content
            """);

        // Act: Get AI section WITHOUT drafts (default)
        var aiContent = await _repository.GetBySectionAsync("ai", includeDraft: false);

        // Assert: Only published items returned
        aiContent.Should().ContainSingle();
        aiContent[0].Title.Should().Be("Published Video");
        aiContent[0].Draft.Should().BeFalse();
    }

    /// <summary>
    /// Test: GetBySectionAsync includes draft items when includeDraft=true
    /// Why: When explicitly requesting drafts (e.g., ghcFeature=true), both published AND draft items returned
    /// </summary>
    [Fact]
    public async Task GetBySectionAsync_IncludesDraftItemsWhenRequested()
    {
        // Arrange: Create published and draft items in same section
        var videosDir = Path.Combine(_collectionsPath, "_videos");
        Directory.CreateDirectory(videosDir);

        // Published video
        await File.WriteAllTextAsync(Path.Combine(videosDir, "2025-01-01-published-video.md"), """
            ---
            title: Published Video
            date: 2025-01-01
            section_names: [ai]
            tags: [AI, Video]
            draft: false
            ---
            Published content
            """);

        // Draft video
        await File.WriteAllTextAsync(Path.Combine(videosDir, "2026-08-29-draft-video.md"), """
            ---
            title: Draft Video
            date: 2026-08-29
            section_names: [ai]
            tags: [AI, Video]
            draft: true
            ---
            Draft content
            """);

        // Act: Get AI section WITH drafts
        var aiContent = await _repository.GetBySectionAsync("ai", includeDraft: true);

        // Assert: Both published AND draft items returned
        aiContent.Should().HaveCount(2);
        aiContent.Should().Contain(v => v.Title == "Published Video" && !v.Draft);
        aiContent.Should().Contain(v => v.Title == "Draft Video" && v.Draft);
    }

    /// <summary>
    /// Test: GetByCollectionAsync includes subcollection items
    /// Why: When querying "videos", should include both root videos and ghc-features videos
    ///      Subcollections are part of the collection hierarchy
    /// </summary>
    [Fact]
    public async Task GetByCollectionAsync_IncludesSubcollectionItems()
    {
        // Arrange: Create videos in root and subdirectory
        var videosDir = Path.Combine(_collectionsPath, "_videos");
        var ghcFeaturesDir = Path.Combine(videosDir, "ghc-features");
        Directory.CreateDirectory(videosDir);
        Directory.CreateDirectory(ghcFeaturesDir);

        // Root video
        await File.WriteAllTextAsync(Path.Combine(videosDir, "2025-01-15-root-video.md"), """
            ---
            title: Root Video
            date: 2025-01-15
            section_names: [github-copilot]
            tags: [Video]
            ---
            Root collection video
            """);

        // Subcollection video
        await File.WriteAllTextAsync(Path.Combine(ghcFeaturesDir, "2025-01-10-feature-video.md"), """
            ---
            title: Feature Video
            date: 2025-01-10
            section_names: [github-copilot]
            tags: [Video]
            ---
            Subcollection video
            """);

        // Act: Get videos collection
        var videosContent = await _repository.GetByCollectionAsync("videos");

        // Assert: Both root and subcollection items returned
        videosContent.Should().HaveCount(2);
        videosContent.Should().Contain(v => v.Title == "Root Video" && v.SubcollectionName == null);
        videosContent.Should().Contain(v => v.Title == "Feature Video" && v.SubcollectionName == "ghc-features");
    }

    /// <summary>
    /// Test: GetByCollectionAsync includes vscode-updates subcollection items
    /// Why: When querying "videos", should include items from vscode-updates subdirectory
    ///      This is a real-world subcollection used for VS Code update videos
    /// </summary>
    [Fact]
    public async Task GetByCollectionAsync_IncludesVscodeUpdatesSubcollection()
    {
        // Arrange: Create videos in root and vscode-updates subdirectory
        var videosDir = Path.Combine(_collectionsPath, "_videos");
        var vscodeUpdatesDir = Path.Combine(videosDir, "vscode-updates");
        Directory.CreateDirectory(videosDir);
        Directory.CreateDirectory(vscodeUpdatesDir);

        // Root video
        await File.WriteAllTextAsync(Path.Combine(videosDir, "2025-01-15-regular-video.md"), """
            ---
            title: Regular Video
            date: 2025-01-15
            section_names: [github-copilot]
            tags: [Video]
            ---
            Regular video content
            """);

        // vscode-updates subcollection video
        await File.WriteAllTextAsync(Path.Combine(vscodeUpdatesDir, "2025-01-10-vscode-update.md"), """
            ---
            title: VS Code Update Video
            date: 2025-01-10
            section_names: [github-copilot]
            tags: [VS Code]
            ---
            VS Code update content
            """);

        // Act: Get videos collection
        var videosContent = await _repository.GetByCollectionAsync("videos");

        // Assert: Both root and vscode-updates items returned
        videosContent.Should().HaveCount(2);
        videosContent.Should().Contain(v => v.Title == "Regular Video" && v.SubcollectionName == null);
        videosContent.Should().Contain(v => v.Title == "VS Code Update Video" && v.SubcollectionName == "vscode-updates");
    }

    /// <summary>
    /// Test: GetByCollectionAsync includes ghc-features subcollection items
    /// Why: When querying "videos", should include items from ghc-features subdirectory
    ///      This is a real-world subcollection used for GitHub Copilot feature demo videos
    /// </summary>
    [Fact]
    public async Task GetByCollectionAsync_IncludesGhcFeaturesSubcollection()
    {
        // Arrange: Create videos in root and ghc-features subdirectory
        var videosDir = Path.Combine(_collectionsPath, "_videos");
        var ghcFeaturesDir = Path.Combine(videosDir, "ghc-features");
        Directory.CreateDirectory(videosDir);
        Directory.CreateDirectory(ghcFeaturesDir);

        // Root video
        await File.WriteAllTextAsync(Path.Combine(videosDir, "2025-01-15-regular-video.md"), """
            ---
            title: Regular Video
            date: 2025-01-15
            section_names: [github-copilot]
            tags: [Video]
            ---
            Regular video content
            """);

        // ghc-features subcollection video (GitHub Copilot feature demo)
        await File.WriteAllTextAsync(Path.Combine(ghcFeaturesDir, "2025-01-10-code-completion-demo.md"), """
            ---
            title: Code Completion Demo
            date: 2025-01-10
            section_names: [github-copilot]
            tags: [GitHub Copilot, Demo]
            ghc_feature: true
            ---
            GitHub Copilot code completion feature demo
            """);

        // Act: Get videos collection
        var videosContent = await _repository.GetByCollectionAsync("videos");

        // Assert: Both root and ghc-features items returned
        videosContent.Should().HaveCount(2);
        videosContent.Should().Contain(v => v.Title == "Regular Video" && v.SubcollectionName == null);
        videosContent.Should().Contain(v => v.Title == "Code Completion Demo" && v.SubcollectionName == "ghc-features");

        // Verify ghc_feature flag is set correctly
        var ghcFeatureVideo = videosContent.Single(v => v.SubcollectionName == "ghc-features");
        ghcFeatureVideo.GhcFeature.Should().BeTrue("ghc-features video should have GhcFeature flag set");
    }

    /// <summary>
    /// Test: ghc-features subcollection items have correct URL with collection name in path
    /// Why: URLs always use collection name, not subcollection (subcollections are for filtering only)
    ///      e.g., /github-copilot/videos/slug (not /github-copilot/ghc-features/slug)
    /// </summary>
    [Fact]
    public async Task GhcFeaturesSubcollectionItems_HaveCorrectUrlWithCollectionInPath()
    {
        // Arrange: Create video in ghc-features subdirectory
        var videosDir = Path.Combine(_collectionsPath, "_videos");
        var ghcFeaturesDir = Path.Combine(videosDir, "ghc-features");
        Directory.CreateDirectory(videosDir);
        Directory.CreateDirectory(ghcFeaturesDir);

        await File.WriteAllTextAsync(Path.Combine(ghcFeaturesDir, "2025-01-10-agent-mode-demo.md"), """
            ---
            title: Agent Mode Demo
            date: 2025-01-10
            section_names: [github-copilot]
            tags: [GitHub Copilot, Agent Mode]
            ghc_feature: true
            ---
            GitHub Copilot agent mode feature demo
            """);

        // Act: Get videos collection
        var videosContent = await _repository.GetByCollectionAsync("videos");

        // Assert: URL uses collection name, not subcollection name (subcollections are for filtering)
        var ghcFeatureItem = videosContent.Single();
        ghcFeatureItem.SubcollectionName.Should().Be("ghc-features");
        ghcFeatureItem.CollectionName.Should().Be("videos");
        ghcFeatureItem.Url.Should().Contain("/videos/", "URL should use collection name in path");
        ghcFeatureItem.Url.Should().NotContain("/ghc-features/", "URL should not contain subcollection name (used for filtering only)");
        ghcFeatureItem.Url.Should().Contain("/agent-mode-demo", "URL should contain the slug");
        ghcFeatureItem.Url.Should().StartWith("/github-copilot/", "URL should start with primary section");
    }

    /// <summary>
    /// Test: Subcollection items have correct URL with collection name in path
    /// Why: URLs always use collection name, not subcollection (subcollections are for filtering only)
    ///      e.g., /github-copilot/videos/slug (not /github-copilot/vscode-updates/slug)
    /// </summary>
    [Fact]
    public async Task SubcollectionItems_HaveCorrectUrlWithCollectionInPath()
    {
        // Arrange: Create video in vscode-updates subdirectory
        var videosDir = Path.Combine(_collectionsPath, "_videos");
        var vscodeUpdatesDir = Path.Combine(videosDir, "vscode-updates");
        Directory.CreateDirectory(videosDir);
        Directory.CreateDirectory(vscodeUpdatesDir);

        await File.WriteAllTextAsync(Path.Combine(vscodeUpdatesDir, "2025-01-10-my-vscode-update.md"), """
            ---
            title: My VS Code Update
            date: 2025-01-10
            section_names: [github-copilot]
            tags: [VS Code]
            ---
            VS Code update content
            """);

        // Act: Get videos collection
        var videosContent = await _repository.GetByCollectionAsync("videos");

        // Assert: URL uses collection name, not subcollection name (subcollections are for filtering)
        var vscodeItem = videosContent.Single();
        vscodeItem.SubcollectionName.Should().Be("vscode-updates");
        vscodeItem.CollectionName.Should().Be("videos");
        vscodeItem.Url.Should().Contain("/videos/", "URL should use collection name in path");
        vscodeItem.Url.Should().NotContain("/vscode-updates/", "URL should not contain subcollection name (used for filtering only)");
        vscodeItem.Url.Should().Contain("/my-vscode-update", "URL should contain the slug");
        vscodeItem.Url.Should().StartWith("/github-copilot/", "URL should start with primary section");
    }

    /// <summary>
    /// Test: GetBySectionAsync filters content by section name
    /// Why: Sections display content filtered by section name (ai, github-copilot, etc.)
    /// </summary>
    [Fact]
    public async Task GetBySectionAsync_SpecificSection_ReturnsMatchingContent()
    {
        // Arrange: Create content with different sections
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "ai-news.md"), """
            ---
            title: AI News
            date: 2025-01-01
            section_names: [ai]
            tags: [AI]
            ---
            AI content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "ghc-news.md"), """
            ---
            title: GitHub Copilot News
            date: 2025-01-02
            section_names: [github-copilot]
            tags: [Copilot]
            ---
            Copilot content
            """);

        // Act: Get AI section content
        var aiContent = await _repository.GetBySectionAsync("ai");

        // Assert: Only AI section items returned
        aiContent.Should().ContainSingle();
        aiContent[0].Title.Should().Be("AI News");
        aiContent[0].SectionNames.Should().Contain("ai");
    }

    /// <summary>
    /// Test: GetBySectionAsync with "all" returns all content
    /// Why: The /all section page should show all content across all sections
    ///      "all" is a virtual section that returns all content from all sections
    /// </summary>
    [Fact]
    public async Task GetBySectionAsync_AllSection_ReturnsAllContent()
    {
        // Arrange: Create content with different sections
        var newsDir = Path.Combine(_collectionsPath, "_news");
        var blogsDir = Path.Combine(_collectionsPath, "_blogs");
        Directory.CreateDirectory(newsDir);
        Directory.CreateDirectory(blogsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "ai-news.md"), """
            ---
            title: AI News
            date: 2025-01-01
            section_names: [ai]
            tags: [AI]
            ---
            AI content
            """);

        await File.WriteAllTextAsync(Path.Combine(blogsDir, "ghc-blog.md"), """
            ---
            title: GitHub Copilot Blog
            date: 2025-01-02
            section_names: [github-copilot]
            tags: [Copilot]
            ---
            Copilot content
            """);

        // Act: Get "all" section content (virtual section)
        var allContent = await _repository.GetBySectionAsync("all");

        // Assert: Returns all content from all sections
        allContent.Should().HaveCount(2);
        allContent.Should().Contain(item => item.Title == "AI News");
        allContent.Should().Contain(item => item.Title == "GitHub Copilot Blog");
    }

    /// <summary>
    /// Test: GetBySlugAsync retrieves content by slug WITHOUT date prefix
    /// Why: URLs should not include date prefix (e.g., /ai/videos/what-quantum-safe-is, not /ai/videos/2026-01-12-what-quantum-safe-is)
    /// </summary>
    [Fact]
    public async Task GetBySlugAsync_FileWithDatePrefix_SlugStripsDatePrefix()
    {
        // Arrange: Create video item with date prefix in filename
        var videosDir = Path.Combine(_collectionsPath, "_videos");
        Directory.CreateDirectory(videosDir);

        await File.WriteAllTextAsync(Path.Combine(videosDir, "2026-01-12-what-quantum-safe-is-and-why-we-need-it.md"), """
            ---
            title: What Quantum-Safe Is And Why We Need It
            date: 2026-01-12
            section_names: [AI]
            tags: [Security, Quantum]
            ---
            
            Quantum computing security details.
            """);

        // Act: Get item by slug WITHOUT date prefix
        var item = await _repository.GetBySlugAsync("videos", "what-quantum-safe-is-and-why-we-need-it");

        // Assert: Item found and slug does NOT contain date prefix
        item.Should().NotBeNull();
        item!.Title.Should().Be("What Quantum-Safe Is And Why We Need It");
        item!.Slug.Should().Be("what-quantum-safe-is-and-why-we-need-it");
        item!.Slug.Should().NotStartWith("2026-");
        item!.CollectionName.Should().Be("videos");
        item!.RenderedHtml.Should().Contain("Quantum computing security");
    }

    /// <summary>
    /// Test: GetBySlugAsync with old date-prefixed slug returns null
    /// Why: Old URLs with date prefix should no longer work after slug format change
    /// </summary>
    [Fact]
    public async Task GetBySlugAsync_OldDatePrefixedSlug_ReturnsNull()
    {
        // Arrange: Create video item with date prefix in filename
        var videosDir = Path.Combine(_collectionsPath, "_videos");
        Directory.CreateDirectory(videosDir);

        await File.WriteAllTextAsync(Path.Combine(videosDir, "2026-01-12-what-quantum-safe-is.md"), """
            ---
            title: What Quantum-Safe Is
            date: 2026-01-12
            section_names: [AI]
            tags: [Security]
            ---
            
            Content here.
            """);

        // Act: Try to get item with OLD date-prefixed slug (should not work anymore)
        var item = await _repository.GetBySlugAsync("videos", "2026-01-12-what-quantum-safe-is");

        // Assert: Old URL format should not work
        item.Should().BeNull();
    }

    /// <summary>
    /// Test: GetBySlugAsync retrieves single content item by ID
    /// Why: Display individual content detail pages by URL slug
    /// </summary>
    [Fact]
    public async Task GetBySlugAsync_ExistingItem_ReturnsItem()
    {
        // Arrange: Create news item with specific ID
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-15-product-launch.md"), """
            ---
            title: Product Launch
            date: 2025-01-15
            section_names: [AI]
            tags: [Announcement]
            ---
            
            Full product launch details here.
            """);

        // Act: Get item by ID (filename without extension)
        var item = await _repository.GetBySlugAsync("news", "product-launch");

        // Assert: Correct item returned with all properties
        item.Should().NotBeNull();
        item!.Title.Should().Be("Product Launch");
        item!.Slug.Should().Be("product-launch");
        item!.CollectionName.Should().Be("news");
        item!.RenderedHtml.Should().Contain("Full product launch details");
    }

    /// <summary>
    /// Test: GetBySlugAsync returns null for non-existent item
    /// Why: Graceful handling of missing content (404 pages)
    /// </summary>
    [Fact]
    public async Task GetBySlugAsync_NonExistentItem_ReturnsNull()
    {
        // Arrange: Create empty news directory
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        // Act: Try to get non-existent item
        var item = await _repository.GetBySlugAsync("news", "missing-item");

        // Assert: Null returned (no exception thrown)
        item.Should().BeNull();
    }
}
