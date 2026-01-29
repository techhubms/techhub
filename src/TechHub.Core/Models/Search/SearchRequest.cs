namespace TechHub.Core.Models;

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
    /// Required - no default. API layer must set this explicitly.
    /// </summary>
    public required int Take { get; init; }

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

    /// <summary>
    /// Whether to include facet aggregations in the response.
    /// Default: false (skip expensive facet computation for simple queries).
    /// Set to true when building a search page with faceted navigation.
    /// </summary>
    public bool IncludeFacets { get; init; }
}
