namespace TechHub.ContentProcessor.Options;

/// <summary>
/// Configuration options for the content processor worker.
/// </summary>
public class ContentProcessorOptions
{
    /// <summary>Configuration section name.</summary>
    public const string SectionName = "ContentProcessor";

    /// <summary>Whether the content processing pipeline is enabled.</summary>
    public bool Enabled { get; init; } = true;

    /// <summary>How often to run the content processing pipeline (in hours).</summary>
    public int IntervalHours { get; init; } = 1;

    /// <summary>Computed interval based on <see cref="IntervalHours"/>.</summary>
    public TimeSpan Interval => TimeSpan.FromHours(IntervalHours);

    /// <summary>
    /// Path to the RSS feeds configuration JSON file inside the container.
    /// Defaults to rss-feeds.json next to the executable.
    /// </summary>
    public string RssFeedsConfigPath { get; init; } = "rss-feeds.json";

    /// <summary>Maximum age in days for RSS items. Items older than this are skipped.</summary>
    public int ItemAgeLimitDays { get; init; } = 365;

    /// <summary>Delay in milliseconds between HTTP requests to prevent rate limiting.</summary>
    public int RequestDelayMs { get; init; } = 10_000;

    /// <summary>HTTP request timeout in seconds.</summary>
    public int RequestTimeoutSeconds { get; init; } = 30;

    /// <summary>Maximum number of items to process per run (0 = unlimited).</summary>
    public int MaxItemsPerRun { get; init; }
}
