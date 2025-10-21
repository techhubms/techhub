---
layout: "post"
title: "Postgres as a Distributed Cache Unlocks Speed and Simplicity for Modern .NET Workloads"
description: "This article explores how .NET developers can leverage the Microsoft.Extensions.Caching.Postgres library to use Postgres databases as distributed caches. It explains performance trade-offs, configuration approaches, and integration with HybridCache, offering a practical perspective for building fast, scalable, and reliable caching solutions within modern .NET workloads."
author: "JaredMeade"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/microsoft-developer-community/postgres-as-a-distributed-cache-unlocks-speed-and-simplicity-for/ba-p/4462139"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-21 07:00:00 +00:00
permalink: "/2025-10-21-Postgres-as-a-Distributed-Cache-Unlocks-Speed-and-Simplicity-for-Modern-NET-Workloads.html"
categories: ["Coding"]
tags: [".NET", "ASP.NET Core", "Cache Expiration", "Caching", "Coding", "Community", "Database", "Distributed Cache", "HybridCache", "Microsoft.Extensions.Caching.Postgres", "NuGet Packages", "Performance Optimization", "Postgres", "Redis", "SQL Server", "UNLOGGED Tables", "Write Ahead Logging"]
tags_normalized: ["dotnet", "aspdotnet core", "cache expiration", "caching", "coding", "community", "database", "distributed cache", "hybridcache", "microsoftdotextensionsdotcachingdotpostgres", "nuget packages", "performance optimization", "postgres", "redis", "sql server", "unlogged tables", "write ahead logging"]
---

JaredMeade demonstrates how .NET developers can use Microsoft.Extensions.Caching.Postgres to implement distributed caching with Postgres, highlighting performance, configuration flexibility, and seamless integration.<!--excerpt_end-->

# Postgres as a Distributed Cache Unlocks Speed and Simplicity for Modern .NET Workloads

## Overview

Modern .NET workloads often need caching to achieve low-latency response times without introducing unnecessary infrastructure complexity. JaredMeade explores how the Microsoft.Extensions.Caching.Postgres NuGet package enables distributed caching using an existing Postgres database, offering a viable alternative to specialized external caching services like Redis.

## Key Concepts and Motivation

- **Why Cache in Postgres?**
  - Leverage familiar, existing Postgres infrastructure for distributed caching.
  - Avoid introducing new cache platforms or managing separate cache servers.
  - Benefit from Postgres' reputation for reliability, extensibility, and support for multiple workloads.
- **.NET Ecosystem Integration**
  - The Microsoft.Extensions.Caching.Postgres library integrates with ASP.NET Core’s caching patterns (including HybridCache).
  - Compatible with existing distributed caching interfaces for drop-in usage.
- **Performance Trade-offs**
  - Memory stores (e.g., Redis) generally offer best latency, but Postgres can closely match performance for most .NET application scenarios via unlogged tables.
  - Developers can selectively trade cache-data durability for speed by leveraging Postgres' UNLOGGED tables (bypassing Write Ahead Log for faster writes at the expense of crash consistency on those tables).

## Technical Highlights

### History and Background

- Database-backed cache stores are common in .NET using libraries like Microsoft.Extensions.Caching.SqlServer.
- This new Postgres-based library builds on that pattern, suited for environments where dedicated cache engines aren't available or practical.

### How the Postgres Cache Works

- Implements the IDistributedCache interface in .NET, enabling drop-in usage in existing apps.
- Allows granular table configuration: choose between logged (ACID-compliant) and unlogged (performance-optimized) modes for caching tables.
- Fully compatible with HybridCache for tiered in-memory and distributed caching.

### Benchmarks and Comparisons

- Benchmarks across varied payload sizes (128, 1024, 10240 bytes) and operations (synchronous, async, single/concurrent, absolute and sliding expirations) show that Postgres can approach Redis-level performance for more intensive cache operations.

### Adoption and Community

- The package is open source, with a GitHub repository and example applications for both Console apps and web APIs (including Aspire samples).
- Developers are encouraged to try, contribute feedback, and explore advanced configuration (e.g., WAL bypass toggling).

## How-To: Integrating HybridCache with Postgres

1. Add the Microsoft.Extensions.Caching.Postgres package from NuGet to your .NET solution.
2. Configure your cache tables in Postgres, optionally using the UNLOGGED setting for performance-critical, non-durable cache data.
3. Integrate via ASP.NET Core’s distributed caching configuration, pairing in-memory HybridCache with the Postgres backplane for failover resilience.
4. Consult the package's GitHub examples for ready-to-run setups in both console and web API projects.

## Trade-offs and Limitations

- Using UNLOGGED tables means that some cache data may be lost in a crash scenario—it’s a conscious performance vs reliability decision for non-critical cached data.
- Memory-based stores like Redis will still outperform for pure raw speed, but Postgres delivers competitive results with the added benefit of simplicity and platform consolidation.

## Final Thoughts

JaredMeade's article highlights how .NET engineers can exploit their existing database stack for performance-critical distributed caching, using community-driven tooling and well-worn architectural patterns. By enabling advanced configuration and seamless ASP.NET Core integration, Microsoft.Extensions.Caching.Postgres offers an approachable path to reduce architectural sprawl while maintaining agility and speed.

For more details, benchmarks, and examples, see the official package and repositories:

- [Microsoft.Extensions.Caching.Postgres on NuGet](https://www.nuget.org/packages/Microsoft.Extensions.Caching.Postgres)
- [HybridCache Documentation (ASP.NET Core)](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/hybrid?view=aspnetcore-9.0)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/postgres-as-a-distributed-cache-unlocks-speed-and-simplicity-for/ba-p/4462139)
