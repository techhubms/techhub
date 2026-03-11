namespace TechHub.Core.Models;

/// <summary>
/// Data structure for the GitHub Copilot Tool Tips page
/// </summary>
public record ToolTipsPageData
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Intro { get; init; }
    public required IReadOnlyList<ToolTip> Tools { get; init; }
}

public record ToolTip
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Url { get; init; }
    public required string Category { get; init; }
}
