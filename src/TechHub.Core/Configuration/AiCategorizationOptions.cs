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

    /// <summary>Delay in seconds between articles during the timeline backfill.</summary>
    public int TimelineBackfillDelaySeconds { get; init; } = 2;

    /// <summary>Maximum number of retries on transient failures.</summary>
    public int MaxRetries { get; init; } = 3;

    /// <summary>How long a Flex request keeps retrying on Flex while capacity is unavailable (429).</summary>
    public TimeSpan FlexMaxWait { get; init; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// After a Flex request gave up, later requests within this period try Flex once and fall back at the first 429 instead of waiting
    /// <see cref="FlexMaxWait"/> again. Zero disables this.
    /// </summary>
    public TimeSpan FlexUnavailableCooldown { get; init; } = TimeSpan.FromMinutes(15);

    /// <summary>Shortest wait between Flex retries; also the base of the exponential backoff when no Retry-After is given.</summary>
    public TimeSpan FlexMinRetryDelay { get; init; } = TimeSpan.FromSeconds(30);

    /// <summary>Longest wait between locally calculated Flex retries. A server-provided Retry-After is never shortened.</summary>
    public TimeSpan FlexMaxRetryDelay { get; init; } = TimeSpan.FromMinutes(2);

    /// <summary>Whether a Flex request that is still rate limited after <see cref="FlexMaxWait"/> is sent once more on the Standard tier.</summary>
    public bool FlexFallbackToDefaultTier { get; init; } = true;
}
