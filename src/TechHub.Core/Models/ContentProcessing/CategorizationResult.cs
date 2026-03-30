namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// Result of AI categorization, containing the optional processed item and the AI's explanation.
/// When <see cref="Item"/> is <c>null</c>, the AI decided to exclude the content.
/// </summary>
public sealed class CategorizationResult
{
    /// <summary>The processed content item, or <c>null</c> if the AI excluded it.</summary>
    public ProcessedContentItem? Item { get; init; }

    /// <summary>AI explanation for why the content was included or excluded.</summary>
    public required string Explanation { get; init; }
}
