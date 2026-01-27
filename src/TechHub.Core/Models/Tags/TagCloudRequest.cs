namespace TechHub.Core.Models;

/// <summary>
/// Request DTO for tag cloud calculation
/// </summary>
public record TagCloudRequest
{
    /// <summary>
    /// Scope of tag cloud calculation
    /// </summary>
    public required TagCloudScope Scope { get; init; }

    /// <summary>
    /// Section name (required for Section/Collection/Content scopes)
    /// </summary>
    public string? SectionName { get; init; }

    /// <summary>
    /// Collection name (required for Collection scope)
    /// </summary>
    public string? CollectionName { get; init; }

    /// <summary>
    /// Content item slug (required for Content scope)
    /// </summary>
    public string? Slug { get; init; }

    /// <summary>
    /// Maximum number of tags to return (default: 20)
    /// </summary>
    public int MaxTags { get; init; } = 20;

    /// <summary>
    /// Minimum usage count for tags (default: 5)
    /// </summary>
    public int MinUses { get; init; } = 5;

    /// <summary>
    /// Include only content from last N days (default: 90)
    /// Null = all time
    /// </summary>
    public int? LastDays { get; init; } = 90;
}

/// <summary>
/// Scope of tag cloud calculation
/// </summary>
public enum TagCloudScope
{
    /// <summary>Global tag cloud (all sections/collections)</summary>
    Homepage = 0,

    /// <summary>Section-scoped tag cloud</summary>
    Section = 1,

    /// <summary>Collection-scoped tag cloud (section + collection)</summary>
    Collection = 2,

    /// <summary>Content item tags only (no aggregation)</summary>
    Content = 3
}
