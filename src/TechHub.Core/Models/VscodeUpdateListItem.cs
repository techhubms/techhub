namespace TechHub.Core.Models;

/// <summary>
/// A VS Code Update entry joined with basic content_items fields for display in the admin UI.
/// </summary>
public sealed class VscodeUpdateListItem
{
    public required string CollectionName { get; init; }
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public required string ExternalUrl { get; init; }
    public long DateEpoch { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}
