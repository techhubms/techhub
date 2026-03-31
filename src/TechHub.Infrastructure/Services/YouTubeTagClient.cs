using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Typed HTTP client that fetches YouTube video snippet data from the YouTube API.
/// Timeout and retry behavior is managed by Polly via AddStandardResilienceHandler.
/// </summary>
public sealed class YouTubeTagClient : IYouTubeTagClient
{
    private const string ApiBaseUrl = "https://ytapi.apps.mattw.io/v3/videos?part=snippet&id=";

    private readonly HttpClient _httpClient;
    private readonly ILogger<YouTubeTagClient> _logger;

    public YouTubeTagClient(
        HttpClient httpClient,
        ILogger<YouTubeTagClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<string?> FetchVideoSnippetAsync(string videoId, CancellationToken ct = default)
    {
        try
        {
            var url = $"{ApiBaseUrl}{Uri.EscapeDataString(videoId)}";
            _logger.LogDebug("Fetching YouTube tags for video {VideoId}", videoId);

            using var response = await _httpClient.GetAsync(url, ct);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("YouTube tag API returned {Status} for video {VideoId}", (int)response.StatusCode, videoId);
                return null;
            }

            return await response.Content.ReadAsStringAsync(ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to fetch YouTube tags for video {VideoId}", videoId);
            return null;
        }
    }
}
