namespace TechHub.Core.Models;

/// <summary>
/// Unified data structure for all GenAI pages (Basics, Applied, Advanced).
/// JSON structure with markdown content and embedded diagrams.
/// </summary>
public record GenAIPageData
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required List<GenAIBasicsSection> Sections { get; init; }
}

/// <summary>
/// Section with markdown content, mermaid diagrams, FAQs, and resources.
/// Content field contains markdown with {{mermaid:id}} placeholders.
/// </summary>
public record GenAIBasicsSection
{
    public required string Title { get; init; }
    public required string Content { get; init; }
    public List<MermaidDiagram>? Mermaid { get; init; }
    public List<GenAIFaq>? Faq { get; init; }
    public List<MoreInfoLink>? MoreInfo { get; init; }
}

/// <summary>
/// Mermaid diagram with semantic ID for placeholder replacement
/// </summary>
public record MermaidDiagram
{
    public required string Id { get; init; }
    public string? Title { get; init; }
    public required string Diagram { get; init; }
}

/// <summary>
/// FAQ question and answer pair
/// </summary>
public record GenAIFaq
{
    public required string Question { get; init; }
    public required string Answer { get; init; }
}

/// <summary>
/// Resource link in More Information sections
/// </summary>
public record MoreInfoLink
{
    public required string Text { get; init; }
    public required string Url { get; init; }
}
