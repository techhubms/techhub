using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Typed HTTP client that fetches YouTube video snippet data from the YouTube API.
/// </summary>
public sealed class YouTubeTagClient : IYouTubeTagClient
{
    private const string ApiBaseUrl = "https://ytapi.apps.mattw.io/v3/videos?part=snippet&id=";

    private readonly HttpClient _httpClient;
    private readonly ContentProcessorOptions _options;
    private readonly ILogger<YouTubeTagClient> _logger;

    public YouTubeTagClient(
        HttpClient httpClient,
        IOptions<ContentProcessorOptions> options,
        ILogger<YouTubeTagClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<string?> FetchVideoSnippetAsync(string videoId, CancellationToken ct = default)
    {
        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromSeconds(_options.RequestTimeoutSeconds));

            var url = $"{ApiBaseUrl}{Uri.EscapeDataString(videoId)}";
            _logger.LogDebug("Fetching YouTube tags for video {VideoId}", videoId);

            using var response = await _httpClient.GetAsync(url, cts.Token);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("YouTube tag API returned {Status} for video {VideoId}", (int)response.StatusCode, videoId);
                return null;
            }

            return await response.Content.ReadAsStringAsync(cts.Token);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Timeout fetching YouTube tags for video {VideoId}", videoId);
            return null;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to fetch YouTube tags for video {VideoId}", videoId);
            return null;
        }
    }
}
