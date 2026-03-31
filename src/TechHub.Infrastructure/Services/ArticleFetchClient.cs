using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Typed HTTP client that fetches article HTML content from external URLs.
/// Timeout and retry behavior is managed by Polly via AddStandardResilienceHandler.
/// </summary>
public sealed class ArticleFetchClient : IArticleFetchClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ArticleFetchClient> _logger;

    public ArticleFetchClient(
        HttpClient httpClient,
        ILogger<ArticleFetchClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<string?> FetchHtmlAsync(string url, CancellationToken ct = default)
    {
        try
        {
            _logger.LogDebug("Fetching content for: {Url}", url);

            using var response = await _httpClient.GetAsync(url, ct);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch content for {Url}: HTTP {Status}", url, (int)response.StatusCode);
                return null;
            }

            return await response.Content.ReadAsStringAsync(ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "HTTP error fetching content for {Url}", url);
            return null;
        }
    }
}
