namespace TechHub.Core.Models;

/// <summary>
/// RSS item (individual feed entry) for RSS feed generation
/// </summary>
public record RssItem
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Link { get; init; }
#pragma warning disable CA1720 // Identifier contains type name - "Guid" is the RSS 2.0 standard field name, not System.Guid
    public required string Guid { get; init; }
#pragma warning restore CA1720
    public required DateTimeOffset PubDate { get; init; }
    public string? Author { get; init; }
    public IReadOnlyList<string> Categories { get; init; } = [];

}
