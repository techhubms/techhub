namespace TechHub.Core.Models;

/// <summary>
/// Represents individual content item (article, blog, video, etc.) with metadata and rendered content
/// </summary>
public class ContentItem
{
    /// <summary>
    /// Unique identifier slug derived from filename
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Article title
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Brief summary/description
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Optional author name
    /// </summary>
    public string? Author { get; init; }

    /// <summary>
    /// Unix timestamp for publication date (stored in UTC)
    /// </summary>
    public required long DateEpoch { get; init; }

    /// <summary>
    /// Collection this item belongs to (e.g., "news", "blogs", "videos")
    /// </summary>
    public required string Collection { get; init; }

    /// <summary>
    /// Optional alt-collection for content organized in subfolders
    /// (e.g., "ghc-features" for _videos/ghc-features/)
    /// </summary>
    public string? AltCollection { get; init; }

    /// <summary>
    /// Categories this content belongs to (for section filtering)
    /// </summary>
    public required IReadOnlyList<string> Categories { get; init; }

    /// <summary>
    /// Normalized tags for filtering (lowercase, hyphen-separated)
    /// </summary>
    public required IReadOnlyList<string> Tags { get; init; }

    /// <summary>
    /// Full markdown content rendered to HTML
    /// </summary>
    public required string RenderedHtml { get; init; }

    /// <summary>
    /// Content excerpt (everything before &lt;!--excerpt_end--&gt; marker)
    /// </summary>
    public required string Excerpt { get; init; }

    /// <summary>
    /// Optional external link URL
    /// </summary>
    public string? ExternalUrl { get; init; }

    /// <summary>
    /// Optional YouTube video ID for video content
    /// </summary>
    public string? VideoId { get; init; }

    /// <summary>
    /// Computed property: Date as DateTime (UTC)
    /// </summary>
    public DateTime DateUtc => DateTimeOffset.FromUnixTimeSeconds(DateEpoch).UtcDateTime;

    /// <summary>
    /// Computed property: Date in ISO format (YYYY-MM-DD)
    /// </summary>
    public string DateIso => DateUtc.ToString("yyyy-MM-dd");

    /// <summary>
    /// Get the URL for this content item within a specific section
    /// </summary>
    public string GetUrlInSection(string sectionUrl) => $"{sectionUrl}/{Collection}/{Id}.html";

    /// <summary>
    /// Validates that all required properties are correctly formatted
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Id))
            throw new ArgumentException("Content ID cannot be empty", nameof(Id));

        if (string.IsNullOrWhiteSpace(Title))
            throw new ArgumentException("Content title cannot be empty", nameof(Title));

        if (DateEpoch <= 0)
            throw new ArgumentException("Date epoch must be a valid Unix timestamp", nameof(DateEpoch));

        if (Categories.Count == 0)
            throw new ArgumentException("Content must have at least one category", nameof(Categories));

        if (string.IsNullOrWhiteSpace(RenderedHtml))
            throw new ArgumentException("Rendered HTML cannot be empty", nameof(RenderedHtml));

        if (Excerpt.Length > 1000)
            throw new ArgumentException("Excerpt should not exceed 1000 characters", nameof(Excerpt));
    }
}
