---
layout: post
title: Scaling Azure Functions Python with orjson
author: eroman
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/scaling-azure-functions-python-with-orjson/ba-p/4445780
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-10-24 18:26:31 +00:00
permalink: /coding/community/Scaling-Azure-Functions-Python-with-orjson
tags:
- API
- Azure Functions
- Azure Load Testing
- Benchmarks
- Cloud Engineering
- Cosmos DB
- Deserialization
- Event Hubs
- Flex Consumption
- JSON Serialization
- Orjson
- Performance Optimization
- Python
- Requirements.txt
- Rust
- Serverless
- Service Bus
- Virtual Environment
section_names:
- azure
- coding
---
eroman examines how Azure Functions for Python apps benefit from native orjson support, showing measurable performance improvements in latency and throughput for serverless solutions.<!--excerpt_end-->

# Scaling Azure Functions Python with orjson

## Introduction

Delivering high-performance serverless applications is essential for Azure Functions, which powers workloads like APIs, event processing, and automation. A major contributor to overall performance is JSON serialization and deserialization for Python-based functions, whether they're interfacing via HTTP, Event Hubs, Service Bus, or Cosmos DB. Recognizing this, Microsoft has introduced support for [orjson](https://github.com/ijl/orjson)—a fast, Rust-based JSON library—in the Python worker for Azure Functions.

## What is orjson, and Why Does It Matter?

- **orjson** is a performance-oriented library for JSON serialization/deserialization, leveraging Rust for memory efficiency and speed.
- Azure Functions Python Library now auto-detects orjson: if it's found in your requirements.txt or virtual environment, Azure Functions switches to orjson, bypassing the slower standard Python json module. This improvement requires no code change in your apps.
- JSON serialization is critical for any serverless function processing messages, API payloads, or integrating event-driven Azure services.

## Why orjson Beats Traditional Libraries

- Written in Rust for optimized memory handling
- 2-4x faster than ujson, up to 10x faster than json for large or deeply nested payloads
- Generates more compact output—useful for bandwidth-sensitive applications
- Strict RFC 8259 compliance for interoperability

## Measured Performance Gains

### HTTP and Event Hub Benchmarks

**Locally:**

- HTTP: 10,000 requests (20KB payload)
  - json: 10:01 min total, avg request 3.08s, median 2.16s, dropped: 289
  - orjson: 6:01 min total, avg request 1.8s, median 1.51s, dropped: 0
- Event Hub: 1,000 messages (20KB)
  - json: 4:11 min, 3.98 messages/sec
  - orjson: 2:50 min, 5.88 messages/sec

**Cloud Results (Azure Functions Flex Consumption):**

- 100 instances, 2GB RAM, Azure Load Testing for throughput (`req/s`) and latency (ms)
- At 1KB to 992KB JSON payloads, orjson improved throughput by up to 6% and reduced average response time by up to 400ms
- For Service Bus, orjson showed +38% message rate at 1,000 messages, up to +15% at 10,000 messages

## Integration Challenges & Lessons

- Compatibility across supported Python versions (3.9–3.13) required robust testing
- Conditional loading means orjson is only used if present; otherwise, fallback to json
- Accurate benchmarks needed to account for cold starts, payload variance, and cloud transience

## Success Stories

- Up to 40% reduction in average request times with larger payloads
- Event Hub scenarios: up to 50% higher message throughput
- Drop-in improvement by merely adding orjson as a dependency, no code refactoring required

## Next Steps for Developers

- Review [Azure Functions Python Guide](https://learn.microsoft.com/en-us/azure/azure-functions/functions-reference-python?tabs=get-started%2Casgi%2Capplication-level&pivots=python-mode-decorators) for best practices
- Consider adding orjson to requirements.txt for immediate performance upgrades
- Monitor upcoming enhancements for deeper worker-level optimizations

## Conclusion

Native orjson support in Azure Functions for Python apps offers developers fast, easy performance benefits for JSON-heavy serverless workloads. Results are both significant and practical for production scenarios, especially for applications with high message volume or large payloads.

## Further Reading

- [Azure Functions Overview](https://learn.microsoft.com/en-us/azure/azure-functions/functions-overview)
- [Azure Functions Python Developer Reference Guide](https://learn.microsoft.com/en-us/azure/azure-functions/functions-reference-python?tabs=get-started%2Casgi%2Capplication-level&pivots=python-mode-decorators)
- [Azure Functions Performance Optimizer](https://learn.microsoft.com/en-us/azure/app-testing/load-testing/how-to-optimize-azure-functions)
- [Azure Functions Python Worker](https://github.com/Azure/azure-functions-python-worker)
- [Azure Functions Python Library](https://github.com/Azure/azure-functions-python-library)
- [Azure Load Testing Overview](https://learn.microsoft.com/en-us/azure/app-testing/load-testing/overview-what-is-azure-load-testing)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/scaling-azure-functions-python-with-orjson/ba-p/4445780)
