namespace TechHub.Core.Interfaces;

/// <summary>
/// Result of an Azure OpenAI chat completion request.
/// </summary>
public sealed record AiCompletionResult(bool IsRateLimited, string? ResponseBody);

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
