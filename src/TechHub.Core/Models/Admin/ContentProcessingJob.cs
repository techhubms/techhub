namespace TechHub.Core.Models.Admin;

/// <summary>Status values for a <see cref="ContentProcessingJob"/>.</summary>
public static class ContentProcessingJobStatus
{
    public const string Running = "running";
    public const string Completed = "completed";
    public const string Failed = "failed";
    public const string Aborted = "aborted";
}

/// <summary>Job type constants for <see cref="ContentProcessingJob"/>.</summary>
public static class ContentProcessingJobType
{
    public const string ContentProcessing = "content-processing";
    public const string RoundupGeneration = "roundup-generation";
}

/// <summary>
/// Represents a single background job run (content processing or roundup generation).
/// </summary>
public sealed class ContentProcessingJob
{
    /// <summary>Database-generated primary key.</summary>
    public long Id { get; init; }

    /// <summary>When this job started.</summary>
    public DateTimeOffset StartedAt { get; init; }

    /// <summary>When this job completed (null if still running).</summary>
    public DateTimeOffset? CompletedAt { get; init; }

    /// <summary>Total wall-clock duration in milliseconds.</summary>
    public long? DurationMs { get; init; }

    /// <summary>Job status: "running", "completed", or "failed".</summary>
    public string Status { get; init; } = ContentProcessingJobStatus.Running;

    /// <summary>How the job was triggered: "scheduled" or "manual".</summary>
    public string TriggerType { get; init; } = "scheduled";

    /// <summary>Job type: "content-processing" or "roundup-generation".</summary>
    public string JobType { get; init; } = ContentProcessingJobType.ContentProcessing;

    /// <summary>Number of RSS feeds processed during this run.</summary>
    public int FeedsProcessed { get; init; }

    /// <summary>Number of new content items written to the database.</summary>
    public int ItemsAdded { get; init; }

    /// <summary>Number of items skipped (already exists, AI rejected, etc.).</summary>
    public int ItemsSkipped { get; init; }

    /// <summary>Number of items that encountered errors.</summary>
    public int ErrorCount { get; init; }

    /// <summary>Accumulated log output from the run.</summary>
    public string? LogOutput { get; init; }
}
