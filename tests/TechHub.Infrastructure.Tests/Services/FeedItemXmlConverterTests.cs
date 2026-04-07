using System.Xml;
using FluentAssertions;
using TechHub.Infrastructure.Services.ContentProcessing;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Tests for <see cref="FeedItemXmlConverter"/> — the XML-to-compact-text converter
/// used to create token-efficient feed item representations for AI processing.
/// </summary>
public class FeedItemXmlConverterTests
{
    private static XmlNode ParseItemNode(string xml, string nodeName = "item")
    {
        var doc = new XmlDocument();
        doc.LoadXml(xml);
        return doc.SelectSingleNode($"//{nodeName}")!;
    }

    // ── ToCompactText ────────────────────────────────────────────────────────

    [Fact]
    public void ToCompactText_SimpleRssItem_ProducesKeyValueLines()
    {
        var node = ParseItemNode("""
            <item>
                <title>Test Article</title>
                <link>https://example.com/article</link>
                <description>A short description.</description>
            </item>
            """);

        var result = FeedItemXmlConverter.ToCompactText(node);

        result.Should().Contain("title: Test Article");
        result.Should().Contain("link: https://example.com/article");
        result.Should().Contain("description: A short description.");
    }

    [Fact]
    public void ToCompactText_WithNamespacedElements_IncludesPrefix()
    {
        var node = ParseItemNode("""
            <rss xmlns:dc="http://purl.org/dc/elements/1.1/">
                <item>
                    <title>Article</title>
                    <dc:creator>Jane Doe</dc:creator>
                </item>
            </rss>
            """);

        var result = FeedItemXmlConverter.ToCompactText(node);

        result.Should().Contain("dc:creator: Jane Doe");
    }

    [Fact]
    public void ToCompactText_WithNestedElements_UsesSlashNotation()
    {
        var node = ParseItemNode("""
            <feed>
                <entry>
                    <author><name>John Smith</name></author>
                </entry>
            </feed>
            """, "entry");

        var result = FeedItemXmlConverter.ToCompactText(node);

        result.Should().Contain("author/name: John Smith");
    }

    [Fact]
    public void ToCompactText_WithAttributes_IncludesAttributesInline()
    {
        var node = ParseItemNode("""
            <feed>
                <entry>
                    <link rel="alternate" href="https://example.com/article"/>
                </entry>
            </feed>
            """, "entry");

        var result = FeedItemXmlConverter.ToCompactText(node);

        result.Should().Contain("link (rel=alternate, href=https://example.com/article):");
    }

    [Fact]
    public void ToCompactText_SkipsXmlnsAttributes()
    {
        var node = ParseItemNode("""
            <rss xmlns:dc="http://purl.org/dc/elements/1.1/">
                <item>
                    <title>Test</title>
                </item>
            </rss>
            """);

        var result = FeedItemXmlConverter.ToCompactText(node);

        result.Should().NotContain("xmlns");
    }

    [Fact]
    public void ToCompactText_SkipsEmptyElements()
    {
        var node = ParseItemNode("""
            <item>
                <title>Test</title>
                <description></description>
            </item>
            """);

        var result = FeedItemXmlConverter.ToCompactText(node);

        result.Should().Contain("title: Test");
        result.Should().NotContain("description:");
    }

    [Fact]
    public void ToCompactText_WithHtmlEncodedDescription_DecodesAndConverts()
    {
        var node = ParseItemNode("""
            <item>
                <title>Test</title>
                <description>&lt;p&gt;We are &lt;strong&gt;thrilled&lt;/strong&gt; to announce this.&lt;/p&gt;</description>
            </item>
            """);

        var result = FeedItemXmlConverter.ToCompactText(node);

        result.Should().Contain("We are **thrilled** to announce this.");
        result.Should().NotContain("&lt;");
        result.Should().NotContain("<p>");
    }

    [Fact]
    public void ToCompactText_MediaGroupNested_UsesSlashNotation()
    {
        var node = ParseItemNode("""
            <feed xmlns:media="http://search.yahoo.com/mrss/">
                <entry>
                    <title>Video</title>
                    <media:group>
                        <media:title>Video Title</media:title>
                        <media:description>Video description text.</media:description>
                    </media:group>
                </entry>
            </feed>
            """, "entry");

        var result = FeedItemXmlConverter.ToCompactText(node);

        result.Should().Contain("media:group/media:title: Video Title");
        result.Should().Contain("media:group/media:description: Video description text.");
    }

    [Fact]
    public void ToCompactText_NullNode_ThrowsArgumentNullException()
    {
        var act = () => FeedItemXmlConverter.ToCompactText(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToCompactText_CdataContent_ExtractsInnerText()
    {
        var node = ParseItemNode("""
            <item>
                <title>CDATA Test</title>
                <description><![CDATA[Content inside CDATA block.]]></description>
            </item>
            """);

        var result = FeedItemXmlConverter.ToCompactText(node);

        result.Should().Contain("description: Content inside CDATA block.");
    }

    [Fact]
    public void ToCompactText_MultipleCategories_EachOnOwnLine()
    {
        var node = ParseItemNode("""
            <item>
                <title>Test</title>
                <category>Azure</category>
                <category>DevOps</category>
                <category>AI</category>
            </item>
            """);

        var result = FeedItemXmlConverter.ToCompactText(node);

        result.Should().Contain("category: Azure");
        result.Should().Contain("category: DevOps");
        result.Should().Contain("category: AI");
    }

    // ── CleanValue ───────────────────────────────────────────────────────────

    [Fact]
    public void CleanValue_PlainText_ReturnsUnchanged()
    {
        var result = FeedItemXmlConverter.CleanValue("Hello world");

        result.Should().Be("Hello world");
    }

    [Fact]
    public void CleanValue_HtmlEntities_Decodes()
    {
        var result = FeedItemXmlConverter.CleanValue("&amp; &lt; &gt; &quot;");

        result.Should().Be("& < > \"");
    }

    [Fact]
    public void CleanValue_HtmlParagraphs_ConvertedToMarkdown()
    {
        var result = FeedItemXmlConverter.CleanValue("<p>First paragraph.</p><p>Second paragraph.</p>");

        result.Should().Contain("First paragraph.");
        result.Should().Contain("Second paragraph.");
        result.Should().NotContain("<p>");
    }

    [Fact]
    public void CleanValue_StrongAndEmTags_ConvertedToMarkdown()
    {
        var result = FeedItemXmlConverter.CleanValue("<strong>bold</strong> and <em>italic</em>");

        result.Should().Contain("**bold**");
        result.Should().Contain("*italic*");
    }

    [Fact]
    public void CleanValue_AnchorTag_ConvertedToMarkdownLink()
    {
        var result = FeedItemXmlConverter.CleanValue("""<a href="https://example.com">click here</a>""");

        result.Should().Contain("[click here](https://example.com)");
    }

    [Fact]
    public void CleanValue_ListItems_ConvertedToDashes()
    {
        var result = FeedItemXmlConverter.CleanValue("<ul><li>Item A</li><li>Item B</li></ul>");

        result.Should().Contain("- Item A");
        result.Should().Contain("- Item B");
    }

    [Fact]
    public void CleanValue_HeadingTags_ConvertedToMarkdownHeadings()
    {
        var result = FeedItemXmlConverter.CleanValue("<h2>Section Title</h2>");

        result.Should().Contain("## Section Title");
    }

    [Fact]
    public void CleanValue_CodeBlock_ConvertedToFencedCode()
    {
        var result = FeedItemXmlConverter.CleanValue("<pre><code>var x = 1;</code></pre>");

        result.Should().Contain("```");
        result.Should().Contain("var x = 1;");
    }

    [Fact]
    public void CleanValue_EmptyOrWhitespace_ReturnsEmpty()
    {
        FeedItemXmlConverter.CleanValue("").Should().BeEmpty();
        FeedItemXmlConverter.CleanValue("   ").Should().BeEmpty();
    }

    [Fact]
    public void CleanValue_RemainingHtmlTags_AreStripped()
    {
        var result = FeedItemXmlConverter.CleanValue("<div class=\"wrapper\"><span>text</span></div>");

        result.Should().Be("text");
        result.Should().NotContain("<");
    }

    [Fact]
    public void CleanValue_ExcessiveWhitespace_IsCollapsed()
    {
        var result = FeedItemXmlConverter.CleanValue("word1     word2\n\n\n\n\nword3");

        result.Should().Be("word1 word2\n\nword3");
    }
}

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
