using System.Text.Json;

namespace TechHub.Core.DTOs;

/// <summary>
/// DTO for full content item detail view (includes rendered HTML)
/// </summary>
public record ContentItemDetailDto
{
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public string? Author { get; init; }
    public required long DateEpoch { get; init; }
    public required string DateIso { get; init; }
    public required string CollectionName { get; init; }
    public string? FeedName { get; init; }
    public required IReadOnlyList<string> SectionNames { get; init; }
    public required string PrimarySection { get; init; }
    public required IReadOnlyList<string> Tags { get; init; }
    public required string RenderedHtml { get; init; }
    public required string Excerpt { get; init; }
    public string? ExternalUrl { get; init; }
    public string? ViewingMode { get; init; }
    public required string Url { get; init; }

    /// <summary>
    /// Dynamic sidebar info as JSON (from 'sidebar-info' frontmatter field)
    /// Can be deserialized into custom structures as needed per page
    /// </summary>
    public JsonElement? SidebarInfo { get; init; }
}
