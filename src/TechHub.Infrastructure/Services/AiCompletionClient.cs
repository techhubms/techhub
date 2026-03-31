using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Typed HTTP client that sends chat completion requests to Azure OpenAI.
/// </summary>
public sealed class AiCompletionClient : IAiCompletionClient
{
    private readonly HttpClient _httpClient;
    private readonly AiCategorizationOptions _options;
    private readonly ILogger<AiCompletionClient> _logger;

    public AiCompletionClient(
        HttpClient httpClient,
        IOptions<AiCategorizationOptions> options,
        ILogger<AiCompletionClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<AiCompletionResult> SendCompletionAsync(string jsonBody, CancellationToken ct = default)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            string.Create(CultureInfo.InvariantCulture,
                $"{_options.Endpoint.TrimEnd('/')}/openai/deployments/{_options.DeploymentName}/chat/completions?api-version={_options.ApiVersion}"));

        request.Headers.Add("api-key", _options.ApiKey);
        request.Content = new StringContent(jsonBody, Encoding.UTF8, new MediaTypeHeaderValue("application/json"));

        using var response = await _httpClient.SendAsync(request, ct);

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            var retryAfter = ExtractRetryAfterSeconds(response);
            _logger.LogWarning("AI rate limit hit{RetryInfo}",
                retryAfter.HasValue ? $", retry-after: {retryAfter}s" : "");
            return new AiCompletionResult(IsRateLimited: true, ResponseBody: null, RetryAfterSeconds: retryAfter);
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(ct);
            _logger.LogError("AI API returned {StatusCode}: {ErrorBody}", (int)response.StatusCode, errorBody);

            // Detect content filter violations and context length exceeded (400) — no point retrying these
            if (response.StatusCode == HttpStatusCode.BadRequest && !string.IsNullOrEmpty(errorBody))
            {
                var errorLower = errorBody.ToLowerInvariant();
                if (errorLower.Contains("content_filter", StringComparison.Ordinal)
                    || errorLower.Contains("content filter", StringComparison.Ordinal)
                    || errorLower.Contains("responsibleaipolicyviolation", StringComparison.Ordinal)
                    || errorLower.Contains("jailbreak", StringComparison.Ordinal))
                {
                    return new AiCompletionResult(IsRateLimited: false, ResponseBody: null,
                        ContentFilterMessage: "Content blocked by Azure AI content filter");
                }

                if (errorLower.Contains("context_length_exceeded", StringComparison.Ordinal)
                    || errorLower.Contains("too many tokens", StringComparison.Ordinal))
                {
                    return new AiCompletionResult(IsRateLimited: false, ResponseBody: null,
                        ContentFilterMessage: "Content too large for AI model context window");
                }
            }
        }

        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync(ct);
        return new AiCompletionResult(IsRateLimited: false, ResponseBody: responseJson);
    }

    /// <summary>
    /// Extracts the number of seconds to wait from rate-limit response headers.
    /// Checks <c>Retry-After</c>, <c>x-ratelimit-reset-requests</c>, and <c>x-ratelimit-timeremaining</c>.
    /// </summary>
    private static int? ExtractRetryAfterSeconds(HttpResponseMessage response)
    {
        // Standard Retry-After header (seconds or HTTP-date)
        if (response.Headers.RetryAfter is { } retryAfter)
        {
            if (retryAfter.Delta.HasValue)
            {
                return (int)retryAfter.Delta.Value.TotalSeconds;
            }

            if (retryAfter.Date.HasValue)
            {
                var wait = (int)(retryAfter.Date.Value - DateTimeOffset.UtcNow).TotalSeconds;
                return wait > 0 ? wait : null;
            }
        }

        // Azure OpenAI custom headers
        foreach (var headerName in new[] { "x-ratelimit-reset-requests", "x-ratelimit-timeremaining", "retry-after-ms" })
        {
            if (response.Headers.TryGetValues(headerName, out var values))
            {
                var value = values.FirstOrDefault();
                if (int.TryParse(value, out var seconds) && seconds > 0)
                {
                    // retry-after-ms is in milliseconds
                    return headerName == "retry-after-ms" ? Math.Max(1, seconds / 1000) : seconds;
                }
            }
        }

        return null;
    }
}
