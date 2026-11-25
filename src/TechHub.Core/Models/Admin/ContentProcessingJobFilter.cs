namespace TechHub.Core.Models.Admin;

/// <summary>Trigger values for a <see cref="ContentProcessingJob"/>.</summary>
public static class ContentProcessingJobTrigger
{
    public const string Scheduled = "scheduled";
    public const string Manual = "manual";
}

/// <summary>
/// Optional filters for the job history. A <c>null</c> or empty property does not filter on that field.
/// </summary>
/// <param name="Status">One of <see cref="ContentProcessingJobStatus"/>.</param>
/// <param name="JobType">One of <see cref="ContentProcessingJobType"/>.</param>
/// <param name="TriggerType">One of <see cref="ContentProcessingJobTrigger"/>.</param>
public sealed record ContentProcessingJobFilter(string? Status = null, string? JobType = null, string? TriggerType = null)
{
    /// <summary>True when at least one property filters the history.</summary>
    public bool IsActive =>
        !string.IsNullOrEmpty(Status) || !string.IsNullOrEmpty(JobType) || !string.IsNullOrEmpty(TriggerType);
}
