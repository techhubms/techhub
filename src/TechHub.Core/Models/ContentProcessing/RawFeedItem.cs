namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// A raw item downloaded from an RSS/Atom feed before AI processing.
/// </summary>
public sealed class RawFeedItem
{
    /// <summary>Item title from the feed (used for logging, not authoritative — AI extracts from feed data).</summary>
    public required string Title { get; init; }

    /// <summary>External URL to the original article/video/post.</summary>
    public required string ExternalUrl { get; init; }

    /// <summary>Publication date from the feed.</summary>
    public required DateTimeOffset PublishedAt { get; init; }

    /// <summary>
    /// Compact key-value representation of the raw feed item XML.
    /// Contains all metadata from the feed entry (title, description, author, categories, etc.)
    /// in a token-efficient format for AI processing. HTML is converted to markdown.
    /// </summary>
    public string FeedItemData { get; init; } = string.Empty;

    /// <summary>
    /// Feed-level author fallback (e.g. from <c>managingEditor</c>, <c>webMaster</c>,
    /// or feed-level <c>author/name</c>). Used when no per-item author is found in the feed data.
    /// Falls back to <see cref="FeedName"/> if no feed-level author exists.
    /// </summary>
    public string FeedLevelAuthor { get; init; } = string.Empty;

    /// <summary>Tags/categories already extracted from the feed entry.</summary>
    public IReadOnlyList<string> FeedTags { get; init; } = [];

    /// <summary>Name of the feed this item came from (e.g. "The GitHub Blog").</summary>
    public required string FeedName { get; init; }

    /// <summary>Target collection name (e.g. "news", "blogs", "videos", "community").</summary>
    public required string CollectionName { get; init; }

    /// <summary>Full text content of the article (fetched separately from the feed URL).</summary>
    public string? FullContent { get; init; }

    /// <summary>
    /// Reason why the YouTube transcript could not be fetched, if applicable.
    /// <see langword="null"/> when not a YouTube item or when the transcript was fetched successfully.
    /// </summary>
    public string? TranscriptFailureReason { get; init; }

    /// <summary>Whether this item links to a YouTube video.</summary>
    public bool IsYouTube => ExternalUrl.Contains("youtube.com", StringComparison.OrdinalIgnoreCase)
        || ExternalUrl.Contains("youtu.be", StringComparison.OrdinalIgnoreCase);
}
