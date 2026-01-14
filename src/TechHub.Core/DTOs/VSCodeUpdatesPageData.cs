namespace TechHub.Core.DTOs;

/// <summary>
/// Data structure for the Visual Studio Code Updates page
/// </summary>
public record VSCodeUpdatesPageData
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required List<string> Intro { get; init; }
    public required string VideoCollection { get; init; }
}
