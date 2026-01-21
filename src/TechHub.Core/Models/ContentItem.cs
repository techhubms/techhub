namespace TechHub.Core.Models;

/// <summary>
/// Represents individual content item (article, blog, video, etc.) with metadata and rendered content
/// </summary>
public class ContentItem
{
    /// <summary>
    /// URL-friendly slug derived from filename (e.g., "2025-01-15-product-launch")
    /// </summary>
    public required string Slug { get; init; }

    /// <summary>
    /// Article title
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Optional author name
    /// </summary>
    public string? Author { get; init; }

    /// <summary>
    /// Unix timestamp for publication date (stored in UTC)
    /// </summary>
    public required long DateEpoch { get; init; }

    /// <summary>
    /// Collection name derived from parent directory
    /// (e.g., \"videos\" for all items in _videos/, \"news\" for all items in _news/)
    /// </summary>
    public required string CollectionName { get; init; }

    /// <summary>
    /// Optional subcollection name from subfolder organization
    /// (e.g., \"vscode-updates\" for _videos/vscode-updates/, null for _videos/ root files)
    /// Used by custom pages to filter specific video series or content groupings
    /// </summary>
    public string? SubcollectionName { get; init; }

    /// <summary>
    /// Optional RSS feed name this content was sourced from (e.g., "Microsoft Tech Community", "Azure Updates")
    /// Used for attribution and potential sidebar display
    /// </summary>
    public string? FeedName { get; init; }

    /// <summary>
    /// Section names (lowercase identifiers) this content belongs to (e.g., "ai", "github-copilot", "coding").
    /// Mapped from legacy 'categories' frontmatter field (converted to lowercase).
    /// When filtering content by section, match section.Name against this property.
    /// Values match Section.Name property (lowercase), NOT Section.Title (display name).
    /// </summary>
    public required IReadOnlyList<string> SectionNames { get; init; }

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
    /// Dynamic sidebar info as JSON (from 'sidebar-info' frontmatter field)
    /// Can be deserialized into custom structures as needed per page
    /// </summary>
    public System.Text.Json.JsonElement? SidebarInfo { get; init; }

    /// <summary>
    /// Computed property: Date as DateTime (UTC)
    /// </summary>
    public DateTime DateUtc => DateTimeOffset.FromUnixTimeSeconds(DateEpoch).UtcDateTime;

    /// <summary>
    /// Computed property: Date in ISO format (YYYY-MM-DD)
    /// </summary>
    public string DateIso => DateUtc.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

    /// <summary>
    /// Validates that all required properties are correctly formatted
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Slug))
        {
            throw new ArgumentException("Content slug cannot be empty", nameof(Slug));
        }

        if (string.IsNullOrWhiteSpace(Title))
        {
            throw new ArgumentException("Content title cannot be empty", nameof(Title));
        }

        if (DateEpoch <= 0)
        {
            throw new ArgumentException("Date epoch must be a valid Unix timestamp", nameof(DateEpoch));
        }

        if (SectionNames.Count == 0)
        {
            throw new ArgumentException("Content must have at least one section", nameof(SectionNames));
        }

        if (string.IsNullOrWhiteSpace(RenderedHtml))
        {
            throw new ArgumentException("Rendered HTML cannot be empty", nameof(RenderedHtml));
        }

        if (Excerpt.Length > 1000)
        {
            throw new ArgumentException("Excerpt should not exceed 1000 characters", nameof(Excerpt));
        }
    }

    /// <summary>
    /// Determines the primary section URL based on section names and collection
    /// </summary>
    public string GetPrimarySectionUrl()
    {
        return Helpers.SectionPriorityHelper.GetPrimarySectionUrl(SectionNames, CollectionName);
    }

    /// <summary>
    /// Gets the content item URL within a specific section
    /// </summary>
    /// <param name="sectionUrl">The section URL path (e.g., "ai", "github-copilot" or "/ai", "/github-copilot")</param>
    /// <returns>Full URL path for this item in the specified section</returns>
    public string GetUrlInSection(string sectionUrl)
    {
        ArgumentNullException.ThrowIfNull(sectionUrl);

        // Ensure section URL starts with slash
        var normalizedSectionUrl = sectionUrl.StartsWith('/') ? sectionUrl : $"/{sectionUrl}";

        // If item has a subcollection, use that for the URL path (e.g., /github-copilot/vscode-updates/slug)
        // Otherwise use the collection name (e.g., /github-copilot/videos/slug)
        var pathSegment = SubcollectionName ?? CollectionName;
        return $"{normalizedSectionUrl}/{pathSegment}/{Slug}";
    }
}
