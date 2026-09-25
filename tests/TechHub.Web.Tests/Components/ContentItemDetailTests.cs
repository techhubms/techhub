using Bunit;
using FluentAssertions;
using TechHub.TestUtilities.Builders;
using TechHub.Web.Components;

namespace TechHub.Web.Tests.Components;

public class ContentItemDetailTests : BunitContext
{
    [Fact]
    public void ContentItemDetail_ExternalItem_RendersEncodedExcerptAndOriginalArticleLink()
    {
        // Arrange
        var item = A.ContentItem
            .WithTitle("External Article")
            .WithCollectionName("news")
            .WithPrimarySectionName("ai")
            .WithSlug("external-article")
            .WithExcerpt("<strong>Unsafe</strong> excerpt")
            .WithExternalUrl("https://example.com/original-article")
            .WithRenderedHtml("<p>Full body should not render</p>")
            .BuildDetail();

        // Act
        var cut = Render<ContentItemDetail>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        cut.Markup.Should().Contain("&lt;strong&gt;Unsafe&lt;/strong&gt; excerpt");
        cut.Markup.Should().NotContain("<strong>Unsafe</strong> excerpt");
        cut.Markup.Should().NotContain("Full body should not render");

        var sourceLink = cut.Find(".article-external-cta a");
        sourceLink.GetAttribute("href").Should().Be("https://example.com/original-article");
        sourceLink.GetAttribute("target").Should().Be("_blank");
        sourceLink.GetAttribute("rel").Should().Be("noopener noreferrer");
    }
}
