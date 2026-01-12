using System.Text.Json;

namespace TechHub.Core.Models;

/// <summary>
/// Domain model for custom pages from the _custom collection
/// Custom pages are standalone content pages with their own routes
/// </summary>
public record CustomPage
{
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Permalink { get; init; }
    public required IReadOnlyList<string> Categories { get; init; }
    public required string Content { get; init; }
    public JsonElement? SidebarInfo { get; init; }
}
