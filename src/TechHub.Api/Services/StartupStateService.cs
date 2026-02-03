namespace TechHub.Api.Services;

/// <summary>
/// Tracks startup operations completion state for health checks.
/// Ensures the API only reports healthy after critical startup operations complete.
/// </summary>
public class StartupStateService
{
    public bool IsContentSyncCompleted { get; private set; }
    public bool IsMigrationsCompleted { get; private set; }
    public bool IsFullyStarted => IsMigrationsCompleted && IsContentSyncCompleted;

    public void MarkMigrationsCompleted() => IsMigrationsCompleted = true;
    public void MarkContentSyncCompleted() => IsContentSyncCompleted = true;
}
