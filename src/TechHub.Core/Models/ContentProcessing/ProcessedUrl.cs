namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// Tracks a URL that the content processing pipeline has attempted to process.
/// Used to prevent re-processing URLs that have already been handled.
/// </summary>
public sealed class ProcessedUrl
{
    /// <summary>The external URL that was processed.</summary>
    public required string ExternalUrl { get; init; }

    /// <summary>Processing outcome: "succeeded" or "failed".</summary>
    public required string Status { get; init; }

    /// <summary>Error message when status is "failed" (null on success).</summary>
    public string? ErrorMessage { get; init; }

    /// <summary>YouTube tags fetched from the external API (null for non-YouTube items).</summary>
    public IReadOnlyList<string>? YouTubeTags { get; init; }

    /// <summary>When this URL was first processed.</summary>
    public DateTimeOffset ProcessedAt { get; init; }

    /// <summary>When this record was last updated.</summary>
    public DateTimeOffset UpdatedAt { get; init; }
}
