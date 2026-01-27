namespace TechHub.Core.Models;

/// <summary>
/// Response DTO containing filtered content items and metadata
/// </summary>
public record FilterResponse
{
    /// <summary>
    /// Filtered content items matching the criteria
    /// </summary>
    public required IReadOnlyList<ContentItem> Items { get; init; }

    /// <summary>
    /// Total count of filtered items
    /// </summary>
    public required int TotalCount { get; init; }

    /// <summary>
    /// Summary of applied filters for display
    /// </summary>
    public required FilterSummary AppliedFilters { get; init; }
}
