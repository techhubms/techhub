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
        // Configure Markdig pipeline with comprehensive GitHub Flavored Markdown extensions
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
    /// <returns>Rendered HTML</returns>
    public string RenderToHtml(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            return string.Empty;
        }

        return Markdown.ToHtml(markdown, _pipeline);
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
    /// Converts [YouTube: VIDEO_ID] tags to iframe embeds
    /// </summary>
    /// <param name="html">HTML content that may contain YouTube embed markers</param>
    /// <returns>HTML with YouTube embeds replaced with iframe HTML</returns>
    public string ProcessYouTubeEmbeds(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return string.Empty;
        }

        // Match pattern: [YouTube: VIDEO_ID] or [youtube: VIDEO_ID]
        var pattern = @"\[YouTube:\s*([a-zA-Z0-9_-]+)\]";

        return System.Text.RegularExpressions.Regex.Replace(
            html,
            pattern,
            match =>
            {
                var videoId = match.Groups[1].Value;
                return $"""
                    <div class="video-container">
                        <iframe
                            src="https://www.youtube.com/embed/{videoId}"
                            title="YouTube video player"
                            frameborder="0"
                            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                            allowfullscreen>
                        </iframe>
                    </div>
                    """;
            },
            System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );
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
