namespace TechHub.Core.DTOs;

/// <summary>
/// DTO for section information returned from API
/// </summary>
public record SectionDto
{
    public required string Name { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Url { get; init; }
    public required IReadOnlyList<CollectionReferenceDto> Collections { get; init; }
}
