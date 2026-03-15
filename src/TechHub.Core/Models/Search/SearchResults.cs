namespace TechHub.Core.Models;

/// <summary>
/// Search results with items, facets, and pagination.
/// </summary>
public record SearchResults<T>
{
    /// <summary>
    /// Content items matching the search criteria.
    /// </summary>
    public required IReadOnlyList<T> Items { get; init; }

    /// <summary>
    /// Total count of items matching the filters (before pagination).
    /// Used for displaying "Showing 1-20 of 4,013 results".
    /// </summary>
    public required long TotalCount { get; init; }

    /// <summary>
    /// Facet counts for dynamic filtering.
    /// Shows how many items would match if user applies additional filters.
    /// Null when IncludeFacets=false in the request (default).
    /// </summary>
    public FacetResults? Facets { get; init; }

    /// <summary>
    /// Continuation token for next page (keyset pagination).
    /// Null if this is the last page.
    /// </summary>
    public string? ContinuationToken { get; init; }
}
