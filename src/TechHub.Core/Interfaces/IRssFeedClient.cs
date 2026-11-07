using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Typed HTTP client for downloading RSS/Atom feed XML from external URLs.
/// </summary>
public interface IRssFeedClient
{
    /// <summary>
    /// Downloads the feed XML at <paramref name="url"/>.
    /// Returns a failed result with the reason (HTTP status, timeout or connection failure) instead of throwing.
    /// </summary>
    Task<FeedDownloadResult> FetchFeedXmlAsync(string url, CancellationToken ct = default);
}
