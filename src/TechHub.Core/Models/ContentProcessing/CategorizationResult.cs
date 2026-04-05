namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// Result of AI categorization, containing the optional processed item and the AI's explanation.
/// When <see cref="Item"/> is <c>null</c> and <see cref="IsFailure"/> is <c>false</c>, the AI intentionally excluded the content.
/// When <see cref="Item"/> is <c>null</c> and <see cref="IsFailure"/> is <c>true</c>, categorization failed
/// (empty response, parse error, content filter, validation failure, etc.).
/// </summary>
public sealed class CategorizationResult
{
    /// <summary>The processed content item, or <c>null</c> if the AI excluded it or categorization failed.</summary>
    public ProcessedContentItem? Item { get; init; }

    /// <summary>AI explanation for why the content was included or excluded, or a failure reason.</summary>
    public required string Explanation { get; init; }

    /// <summary>
    /// When <c>true</c>, categorization failed (the AI did not produce a valid, actionable response).
    /// Distinguished from intentional exclusions where the AI returns a valid "skip" response.
    /// </summary>
    public bool IsFailure { get; init; }
}
