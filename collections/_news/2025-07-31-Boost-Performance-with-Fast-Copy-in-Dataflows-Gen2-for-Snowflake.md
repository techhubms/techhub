---
external_url: https://blog.fabric.microsoft.com/en-US/blog/boost-performance-with-fast-copy-in-dataflows-gen2-for-snowflake/
title: Boost Performance with Fast Copy in Dataflows Gen2 for Snowflake
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-07-31 11:00:00 +00:00
tags:
- Benchmarking
- Big Data
- Copy Activity
- Data Ingestion
- Data Migration
- Data Pipelines
- Dataflows Gen2
- ETL
- Fast Copy
- Lakehouse
- Microsoft Fabric
- Performance Optimization
- Production Workloads
- Snowflake
- Machine Learning
- News
section_names:
- ml
primary_section: ml
---
Microsoft Fabric Blog demonstrates how Fast Copy in Dataflows Gen2 dramatically improves Snowflake-to-Lakehouse loading performance, with insights and benchmarks for data engineers.<!--excerpt_end-->

# Boost Performance with Fast Copy in Dataflows Gen2 for Snowflake

**Author:** Microsoft Fabric Blog

Fast Copy in Dataflows Gen2 is designed to enhance the speed and cost efficiency of data loading operations within Microsoft Fabric. By utilizing the same optimized backend as the Copy Activity used in data pipelines, Fast Copy can significantly improve your production workloads when moving large datasets, especially from sources like Snowflake.

## What is Fast Copy in Dataflows Gen2?

- Fast Copy is now generally available and enabled by default in all new Dataflows Gen2.
- It leverages high-performance backend technology to drastically reduce processing time and compute costs during data ingestion.

## Real-World Performance Comparison

The blog presents a scenario where 180 million rows from Snowflake are loaded into a Microsoft Fabric Lakehouse table. The performance impact of enabling Fast Copy is measured as follows:

### Case 1: Without Fast Copy

- Dataflows Gen2 loads 180M rows from Snowflake (Fast Copy disabled)
- Data is written into a Lakehouse destination
- **Result:** Full refresh takes approximately **42 minutes**

### Case 2: With Fast Copy

- Same data loading setup, but Fast Copy enabled (default setting)
- **Result:** Ingestion completes in about **5 minutes**

### Summary Table

| Scenario         | Time to Load 180M Rows |
|------------------|-----------------------|
| Without Fast Copy| 42 minutes            |
| With Fast Copy   | 5 minutes             |

- This showcases an **eightfold performance improvement** for bulk data ingestion use cases.

## Getting Started

To leverage Fast Copy:

1. Create a Dataflow Gen2 that pulls data from Snowflake (or similar sources)
2. Ensure Fast Copy is enabled (default setting)
3. Set Lakehouse as the output destination
4. Publish and refresh the dataflow

## Resources

- [Fast copy in Dataflows Gen2 – Microsoft Fabric | Microsoft Learn](https://learn.microsoft.com/fabric/data-factory/dataflows-gen2-fast-copy)
- [Microsoft Data Factory Community Forum](https://aka.ms/DataFactoryCommunity)
- [Microsoft Fabric Ideas portal](https://aka.ms/FabricIdeas)

By adopting Fast Copy in your dataflows, especially for high-volume scenarios involving Snowflake and Microsoft Fabric Lakehouse, you can achieve significant performance and efficiency gains.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/boost-performance-with-fast-copy-in-dataflows-gen2-for-snowflake/)
