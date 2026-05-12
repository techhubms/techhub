namespace TechHub.Core.Models;

/// <summary>
/// A GitHub Copilot feature entry shown on the /github-copilot/features timeline.
/// This is an independent entity — it is NOT a <see cref="ContentItem"/>.
/// One feature can reference zero or more content items (videos, blog posts) through
/// <see cref="ContentLinks"/>, with one optional link marked as the thumbnail.
/// </summary>
public record GhcFeature
{
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Optional release date as a Unix epoch seconds timestamp (UTC).
    /// Display-timezone conversion (e.g. to Europe/Brussels) is a separate UI concern.
    /// Null when the feature has been announced but not yet shipped.
    /// </summary>
    public long? ReleaseDate { get; init; }

    /// <summary>
    /// All GitHub Copilot subscription plans that include this feature.
    /// Stores every applicable tier explicitly: Free, Student, Pro, Business, Pro+, Enterprise.
    /// </summary>
    public IReadOnlyList<string> Plans { get; init; } = [];

    /// <summary>
    /// Whether this feature is available in GitHub Enterprise Server (GHES).
    /// </summary>
    public bool GhesSupport { get; init; }

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
