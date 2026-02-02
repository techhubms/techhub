namespace TechHub.Api.Services;

/// <summary>
/// Tracks startup operations completion state for health checks.
/// Ensures the API only reports healthy after critical startup operations complete.
/// </summary>
public class StartupStateService
{
    private bool _contentSyncCompleted;
    private bool _migrationsCompleted;

    public bool IsContentSyncCompleted => _contentSyncCompleted;
    public bool IsMigrationsCompleted => _migrationsCompleted;
    public bool IsFullyStarted => _migrationsCompleted && _contentSyncCompleted;

    public void MarkMigrationsCompleted() => _migrationsCompleted = true;
    public void MarkContentSyncCompleted() => _contentSyncCompleted = true;
}
