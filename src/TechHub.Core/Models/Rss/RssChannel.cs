namespace TechHub.Core.Models;

/// <summary>
/// RSS channel (feed metadata) for RSS feed generation
/// </summary>
public record RssChannel
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Link { get; init; }
    public required string Language { get; init; }
    public required DateTimeOffset LastBuildDate { get; init; }
    public required IReadOnlyList<RssItem> Items { get; init; }
}
