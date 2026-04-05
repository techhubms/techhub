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

    /// <summary>
    /// When <c>true</c>, YouTube items from this feed that fail transcript
    /// fetching are marked as failed instead of proceeding without a transcript.
    /// </summary>
    public bool TranscriptMandatory { get; set; }

    /// <summary>Collection name with leading underscore stripped.</summary>
    public string CollectionName => OutputDir.TrimStart('_').ToLowerInvariant();
}
