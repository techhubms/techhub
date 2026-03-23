namespace TechHub.Core.Configuration;

/// <summary>
/// Configuration for the Azure OpenAI-backed AI categorization service.
/// </summary>
public class AiCategorizationOptions
{
    /// <summary>Configuration section name.</summary>
    public const string SectionName = "AiCategorization";

    /// <summary>Whether AI categorization is enabled. When false, items are skipped.</summary>
    public bool Enabled { get; init; } = true;

    /// <summary>Azure OpenAI endpoint URL (e.g. https://oai-techhub-prod.openai.azure.com/).</summary>
    public required string Endpoint { get; init; }

    /// <summary>Azure OpenAI API key.</summary>
    public required string ApiKey { get; init; }

    /// <summary>Azure OpenAI deployment name (e.g. gpt-4.1).</summary>
    public required string DeploymentName { get; init; }

    /// <summary>Maximum content length in characters sent to the AI model.</summary>
    public int MaxContentLength { get; init; } = 200_000;

    /// <summary>Delay in seconds between AI API calls to prevent rate limiting.</summary>
    public int RateLimitDelaySeconds { get; init; } = 15;

    /// <summary>Maximum number of retries on transient failures.</summary>
    public int MaxRetries { get; init; } = 3;
}
