using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Caching.Memory;

namespace TechHub.Infrastructure.Data;

/// <summary>
/// Single-flight cache population for <see cref="IMemoryCache"/>.
/// The built-in <c>GetOrCreateAsync</c> lets every concurrent caller that misses
/// run the factory itself, so a burst of requests after a restart or a cache clear runs the same
/// database query many times at once. These helpers let one caller run the factory while the
/// others wait for its result.
/// Invalidate through <see cref="ClearSingleFlight"/>: it starts a new generation, so lookups that were
/// already running when the data changed can neither be joined by later callers nor publish their result.
/// </summary>
public static class MemoryCacheExtensions
{
    // Flight state per cache instance, so separate caches (for example in tests) never share work.
    private static readonly ConditionalWeakTable<IMemoryCache, FlightState> _states = new();

    /// <summary>
    /// Returns the cached value for <paramref name="key"/>. On a miss, exactly one concurrent caller runs
    /// <paramref name="factory"/> and the others await its result. A factory failure propagates to all
    /// waiting callers; nothing is cached, so the next call retries. If the caller running the factory
    /// is cancelled, a waiting caller takes over instead of inheriting the cancellation.
    /// </summary>
    public static async Task<TItem> GetOrCreateSingleFlightAsync<TItem>(
        this IMemoryCache cache,
        object key,
        Func<ICacheEntry, Task<TItem>> factory,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(cache);
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(factory);

        if (cache.TryGetValue(key, out TItem? cached))
        {
            return cached!;
        }

        var state = _states.GetValue(cache, static _ => new FlightState());

        while (true)
        {
            ct.ThrowIfCancellationRequested();

            // Re-read every attempt: an invalidation moves new callers to a fresh generation.
            var generation = state.Current;
            var completion = new TaskCompletionSource<TItem>(TaskCreationOptions.RunContinuationsAsynchronously);
            var running = generation.Flights.GetOrAdd(key, completion.Task);

            if (ReferenceEquals(running, completion.Task))
            {
                return await RunFactoryAsync(cache, key, factory, state, generation, completion);
            }

            try
            {
                return await ((Task<TItem>)running).WaitAsync(ct);
            }
            catch (OperationCanceledException) when (!ct.IsCancellationRequested)
            {
                // The caller that was running the factory was cancelled, not this one: retry as the new runner.
            }
        }
    }

    /// <summary>
    /// Clears <paramref name="cache"/> and starts a new single-flight generation. Lookups that started before
    /// this call are not joined by later callers and do not publish their result, so a read that began
    /// before a write can never repopulate the cache with stale data.
    /// </summary>
    public static void ClearSingleFlight(this MemoryCache cache)
    {
        ArgumentNullException.ThrowIfNull(cache);

        var state = _states.GetValue(cache, static _ => new FlightState());
        lock (state.Gate)
        {
            state.Current = new Generation();
            cache.Clear();
        }
    }

    private static async Task<TItem> RunFactoryAsync<TItem>(
        IMemoryCache cache,
        object key,
        Func<ICacheEntry, Task<TItem>> factory,
        FlightState state,
        Generation generation,
        TaskCompletionSource<TItem> completion)
    {
        try
        {
            var value = await CreateAndPublishAsync(cache, key, factory, state, generation);
            completion.SetResult(value);
            return value;
        }
        catch (OperationCanceledException ex)
        {
            completion.SetCanceled(ex.CancellationToken);
            throw;
        }
        catch (Exception ex)
        {
            completion.SetException(ex);
            throw;
        }
        finally
        {
            // Remove only our own entry so a newer flight for the same key is left alone.
            ((ICollection<KeyValuePair<object, Task>>)generation.Flights).Remove(new KeyValuePair<object, Task>(key, completion.Task));
        }
    }

    private static async Task<TItem> CreateAndPublishAsync<TItem>(
        IMemoryCache cache,
        object key,
        Func<ICacheEntry, Task<TItem>> factory,
        FlightState state,
        Generation generation)
    {
        // A value stored between our miss and now is reused.
        if (cache.TryGetValue(key, out TItem? existing))
        {
            return existing!;
        }

        // An entry is committed to the cache on Dispose, and only when its value was set.
        var entry = cache.CreateEntry(key);
        try
        {
            var value = await factory(entry);

            // Publishing and invalidating share a gate, so a result computed before an invalidation can't land after it.
            lock (state.Gate)
            {
                if (ReferenceEquals(state.Current, generation))
                {
                    entry.Value = value;
                }

                entry.Dispose();
            }

            return value;
        }
        finally
        {
            entry.Dispose();
        }
    }

    private sealed class FlightState
    {
        public object Gate { get; } = new();

        public Generation Current { get; set; } = new();
    }

    private sealed class Generation
    {
        public ConcurrentDictionary<object, Task> Flights { get; } = new();
    }
}
