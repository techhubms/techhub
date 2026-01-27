namespace TechHub.Core.DTOs;

/// <summary>
/// Data structure for the AI SDLC page
/// </summary>
public record SDLCPageData
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Intro { get; init; }
    public required List<SDLCPhase> Phases { get; init; }
    public required List<SDLCPrecondition> Preconditions { get; init; }
    public required SDLCAdditionalInfo AdditionalInfo { get; init; }
}

public record SDLCPhase
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Icon { get; init; }
    public required string What { get; init; }
    public string? How { get; init; }
    public required List<SDLCTool> Tools { get; init; }
    public required SDLCAIEnhancements AIEnhancements { get; init; }
    public string? Handover { get; init; }
    public required string BestPractices { get; init; }
    public List<TestingType>? TestingTypes { get; init; }
}

public record SDLCTool
{
    public required string Name { get; init; }
    public required string Url { get; init; }
}

public record SDLCAIEnhancements
{
    public required string Intro { get; init; }
    public required List<SDLCRoleEnhancement> Roles { get; init; }
}

public record SDLCRoleEnhancement
{
    public required string Role { get; init; }
    public required string Content { get; init; }
}

public record TestingType
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}

public record SDLCPrecondition
{
    public required string Id { get; init; }
    public required string Icon { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required List<string> Actions { get; init; }
    public string? Example { get; init; }
    public string? Why { get; init; }
}

public record SDLCAdditionalInfo
{
    public required List<SDLCBenefit> Benefits { get; init; }
    public required SDLCMetrics Metrics { get; init; }
    public required List<SDLCChallenge> Challenges { get; init; }
    public required List<SDLCMethodology> Methodologies { get; init; }
}

public record SDLCBenefit
{
    public required string Icon { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
}

public record SDLCMetrics
{
    public required string Intro { get; init; }
    public required List<MetricsFramework> Frameworks { get; init; }
    public required SeeAlsoLink SeeAlso { get; init; }
}

public record MetricsFramework
{
    public required string Name { get; init; }
    public required string Icon { get; init; }
    public required string Description { get; init; }
}

public record SeeAlsoLink
{
    public required string Text { get; init; }
    public required string Url { get; init; }
}

public record SDLCChallenge
{
    public required string Title { get; init; }
    public required string Description { get; init; }
}

public record SDLCMethodology
{
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string Description { get; init; }
    public required string BestFor { get; init; }
    public required string CycleTime { get; init; }
    public required string Flexibility { get; init; }
}
