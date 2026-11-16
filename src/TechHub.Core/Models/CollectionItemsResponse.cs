namespace TechHub.Core.Models;

/// <summary>
/// Response DTO for collection items endpoint, includes items and total count.
/// <paramref name="CorrectedQuery"/> is the search that was run when the typed query contained a word that no article has and
/// it was replaced by a close one (<c>null</c> when the query was used as typed).
/// </summary>
public record CollectionItemsResponse(
    IReadOnlyList<ContentItem> Items,
    long TotalCount,
    string? CorrectedQuery = null
);
