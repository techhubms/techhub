namespace TechHub.Core.DTOs;

/// <summary>
/// Data structure for the GitHub Copilot Handbook page
/// </summary>
public record HandbookPageData
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required List<string> Authors { get; init; }
    public required BookDetails BookDetails { get; init; }
    public required string Intro { get; init; }
    public required HandbookLink PurchaseLink { get; init; }
    public required List<string> AboutBook { get; init; }
    public required List<string> Learnings { get; init; }
    public required HandbookAudience Audience { get; init; }
    public required List<HandbookKeyFeature> KeyFeatures { get; init; }
    public required List<string> TableOfContents { get; init; }
    public required List<AuthorBio> AuthorBios { get; init; }
    public required string Conclusion { get; init; }
}

public record BookDetails
{
    public required string Publisher { get; init; }
    public required string Released { get; init; }
    public required int Pages { get; init; }
}

public record HandbookLink
{
    public required string Url { get; init; }
    public required string Text { get; init; }
}

public record HandbookAudience
{
    public required string Intro { get; init; }
    public required string Details { get; init; }
}

public record HandbookKeyFeature
{
    public required string Title { get; init; }
    public required string Description { get; init; }
}

public record AuthorBio
{
    public required string Name { get; init; }
    public required string Bio { get; init; }
}
