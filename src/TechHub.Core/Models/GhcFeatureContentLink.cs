namespace TechHub.Core.Models;

/// <summary>
/// A link from a <see cref="GhcFeature"/> to a <see cref="ContentItem"/>.
/// One feature can be associated with multiple content items (videos, blog posts, etc.).
/// </summary>
public record GhcFeatureContentLink
{
    public required string FeatureSlug { get; init; }
    public required string CollectionName { get; init; }
    public required string ItemSlug { get; init; }

    /// <summary>
    /// When true, the linked item's YouTube thumbnail is shown when the feature card is expanded.
    /// At most one link per feature may be the thumbnail.
    /// </summary>
    public bool IsThumbnail { get; init; }

    public int SortOrder { get; init; }

    // Denormalized from content_items for rendering — populated only in joined queries.
    public string? ItemTitle { get; init; }
    public string? ItemExternalUrl { get; init; }
    public string? ItemPrimarySectionName { get; init; }

    public bool LinksExternally() =>
        CollectionName is "news" or "blogs" or "community";

    public string GetHref()
    {
        if (LinksExternally())
        {
            return ItemExternalUrl ?? string.Empty;
        }

        if (CollectionName == "roundups")
        {
            return $"/all/roundups/{ItemSlug}".ToLowerInvariant();
        }

        if (!string.IsNullOrWhiteSpace(ItemPrimarySectionName))
        {
            return $"/{ItemPrimarySectionName}/{CollectionName}/{ItemSlug}".ToLowerInvariant();
        }

        return ItemExternalUrl ?? string.Empty;
    }
}
