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
    public int IntervalMinutes { get; init; } = 60;

    /// <summary>Maximum age in days for RSS items. Items older than this are skipped.</summary>
    public int ItemAgeLimitDays { get; init; } = 365;

    /// <summary>Delay in milliseconds between HTTP requests to prevent rate limiting.</summary>
    public int RequestDelayMs { get; init; } = 10_000;

    /// <summary>HTTP request timeout in seconds for fetching article content.</summary>
    public int RequestTimeoutSeconds { get; init; } = 30;

    /// <summary>
    /// Whether the YoutubeExplode-based transcript fetcher is enabled.
    /// When both fetchers are enabled, YoutubeExplode serves as fallback after yt-dlp.
    /// </summary>
    public bool YouTubeExplodeEnabled { get; init; } = true;

    /// <summary>
    /// Whether the yt-dlp-based transcript fetcher is enabled.
    /// When both fetchers are enabled, yt-dlp is tried first with YoutubeExplode as fallback.
    /// When only yt-dlp is enabled, it is used as the primary (and only) fetcher.
    /// Requires yt-dlp to be installed and available on PATH.
    /// </summary>
    public bool YtDlpEnabled { get; init; } = true;

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

    /// <summary>
    /// Browser User-Agent header sent with YouTube requests (YoutubeExplode).
    /// Keep this in sync with a current Chrome stable release to avoid bot detection.
    /// Chrome uses a "reduced" UA format since v101: major version only, frozen OS/platform tokens.
    /// See <see href="https://developer.chrome.com/docs/privacy-security/user-agent-client-hints"/>.
    /// Must be configured explicitly — the content processor will fail at startup if empty.
    /// </summary>
    public required string BrowserUserAgent { get; init; }

    /// <summary>
    /// Persistent cookies to send with YouTube requests (YoutubeExplode).
    /// Semicolon-delimited "name=value" pairs, e.g. "PREF=tz=Europe.Amsterdam;VISITOR_PRIVACY_METADATA=CgJOT...".
    /// Populated from Key Vault secret <c>techhub-{env}-youtube-cookies</c> at runtime.
    /// </summary>
    public string YouTubeCookies { get; init; } = string.Empty;

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
