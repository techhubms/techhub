using System.Net;
using System.Text.RegularExpressions;

namespace TechHub.Infrastructure.Services.ContentProcessing;

/// <summary>
/// Converts HTML content to structured markdown.
/// Handles noise removal (nav, script, footer, etc.) and converts semantic HTML elements
/// (headings, lists, code blocks, links, images) to their markdown equivalents.
/// Shared by <see cref="FeedItemXmlConverter"/> (RSS descriptions) and
/// <see cref="ArticleContentService"/> (full article HTML).
/// </summary>
internal static partial class HtmlToMarkdownConverter
{
    /// <summary>
    /// Removes non-content HTML elements (nav, script, style, button, etc.),
    /// then converts the remaining semantic HTML to GitHub-flavoured markdown.
    /// Also decodes HTML entities.
    /// </summary>
    internal static string Convert(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return string.Empty;
        }

        // 1. Decode HTML entities first so regexes match real characters.
        var result = WebUtility.HtmlDecode(html);

        // 2. Remove non-content blocks (order matters — remove containers before their children).
        result = NoiseBlock().Replace(result, string.Empty);   // script, style, noscript
        result = NavBlock().Replace(result, string.Empty);     // nav
        // Note: <header> is NOT stripped here because the converter may receive article-level HTML
        // (e.g. from ArticleContentService after article extraction) where <header> contains the
        // article title (h1). Page-level <header> noise should be removed upstream before calling
        // this converter. See ArticleContentService.ExtractMainContent for the pre-pass.
        result = FooterBlock().Replace(result, string.Empty);  // footer
        result = AsideBlock().Replace(result, string.Empty);   // aside
        result = IframeBlock().Replace(result, string.Empty);  // iframe
        result = ButtonBlock().Replace(result, string.Empty);  // button
        result = SelectBlock().Replace(result, string.Empty);  // select
        result = HtmlComment().Replace(result, string.Empty);  // <!-- comments -->

        // 3. Convert block-level semantic elements to markdown (order: outer before inner).

        // Headings
        result = Heading1().Replace(result, "\n# $1\n");
        result = Heading2().Replace(result, "\n## $1\n");
        result = Heading3().Replace(result, "\n### $1\n");
        result = Heading4().Replace(result, "\n#### $1\n");
        result = Heading5().Replace(result, "\n##### $1\n");
        result = Heading6().Replace(result, "\n###### $1\n");

        // Code blocks: <pre><code class="language-X"> → ```X ... ```.
        // Process pre>code before standalone pre/code so the language is captured.
        result = PreCodeWithLang().Replace(result, m =>
        {
            var lang = m.Groups[1].Value.Trim();
            var code = WebUtility.HtmlDecode(m.Groups[2].Value);
            return $"\n```{lang}\n{code.Trim()}\n```\n";
        });
        result = PreCode().Replace(result, "\n```\n$1\n```\n");
        result = PreBlock().Replace(result, "\n```\n$1\n```\n");

        // Blockquotes
        result = BlockquoteBlock().Replace(result, "\n> $1\n");

        // Paragraphs
        result = ParagraphTag().Replace(result, "\n\n$1\n\n");

        // Line breaks
        result = LineBreak().Replace(result, "\n");

        // Lists — list items before their wrappers
        result = ListItemTag().Replace(result, "\n- $1");
        result = ListWrapperTag().Replace(result, "\n");

        // Tables — rudimentary: just preserve text flow, remove table markup
        result = TableTag().Replace(result, "\n");

        // 4. Convert inline elements.
        result = StrongTag().Replace(result, "**$1**");
        result = EmTag().Replace(result, "*$1*");
        result = InlineCode().Replace(result, "`$1`");

        // Links: honour both double-quoted and single-quoted href.
        result = AnchorDoubleQuote().Replace(result, "[$2]($1)");
        result = AnchorSingleQuote().Replace(result, "[$2]($1)");
        // Clean up anchor tags that lost their href (navigation artifacts).
        result = BareAnchor().Replace(result, string.Empty);

        // Images: try src first; fall back to data-src / data-lazy-src / data-original for lazy loading.
        // Handle both attribute orderings (src before alt, and alt before src).
        result = ImgWithSrcAndAlt().Replace(result, "![$2]($1)");           // src=G1, alt=G2
        result = ImgWithSrcAndAltReversed().Replace(result, "![$1]($2)");   // alt=G1, src=G2
        result = ImgWithDataSrcAndAlt().Replace(result, "![$2]($1)");       // url=G1, alt=G2
        result = ImgWithSrcNoAlt().Replace(result, "![]($1)");
        result = ImgWithDataSrcNoAlt().Replace(result, "![]($1)");
        // Remove any remaining img tags we couldn't parse.
        result = BareImg().Replace(result, string.Empty);

        // 5. Strip remaining HTML tags.
        result = AnyHtmlTag().Replace(result, string.Empty);

        // 6. Clean up whitespace.
        result = ExcessiveNewlines().Replace(result, "\n\n");
        result = ExcessiveSpaces().Replace(result, " ");

        return result.Trim();
    }

    // ── Noise blocks ──────────────────────────────────────────────────────────

    [GeneratedRegex(@"<(script|style|noscript)[^>]*>[\s\S]*?</(script|style|noscript)>",
        RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 5000)]
    private static partial Regex NoiseBlock();

    [GeneratedRegex(@"<nav[^>]*>[\s\S]*?</nav>",
        RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 5000)]
    private static partial Regex NavBlock();

    // <header> is intentionally NOT removed at the converter level; see Convert() comment.

    [GeneratedRegex(@"<footer[^>]*>[\s\S]*?</footer>",
        RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 5000)]
    private static partial Regex FooterBlock();

    [GeneratedRegex(@"<aside[^>]*>[\s\S]*?</aside>",
        RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 5000)]
    private static partial Regex AsideBlock();

    [GeneratedRegex(@"<iframe[^>]*>[\s\S]*?</iframe>|<iframe[^>]*/?>",
        RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 5000)]
    private static partial Regex IframeBlock();

    [GeneratedRegex(@"<button[^>]*>[\s\S]*?</button>|<button[^>]*/?>",
        RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 5000)]
    private static partial Regex ButtonBlock();

    [GeneratedRegex(@"<select[^>]*>[\s\S]*?</select>",
        RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 5000)]
    private static partial Regex SelectBlock();

    [GeneratedRegex(@"<!--[\s\S]*?-->",
        RegexOptions.None, matchTimeoutMilliseconds: 5000)]
    private static partial Regex HtmlComment();

    // ── Headings ──────────────────────────────────────────────────────────────

    [GeneratedRegex(@"<h1[^>]*>(.*?)</h1>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex Heading1();

    [GeneratedRegex(@"<h2[^>]*>(.*?)</h2>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex Heading2();

    [GeneratedRegex(@"<h3[^>]*>(.*?)</h3>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex Heading3();

    [GeneratedRegex(@"<h4[^>]*>(.*?)</h4>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex Heading4();

    [GeneratedRegex(@"<h5[^>]*>(.*?)</h5>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex Heading5();

    [GeneratedRegex(@"<h6[^>]*>(.*?)</h6>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex Heading6();

    // ── Code blocks ───────────────────────────────────────────────────────────

    // <pre><code class="language-X"> or <pre><code class="lang-X">
    // Handles multiple CSS classes (e.g. class="cs language-csharp").
    [GeneratedRegex(@"<pre[^>]*>\s*<code[^>]*\bclass=[""'][^""']*(?:language-|lang-)([^""'\s]+)[^""']*[""'][^>]*>([\s\S]*?)</code>\s*</pre>",
        RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 2000)]
    private static partial Regex PreCodeWithLang();

    // <pre><code> without language class
    [GeneratedRegex(@"<pre[^>]*>\s*<code[^>]*>([\s\S]*?)</code>\s*</pre>",
        RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 2000)]
    private static partial Regex PreCode();

    // Bare <pre> (no <code> inside)
    [GeneratedRegex(@"<pre[^>]*>([\s\S]*?)</pre>",
        RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 2000)]
    private static partial Regex PreBlock();

    // ── Other block elements ──────────────────────────────────────────────────

    [GeneratedRegex(@"<blockquote[^>]*>(.*?)</blockquote>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex BlockquoteBlock();

    [GeneratedRegex(@"<p[^>]*>(.*?)</p>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex ParagraphTag();

    [GeneratedRegex(@"<br\s*/?>", RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 2000)]
    private static partial Regex LineBreak();

    [GeneratedRegex(@"<li[^>]*>(.*?)</li>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex ListItemTag();

    [GeneratedRegex(@"</?(?:ul|ol)[^>]*>", RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 2000)]
    private static partial Regex ListWrapperTag();

    [GeneratedRegex(@"</?(?:table|thead|tbody|tfoot|tr|th|td)[^>]*>", RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 2000)]
    private static partial Regex TableTag();

    // ── Inline elements ───────────────────────────────────────────────────────

    [GeneratedRegex(@"<(?:strong|b)[^>]*>(.*?)</(?:strong|b)>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex StrongTag();

    [GeneratedRegex(@"<(?:em|i)[^>]*>(.*?)</(?:em|i)>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex EmTag();

    [GeneratedRegex(@"<code[^>]*>(.*?)</code>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex InlineCode();

    // Links — double-quoted href
    [GeneratedRegex(@"<a\s+[^>]*href=""([^""]*)""\s*[^>]*>(.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex AnchorDoubleQuote();

    // Links — single-quoted href
    [GeneratedRegex(@"<a\s+[^>]*href='([^']*)'\s*[^>]*>(.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex AnchorSingleQuote();

    // Bare anchor tags (no href or already converted) - strip them
    [GeneratedRegex(@"<a[^>]*>|</a>", RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 2000)]
    private static partial Regex BareAnchor();

    // Images with src + alt (attribute order independent, double or single quotes)
    [GeneratedRegex(@"<img\s+(?=[^>]*\bsrc=)(?=[^>]*\balt=)[^>]*\bsrc=[""']([^""']*)[""'][^>]*\balt=[""']([^""']*)[""'][^>]*/?>",
        RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 2000)]
    private static partial Regex ImgWithSrcAndAlt();

    // Images with alt before src
    [GeneratedRegex(@"<img\s+(?=[^>]*\balt=)(?=[^>]*\bsrc=)[^>]*\balt=[""']([^""']*)[""'][^>]*\bsrc=[""']([^""']*)[""'][^>]*/?>",
        RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 2000)]
    private static partial Regex ImgWithSrcAndAltReversed();

    // Images with data-src (lazy loading) + alt — prefer canonical src fallback first, then data-src
    [GeneratedRegex(
        @"<img\s+(?=[^>]*\b(?:data-src|data-lazy-src|data-original)=)(?=[^>]*\balt=)[^>]*\b(?:data-src|data-lazy-src|data-original)=[""']([^""']*)[""'][^>]*\balt=[""']([^""']*)[""'][^>]*/?>",
        RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 2000)]
    private static partial Regex ImgWithDataSrcAndAlt();

    // Images with src only (no alt), double or single quotes
    [GeneratedRegex(@"<img\s+[^>]*\bsrc=[""']([^""']*)[""'][^>]*/?>",
        RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 2000)]
    private static partial Regex ImgWithSrcNoAlt();

    // Images with data-src only (no alt)
    [GeneratedRegex(@"<img\s+[^>]*\b(?:data-src|data-lazy-src|data-original)=[""']([^""']*)[""'][^>]*/?>",
        RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 2000)]
    private static partial Regex ImgWithDataSrcNoAlt();

    // Remaining unmatched img tags
    [GeneratedRegex(@"<img[^>]*/?>", RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 2000)]
    private static partial Regex BareImg();

    // ── Catch-all + cleanup ───────────────────────────────────────────────────

    [GeneratedRegex(@"<[^>]+>", RegexOptions.None, matchTimeoutMilliseconds: 2000)]
    private static partial Regex AnyHtmlTag();

    [GeneratedRegex(@"\n{3,}", RegexOptions.None, matchTimeoutMilliseconds: 2000)]
    private static partial Regex ExcessiveNewlines();

    [GeneratedRegex(@"[ \t]{2,}", RegexOptions.None, matchTimeoutMilliseconds: 2000)]
    private static partial Regex ExcessiveSpaces();
}
