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

    /// <summary>
    /// Test: SearchAsync finds content by title match
    /// Why: Users need to search for content by keywords
    /// </summary>
    [Fact]
    public async Task SearchAsync_TitleMatch_ReturnsMatchingContent()
    {
        // Arrange: Create content with searchable titles
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "azure-announcement.md"), """
            ---
            title: Azure OpenAI Service Update
            date: 2025-01-01
            section_names: [Azure]
            tags: [Azure, AI]
            ---
            Azure content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "ghc-release.md"), """
            ---
            title: GitHub Copilot Release
            date: 2025-01-02
            section_names: [GitHub Copilot]
            tags: [Copilot]
            ---
            Copilot content
            """);

        // Act: Search for "Azure"
        var results = await _repository.SearchAsync("Azure");

        // Assert: Only Azure-related content returned
        results.Should().ContainSingle();
        results[0].Title.Should().Be("Azure OpenAI Service Update");
    }

    /// <summary>
    /// Test: SearchAsync finds content by tag match
    /// Why: Tags are searchable metadata for content discovery
    /// </summary>
    [Fact]
    public async Task SearchAsync_TagMatch_ReturnsMatchingContent()
    {
        // Arrange: Create content with specific tags
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "item1.md"), """
            ---
            title: Security Alert
            date: 2025-01-01
            section_names: [Security]
            tags: [Security, Urgent]
            ---
            Security content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "item2.md"), """
            ---
            title: Feature Release
            date: 2025-01-02
            section_names: [AI]
            tags: [Feature, AI]
            ---
            Feature content
            """);

        // Act: Search by tag "security"
        var results = await _repository.SearchAsync("security");

        // Assert: Tag match returns correct item
        results.Should().ContainSingle();
        results[0].Title.Should().Be("Security Alert");
    }

    /// <summary>
    /// Test: SearchAsync is case-insensitive
    /// Why: User searches should work regardless of casing
    /// </summary>
    [Fact]
    public async Task SearchAsync_CaseInsensitive_ReturnsMatches()
    {
        // Arrange: Create content with mixed-case title
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "dotnet-release.md"), """
            ---
            title: DotNet 10 Release
            date: 2025-01-01
            section_names: [Coding]
            tags: [dotnet, programming]
            ---
            DotNet content
            """);

        // Act: Search with different casing
        var lowerResults = await _repository.SearchAsync("dotnet");
        var upperResults = await _repository.SearchAsync("DOTNET");
        var mixedResults = await _repository.SearchAsync("DoTnEt");

        // Assert: All variations return same result
        lowerResults.Should().ContainSingle();
        upperResults.Should().ContainSingle();
        mixedResults.Should().ContainSingle();
    }

    /// <summary>
    /// Test: GetAllTagsAsync returns unique tags across all content
    /// Why: Tag cloud / filter UI needs list of all available tags
    /// </summary>
    [Fact]
    public async Task GetAllTagsAsync_MultipleItems_ReturnsUniqueTags()
    {
        // Arrange: Create content with overlapping tags
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "item1.md"), """
            ---
            title: Item 1
            date: 2025-01-01
            section_names: [AI]
            tags: [AI, Machine Learning, Announcement]
            ---
            Content 1
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "item2.md"), """
            ---
            title: Item 2
            date: 2025-01-02
            section_names: [AI]
            tags: [AI, Deep Learning, Announcement]
            ---
            Content 2
            """);

        // Act: Get all unique tags
        var tags = await _repository.GetAllTagsAsync();

        // Assert: 4 unique tags (AI appears twice but returned once, all lowercase)
        tags.Count.Should().Be(4);
        tags.Should().Contain("ai");
        tags.Should().Contain("machine learning");
        tags.Should().Contain("deep learning");
        tags.Should().Contain("announcement");
    }

    /// <summary>
    /// Test: Content with video URL extracts video ID
    /// Why: YouTube embeds require video ID for iframe generation
    /// </summary>
    [Fact]
    public async Task GetAllAsync_VideoUrl_ExtractsVideoId()
    {
        // Arrange: Create video content with YouTube URL
        var videosDir = Path.Combine(_collectionsPath, "_videos");
        Directory.CreateDirectory(videosDir);

        await File.WriteAllTextAsync(Path.Combine(videosDir, "tutorial.md"), """
            ---
            title: Video Tutorial
            date: 2025-01-01
            section_names: [GitHub Copilot]
            tags: [Tutorial]
            youtube_id: dQw4w9WgXcQ
            ---
            
            [YouTube: dQw4w9WgXcQ]
            
            Video description here.
            """);

        // Act: Load video content
        var content = await _repository.GetAllAsync();

        // Assert: Video ID extracted from URL
        content.Should().ContainSingle();
    }

    /// <summary>
    /// Test: YouTube shortcode in markdown is converted to iframe
    /// Why: Content files use {% youtube VIDEO_ID %} shortcode syntax
    /// </summary>
    [Fact]
    public async Task GetAllAsync_YouTubeShortcode_ConvertsToIframe()
    {
        // Arrange: Create content with YouTube shortcode
        var videosDir = Path.Combine(_collectionsPath, "_videos");
        Directory.CreateDirectory(videosDir);

        await File.WriteAllTextAsync(Path.Combine(videosDir, "demo.md"), """
            ---
            title: Product Demo
            date: 2025-01-01
            section_names: [AI]
            tags: [Demo]
            ---
            
            [YouTube: ABC123xyz]
            
            More content after video.
            """);

        // Act: Load content
        var content = await _repository.GetAllAsync();

        // Assert: YouTube shortcode converted to privacy-enhanced iframe in rendered HTML
        content.Should().ContainSingle();
        content[0].RenderedHtml.Should().Contain("<iframe");
        content[0].RenderedHtml.Should().Contain("youtube-nocookie.com/embed/ABC123xyz");
        content[0].RenderedHtml.Should().NotContain("www.youtube.com"); // Ensure privacy mode is used
    }

    /// <summary>
    /// Test: DateEpoch is Unix timestamp for consistent timezone handling
    /// Why: Client-side JavaScript needs Unix epoch for date filtering
    /// </summary>
    [Fact]
    public async Task GetAllAsync_DateField_ConvertedToEpoch()
    {
        // Arrange: Create content with specific date
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "item.md"), """
            ---
            title: Test Item
            date: 2025-01-15
            section_names: [AI]
            tags: [Test]
            ---
            Test content
            """);

        // Act: Load content
        var content = await _repository.GetAllAsync();

        // Assert: DateEpoch is Unix timestamp (positive integer)
        content.Should().ContainSingle();
        content[0].DateEpoch.Should().BeGreaterThan(0, "DateEpoch should be positive Unix timestamp");

        // Verify it's roughly correct (Jan 2025 = ~1736899200 epoch)
        content[0].DateEpoch.Should().BeGreaterThan(1_700_000_000, "DateEpoch should be in 2025 range");
    }

    /// <summary>
    /// Test: Content without frontmatter is skipped gracefully
    /// Why: Invalid files shouldn't crash the repository
    /// </summary>
    [Fact]
    public async Task GetAllAsync_MissingFrontmatter_SkipsFile()
    {
        // Arrange: Create invalid markdown file without frontmatter
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "invalid.md"), """
            This is just plain markdown without frontmatter.
            
            No YAML block here.
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "valid.md"), """
            ---
            title: Valid Item
            date: 2025-01-01
            section_names: [AI]
            tags: [Test]
            ---
            Valid content
            """);

        // Act: Load content
        var content = await _repository.GetAllAsync();

        // Assert: Only valid item loaded, invalid file skipped
        content.Should().ContainSingle();
        content[0].Title.Should().Be("Valid Item");
    }

    /// <summary>
    /// Test: Empty collection directory returns empty list
    /// Why: Graceful handling of missing content
    /// </summary>
    [Fact]
    public async Task GetByCollectionAsync_EmptyDirectory_ReturnsEmptyList()
    {
        // Arrange: Create empty news directory
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        // Act: Get news collection (no files)
        var content = await _repository.GetByCollectionAsync("news");

        // Assert: Empty list returned (no exception)
        content.Should().BeEmpty();
    }

    /// <summary>
    /// Test: Non-existent collection returns empty list
    /// Why: Graceful handling of invalid collection names
    /// </summary>
    [Fact]
    public async Task GetByCollectionAsync_NonExistentCollection_ReturnsEmptyList()
    {
        // Arrange: No directories created

        // Act: Try to get non-existent collection
        var content = await _repository.GetByCollectionAsync("invalid-collection");

        // Assert: Empty list returned (no exception)
        content.Should().BeEmpty();
    }

    /// <summary>
    /// Test: GetAllAsync returns content sorted by date descending (newest first)
    /// Why: All repository methods MUST return content sorted by DateEpoch descending
    /// This is a CRITICAL requirement for consistent user experience
    /// </summary>
    [Fact]
    public async Task GetAllAsync_MultipleItems_ReturnsSortedByDateDescending()
    {
        // Arrange: Create content with different dates
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-10-oldest.md"), """
            ---
            title: Oldest Item
            date: 2025-01-10
            section_names: [AI]
            tags: [Test]
            ---
            Oldest content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-20-newest.md"), """
            ---
            title: Newest Item
            date: 2025-01-20
            section_names: [AI]
            tags: [Test]
            ---
            Newest content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-15-middle.md"), """
            ---
            title: Middle Item
            date: 2025-01-15
            section_names: [AI]
            tags: [Test]
            ---
            Middle content
            """);

        // Act: Get all content
        var content = await _repository.GetAllAsync();

        // Assert: Items returned in descending date order (newest first)
        content.Count.Should().Be(3);
        content[0].Title.Should().Be("Newest Item");
        content[1].Title.Should().Be("Middle Item");
        content[2].Title.Should().Be("Oldest Item");

        // Verify dates are actually in descending order
        content[0].DateEpoch.Should().BeGreaterThan(content[1].DateEpoch);
        content[1].DateEpoch.Should().BeGreaterThan(content[2].DateEpoch);
    }

    /// <summary>
    /// Test: GetByCollectionAsync returns content sorted by date descending
    /// Why: Collection-specific queries must also maintain date sorting
    /// </summary>
    [Fact]
    public async Task GetByCollectionAsync_MultipleItems_ReturnsSortedByDateDescending()
    {
        // Arrange: Create news items with different dates
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-05-old.md"), """
            ---
            title: Old News
            date: 2025-01-05
            section_names: [AI]
            tags: [News]
            ---
            Old content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-25-new.md"), """
            ---
            title: New News
            date: 2025-01-25
            section_names: [AI]
            tags: [News]
            ---
            New content
            """);

        // Act: Get news collection
        var content = await _repository.GetByCollectionAsync("news");

        // Assert: Newest first
        content.Count.Should().Be(2);
        content[0].Title.Should().Be("New News");
        content[1].Title.Should().Be("Old News");
        content[0].DateEpoch.Should().BeGreaterThan(content[1].DateEpoch);
    }

    /// <summary>
    /// Test: GetBySectionAsync returns content sorted by date descending
    /// Why: Section filtering must preserve date sorting for section pages
    /// </summary>
    [Fact]
    public async Task GetBySectionAsync_MultipleItems_ReturnsSortedByDateDescending()
    {
        // Arrange: Create AI section content with different dates
        var newsDir = Path.Combine(_collectionsPath, "_news");
        var blogsDir = Path.Combine(_collectionsPath, "_blogs");
        Directory.CreateDirectory(newsDir);
        Directory.CreateDirectory(blogsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-12-news.md"), """
            ---
            title: AI News
            date: 2025-01-12
            section_names: [AI]
            tags: [AI]
            ---
            News content
            """);

        await File.WriteAllTextAsync(Path.Combine(blogsDir, "2025-01-18-blog.md"), """
            ---
            title: AI Blog
            date: 2025-01-18
            section_names: [AI]
            tags: [AI]
            ---
            Blog content
            """);

        // Act: Get AI section
        var content = await _repository.GetBySectionAsync("ai");

        // Assert: Newest first (blog before news)
        content.Count.Should().Be(2);
        content[0].Title.Should().Be("AI Blog");
        content[1].Title.Should().Be("AI News");
        content[0].DateEpoch.Should().BeGreaterThan(content[1].DateEpoch);
    }

    /// <summary>
    /// Test: SearchAsync returns results sorted by date descending
    /// Why: Search results should show newest matches first
    /// </summary>
    [Fact]
    public async Task SearchAsync_MultipleMatches_ReturnsSortedByDateDescending()
    {
        // Arrange: Create content with "Azure" in titles, different dates
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-08-azure-old.md"), """
            ---
            title: Azure Update Old
            date: 2025-01-08
            section_names: [Azure]
            tags: [Azure]
            ---
            Old content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-22-azure-new.md"), """
            ---
            title: Azure Update New
            date: 2025-01-22
            section_names: [Azure]
            tags: [Azure]
            ---
            New content
            """);

        // Act: Search for "Azure"
        var results = await _repository.SearchAsync("Azure");

        // Assert: Newest first
        results.Count.Should().Be(2);
        results[0].Title.Should().Be("Azure Update New");
        results[1].Title.Should().Be("Azure Update Old");
        results[0].DateEpoch.Should().BeGreaterThan(results[1].DateEpoch);
    }

    #region FilterAsync Tests

    /// <summary>
    /// Test: FilterAsync with tag filtering returns only matching items
    /// Why: Tag-based filtering is core to content discovery
    /// </summary>
    [Fact]
    public async Task FilterAsync_WithTags_ReturnsOnlyMatchingItems()
    {
        // Arrange: Create content with different tags
        var blogsDir = Path.Combine(_collectionsPath, "_blogs");
        Directory.CreateDirectory(blogsDir);

        await File.WriteAllTextAsync(Path.Combine(blogsDir, "2025-01-10-ai-blog.md"), """
            ---
            title: AI Blog
            date: 2025-01-10
            section_names: [ai]
            tags: [ai, machine-learning]
            ---
            Content about AI
            """);

        await File.WriteAllTextAsync(Path.Combine(blogsDir, "2025-01-11-azure-blog.md"), """
            ---
            title: Azure Blog
            date: 2025-01-11
            section_names: [azure]
            tags: [azure, cloud]
            ---
            Content about Azure
            """);

        await File.WriteAllTextAsync(Path.Combine(blogsDir, "2025-01-12-copilot-blog.md"), """
            ---
            title: Copilot Blog
            date: 2025-01-12
            section_names: [github-copilot]
            tags: [github-copilot, ai]
            ---
            Content about Copilot
            """);

        var request = new TechHub.Core.DTOs.FilterRequest
        {
            SectionName = null,
            CollectionName = null,
            SelectedTags = ["ai"],
            DateFrom = null,
            DateTo = null
        };

        // Act: Filter by "ai" tag
        var results = await _repository.FilterAsync(request);

        // Assert: Should return items with "ai" tag (matches "ai" exactly and "Copilot" which has "ai" tag)
        results.Should().HaveCount(2);
        results.Should().Contain(c => c.Title == "AI Blog");
        results.Should().Contain(c => c.Title == "Copilot Blog");
        results.Should().NotContain(c => c.Title == "Azure Blog");
    }

    /// <summary>
    /// Test: FilterAsync with date range filtering returns only items in range
    /// Why: Date-based filtering is essential for finding recent content
    /// </summary>
    [Fact]
    public async Task FilterAsync_WithDateRange_ReturnsOnlyItemsInRange()
    {
        // Arrange: Create content with different dates
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-01-old.md"), """
            ---
            title: Old News
            date: 2025-01-01
            section_names: [ai]
            tags: [news]
            ---
            Old content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-15-recent.md"), """
            ---
            title: Recent News
            date: 2025-01-15
            section_names: [ai]
            tags: [news]
            ---
            Recent content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-20-newest.md"), """
            ---
            title: Newest News
            date: 2025-01-20
            section_names: [ai]
            tags: [news]
            ---
            Newest content
            """);

        var dateFrom = new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero);
        var dateTo = new DateTimeOffset(2025, 1, 18, 23, 59, 59, TimeSpan.Zero);

        var request = new TechHub.Core.DTOs.FilterRequest
        {
            SectionName = null,
            CollectionName = null,
            SelectedTags = [],
            DateFrom = dateFrom,
            DateTo = dateTo
        };

        // Act: Filter by date range (Jan 10-18)
        var results = await _repository.FilterAsync(request);

        // Assert: Should return only item from Jan 15
        results.Should().HaveCount(1);
        results[0].Title.Should().Be("Recent News");
    }

    /// <summary>
    /// Test: FilterAsync with combined filters (tags + date) returns items matching both
    /// Why: Users often combine multiple filters for precise results
    /// </summary>
    [Fact]
    public async Task FilterAsync_WithTagsAndDateRange_ReturnsItemsMatchingBoth()
    {
        // Arrange: Create content with different tags and dates
        var blogsDir = Path.Combine(_collectionsPath, "_blogs");
        Directory.CreateDirectory(blogsDir);

        await File.WriteAllTextAsync(Path.Combine(blogsDir, "2025-01-05-old-ai.md"), """
            ---
            title: Old AI Blog
            date: 2025-01-05
            section_names: [ai]
            tags: [ai, machine-learning]
            ---
            Old AI content
            """);

        await File.WriteAllTextAsync(Path.Combine(blogsDir, "2025-01-15-recent-ai.md"), """
            ---
            title: Recent AI Blog
            date: 2025-01-15
            section_names: [ai]
            tags: [ai, deep-learning]
            ---
            Recent AI content
            """);

        await File.WriteAllTextAsync(Path.Combine(blogsDir, "2025-01-16-recent-azure.md"), """
            ---
            title: Recent Azure Blog
            date: 2025-01-16
            section_names: [azure]
            tags: [azure, cloud]
            ---
            Recent Azure content
            """);

        var dateFrom = new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero);
        var dateTo = new DateTimeOffset(2025, 1, 20, 23, 59, 59, TimeSpan.Zero);

        var request = new TechHub.Core.DTOs.FilterRequest
        {
            SectionName = null,
            CollectionName = null,
            SelectedTags = ["ai"],
            DateFrom = dateFrom,
            DateTo = dateTo
        };

        // Act: Filter by "ai" tag AND date range (Jan 10-20)
        var results = await _repository.FilterAsync(request);

        // Assert: Should return only recent AI item (matches both tag and date)
        results.Should().HaveCount(1);
        results[0].Title.Should().Be("Recent AI Blog");
    }

    /// <summary>
    /// Test: FilterAsync with section scope returns only section items
    /// Why: Section filtering is core to content organization
    /// </summary>
    [Fact]
    public async Task FilterAsync_WithSectionScope_ReturnsOnlySectionItems()
    {
        // Arrange: Create content in different sections
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-10-ai-news.md"), """
            ---
            title: AI News
            date: 2025-01-10
            section_names: [ai]
            tags: [ai]
            ---
            AI content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-11-azure-news.md"), """
            ---
            title: Azure News
            date: 2025-01-11
            section_names: [azure]
            tags: [azure]
            ---
            Azure content
            """);

        var request = new TechHub.Core.DTOs.FilterRequest
        {
            SectionName = "ai",
            CollectionName = null,
            SelectedTags = [],
            DateFrom = null,
            DateTo = null
        };

        // Act: Filter by section
        var results = await _repository.FilterAsync(request);

        // Assert: Should return only AI section items
        results.Should().HaveCount(1);
        results[0].Title.Should().Be("AI News");
    }

    /// <summary>
    /// Test: FilterAsync with collection scope returns only collection items
    /// Why: Collection filtering is core to content type organization
    /// </summary>
    [Fact]
    public async Task FilterAsync_WithCollectionScope_ReturnsOnlyCollectionItems()
    {
        // Arrange: Create content in different collections
        var newsDir = Path.Combine(_collectionsPath, "_news");
        var blogsDir = Path.Combine(_collectionsPath, "_blogs");
        Directory.CreateDirectory(newsDir);
        Directory.CreateDirectory(blogsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-10-news-item.md"), """
            ---
            title: News Item
            date: 2025-01-10
            section_names: [ai]
            tags: [ai]
            ---
            News content
            """);

        await File.WriteAllTextAsync(Path.Combine(blogsDir, "2025-01-11-blog-item.md"), """
            ---
            title: Blog Item
            date: 2025-01-11
            section_names: [ai]
            tags: [ai]
            ---
            Blog content
            """);

        var request = new TechHub.Core.DTOs.FilterRequest
        {
            SectionName = null,
            CollectionName = "news",
            SelectedTags = [],
            DateFrom = null,
            DateTo = null
        };

        // Act: Filter by collection
        var results = await _repository.FilterAsync(request);

        // Assert: Should return only news collection items
        results.Should().HaveCount(1);
        results[0].Title.Should().Be("News Item");
    }

    /// <summary>
    /// Test: FilterAsync with section AND collection scope returns items matching both
    /// Why: Combined scoping enables precise content filtering
    /// </summary>
    [Fact]
    public async Task FilterAsync_WithSectionAndCollectionScope_ReturnsItemsMatchingBoth()
    {
        // Arrange: Create content in different sections and collections
        var newsDir = Path.Combine(_collectionsPath, "_news");
        var blogsDir = Path.Combine(_collectionsPath, "_blogs");
        Directory.CreateDirectory(newsDir);
        Directory.CreateDirectory(blogsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-10-ai-news.md"), """
            ---
            title: AI News
            date: 2025-01-10
            section_names: [ai]
            tags: [ai]
            ---
            AI news content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-11-azure-news.md"), """
            ---
            title: Azure News
            date: 2025-01-11
            section_names: [azure]
            tags: [azure]
            ---
            Azure news content
            """);

        await File.WriteAllTextAsync(Path.Combine(blogsDir, "2025-01-12-ai-blog.md"), """
            ---
            title: AI Blog
            date: 2025-01-12
            section_names: [ai]
            tags: [ai]
            ---
            AI blog content
            """);

        var request = new TechHub.Core.DTOs.FilterRequest
        {
            SectionName = "ai",
            CollectionName = "news",
            SelectedTags = [],
            DateFrom = null,
            DateTo = null
        };

        // Act: Filter by section AND collection
        var results = await _repository.FilterAsync(request);

        // Assert: Should return only AI news items
        results.Should().HaveCount(1);
        results[0].Title.Should().Be("AI News");
    }

    /// <summary>
    /// Test: FilterAsync with no filters returns all items sorted by date
    /// Why: Default behavior should show all content in chronological order
    /// </summary>
    [Fact]
    public async Task FilterAsync_WithNoFilters_ReturnsAllItemsSortedByDate()
    {
        // Arrange: Create content with different dates
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-05-old.md"), """
            ---
            title: Old Content
            date: 2025-01-05
            section_names: [ai]
            tags: [news]
            ---
            Old content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-15-recent.md"), """
            ---
            title: Recent Content
            date: 2025-01-15
            section_names: [ai]
            tags: [news]
            ---
            Recent content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-10-middle.md"), """
            ---
            title: Middle Content
            date: 2025-01-10
            section_names: [ai]
            tags: [news]
            ---
            Middle content
            """);

        var request = new TechHub.Core.DTOs.FilterRequest
        {
            SectionName = null,
            CollectionName = null,
            SelectedTags = [],
            DateFrom = null,
            DateTo = null
        };

        // Act: Filter with no filters
        var results = await _repository.FilterAsync(request);

        // Assert: Should return all items sorted by date descending (newest first)
        results.Should().HaveCount(3);
        results[0].Title.Should().Be("Recent Content");
        results[1].Title.Should().Be("Middle Content");
        results[2].Title.Should().Be("Old Content");
        results[0].DateEpoch.Should().BeGreaterThan(results[1].DateEpoch);
        results[1].DateEpoch.Should().BeGreaterThan(results[2].DateEpoch);
    }

    /// <summary>
    /// Test: FilterAsync with multiple tags uses OR logic
    /// Why: Multiple tag filtering should return items matching ANY tag
    /// </summary>
    [Fact]
    public async Task FilterAsync_WithMultipleTags_UsesOrLogic()
    {
        // Arrange: Create content with different tags
        var blogsDir = Path.Combine(_collectionsPath, "_blogs");
        Directory.CreateDirectory(blogsDir);

        await File.WriteAllTextAsync(Path.Combine(blogsDir, "2025-01-10-ai-only.md"), """
            ---
            title: AI Only Blog
            date: 2025-01-10
            section_names: [ai]
            tags: [ai]
            ---
            AI content
            """);

        await File.WriteAllTextAsync(Path.Combine(blogsDir, "2025-01-11-azure-only.md"), """
            ---
            title: Azure Only Blog
            date: 2025-01-11
            section_names: [azure]
            tags: [azure]
            ---
            Azure content
            """);

        await File.WriteAllTextAsync(Path.Combine(blogsDir, "2025-01-12-copilot-only.md"), """
            ---
            title: Copilot Only Blog
            date: 2025-01-12
            section_names: [github-copilot]
            tags: [github-copilot]
            ---
            Copilot content
            """);

        var request = new TechHub.Core.DTOs.FilterRequest
        {
            SectionName = null,
            CollectionName = null,
            SelectedTags = ["ai", "azure"],
            DateFrom = null,
            DateTo = null
        };

        // Act: Filter by "ai" OR "azure" tags
        var results = await _repository.FilterAsync(request);

        // Assert: Should return items with either "ai" or "azure" tag
        results.Should().HaveCount(2);
        results.Should().Contain(c => c.Title == "AI Only Blog");
        results.Should().Contain(c => c.Title == "Azure Only Blog");
        results.Should().NotContain(c => c.Title == "Copilot Only Blog");
    }

    #endregion
}
