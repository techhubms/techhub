namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// An article candidate read from the database for inclusion in a weekly roundup.
/// Populated from <c>content_items</c> using section boolean columns and AI metadata.
/// </summary>
public sealed record RoundupArticle
{
    /// <summary>Section this article was registered under (e.g. "ai", "azure", "dotnet").</summary>
    public required string SectionName { get; init; }

    /// <summary>Primary section of the article, where it always gets a full write-up.</summary>
    public string PrimarySectionName { get; init; } = string.Empty;

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

    /// <summary>Whether this article is missing current (schema version 2) AI metadata and needs on-the-fly backfill.</summary>
    public bool NeedsAiMetadata { get; init; }

    /// <summary>Concrete facts (versions, dates, numbers, names) from the article. From <c>ai_metadata.key_facts</c> (schema version 2).</summary>
    public IReadOnlyList<string> KeyFacts { get; init; } = [];

    /// <summary>Impact on developers: "high", "medium" or "low". From <c>ai_metadata.significance</c> (schema version 2).</summary>
    public string Significance { get; init; } = "medium";

    /// <summary>Kind of content. From <c>ai_metadata.content_kind</c> (schema version 2).</summary>
    public string ContentKind { get; init; } = "announcement";

    /// <summary>Product lifecycle stage. From <c>ai_metadata.lifecycle</c> (schema version 2).</summary>
    public string Lifecycle { get; init; } = "none";

    /// <summary>Official names of the products the article is about. From <c>ai_metadata.products</c> (schema version 2).</summary>
    public IReadOnlyList<string> Products { get; init; } = [];

    /// <summary>
    /// Roundup role per section slug ("lead" or "brief"). From <c>ai_metadata.section_roles</c> (schema version 2).
    /// Empty for articles without version 2 metadata.
    /// </summary>
    public IReadOnlyDictionary<string, string> SectionRoles { get; init; } = new Dictionary<string, string>();

    /// <summary>
    /// Returns a copy of this article with the fields derived from <paramref name="metadata"/>.
    /// The summary-based fields are derived from the key facts so the roundup writer keeps working on them.
    /// </summary>
    public RoundupArticle WithMetadata(RoundupMetadata metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);

        return this with
        {
            NeedsAiMetadata = false,
            KeyFacts = metadata.KeyFacts,
            Significance = metadata.Significance,
            ContentKind = metadata.ContentKind,
            Lifecycle = metadata.Lifecycle,
            Products = metadata.Products,
            SectionRoles = metadata.SectionRoles,
            Summary = string.Join(" ", metadata.KeyFacts),
            KeyTopics = metadata.Products,
            Relevance = metadata.Significance,
            TopicType = metadata.ContentKind,
            ImpactLevel = metadata.Significance
        };
    }

    /// <summary>Stored excerpt. The article text continues after it, so a milestone can be stated only here.</summary>
    public string Excerpt { get; init; } = string.Empty;

    /// <summary>Full article content (for on-the-fly AI categorization when ai_metadata is NULL).</summary>
    public string Content { get; init; } = string.Empty;

    /// <summary>Feed name (for on-the-fly AI categorization when ai_metadata is NULL).</summary>
    public string FeedName { get; init; } = string.Empty;

    /// <summary>Unix epoch publication date (for on-the-fly AI categorization when ai_metadata is NULL).</summary>
    public long DateEpoch { get; init; }
}
