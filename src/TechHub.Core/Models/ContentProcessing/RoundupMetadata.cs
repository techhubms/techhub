namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// AI-extracted metadata to assist in generating weekly roundups.
/// Stored as JSONB in the <c>ai_metadata</c> column on <c>content_items</c>.
/// </summary>
public sealed class RoundupMetadata
{
    /// <summary>
    /// 1–2 sentence summary of the article suitable for direct inclusion in a roundup.
    /// Written in a down-to-earth tone without marketing language.
    /// </summary>
    public required string Summary { get; init; }

    /// <summary>Key technical topics/concepts covered in the article (e.g. "Semantic Kernel", "MCP", "RAG").</summary>
    public IReadOnlyList<string> KeyTopics { get; init; } = [];

    /// <summary>
    /// How relevant this item is for a weekly roundup.
    /// Values: "high" (major announcement/release), "medium" (useful update), "low" (minor or niche).
    /// </summary>
    public required string Relevance { get; init; }

    /// <summary>
    /// The type of content, used for thematic grouping within roundup sections.
    /// Values: announcement | tutorial | update | guide | analysis | feature |
    ///         troubleshooting | case-study | news | preview | ga-release |
    ///         deprecation | migration | integration | comparison
    /// </summary>
    public required string TopicType { get; init; }

    /// <summary>
    /// How much this item directly affects developer workflows.
    /// Values: "high" (direct impact on developer productivity), "medium" (useful update),
    ///         "low" (niche or minor).
    /// </summary>
    public required string ImpactLevel { get; init; }

    /// <summary>
    /// How time-sensitive the content is for developers reading it this week.
    /// Values: "immediate" (act now), "this-week" (relevant this week),
    ///         "this-month" (useful soon), "long-term" (reference material).
    /// </summary>
    public required string TimeSensitivity { get; init; }
}
