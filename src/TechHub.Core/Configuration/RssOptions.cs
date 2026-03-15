namespace TechHub.Core.Configuration;

/// <summary>
/// Configuration options for RSS feed generation
/// </summary>
public class RssOptions
{
    /// <summary>
    /// Maximum number of items to include in any RSS feed
    /// </summary>
    public int MaxItemsInFeed { get; set; } = 50;
}
