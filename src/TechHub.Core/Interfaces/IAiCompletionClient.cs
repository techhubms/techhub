namespace TechHub.Core.Interfaces;

/// <summary>
/// Result of an Azure OpenAI chat completion request.
/// </summary>
/// <param name="IsRateLimited">Whether a 429 response was received.</param>
/// <param name="ResponseBody">The response body on success, or <c>null</c> on rate limit.</param>
/// <param name="RetryAfterSeconds">
/// Number of seconds to wait before retrying, extracted from
/// <c>Retry-After</c> or <c>x-ratelimit-reset-requests</c> headers. <c>null</c> if not available.
/// </param>
/// <param name="ContentFilterMessage">Non-null when the request was blocked by Azure content filters.</param>
public sealed record AiCompletionResult(
    bool IsRateLimited,
    string? ResponseBody,
    int? RetryAfterSeconds = null,
    string? ContentFilterMessage = null);

/// <summary>
/// Typed HTTP client for sending chat completion requests to Azure OpenAI.
/// </summary>
public interface IAiCompletionClient
{
    /// <summary>
    /// Sends a chat completion request with the given JSON body.
    /// Returns a result indicating rate-limiting or the response body.
    /// Throws <see cref="HttpRequestException"/> on transport errors.
    /// </summary>
    Task<AiCompletionResult> SendCompletionAsync(string jsonBody, CancellationToken ct = default);
}
