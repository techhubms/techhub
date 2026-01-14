namespace TechHub.Core.DTOs;

/// <summary>
/// Data structure for the GenAI Basics page
/// </summary>
public record GenAIBasicsPageData
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required List<string> Categories { get; init; }
    public required List<string> Tags { get; init; }
    public required string Intro { get; init; }
    public required List<RelatedPage> RelatedPages { get; init; }
    public required List<TableOfContentsItem> TableOfContents { get; init; }
    public required List<GenAIBasicsSection> Sections { get; init; }
    public required NavigationLink NextPage { get; init; }
}

public record RelatedPage
{
    public required string Title { get; init; }
    public required string Url { get; init; }
}

public record TableOfContentsItem
{
    public required string Id { get; init; }
    public required string Title { get; init; }
}

public record GenAIBasicsSection
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public string? Intro { get; init; }
    public List<GenAISubsection>? Subsections { get; init; }
    public List<GenAIVendor>? Vendors { get; init; }
    public List<GenAIModelCategory>? ModelCategories { get; init; }
    public List<GenAIFaq>? Faq { get; init; }
    public List<MoreInfoLink>? MoreInfo { get; init; }
}

public record GenAISubsection
{
    public required string Title { get; init; }
    public string? Content { get; init; }
    public List<string>? List { get; init; }
    public List<TimelineEvent>? Timeline { get; init; }
}

public record TimelineEvent
{
    public required string Year { get; init; }
    public required string Event { get; init; }
    public string? Detail { get; init; }
    public bool Highlight { get; init; }
}

public record GenAIVendor
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}

public record GenAIModelCategory
{
    public required string Title { get; init; }
    public required List<GenAIModel> Models { get; init; }
}

public record GenAIModel
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}

public record GenAIFaq
{
    public required string Question { get; init; }
    public required string Answer { get; init; }
}

public record MoreInfoLink
{
    public required string Text { get; init; }
    public required string Url { get; init; }
}

public record NavigationLink
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required string Url { get; init; }
    public required string LinkText { get; init; }
}
