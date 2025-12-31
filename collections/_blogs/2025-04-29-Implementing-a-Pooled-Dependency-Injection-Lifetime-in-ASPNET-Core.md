---
layout: "post"
title: "Implementing a Pooled Dependency Injection Lifetime in ASP.NET Core"
description: "Andrew Lock examines how to implement a 'pooled' dependency injection scope in ASP.NET Core, inspired by EF Core's DbContext pooling. The post details requirements, design decisions, implementation, testing, limitations, and considerations for pooling within the .NET dependency injection system."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/creating-a-pooled-dependency-injection-lifetime/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-04-29 09:00:00 +00:00
permalink: "/blogs/2025-04-29-Implementing-a-Pooled-Dependency-Injection-Lifetime-in-ASPNET-Core.html"
categories: ["Coding"]
tags: [".NET Core", "ASP.NET Core", "Coding", "DbContext Pooling", "Dependency Injection", "IResettableService", "Performance", "Pooled Services", "Blogs", "Scoped Services", "Service Lifetimes", "Singleton Services"]
tags_normalized: ["dotnet core", "aspdotnet core", "coding", "dbcontext pooling", "dependency injection", "iresettableservice", "performance", "pooled services", "blogs", "scoped services", "service lifetimes", "singleton services"]
---

In this in-depth post, Andrew Lock explores how to create a pooled dependency injection lifetime in ASP.NET Core, discussing design choices, implementation details, and broader implications.<!--excerpt_end-->

# Implementing a Pooled Dependency Injection Lifetime in ASP.NET Core

*By Andrew Lock*

This article builds upon [a previous discussion](/going-beyond-singleton-scoped-and-transient-lifetimes/) about non-standard dependency injection (DI) lifetimes in .NET. Inspired by [episode 36 of The Breakpoint Show](https://www.breakpoint.show/podcast/episode-036-episode-of-a-lifetime/) and EF Core's `DbContext` pooling, Andrew Lock explores a 'pooled' DI lifetime, presents its implementation, tests it, and discusses its practicality.

## Background: Standard and Alternative DI Lifetimes

When registering services in the .NET Core DI container, three core lifetimes are available:

- **Singleton:** Only one instance ever created.
- **Scoped:** New instance per DI 'scope' (typically per request).
- **Transient:** New instance every time it's needed.

Alternative theoretical lifetimes discussed include:

- **Tenant-scoped:** Singleton per tenant.
- **Time-based ('drifter')**: Singleton replaced periodically.
- **Pooled:** Instances are reused from a pool up to a maximum number.

For general introduction to DI lifetimes:

- [Microsoft Documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
- [Chapters 8 and 9 of ASP.NET Core in Action, Third Edition by Andrew Lock](https://livebook.manning.com/book/asp-net-core-in-action-third-edition/)

## Pooled Lifetime: Requirements

Andrew outlines the requirements for pooled lifetimes as follows:

- Pooled services should have scoped semantics: used for the entirety of a request but not shared between parallel requests.
- The pool is preferred when servicing requests: if no instance is available, a new one is created.
- When a scope is disposed, pooled services are returned to the pool.
- The DI container should maintain at most _N_ instances for each service. If the pool is full, returned instances are discarded (disposed if necessary).
- Pooled services must implement `IResettableService` with a `Reset()` method, ensuring they're safe to re-use.
- Aside from `IResettableService`, no additional requirements for pooled services.
- `IDisposable` is honored: non-pooled instances are disposed when appropriate.

To keep the prototype implementation simple:

- Returning a 'wrapper' type (like `IOptions<T>`) is allowed.
- Asynchronous support is ignored for now.
- Pool configuration is fixed (not dynamic).

Notably, Andrew did **not** use `ObjectPool<T>` but notes this as a valid alternative. ([See Microsoft docs](https://learn.microsoft.com/en-us/aspnet/core/performance/objectpool?view=aspnetcore-9.0))

## Implementation Details

There are five main components:

1. `IResettableService` — Interface with a single `Reset()` method.

    ```csharp
    public interface IResettableService
    {
        void Reset();
    }
    ```

2. `IPooledService<T>` — Interface to access an instance of a pooled service (like `IOptions<T>`).

    ```csharp
    public interface IPooledService<out T> where T : IResettableService
    {
        T Value { get; }
    }
    ```

3. `PooledService<T>` — Internal implementation that rents from the pool and returns to it on disposal.

    ```csharp
    internal class PooledService<T> : IPooledService<T>, IDisposable where T : IResettableService
    {
        private readonly DependencyPool<T> _pool;
        public PooledService(DependencyPool<T> pool)
        {
            _pool = pool;
            Value = _pool.Rent();
        }
        public T Value { get; }
        void IDisposable.Dispose()
        {
            _pool.Return(Value);
        }
    }
    ```

4. `DependencyPool<T>` — Manages pooling logic, ensuring no more than the max instances are stored.

    ```csharp
    internal class DependencyPool<T>(IServiceProvider provider) : IDisposable where T : IResettableService
    {
        private int _count = 0;
        private int _maxPoolSize = 3;
        private readonly ConcurrentQueue<T> _pool = new();
        private readonly Func<T> _factory = () => ActivatorUtilities.CreateInstance<T>(provider);

        public T Rent()
        {
            if (_pool.TryDequeue(out var service))
            {
                Interlocked.Decrement(ref _count);
                return service;
            }
            return _factory();
        }
        public void Return(T service)
        {
            if (Interlocked.Increment(ref _count) <= _maxPoolSize)
            {
                service.Reset();
                _pool.Enqueue(service);
            }
            else
            {
                Interlocked.Decrement(ref _count);
                (service as IDisposable)?.Dispose();
            }
        }
        public void Dispose()
        {
            _maxPoolSize = 0;
            while (_pool.TryDequeue(out var service))
            {
                (service as IDisposable)?.Dispose();
            }
        }
    }
    ```

5. `PoolingExtensions` — Registers the pooling lifetime extension with DI.

    ```csharp
    public static class PoolingExtensions
    {
        public static IServiceCollection AddScopedPooling<T>(this IServiceCollection services)
            where T : class, IResettableService
        {
            services.TryAddSingleton<DependencyPool<T>>();
            services.TryAddScoped<IPooledService<T>, PooledService<T>>();
            return services;
        }
    }
    ```

**Important note:** Do not register `T` itself in the container directly; always require access through `IPooledService<T>`. Attempting to inject `T` directly results in the DI container disposing it at the end of the scope, which undermines pooling.

## Testing the Implementation

Andrew provides a `TestService` implementing `IResettableService` and `IDisposable`, with unique IDs and logging in `Reset()` and `Dispose()`:

```csharp
public class TestService : IResettableService, IDisposable
{
    private static int _id = 0;
    public int Id { get; } = Interlocked.Increment(ref _id);
    public void Dispose() => Console.WriteLine($"Disposing service: {Id}");
    public void Reset() => Console.WriteLine($"Resetting service: {Id}");
}
```

A test then creates multiple parallel scopes, retrieves pooled services, and observes their behavior, such as which IDs are reused and when instances are reset versus disposed. Example output:

```
Generating scopes A
Received value: 1
Received value: 2
Received value: 3
Received value: 4
Received value: 5
Resetting service: 1
Resetting service: 2
Resetting service: 3
Disposing service: 4
Disposing service: 5

Generating scopes B
Received value: 1
Received value: 2
Received value: 3
Received value: 6
Received value: 7
...
```

## Limitations and Considerations

- **Usability:** Unlike EF Core's `DbContext`, consumer code must use `IPooledService<T>`, introducing an extra level of indirection.
- **Lifetime Constraints:** Pooled services cannot depend on scoped DI services; only singleton or transient dependencies are safe.
- **Simplicity:** The pool is first-come-first-served and non-configurable (though extensible).

### Is pooling worth it?

Pooling can reduce allocations/GC pressure for large or expensive objects but is a form of custom memory management. If `Reset()` is more expensive than garbage collection, or if pooled objects unintentionally hold references to short-lived objects, performance can degrade. Microsoft documentation and .NET insiders recommend careful benchmark-driven evaluation.

## Conclusion

Andrew Lock's exploration of pooled dependency injection lifetimes in ASP.NET Core shows both conceptual interest and practical caution. While possible, such pooling should be applied judiciously and with thorough performance consideration.

---

*For further reading and implementation samples, see the original post and referenced podcasts/documents as linked above.*

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/creating-a-pooled-dependency-injection-lifetime/)
