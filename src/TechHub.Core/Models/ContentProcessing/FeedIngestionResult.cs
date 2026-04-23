namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// Result of ingesting a single RSS/Atom feed.
/// Distinguishes between a successful fetch (possibly with 0 items) and a failed download/parse.
/// </summary>
public sealed class FeedIngestionResult
{
    /// <summary>Items parsed from the feed (empty on failure).</summary>
    public IReadOnlyList<RawFeedItem> Items { get; init; } = [];

    /// <summary>Whether the feed download or parsing failed entirely.</summary>
    public bool Failed { get; init; }

    /// <summary>Error message when <see cref="Failed"/> is <c>true</c>.</summary>
    public string? ErrorMessage { get; init; }

    /// <summary>Creates a successful result with the parsed items.</summary>
    public static FeedIngestionResult Success(IReadOnlyList<RawFeedItem> items) => new() { Items = items };

    /// <summary>Creates a failure result indicating the feed could not be fetched or parsed.</summary>
    public static FeedIngestionResult Failure(string errorMessage) => new() { Failed = true, ErrorMessage = errorMessage };
}
