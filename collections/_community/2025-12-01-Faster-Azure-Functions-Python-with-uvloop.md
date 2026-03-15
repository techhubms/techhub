---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/faster-azure-functions-python-with-uvloop/ba-p/4455323
title: Faster Azure Functions Python with uvloop
author: eroman
feed_name: Microsoft Tech Community
date: 2025-12-01 19:58:50 +00:00
tags:
- Asynchronous Workloads
- AsyncIO
- Azure App Service
- Azure Functions
- Benchmarking
- Cloud Scalability
- Concurrency
- Event Loop
- Flex Consumption
- Libuv
- Performance Optimization
- Python 3.13
- Python Worker
- Serverless
- Uvloop
- Azure
- Community
- .NET
section_names:
- azure
- dotnet
primary_section: dotnet
---
eroman outlines how switching to uvloop boosts the performance of Azure Functions with Python 3.13+. Serverless apps now run asynchronous workloads faster and scale more efficiently – no code changes needed.<!--excerpt_end-->

# Faster Azure Functions Python with uvloop

With the rollout of Python 3.13 support in Azure Functions, asynchronous workloads on the platform now benefit from uvloop as the default event loop. This update delivers higher throughput, lower latency, and better scalability for serverless Python applications—without requiring any code changes from developers.

## Introduction

Azure Functions backs a wide array of solutions, such as real-time APIs and event-driven automation. For Python developers, efficient I/O, concurrency, and async task handling are crucial for scalability. The Python worker for Azure Functions now sets uvloop as its default event loop on Python 3.13+, replacing the standard UnixSelectorEventLoop with a faster alternative built on libuv (the library behind Node.js).

## Why Event Loops Matter in Serverless Python

The event loop is at the heart of every asynchronous Python app. It:

- Schedules coroutines and completes async tasks
- Handles incoming HTTP requests
- Manages parallel event processing (Event Hubs, Service Bus)

While the UnixSelectorEventLoop is stable, it isn't tuned for high-throughput scenarios. uvloop—implemented in Cython—boosts both throughput and latency, making Azure Functions more responsive, especially for concurrent workloads.

## How uvloop Works in Azure Functions

On Python 3.13 and higher in Azure Functions, the Python worker sets uvloop policy at startup:

```python
import uvloop, asyncio
asyncio.set_event_loop_policy(uvloop.EventLoopPolicy())
```

All async workloads (async def functions, async API calls, asyncio.gather) benefit from increased performance immediately. No runtime configuration, requirements.txt updates, or feature flags are required.

## Benchmark Results

Performance testing compared uvloop (Python 3.13) with Unix event loop (Python 3.12) in several scenarios:

### Test 1: 10k Requests, 50 Users

- **Azure, unix:** 54.34 ms/request, 882 req/sec
- **Azure, uvloop:** 51.77 ms/request, 923 req/sec (**+4.8% throughput**)

### Test 2: Sustained Load, 100 Users

- **Azure, unix:** 1,898 req/sec
- **Azure, uvloop:** 1,961 req/sec (**+2.9% throughput**)

### Test 3: Heavy Concurrency, 500 Users + async tasks

- **Azure, unix:** 5,696 req/sec
- **Azure, uvloop:** 6,020 req/sec (**+1% throughput**)

Under load, uvloop consistently raised throughput and reduced latency. Under concurrency, the standard loop started showing request failures, while uvloop proved more resilient.

## Why This Ships with Python 3.13+

uvloop could work on earlier Python versions, but was adopted for 3.13+ to:

- Guarantee positive impact on performance and stability
- Enable smooth rollout for all Azure Functions SKUs
- Offset Python 3.13 worker's additional Proxy Worker overhead

Earlier runtimes stay on the classic loop to avoid breaking compatibility.

## Lessons Learned

- Ensured cross-platform Linux stability for uvloop
- Enhanced observability (logging which loop is active)
- Benchmarked real-world, not just microbenchmark, workloads

## Roadmap

Future enhancements may target:

- Deeper async and gRPC optimization
- Faster data serialization (e.g., with orjson)
- Further cold start reduction for Python workers

## Conclusion

Azure Functions on Python 3.13+ benefit from uvloop's event loop performance gains right out of the box—no user code or configuration changes required. If you're running or upgrading to Python 3.13 on Azure Functions, uvloop enables faster, more scalable serverless apps immediately.

## Further Reading

- [Azure Functions](https://learn.microsoft.com/en-us/azure/azure-functions/functions-overview)
- [Azure Functions Python Developer Reference Guide](https://learn.microsoft.com/en-us/azure/azure-functions/functions-reference-python?tabs=get-started%2Casgi%2Capplication-level&pivots=python-mode-decorators)
- [Azure Functions Performance Optimizer](https://learn.microsoft.com/en-us/azure/app-testing/load-testing/how-to-optimize-azure-functions)
- [Azure Functions Python Worker](https://github.com/Azure/azure-functions-python-worker)
- [Azure Functions Python Library](https://github.com/Azure/azure-functions-python-library)
- [Azure Load Testing Overview](https://learn.microsoft.com/en-us/azure/app-testing/load-testing/overview-what-is-azure-load-testing)

---

*Author: eroman*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/faster-azure-functions-python-with-uvloop/ba-p/4455323)
