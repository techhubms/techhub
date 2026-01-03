using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Infrastructure.Repositories;
using TechHub.Infrastructure.Services;
using Xunit;

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
                SectionsConfigPath = Path.Combine(_tempDirectory, "sections.json"),
                Timezone = "Europe/Brussels"
            },
            Caching = new CachingSettings(),
            Seo = new SeoSettings
            {
                BaseUrl = "https://test.local",
                SiteTitle = "Test Site",
                SiteDescription = "Test Description"
            },
            Performance = new PerformanceSettings()
        };

        // Setup: Create dependencies
        _markdownService = new MarkdownService();
        _repository = new FileBasedContentRepository(
            Options.Create(_settings),
            _markdownService
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
            categories: [ai]
            tags: [announcement, ai]
            excerpt: Major AI product announcement
            ---
            
            Full details about the AI product launch.
            """);

        var videoFile = Path.Combine(videosDir, "2025-01-10-tutorial.md");
        await File.WriteAllTextAsync(videoFile, """
            ---
            title: Getting Started Tutorial
            date: 2025-01-10
            author: Educator
            categories: [github-copilot]
            tags: [tutorial, beginner]
            excerpt: Learn the basics
            videoUrl: https://www.youtube.com/watch?v=ABC123
            ---
            
            This tutorial covers the fundamentals.
            """);

        // Act: Load all content
        var content = await _repository.GetAllAsync();

        // Assert: Both items loaded from different collections
        Assert.Equal(2, content.Count);
        Assert.Contains(content, c => c.Title == "AI Product Launch");
        Assert.Contains(content, c => c.Title == "Getting Started Tutorial");
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
            categories: [ai]
            tags: [news]
            excerpt: News excerpt
            ---
            News content
            """);

        await File.WriteAllTextAsync(Path.Combine(blogsDir, "blog1.md"), """
            ---
            title: Blog Post 1
            date: 2025-01-02
            categories: [ai]
            tags: [blog]
            excerpt: Blog excerpt
            ---
            Blog content
            """);

        // Act: Get only news collection
        var newsContent = await _repository.GetByCollectionAsync("news");

        // Assert: Only news items returned, not blogs
        Assert.Single(newsContent);
        Assert.Equal("News Item 1", newsContent[0].Title);
    }

    /// <summary>
    /// Test: GetByCategoryAsync filters content by category
    /// Why: Sections display content filtered by category (ai, github-copilot, etc.)
    /// </summary>
    [Fact]
    public async Task GetByCategoryAsync_SpecificCategory_ReturnsMatchingContent()
    {
        // Arrange: Create content with different categories
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "ai-news.md"), """
            ---
            title: AI News
            date: 2025-01-01
            categories: [ai]
            tags: [ai]
            excerpt: AI news excerpt
            ---
            AI content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "ghc-news.md"), """
            ---
            title: GitHub Copilot News
            date: 2025-01-02
            categories: [github-copilot]
            tags: [copilot]
            excerpt: Copilot news excerpt
            ---
            Copilot content
            """);

        // Act: Get AI category content
        var aiContent = await _repository.GetByCategoryAsync("ai");

        // Assert: Only AI category items returned
        Assert.Single(aiContent);
        Assert.Equal("AI News", aiContent[0].Title);
        Assert.Contains("ai", aiContent[0].Categories);
    }

    /// <summary>
    /// Test: GetByIdAsync retrieves single content item by ID
    /// Why: Display individual content detail pages by URL slug
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_ExistingItem_ReturnsItem()
    {
        // Arrange: Create news item with specific ID
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-15-product-launch.md"), """
            ---
            title: Product Launch
            date: 2025-01-15
            categories: [ai]
            tags: [announcement]
            excerpt: Exciting product launch
            ---
            
            Full product launch details here.
            """);

        // Act: Get item by ID (filename without extension)
        var item = await _repository.GetByIdAsync("news", "2025-01-15-product-launch");

        // Assert: Correct item returned with all properties
        Assert.NotNull(item);
        Assert.Equal("Product Launch", item.Title);
        Assert.Equal("2025-01-15-product-launch", item.Id);
        Assert.Equal("news", item.Collection);
        Assert.Contains("Full product launch details", item.RenderedHtml);
    }

    /// <summary>
    /// Test: GetByIdAsync returns null for non-existent item
    /// Why: Graceful handling of missing content (404 pages)
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_NonExistentItem_ReturnsNull()
    {
        // Arrange: Create empty news directory
        var newsDir = Path.Combine(_collectionsPath, "_news");
        Directory.CreateDirectory(newsDir);

        // Act: Try to get non-existent item
        var item = await _repository.GetByIdAsync("news", "missing-item");

        // Assert: Null returned (no exception thrown)
        Assert.Null(item);
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
            categories: [azure]
            tags: [azure, ai]
            excerpt: Azure update
            ---
            Azure content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "ghc-release.md"), """
            ---
            title: GitHub Copilot Release
            date: 2025-01-02
            categories: [github-copilot]
            tags: [copilot]
            excerpt: Copilot release
            ---
            Copilot content
            """);

        // Act: Search for "Azure"
        var results = await _repository.SearchAsync("Azure");

        // Assert: Only Azure-related content returned
        Assert.Single(results);
        Assert.Equal("Azure OpenAI Service Update", results[0].Title);
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
            categories: [security]
            tags: [security, urgent]
            excerpt: Security update
            ---
            Security content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "item2.md"), """
            ---
            title: Feature Release
            date: 2025-01-02
            categories: [ai]
            tags: [feature, ai]
            excerpt: New feature
            ---
            Feature content
            """);

        // Act: Search by tag "security"
        var results = await _repository.SearchAsync("security");

        // Assert: Tag match returns correct item
        Assert.Single(results);
        Assert.Equal("Security Alert", results[0].Title);
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
            title: .NET 10 Release
            date: 2025-01-01
            categories: [coding]
            tags: [dotnet]
            excerpt: .NET release
            ---
            .NET content
            """);

        // Act: Search with different casing
        var lowerResults = await _repository.SearchAsync("dotnet");
        var upperResults = await _repository.SearchAsync("DOTNET");
        var mixedResults = await _repository.SearchAsync("DoTnEt");

        // Assert: All variations return same result
        Assert.Single(lowerResults);
        Assert.Single(upperResults);
        Assert.Single(mixedResults);
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
            categories: [ai]
            tags: [ai, machine-learning, announcement]
            excerpt: Item 1 excerpt
            ---
            Content 1
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "item2.md"), """
            ---
            title: Item 2
            date: 2025-01-02
            categories: [ai]
            tags: [ai, deep-learning, announcement]
            excerpt: Item 2 excerpt
            ---
            Content 2
            """);

        // Act: Get all unique tags
        var tags = await _repository.GetAllTagsAsync();

        // Assert: 4 unique tags (ai appears twice but returned once)
        Assert.Equal(4, tags.Count);
        Assert.Contains("ai", tags);
        Assert.Contains("machine-learning", tags);
        Assert.Contains("deep-learning", tags);
        Assert.Contains("announcement", tags);
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
            categories: [github-copilot]
            tags: [tutorial]
            excerpt: Watch this tutorial
            video_url: https://www.youtube.com/watch?v=dQw4w9WgXcQ
            ---
            
            [YouTube: dQw4w9WgXcQ]
            
            Video description here.
            """);

        // Act: Load video content
        var content = await _repository.GetAllAsync();

        // Assert: Video ID extracted from URL
        Assert.Single(content);
        Assert.Equal("dQw4w9WgXcQ", content[0].VideoId);
    }

    /// <summary>
    /// Test: YouTube shortcode in markdown is converted to iframe
    /// Why: Jekyll's {% youtube %} tag needs .NET equivalent
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
            categories: [ai]
            tags: [demo]
            excerpt: Watch the demo
            ---
            
            [YouTube: ABC123xyz]
            
            More content after video.
            """);

        // Act: Load content
        var content = await _repository.GetAllAsync();

        // Assert: YouTube shortcode converted to iframe in rendered HTML
        Assert.Single(content);
        Assert.Contains("<iframe", content[0].RenderedHtml);
        Assert.Contains("youtube.com/embed/ABC123xyz", content[0].RenderedHtml);
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
            categories: [ai]
            tags: [test]
            excerpt: Test excerpt
            ---
            Test content
            """);

        // Act: Load content
        var content = await _repository.GetAllAsync();

        // Assert: DateEpoch is Unix timestamp (positive integer)
        Assert.Single(content);
        Assert.True(content[0].DateEpoch > 0, "DateEpoch should be positive Unix timestamp");
        
        // Verify it's roughly correct (Jan 2025 = ~1736899200 epoch)
        Assert.True(content[0].DateEpoch > 1_700_000_000, "DateEpoch should be in 2025 range");
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
            categories: [ai]
            tags: [test]
            excerpt: Valid excerpt
            ---
            Valid content
            """);

        // Act: Load content
        var content = await _repository.GetAllAsync();

        // Assert: Only valid item loaded, invalid file skipped
        Assert.Single(content);
        Assert.Equal("Valid Item", content[0].Title);
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
        Assert.Empty(content);
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
        Assert.Empty(content);
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
            categories: [ai]
            tags: [test]
            excerpt: Oldest excerpt
            ---
            Oldest content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-20-newest.md"), """
            ---
            title: Newest Item
            date: 2025-01-20
            categories: [ai]
            tags: [test]
            excerpt: Newest excerpt
            ---
            Newest content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-15-middle.md"), """
            ---
            title: Middle Item
            date: 2025-01-15
            categories: [ai]
            tags: [test]
            excerpt: Middle excerpt
            ---
            Middle content
            """);

        // Act: Get all content
        var content = await _repository.GetAllAsync();

        // Assert: Items returned in descending date order (newest first)
        Assert.Equal(3, content.Count);
        Assert.Equal("Newest Item", content[0].Title);
        Assert.Equal("Middle Item", content[1].Title);
        Assert.Equal("Oldest Item", content[2].Title);
        
        // Verify dates are actually in descending order
        Assert.True(content[0].DateEpoch > content[1].DateEpoch);
        Assert.True(content[1].DateEpoch > content[2].DateEpoch);
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
            categories: [ai]
            tags: [news]
            excerpt: Old news
            ---
            Old content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-25-new.md"), """
            ---
            title: New News
            date: 2025-01-25
            categories: [ai]
            tags: [news]
            excerpt: New news
            ---
            New content
            """);

        // Act: Get news collection
        var content = await _repository.GetByCollectionAsync("news");

        // Assert: Newest first
        Assert.Equal(2, content.Count);
        Assert.Equal("New News", content[0].Title);
        Assert.Equal("Old News", content[1].Title);
        Assert.True(content[0].DateEpoch > content[1].DateEpoch);
    }

    /// <summary>
    /// Test: GetByCategoryAsync returns content sorted by date descending
    /// Why: Category filtering must preserve date sorting for section pages
    /// </summary>
    [Fact]
    public async Task GetByCategoryAsync_MultipleItems_ReturnsSortedByDateDescending()
    {
        // Arrange: Create AI category content with different dates
        var newsDir = Path.Combine(_collectionsPath, "_news");
        var blogsDir = Path.Combine(_collectionsPath, "_blogs");
        Directory.CreateDirectory(newsDir);
        Directory.CreateDirectory(blogsDir);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-12-news.md"), """
            ---
            title: AI News
            date: 2025-01-12
            categories: [ai]
            tags: [ai]
            excerpt: AI news
            ---
            News content
            """);

        await File.WriteAllTextAsync(Path.Combine(blogsDir, "2025-01-18-blog.md"), """
            ---
            title: AI Blog
            date: 2025-01-18
            categories: [ai]
            tags: [ai]
            excerpt: AI blog
            ---
            Blog content
            """);

        // Act: Get AI category
        var content = await _repository.GetByCategoryAsync("ai");

        // Assert: Newest first (blog before news)
        Assert.Equal(2, content.Count);
        Assert.Equal("AI Blog", content[0].Title);
        Assert.Equal("AI News", content[1].Title);
        Assert.True(content[0].DateEpoch > content[1].DateEpoch);
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
            categories: [azure]
            tags: [azure]
            excerpt: Old update
            ---
            Old content
            """);

        await File.WriteAllTextAsync(Path.Combine(newsDir, "2025-01-22-azure-new.md"), """
            ---
            title: Azure Update New
            date: 2025-01-22
            categories: [azure]
            tags: [azure]
            excerpt: New update
            ---
            New content
            """);

        // Act: Search for "Azure"
        var results = await _repository.SearchAsync("Azure");

        // Assert: Newest first
        Assert.Equal(2, results.Count);
        Assert.Equal("Azure Update New", results[0].Title);
        Assert.Equal("Azure Update Old", results[1].Title);
        Assert.True(results[0].DateEpoch > results[1].DateEpoch);
    }
}
