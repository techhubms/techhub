namespace TechHub.Core.DTOs;

/// <summary>
/// Data structure for the GitHub Copilot Levels of Enlightenment page
/// </summary>
public record LevelsPageData
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Intro { get; init; }
    public required LevelsOverview Overview { get; init; }
    public required List<EnlightenmentLevel> Levels { get; init; }
    public required string Conclusion { get; init; }
    public required string PlaylistUrl { get; init; }
}

public record LevelsOverview
{
    public required string Content { get; init; }
    public required string ImageUrl { get; init; }
    public required string ImageAlt { get; init; }
}

public record EnlightenmentLevel
{
    public required int Number { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string YoutubeId { get; init; }
}
