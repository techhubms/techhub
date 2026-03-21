namespace TechHub.ContentProcessor.Models;

/// <summary>
/// Configuration for a single RSS feed source from rss-feeds.json.
/// </summary>
public sealed class FeedConfig
{
    /// <summary>Display name for the feed (e.g. "The GitHub Blog").</summary>
    public required string Name { get; init; }

    /// <summary>
    /// Target collection directory without underscore (e.g. "news", "blogs", "videos", "community").
    /// Maps to the outputDir field in the JSON config which may include the leading underscore.
    /// </summary>
    public required string OutputDir { get; init; }

    /// <summary>RSS/Atom feed URL.</summary>
    public required string Url { get; init; }
}
