using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Typed HTTP client that downloads RSS/Atom feed XML from external URLs.
/// </summary>
public sealed class RssFeedClient : IRssFeedClient
{
    private readonly HttpClient _httpClient;
    private readonly ContentProcessorOptions _options;
    private readonly ILogger<RssFeedClient> _logger;

    public RssFeedClient(
        HttpClient httpClient,
        IOptions<ContentProcessorOptions> options,
        ILogger<RssFeedClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<string?> FetchFeedXmlAsync(string url, CancellationToken ct = default)
    {
        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromSeconds(_options.RequestTimeoutSeconds));

            return await _httpClient.GetStringAsync(url, cts.Token);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to download feed from {Url}", url);
            return null;
        }
    }
}
