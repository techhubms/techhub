namespace TechHub.Core.DTOs;

/// <summary>
/// Request DTO for content filtering operations
/// </summary>
public record FilterRequest
{
    /// <summary>
    /// Selected tags for filtering (OR logic within tags)
    /// </summary>
    public IReadOnlyList<string> SelectedTags { get; init; } = [];

    /// <summary>
    /// Start of date range filter (inclusive)
    /// </summary>
    public DateTimeOffset? DateFrom { get; init; }

    /// <summary>
    /// End of date range filter (inclusive)
    /// </summary>
    public DateTimeOffset? DateTo { get; init; }

    /// <summary>
    /// Section scope for filtering (optional)
    /// </summary>
    public string? SectionName { get; init; }

    /// <summary>
    /// Collection scope for filtering (optional)
    /// </summary>
    public string? CollectionName { get; init; }
}
