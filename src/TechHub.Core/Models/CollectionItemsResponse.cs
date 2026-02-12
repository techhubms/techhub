namespace TechHub.Core.Models;

/// <summary>
/// Response DTO for collection items endpoint, includes items and total count.
/// </summary>
public record CollectionItemsResponse(
    IReadOnlyList<ContentItem> Items,
    long TotalCount
);
