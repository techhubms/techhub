namespace TechHub.Core.Models;

/// <summary>
/// Facet results with counts for each filter value.
/// </summary>
public record FacetResults
{
    /// <summary>
    /// Facet counts grouped by field name.
    /// Key: "tags", "collections", "sections", "authors"
    /// Value: List of (value, count) pairs
    /// </summary>
    public required IReadOnlyDictionary<string, IReadOnlyList<FacetValue>> Facets { get; init; }

    /// <summary>
    /// Total count of items in the current filtered scope.
    /// </summary>
    public required long TotalCount { get; init; }
}
