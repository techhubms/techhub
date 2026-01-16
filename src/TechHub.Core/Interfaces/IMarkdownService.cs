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
    /// Render markdown content to HTML with link rewriting
    /// </summary>
    /// <param name="markdown">Raw markdown content</param>
    /// <param name="currentPagePath">Current page path for fixing hash links (e.g., "/all/roundups/2025-12-29-weekly-ai-and-tech-news-roundup")</param>
    /// <param name="sectionName">Section name for internal link rewriting</param>
    /// <param name="collectionName">Collection name for internal link rewriting</param>
    /// <returns>Rendered HTML with properly formatted links</returns>
    string RenderToHtml(string markdown, string? currentPagePath = null, string? sectionName = null, string? collectionName = null);

    /// <summary>
    /// Extract excerpt from markdown (content before &lt;!--excerpt_end--&gt; marker)
    /// </summary>
    string ExtractExcerpt(string markdown, int maxLength = 1000);

    /// <summary>
    /// Process YouTube embeds in HTML content
    /// </summary>
    string ProcessYouTubeEmbeds(string html);

}
