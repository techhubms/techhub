namespace TechHub.Core.DTOs;

/// <summary>
/// DTO for RSS channel (feed metadata)
/// </summary>
public record RssChannelDto
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Link { get; init; }
    public required string Language { get; init; }
    public required DateTimeOffset LastBuildDate { get; init; }
    public required IReadOnlyList<RssItemDto> Items { get; init; }
}

/// <summary>
/// DTO for RSS item (individual feed entry)
/// </summary>
public record RssItemDto
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Link { get; init; }
    public required string Guid { get; init; }
    public required DateTimeOffset PubDate { get; init; }
    public string? Author { get; init; }
    public required IReadOnlyList<string> Sections { get; init; }
}
