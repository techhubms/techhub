namespace TechHub.Core.Models;

/// <summary>
/// Tag display information for tag cloud rendering
/// </summary>
public record TagCloudItem
{
    /// <summary>
    /// Tag name (normalized for display)
    /// </summary>
    public required string Tag { get; init; }

    /// <summary>
    /// Number of content items with this tag in current scope
    /// </summary>
    public required int Count { get; init; }

    /// <summary>
    /// Visual size category for tag cloud display
    /// </summary>
    public required TagSize Size { get; init; }
}

/// <summary>
/// Tag size categories for quantile-based sizing
/// </summary>
public enum TagSize
{
    /// <summary>Bottom 25% of tag cloud (least popular within top 20)</summary>
    Small = 0,

    /// <summary>Middle 50% of tag cloud (moderately popular)</summary>
    Medium = 1,

    /// <summary>Top 25% of tag cloud (most popular)</summary>
    Large = 2
}
