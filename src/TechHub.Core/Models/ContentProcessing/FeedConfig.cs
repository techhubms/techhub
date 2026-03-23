namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// Configuration for a single RSS feed source from rss-feeds.json.
/// </summary>
public sealed class FeedConfig
{
    /// <summary>Display name for the feed (e.g. "The GitHub Blog").</summary>
    public required string Name { get; init; }

    /// <summary>
    /// Target collection directory, may include a leading underscore (e.g. "_news" or "news").
    /// Callers should normalize via <see cref="CollectionName"/>.
    /// </summary>
    public required string OutputDir { get; init; }

    /// <summary>RSS/Atom feed URL.</summary>
    public required string Url { get; init; }

    /// <summary>Collection name with leading underscore stripped.</summary>
    public string CollectionName => OutputDir.TrimStart('_').ToLowerInvariant();
}
