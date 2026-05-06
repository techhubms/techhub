namespace TechHub.Core.Configuration;

/// <summary>
/// Configuration options for the weekly roundup generation pipeline.
/// </summary>
public class RoundupGeneratorOptions
{
    /// <summary>Configuration section name.</summary>
    public const string SectionName = "RoundupGenerator";

    /// <summary>
    /// Whether scheduled roundup generation is enabled. Defaults to true.
    /// Set <c>RoundupGenerator__Enabled=false</c> via environment variable to disable scheduling
    /// in environments where automatic generation should not run (e.g. PR preview environments).
    /// Manual admin-triggered runs are unaffected by this setting.
    /// </summary>
    public bool Enabled { get; init; } = true;

    /// <summary>Hour of day (UTC) to run roundup generation. Default: 8 (8:00 AM UTC = ~9-10 AM Brussels).</summary>
    public int RunHourUtc { get; init; } = 8;

    /// <summary>
    /// Minimum number of articles per section. High-relevance articles are always included.
    /// If the total is below this threshold, medium then low articles are added to reach it.
    /// </summary>
    public int MinArticlesPerSection { get; init; } = 10;

    /// <summary>Whether the condensing step is enabled. When false, the pipeline skips AI condensing.</summary>
    public bool CondensingEnabled { get; init; } = true;

    /// <summary>Delay in seconds between AI API calls to prevent rate limiting.</summary>
    public int RateLimitDelaySeconds { get; init; } = 15;

    /// <summary>Maximum number of retries on transient AI failures.</summary>
    public int MaxRetries { get; init; } = 3;
}
