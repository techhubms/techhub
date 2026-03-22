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
    public string Summary { get; init; } = string.Empty;

    /// <summary>Key technical topics/concepts covered in the article (e.g. "Semantic Kernel", "MCP", "RAG").</summary>
    public IReadOnlyList<string> KeyTopics { get; init; } = [];

    /// <summary>
    /// How relevant this item is for a weekly roundup.
    /// Values: "high" (major announcement/release), "medium" (useful update), "low" (minor or niche).
    /// </summary>
    public string Relevance { get; init; } = "medium";
}
