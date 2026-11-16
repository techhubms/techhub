using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using TechHub.Infrastructure.Data;

namespace TechHub.Infrastructure.Tests.Data;

public class MemoryCacheExtensionsTests
{
    [Fact]
    public async Task GetOrCreateSingleFlightAsync_ConcurrentMisses_RunFactoryOnce()
    {
        // Arrange
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var calls = 0;
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        async Task<string> Factory(ICacheEntry entry)
        {
            Interlocked.Increment(ref calls);
            await release.Task;
            return "value";
        }

        // Act — start all callers before the factory is allowed to finish
        var callers = Enumerable.Range(0, 50)
            .Select(_ => cache.GetOrCreateSingleFlightAsync("key", Factory, CancellationToken.None))
            .ToArray();
        release.SetResult();
        var results = await Task.WhenAll(callers);

        // Assert
        calls.Should().Be(1);
        results.Should().AllBe("value");
    }

    [Fact]
    public async Task GetOrCreateSingleFlightAsync_CachedValue_SkipsFactory()
    {
        // Arrange
        using var cache = new MemoryCache(new MemoryCacheOptions());
        await cache.GetOrCreateSingleFlightAsync("key", _ => Task.FromResult(1), CancellationToken.None);

        // Act
        var value = await cache.GetOrCreateSingleFlightAsync("key", _ => Task.FromResult(2), CancellationToken.None);

        // Assert
        value.Should().Be(1);
    }

    [Fact]
    public async Task GetOrCreateSingleFlightAsync_NullResult_IsCached()
    {
        // Arrange
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var calls = 0;

        Task<string?> Factory(ICacheEntry entry)
        {
            calls++;
            return Task.FromResult<string?>(null);
        }

        // Act
        var first = await cache.GetOrCreateSingleFlightAsync("key", Factory, CancellationToken.None);
        var second = await cache.GetOrCreateSingleFlightAsync("key", Factory, CancellationToken.None);

        // Assert — a lookup that found nothing must not hit the database on every request
        first.Should().BeNull();
        second.Should().BeNull();
        calls.Should().Be(1);
    }

    [Fact]
    public async Task GetOrCreateSingleFlightAsync_FactoryFails_FailsWaitersAndRetriesOnNextCall()
    {
        // Arrange
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var calls = 0;

        async Task<int> Factory(ICacheEntry entry)
        {
            if (Interlocked.Increment(ref calls) == 1)
            {
                await release.Task;
                throw new InvalidOperationException("database unavailable");
            }

            return 42;
        }

        // Act
        var runner = cache.GetOrCreateSingleFlightAsync("key", Factory, CancellationToken.None);
        var waiter = cache.GetOrCreateSingleFlightAsync("key", Factory, CancellationToken.None);
        release.SetResult();
        var runnerAct = async () => await runner;
        var waiterAct = async () => await waiter;

        // Assert — nothing is cached, so the next call runs the factory again
        await runnerAct.Should().ThrowAsync<InvalidOperationException>();
        await waiterAct.Should().ThrowAsync<InvalidOperationException>();
        var retry = await cache.GetOrCreateSingleFlightAsync("key", Factory, CancellationToken.None);
        retry.Should().Be(42);
        calls.Should().Be(2);
    }

    [Fact]
    public async Task GetOrCreateSingleFlightAsync_RunnerCancelled_WaiterTakesOver()
    {
        // Arrange
        using var cache = new MemoryCache(new MemoryCacheOptions());
        using var runnerCts = new CancellationTokenSource();
        var runnerStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var calls = 0;

        async Task<string> Factory(ICacheEntry entry)
        {
            if (Interlocked.Increment(ref calls) == 1)
            {
                runnerStarted.SetResult();
                await Task.Delay(Timeout.Infinite, runnerCts.Token);
            }

            return "from-waiter";
        }

        var runner = cache.GetOrCreateSingleFlightAsync("key", Factory, runnerCts.Token);
        await runnerStarted.Task;
        var waiter = cache.GetOrCreateSingleFlightAsync("key", Factory, CancellationToken.None);

        // Act — the client of the running request disconnects
        await runnerCts.CancelAsync();

        // Assert — the waiter is not cancelled with it
        var runnerAct = async () => await runner;
        await runnerAct.Should().ThrowAsync<OperationCanceledException>();
        (await waiter).Should().Be("from-waiter");
    }

    [Fact]
    public async Task GetOrCreateSingleFlightAsync_WaiterCancelled_StopsWaitingWithoutAffectingRunner()
    {
        // Arrange
        using var cache = new MemoryCache(new MemoryCacheOptions());
        using var waiterCts = new CancellationTokenSource();
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var runnerStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        async Task<string> Factory(ICacheEntry entry)
        {
            runnerStarted.TrySetResult();
            await release.Task;
            return "value";
        }

        var runner = cache.GetOrCreateSingleFlightAsync("key", Factory, CancellationToken.None);
        await runnerStarted.Task;
        var waiter = cache.GetOrCreateSingleFlightAsync("key", Factory, waiterCts.Token);

        // Act
        await waiterCts.CancelAsync();
        release.SetResult();

        // Assert
        var waiterAct = async () => await waiter;
        await waiterAct.Should().ThrowAsync<OperationCanceledException>();
        (await runner).Should().Be("value");
    }

    [Fact]
    public async Task GetOrCreateSingleFlightAsync_SeparateCaches_DoNotShareWork()
    {
        // Arrange
        using var first = new MemoryCache(new MemoryCacheOptions());
        using var second = new MemoryCache(new MemoryCacheOptions());
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var calls = 0;

        async Task<int> Factory(ICacheEntry entry)
        {
            Interlocked.Increment(ref calls);
            await release.Task;
            return 1;
        }

        // Act
        var a = first.GetOrCreateSingleFlightAsync("key", Factory, CancellationToken.None);
        var b = second.GetOrCreateSingleFlightAsync("key", Factory, CancellationToken.None);
        release.SetResult();
        await Task.WhenAll(a, b);

        // Assert
        calls.Should().Be(2);
    }

    [Fact]
    public async Task ClearSingleFlight_LookupStartedBeforeInvalidation_IsNeitherJoinedNorPublished()
    {
        // Arrange — a lookup that began before the data changed is still running when the cache is invalidated
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var calls = 0;

        async Task<string> Factory(ICacheEntry entry)
        {
            var call = Interlocked.Increment(ref calls);
            if (call == 1)
            {
                started.SetResult();
                await release.Task;
                return "stale";
            }

            return "fresh";
        }

        var old = cache.GetOrCreateSingleFlightAsync("key", Factory, CancellationToken.None);
        await started.Task;

        // Act
        cache.ClearSingleFlight();
        var afterInvalidation = await cache.GetOrCreateSingleFlightAsync("key", Factory, CancellationToken.None);
        release.SetResult();
        await old;

        // Assert — the later caller did not join the old lookup, and the old result did not overwrite the cache
        afterInvalidation.Should().Be("fresh");
        cache.TryGetValue("key", out string? cached).Should().BeTrue();
        cached.Should().Be("fresh");
    }

    [Fact]
    public async Task ClearSingleFlight_LookupStartedBeforeInvalidation_DoesNotRepopulateEmptyCache()
    {
        // Arrange
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        async Task<string> Factory(ICacheEntry entry)
        {
            started.SetResult();
            await release.Task;
            return "stale";
        }

        var old = cache.GetOrCreateSingleFlightAsync("key", Factory, CancellationToken.None);
        await started.Task;

        // Act
        cache.ClearSingleFlight();
        release.SetResult();
        await old;

        // Assert
        cache.TryGetValue("key", out _).Should().BeFalse();
    }

    [Fact]
    public void SetIfCurrent_NoInvalidationSinceTheGeneration_StoresTheValue()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var generation = cache.GetGeneration();

        var stored = cache.SetIfCurrent(generation, "key", "value", TimeSpan.FromMinutes(1));

        stored.Should().BeTrue();
        cache.TryGetValue("key", out string? cached).Should().BeTrue();
        cached.Should().Be("value");
    }

    [Fact]
    public void SetIfCurrent_InvalidationSinceTheGeneration_DoesNotStoreTheValue()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var generation = cache.GetGeneration();

        cache.ClearSingleFlight();
        var stored = cache.SetIfCurrent(generation, "key", "stale", TimeSpan.FromMinutes(1));

        stored.Should().BeFalse();
        cache.TryGetValue("key", out _).Should().BeFalse();
    }
}
