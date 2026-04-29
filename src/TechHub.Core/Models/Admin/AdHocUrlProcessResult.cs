namespace TechHub.Core.Models.Admin;

/// <summary>
/// Outcome of a single ad-hoc URL processing request.
/// </summary>
public sealed class AdHocUrlProcessResult
{
    /// <summary>Outcome of the processing attempt.</summary>
    public required AdHocUrlProcessOutcome Outcome { get; init; }

    /// <summary>
    /// URL-friendly slug of the content item that was created, if outcome is <see cref="AdHocUrlProcessOutcome.Added"/>.
    /// </summary>
    public string? Slug { get; init; }

    /// <summary>Human-readable explanation from the AI or a summary of the failure reason.</summary>
    public required string Message { get; init; }
}

/// <summary>
/// Describes the outcome of processing a single URL.
/// </summary>
public enum AdHocUrlProcessOutcome
{
    /// <summary>The URL was successfully processed and written to the database.</summary>
    Added,

    /// <summary>The AI intentionally excluded the content (not relevant enough to include).</summary>
    Skipped,

    /// <summary>Processing failed (AI error, network error, invalid content, etc.).</summary>
    Failed
}
