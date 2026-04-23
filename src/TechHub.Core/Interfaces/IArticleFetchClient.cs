namespace TechHub.Core.Interfaces;

/// <summary>
/// Typed HTTP client for fetching article HTML content from external URLs.
/// </summary>
public interface IArticleFetchClient
{
    /// <summary>
    /// Fetches the HTML content at <paramref name="url"/>.
    /// Returns <see langword="null"/> on HTTP failure or timeout.
    /// </summary>
    Task<string?> FetchHtmlAsync(string url, CancellationToken ct = default);
}
