namespace TechHub.Core.Models;

/// <summary>
/// Request for facet counts (used separately from search for dynamic filtering).
/// </summary>
public record FacetRequest
{
    /// <summary>
    /// Current filters to scope facet counts.
    /// Facets show counts within this filtered scope.
    /// </summary>
    public IReadOnlyList<string>? Tags { get; init; }
    public IReadOnlyList<string>? Sections { get; init; }
    public IReadOnlyList<string>? Collections { get; init; }
    public DateTimeOffset? DateFrom { get; init; }
    public DateTimeOffset? DateTo { get; init; }

    /// <summary>
    /// Which facets to compute.
    /// Options: "tags", "collections", "sections", "authors"
    /// </summary>
    public required IReadOnlyList<string> FacetFields { get; init; }

    /// <summary>
    /// Maximum number of facet values to return per field.
    /// Default: 50 for tags, 20 for others.
    /// </summary>
    public int MaxFacetValues { get; init; } = 50;
}
