---
feed_name: Microsoft Fabric Blog
section_names:
- ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/proactive-and-incremental-statistics-refresh-for-fabric-data-warehouse-and-sql-analytics-endpoint/
title: Proactive and incremental statistics refresh for Fabric Data Warehouse and SQL Analytics Endpoint
tags:
- Analytics Workloads
- Column Statistics
- Data Ingestion
- Fabric Data Warehouse
- Histogram Statistics
- Incremental Statistics Refresh
- Large Tables
- Microsoft Fabric
- ML
- News
- Partitioned Data
- Performance Tuning
- Proactive Statistics Refresh
- Query Compilation
- Query Optimizer
- Query Planning
- Regression Prevention
- Sampling
- SQL Analytics Endpoint
- Statistics Maintenance
- T SQL
- Workspaces
date: 2026-03-19 15:30:00 +00:00
primary_section: ml
author: Microsoft Fabric Blog
---

Microsoft Fabric Blog announces two Fabric SQL performance features—Proactive Statistics Refresh and Incremental Statistics Refresh—for Fabric Data Warehouse and the SQL Analytics Endpoint, aimed at keeping optimizer statistics current and reducing query compilation delays.<!--excerpt_end-->

# Proactive and incremental statistics refresh for Fabric Data Warehouse and SQL Analytics Endpoint

*If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at FabCon and SQLCon announcements across both Fabric and database offerings.*

In December, Microsoft released two optimizations for statistics maintenance in **Fabric Data Warehouse** and the **SQL Analytics Endpoint**:

- **Proactive Statistics Refresh**
- **Incremental Statistics Refresh**

The goal is to keep query optimizer statistics aligned with how data changes over time, improving:

- query plan generation speed
- query stability (fewer regressions after ingestion)
- reduced need for manual statistics maintenance

Modern analytics tables often grow continuously, distributions shift, and “hot” partitions change frequently. If data changes significantly and statistics lag behind, query performance can drift.

## Why statistics matter

The query optimizer uses **column statistics** to estimate:

- row counts
- data distribution for a given selection

These estimates influence planning decisions like:

- join order
- resource estimation used to execute the query

If a query is submitted and the optimizer detects that required statistics are stale (for example, after heavy data change), it may refresh statistics **synchronously during query compilation** to avoid planning with outdated information. That synchronous refresh can increase end-to-end query duration—especially when each statistic requires resampling an entire column.

## What’s new in Fabric

### Proactive statistics refresh

**Proactive statistics refresh** detects statistics staleness **after data change** (instead of waiting for a user-triggered query), and kicks off background statistic updates.

- Runs in the background using built-in policies
- Reduces the likelihood that a `SELECT` query needs to update stats during compilation
- Aims to save time while keeping accuracy

### Incremental statistics refresh

**Incremental statistics refresh** targets large, fast-growing tables where full refreshes are expensive.

Instead of resampling entire columns (typical statistic updates), it:

- assesses only recently added data
- combines the new information with the existing **histogram** statistic

This makes the statistics update operation significantly faster.

![A GIF of multiple T-SQL queries in a Fabric Data Warehouse. First inserting into a table, then checking that statistics have already been refreshed, and finally observing a fast user query execution time. All leveraging proactive statistics refresh and incremental statistics refresh.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/a-gif-of-multiple-t-sql-queries-in-a-fabric-data-w.gif)

*Figure: When applicable, statistics refresh proactively and incrementally, minimizing user query duration.*

## Current observations and reports

Microsoft reports that as of **March 2026**:

- **90% of workspaces** have cut statistic updates during query compilation **by half** due to these features

This reduction shortens the overall query duration users perceive day-to-day.

The post also mentions usage from customers with high-change, low-SLA analytics workloads (frequent data changes, frequent dashboard refreshes, sensitivity to plan shifts). In internal testing with the “IDEAS” internal customer serving Copilot Analytics, enabling both features together reportedly:

- reduced statistic maintenance compute by **5x**
- improved predictability

Outcomes depend on workload characteristics (data volatility, query patterns, table sizes, concurrency).

## Learn more

- Data Warehouse Statistics documentation: https://learn.microsoft.com/fabric/data-warehouse/statistics


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/proactive-and-incremental-statistics-refresh-for-fabric-data-warehouse-and-sql-analytics-endpoint/)

