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
    /// Determines if this content is from an external collection (links to original source)
    /// </summary>
    public bool IsExternalCollection() =>
        CollectionName is "news" or "blogs" or "community";

    /// <summary>
    /// Gets the target URL for links (external URL for external collections, internal URL otherwise)
    /// </summary>
    public string GetHref() =>
        IsExternalCollection() ? ExternalUrl ?? "" : Url;

    /// <summary>
    /// Gets the link target attribute (opens in new tab for external collections)
    /// </summary>
    public string? GetTarget() =>
        IsExternalCollection() ? "_blank" : null;

    /// <summary>
    /// Gets the link rel attribute (security attributes for external links)
    /// </summary>
    public string? GetRel() =>
        IsExternalCollection() ? "noopener noreferrer" : null;

    /// <summary>
    /// Gets the aria-label for accessibility
    /// </summary>
    public string GetAriaLabel() =>
        IsExternalCollection() ? $"{Title} - opens in new tab" : Title;

    /// <summary>
    /// Gets the data-enhance-nav attribute for Blazor navigation enhancement
    /// </summary>
    public string? GetDataEnhanceNav() =>
        IsExternalCollection() ? null : "true";
}
