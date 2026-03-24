using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Configuration;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Unit tests for ArticleContentService YouTube transcript enrichment.
/// Tests verify that YouTube items are correctly enriched with transcript text
/// via the YouTubeTranscriptService dependency.
/// </summary>
public class ArticleContentServiceTests : IDisposable
{
    private readonly Mock<YouTubeTranscriptService> _mockTranscript;
    private readonly ArticleContentService _service;
    private bool _disposed;

    public ArticleContentServiceTests()
    {
        var httpClient = new HttpClient();
        var options = Options.Create(new ContentProcessorOptions { RequestTimeoutSeconds = 10 });
        _mockTranscript = new Mock<YouTubeTranscriptService>(
            Options.Create(new ContentProcessorOptions { RequestTimeoutSeconds = 10 }),
            NullLogger<YouTubeTranscriptService>.Instance);

        _service = new ArticleContentService(
            httpClient,
            options,
            _mockTranscript.Object,
            NullLogger<ArticleContentService>.Instance);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }

    private static RawFeedItem CreateYouTubeItem(string url = "https://www.youtube.com/watch?v=abc123") => new()
    {
        Title = "Test Video",
        ExternalUrl = url,
        PublishedAt = DateTimeOffset.UtcNow,
        Description = "A test video description",
        FeedName = "Test Feed",
        CollectionName = "videos"
    };

    private static RawFeedItem CreateNonYouTubeItem() => new()
    {
        Title = "Test Article",
        ExternalUrl = "https://example.com/article",
        PublishedAt = DateTimeOffset.UtcNow,
        Description = "A test article",
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
            .ReturnsAsync("This is the transcript text from the video.");

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        result.FullContent.Should().Be("This is the transcript text from the video.");
        result.Title.Should().Be(item.Title);
        result.ExternalUrl.Should().Be(item.ExternalUrl);
        result.Description.Should().Be(item.Description);
    }

    [Fact]
    public async Task EnrichWithContentAsync_YouTubeItem_WithoutTranscript_ReturnsOriginalItem()
    {
        // Arrange
        var item = CreateYouTubeItem();
        _mockTranscript
            .Setup(t => t.GetTranscriptAsync(item.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(item);
        result.FullContent.Should().BeNull();
    }

    [Fact]
    public async Task EnrichWithContentAsync_YouTubeItem_CallsTranscriptService()
    {
        // Arrange
        var item = CreateYouTubeItem();
        _mockTranscript
            .Setup(t => t.GetTranscriptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("transcript");

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
            Description = "Video about Copilot",
            Author = "John Doe",
            FeedTags = ["AI", "Copilot"],
            FeedName = "GitHub Blog",
            CollectionName = "videos"
        };
        _mockTranscript
            .Setup(t => t.GetTranscriptAsync(item.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync("Full transcript here.");

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        result.Title.Should().Be("Copilot Features");
        result.ExternalUrl.Should().Be("https://www.youtube.com/watch?v=xyz789");
        result.PublishedAt.Should().Be(item.PublishedAt);
        result.Description.Should().Be("Video about Copilot");
        result.Author.Should().Be("John Doe");
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
            .ReturnsAsync("Short url transcript");

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
            .ReturnsAsync("   ");

        // Act
        var result = await _service.EnrichWithContentAsync(item, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(item);
    }
}
