namespace TechHub.Core.Interfaces;

/// <summary>
/// Typed HTTP client for downloading RSS/Atom feed XML from external URLs.
/// </summary>
public interface IRssFeedClient
{
    /// <summary>
    /// Downloads the feed XML at <paramref name="url"/>.
    /// Returns <see langword="null"/> on HTTP failure or timeout.
    /// </summary>
    Task<string?> FetchFeedXmlAsync(string url, CancellationToken ct = default);
}
