---
external_url: https://code-maze.com/dotnet-hybrid-caching/
title: 'Hybrid Caching in ASP.NET Core: Bridging In-Memory and Distributed Caching'
author: Bozo Spoljaric
feed_name: Code Maze Blog
date: 2025-02-10 07:02:05 +00:00
tags:
- .NET
- ASP.NET Core
- Cache Invalidation
- Caching
- Caching Patterns
- Distributed Cache
- Distributed Caching
- Hybrid Caching
- in Memory Cache
- InMemory Caching
- Microsoft.Extensions.Caching.Hybrid
- Service Configuration
- Web Application Performance
- Coding
- Blogs
section_names:
- coding
primary_section: coding
---
Bozo Spoljaric explains hybrid caching in ASP.NET Core, presenting its implementation, configuration, and practical examples. The article targets developers optimizing data access and scalability in .NET applications by combining local and distributed cache strategies.<!--excerpt_end-->

# Hybrid Caching in ASP.NET Core

*By Bozo Spoljaric*

---

## Introduction

Hybrid caching in .NET is a newly supported approach that combines the capabilities of in-memory and distributed caching. This article explains the fundamental concepts of caching in .NET, introduces the HybridCache solution, and provides in-depth practical examples for its implementation and configuration in ASP.NET Core applications.

## What is Caching in .NET?

Caching temporarily stores frequently accessed data to improve application performance by reducing source retrieval overhead. In .NET, there are two principal forms:

- **In-memory caching**: Uses the server's local memory. It's very fast but bound to a single application instance.
- **Distributed caching**: Relies on external services (e.g., Redis, SQL Server), making cached data accessible across multiple servers—key in scalable scenarios.

## Hybrid Caching: A Unified Approach

**Hybrid caching bridges in-memory and distributed caching, creating a two-level architecture:**

- **L1**: Local, in-memory cache for rapid access
- **L2**: Distributed cache for persistence and multi-server synchronization

This dual-layer ensures the speed of in-memory caching while providing the durability and scalability of distributed caches.

## Adding and Configuring Hybrid Caching in ASP.NET Core

### Step 1: Install the NuGet Package

To add hybrid caching, install the following package:

```shell
dotnet add package Microsoft.Extensions.Caching.Hybrid
```

### Step 2: Register the HybridCache Service

In your application setup (typically `Program.cs`), register HybridCache:

```csharp
builder.Services.AddHybridCache();
```

#### Advanced Configuration Example

`AddHybridCache` allows additional configuration:

```csharp
builder.Services.AddHybridCache(options => {
    options.MaximumPayloadBytes = 10 * 1024 * 1024; // 10MB
    options.MaximumKeyLength = 256;
    options.ReportTagMetrics = true;
    options.DisableCompression = true;
    options.DefaultEntryOptions = new HybridCacheEntryOptions {
        Expiration = TimeSpan.FromMinutes(30),
        LocalCacheExpiration = TimeSpan.FromMinutes(30)
    };
});
```

**Option details:**

- `MaximumPayloadBytes`: Max item size in bytes (default: 1MB).
- `MaximumKeyLength`: Max length for keys (default: 1024 chars).
- `ReportTagMetrics`: Enables tag-based metric reporting.
- `DisableCompression`: Disables built-in compression.
- `DefaultEntryOptions`: Sets default expiration for both distributed and local caches.

By default, HybridCache detects configured distributed caches (e.g., Redis, SQL Server) and uses them as L2 storage. For serialization, it uses `System.Text.Json` but supports custom serializers via `AddSerializer<T>()` or `AddSerializerFactory()`.

## Using Hybrid Caching: Service and API Examples

Suppose you have a `CmCourse` class:

```csharp
public class CmCourse {
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Category { get; set; }
}
```

Define a cache-aware service interface:

```csharp
public interface ICmCourseService {
    Task<CmCourse?> GetCourseAsync(int id, CancellationToken cancellationToken = default);
    Task PostCourseAsync(CmCourse course, CancellationToken cancellationToken = default);
    Task InvalidateByCourseIdAsync(int id, CancellationToken cancellationToken = default);
    Task InvalidateByCategoryAsync(string tag, CancellationToken cancellationToken = default);
}
```

An example implementation, `CmCourseService`, injects `HybridCache`:

```csharp
public class CmCourseService(HybridCache cache) : ICmCourseService {
    public static readonly List<CmCourse> courseList = [
        new CmCourse { Id = 1, Name = "WebAPI", Category = "Backend" },
        new CmCourse { Id = 2, Name = "Microservices", Category = "Backend" },
        new CmCourse { Id = 3, Name = "Blazor", Category = "Frontend" },
    ];
    // ... method implementations ...
}
```

### Reading from the Cache

Use `GetOrCreateAsync()` to fetch or populate data:

```csharp
public async Task<CmCourse?> GetCourseAsync(int id, CancellationToken cancellationToken = default) {
    return await cache.GetOrCreateAsync($"course-{id}", async token => {
        await Task.Delay(1000, token); // Simulating data fetch
        var course = courseList.FirstOrDefault(course => course.Id == id);
        return course;
    },
    options: new HybridCacheEntryOptions {
        Expiration = TimeSpan.FromMinutes(30),
        LocalCacheExpiration = TimeSpan.FromMinutes(30)
    },
    tags: ["course"],
    cancellationToken: cancellationToken);
}
```

- If the entry is in the local cache, it is fetched directly.
- If not, the distributed cache is checked.
- If still missing, the factory method is executed, caches are populated, and the value is returned.

> *HybridCache handles concurrent access, preventing a cache stampede by letting only the first request repopulate the entry while others wait.*

### Writing to the Cache

To insert or update a cache entry directly, use `SetAsync()`:

```csharp
public async Task PostCourseAsync(CmCourse course, CancellationToken cancellationToken = default) {
    courseList.Add(course);
    await cache.SetAsync($"course-{course.Id}", course,
        options: new HybridCacheEntryOptions {
            Expiration = TimeSpan.FromMinutes(30),
            LocalCacheExpiration = TimeSpan.FromMinutes(30)
        },
        tags: [$"cat-{course.Category}"],
        cancellationToken: cancellationToken);
}
```

### Cache Invalidation

**By Key:**

```csharp
public async Task InvalidateByCourseIdAsync(int id, CancellationToken cancellationToken = default) {
    await cache.RemoveAsync($"course-{id}", cancellationToken);
}
```

**By Tag:**

```csharp
public async Task InvalidateByCategoryAsync(string tag, CancellationToken cancellationToken = default) {
    await cache.RemoveByTagAsync($"cat-{tag}", cancellationToken);
}
```

You can also pass collections of keys or tags for bulk invalidation.

## Benefits of Hybrid Caching

- **Unified API**: Code for both local and distributed cache with a consistent interface.
- **Simple migration**: You can replace `IMemoryCache` usage with `HybridCache` in most cases with minimal disruption.
- **Async and concurrency-aware**: Handles concurrent requests automatically, offering built-in cache stampede protection.
- **Flexible serialization**: Works out-of-the-box with strings and byte arrays, extensible to custom serializers (e.g., ProtoBuf).
- **Metrics and monitoring**: Built-in support for metrics helps with observability and fine-tuning.
- **Tags for invalidation**: One call can invalidate related entries by tag, simplifying cache management.

## Conclusion

HybridCache enriches .NET caching by merging the speed of in-memory storage with the robustness and scalability of distributed caching. Its design improves performance, supports scalable architectures, and minimizes costs through optimized data storage and cache management. Developers can now leverage this two-level caching strategy with minimal code changes, enhancing both reliability and efficiency in ASP.NET Core applications.

This post appeared first on "Code Maze Blog". [Read the entire article here](https://code-maze.com/dotnet-hybrid-caching/)
