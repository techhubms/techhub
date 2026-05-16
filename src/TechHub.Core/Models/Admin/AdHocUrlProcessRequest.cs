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
    /// Optional manually provided transcript for YouTube video items.
    /// When set, the transcript is used as the content for AI processing.
    /// When null, the item is processed using only available metadata (tags, description, etc.).
    /// </summary>
    public string? Transcript { get; init; }

    /// <summary>
    /// Optional author override. When set, this value is used as the author of the content item
    /// instead of the author extracted by the AI from the page content.
    /// </summary>
    public string? AuthorOverride { get; init; }
}
