namespace TechHub.Core.Configuration;

/// <summary>
/// Configuration for the Azure OpenAI-backed AI categorization service.
/// </summary>
public class AiCategorizationOptions
{
    /// <summary>Configuration section name.</summary>
    public const string SectionName = "AiCategorization";

    /// <summary>Azure OpenAI endpoint URL (e.g. https://oai-techhub-prod.openai.azure.com/).</summary>
    public string Endpoint { get; init; } = string.Empty;

    /// <summary>Azure OpenAI API key.</summary>
    public string ApiKey { get; init; } = string.Empty;

    /// <summary>Azure OpenAI deployment name (e.g. gpt-4.1).</summary>
    public string DeploymentName { get; init; } = string.Empty;

    /// <summary>Maximum content length in characters sent to the AI model.</summary>
    public int MaxContentLength { get; init; } = 200_000;

    /// <summary>Delay in seconds between AI API calls to prevent rate limiting.</summary>
    public int RateLimitDelaySeconds { get; init; } = 15;

    /// <summary>Maximum number of retries on transient failures.</summary>
    public int MaxRetries { get; init; } = 3;
}
