namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// An article candidate read from the database for inclusion in a weekly roundup.
/// Populated from <c>content_items</c> using section boolean columns and AI metadata.
/// </summary>
public sealed class RoundupArticle
{
    /// <summary>Section this article was registered under (e.g. "ai", "azure", "dotnet").</summary>
    public required string SectionName { get; init; }

    /// <summary>Article title.</summary>
    public required string Title { get; init; }

    /// <summary>Canonical URL (external for news/blogs/community, internal path for videos/roundups).</summary>
    public required string ExternalUrl { get; init; }

    /// <summary>URL-friendly slug.</summary>
    public required string Slug { get; init; }

    /// <summary>Collection name (e.g. "news", "blogs", "videos").</summary>
    public required string CollectionName { get; init; }

    /// <summary>
    /// Whether the URL is internal (relative path).
    /// True for videos, roundups, and custom collections.
    /// </summary>
    public bool IsInternal { get; init; }

    /// <summary>
    /// 1-2 sentence AI-generated summary suitable for direct inclusion in a roundup.
    /// From <c>ai_metadata.roundup_summary</c>.
    /// </summary>
    public string Summary { get; init; } = string.Empty;

    /// <summary>
    /// Key technical topics covered (e.g. "MCP", "Semantic Kernel").
    /// From <c>ai_metadata.key_topics</c>.
    /// </summary>
    public IReadOnlyList<string> KeyTopics { get; init; } = [];

    /// <summary>
    /// Roundup relevance: "high", "medium", or "low".
    /// From <c>ai_metadata.roundup_relevance</c>.
    /// </summary>
    public string Relevance { get; init; } = "medium";

    /// <summary>
    /// Content type: "announcement", "tutorial", "update", "guide", etc.
    /// From <c>ai_metadata.topic_type</c>.
    /// </summary>
    public string TopicType { get; init; } = "news";

    /// <summary>
    /// Developer impact: "high", "medium", or "low".
    /// From <c>ai_metadata.impact_level</c>.
    /// </summary>
    public string ImpactLevel { get; init; } = "medium";

    /// <summary>
    /// Time sensitivity: "immediate", "this-week", "this-month", or "long-term".
    /// From <c>ai_metadata.time_sensitivity</c>.
    /// </summary>
    public string TimeSensitivity { get; init; } = "this-week";
}
