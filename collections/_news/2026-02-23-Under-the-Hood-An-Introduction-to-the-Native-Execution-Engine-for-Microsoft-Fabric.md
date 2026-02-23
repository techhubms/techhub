---
layout: "post"
title: "Under the Hood: An Introduction to the Native Execution Engine for Microsoft Fabric"
description: "This post from the Microsoft Fabric Blog explores how the Native Execution Engine brings high-performance, vectorized C++ execution to Spark workloads in Microsoft Fabric. It covers why a new execution model is needed, technical details of the integration with Spark, and how users can leverage this feature for faster, more efficient analytical pipelines—all without changing their existing Spark code."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/under-the-hood-an-introduction-to-the-native-execution-engine-for-microsoft-fabric/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2026-02-23 14:00:00 +00:00
permalink: "/2026-02-23-Under-the-Hood-An-Introduction-to-the-Native-Execution-Engine-for-Microsoft-Fabric.html"
categories: ["Azure", "ML"]
tags: ["Analytics Performance", "Apache Gluten", "Apache Spark", "Azure", "C++", "Columnar Processing", "Data Engineering", "Delta Lake", "Microsoft Fabric", "ML", "Native Execution Engine", "News", "Parquet", "SIMD", "Spark Optimization", "Spark Workloads", "Vectorized Execution", "Velox"]
tags_normalized: ["analytics performance", "apache gluten", "apache spark", "azure", "cplusplus", "columnar processing", "data engineering", "delta lake", "microsoft fabric", "ml", "native execution engine", "news", "parquet", "simd", "spark optimization", "spark workloads", "vectorized execution", "velox"]
---

Microsoft Fabric Blog explains the Native Execution Engine, a performance-boosting vectorized execution layer for Spark on Fabric. This technical write-up guides readers through its architecture, real-world advantages, and how existing Spark workflows can benefit. Authored by the Microsoft Fabric Blog team.<!--excerpt_end-->

# Under the Hood: An Introduction to the Native Execution Engine for Microsoft Fabric

## Introduction

Organizations are processing ever-larger datasets, straining the performance of analytics pipelines. While Apache Spark is a long-standing foundation for large-scale data processing, growing data volumes and real-time demands require even greater speed and efficiency.

Microsoft Fabric addresses these needs with the Native Execution Engine, a new vectorized C++ execution layer that accelerates Spark workloads without any code changes and at no extra compute cost.

## Why Spark Needed a New Execution Approach

Spark's JVM-based execution introduces limitations such as garbage collection overhead, inefficient row-based processing for columnar data (e.g., Parquet and Delta Lake), and underutilization of modern CPU vectorization (SIMD). As analytical workloads increase in frequency and complexity, traditional Spark runtimes may not keep up with demand or cost expectations.

## The Native Execution Engine in Fabric

Fabric's Native Execution Engine enhances Spark by offloading compute-heavy routines to a native C++ backend while Spark retains familiar user-facing APIs and responsibilities like planning, scheduling, and fault tolerance.

- **Key technologies:**
  - **Velox (by Meta):** Vectorized, native C++ execution over columnar data
  - **Apache Gluten:** Connects Spark's execution plans to Velox, facilitating native execution

With this architecture, users can instantly benefit from performance improvements simply by enabling the engine via configuration—no code rewrites required.

#### Optimized Workloads

- Best suited for Parquet/Delta workflows, complex aggregations, and transformations
- Delivers speedup by maximizing columnar processing and vectorization
- Avoids costly data conversions and serialization

## How It Works

The engine selectively routes operators from Spark plans to Velox through Gluten. Supported operations run in high-performance C++ with columnar layouts and SIMD acceleration. Non-supported operations fall back to JVM-based Spark with minimal overhead.

- Example: Benchmarking at TPC‑DS SF 1000 using Delta format yielded up to **6× speedup** vs open-source Spark and 83% compute-cost savings.
- Users can compare runs by toggling `spark.native.enabled` and reviewing performance metrics.

## Technical Advantages of Columnar & SIMD Execution

Instead of using row-based processing, the engine stores columns contiguously in memory, unlocking CPU caching benefits and parallel computation via SIMD. Operators like aggregation and filtering can process vectors of values at once, significantly boosting throughput and overall performance.

- **Benefits**:
  - Improved memory locality
  - Reduced overhead from JVM garbage collection
  - Higher utilization of modern processor features

## Seamless Integration with Spark Optimizer

Native Execution Engine acts after Spark’s logical and physical optimizations, preserving all enhancements (adaptive execution, column pruning, predicate pushdown). This guarantees no disruptions for developers—the familiar development and deployment workflows remain unchanged.

1. Spark optimizer builds query plan
2. Gluten intercepts the plan, translating supported nodes to native execution
3. Velox executes columnar, vectorized operators
4. Unsupported features gracefully fall back to JVM Spark

## Real-Time Fallback Alerts

Some Spark features/operators may not be supported natively. The updated Spark Advisor in Fabric provides real-time alerts and troubleshooting guidance in notebooks, helping data engineers identify when and why queries fallback to the JVM path.

- Example: `.show()` triggers fallback; using `.collect()` keeps the computation on the native engine.

## Getting Started

To take advantage of the Native Execution Engine:

- Enable it globally in environment settings or toggle in code via documented configuration
- Use built-in tools (Spark Advisor, run series, query monitoring) to evaluate and tune workload performance
- Review operator support in [Apache Gluten documentation](https://github.com/apache/incubator-gluten/blob/main/docs/velox-backend-support-progress.md)

## Summary

By harnessing native C++ execution and advanced vectorization, Microsoft Fabric’s Native Execution Engine lets Spark users realize drastic speedups and cost savings—all with their existing codebase and cloud resources.

---
**References & Further Reading**

- [Native Execution Engine Overview](https://learn.microsoft.com/fabric/data-engineering/native-execution-engine-overview?tabs=sparksql)
- [Apache Gluten Support Documentation](https://github.com/apache/incubator-gluten/blob/main/docs/velox-backend-support-progress.md)
- [NYC Taxi Dataset](https://learn.microsoft.com/azure/open-datasets/dataset-taxi-yellow?tabs=azureml-opendatasets)
- [Spark Advisor Introduction](https://learn.microsoft.com/fabric/data-engineering/spark-advisor-introduction)

*Published by Microsoft Fabric Blog.*

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/under-the-hood-an-introduction-to-the-native-execution-engine-for-microsoft-fabric/)
