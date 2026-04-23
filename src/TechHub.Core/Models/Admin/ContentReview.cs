namespace TechHub.Core.Models.Admin;

/// <summary>
/// Status constants for content review records.
/// </summary>
public static class ContentReviewStatus
{
    public const string Pending = "pending";
    public const string Approved = "approved";
    public const string Rejected = "rejected";
}

/// <summary>
/// Change type constants for content review records.
/// </summary>
public static class ContentReviewChangeType
{
    public const string Tags = "tags";
    public const string Markdown = "markdown";
    public const string Author = "author";
    public const string Validation = "validation";
}

/// <summary>
/// A content fixer change awaiting admin review, showing before and after values.
/// </summary>
public sealed class ContentReview
{
    public long Id { get; init; }
    public required string Slug { get; init; }
    public required string CollectionName { get; init; }
    public required string ChangeType { get; init; }
    public required string OriginalValue { get; init; }
    public string FixedValue { get; set; } = string.Empty;
    public string Status { get; init; } = ContentReviewStatus.Pending;
    public long? JobId { get; init; }
    public string? PrimarySectionName { get; init; }
    public string? ExternalUrl { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? ReviewedAt { get; init; }
}

/// <summary>
/// Summary counts for the content review queue.
/// </summary>
public sealed class ContentReviewSummary
{
    public int Pending { get; init; }
    public int Approved { get; init; }
    public int Rejected { get; init; }
    public int Total => Pending + Approved + Rejected;
}
