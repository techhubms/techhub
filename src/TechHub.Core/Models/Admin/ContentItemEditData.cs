namespace TechHub.Core.Models.Admin;

/// <summary>
/// All editable fields for a content item, used by the admin UI for inline editing.
/// CollectionName and Slug are included for reference but are not updatable.
/// </summary>
public sealed record ContentItemEditData
{
    public required string CollectionName { get; init; }
    public required string Slug { get; init; }

    /// <summary>Unix epoch publication date (seconds). Editable in admin UI.</summary>
    public long DateEpoch { get; init; }

    public required string Title { get; init; }
    public required string Author { get; init; }
    public required string Excerpt { get; init; }
    public required string Content { get; init; }
    public required string PrimarySectionName { get; init; }
    public string? FeedName { get; init; }
    public required IReadOnlyList<string> Tags { get; init; }
    public required IReadOnlyList<string> Sections { get; init; }

    /// <summary>Raw JSON string from the ai_metadata column (may be null).</summary>
    public string? AiMetadata { get; init; }
}
