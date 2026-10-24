namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// AI-extracted metadata to assist in generating weekly roundups.
/// Stored as JSONB in the <c>ai_metadata</c> column on <c>content_items</c>.
/// </summary>
public sealed class RoundupMetadata
{
    /// <summary>Schema version written by the classification step.</summary>
    public const int CurrentSchemaVersion = 2;

    /// <summary>Roundup role: the article gets a full write-up in the roundup of that section.</summary>
    public const string RoleLead = "lead";

    /// <summary>Roundup role: the article only gets a one-line mention in the roundup of that section.</summary>
    public const string RoleBrief = "brief";

    /// <summary>Allowed values of <see cref="Significance"/>.</summary>
    public static readonly IReadOnlySet<string> ValidSignificance = new HashSet<string>(StringComparer.Ordinal)
    {
        "high", "medium", "low"
    };

    /// <summary>Allowed values of <see cref="ContentKind"/>.</summary>
    public static readonly IReadOnlySet<string> ValidContentKinds = new HashSet<string>(StringComparer.Ordinal)
    {
        "announcement", "tutorial", "analysis", "case-study", "reference", "event"
    };

    /// <summary>Allowed values of <see cref="Lifecycle"/>.</summary>
    public static readonly IReadOnlySet<string> ValidLifecycles = new HashSet<string>(StringComparer.Ordinal)
    {
        "new", "preview", "ga", "update", "deprecation", "retirement", "none"
    };

    /// <summary>
    /// 1 for the original summary-based metadata, 2 for the classification output with key facts and section roles.
    /// </summary>
    public int SchemaVersion { get; init; } = 1;

    /// <summary>2–5 concrete facts (versions, dates, numbers, names) taken from the article. The roundup writer's source of truth.</summary>
    public IReadOnlyList<string> KeyFacts { get; init; } = [];

    /// <summary>Impact on developers: "high", "medium" or "low". Orders articles inside a roundup.</summary>
    public string Significance { get; init; } = "medium";

    /// <summary>Kind of content: announcement, tutorial, analysis, case-study, reference or event.</summary>
    public string ContentKind { get; init; } = "announcement";

    /// <summary>Product lifecycle stage: new, preview, ga, update, deprecation, retirement or none.</summary>
    public string Lifecycle { get; init; } = "none";

    /// <summary>Official names of the 1–3 products the article is mainly about, most relevant first.</summary>
    public IReadOnlyList<string> Products { get; init; } = [];

    /// <summary>
    /// Roundup role per section slug: <see cref="RoleLead"/> or <see cref="RoleBrief"/>.
    /// The primary section is always <see cref="RoleLead"/>.
    /// </summary>
    public IReadOnlyDictionary<string, string> SectionRoles { get; init; } = new Dictionary<string, string>();

    /// <summary>
    /// 1–2 sentence summary of the article suitable for direct inclusion in a roundup (schema version 1 only).
    /// Written in a down-to-earth tone without marketing language.
    /// </summary>
    public string Summary { get; init; } = string.Empty;

    /// <summary>Key technical topics/concepts covered in the article (schema version 1 only).</summary>
    public IReadOnlyList<string> KeyTopics { get; init; } = [];

    /// <summary>
    /// How relevant this item is for a weekly roundup (schema version 1 only).
    /// Values: "high" (major announcement/release), "medium" (useful update), "low" (minor or niche).
    /// </summary>
    public string Relevance { get; init; } = "medium";

    /// <summary>
    /// The type of content, used for thematic grouping within roundup sections (schema version 1 only).
    /// Values: announcement | tutorial | update | guide | analysis | feature |
    ///         troubleshooting | case-study | news | preview | ga-release |
    ///         deprecation | migration | integration | comparison
    /// </summary>
    public string TopicType { get; init; } = "news";

    /// <summary>
    /// How much this item directly affects developer workflows (schema version 1 only).
    /// Values: "high", "medium", "low".
    /// </summary>
    public string ImpactLevel { get; init; } = "medium";

    /// <summary>
    /// How time-sensitive the content is for developers reading it this week (schema version 1 only).
    /// Values: "immediate", "this-week", "this-month", "long-term".
    /// </summary>
    public string TimeSensitivity { get; init; } = "this-week";
}
