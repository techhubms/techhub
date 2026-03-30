namespace TechHub.Core.Models.Admin;

/// <summary>
/// The ai_metadata JSON for a content item, used by the admin UI for inline editing.
/// </summary>
public sealed record ContentItemAiMetadataResult
{
    public required string CollectionName { get; init; }
    public required string Slug { get; init; }

    /// <summary>Raw JSON string from the ai_metadata column (may be null).</summary>
    public string? AiMetadata { get; init; }
}
