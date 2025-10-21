using System.Text.RegularExpressions;

namespace techhub.webapp.ApiService.Infrastructure.Parsers;

/// <summary>
/// Parses markdown content and extracts excerpts.
/// Handles Jekyll-style excerpt separators.
/// </summary>
public class MarkdownParser
{
    private static readonly Regex ExcerptSeparatorRegex = new(
        @"<!--excerpt_end-->",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    /// <summary>
    /// Extracts excerpt from markdown content.
    /// Excerpt is everything before the <!--excerpt_end--> marker.
    /// </summary>
    /// <param name="content">Markdown content</param>
    /// <returns>Excerpt text (without the separator)</returns>
    public string ExtractExcerpt(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return string.Empty;

        var match = ExcerptSeparatorRegex.Match(content);
        if (!match.Success)
            return string.Empty;

        var excerpt = content[..match.Index].Trim();
        return excerpt;
    }

    /// <summary>
    /// Gets the main content (after excerpt).
    /// </summary>
    /// <param name="content">Markdown content</param>
    /// <returns>Content after excerpt separator</returns>
    public string ExtractMainContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return string.Empty;

        var match = ExcerptSeparatorRegex.Match(content);
        if (!match.Success)
            return content.Trim();

        var mainContent = content[(match.Index + match.Length)..].Trim();
        return mainContent;
    }

    /// <summary>
    /// Strips all markdown formatting for plain text search.
    /// </summary>
    /// <param name="markdown">Markdown text</param>
    /// <returns>Plain text without markdown formatting</returns>
    public string StripMarkdown(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
            return string.Empty;

        var text = markdown;

        // Remove HTML tags
        text = Regex.Replace(text, @"<[^>]*>", string.Empty);

        // Remove markdown headers
        text = Regex.Replace(text, @"^#+\s+", string.Empty, RegexOptions.Multiline);

        // Remove bold/italic
        text = Regex.Replace(text, @"[*_]{1,3}", string.Empty);

        // Remove links but keep text
        text = Regex.Replace(text, @"\[([^\]]+)\]\([^\)]+\)", "$1");

        // Remove code blocks
        text = Regex.Replace(text, @"```[^```]*```", string.Empty, RegexOptions.Singleline);
        text = Regex.Replace(text, @"`[^`]+`", string.Empty);

        // Remove blockquotes
        text = Regex.Replace(text, @"^>\s+", string.Empty, RegexOptions.Multiline);

        // Normalize whitespace
        text = Regex.Replace(text, @"\s+", " ");

        return text.Trim();
    }

    /// <summary>
    /// Creates searchable content from markdown.
    /// Used for text search indexing.
    /// </summary>
    /// <param name="title">Content title</param>
    /// <param name="description">Content description</param>
    /// <param name="author">Content author</param>
    /// <param name="content">Markdown content</param>
    /// <param name="tags">Content tags</param>
    /// <returns>Combined searchable text</returns>
    public string CreateSearchableContent(
        string title,
        string description,
        string author,
        string content,
        IEnumerable<string> tags)
    {
        var parts = new List<string>
        {
            title,
            description,
            author,
            StripMarkdown(content),
            string.Join(" ", tags)
        };

        var searchable = string.Join(" ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
        return searchable.ToLowerInvariant();
    }
}
