namespace TechHub.Core.DTOs;

/// <summary>
/// DTO for custom page list item returned from API
/// </summary>
public record CustomPageDto
{
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Url { get; init; }
    public required IReadOnlyList<string> SectionNames { get; init; }
}
