---
layout: "post"
title: "Boosting Python Performance on Azure Functions with uvloop"
description: "This community post by eroman explores how upgrading to Python 3.13 on Azure Functions now leverages uvloop as the default event loop, delivering increased throughput and reduced latency for asynchronous workloads. The post examines implementation details, benchmark results, compatibility considerations, and practical impact for serverless Python developers on Azure."
author: "eroman"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/faster-python-on-azure-functions-with-uvloop/ba-p/4455323"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-12-01 21:11:10 +00:00
permalink: "/community/2025-12-01-Boosting-Python-Performance-on-Azure-Functions-with-uvloop.html"
categories: ["Azure", "Coding"]
tags: ["Asynchronous Workloads", "Asyncio", "Azure", "Azure Functions", "Coding", "Community", "Concurrency", "Event Loop", "Flex Consumption", "Latency", "Libuv", "Performance Optimization", "Python 3.13", "Python Worker", "Serverless", "Serverless Architecture", "Throughput", "Uvloop"]
tags_normalized: ["asynchronous workloads", "asyncio", "azure", "azure functions", "coding", "community", "concurrency", "event loop", "flex consumption", "latency", "libuv", "performance optimization", "python 3dot13", "python worker", "serverless", "serverless architecture", "throughput", "uvloop"]
---

eroman details how switching to uvloop as the default event loop in Azure Functions for Python 3.13+ delivers faster async performance with no code changes needed, supported by benchmark data and practical integration notes.<!--excerpt_end-->

# Boosting Python Performance on Azure Functions with uvloop

With Python 3.13+, Azure Functions makes significant strides in performance by adopting uvloop as the default event loop. This change delivers faster, more scalable async workloads—crucial for real-time APIs, event-driven automation, and high-concurrency serverless scenarios.

## Introduction

Azure Functions is central to countless modern applications, especially for Python developers who rely on its I/O and async capabilities for scalability. The new default event loop, uvloop, replaces Python's standard asyncio loop in Functions on Python 3.13+, improving request throughput and lowering latency without any configuration or code changes.

## Why Are Event Loops Important?

In asynchronous Python apps, the event loop schedules coroutines and manages I/O events. For serverless workloads like Azure Functions, this loop handles:

- Incoming HTTP requests
- Async tasks (e.g., database queries, external service calls)
- Parallel event processing (e.g., Event Hubs, Service Bus)

While the traditional UnixSelectorEventLoop is reliable, it's not optimized for high-throughput demands. uvloop, built atop libuv in Cython, consistently outperforms it, making it an efficient choice for cloud environments.

## Implementation: uvloop in Azure Functions

Starting with Python 3.13, Azure Functions Python worker sets the default as follows:

```python
import uvloop
import asyncio
asyncio.set_event_loop_policy(uvloop.EventLoopPolicy())
```

All asynchronous workloads—whether using `async def`, external API calls, or parallel async operations—immediately benefit from uvloop's enhanced scheduling and I/O. No special configuration or requirements.txt edits are needed; uvloop is pre-installed in the Functions runtime for Python 3.13+.

## Benchmarking Performance: Real-World Results

Three types of load tests were run using Azure's Flex Consumption plan (2048 MB instance), comparing Python 3.12 (Unix event loop) and Python 3.13 (uvloop):

**Test 1: 10k Requests, 50 Virtual Users**

- Local: uvloop saw +9.7% requests/sec over UnixSelectorEventLoop
- Azure: uvloop achieved +4.8% improvement

**Test 2: Sustained Load, 100 Virtual Users (5 min)**

- Local: uvloop gave +6.4% more requests/sec
- Azure: uvloop delivered +2.9% improvement

**Test 3: 500 Virtual Users, 5 Async Tasks per Request**

- Local: uvloop performed +7% better
- Azure: uvloop resulted in +1% improvement

Notably, the Unix event loop encountered failure rates of around 2% at high loads, which uvloop mitigated.

## Why Only Python 3.13+?

Although uvloop supports earlier Python versions, it's the default only in 3.13 for:

- Improved performance and stability
- Simpler, risk-free rollout across all Azure Functions SKUs
- Coordination with new features in Python 3.13, including a Proxy Worker for added performance

Older workers use the standard event loop to avoid compatibility issues.

## Challenges & Lessons Learned

- **Compatibility:** Ensured uvloop runs reliably across Azure's Linux environments
- **Observability:** Log updates confirm which event loop policy is active
- **Benchmarks:** Designed to reflect real-world workloads, testing more than just microbenchmarks

## Looking Ahead

Azure Functions will continue to optimize Python performance with:

- Deeper asyncio and gRPC optimizations
- Serialization enhancements (e.g., via orjson)
- Faster cold starts for Python workers

## Conclusion

Upgrading to Python 3.13+ on Azure Functions brings instantaneous, configuration-free performance improvements thanks to uvloop, with no changes needed to your existing code.

## Further Reading

- [Azure Functions Overview](https://learn.microsoft.com/en-us/azure/azure-functions/functions-overview)
- [Azure Functions Python Developer Guide](https://learn.microsoft.com/en-us/azure/azure-functions/functions-reference-python?tabs=get-started%2Casgi%2Capplication-level&pivots=python-mode-decorators)
- [Performance Optimizer for Azure Functions](https://learn.microsoft.com/en-us/azure/app-testing/load-testing/how-to-optimize-azure-functions)
- [Azure Functions Python Worker (GitHub)](https://github.com/Azure/azure-functions-python-worker)
- [Azure Functions Python Library (GitHub)](https://github.com/Azure/azure-functions-python-library)
- [Azure Load Testing Overview](https://learn.microsoft.com/en-us/azure/app-testing/load-testing/overview-what-is-azure-load-testing)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/faster-python-on-azure-functions-with-uvloop/ba-p/4455323)
