namespace TechHub.Api.Services;

/// <summary>
/// Tracks startup operations completion state for health checks.
/// Ensures the API only reports healthy after critical startup operations complete.
/// </summary>
public class StartupStateService
{
    private readonly TaskCompletionSource _startupCompleted = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public bool IsStartupCompleted { get; private set; }

    /// <summary>
    /// Gets a task that completes when all startup operations have finished.
    /// Useful for integration tests that need to wait for startup before making requests.
    /// </summary>
    public Task StartupTask => _startupCompleted.Task;

    public void MarkStartupCompleted()
    {
        IsStartupCompleted = true;
        _startupCompleted.TrySetResult();
    }
}
