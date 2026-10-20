namespace TechHub.Core.Configuration;

/// <summary>
/// Configuration for the Azure OpenAI-backed AI categorization service.
/// </summary>
public class AiCategorizationOptions
{
    /// <summary>Configuration section name.</summary>
    public const string SectionName = "AiCategorization";

    /// <summary>Azure AI Foundry endpoint URL (e.g. https://oai-techhub-prod.openai.azure.com/).</summary>
    public string Endpoint { get; init; } = string.Empty;

    /// <summary>Model, reasoning effort and service tier per pipeline step.</summary>
    public AiProfilesOptions Profiles { get; init; } = new();

    /// <summary>Delay in seconds between AI API calls to prevent rate limiting.</summary>
    public int RateLimitDelaySeconds { get; init; } = 15;

    /// <summary>Maximum number of retries on transient failures.</summary>
    public int MaxRetries { get; init; } = 3;
}
