using System.Text.Json;

namespace TechHub.Core.DTOs;

/// <summary>
/// DTO for custom page detail returned from API
/// Includes full rendered HTML content
/// </summary>
public record CustomPageDetailDto
{
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Url { get; init; }
    public required IReadOnlyList<string> Categories { get; init; }
    public required string RenderedHtml { get; init; }
    public JsonElement? SidebarInfo { get; init; }
}
