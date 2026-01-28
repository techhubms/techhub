namespace TechHub.Core.Models;

/// <summary>
/// RSS item (individual feed entry) for RSS feed generation
/// </summary>
public record RssItem
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Link { get; init; }
    public required string Guid { get; init; }
    public required DateTimeOffset PubDate { get; init; }
    public string? Author { get; init; }
    public IReadOnlyList<string> Categories { get; init; } = [];

}
