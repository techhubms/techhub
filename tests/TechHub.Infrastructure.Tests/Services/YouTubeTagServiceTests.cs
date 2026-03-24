using FluentAssertions;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Unit tests for YouTubeTagService video ID extraction and tag parsing logic.
/// </summary>
public class YouTubeTagServiceTests
{
    #region ExtractVideoId Tests

    [Theory]
    [InlineData("https://www.youtube.com/watch?v=dQw4w9WgXcQ", "dQw4w9WgXcQ")]
    [InlineData("https://youtube.com/watch?v=dQw4w9WgXcQ", "dQw4w9WgXcQ")]
    [InlineData("https://m.youtube.com/watch?v=dQw4w9WgXcQ", "dQw4w9WgXcQ")]
    [InlineData("https://www.youtube.com/watch?v=dQw4w9WgXcQ&t=30", "dQw4w9WgXcQ")]
    public void ExtractVideoId_WatchUrl_ReturnsVideoId(string url, string expected)
    {
        YouTubeTagService.ExtractVideoId(url).Should().Be(expected);
    }

    [Theory]
    [InlineData("https://youtu.be/dQw4w9WgXcQ", "dQw4w9WgXcQ")]
    public void ExtractVideoId_ShortUrl_ReturnsVideoId(string url, string expected)
    {
        YouTubeTagService.ExtractVideoId(url).Should().Be(expected);
    }

    [Theory]
    [InlineData("https://www.youtube.com/embed/dQw4w9WgXcQ", "dQw4w9WgXcQ")]
    [InlineData("https://www.youtube.com/v/dQw4w9WgXcQ", "dQw4w9WgXcQ")]
    [InlineData("https://www.youtube.com/shorts/dQw4w9WgXcQ", "dQw4w9WgXcQ")]
    public void ExtractVideoId_EmbedAndShortsUrls_ReturnsVideoId(string url, string expected)
    {
        YouTubeTagService.ExtractVideoId(url).Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("not-a-url")]
    [InlineData("https://example.com/watch?v=dQw4w9WgXcQ")]
    [InlineData("https://www.youtube.com/channel/UCwhatever")]
    [InlineData("https://www.youtube.com/watch")] // no v= param
    public void ExtractVideoId_InvalidUrls_ReturnsNull(string? url)
    {
        YouTubeTagService.ExtractVideoId(url!).Should().BeNull();
    }

    [Fact]
    public void ExtractVideoId_WatchWithInvalidVideoId_ReturnsNull()
    {
        // Video ID too short
        YouTubeTagService.ExtractVideoId("https://www.youtube.com/watch?v=abc").Should().BeNull();
    }

    #endregion
}
