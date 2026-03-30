namespace TechHub.Core.Configuration;

/// <summary>
/// Configuration options for the weekly roundup generation pipeline.
/// </summary>
public class RoundupGeneratorOptions
{
    /// <summary>Configuration section name.</summary>
    public const string SectionName = "RoundupGenerator";

    /// <summary>Whether the roundup generator is enabled.</summary>
    public bool Enabled { get; init; } = true;

    /// <summary>Hour of day (UTC) to run roundup generation. Default: 8 (8:00 AM UTC = ~9-10 AM Brussels).</summary>
    public int RunHourUtc { get; init; } = 8;

    /// <summary>
    /// Minimum number of high-relevance articles per section before medium items are added.
    /// If a section has fewer than this many "high" articles, "medium" items are also included.
    /// </summary>
    public int MinHighArticlesPerSection { get; init; } = 3;

    /// <summary>
    /// Minimum total articles per section (high + medium) before low items are added.
    /// If a section has fewer than this many "high" + "medium" articles, "low" items are also included.
    /// </summary>
    public int MinTotalArticlesPerSection { get; init; } = 5;

    /// <summary>Delay in seconds between AI API calls to prevent rate limiting.</summary>
    public int RateLimitDelaySeconds { get; init; } = 15;

    /// <summary>Maximum number of retries on transient AI failures.</summary>
    public int MaxRetries { get; init; } = 3;
}
