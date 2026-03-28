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
                $"{_options.Endpoint.TrimEnd('/')}/openai/deployments/{_options.DeploymentName}/chat/completions?api-version=2024-10-21"));

        request.Headers.Add("api-key", _options.ApiKey);
        request.Content = new StringContent(jsonBody, Encoding.UTF8, new MediaTypeHeaderValue("application/json"));

        using var response = await _httpClient.SendAsync(request, ct);

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            _logger.LogWarning("AI rate limit hit");
            return new AiCompletionResult(IsRateLimited: true, ResponseBody: null);
        }

        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync(ct);
        return new AiCompletionResult(IsRateLimited: false, ResponseBody: responseJson);
    }
}
