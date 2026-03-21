namespace TechHub.ContentProcessor.Models;

/// <summary>
/// A raw item downloaded from an RSS/Atom feed before AI processing.
/// </summary>
public sealed class RawFeedItem
{
    /// <summary>Item title from the feed.</summary>
    public required string Title { get; init; }

    /// <summary>External URL to the original article/video/post.</summary>
    public required string ExternalUrl { get; init; }

    /// <summary>Publication date from the feed.</summary>
    public required DateTimeOffset PublishedAt { get; init; }

    /// <summary>Short description/summary from the feed entry.</summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>Author name from the feed entry, if available.</summary>
    public string? Author { get; init; }

    /// <summary>Tags/categories already present in the feed entry.</summary>
    public IReadOnlyList<string> FeedTags { get; init; } = [];

    /// <summary>Name of the feed this item came from (e.g. "The GitHub Blog").</summary>
    public required string FeedName { get; init; }

    /// <summary>Target collection name (e.g. "news", "blogs", "videos", "community").</summary>
    public required string CollectionName { get; init; }

    /// <summary>Full HTML content of the article (fetched separately from the feed URL).</summary>
    public string? FullContent { get; init; }

    /// <summary>Whether this item links to a YouTube video.</summary>
    public bool IsYouTube => ExternalUrl.Contains("youtube.com", StringComparison.OrdinalIgnoreCase)
        || ExternalUrl.Contains("youtu.be", StringComparison.OrdinalIgnoreCase);
}
