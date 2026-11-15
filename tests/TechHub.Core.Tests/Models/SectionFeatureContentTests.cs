using FluentAssertions;
using TechHub.Core.Models;

namespace TechHub.Core.Tests.Models;

public class SectionFeatureContentTests
{
    private static SectionFeatureContent Content(string externalUrl, string collection = "videos") => new()
    {
        CollectionName = collection,
        ItemSlug = "My-Slug",
        Title = "Title",
        ExternalUrl = externalUrl,
        PrimarySectionName = "github-copilot"
    };

    [Theory]
    [InlineData("https://youtu.be/dQw4w9WgXcQ", "dQw4w9WgXcQ")]
    [InlineData("https://youtu.be/dQw4w9WgXcQ?t=10", "dQw4w9WgXcQ")]
    [InlineData("https://www.youtube.com/watch?v=dQw4w9WgXcQ", "dQw4w9WgXcQ")]
    [InlineData("https://youtube.com/watch?feature=share&v=abc_123-XYZ", "abc_123-XYZ")]
    public void GetYouTubeVideoId_YouTubeAddress_ReturnsTheId(string url, string expected)
    {
        Content(url).GetYouTubeVideoId().Should().Be(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("not a url")]
    [InlineData("https://example.com/watch?v=dQw4w9WgXcQ")]
    [InlineData("https://notyoutube.com/watch?v=dQw4w9WgXcQ")]
    [InlineData("https://www.youtube.com/watch")]
    [InlineData("https://youtu.be/")]
    [InlineData("ftp://youtu.be/dQw4w9WgXcQ")]
    [InlineData("https://youtu.be/id%22onerror%3D%22x")]
    public void GetYouTubeVideoId_NoYouTubeVideo_ReturnsNull(string url)
    {
        Content(url).GetYouTubeVideoId().Should().BeNull();
    }

    [Fact]
    public void GetHref_InternalCollection_UsesThePageOfTheItem()
    {
        Content("https://youtu.be/dQw4w9WgXcQ").GetHref().Should().Be("/github-copilot/videos/my-slug");
    }

    [Fact]
    public void GetHref_NewsItem_UsesTheOriginalArticle()
    {
        Content("https://example.com/article", collection: "news").GetHref().Should().Be("https://example.com/article");
    }

    [Fact]
    public void LinkAttributes_ArticleOnAnotherSite_OpensInANewTabAndSaysSo()
    {
        var content = Content("https://example.com/article", collection: "news");

        content.GetTarget().Should().Be("_blank");
        content.GetRel().Should().Be("noopener noreferrer");
        content.GetAriaLabel("Agent mode").Should().Be("Agent mode - opens in new tab");
    }

    [Fact]
    public void LinkAttributes_PageOfThisSite_StaysInTheSameTab()
    {
        var content = Content("https://youtu.be/dQw4w9WgXcQ");

        content.GetTarget().Should().BeNull();
        content.GetRel().Should().BeNull();
        content.GetAriaLabel("Agent mode").Should().Be("Agent mode");
    }

    [Fact]
    public void LinkAttributes_UnsafeExternalAddress_StaysInTheSameTab()
    {
        var content = Content("javascript:alert('xss')", collection: "news");

        content.GetHref().Should().StartWith("/", "an unsafe address is never used as the link");
        content.GetTarget().Should().BeNull();
    }
}
