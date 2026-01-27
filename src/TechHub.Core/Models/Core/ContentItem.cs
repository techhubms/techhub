using System.Text.Json;

namespace TechHub.Core.Models;

/// <summary>
/// Content item model - used for both list views and detail views.
/// For list views: RenderedHtml and SidebarInfo will be null.
/// For detail views: RenderedHtml is required (access throws if null).
/// </summary>
public record ContentItem
{
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public string? Author { get; init; }
    public required long DateEpoch { get; init; }
    public required string DateIso { get; init; }
    public required string CollectionName { get; init; }
    public string? SubcollectionName { get; init; }
    public string? FeedName { get; init; }
    public required IReadOnlyList<string> SectionNames { get; init; }
    public required string PrimarySectionName { get; init; }
    public required IReadOnlyList<string> Tags { get; init; }
    public required string Excerpt { get; init; }
    public string? ExternalUrl { get; init; }
    public required string Url { get; init; }

    /// <summary>
    /// Full rendered HTML content (only populated for detail views).
    /// Throws InvalidOperationException if accessed when null.
    /// </summary>
    private string? _renderedHtml;
    public string RenderedHtml
    {
        get => _renderedHtml ?? throw new InvalidOperationException("RenderedHtml is only available for detail views. This content item was loaded as a summary.");
        init => _renderedHtml = value;
    }

    /// <summary>
    /// Dynamic sidebar info as JSON (from 'sidebar-info' frontmatter field).
    /// Only populated for detail views.
    /// </summary>
    public JsonElement? SidebarInfo { get; init; }

    /// <summary>
    /// GitHub Copilot subscription plans this feature is available in (e.g., "Free", "Pro", "Business", "Pro+", "Enterprise")
    /// Used for filtering features by plan tier on the Features page
    /// </summary>
    public IReadOnlyList<string> Plans { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Indicates whether this feature is available in GitHub Enterprise Server (GHES)
    /// Used for filtering features with GHES support
    /// </summary>
    public bool GhesSupport { get; init; }

    /// <summary>
    /// Indicates whether this content is a draft (not yet released)
    /// Draft content shows as "Coming Soon" on the Features page
    /// </summary>
    public bool Draft { get; init; }

    /// <summary>
    /// Indicates whether this is a GitHub Copilot feature (from ghc-features subcollection)
    /// Used to identify content that should appear on the GitHub Copilot Features page
    /// </summary>
    public bool GhcFeature { get; init; }

    /// <summary>
    /// Determines if this item links to an external source (vs linking internally to our site).
    /// News, blogs, and community items redirect to the original source.
    /// Videos and roundups (and custom pages) link internally since we can present them on our site.
    /// </summary>
    public bool LinksExternally() =>
        CollectionName is "news" or "blogs" or "community";

    /// <summary>
    /// Gets the target URL for links (external URL for items that link externally, internal URL otherwise)
    /// </summary>
    public string GetHref() =>
        LinksExternally() ? ExternalUrl ?? "" : Url;

    /// <summary>
    /// Gets the link target attribute (opens in new tab for items that link externally)
    /// </summary>
    public string? GetTarget() =>
        LinksExternally() ? "_blank" : null;

    /// <summary>
    /// Gets the link rel attribute (security attributes for items that link externally)
    /// </summary>
    public string? GetRel() =>
        LinksExternally() ? "noopener noreferrer" : null;

    /// <summary>
    /// Gets the aria-label for accessibility
    /// </summary>
    public string GetAriaLabel() =>
        LinksExternally() ? $"{Title} - opens in new tab" : Title;

    /// <summary>
    /// Gets the data-enhance-nav attribute for Blazor navigation enhancement (only for internal links)
    /// </summary>
    public string? GetDataEnhanceNav() =>
        LinksExternally() ? null : "true";

    /// <summary>
    /// Computed property: Date as DateTime (UTC)
    /// </summary>
    public DateTime DateUtc => DateTimeOffset.FromUnixTimeSeconds(DateEpoch).UtcDateTime;

    /// <summary>
    /// Validates that all required properties are correctly formatted (for detail views)
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

        if (_renderedHtml != null && string.IsNullOrWhiteSpace(_renderedHtml))
        {
            throw new ArgumentException("Rendered HTML cannot be empty when set", nameof(RenderedHtml));
        }

        if (Excerpt.Length > 1000)
        {
            throw new ArgumentException("Excerpt should not exceed 1000 characters", nameof(Excerpt));
        }
    }

    /// <summary>
    /// Determines the primary section URL based on section names and collection.
    /// Returns the PrimarySectionName property which is populated from frontmatter.
    /// </summary>
    public string GetPrimarySectionUrl()
    {
        return PrimarySectionName;
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
