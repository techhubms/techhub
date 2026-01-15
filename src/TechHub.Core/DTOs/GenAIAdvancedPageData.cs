namespace TechHub.Core.DTOs;

/// <summary>
/// Data structure for the GenAI Advanced page
/// </summary>
public record GenAIAdvancedPageData
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required List<string> SectionNames { get; init; }
    public required List<string> Tags { get; init; }
    public required string Intro { get; init; }
    public required List<RelatedPage> RelatedPages { get; init; }
    public required List<TableOfContentsItem> TableOfContents { get; init; }
    public required List<GenAIAdvancedSection> Sections { get; init; }
    public required NavigationLink PreviousPage { get; init; }
    public required NavigationLink NextPage { get; init; }
}

public record GenAIAdvancedSection
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public string? Intro { get; init; }
    public List<string>? Content { get; init; }
    public List<GenAIAdvancedSubsection>? Subsections { get; init; }
    public List<FineTuningTechnique>? Techniques { get; init; }
    public List<string>? Workflow { get; init; }
    public List<string>? Benefits { get; init; }
    public List<string>? Characteristics { get; init; }
    public List<AgentPattern>? Patterns { get; init; }
    public List<string>? Metrics { get; init; }
    public List<string>? Considerations { get; init; }
    public List<MoreInfoLink>? MoreInfo { get; init; }
}

public record GenAIAdvancedSubsection
{
    public required string Title { get; init; }
    public required string Content { get; init; }
}

public record FineTuningTechnique
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public List<TemperatureSetting>? Settings { get; init; }
}

public record TemperatureSetting
{
    public required string Range { get; init; }
    public required string Description { get; init; }
}

public record AgentPattern
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}
