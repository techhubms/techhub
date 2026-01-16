namespace TechHub.Core.DTOs;

/// <summary>
/// Tag with usage count information
/// </summary>
public record TagWithCount
{
    /// <summary>
    /// Tag name
    /// </summary>
    public required string Tag { get; init; }

    /// <summary>
    /// Number of content items with this tag
    /// </summary>
    public required int Count { get; init; }
}
