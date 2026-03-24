namespace TechHub.Core.Configuration;

/// <summary>
/// Configuration options for the content processing pipeline.
/// </summary>
public class ContentProcessorOptions
{
    /// <summary>Configuration section name.</summary>
    public const string SectionName = "ContentProcessor";

    /// <summary>Whether the content processing pipeline is enabled.</summary>
    public bool Enabled { get; init; } = true;

    /// <summary>How often to run the content processing pipeline (in minutes).</summary>
    public int IntervalMinutes { get; init; } = 15;

    /// <summary>
    /// Path to the RSS feeds configuration JSON file.
    /// Supports absolute paths and paths relative to the application base directory.
    /// Defaults to "rss-feeds.json" next to the executable.
    /// </summary>
    public string RssFeedsConfigPath { get; init; } = "rss-feeds.json";

    /// <summary>Maximum age in days for RSS items. Items older than this are skipped.</summary>
    public int ItemAgeLimitDays { get; init; } = 365;

    /// <summary>Delay in milliseconds between HTTP requests to prevent rate limiting.</summary>
    public int RequestDelayMs { get; init; } = 10_000;

    /// <summary>HTTP request timeout in seconds for fetching article content.</summary>
    public int RequestTimeoutSeconds { get; init; } = 30;

    /// <summary>Maximum number of items to process per run (0 = unlimited).</summary>
    public int MaxItemsPerRun { get; init; }

    /// <summary>
    /// Maximum number of YouTube tags to accept from the external API.
    /// Videos with more tags than this are assumed to be SEO-spammed and their tags are ignored.
    /// Set to 0 to disable YouTube tag fetching entirely.
    /// </summary>
    public int MaxYouTubeTagCount { get; init; } = 15;

    /// <summary>
    /// Number of days to keep failed URL records before purging them for retry.
    /// Set to 0 to never purge (failed URLs are never retried).
    /// </summary>
    public int FailedUrlRetentionDays { get; init; } = 7;

    /// <summary>Computed processing interval.</summary>
    public TimeSpan Interval => TimeSpan.FromMinutes(IntervalMinutes);
}
