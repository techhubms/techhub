namespace TechHub.Core.Models.Admin;

/// <summary>
/// Request body for ad-hoc URL processing outside the RSS pipeline.
/// </summary>
public sealed record AdHocUrlProcessRequest
{
    /// <summary>The URL to process (must be a valid HTTP/HTTPS URL).</summary>
    public required string Url { get; init; }

    /// <summary>
    /// Target collection name (e.g. "blogs", "news", "videos", "community").
    /// Must be a known collection name.
    /// </summary>
    public required string CollectionName { get; init; }

    /// <summary>
    /// Display name used as the feed origin (e.g. "GitHub Blog", "YouTube").
    /// Used in processed_urls tracking and content attribution.
    /// Defaults to "TechHub" when omitted or left blank.
    /// </summary>
    public string FeedName { get; init; } = "TechHub";

    /// <summary>
    /// Optional hint for the AI about the expected title of the content.
    /// The AI will use this as a starting point but will extract the final title
    /// from the fetched page content. Not used as-is.
    /// </summary>
    public string? TitleHint { get; init; }

    /// <summary>
    /// When <c>true</c>, assigns this video to the <c>ghc-features</c> subcollection.
    /// Requires <see cref="CollectionName"/> to be "videos" and a YouTube URL.
    /// </summary>
    public bool IsGhcFeature { get; init; }

    /// <summary>
    /// Subscription tier names for a ghc-features video.
    /// Required when <see cref="IsGhcFeature"/> is <c>true</c>.
    /// Valid values: Free, Student, Pro, Business, Pro+, Enterprise.
    /// </summary>
    public IReadOnlyList<string> Plans { get; init; } = [];

    /// <summary>
    /// Whether this ghc-feature is available in GitHub Enterprise Server.
    /// Only relevant when <see cref="IsGhcFeature"/> is <c>true</c>.
    /// </summary>
    public bool GhesSupport { get; init; }
}
