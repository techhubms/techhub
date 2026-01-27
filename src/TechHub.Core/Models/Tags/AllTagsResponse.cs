namespace TechHub.Core.Models;

/// <summary>
/// Response containing all tags with their usage counts.
/// </summary>
public record AllTagsResponse
{
    /// <summary>
    /// All tags with their usage counts
    /// </summary>
    public required IReadOnlyList<TagWithCount> Tags { get; init; }

    /// <summary>
    /// Total number of unique tags
    /// </summary>
    public required int TotalCount { get; init; }
}
