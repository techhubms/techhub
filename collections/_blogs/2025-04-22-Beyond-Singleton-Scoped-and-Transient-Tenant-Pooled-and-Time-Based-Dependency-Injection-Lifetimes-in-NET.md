---
external_url: https://andrewlock.net/going-beyond-singleton-scoped-and-transient-lifetimes/
title: 'Beyond Singleton, Scoped, and Transient: Tenant, Pooled, and Time-Based Dependency Injection Lifetimes in .NET'
author: Andrew Lock
viewing_mode: external
feed_name: Andrew Lock's Blog
date: 2025-04-22 09:00:00 +00:00
tags:
- .NET
- ASP.NET Core
- Dependency Injection
- Factory Pattern
- Multi Tenancy
- Pooled Services
- Scoped
- Service Container
- Service Lifetimes
- Singleton
- Tenant Scoped
- Time Based Services
- Transient
section_names:
- coding
---
In this post, Andrew Lock investigates experimental dependency injection lifetimes in .NET beyond the standard singleton, scoped, and transient, providing practical implementations and discussing their tradeoffs.<!--excerpt_end-->

# Beyond Singleton, Scoped, and Transient: Tenant, Pooled, and Time-Based Dependency Injection Lifetimes in .NET

**Author:** Andrew Lock

This post delves into experimental and hypothetical dependency injection (DI) lifetimes beyond the well-established Singleton, Scoped, and Transient scopes in .NET, inspired by an episode of [The Breakpoint Show](https://www.breakpoint.show/podcast/episode-036-episode-of-a-lifetime/) that discussed wishes for more flexible and nuanced lifetimes. Alongside a summary of existing lifetimes, this article explains three new scopes—tenant, pooled, and time-based lifetimes—and provides a hands-on implementation for a time-based (drifter) lifetime.

## Standard Lifetimes in Microsoft.Extensions.DependencyInjection

When registering services with the .NET Core DI container, three standard lifetimes are available:

- **Singleton:** Only one instance is ever created for the application's lifetime.
- **Scoped:** An instance is created per DI scope (e.g., per web request in ASP.NET Core).
- **Transient:** A new instance is created every time the service is requested.

### Singleton

Singleton registration ensures a single instance is created and shared:

```csharp
builder.Services.AddSingleton(new SingletonClass1()); // explicit instance
builder.Services.AddSingleton<SingletonClass2>(); // container creates instance
builder.Services.AddSingleton(serviceProvider => new SingletonClass3()); // factory
```

### Scoped

A scoped service is unique per logical operation (request in ASP.NET Core):

```csharp
builder.Services.AddScoped<ScopedClass>();
builder.Services.AddScoped(serviceProvider => new ScopedClass2());

using (var scope = app.Services.CreateScope()) {
    var instance1 = scope.ServiceProvider.GetRequiredService<ScopedClass>();
    var instance2 = scope.ServiceProvider.GetRequiredService<ScopedClass>();
    // instance1 == instance2 (same scope)
}
```

### Transient

Transient services provide a new instance for every injection:

```csharp
builder.Services.AddTransient<TransientClass>();
builder.Services.AddTransient(serviceProvider => new TransientClass2());

using (var scope = app.Services.CreateScope()) {
    var service1 = scope.ServiceProvider.GetRequiredService<TransientClass>();
    var service2 = scope.ServiceProvider.GetRequiredService<TransientClass>();
    // service1 != service2, every time
}
```

## Hypothetical Lifetimes from The Breakpoint Show

The show discussed three non-standard, hypothetical lifetimes, each intended for advanced scenarios:

### 1. Tenant-Scoped Services

- **Purpose:** To have services act as a singleton per tenant, not per application.
- **Use case:** Multi-tenant apps that must segregate state or data between tenants.
- **Implementation:** Solutions exist (e.g., via tenant-rooted DI containers or libraries), with a notable explanation from Michael McKenna ([tenant-scoped services in ASP.NET Core](https://michael-mckenna.com/multi-tenant-asp-dot-net-8-tenant-services/)).

### 2. Pooled Services

- **Idea:** Inspired by EF Core's DbContext pooling, this would reduce allocations and potentially improve performance by recycling rather than always creating new service instances.
- **Tradeoff:** While pooling can boost efficiency, resetting state between usages can offset these benefits, so suitability is workload-dependent.
- **Planned Implementation:** To be detailed in the next post.

### 3. Time-Based (Drifter) Services

- **Concept:** Services behave like a scoped lifetime, but only within a specified time window—after which a new instance is created.
- **Analogy:** Similar to a cache with a sliding expiration.
- **Potential Use Cases:** Not always clear; possible applications where data needs to refresh periodically or disposable resources need regular cycling.
- **Caveat:** Not recommended where disposables are involved, due to complex lifetime management.

## Implementing a Simple Time-Based Lifetime Service

### Characteristics

- Requests within a time window get the same instance.
- After expiration, new instances are created as needed.

### Factory Pattern Implementation

A factory retains the current service instance for its valid period, handing out the same instance until the time expires.

#### Lock-Free Version (with Lazy<T>)

```csharp
private class TimedDependencyFactory<T>
{
    private readonly TimeProvider _time;
    private readonly TimeSpan _lifetime;
    private readonly Func<T> _factory;
    private Tuple<Lazy<T>, DateTimeOffset>? _instance;

    public TimedDependencyFactory(TimeProvider time, TimeSpan lifetime, IServiceProvider serviceProvider)
    {
        _lifetime = lifetime;
        _factory = () => ActivatorUtilities.CreateInstance<T>(serviceProvider);
        _time = time;
    }

    public T GetInstance()
    {
        var instance = _instance;
        var now = _time.GetUtcNow();
        if (instance is not null && now < instance.Item2)
            return instance.Item1.Value;

        var newInstance = new Tuple<Lazy<T>, DateTimeOffset>(new Lazy<T>(_factory), now.Add(_lifetime));
        var previous = Interlocked.CompareExchange(ref _instance, newInstance, instance);
        if (ReferenceEquals(previous, instance))
            return newInstance.Item1.Value;
        return GetInstance();
    }
}
```

#### Lock-Based Version

For clarity and sometimes better practical performance:

```csharp
private class TimedDependencyFactory<T>
{
    private readonly TimeProvider _time;
    private readonly TimeSpan _lifetime;
    private readonly Func<T> _factory;
    private readonly object _lock = new();
    private Tuple<T, DateTimeOffset>? _instance;

    public TimedDependencyFactory(TimeProvider time, TimeSpan lifetime, IServiceProvider serviceProvider)
    {
        _lifetime = lifetime;
        _factory = () => ActivatorUtilities.CreateInstance<T>(serviceProvider);
        _time = time;
    }

    public T GetInstance()
    {
        var instance = _instance;
        var now = _time.GetUtcNow();
        if (instance is null || now > instance.Item2)
        {
            lock (_lock)
            {
                instance = _instance;
                if (instance is null || now > instance.Item2)
                {
                    instance = new Tuple<T, DateTimeOffset>(
                        _factory(), now.Add(_lifetime));
                    _instance = instance;
                }
            }
        }
        return instance.Item1;
    }
}
```

### Registering with DI

You can expose this new lifetime via an extension:

```csharp
public static class TimedScopeExtensions
{
    public static IServiceCollection AddTimed<T>(this IServiceCollection services, TimeSpan lifetime)
        where T : class
    {
        services.AddSingleton(provider => new TimedDependencyFactory<T>(
            TimeProvider.System, lifetime, provider));
        services.AddScoped(provider =>
            provider.GetRequiredService<TimedDependencyFactory<T>>().GetInstance());
        return services;
    }
}
```

### Example Usage

```csharp
builder.Services.AddTimed<TimedService>(TimeSpan.FromSeconds(5));

app.MapGet("/", (TimedService service) => service.GetValue);

public class TimedService
{
    private static int _id = 0;
    public int GetValue { get; } = Interlocked.Increment(ref _id);
}
```

Repeated requests within 5 seconds will get the same value; a new instance (with incremented ID) is provided after expiration.

### Limitations and Tradeoffs

- **IDisposable Issue:** The approach fails if `T` implements `IDisposable`, as the DI container will dispose it at the end of the request, even if a subsequent scope should reuse it.
- **Multiple Active Instances:** If a request is taking longer than the time window, you may have more than one instance active across requests at a time.
- **Potential Use Cases:** Edge cases like short-lived caching or resource throttling, but generally not something widely needed.

## Summary

This post first revisited .NET's built-in service lifetimes and then introduced three proposed DI scopes—tenant, pooled, and time-based. The post detailed how to implement a time-based (drifter) lifetime for experimental purposes and highlighted the limitations, especially with disposables and concurrent scopes. In follow-up content, a practical implementation for pooled lifetimes will be explored.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/going-beyond-singleton-scoped-and-transient-lifetimes/)
