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
/// </summary>
public static class MemoryCacheExtensions
{
    // In-flight factory calls per cache instance, so separate caches (for example in tests) never share work.
    private static readonly ConditionalWeakTable<IMemoryCache, ConcurrentDictionary<object, Task>> _inFlight = new();

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

        var flights = _inFlight.GetOrCreateValue(cache);

        while (true)
        {
            ct.ThrowIfCancellationRequested();

            var completion = new TaskCompletionSource<TItem>(TaskCreationOptions.RunContinuationsAsynchronously);
            var running = flights.GetOrAdd(key, completion.Task);

            if (ReferenceEquals(running, completion.Task))
            {
                return await RunFactoryAsync(cache, key, factory, flights, completion);
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

    private static async Task<TItem> RunFactoryAsync<TItem>(
        IMemoryCache cache,
        object key,
        Func<ICacheEntry, Task<TItem>> factory,
        ConcurrentDictionary<object, Task> flights,
        TaskCompletionSource<TItem> completion)
    {
        try
        {
            // GetOrCreateAsync checks the cache again, so a value stored between our miss and now is reused.
            var value = (await cache.GetOrCreateAsync(key, factory))!;
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
            ((ICollection<KeyValuePair<object, Task>>)flights).Remove(new KeyValuePair<object, Task>(key, completion.Task));
        }
    }
}
