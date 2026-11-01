namespace TechHub.Core.Models;

/// <summary>
/// A feature of GitHub Copilot as shown on the /github-copilot/features timeline: a feature of the product catalog
/// (<c>product_features</c>) with the page data of the feature. It is NOT a <see cref="ContentItem"/>.
/// One feature can reference zero or more content items (videos, blog posts) through
/// <see cref="ContentLinks"/>, with one optional link marked as the thumbnail.
/// </summary>
public record GhcFeature
{
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Release date as a Unix epoch seconds timestamp (UTC): the date set by an admin, else the first launch, preview, ga or
    /// release date of the timeline events of the feature.
    /// Display-timezone conversion (e.g. to Europe/Brussels) is a separate UI concern.
    /// Null when no date is known.
    /// </summary>
    public long? ReleaseDate { get; init; }

    /// <summary>
    /// Content items (videos, blog posts, etc.) associated with this feature.
    /// Populated only in joined queries; empty list by default.
    /// </summary>
    public IReadOnlyList<GhcFeatureContentLink> ContentLinks { get; init; } = [];

    /// <summary>
    /// Convenience accessor for the thumbnail link, if one has been designated.
    /// </summary>
    public GhcFeatureContentLink? ThumbnailLink =>
        ContentLinks.FirstOrDefault(l => l.IsThumbnail);
}
