namespace TechHub.Api.Tests.TestSupport;

/// <summary>
/// Bounded polling for assertions on work done by background services that expose no completion hook.
/// </summary>
internal static class Eventually
{
    private static readonly TimeSpan _defaultPollInterval = TimeSpan.FromMilliseconds(50);

    /// <summary>
    /// Polls <paramref name="condition"/> until it returns <c>true</c> or <paramref name="timeout"/> elapses.
    /// Returns the result of a final check so a condition met right at the deadline is not missed.
    /// </summary>
    public static async Task<bool> WaitAsync(
        Func<Task<bool>> condition,
        TimeSpan timeout,
        CancellationToken ct,
        TimeSpan? pollInterval = null)
    {
        ArgumentNullException.ThrowIfNull(condition);

        if (await condition())
        {
            return true;
        }

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        timeoutCts.CancelAfter(timeout);
        using var timer = new PeriodicTimer(pollInterval ?? _defaultPollInterval);

        try
        {
            while (await timer.WaitForNextTickAsync(timeoutCts.Token))
            {
                if (await condition())
                {
                    return true;
                }
            }
        }
        catch (OperationCanceledException) when (!ct.IsCancellationRequested)
        {
            // Timeout elapsed (not a test cancellation) — fall through to the final check.
        }

        return await condition();
    }
}
