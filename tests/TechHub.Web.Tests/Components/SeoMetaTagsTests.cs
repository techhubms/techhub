using FluentAssertions;
using TechHub.Web.Components;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Unit tests for SeoMetaTags static helper methods.
/// These test the pure logic without needing a Blazor component render context.
/// </summary>
public class SeoMetaTagsTests
{
    // ────────────────────────────────────────────────────────────────────────
    // TruncateDescription
    // ────────────────────────────────────────────────────────────────────────

    [Fact]
    public void TruncateDescription_ShortText_ReturnsUnchanged()
    {
        // Arrange
        var input = "Short description";

        // Act
        var result = SeoMetaTags.TruncateDescription(input, 160);

        // Assert
        result.Should().Be("Short description");
    }

    [Fact]
    public void TruncateDescription_ExactlyMaxLength_ReturnsUnchanged()
    {
        // Arrange
        var input = new string('a', 160);

        // Act
        var result = SeoMetaTags.TruncateDescription(input, 160);

        // Assert
        result.Should().HaveLength(160);
        result.Should().NotEndWith("...");
    }

    [Fact]
    public void TruncateDescription_LongText_TruncatesWithEllipsis()
    {
        // Arrange
        var input = new string('a', 200);

        // Act
        var result = SeoMetaTags.TruncateDescription(input, 160);

        // Assert
        result.Should().EndWith("...");
        result.Length.Should().BeLessThanOrEqualTo(160);
    }

    [Fact]
    public void TruncateDescription_LongTextWithWords_TruncatesAtWordBoundary()
    {
        // Arrange
        // 150 "a" chars + space + 20 "b" chars = 171 chars total
        var input = new string('a', 150) + " " + new string('b', 20);

        // Act
        var result = SeoMetaTags.TruncateDescription(input, 160);

        // Assert
        result.Should().EndWith("...");
        result.Should().Contain(new string('a', 150));
        result.Should().NotContain(new string('b', 5), "word after boundary should be excluded");
    }

    [Fact]
    public void TruncateDescription_HtmlInput_StripsTagsBeforeTruncating()
    {
        // Arrange
        var input = "<p>This is a <strong>description</strong> with <em>HTML</em> tags.</p>";

        // Act
        var result = SeoMetaTags.TruncateDescription(input, 160);

        // Assert
        result.Should().Be("This is a description with HTML tags.");
        result.Should().NotContain("<");
        result.Should().NotContain(">");
    }

    [Fact]
    public void TruncateDescription_EmptyString_ReturnsEmpty()
    {
        // Act
        var result = SeoMetaTags.TruncateDescription(string.Empty, 160);

        // Assert
        result.Should().Be(string.Empty);
    }

    [Fact]
    public void TruncateDescription_NullString_ReturnsNull()
    {
        // Act
        var result = SeoMetaTags.TruncateDescription(null!, 160);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TruncateDescription_ExcessiveWhitespace_NormalizesSpaces()
    {
        // Arrange
        var input = "  word1   word2    word3  ";

        // Act
        var result = SeoMetaTags.TruncateDescription(input, 160);

        // Assert
        result.Should().Be("word1 word2 word3");
    }

    // ────────────────────────────────────────────────────────────────────────
    // TryGetYouTubeEmbedUrl
    // ────────────────────────────────────────────────────────────────────────

    [Fact]
    public void TryGetYouTubeEmbedUrl_StandardWatchUrl_ReturnsEmbedUrl()
    {
        // Arrange
        var url = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";

        // Act
        var result = SeoMetaTags.TryGetYouTubeEmbedUrl(url);

        // Assert
        result.Should().Be("https://www.youtube.com/embed/dQw4w9WgXcQ");
    }

    [Fact]
    public void TryGetYouTubeEmbedUrl_YoutubeBeUrl_ReturnsEmbedUrl()
    {
        // Arrange
        var url = "https://youtu.be/dQw4w9WgXcQ";

        // Act
        var result = SeoMetaTags.TryGetYouTubeEmbedUrl(url);

        // Assert
        result.Should().Be("https://www.youtube.com/embed/dQw4w9WgXcQ");
    }

    [Fact]
    public void TryGetYouTubeEmbedUrl_NonYouTubeUrl_ReturnsNull()
    {
        // Arrange
        var url = "https://vimeo.com/123456789";

        // Act
        var result = SeoMetaTags.TryGetYouTubeEmbedUrl(url);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetYouTubeEmbedUrl_NullUrl_ReturnsNull()
    {
        // Act
        var result = SeoMetaTags.TryGetYouTubeEmbedUrl(null!);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetYouTubeEmbedUrl_EmptyUrl_ReturnsNull()
    {
        // Act
        var result = SeoMetaTags.TryGetYouTubeEmbedUrl(string.Empty);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetYouTubeEmbedUrl_YouTubeWithoutVideoId_ReturnsNull()
    {
        // Arrange
        var url = "https://www.youtube.com/watch";

        // Act
        var result = SeoMetaTags.TryGetYouTubeEmbedUrl(url);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetYouTubeEmbedUrl_YouTubeNoDomainPrefix_ReturnsEmbedUrl()
    {
        // Arrange - youtube.com without www
        var url = "https://youtube.com/watch?v=abc123";

        // Act
        var result = SeoMetaTags.TryGetYouTubeEmbedUrl(url);

        // Assert
        result.Should().Be("https://www.youtube.com/embed/abc123");
    }

    // ────────────────────────────────────────────────────────────────────────
    // TryGetYouTubeThumbnailUrl
    // ────────────────────────────────────────────────────────────────────────

    [Fact]
    public void TryGetYouTubeThumbnailUrl_StandardWatchUrl_ReturnsThumbnailUrl()
    {
        // Arrange
        var url = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";

        // Act
        var result = SeoMetaTags.TryGetYouTubeThumbnailUrl(url);

        // Assert
        result.Should().Be("https://img.youtube.com/vi/dQw4w9WgXcQ/maxresdefault.jpg");
    }

    [Fact]
    public void TryGetYouTubeThumbnailUrl_YoutubeBeUrl_ReturnsThumbnailUrl()
    {
        // Arrange
        var url = "https://youtu.be/dQw4w9WgXcQ";

        // Act
        var result = SeoMetaTags.TryGetYouTubeThumbnailUrl(url);

        // Assert
        result.Should().Be("https://img.youtube.com/vi/dQw4w9WgXcQ/maxresdefault.jpg");
    }

    [Fact]
    public void TryGetYouTubeThumbnailUrl_NonYouTubeUrl_ReturnsNull()
    {
        // Arrange
        var url = "https://vimeo.com/123456789";

        // Act
        var result = SeoMetaTags.TryGetYouTubeThumbnailUrl(url);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetYouTubeThumbnailUrl_NullUrl_ReturnsNull()
    {
        // Act
        var result = SeoMetaTags.TryGetYouTubeThumbnailUrl(null);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetYouTubeThumbnailUrl_EmptyUrl_ReturnsNull()
    {
        // Act
        var result = SeoMetaTags.TryGetYouTubeThumbnailUrl(string.Empty);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetYouTubeThumbnailUrl_YouTubeWithoutVideoId_ReturnsNull()
    {
        // Arrange
        var url = "https://www.youtube.com/watch";

        // Act
        var result = SeoMetaTags.TryGetYouTubeThumbnailUrl(url);

        // Assert
        result.Should().BeNull();
    }
}
