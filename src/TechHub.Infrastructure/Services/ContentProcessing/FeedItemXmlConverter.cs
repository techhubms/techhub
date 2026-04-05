using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace TechHub.Infrastructure.Services.ContentProcessing;

/// <summary>
/// Converts an XML feed item/entry node into a compact key-value text representation
/// suitable for AI processing. HTML content is converted to lightweight markdown.
/// This avoids the token overhead of raw XML (closing tags, namespace declarations, etc.).
/// </summary>
internal static partial class FeedItemXmlConverter
{
    /// <summary>
    /// Converts an XML node (RSS <c>&lt;item&gt;</c> or Atom <c>&lt;entry&gt;</c>) into
    /// a compact, line-based representation. Each child element becomes a <c>name: value</c> line.
    /// HTML in values is converted to markdown. Attributes are included inline.
    /// </summary>
    internal static string ToCompactText(XmlNode itemNode)
    {
        ArgumentNullException.ThrowIfNull(itemNode);

        var sb = new StringBuilder();

        foreach (XmlNode child in itemNode.ChildNodes)
        {
            if (child.NodeType != XmlNodeType.Element)
            {
                continue;
            }

            var name = child.LocalName;
            var prefix = child.Prefix;
            var key = string.IsNullOrEmpty(prefix) ? name : $"{prefix}:{name}";

            // For elements with nested children (e.g. <author><name>X</name></author>,
            // <media:group><media:description>...</media:description></media:group>),
            // recurse into child elements as indented sub-keys.
            if (HasElementChildren(child))
            {
                AppendNestedElement(sb, child, key);
                continue;
            }

            // Collect attributes (except xmlns) for inline display
            var attrs = FormatAttributes(child);

            var rawValue = child.InnerText?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(rawValue) && string.IsNullOrEmpty(attrs))
            {
                continue;
            }

            // Decode HTML entities and convert HTML→markdown
            var value = CleanValue(rawValue);

            if (!string.IsNullOrEmpty(attrs))
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"{key} ({attrs}): {value}");
            }
            else
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"{key}: {value}");
            }
        }

        return sb.ToString().TrimEnd();
    }

    private static void AppendNestedElement(StringBuilder sb, XmlNode parent, string parentKey)
    {
        foreach (XmlNode child in parent.ChildNodes)
        {
            if (child.NodeType != XmlNodeType.Element)
            {
                continue;
            }

            var childPrefix = child.Prefix;
            var childName = string.IsNullOrEmpty(childPrefix) ? child.LocalName : $"{childPrefix}:{child.LocalName}";

            if (HasElementChildren(child))
            {
                AppendNestedElement(sb, child, $"{parentKey}/{childName}");
                continue;
            }

            var rawValue = child.InnerText?.Trim() ?? string.Empty;
            var attrs = FormatAttributes(child);

            if (string.IsNullOrEmpty(rawValue) && string.IsNullOrEmpty(attrs))
            {
                continue;
            }

            var value = CleanValue(rawValue);

            if (!string.IsNullOrEmpty(attrs))
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"{parentKey}/{childName} ({attrs}): {value}");
            }
            else
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"{parentKey}/{childName}: {value}");
            }
        }
    }

    private static bool HasElementChildren(XmlNode node)
    {
        foreach (XmlNode child in node.ChildNodes)
        {
            if (child.NodeType == XmlNodeType.Element)
            {
                return true;
            }
        }

        return false;
    }

    private static string FormatAttributes(XmlNode node)
    {
        if (node.Attributes == null || node.Attributes.Count == 0)
        {
            return string.Empty;
        }

        var parts = new List<string>();
        foreach (XmlAttribute attr in node.Attributes)
        {
            // Skip namespace declarations — they're just XML plumbing
            if (attr.Name.StartsWith("xmlns", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            parts.Add($"{attr.Name}={attr.Value}");
        }

        return string.Join(", ", parts);
    }

    /// <summary>
    /// Decodes HTML entities and converts common HTML to lightweight markdown.
    /// </summary>
    internal static string CleanValue(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return string.Empty;
        }

        // Decode HTML entities first (e.g. &lt;p&gt; → <p>)
        var decoded = WebUtility.HtmlDecode(raw);

        // If the decoded text contains HTML tags, convert to markdown
        if (decoded.Contains('<', StringComparison.Ordinal) && HtmlTagPattern().IsMatch(decoded))
        {
            decoded = HtmlToMarkdown(decoded);
        }

        // Collapse excessive whitespace/newlines
        decoded = ExcessiveNewlines().Replace(decoded, "\n\n");
        decoded = ExcessiveSpaces().Replace(decoded, " ");

        return decoded.Trim();
    }

    private static string HtmlToMarkdown(string html)
    {
        var result = html;

        // Block-level elements first (order matters)

        // Headings: <h1>...<h6>
        result = Heading1().Replace(result, "\n# $1\n");
        result = Heading2().Replace(result, "\n## $1\n");
        result = Heading3().Replace(result, "\n### $1\n");
        result = Heading4().Replace(result, "\n#### $1\n");
        result = Heading5().Replace(result, "\n##### $1\n");
        result = Heading6().Replace(result, "\n###### $1\n");

        // Paragraphs
        result = ParagraphTag().Replace(result, "\n\n$1\n\n");

        // Line breaks
        result = LineBreakTag().Replace(result, "\n");

        // List items
        result = ListItemTag().Replace(result, "\n- $1");

        // Unordered/ordered list wrappers (just remove tags, items are already converted)
        result = ListWrapperTag().Replace(result, string.Empty);

        // Blockquotes
        result = BlockquoteTag().Replace(result, "\n> $1\n");

        // Code blocks (pre > code)
        result = PreCodeTag().Replace(result, "\n```\n$1\n```\n");
        result = PreTag().Replace(result, "\n```\n$1\n```\n");

        // Inline elements
        result = StrongTag().Replace(result, "**$1**");
        result = EmTag().Replace(result, "*$1*");
        result = CodeTag().Replace(result, "`$1`");

        // Links: <a href="url">text</a>
        result = AnchorTag().Replace(result, "[$2]($1)");

        // Images: <img src="url" alt="text">
        result = ImgTag().Replace(result, "![$2]($1)");

        // Strip remaining HTML tags
        result = AnyHtmlTag().Replace(result, string.Empty);

        return result;
    }

    // HTML detection
    [GeneratedRegex(@"<[a-zA-Z][^>]*>", RegexOptions.None, matchTimeoutMilliseconds: 2000)]
    private static partial Regex HtmlTagPattern();

    // Block elements
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

    [GeneratedRegex(@"<p[^>]*>(.*?)</p>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex ParagraphTag();

    [GeneratedRegex(@"<br\s*/?>", RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 2000)]
    private static partial Regex LineBreakTag();

    [GeneratedRegex(@"<li[^>]*>(.*?)</li>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex ListItemTag();

    [GeneratedRegex(@"</?(?:ul|ol)[^>]*>", RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 2000)]
    private static partial Regex ListWrapperTag();

    [GeneratedRegex(@"<blockquote[^>]*>(.*?)</blockquote>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex BlockquoteTag();

    [GeneratedRegex(@"<pre[^>]*>\s*<code[^>]*>(.*?)</code>\s*</pre>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex PreCodeTag();

    [GeneratedRegex(@"<pre[^>]*>(.*?)</pre>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex PreTag();

    // Inline elements
    [GeneratedRegex(@"<(?:strong|b)[^>]*>(.*?)</(?:strong|b)>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex StrongTag();

    [GeneratedRegex(@"<(?:em|i)[^>]*>(.*?)</(?:em|i)>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex EmTag();

    [GeneratedRegex(@"<code[^>]*>(.*?)</code>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex CodeTag();

    [GeneratedRegex(@"<a\s+[^>]*href=""([^""]*)""[^>]*>(.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex AnchorTag();

    [GeneratedRegex(@"<img\s+[^>]*src=""([^""]*)""[^>]*alt=""([^""]*)""[^>]*/?>", RegexOptions.IgnoreCase | RegexOptions.Singleline, matchTimeoutMilliseconds: 2000)]
    private static partial Regex ImgTag();

    // Cleanup
    [GeneratedRegex(@"<[^>]+>", RegexOptions.None, matchTimeoutMilliseconds: 2000)]
    private static partial Regex AnyHtmlTag();

    [GeneratedRegex(@"\n{3,}", RegexOptions.None, matchTimeoutMilliseconds: 2000)]
    private static partial Regex ExcessiveNewlines();

    [GeneratedRegex(@"[ \t]{2,}", RegexOptions.None, matchTimeoutMilliseconds: 2000)]
    private static partial Regex ExcessiveSpaces();
}
