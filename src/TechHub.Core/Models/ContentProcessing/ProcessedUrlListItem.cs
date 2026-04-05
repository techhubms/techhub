namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// A processed URL entry with feed metadata.
/// Used for the admin processed URLs listing page.
/// </summary>
public sealed class ProcessedUrlListItem
{
    /// <summary>The external URL that was processed.</summary>
    public required string ExternalUrl { get; init; }

    /// <summary>Processing outcome: "succeeded" (AI included), "skipped" (AI excluded), or "failed" (error).</summary>
    public required string Status { get; init; }

    /// <summary>Error message when status is "failed" (null on success).</summary>
    public string? ErrorMessage { get; init; }

    /// <summary>AI explanation for why the content was included, excluded, or failed.</summary>
    public string? Reason { get; init; }

    /// <summary>Feed name from the RSS feed that produced this URL (null for legacy data).</summary>
    public string? FeedName { get; init; }

    /// <summary>Collection name from the RSS feed (null for legacy data).</summary>
    public string? CollectionName { get; init; }

    /// <summary>Slug of the associated content item (null when no content item exists, e.g. failed or skipped URLs).</summary>
    public string? Slug { get; init; }

    /// <summary>Whether a YouTube transcript was successfully fetched. Null for non-YouTube items.</summary>
    public bool? HasTranscript { get; init; }

    /// <summary>ID of the processing job that last touched this URL. Null for legacy data or after job purge.</summary>
    public long? JobId { get; init; }

    /// <summary>When this URL was first processed.</summary>
    public DateTimeOffset ProcessedAt { get; init; }

    /// <summary>When this record was last updated.</summary>
    public DateTimeOffset UpdatedAt { get; init; }
}
