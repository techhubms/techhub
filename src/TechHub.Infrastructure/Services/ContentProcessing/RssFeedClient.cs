using Microsoft.Extensions.Logging;
using Polly.Timeout;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Services.ContentProcessing;

/// <summary>
/// Typed HTTP client that downloads RSS/Atom feed XML from external URLs.
/// Timeout and retry behavior is managed by Polly via AddStandardResilienceHandler.
/// </summary>
public sealed class RssFeedClient : IRssFeedClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RssFeedClient> _logger;

    public RssFeedClient(
        HttpClient httpClient,
        ILogger<RssFeedClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<string?> FetchFeedXmlAsync(string url, CancellationToken ct = default)
    {
        try
        {
            return await _httpClient.GetStringAsync(url, ct);
        }
        // A slow feed is an expected failure: the Polly timeout (TimeoutRejectedException) and HttpClient.Timeout
        // (TaskCanceledException) must not abort the whole run, so only caller cancellation propagates.
        catch (Exception ex) when (ex is HttpRequestException or TimeoutRejectedException
            || (ex is OperationCanceledException && !ct.IsCancellationRequested))
        {
            _logger.LogError(ex, "Failed to download feed from {Url}", url);
            return null;
        }
    }
}
