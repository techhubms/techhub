namespace TechHub.Core.DTOs;

/// <summary>
/// DTO for collection reference information
/// </summary>
public record CollectionReferenceDto
{
    public required string Title { get; init; }
    public required string Collection { get; init; }
    public required string Url { get; init; }
    public required string Description { get; init; }
    public bool IsCustom { get; init; }
}
