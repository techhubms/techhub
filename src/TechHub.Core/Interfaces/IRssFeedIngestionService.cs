using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Downloads and parses RSS/Atom feeds into raw feed items.
/// </summary>
public interface IRssFeedIngestionService
{
    /// <summary>
    /// Downloads the feed at <paramref name="feedConfig"/> and returns a result indicating
    /// success (with items published within the configured age limit, newest first) or failure
    /// (feed download or parse error).
    /// </summary>
    Task<FeedIngestionResult> IngestAsync(FeedConfig feedConfig, CancellationToken ct = default);
}
