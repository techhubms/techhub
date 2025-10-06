---
layout: "post"
title: "Introducing Optimized Compaction in Fabric Spark"
description: "This article presents new features for managing Delta Lake table compaction in Microsoft Fabric Spark, focusing on reducing write amplification and automating table maintenance. It covers Fast Optimize, File Level Compaction Target, and Auto Compaction, each designed to improve efficiency, minimize manual intervention, and ensure predictable performance and costs for data engineers working with Lakehouse architectures."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/announcing-optimized-compaction-in-fabric-spark/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-10-06 11:00:00 +00:00
permalink: "/2025-10-06-Introducing-Optimized-Compaction-in-Fabric-Spark.html"
categories: ["Azure", "ML"]
tags: ["Auto Compaction", "Azure", "Blob Storage", "Compaction", "Data Engineering", "Delta Lake", "Delta Table", "ELT", "Fast Optimize", "File Level Compaction Target", "Lakehouse Architecture", "Microsoft Fabric Spark", "ML", "News", "Performance Optimization", "Spark SQL", "Table Maintenance", "Write Amplification"]
tags_normalized: ["auto compaction", "azure", "blob storage", "compaction", "data engineering", "delta lake", "delta table", "elt", "fast optimize", "file level compaction target", "lakehouse architecture", "microsoft fabric spark", "ml", "news", "performance optimization", "spark sql", "table maintenance", "write amplification"]
---

Microsoft Fabric Blog explores advanced compaction strategies for Delta tables in Fabric Spark, helping data engineers reduce manual maintenance, control costs, and keep Lakehouse performance consistent.<!--excerpt_end-->

# Introducing Optimized Compaction in Fabric Spark

Efficient table maintenance is key to sustaining performance and controlling costs in modern Lakehouse architectures. This article explains how Microsoft Fabric Spark introduces three features—Fast Optimize, File Level Compaction Target, and Auto Compaction—to simplify and accelerate Delta table compaction.

## Why Table Compaction Matters

Lakehouse data can fragment over time, leading to inefficient storage and costly query performance degradation. The traditional OPTIMIZE command mitigates this by rewriting small files into optimally sized ones, but as workloads grow and table update patterns change, standard compaction jobs can become wasteful and expensive.

### Common Compaction Challenges

- **Write Amplification**: Same data rewritten repeatedly, especially when table target file sizes change.
- **Manual Maintenance**: Frequent scheduling and monitoring of optimize jobs distracts from core data tasks.
- **Degrading Query Performance**: Small files pile up between optimizations, slowing queries.
- **Unpredictable Costs**: Without automation, compute and storage costs can spike unexpectedly.

## Fast Optimize: Intelligent Compaction Short-circuiting

Fast Optimize introduces smarter file binning, only compacting eligible files that contribute meaningful performance improvements. It analyzes bins of files, skipping those that wouldn't result in sufficiently large compacted files based on configured thresholds like `delta.databricks.delta.optimize.minFileSize`. This feature:

- Reduces unnecessary compute usage.
- Speeds up compaction jobs.
- Minimizes write amplification over time.

To enable:

```python
spark.conf.set('spark.microsoft.delta.optimize.fast.enabled', True)
```

## File Level Compaction Target: Context-aware Optimization

Delta Lake now tags files with metadata about their compaction target size at creation time. This ensures that changing the desired file size later doesn’t recompact files that already meet previous optimization criteria, reducing redundant data movement. Enable it at the session level:

```python
spark.conf.set('spark.microsoft.delta.optimize.fileLevelTarget.enabled', True)
```

## Auto Compaction: Fully Automated Maintenance

Auto Compaction continually monitors the file size distribution on each write, automatically triggering optimizations when too many small files accumulate. This removes the need for scheduled jobs and keeps performance steady. Enable it with:

Session:

```python
spark.conf.set('spark.databricks.delta.autoCompact.enabled', True)
```

Table:

```sql
CREATE TABLE dbo.ac_enabled_table TBLPROPERTIES ('delta.autoOptimize.autoCompact' = 'true')
ALTER TABLE dbo.ac_enabled_table SET TBLPROPERTIES ('delta.autoOptimize.autoCompact' = 'true')
```

### Benefits

- Tables avoid the typical sawtooth pattern of performance degradation.
- Compute resources are used only when needed, reducing costs.
- Little manual intervention required.

## Conclusion: A Maintenance-Free Future

With Fast Optimize, File Level Compaction Target, and Auto Compaction, Microsoft Fabric Spark provides data engineers with tools to:

- Prevent write amplification and reduce overhead.
- Maintain optimal query performance.
- Automate table maintenance and minimize operational burden.

All features are compatible with existing Delta Lake tables. For further setup details, refer to the [official documentation](https://learn.microsoft.com/fabric/data-engineering/table-compaction?tabs=sparksql).

---
*Author: Microsoft Fabric Blog*

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/announcing-optimized-compaction-in-fabric-spark/)
