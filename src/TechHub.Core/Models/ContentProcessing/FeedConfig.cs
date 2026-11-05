namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// Configuration for a single RSS feed source. Stored in the <c>rss_feed_configs</c> table.
/// </summary>
public sealed class FeedConfig
{
    /// <summary>Database primary key (0 for new/unsaved configs).</summary>
    public long Id { get; set; }

    /// <summary>Display name for the feed (e.g. "The GitHub Blog").</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Target collection directory, may include a leading underscore (e.g. "_news" or "news").
    /// Callers should normalize via <see cref="CollectionName"/>.
    /// </summary>
    public string OutputDir { get; set; } = string.Empty;

    /// <summary>RSS/Atom feed URL.</summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>Whether this feed is active and should be processed.</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>Minutes between scheduled checks of this feed. Must be positive.</summary>
    public int PollingIntervalMinutes { get; set; } = 60;

    /// <summary>
    /// True when content items or processed URLs still reference this feed, which blocks deletion.
    /// Populated by the repository on reads; ignored on create and update.
    /// </summary>
    public bool HasContent { get; set; }

    /// <summary>When the content processing pipeline last downloaded this feed, or null when it never did.</summary>
    public DateTimeOffset? LastCheckedAt { get; set; }

    /// <summary>
    /// Why the last download or parse of this feed failed (for example "HTTP 404 (NotFound)"),
    /// or null when the last check succeeded or the feed was never checked.
    /// </summary>
    public string? LastCheckError { get; set; }

    /// <summary>
    /// When the newest content item of this feed was added, or null when the feed has no content items.
    /// Populated by the repository on reads; ignored on create and update.
    /// </summary>
    public DateTimeOffset? LastContentAt { get; set; }

    /// <summary>Collection name with leading underscore stripped.</summary>
    public string CollectionName => OutputDir.TrimStart('_').ToLowerInvariant();
}
