using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Downloads and parses RSS/Atom feeds into raw feed items.
/// </summary>
public interface IRssFeedIngestionService
{
    /// <summary>
    /// Downloads the feed at <paramref name="feedConfig"/> and returns items published
    /// within the configured age limit, newest first.
    /// </summary>
    Task<IReadOnlyList<RawFeedItem>> IngestAsync(FeedConfig feedConfig, CancellationToken ct = default);
}
