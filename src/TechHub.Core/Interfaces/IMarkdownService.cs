namespace TechHub.Core.Interfaces;

/// <summary>
/// Service for parsing and rendering markdown content
/// </summary>
public interface IMarkdownService
{
    /// <summary>
    /// Parse YAML frontmatter from markdown file
    /// </summary>
    Task<Dictionary<string, object>> ParseFrontMatterAsync(
        string filePath,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Render markdown content to HTML
    /// </summary>
    string RenderToHtml(string markdown);

    /// <summary>
    /// Extract excerpt from markdown (content before &lt;!--excerpt_end--&gt; marker)
    /// </summary>
    string ExtractExcerpt(string markdown, int maxLength = 1000);

    /// <summary>
    /// Process YouTube embeds in HTML content
    /// </summary>
    string ProcessYouTubeEmbeds(string html);
}
