namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// Result of a YouTube transcript fetch attempt.
/// Carries both the transcript text (when successful) and the failure reason (when not).
/// </summary>
public sealed record TranscriptResult
{
    /// <summary>The transcript text, or <see langword="null"/> if the fetch failed.</summary>
    public string? Text { get; init; }

    /// <summary>
    /// Human-readable reason why the transcript could not be fetched.
    /// <see langword="null"/> when the fetch succeeded.
    /// </summary>
    public string? FailureReason { get; init; }

    /// <summary>Whether a usable transcript was obtained.</summary>
    public bool IsSuccess => !string.IsNullOrWhiteSpace(Text);

    /// <summary>Creates a successful result with transcript text.</summary>
    public static TranscriptResult Success(string text) => new() { Text = text };

    /// <summary>Creates a failed result with a reason.</summary>
    public static TranscriptResult Failure(string reason) => new() { FailureReason = reason };
}
