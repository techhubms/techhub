namespace TechHub.Infrastructure.Services;

/// <summary>
/// Remembers that the Flex tier had no capacity, so calls that follow shortly after do not each wait
/// out the full Flex retry window before falling back to the Standard tier.
/// </summary>
public sealed class AiFlexCapacityTracker
{
    private readonly TimeProvider _timeProvider;
    private long _unavailableUntilTicks;

    public AiFlexCapacityTracker(TimeProvider? timeProvider = null)
    {
        _timeProvider = timeProvider ?? TimeProvider.System;
    }

    /// <summary>Whether Flex was recently found without capacity.</summary>
    public bool IsUnavailable => _timeProvider.GetUtcNow().UtcTicks < Interlocked.Read(ref _unavailableUntilTicks);

    /// <summary>Records that Flex had no capacity; for <paramref name="cooldown"/> calls try Flex once and fall back without waiting.</summary>
    public void MarkUnavailable(TimeSpan cooldown)
    {
        if (cooldown > TimeSpan.Zero)
        {
            Interlocked.Exchange(ref _unavailableUntilTicks, (_timeProvider.GetUtcNow() + cooldown).UtcTicks);
        }
    }

    /// <summary>Records that Flex served a request.</summary>
    public void MarkAvailable() => Interlocked.Exchange(ref _unavailableUntilTicks, 0);
}
