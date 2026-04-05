namespace TechHub.Core.Configuration;

/// <summary>
/// Configuration options for the content processing pipeline.
/// </summary>
public class ContentProcessorOptions
{
    /// <summary>Configuration section name.</summary>
    public const string SectionName = "ContentProcessor";

    /// <summary>Whether scheduled content processing is enabled.</summary>
    public bool Enabled { get; init; } = true;

    /// <summary>How often to run the content processing pipeline (in minutes).</summary>
    public int IntervalMinutes { get; init; } = 15;

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

    /// <summary>
    /// Number of recent processing jobs to keep when purging old records.
    /// Older jobs beyond this count are deleted during each run.
    /// </summary>
    public int PurgeJobKeepCount { get; init; } = 500;

    /// <summary>
    /// Rules for automatically assigning subcollections to content items
    /// based on the feed name and video/article title pattern.
    /// </summary>
    public IReadOnlyList<SubcollectionRule> SubcollectionRules { get; init; } = [];

    /// <summary>Computed processing interval.</summary>
    public TimeSpan Interval => TimeSpan.FromMinutes(IntervalMinutes);
}

/// <summary>
/// A rule that assigns a subcollection to content items matching a feed name and title pattern.
/// </summary>
public class SubcollectionRule
{
    /// <summary>Feed name to match (exact, case-insensitive).</summary>
    public string FeedName { get; init; } = string.Empty;

    /// <summary>
    /// Title pattern with wildcard support. Use <c>*</c> to match any sequence of characters.
    /// Example: <c>Visual Studio Code and GitHub Copilot*</c>
    /// </summary>
    public string TitlePattern { get; init; } = string.Empty;

    /// <summary>Subcollection name to assign when the rule matches (e.g. "vscode-updates").</summary>
    public string Subcollection { get; init; } = string.Empty;
}
