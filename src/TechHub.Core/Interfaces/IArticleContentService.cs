using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Fetches full article content and enriches feed items with body text or transcripts.
/// </summary>
public interface IArticleContentService
{
    /// <summary>
    /// Fetches the full article content for <paramref name="item"/> and returns a new instance
    /// with <see cref="RawFeedItem.FullContent"/> populated.
    /// YouTube items are enriched with transcript text when available.
    /// Returns the original item unchanged on failure.
    /// </summary>
    Task<RawFeedItem> EnrichWithContentAsync(RawFeedItem item, CancellationToken ct = default);
}
