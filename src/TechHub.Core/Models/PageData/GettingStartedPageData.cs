namespace TechHub.Core.Models;

/// <summary>
/// Data structure for the GitHub Copilot Getting Started guide page.
/// Sections contain markdown content that is rendered to HTML by the API.
/// </summary>
public record GettingStartedPageData
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required List<GettingStartedSection> Sections { get; init; }
}

public record GettingStartedSection
{
    public required string Title { get; init; }
    public required string Content { get; init; }
    public List<MoreInfoLink>? MoreInfo { get; init; }
}
