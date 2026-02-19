namespace TechHub.Api.Services;

/// <summary>
/// Tracks startup operations completion state for health checks.
/// Ensures the API only reports healthy after critical startup operations complete.
/// </summary>
public class StartupStateService
{
    private readonly TaskCompletionSource _startupCompleted = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public bool IsContentSyncCompleted { get; private set; }
    public bool IsMigrationsCompleted { get; private set; }
    public bool IsFullyStarted => IsMigrationsCompleted && IsContentSyncCompleted;

    /// <summary>
    /// Gets a task that completes when all startup operations have finished.
    /// Useful for integration tests that need to wait for startup before making requests.
    /// </summary>
    public Task StartupTask => _startupCompleted.Task;

    public void MarkMigrationsCompleted() => IsMigrationsCompleted = true;

    public void MarkContentSyncCompleted()
    {
        IsContentSyncCompleted = true;
        _startupCompleted.TrySetResult();
    }
}
