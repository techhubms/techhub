namespace TechHub.Core.Interfaces;

/// <summary>
/// Typed HTTP client for fetching YouTube video snippet data from the YouTube API.
/// </summary>
public interface IYouTubeTagClient
{
    /// <summary>
    /// Fetches the snippet JSON for the given <paramref name="videoId"/>.
    /// Returns <see langword="null"/> on HTTP failure or timeout.
    /// </summary>
    Task<string?> FetchVideoSnippetAsync(string videoId, CancellationToken ct = default);
}
