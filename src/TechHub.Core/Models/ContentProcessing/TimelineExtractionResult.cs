namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// Result of extracting timeline events from an already stored article.
/// </summary>
public sealed class TimelineExtractionResult
{
    /// <summary>The events found; empty when the article states none or when <see cref="IsFailure"/> is <c>true</c>.</summary>
    public IReadOnlyList<TimelineEvent> Events { get; init; } = [];

    /// <summary>When <c>true</c> the extraction failed and the article must not be marked as scanned.</summary>
    public bool IsFailure { get; init; }

    /// <summary>Failure reason, or <c>null</c>.</summary>
    public string? Explanation { get; init; }
}
