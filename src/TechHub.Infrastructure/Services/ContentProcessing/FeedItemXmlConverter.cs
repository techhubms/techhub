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
    /// Delegates to <see cref="HtmlToMarkdownConverter"/> which is the shared implementation.
    /// </summary>
    internal static string CleanValue(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return string.Empty;
        }

        // If the raw text contains HTML tags, run it through the full converter.
        if (raw.Contains('<', StringComparison.Ordinal) && HtmlTagPattern().IsMatch(raw))
        {
            return HtmlToMarkdownConverter.Convert(raw);
        }

        // Plain text — decode entities and collapse excessive whitespace.
        var result = WebUtility.HtmlDecode(raw);
        result = ExcessiveNewlines().Replace(result, "\n\n");
        result = ExcessiveSpaces().Replace(result, " ");
        return result.Trim();
    }

    // HTML detection — kept here so CleanValue can short-circuit on plain text.
    [GeneratedRegex(@"<[a-zA-Z][^>]*>", RegexOptions.None, matchTimeoutMilliseconds: 2000)]
    private static partial Regex HtmlTagPattern();

    [GeneratedRegex(@"\n{3,}", RegexOptions.None, matchTimeoutMilliseconds: 2000)]
    private static partial Regex ExcessiveNewlines();

    [GeneratedRegex(@"[ \t]{2,}", RegexOptions.None, matchTimeoutMilliseconds: 2000)]
    private static partial Regex ExcessiveSpaces();
}
