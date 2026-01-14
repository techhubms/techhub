namespace TechHub.Core.DTOs;

/// <summary>
/// Data structure for the GenAI Applied page
/// </summary>
public record GenAIAppliedPageData
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required List<string> Categories { get; init; }
    public required List<string> Tags { get; init; }
    public required string Intro { get; init; }
    public required List<RelatedPage> RelatedPages { get; init; }
    public required List<TableOfContentsItem> TableOfContents { get; init; }
    public required List<GenAIAppliedSection> Sections { get; init; }
    public required NavigationLink PreviousPage { get; init; }
}

public record GenAIAppliedSection
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public string? Intro { get; init; }
    public List<string>? LearningPath { get; init; }
    public List<StarterProject>? Projects { get; init; }
    public List<IntegrationLayer>? Layers { get; init; }
    public List<DevTool>? Tools { get; init; }
    public List<string>? Capabilities { get; init; }
    public List<CopilotMode>? Modes { get; init; }
    public List<string>? Customization { get; init; }
    public List<GenAIFaq>? Faq { get; init; }
}

public record StarterProject
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public ProjectLink? Link { get; init; }
}

public record ProjectLink
{
    public required string Text { get; init; }
    public required string Url { get; init; }
}

public record IntegrationLayer
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}

public record DevTool
{
    public required string Name { get; init; }
    public required List<string> Features { get; init; }
}

public record CopilotMode
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}
