using FluentAssertions;
using Moq;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Unit tests for ArticleContentService.
/// Tests verify YouTube transcript enrichment and HTML content extraction.
/// </summary>
public class ArticleContentServiceTests
{
    private readonly Mock<IYouTubeTranscriptService> _mockTranscript;
    private readonly Mock<IArticleFetchClient> _mockFetchClient;
    private readonly IArticleContentService _service;

    public ArticleContentServiceTests()
    {
        _mockTranscript = new Mock<IYouTubeTranscriptService>();
        _mockFetchClient = new Mock<IArticleFetchClient>();

        _service = new Infrastructure.Services.ContentProcessing.ArticleContentService(
            _mockFetchClient.Object,
            _mockTranscript.Object);
    }

    private static RawFeedItem CreateYouTubeItem(string url = "https://www.youtube.com/watch?v=abc123") => new()
    {
        Title = "Test Video",
        ExternalUrl = url,
        PublishedAt = DateTimeOffset.UtcNow,
        FeedItemData = "A test video description",
        FeedName = "Test Feed",
        CollectionName = "videos"
    };

    private static RawFeedItem CreateNonYouTubeItem() => new()
    {
        Title = "Test Article",
        ExternalUrl = "https://example.com/article",
        PublishedAt = DateTimeOffset.UtcNow,
        FeedItemData = "A test article",
        FeedName = "Test Feed",
        CollectionName = "blogs"
    };

    [Fact]
    public async Task EnrichWithContentAsync_YouTubeItem_WithTranscript_SetsFullContent()
    {
        // Arrange
        var item = CreateYouTubeItem();
        _mockTranscript
            .Setup(t => t.GetTranscriptAsync(item.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(TranscriptResult.Success("This is the transcript text from the video."));

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        result.FullContent.Should().Be("This is the transcript text from the video.");
        result.Title.Should().Be(item.Title);
        result.ExternalUrl.Should().Be(item.ExternalUrl);
        result.FeedItemData.Should().Be(item.FeedItemData);
        result.TranscriptFailureReason.Should().BeNull();
    }

    [Fact]
    public async Task EnrichWithContentAsync_YouTubeItem_WithoutTranscript_ReturnsOriginalItem()
    {
        // Arrange
        var item = CreateYouTubeItem();
        _mockTranscript
            .Setup(t => t.GetTranscriptAsync(item.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(TranscriptResult.Failure("No caption tracks available"));

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        result.FullContent.Should().BeNull();
        result.TranscriptFailureReason.Should().Be("No caption tracks available");
    }

    [Fact]
    public async Task EnrichWithContentAsync_YouTubeItem_CallsTranscriptService()
    {
        // Arrange
        var item = CreateYouTubeItem();
        _mockTranscript
            .Setup(t => t.GetTranscriptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TranscriptResult.Success("transcript"));

        // Act
        await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        _mockTranscript.Verify(t => t.GetTranscriptAsync(item.ExternalUrl, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task EnrichWithContentAsync_NonYouTubeItem_DoesNotCallTranscriptService()
    {
        // Arrange
        var item = CreateNonYouTubeItem();

        // Act - will fail because no HTTP handler, but we only care about transcript not being called
        await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        _mockTranscript.Verify(t => t.GetTranscriptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task EnrichWithContentAsync_YouTubeItem_PreservesAllFields()
    {
        // Arrange
        var item = new RawFeedItem
        {
            Title = "Copilot Features",
            ExternalUrl = "https://www.youtube.com/watch?v=xyz789",
            PublishedAt = new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero),
            FeedItemData = "Video about Copilot",
            FeedLevelAuthor = "John Doe",
            FeedTags = ["AI", "Copilot"],
            FeedName = "GitHub Blog",
            CollectionName = "videos"
        };
        _mockTranscript
            .Setup(t => t.GetTranscriptAsync(item.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(TranscriptResult.Success("Full transcript here."));

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        result.Title.Should().Be("Copilot Features");
        result.ExternalUrl.Should().Be("https://www.youtube.com/watch?v=xyz789");
        result.PublishedAt.Should().Be(item.PublishedAt);
        result.FeedItemData.Should().Be("Video about Copilot");
        result.FeedLevelAuthor.Should().Be("John Doe");
        result.FeedTags.Should().BeEquivalentTo(new[] { "AI", "Copilot" });
        result.FeedName.Should().Be("GitHub Blog");
        result.CollectionName.Should().Be("videos");
        result.FullContent.Should().Be("Full transcript here.");
    }

    [Fact]
    public async Task EnrichWithContentAsync_YouTubeShortUrl_IsRecognizedAsYouTube()
    {
        // Arrange - youtu.be short URL
        var item = CreateYouTubeItem("https://youtu.be/abc123");
        _mockTranscript
            .Setup(t => t.GetTranscriptAsync(item.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(TranscriptResult.Success("Short url transcript"));

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        result.FullContent.Should().Be("Short url transcript");
        _mockTranscript.Verify(t => t.GetTranscriptAsync(item.ExternalUrl, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task EnrichWithContentAsync_YouTubeEmptyTranscript_ReturnsOriginalItem()
    {
        // Arrange
        var item = CreateYouTubeItem();
        _mockTranscript
            .Setup(t => t.GetTranscriptAsync(item.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(TranscriptResult.Failure("Caption track was empty"));

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        result.FullContent.Should().BeNull();
        result.TranscriptFailureReason.Should().Be("Caption track was empty");
    }

    // ── HTML Content Extraction Tests ────────────────────────────────────────

    [Fact]
    public async Task EnrichWithContentAsync_NonYouTubeItem_WithArticleTag_ExtractsArticleContent()
    {
        // Arrange
        var item = CreateNonYouTubeItem();
        var html = """
            <html><head><title>Page</title></head><body>
            <nav>Navigation links here</nav>
            <article class="main-article">
                <h1>Article Title</h1>
                <p>This is the article content with important information.</p>
            </article>
            <footer>Footer content</footer>
            </body></html>
            """;
        _mockFetchClient
            .Setup(c => c.FetchHtmlAsync(item.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(html);

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        result.FullContent.Should().Contain("Article Title");
        result.FullContent.Should().Contain("important information");
        result.FullContent.Should().NotContain("Navigation links");
        result.FullContent.Should().NotContain("Footer content");
    }

    [Fact]
    public async Task EnrichWithContentAsync_NonYouTubeItem_WithMainTag_ExtractsMainContent()
    {
        // Arrange
        var item = CreateNonYouTubeItem();
        var html = """
            <html><body>
            <header>Header</header>
            <main>
                <h1>Main Content</h1>
                <p>Important text inside main tag.</p>
            </main>
            <footer>Footer</footer>
            </body></html>
            """;
        _mockFetchClient
            .Setup(c => c.FetchHtmlAsync(item.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(html);

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        result.FullContent.Should().Contain("Main Content");
        result.FullContent.Should().Contain("Important text");
        result.FullContent.Should().NotContain("Header");
        result.FullContent.Should().NotContain("Footer");
    }

    [Fact]
    public async Task EnrichWithContentAsync_NonYouTubeItem_StripsHtmlTags()
    {
        // Arrange
        var item = CreateNonYouTubeItem();
        var html = """
            <html><body>
            <article>
                <h1>Title</h1>
                <p>Some <strong>bold</strong> and <em>italic</em> text.</p>
                <ul><li>Item 1</li><li>Item 2</li></ul>
            </article>
            </body></html>
            """;
        _mockFetchClient
            .Setup(c => c.FetchHtmlAsync(item.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(html);

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        result.FullContent.Should().NotContain("<");
        result.FullContent.Should().NotContain(">");
        result.FullContent.Should().Contain("bold");
        result.FullContent.Should().Contain("italic");
    }

    [Fact]
    public async Task EnrichWithContentAsync_NonYouTubeItem_RemovesScriptAndStyle()
    {
        // Arrange
        var item = CreateNonYouTubeItem();
        var html = """
            <html><body>
            <article>
                <h1>Title</h1>
                <script>alert('xss');</script>
                <style>.hidden { display: none; }</style>
                <p>Actual content here.</p>
            </article>
            </body></html>
            """;
        _mockFetchClient
            .Setup(c => c.FetchHtmlAsync(item.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(html);

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        result.FullContent.Should().Contain("Actual content here");
        result.FullContent.Should().NotContain("alert");
        result.FullContent.Should().NotContain("display: none");
    }

    [Fact]
    public async Task EnrichWithContentAsync_NonYouTubeItem_LazyLoadedImages_StripsCleanly()
    {
        // Arrange — TechCommunity-style lazy-loaded images use data-src instead of src
        var item = CreateNonYouTubeItem();
        var html = """
            <html><body>
            <article>
                <h1>GitHub Copilot CLI</h1>
                <p>Content about Copilot CLI features.</p>
                <div class="lia-image">
                    <img src="" data-src="https://cdn.example.com/image1.png" alt="">
                </div>
                <p>More content after image.</p>
                <span class="lia-inline-image">
                    <img data-li-src="https://cdn.example.com/image2.png" alt="" src="data:image/gif;base64,PLACEHOLDER">
                </span>
                <p>Final paragraph.</p>
            </article>
            </body></html>
            """;
        _mockFetchClient
            .Setup(c => c.FetchHtmlAsync(item.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(html);

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert — images should be stripped without leaving empty markdown references
        result.FullContent.Should().Contain("GitHub Copilot CLI");
        result.FullContent.Should().Contain("Content about Copilot CLI features");
        result.FullContent.Should().Contain("More content after image");
        result.FullContent.Should().Contain("Final paragraph");
        result.FullContent.Should().NotContain("![");
        result.FullContent.Should().NotContain("data-src");
        result.FullContent.Should().NotContain("<img");
        result.FullContent.Should().NotContain("PLACEHOLDER");
    }

    [Fact]
    public async Task EnrichWithContentAsync_NonYouTubeItem_NestedArticleTags_ExtractsOuterContent()
    {
        // Arrange — some sites have nested <article> elements
        var item = CreateNonYouTubeItem();
        var html = """
            <html><body>
            <article class="post">
                <h1>Outer Article</h1>
                <article class="embedded">
                    <p>Embedded content</p>
                </article>
                <p>Outer content continues here with important details.</p>
            </article>
            </body></html>
            """;
        _mockFetchClient
            .Setup(c => c.FetchHtmlAsync(item.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(html);

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert — should capture all content from the outer article,
        // not stop at the first </article> (nested close)
        result.FullContent.Should().Contain("Outer Article");
        result.FullContent.Should().Contain("Embedded content");
        result.FullContent.Should().Contain("Outer content continues here");
    }

    [Fact]
    public async Task EnrichWithContentAsync_NonYouTubeItem_FetchReturnsNull_ReturnsOriginal()
    {
        // Arrange
        var item = CreateNonYouTubeItem();
        _mockFetchClient
            .Setup(c => c.FetchHtmlAsync(item.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(item);
    }

    [Fact]
    public async Task EnrichWithContentAsync_NonYouTubeItem_EmptyExternalUrl_ReturnsOriginal()
    {
        // Arrange
        var item = new RawFeedItem
        {
            Title = "Test",
            ExternalUrl = "",
            PublishedAt = DateTimeOffset.UtcNow,
            FeedItemData = "Desc",
            FeedName = "Feed",
            CollectionName = "blogs"
        };

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(item);
        _mockFetchClient.Verify(c => c.FetchHtmlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task EnrichWithContentAsync_NonYouTubeItem_PreservesAllFields()
    {
        // Arrange
        var item = new RawFeedItem
        {
            Title = "Article Title",
            ExternalUrl = "https://example.com/article",
            PublishedAt = new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero),
            FeedItemData = "Article description",
            FeedLevelAuthor = "Jane Doe",
            FeedTags = ["Azure", "DevOps"],
            FeedName = "Tech Blog",
            CollectionName = "blogs"
        };
        _mockFetchClient
            .Setup(c => c.FetchHtmlAsync(item.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync("<article><p>Content</p></article>");

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        result.Title.Should().Be("Article Title");
        result.ExternalUrl.Should().Be("https://example.com/article");
        result.PublishedAt.Should().Be(item.PublishedAt);
        result.FeedItemData.Should().Be("Article description");
        result.FeedLevelAuthor.Should().Be("Jane Doe");
        result.FeedTags.Should().BeEquivalentTo(new[] { "Azure", "DevOps" });
        result.FeedName.Should().Be("Tech Blog");
        result.CollectionName.Should().Be("blogs");
        result.FullContent.Should().Be("Content");
    }

    [Fact]
    public async Task EnrichWithContentAsync_NonYouTubeItem_LargeContent_TruncatesAt50K()
    {
        // Arrange
        var item = CreateNonYouTubeItem();
        var largeContent = new string('A', 60_000);
        var html = $"<article>{largeContent}</article>";
        _mockFetchClient
            .Setup(c => c.FetchHtmlAsync(item.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(html);

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        result.FullContent!.Length.Should().BeLessThanOrEqualTo(50_000);
    }

    [Fact]
    public async Task EnrichWithContentAsync_NonYouTubeItem_NoArticleOrMainTag_UsesFullHtml()
    {
        // Arrange — fallback to full HTML when no article/main tags found
        var item = CreateNonYouTubeItem();
        var html = """
            <html><body>
            <div class="content">
                <h1>Page Title</h1>
                <p>Body content without semantic tags.</p>
            </div>
            </body></html>
            """;
        _mockFetchClient
            .Setup(c => c.FetchHtmlAsync(item.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(html);

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        result.FullContent.Should().Contain("Page Title");
        result.FullContent.Should().Contain("Body content without semantic tags");
    }

    [Fact]
    public async Task EnrichWithContentAsync_NonYouTubeItem_CollapsesWhitespace()
    {
        // Arrange
        var item = CreateNonYouTubeItem();
        var html = """
            <article>
                <p>Word1</p>


                <p>Word2</p>
            </article>
            """;
        _mockFetchClient
            .Setup(c => c.FetchHtmlAsync(item.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(html);

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert — multiple whitespace should be collapsed
        result.FullContent.Should().NotContainAll("  ");
    }
}
