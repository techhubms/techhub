---
layout: post
title: 'Preparing for the .NET 10 GC: Understanding and Tuning DATAS'
author: maoni
canonical_url: https://devblogs.microsoft.com/dotnet/preparing-for-dotnet-10-gc/
viewing_mode: external
feed_name: Microsoft .NET Blog
feed_url: https://devblogs.microsoft.com/dotnet/feed/
date: 2025-10-08 17:00:00 +00:00
permalink: /coding/news/Preparing-for-the-NET-10-GC-Understanding-and-Tuning-DATAS
tags:
- .NET
- .NET 10
- Application Optimization
- C#
- Configuration Settings
- Containerization
- DATAS
- Garbage Collection
- GC
- GCDTargetTCP
- GCDynAdaptationMode
- GCHeapCount
- Kubernetes
- Memory
- Memory Management
- Performance
- Runtime Configuration
- Server GC
- Throughput
- Tuning
section_names:
- coding
---
Maoni presents a comprehensive guide for developers preparing for the .NET 10 release, focusing on how the new DATAS feature in Garbage Collection changes memory and performance profiles, with actionable advice for evaluating, tuning, or disabling this functionality.<!--excerpt_end-->

# Preparing for the .NET 10 GC (DATAS)

**Author: Maoni**

> *Originally published on the .NET Blog. [Read the original post.](https://devblogs.microsoft.com/dotnet/preparing-for-dotnet-10-gc/)*

## Introduction

With the arrival of .NET 10, DATAS (Dynamic Adaptation To Application Sizes) significantly changes the default behavior for Server Garbage Collection (GC). Many developers will encounter DATAS for the first time in .NET 10, as the previous default in .NET 9 did not affect long-term support (LTS) users. This guide helps you understand what to expect, when to tune DATAS, when to disable it, and how to make informed decisions about your application's memory and throughput goals.

---

## Key Terms (Glossary)

- **GC**: Garbage Collector manages memory allocation and release
- **DATAS**: Dynamic Adaptation To Application Sizes
- **TCP**: Throughput Cost Percentage (GC overhead incl. pauses and allocation waits)
- **BCD**: Budget Computed via DATAS (upper bound for gen0 allocation budget)
- **LDS**: Live Data Size (minimum application memory usage)
- **UOH**: User Old Heap (includes LOH and POH)
- **LOH**: Large Object Heap (objects ≥85,000 bytes)
- **POH**: Pinned Object Heap (for pinned allocations)

---

## Why DATAS? When Should You Care?

- **DATAS** dynamically adapts heap size to match the application's workload, allowing memory usage to shrink when demand decreases.
- **Who benefits:**
  - Apps with bursty workloads or running in memory-constrained environments (e.g., containers, Kubernetes)
  - Small-scale apps using Server GC
- **Who may not benefit:**
  - Apps where throughput must never regress
  - Scenarios with negligible benefit from reduced memory usage
  - Scenarios where startup performance is critical
  - Workloads dominated by gen2 GCs

---

## Changes in Behavior: DATAS vs. Server GC

- **Traditional Server GC** does not adapt heap size; behavior is mostly driven by generation survival rates and number of cores (affecting number of heaps).
- **DATAS** targets a consistent heap size regardless of available cores by basing the heap budget on LDS, bringing more predictability especially for variable hardware and workloads.
- **Key insight:** "Max heap size" is no longer directly tied to number of cores but adapts based on live data and performance tuning metrics (TCP).

---

## Evaluating DATAS for Your Application

- **If you use free memory for other tasks or need memory to scale elastically,** DATAS can be beneficial (especially with orchestrated environments like Kubernetes).
- **If maximizing throughput or startup performance is your primary concern,** you may want to tune DATAS or leave it disabled.

---

## Metrics and Tuning

- **BCD (Budget Computed via DATAS):** Determines upper bound for gen0 allocations, calculated as a function of LDS.
- **TCP (Throughput Cost Percentage):** Approximated by % pause time in GC, with the default target being 2%. This can be tuned using the `GCDTargetTCP` config.
- **Adjusting budgets and multipliers:** You can fine-tune heap growth and allocation policies with configs like `GCDGen0GrowthPercent`, `GCDGen0GrowthMinFactor`, and more.

### Example Tuning Scenario

- Comparing behavior with and without DATAS, you may see more frequent GCs (smaller budget) and higher % pause time, but also lower working set (memory usage).
- Adjusting the growth percent and min/max multipliers can bring heap budgets closer to pre-DATAS (larger allocations before each GC) and reduce the frequency of GCs.

---

## Practical Guidance: Should You Enable, Tune, or Disable DATAS?

**Enable DATAS if:**

- You need dynamic memory management due to bursty workloads or in containers
- You want predictable memory usage across environments/cores

**Tune DATAS if:**

- Out-of-the-box % pause time increases and negatively impacts throughput
- Executed GC frequency becomes problematic
- You want to match previous gen0 allocation budgets

**Disable DATAS if:**

- You do not benefit from memory reduction (e.g., have dedicated resources)
- Startup or throughput regressions are unacceptable
- Your workload mainly performs gen2 GCs

---

## Configurations and Diagnostic Tips

- **Disable/enable DATAS:** Use `GCDynamicAdaptationMode`
- **Tune throughput target:** Use `GCDTargetTCP`
- **Adjust gen0 allocation policy:** Use `GCDGen0GrowthPercent` and `GCDGen0GrowthMinFactor`
- To analyze LDS and TCP metrics, leverage .NET diagnostic tracing and the TraceEvent library

---

## Real-World Examples

- *Customer Case 1:* Server app in containerization transition observed a 10% memory reduction but a 6.8% throughput regression; tuning configs achieved acceptable tradeoffs.
- *Customer Case 2:* ASP.NET app benefited from removing heap count restriction, leveraging DATAS for flexible throughput improvement and memory management at varying load levels.

---

## Conclusion

DATAS marks a significant shift in Server GC behavior in .NET 10. Evaluate your application's performance and memory requirements to decide whether to adopt, tune, or disable this feature. Use available diagnostic tools and configuration options for optimal results.

---

**Further Reading**

- [.NET GC Configuration Docs](https://learn.microsoft.com/dotnet/core/runtime-config/garbage-collector)
- [Original Blog Post](https://devblogs.microsoft.com/dotnet/preparing-for-dotnet-10-gc/)

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/preparing-for-dotnet-10-gc/)
