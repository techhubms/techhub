using Markdig;
using TechHub.Core.Interfaces;
using TechHub.Infrastructure.Markdown;

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
    /// Convert markdown to HTML with link rewriting
    /// </summary>
    /// <param name="markdown">Raw markdown content</param>
    /// <param name="currentPagePath">Current page path for fixing hash links</param>
    /// <param name="sectionName">Section name for internal link rewriting</param>
    /// <param name="collectionName">Collection name for internal link rewriting</param>
    /// <returns>Rendered HTML with properly formatted links</returns>
    public string RenderToHtml(string markdown, string? currentPagePath = null, string? sectionName = null, string? collectionName = null)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            return string.Empty;
        }

        // If context provided, create a custom pipeline with link rewriter
        if (!string.IsNullOrEmpty(currentPagePath) || !string.IsNullOrEmpty(sectionName))
        {
            var customPipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseEmojiAndSmiley()
                .UseYamlFrontMatter()
                .UseAutoLinks()
                .UsePipeTables()
                .UseGridTables()
                .UseListExtras()
                .UseCitations()
                .UseCustomContainers()
                .UseGenericAttributes()
                .UseAutoIdentifiers()
                .UseTaskLists()
                .UseMediaLinks()
                .UseSmartyPants()
                .UseBootstrap()
                .UseDiagrams()
                .UseMathematics()
                .UseFigures()
                .Use(new LinkRewriterExtension(currentPagePath, sectionName, collectionName))
                .Build();

            return Markdig.Markdown.ToHtml(markdown, customPipeline);
        }

        // Use default pipeline without link rewriting
        return Markdig.Markdown.ToHtml(markdown, _pipeline);
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
    /// - {% youtube VIDEO_ID %} - Jekyll/Liquid tag format
    /// </summary>
    /// <param name="html">HTML content that may contain YouTube embed markers</param>
    /// <returns>HTML with YouTube embeds replaced with iframe HTML</returns>
    public string ProcessYouTubeEmbeds(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return string.Empty;
        }

        // Helper to create YouTube embed HTML
        static string CreateYouTubeEmbed(string videoId) => $"""
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

        // Pattern 1: [YouTube: VIDEO_ID] or [youtube: VIDEO_ID]
        var markdownPattern = @"\[YouTube:\s*([a-zA-Z0-9_-]+)\]";
        html = System.Text.RegularExpressions.Regex.Replace(
            html,
            markdownPattern,
            match => CreateYouTubeEmbed(match.Groups[1].Value),
            System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        // Pattern 2: {% youtube VIDEO_ID %} - Jekyll/Liquid tag format
        var jekyllPattern = @"\{%\s*youtube\s+([a-zA-Z0-9_-]+)\s*%\}";
        html = System.Text.RegularExpressions.Regex.Replace(
            html,
            jekyllPattern,
            match => CreateYouTubeEmbed(match.Groups[1].Value),
            System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        return html;
    }

    /// <summary>
    /// Process Jekyll/Liquid template variables like {{ page.variable }}
    /// Replaces them with values from frontmatter
    /// </summary>
    /// <param name="content">Markdown content with Jekyll variables</param>
    /// <param name="frontMatter">Frontmatter dictionary with values</param>
    /// <returns>Content with variables replaced</returns>
    public string ProcessJekyllVariables(string content, Dictionary<string, object> frontMatter)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return content ?? string.Empty;
        }

        // Step 1: Remove {% raw %} and {% endraw %} tags (used to escape GitHub Actions syntax)
        content = System.Text.RegularExpressions.Regex.Replace(
            content,
            @"\{%\s*(?:raw|endraw)\s*%\}",
            string.Empty,
            System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        // Step 2: Process relative_url filter: {{ "/path" | relative_url }} -> /path
        content = System.Text.RegularExpressions.Regex.Replace(
            content,
            @"\{\{\s*""([^""]+)""\s*\|\s*relative_url\s*\}\}",
            match => match.Groups[1].Value,
            System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        // Step 3: Process page variables if frontmatter is provided
        if (frontMatter == null || frontMatter.Count == 0)
        {
            return content;
        }

        // Step 4: Expand page.variable inside Jekyll tags: {% youtube page.youtube_id %} -> {% youtube actual_id %}
        content = System.Text.RegularExpressions.Regex.Replace(
            content,
            @"(\{%\s*\w+\s+)page\.(\w+)(\s*%\})",
            match =>
            {
                var variableName = match.Groups[2].Value;
                var value = LookupFrontMatterValue(variableName, frontMatter);
                return $"{match.Groups[1].Value}{value}{match.Groups[3].Value}";
            },
            System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        // Step 5: Match patterns like {{ page.variable }} or {{ page.variable_name }}
        // ONLY match page.variable pattern (not standalone {{ variable }})
        var pattern = @"\{\{\s*page\.(\w+)\s*\}\}";

        return System.Text.RegularExpressions.Regex.Replace(
            content,
            pattern,
            match => LookupFrontMatterValue(match.Groups[1].Value, frontMatter),
            System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );
    }

    /// <summary>
    /// Convert frontmatter value to string representation
    /// </summary>
    private static string ConvertToString(object value)
    {
        return value switch
        {
            null => string.Empty,
            string s => s,
            IEnumerable<object> list => string.Join(", ", list),
            _ => value.ToString() ?? string.Empty
        };
    }

    /// <summary>
    /// Look up a variable name in frontmatter (case-insensitive)
    /// Throws if not found to catch content errors early
    /// </summary>
    private static string LookupFrontMatterValue(string variableName, Dictionary<string, object> frontMatter)
    {
        // Try exact match first
        if (frontMatter.TryGetValue(variableName, out var value))
        {
            return ConvertToString(value);
        }

        // Try case-insensitive match
        var key = frontMatter.Keys.FirstOrDefault(k =>
            string.Equals(k, variableName, StringComparison.OrdinalIgnoreCase));

        if (key != null && frontMatter.TryGetValue(key, out var caseInsensitiveValue))
        {
            return ConvertToString(caseInsensitiveValue);
        }

        // Variable not found - throw exception to catch content errors early
        throw new InvalidOperationException(
            $"Jekyll variable 'page.{variableName}' not found in frontmatter. " +
            $"Available keys: [{string.Join(", ", frontMatter.Keys)}]");
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
