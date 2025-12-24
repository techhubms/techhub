---
layout: "post"
title: "Comparing Raw ASP.NET Request Throughput: .NET 8.0 vs .NET 9.0"
description: "Rick Strahl benchmarks and analyzes ASP.NET request throughput and resource usage differences between .NET 8.0 and .NET 9.0, focusing on real-world developer scenarios, performance trade-offs, and impacts of garbage collection configuration. Real test data, anecdotes, and code samples provide actionable insights."
author: "Rick Strahl"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://weblog.west-wind.com/posts/2025/Jan/19/Comparing-Raw-ASPNET-Request-Throughput-across-Versions-80-to-90-Edition"
viewing_mode: "external"
feed_name: "Rick Strahl's Blog"
feed_url: "https://feeds.feedburner.com/rickstrahl"
date: 2025-01-19 22:51:02 +00:00
permalink: "/posts/2025-01-19-Comparing-Raw-ASPNET-Request-Throughput-NET-80-vs-NET-90.html"
categories: ["Coding"]
tags: [".NET", ".NET 8.0", ".NET 9.0", ".NET ASP.NET", "ASP.NET", "Coding", "Controllers", "Garbage Collection", "Kestrel", "Load Testing", "Memory Footprint", "Minimal APIs", "Performance", "Posts", "Resource Usage", "Startup Speed", "WebSurge"]
tags_normalized: ["dotnet", "dotnet 8dot0", "dotnet 9dot0", "dotnet aspdotnet", "aspdotnet", "coding", "controllers", "garbage collection", "kestrel", "load testing", "memory footprint", "minimal apis", "performance", "posts", "resource usage", "startup speed", "websurge"]
---

In this detailed article, Rick Strahl compares the raw throughput and resource usage of ASP.NET applications running on .NET 8.0 and .NET 9.0. He shares benchmarks, test setup details, anecdotes from production, and practical performance findings.<!--excerpt_end-->

# Comparing Raw ASP.NET Request Throughput: .NET 8.0 vs .NET 9.0

**Author:** Rick Strahl  
**Published:** January 19, 2025, from Maui, Hawaii  

---

## Introduction

Whenever a new .NET release comes out, I run comparison tests using simple load testing tools to see how the new version stacks up against previous releases. In this post, I focus on .NET 9.0 compared to .NET 8.0. I share not only benchmark numbers, but also anecdotes from running .NET 9.0 in production apps over the past couple months.

## Test Approach

For these tests, I use extremely simple ASP.NET JSON endpoints—both Minimal APIs and ASP.NET Controllers—and hit them with heavy local web load tests. The goal isn't to conduct scientific benchmarks, but to get a practical sense of performance changes across versions on the same hardware.

Benefits of this approach:

- See throughput for high-load ASP.NET servers and .NET clients.
- Compare .NET versions year-over-year on the same machine.

The code and test setup are available [on GitHub](https://github.com/RickStrahl/HighPerformanceAspNet). The main endpoint simply responds with a small JSON payload for both GET and POST requests. For load testing, I use my own [West Wind WebSurge](https://websurge.west-wind.com/), which simplifies setup. Example request and session files are included.

For these runs, everything is local on a high-end laptop (I9, 13th gen, 32 GB RAM, fast NVMe drive). The focus: compare raw throughput under best-case conditions.

## .NET 9.0: First Impressions

Subjectively, .NET 9.0 feels snappier than 8.0:

- Startup speed is noticeably improved for web and desktop apps.
- Overall responsiveness is a bit better.
- Memory usage is significantly reduced.

However, the actual numbers show a mixed bag: .NET 9.0 is roughly on par with 8.0 in throughput, sometimes slightly slower, but with much lower memory consumption.

## Test Setup Details

**Hardware:**

- MSI GE68HX laptop
- Intel I9, 13th gen
- 32 GB RAM, 7,000 MB/sec NVMe drive

**Environment Prep:**

- Windows Defender disabled (to remove interference)
- Docker not running (for network consistency)
- Use plain HTTP (`http://`) instead of HTTPS for maximum throughput (~10-15% performance hit avoided)
- Fresh app startups, warm-up runs before measuring

.NET web apps are run using published release builds and Kestrel directly:

```powershell
 dotnet publish HighPerformanceAspNet.csproj -o ../Publish -c Release
 ../publish/HighPerformanceAspNet --urls http://dev.west-wind.com:5200
```

Note: Using HTTP allows up to 13% more requests/second compared to HTTPS in local tests.

## Test Scenarios & Code

### Minimal API Example

```csharp
app.MapGet("/hello", () => new { name = "Rick", message = "Hello World" });
app.MapPost("/hello", (RequestMessage model) => new { name = model.Name, message = model.Message });
```

### Controller Example

```csharp
[ApiController]
[Route("[controller]")]
public class ClassicController : ControllerBase {
    [HttpGet("hello")]
    public object Hello() => new { name = "Rick", message = "Hello World" };

    [HttpPost("hello")]
    public object Hello(RequestMessage model) => new { name = model.Name, message = model.Message };
}
```

These methods implement the most basic logic to focus purely on routing, serialization, and response throughput—not on realistic business logic or data access.

## Benchmark Results

**All numbers represent requests per second (RPS).**

### WebSurge UI Results

| Framework      | .NET 9.0 | .NET 8.0 | .NET 7.0 |
|---------------|----------|----------|----------|
| Minimal APIs  | 164,250  | 168,250  | 160,250  |
| Controllers   | 151,750  | 160,750  | 150,500  |

### WebSurge CLI Results

| Framework      | .NET 9.0 | .NET 8.0 | .NET 7.0 |
|---------------|----------|----------|----------|
| Minimal APIs  | 180,000  | 189,000  | 161,200  |
| Controllers   | 173,500  | 183,250  | 150,500  |

**Observation:** .NET 9.0 is slightly slower than .NET 8.0 in these micro-benchmark scenarios, despite subjective feelings. However, memory usage is markedly lower in .NET 9.0.

### Resource Usage: .NET 9.0 vs .NET 8.0

Memory consumption for .NET 9.0 is significantly reduced—more than 10x less in some test runs. In real internal apps, the reduction is 2x-3x, which is still substantial.

You can check process memory via PowerShell:

```powershell
get-process -Name "HighPerformanceAspNet" | Select-Object Name, Id, @{Name="WorkingSet (MB)";Expression={---
layout: "post"
title: "Comparing Raw ASP.NET Request Throughput: .NET 8.0 vs .NET 9.0"
description: "Rick Strahl benchmarks and analyzes ASP.NET request throughput and resource usage differences between .NET 8.0 and .NET 9.0, focusing on real-world developer scenarios, performance trade-offs, and impacts of garbage collection configuration. Real test data, anecdotes, and code samples provide actionable insights."
author: "Rick Strahl"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://weblog.west-wind.com/posts/2025/Jan/19/Comparing-Raw-ASPNET-Request-Throughput-across-Versions-80-to-90-Edition"
viewing_mode: "external"
feed_name: "Rick Strahl's Blog"
feed_url: https://feeds.feedburner.com/rickstrahl
date: 2025-01-19 22:51:02 +00:00
permalink: "2025-01-19-Comparing-Raw-ASPNET-Request-Throughput-NET-80-vs-NET-90.html"
categories: ["Coding"]
tags: [".NET", ".NET 8.0", ".NET 9.0", ".NET ASP.NET", "ASP.NET", "Coding", "Controllers", "Garbage Collection", "Kestrel", "Load Testing", "Memory Footprint", "Minimal APIs", "Performance", "Posts", "Resource Usage", "Startup Speed", "WebSurge"]
tags_normalized: [["net", "net 8 dot 0", "net 9 dot 0", "net asp dot net", "asp dot net", "coding", "controllers", "garbage collection", "kestrel", "load testing", "memory footprint", "minimal apis", "performance", "posts", "resource usage", "startup speed", "websurge"]]
---

In this detailed article, Rick Strahl compares the raw throughput and resource usage of ASP.NET applications running on .NET 8.0 and .NET 9.0. He shares benchmarks, test setup details, anecdotes from production, and practical performance findings.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2025/Jan/19/Comparing-Raw-ASPNET-Request-Throughput-across-Versions-80-to-90-Edition)
.WorkingSet / 1MB}}, @{Name="VirtualMemorySize (MB)";Expression={---
layout: "post"
title: "Comparing Raw ASP.NET Request Throughput: .NET 8.0 vs .NET 9.0"
description: "Rick Strahl benchmarks and analyzes ASP.NET request throughput and resource usage differences between .NET 8.0 and .NET 9.0, focusing on real-world developer scenarios, performance trade-offs, and impacts of garbage collection configuration. Real test data, anecdotes, and code samples provide actionable insights."
author: "Rick Strahl"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://weblog.west-wind.com/posts/2025/Jan/19/Comparing-Raw-ASPNET-Request-Throughput-across-Versions-80-to-90-Edition"
viewing_mode: "external"
feed_name: "Rick Strahl's Blog"
feed_url: https://feeds.feedburner.com/rickstrahl
date: 2025-01-19 22:51:02 +00:00
permalink: "2025-01-19-Comparing-Raw-ASPNET-Request-Throughput-NET-80-vs-NET-90.html"
categories: ["Coding"]
tags: [".NET", ".NET 8.0", ".NET 9.0", ".NET ASP.NET", "ASP.NET", "Coding", "Controllers", "Garbage Collection", "Kestrel", "Load Testing", "Memory Footprint", "Minimal APIs", "Performance", "Posts", "Resource Usage", "Startup Speed", "WebSurge"]
tags_normalized: [["net", "net 8 dot 0", "net 9 dot 0", "net asp dot net", "asp dot net", "coding", "controllers", "garbage collection", "kestrel", "load testing", "memory footprint", "minimal apis", "performance", "posts", "resource usage", "startup speed", "websurge"]]
---

In this detailed article, Rick Strahl compares the raw throughput and resource usage of ASP.NET applications running on .NET 8.0 and .NET 9.0. He shares benchmarks, test setup details, anecdotes from production, and practical performance findings.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2025/Jan/19/Comparing-Raw-ASPNET-Request-Throughput-across-Versions-80-to-90-Edition)
.VirtualMemorySize / 1MB}}
```

### Garbage Collection Adaptation Mode

.NET 9.0 introduced a default resource-saving garbage collection mode (`<GarbageCollectionAdaptationMode>1</GarbageCollectionAdaptationMode>`), trading off slight performance for lower memory. Setting this flag to `0` (`<GarbageCollectionAdaptationMode>0</GarbageCollectionAdaptationMode>`) returns to the greedier GC approach, boosting throughput slightly at the cost of higher memory use.

Switching this setting:

- Raises throughput by ~2.5%, matching .NET 8.0 levels.
- Memory usage increases, showing a clear trade-off that developers can tune dynamically.

## UI vs. CLI Clients

Interestingly, the CLI (command-line interface) version of WebSurge is about 10% more efficient than the desktop UI version. Even minimal UI updates add measurable overhead.

Similarly, running WebSurge on .NET 9.0 boosts client performance by about 8% over .NET 8.0. The improvement seems more pronounced on desktop than server, possibly due to framework improvements in UI, text encoding, and HTTP libraries.

## Network vs. Local Tests

All above results are from same-machine tests, which yield highest throughput due to near-zero networking overhead. Even on a 10Gbps network, distributed client/server tests can't match local results—network stack or Windows client throttling may cap maximum throughput.

In real-world, when database access or complex logic is added, throughput drops by over 10x due to contention and resource limits.

## Summary & Takeaways

- **Performance:** .NET 9.0 server throughput is nearly on par with 8.0 but not notably faster. .NET 9.0 is a bit slower out of the box (with default resource-saving GC), but configuration can mitigate the difference.
- **Resource Usage:** Major reduction in memory footprint in .NET 9.0 (2-10x, scenario dependent). Benefits both server and especially desktop/CLI clients like WebSurge.
- **Developer Experience:** Updates are largely seamless; most apps require no code changes.
- **Practical Advice:** Test your own apps, especially with different garbage collection modes, to find your optimal balance between performance and resource savings.
- **General Outlook:** .NET has reached a point of high, stable performance. Future gains may be incremental, but reduction in resource usage is a very real benefit, especially at scale.

**Resources:**

- [HighPerformanceAspNet Test Project (GitHub)](https://github.com/RickStrahl/HighPerformanceAspNet)
- [West Wind WebSurge](https://websurge.west-wind.com)
- [Comparing ASP.NET Throughput: .NET 7.0 vs. 8.0](https://weblog.west-wind.com/posts/2024/Mar/08/Comparing-Raw-ASPNET-Request-Throughput-across-Versions)

---

**If you found this analysis useful, consider making a small donation to support continued content like this.**

This post appeared first on "Rick Strahl's Blog". [Read the entire article here](https://weblog.west-wind.com/posts/2025/Jan/19/Comparing-Raw-ASPNET-Request-Throughput-across-Versions-80-to-90-Edition)
