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
        AdminContentLinkResolver.LinksExternally(CollectionName);

    public string GetHref()
    {
        return AdminContentLinkResolver.GetHref(
            CollectionName,
            ItemSlug,
            ItemExternalUrl ?? string.Empty,
            ItemPrimarySectionName);
    }
}
