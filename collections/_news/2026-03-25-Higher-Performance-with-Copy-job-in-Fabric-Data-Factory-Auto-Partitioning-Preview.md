---
feed_name: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/higher-performance-with-copy-job-in-fabric-data-factory-auto-partitioning-preview/
tags:
- Amazon RDS For SQL Server
- Auto Partitioning
- Azure Data Factory
- Azure SQL Database
- Azure SQL Managed Instance
- Azure Synapse Analytics
- Bulk Copy
- CDC Replication
- Change Data Capture
- Connector Support
- Copy Activity
- Copy Job
- Data Movement
- Data Pipelines
- ETL
- Fabric Data Factory
- Fabric Data Warehouse
- Fabric Lakehouse Tables
- Incremental Copy
- Lakehouse
- Microsoft Fabric
- ML
- News
- Parallel Reads
- Partition Boundaries
- SQL Pool
- SQL Server
- Throughput
- V Order
- Watermark Based Incremental Copy
author: Microsoft Fabric Blog
section_names:
- ml
date: 2026-03-25 08:30:00 +00:00
primary_section: ml
title: Higher Performance with Copy job in Fabric Data Factory Auto Partitioning (Preview)
---

Microsoft Fabric Blog announces preview enhancements to Fabric Data Factory Copy job—auto-partitioning for faster large-table loads and default 2× faster writes to Lakehouse tables—plus how to enable the new settings and which connectors are supported.<!--excerpt_end-->

## Overview

[Copy job](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job) in **Microsoft Fabric Data Factory** is positioned as the main tool for moving data across clouds, with support for:

- Bulk copy
- Incremental copy
- Change data capture (CDC) replication

This update introduces two performance-focused enhancements.

## What’s new

### 1) Auto-partitioning for large datasets (Preview)

Partitioning can drastically improve copy throughput by splitting a large table into chunks that can be read and written concurrently.

Historically, partitioning required manual work in many tools:

1. Identify a partition column (often numeric/date with even distribution)
2. Define partition boundaries (ranges, row counts, hash buckets)
3. Tune parallelism (threads/workers)
4. Test/iterate to avoid skew, hotspots, or excessive source pressure

**Copy job now automates this.** When it detects a large dataset, it analyzes schema and data characteristics to:

- Select a partition column
- Compute balanced boundaries
- Execute parallel reads

#### What this means in practice

- No manual partition configuration (no columns/ranges/parallelism settings)
- Adaptive throughput (more partitions for larger tables; avoids overhead for smaller tables)
- More consistent performance across many tables

#### Supported connectors

- Amazon RDS for SQL Server
- Azure SQL Database
- Azure Synapse Analytics (SQL Pool)
- Fabric Data Warehouse
- SQL database in Fabric
- SQL Server
- Azure SQL Managed Instance

#### Supported copy mode

- Watermark-based incremental copy (including initial full copy + incremental copy)

### 2) 2× faster copy writes to Lakehouse tables by default

When writing to **Fabric Lakehouse tables**, Copy job now targets **2× faster copy performance by default** by **disabling V-Order during ingestion** (reducing write overhead).

- No code changes
- No configuration required
- You can still enable V-Order for writes via **Advanced settings** if you prefer

## If you’re coming from Azure Data Factory (ADF)

The article contrasts ADF Copy activity with Copy job in Fabric:

- **ADF Copy activity**: partitioning is manual (choose partition option/column, bounds, partition count; supported for specific sources)
- **Copy job in Fabric**: partitioning is automatic (Copy job detects large datasets and applies a strategy with no configuration)

The stated shift is that ADF can achieve high performance but typically requires hands-on tuning, while Copy job aims to make high-performance copy the default.

## How to enable / use these features

### Enable auto-partitioning

- In your Copy job, open **Advanced settings**
- Turn on the **Auto-partitioning** toggle

![Snapshot of enabling auto partitioning](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/snapshot-of-enabling-auto-partitioning.png)

### Get faster Lakehouse writes by default

- No action required (V-Order disabled by default for ingestion)
- Optional: enable V-Order for writes in **Advanced settings**

## Learn more (official docs and community)

- [What is Copy job](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job)
- [How to create a Copy job](https://learn.microsoft.com/fabric/data-factory/create-copy-job)
- [How to monitor a Copy job](https://learn.microsoft.com/fabric/data-factory/monitor-copy-job)

Community links:

- [Fabric Ideas (Copy job)](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/data%20factory%20%7C%20copy%20job)
- [Fabric Community (Copy job)](https://community.fabric.microsoft.com/t5/Copy-job/bd-p/db_copyjob)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/higher-performance-with-copy-job-in-fabric-data-factory-auto-partitioning-preview/)

