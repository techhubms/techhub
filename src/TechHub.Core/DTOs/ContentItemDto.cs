namespace TechHub.Core.DTOs;

/// <summary>
/// DTO for content item in list views (summary information)
/// </summary>
public record ContentItemDto
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
}
