using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Services.ContentProcessing;

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
    public Task<string?> FetchHtmlAsync(string url, CancellationToken ct = default)
        => FetchHtmlAsync(url, redirectDepth: 0, ct);

    private async Task<string?> FetchHtmlAsync(string url, int redirectDepth, CancellationToken ct)
    {
        // Guard against redirect loops (e.g. www→no-www→https chains)
        if (redirectDepth > 5)
        {
            _logger.LogWarning("Too many redirects fetching {Url}", url);
            return null;
        }

        try
        {
            _logger.LogDebug("Fetching content for: {Url}", url);

            using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, ct);

            // HttpClient blocks HTTPS→HTTP scheme-downgrade redirects by default (security policy).
            // For public article content (no credentials) this is safe to follow explicitly.
            // Example: www.mindbyte.nl → http://mindbyte.nl → https://mindbyte.nl (200)
            if (response.StatusCode is System.Net.HttpStatusCode.MovedPermanently
                                    or System.Net.HttpStatusCode.Found
                                    or System.Net.HttpStatusCode.TemporaryRedirect
                                    or System.Net.HttpStatusCode.PermanentRedirect)
            {
                var location = response.Headers.Location;
                if (location is not null)
                {
                    var redirectUrl = location.IsAbsoluteUri ? location.AbsoluteUri : new Uri(new Uri(url), location).AbsoluteUri;
                    _logger.LogDebug("Following redirect for {Url} → {Redirect}", url, redirectUrl);
                    return await FetchHtmlAsync(redirectUrl, redirectDepth + 1, ct);
                }
            }

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
