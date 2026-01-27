using System.Text.Json;

namespace TechHub.Core.DTOs;

/// <summary>
/// Search request parameters for filtering, searching, and pagination.
/// </summary>
public record SearchRequest
{
    /// <summary>
    /// Full-text search query (searches title, excerpt, content).
    /// Null or empty means no text search filter.
    /// </summary>
    public string? Query { get; init; }
    
    /// <summary>
    /// Filter by tags (AND logic - all tags must match).
    /// Supports subset matching: "AI" matches "AI", "Azure AI", "Generative AI".
    /// Null or empty means no tag filter.
    /// </summary>
    public IReadOnlyList<string>? Tags { get; init; }
    
    /// <summary>
    /// Filter by sections (OR logic - match any section).
    /// Null or empty means no section filter.
    /// </summary>
    public IReadOnlyList<string>? Sections { get; init; }
    
    /// <summary>
    /// Filter by collections (OR logic - match any collection).
    /// Null or empty means no collection filter.
    /// </summary>
    public IReadOnlyList<string>? Collections { get; init; }
    
    /// <summary>
    /// Filter by date range start (inclusive).
    /// Null means no start date filter.
    /// </summary>
    public DateTimeOffset? DateFrom { get; init; }
    
    /// <summary>
    /// Filter by date range end (inclusive).
    /// Null means no end date filter.
    /// </summary>
    public DateTimeOffset? DateTo { get; init; }
    
    /// <summary>
    /// Use semantic/vector search (Phase 2 feature).
    /// False uses PostgreSQL full-text search (Phase 1).
    /// </summary>
    public bool UseSemanticSearch { get; init; }
    
    /// <summary>
    /// Number of items to return per page.
    /// Default: 20
    /// </summary>
    public int Take { get; init; } = 20;
    
    /// <summary>
    /// Sort order for results.
    /// Options: "date_desc" (default), "date_asc", "relevance" (for search queries)
    /// </summary>
    public string OrderBy { get; init; } = "date_desc";
    
    /// <summary>
    /// Continuation token for keyset pagination.
    /// Base64-encoded JSON containing (date_epoch, id) of last item.
    /// Null for first page.
    /// </summary>
    public string? ContinuationToken { get; init; }
}

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
    /// </summary>
    public required FacetResults Facets { get; init; }
    
    /// <summary>
    /// Continuation token for next page (keyset pagination).
    /// Null if this is the last page.
    /// </summary>
    public string? ContinuationToken { get; init; }
}

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

/// <summary>
/// A single facet value with its count.
/// </summary>
public record FacetValue
{
    /// <summary>
    /// Facet value (e.g., "Azure AI", "videos", "ai").
    /// </summary>
    public required string Value { get; init; }
    
    /// <summary>
    /// Number of items with this facet value in the current scope.
    /// </summary>
    public required long Count { get; init; }
}

/// <summary>
/// Keyset pagination cursor for stable, performant paging.
/// Encodes the sort key values of the last item on the page.
/// </summary>
public record PaginationCursor
{
    /// <summary>
    /// Unix timestamp of the last item (primary sort key).
    /// </summary>
    public long DateEpoch { get; init; }
    
    /// <summary>
    /// ID/slug of the last item (tiebreaker for items with same date).
    /// </summary>
    public string Id { get; init; } = "";
    
    /// <summary>
    /// Encode cursor to Base64 string for URL transmission.
    /// </summary>
    public static string Encode(PaginationCursor cursor) =>
        Convert.ToBase64String(JsonSerializer.SerializeToUtf8Bytes(cursor));
    
    /// <summary>
    /// Decode cursor from Base64 string.
    /// Returns null if token is null or invalid.
    /// </summary>
    public static PaginationCursor? Decode(string? token)
    {
        if (string.IsNullOrEmpty(token))
            return null;
        
        try
        {
            return JsonSerializer.Deserialize<PaginationCursor>(
                Convert.FromBase64String(token));
        }
        catch
        {
            return null; // Invalid cursor - graceful degradation to first page
        }
    }
}
