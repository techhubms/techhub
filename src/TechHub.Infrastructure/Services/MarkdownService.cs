using Markdig;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Markdown rendering service using Markdig
/// Converts markdown content to HTML with GitHub Flavored Markdown support
/// </summary>
public class MarkdownService : IMarkdownService
{
    private readonly MarkdownPipeline _pipeline;

    public MarkdownService()
    {
        // Build single pipeline once - all Markdig extensions configured here
        // Link rewriting handled via preprocessing (before render) and post-processing (after render)
        _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()      // Tables, task lists, footnotes, definition lists, abbreviations, etc.
            .UseEmojiAndSmiley()          // :emoji: support
            .UseYamlFrontMatter()         // YAML frontmatter parsing
            .UseAutoLinks()               // Auto-convert URLs to links
            .UsePipeTables()              // GitHub-style tables
            .UseGridTables()              // Grid-style tables
            .UseListExtras()              // Extra list features
            .UseCitations()               // Citation support
            .UseCustomContainers()        // Custom containers/admonitions
            .UseGenericAttributes()       // CSS classes and IDs on elements
            .UseAutoIdentifiers()         // Auto-generate heading IDs for linking
            .UseTaskLists()               // GitHub-style task lists [x]
            .UseMediaLinks()              // Video and audio embeds
            .UseSmartyPants()             // Smart quotes, dashes, ellipses
            .UseBootstrap()               // Bootstrap CSS classes
            .UseDiagrams()                // Mermaid diagram support
            .UseMathematics()             // LaTeX math support
            .UseFigures()                 // Figure and figcaption elements
            .Build();
    }

    /// <summary>
    /// Parse YAML frontmatter from markdown file
    /// Reads file and extracts frontmatter metadata
    /// </summary>
    public async Task<Dictionary<string, object>> ParseFrontMatterAsync(
        string filePath,
        CancellationToken cancellationToken = default)
    {
        var content = await File.ReadAllTextAsync(filePath, cancellationToken);
        var parser = new FrontMatterParser();
        var (frontMatter, _) = parser.Parse(content);
        return frontMatter;
    }

    /// <summary>
    /// Convert markdown to HTML
    /// </summary>
    /// <param name="markdown">Raw markdown content</param>
    /// <param name="currentPagePath">Current page path for fixing hash-only links (needed for Blazor routing)</param>
    /// <returns>Rendered HTML with properly formatted links</returns>
    public string RenderToHtml(string markdown, string? currentPagePath = null)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            return string.Empty;
        }

        try
        {
            // Preprocess: Rewrite hash-only links for Blazor routing
            if (!string.IsNullOrEmpty(currentPagePath))
            {
                markdown = PreprocessMarkdownLinks(markdown, currentPagePath);
            }

            // Render markdown to HTML using shared pipeline
            var html = Markdig.Markdown.ToHtml(markdown, _pipeline);

            // Post-process: Add target="_blank" to external links
            html = AddTargetBlankToExternalLinks(html);

            return html;
        }
        catch (Exception ex)
        {
            // Add context to Markdig exceptions to help identify problematic content
            var preview = markdown.Length > 200 ? markdown[..200] + "..." : markdown;
            throw new InvalidOperationException(
                $"Failed to render markdown to HTML. CurrentPagePath: '{currentPagePath}'. Markdown preview: {preview}",
                ex);
        }
    }

    /// <summary>
    /// Preprocess markdown to rewrite hash-only links before rendering
    /// </summary>
    private static string PreprocessMarkdownLinks(string markdown, string? currentPagePath)
    {
        // Pattern to match markdown links: [text](url) or [text](url "title")
        var linkPattern = @"\[([^\]]+)\]\(([^\s)]+)(?:\s+""([^""]*)"")?\)";

        return System.Text.RegularExpressions.Regex.Replace(markdown, linkPattern, match =>
        {
            var text = match.Groups[1].Value;
            var url = match.Groups[2].Value;
            var title = match.Groups[3].Success ? match.Groups[3].Value : null;

            // Rewrite URL (only hash-only links)
            var rewrittenUrl = RewriteUrl(url, currentPagePath);

            // Rebuild markdown link
            var result = $"[{text}]({rewrittenUrl}";
            if (!string.IsNullOrEmpty(title))
            {
                result += $" \"{title}\"";
            }

            result += ")";

            return result;
        });
    }

    /// <summary>
    /// Rewrite hash-only links to include current page path (for Blazor routing)
    /// </summary>
    private static string RewriteUrl(string url, string? currentPagePath)
    {
        // Hash-only links (#heading) - prepend current page path
        if (url.StartsWith('#') && !string.IsNullOrEmpty(currentPagePath))
        {
            return $"{currentPagePath}{url}";
        }

        // Note: .html link rewriting is handled by ContentFixer at build time
        // ContentFixer looks up actual article location by slug and rewrites to /section/collection/slug
        // This ensures links always point to the correct section/collection

        // No changes needed
        return url;
    }

    /// <summary>
    /// Post-process HTML to add target="_blank" and rel="noopener noreferrer" to external links
    /// </summary>
    private static string AddTargetBlankToExternalLinks(string html)
    {
        // Pattern to match <a href="..."> tags
        var linkPattern = @"<a\s+href=""([^""]+)""([^>]*)>";

        return System.Text.RegularExpressions.Regex.Replace(html, linkPattern, match =>
        {
            var url = match.Groups[1].Value;
            var otherAttributes = match.Groups[2].Value;

            // Check if external link
            if (IsExternalLink(url))
            {
                // Add target and rel if not already present
                if (!otherAttributes.Contains("target=", StringComparison.Ordinal))
                {
                    otherAttributes += " target=\"_blank\" rel=\"noopener noreferrer\"";
                }
            }

            return $"<a href=\"{url}\"{otherAttributes}>";
        });
    }

    /// <summary>
    /// Check if a URL is external (different domain or protocol)
    /// </summary>
    private static bool IsExternalLink(string url)
    {
        if (string.IsNullOrEmpty(url))
            return false;

        // Hash-only links are internal
        if (url.StartsWith('#'))
            return false;

        // Relative paths are internal
        if (url.StartsWith('/'))
            return false;

        // Check if it has a protocol (http://, https://, mailto:, etc.)
        if (url.Contains("://", StringComparison.Ordinal))
        {
            // URLs with protocols are external unless they're our own domains
            var uri = new Uri(url, UriKind.Absolute);
            return uri.Host != "tech.hub.ms"
                && uri.Host != "tech.xebia.ms"
                && uri.Host != "localhost";
        }

        // Relative paths without / are internal
        return false;
    }

    /// <summary>
    /// Extract excerpt from markdown content
    /// Looks for <!--excerpt_end--> marker and returns content before it
    /// If marker not found, returns first paragraph or maxLength characters
    /// </summary>
    /// <param name="markdown">Raw markdown content</param>
    /// <param name="maxLength">Maximum excerpt length (default 1000)</param>
    /// <returns>Excerpt text (plain text, not HTML)</returns>
    public string ExtractExcerpt(string markdown, int maxLength = 1000)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            return string.Empty;
        }

        const string ExcerptMarker = "<!--excerpt_end-->";

        // If excerpt marker found, return content before it
        var markerIndex = markdown.IndexOf(ExcerptMarker, StringComparison.OrdinalIgnoreCase);
        if (markerIndex > 0)
        {
            var excerpt = markdown[..markerIndex].Trim();
            return StripMarkdownFormatting(excerpt);
        }

        // Fallback: First paragraph or 200 characters
        var paragraphs = markdown.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        var firstParagraph = paragraphs.FirstOrDefault()?.Trim() ?? string.Empty;

        var plainText = StripMarkdownFormatting(firstParagraph);

        // Limit to maxLength characters
        return plainText.Length > maxLength
            ? plainText[..maxLength].Trim() + "..."
            : plainText;
    }

    /// <summary>
    /// Process YouTube embeds in markdown
    /// Converts YouTube markers to iframe embeds. Supports multiple formats:
    /// - [YouTube: VIDEO_ID] - Custom markdown format
    /// - {% youtube VIDEO_ID %} - Template tag format
    /// </summary>
    /// <param name="html">HTML content that may contain YouTube embed markers</param>
    /// <returns>HTML with YouTube embeds replaced with iframe HTML</returns>
    public string ProcessYouTubeEmbeds(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return string.Empty;
        }

        // Helper to create YouTube embed HTML with privacy-enhanced mode
        static string CreateYouTubeEmbed(string videoId) => $"""
            <div class="video-container">
                <iframe
                    src="https://www.youtube-nocookie.com/embed/{videoId}"
                    title="YouTube video player"
                    frameborder="0"
                    allow="encrypted-media; picture-in-picture; web-share"
                    allowfullscreen>
                </iframe>
            </div>
            """;

        // Pattern 1: [YouTube: VIDEO_ID] or [youtube: VIDEO_ID]
        var markdownPattern = @"\[YouTube:\s*([a-zA-Z0-9_-]+)\]";
        html = System.Text.RegularExpressions.Regex.Replace(
            html,
            markdownPattern,
            match => CreateYouTubeEmbed(match.Groups[1].Value),
            System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        // Pattern 2: {% youtube VIDEO_ID %} - Template tag format
        var templatePattern = @"\{%\s*youtube\s+([a-zA-Z0-9_-]+)\s*%\}";
        html = System.Text.RegularExpressions.Regex.Replace(
            html,
            templatePattern,
            match => CreateYouTubeEmbed(match.Groups[1].Value),
            System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        return html;
    }

    /// <summary>
    /// Strip markdown formatting to get plain text
    /// Removes headers, links, bold, italic, code, etc.
    /// </summary>
    private static string StripMarkdownFormatting(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            return string.Empty;
        }

        // Remove headers (# ## ###)
        markdown = System.Text.RegularExpressions.Regex.Replace(markdown, @"^#+\s*", "", System.Text.RegularExpressions.RegexOptions.Multiline);

        // Remove links [text](url) -> text
        markdown = System.Text.RegularExpressions.Regex.Replace(markdown, @"\[([^\]]+)\]\([^\)]+\)", "$1");

        // Remove images ![alt](url)
        markdown = System.Text.RegularExpressions.Regex.Replace(markdown, @"!\[[^\]]*\]\([^\)]+\)", "");

        // Remove inline code `code`
        markdown = System.Text.RegularExpressions.Regex.Replace(markdown, @"`([^`]+)`", "$1");

        // Remove bold/italic **text** __text__ *text* _text_
        markdown = System.Text.RegularExpressions.Regex.Replace(markdown, @"(\*\*|__)(.*?)\1", "$2");
        markdown = System.Text.RegularExpressions.Regex.Replace(markdown, @"(\*|_)(.*?)\1", "$2");

        // Remove HTML tags
        markdown = System.Text.RegularExpressions.Regex.Replace(markdown, @"<[^>]+>", "");

        return markdown.Trim();
    }
}
