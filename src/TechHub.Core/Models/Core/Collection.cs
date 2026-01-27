namespace TechHub.Core.Models;

/// <summary>
/// DTO for collection reference information
/// </summary>
public record Collection
{
    public required string Name { get; init; }
    public required string Title { get; init; }
    public required string Url { get; init; }
    public required string Description { get; init; }
    public required string DisplayName { get; init; }
    public bool IsCustom { get; init; }
}
