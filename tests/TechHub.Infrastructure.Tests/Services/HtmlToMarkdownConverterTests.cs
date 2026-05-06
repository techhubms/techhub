using FluentAssertions;
using TechHub.Infrastructure.Services.ContentProcessing;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Tests for <see cref="HtmlToMarkdownConverter"/> — shared HTML-to-markdown logic.
/// </summary>
public class HtmlToMarkdownConverterTests
{
    [Fact]
    public void Convert_EmptyOrWhitespace_ReturnsEmpty()
    {
        HtmlToMarkdownConverter.Convert("").Should().BeEmpty();
        HtmlToMarkdownConverter.Convert("   ").Should().BeEmpty();
    }

    [Fact]
    public void Convert_HeadingsPreserved()
    {
        var result = HtmlToMarkdownConverter.Convert("<h1>Title</h1><h2>Sub</h2>");

        result.Should().Contain("# Title");
        result.Should().Contain("## Sub");
    }

    [Fact]
    public void Convert_ListsPreserved()
    {
        var result = HtmlToMarkdownConverter.Convert("<ul><li>Alpha</li><li>Beta</li></ul>");

        result.Should().Contain("- Alpha");
        result.Should().Contain("- Beta");
    }

    [Fact]
    public void Convert_CodeBlockWithLanguage()
    {
        var result = HtmlToMarkdownConverter.Convert(
            """<pre><code class="language-csharp">var x = 1;</code></pre>""");

        result.Should().Contain("```csharp");
        result.Should().Contain("var x = 1;");
        result.Should().Contain("```");
    }

    [Fact]
    public void Convert_CodeBlockWithLangPrefix()
    {
        var result = HtmlToMarkdownConverter.Convert(
            """<pre><code class="lang-bash">dotnet run</code></pre>""");

        result.Should().Contain("```bash");
        result.Should().Contain("dotnet run");
    }

    [Fact]
    public void Convert_CodeBlockWithoutLanguage()
    {
        var result = HtmlToMarkdownConverter.Convert("<pre><code>plain code</code></pre>");

        result.Should().Contain("```");
        result.Should().Contain("plain code");
    }

    [Fact]
    public void Convert_AnchorDoubleQuoted()
    {
        var result = HtmlToMarkdownConverter.Convert("""<a href="https://example.com">click</a>""");

        result.Should().Contain("[click](https://example.com)");
    }

    [Fact]
    public void Convert_AnchorSingleQuoted()
    {
        var result = HtmlToMarkdownConverter.Convert("<a href='https://example.com'>click</a>");

        result.Should().Contain("[click](https://example.com)");
    }

    [Fact]
    public void Convert_ImgWithSrcAndAlt()
    {
        var result = HtmlToMarkdownConverter.Convert(
            """<img src="https://example.com/img.png" alt="a photo" />""");

        result.Should().Contain("![a photo](https://example.com/img.png)");
    }

    [Fact]
    public void Convert_ImgWithAltBeforeSrc()
    {
        // Attribute order should not matter
        var result = HtmlToMarkdownConverter.Convert(
            """<img alt="reversed" src="https://example.com/x.jpg" />""");

        result.Should().Contain("![reversed](https://example.com/x.jpg)");
    }

    [Fact]
    public void Convert_ImgWithDataSrcFallback()
    {
        // Lazy-loaded images use data-src when src is a placeholder
        var result = HtmlToMarkdownConverter.Convert(
            """<img data-src="https://cdn.example.com/lazy.jpg" alt="lazy img" />""");

        result.Should().Contain("![lazy img](https://cdn.example.com/lazy.jpg)");
    }

    [Fact]
    public void Convert_ImgWithNoAlt()
    {
        var result = HtmlToMarkdownConverter.Convert("""<img src="https://example.com/img.png" />""");

        result.Should().Contain("![](https://example.com/img.png)");
    }

    [Fact]
    public void Convert_NoiseElementsRemoved()
    {
        var html = """
            <nav>Navigation links</nav>
            <header>Site header</header>
            <main><p>Actual content</p></main>
            <footer>Footer</footer>
            <aside>Sidebar</aside>
            <script>alert('x')</script>
            """;

        var result = HtmlToMarkdownConverter.Convert(html);

        result.Should().Contain("Actual content");
        result.Should().NotContain("Navigation links");
        // Note: <header> content is intentionally preserved by the converter so that
        // article-level headers (containing H1 titles) are not stripped. Page-level
        // <header> removal is the responsibility of the caller (ArticleContentService).
        result.Should().Contain("Site header");
        result.Should().NotContain("Footer");
        result.Should().NotContain("Sidebar");
        result.Should().NotContain("alert");
    }

    [Fact]
    public void Convert_HtmlEntitiesDecoded()
    {
        // &amp; and &quot; decode to & and " respectively.
        // &lt;Hello&gt; decodes to <Hello> which is then stripped as an unknown tag —
        // that is expected behaviour (the converter does not re-encode angle brackets).
        var result = HtmlToMarkdownConverter.Convert("<p>&amp;nice &quot;World&quot;</p>");

        result.Should().Contain("&nice");
        result.Should().Contain("\"World\"");
    }

    [Fact]
    public void Convert_StrongAndEmPreserved()
    {
        var result = HtmlToMarkdownConverter.Convert("<p><strong>bold</strong> and <em>italic</em></p>");

        result.Should().Contain("**bold**");
        result.Should().Contain("*italic*");
    }

    [Fact]
    public void Convert_RemainingTagsStripped()
    {
        var result = HtmlToMarkdownConverter.Convert("<div><span>text</span></div>");

        result.Should().Be("text");
        result.Should().NotContain("<");
    }
}
