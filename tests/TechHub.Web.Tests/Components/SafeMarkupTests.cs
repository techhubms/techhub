using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Web.Components;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for SafeMarkup.razor — raw HTML rendering with hash-only links rewritten to the current page path,
/// working around the document-level <c>&lt;base href="/"&gt;</c>.
/// </summary>
public class SafeMarkupTests : BunitContext
{
    [Fact]
    public void Render_HashLink_IsRewrittenToCurrentPathPlusHash()
    {
        // Arrange
        NavigateTo("/github-copilot/blogs/my-post");

        // Act
        var cut = RenderHtml("""<a href="#section-1">Jump</a>""");

        // Assert
        cut.Find("a").GetAttribute("href").Should().Be("/github-copilot/blogs/my-post#section-1");
    }

    [Fact]
    public void Render_SingleQuotedHashLink_IsRewrittenToCurrentPathPlusHash()
    {
        // Arrange
        NavigateTo("/github-copilot/blogs/my-post");

        // Act
        var cut = RenderHtml("""<a href='#section-1'>Jump</a>""");

        // Assert
        cut.Find("a").GetAttribute("href").Should().Be("/github-copilot/blogs/my-post#section-1");
    }

    [Fact]
    public void Render_HashLinkOnPageWithQuery_PreservesQueryString()
    {
        // Arrange
        NavigateTo("/ai/news?tags=azure&page=2");

        // Act
        var cut = RenderHtml("""<a href="#top">Top</a>""");

        // Assert
        cut.Find("a").GetAttribute("href").Should().Be("/ai/news?tags=azure&page=2#top");
    }

    [Fact]
    public void Render_HashLinkOnPageWithQuery_HtmlEncodesQueryStringInMarkup()
    {
        // Arrange
        NavigateTo("/ai/news?tags=azure&page=2");

        // Act
        var cut = RenderHtml("""<a href="#top">Top</a>""");

        // Assert
        cut.Markup.Should().Contain("""href="/ai/news?tags=azure&amp;page=2#top"""");
        cut.Find("a").GetAttribute("href").Should().Be("/ai/news?tags=azure&page=2#top");
    }

    [Fact]
    public void Render_HashLinkOnRootPage_UsesRootPath()
    {
        // Arrange
        NavigateTo("/");

        // Act
        var cut = RenderHtml("""<a href="#faq">FAQ</a>""");

        // Assert
        cut.Find("a").GetAttribute("href").Should().Be("/#faq");
    }

    [Fact]
    public void Render_MultipleHashLinks_RewritesEveryOne()
    {
        // Arrange
        NavigateTo("/about");

        // Act
        var cut = RenderHtml("""<ul><li><a href="#one">1</a></li><li><a href="#two">2</a></li><li><a href="#three">3</a></li></ul>""");

        // Assert
        cut.FindAll("a").Select(a => a.GetAttribute("href"))
            .Should().Equal("/about#one", "/about#two", "/about#three");
    }

    [Theory]
    [InlineData("https://example.com/page#frag")]
    [InlineData("https://example.com/#frag")]
    [InlineData("/other/page#frag")]
    [InlineData("other.html#frag")]
    [InlineData("mailto:someone@example.com")]
    public void Render_NonHashOnlyLink_IsLeftUntouched(string href)
    {
        // Arrange
        NavigateTo("/current/page");

        // Act
        var cut = RenderHtml($"""<a href="{href}">Link</a>""");

        // Assert
        cut.Find("a").GetAttribute("href").Should().Be(href);
    }

    [Fact]
    public void Render_BareHashLink_IsLeftUntouched()
    {
        // Arrange
        NavigateTo("/current/page");

        // Act
        var cut = RenderHtml("""<a href="#">Top</a>""");

        // Assert
        cut.Find("a").GetAttribute("href").Should().Be("#", "an empty fragment has nothing to anchor to");
    }

    [Fact]
    public void Render_MixedLinks_RewritesOnlyHashOnlyOnes()
    {
        // Arrange
        NavigateTo("/docs");

        // Act
        var cut = RenderHtml("""<a href="#local">L</a> <a href="https://example.com#remote">R</a>""");

        // Assert
        cut.FindAll("a").Select(a => a.GetAttribute("href"))
            .Should().Equal("/docs#local", "https://example.com#remote");
    }

    [Fact]
    public void Render_HtmlWithoutLinks_RendersMarkupUnchanged()
    {
        // Arrange
        NavigateTo("/docs");

        // Act
        var cut = RenderHtml("<h2 id=\"intro\">Intro</h2><p>Some <strong>bold</strong> text</p>");

        // Assert
        cut.Find("h2#intro").TextContent.Should().Be("Intro");
        cut.Find("p strong").TextContent.Should().Be("bold");
    }

    [Fact]
    public void Render_HtmlIsInterpretedAsMarkupNotEscaped()
    {
        // Arrange
        NavigateTo("/docs");

        // Act
        var cut = RenderHtml("<em>emphasis</em>");

        // Assert
        cut.Find("em").TextContent.Should().Be("emphasis");
        cut.Markup.Should().NotContain("&lt;em&gt;");
    }

    [Fact]
    public void Render_TargetIdAttribute_IsNotTouched()
    {
        // Arrange
        NavigateTo("/docs");

        // Act
        var cut = RenderHtml("""<h2 id="#odd">Heading</h2>""");

        // Assert
        cut.Find("h2").GetAttribute("id").Should().Be("#odd", "only href attributes are rewritten");
    }

    [Fact]
    public void Render_NullHtml_RendersNothing()
    {
        // Arrange
        NavigateTo("/docs");

        // Act
        var cut = RenderHtml(null);

        // Assert
        cut.Markup.Trim().Should().BeEmpty();
    }

    [Fact]
    public void Render_EmptyHtml_RendersNothing()
    {
        // Arrange
        NavigateTo("/docs");

        // Act
        var cut = RenderHtml(string.Empty);

        // Assert
        cut.Markup.Trim().Should().BeEmpty();
    }

    [Fact]
    public void SetParametersAsync_HtmlChanged_RewritesLinksInNewHtml()
    {
        // Arrange
        NavigateTo("/docs");
        var cut = RenderHtml("""<a href="#old">Old</a>""");

        // Act
        cut.Render(p => p.Add(x => x.Html, """<a href="#new">New</a>"""));

        // Assert
        cut.Find("a").GetAttribute("href").Should().Be("/docs#new");
    }

    private void NavigateTo(string relativeUri) =>
        Services.GetRequiredService<NavigationManager>().NavigateTo(relativeUri);

    private IRenderedComponent<SafeMarkup> RenderHtml(string? html) =>
        Render<SafeMarkup>(p => p.Add(x => x.Html, html));
}
