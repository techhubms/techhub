namespace TechHub.Core.Models.Admin;

/// <summary>
/// Represents a toggleable background job setting stored in the database.
/// </summary>
public class BackgroundJobSetting
{
    /// <summary>Unique name identifying the background job (e.g., "ContentProcessor").</summary>
    public required string JobName { get; init; }

    /// <summary>Whether the background job's scheduled runs are enabled.</summary>
    public bool Enabled { get; init; }

    /// <summary>Human-readable description of what the job does.</summary>
    public required string Description { get; init; }
}
