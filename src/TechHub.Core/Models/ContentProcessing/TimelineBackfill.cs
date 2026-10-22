namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// What the timeline backfill job should scan.
/// </summary>
public sealed record TimelineBackfillRequest
{
    /// <summary>When <c>true</c> articles that were already scanned are scanned again.</summary>
    public bool Force { get; init; }

    /// <summary>Only articles published on or after this date (Europe/Brussels), or <c>null</c> for all.</summary>
    public DateOnly? PublishedAfter { get; init; }

    /// <summary>Only articles of this collection (e.g. "news"), or <c>null</c> for all.</summary>
    public string? Collection { get; init; }

    /// <summary>Maximum number of articles to scan in this run, or <c>null</c> for no limit.</summary>
    public int? Limit { get; init; }
}

/// <summary>Key of an article that still has to be scanned for timeline events.</summary>
public sealed record TimelineBackfillCandidate(string CollectionName, string Slug);

/// <summary>An already stored article, with what the extraction needs.</summary>
public sealed record TimelineBackfillItem
{
    /// <summary>Collection of the article.</summary>
    public required string CollectionName { get; init; }

    /// <summary>Slug of the article.</summary>
    public required string Slug { get; init; }

    /// <summary>Article title.</summary>
    public required string Title { get; init; }

    /// <summary>The stored excerpt. The article text continues after it, so events can be stated only here.</summary>
    public string Excerpt { get; init; } = string.Empty;

    /// <summary>The stored article text.</summary>
    public required string Content { get; init; }

    /// <summary>Canonical URL of the article.</summary>
    public required string ExternalUrl { get; init; }

    /// <summary>Name of the feed the article came from.</summary>
    public required string FeedName { get; init; }

    /// <summary>Publication date as Unix epoch seconds.</summary>
    public required long DateEpoch { get; init; }
}

/// <summary>Outcome of a timeline backfill run.</summary>
public sealed record TimelineBackfillResult
{
    /// <summary>Number of articles that matched the request.</summary>
    public int Candidates { get; init; }

    /// <summary>Articles that were scanned successfully.</summary>
    public int Scanned { get; init; }

    /// <summary>Scanned articles that contain at least one event.</summary>
    public int WithEvents { get; init; }

    /// <summary>Events stored.</summary>
    public int EventsStored { get; init; }

    /// <summary>Articles whose extraction failed. They are not marked as scanned and are picked up by the next run.</summary>
    public int Failed { get; init; }
}
