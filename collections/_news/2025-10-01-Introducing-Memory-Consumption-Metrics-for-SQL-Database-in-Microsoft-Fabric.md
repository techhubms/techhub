---
external_url: https://blog.fabric.microsoft.com/en-US/blog/memory-consumption-metrics-now-available-for-fabric-sql-database/
title: Introducing Memory Consumption Metrics for SQL Database in Microsoft Fabric
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-10-01 15:00:00 +00:00
tags:
- Automatic Index
- CPU Usage
- Dashboard Metrics
- Data Engineering
- Data Workloads
- Database Administration
- Database Metrics
- Database Optimization
- Memory Consumption
- Metrics Tracking
- Microsoft Fabric
- Performance Dashboard
- Performance Monitoring
- Query Analysis
- Query Performance
- Resource Utilization
- SQL Database
- Tempdb Spillover
- Visualization
- Azure
- ML
- News
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog introduces real-time memory consumption metrics for SQL Database in Fabric, helping administrators monitor, analyze, and optimize query resource usage more effectively.<!--excerpt_end-->

# Memory Consumption Metrics in Fabric SQL Database

## Overview

SQL Database in Microsoft Fabric now features expanded monitoring capabilities, most notably the addition of real-time memory consumption metrics. This enhancement builds on the earlier release of the performance dashboard, aimed at empowering administrators to efficiently monitor, analyze, and optimize database resources.

## What’s New?

- **Memory Consumption Metrics**: Track real-time and historical memory usage at the individual query level.
- **Comprehensive Dashboard**: Access CPU usage, user connections, requests per second, blocked queries, database size, automatic index info, query performance, and now, memory consumption.
- **Tempdb Spillover Detection**: Easily spot when queries exceed available memory and spill over to tempdb, with the dashboard showing the timing and magnitude of these events.
- **Interactive Query Drill-Down**: Select and inspect executed queries, view query text, analyze execution counts over time, and compare CPU/memory consumption across periods.

## Visualizing Memory Consumption

The performance dashboard includes a dedicated tab for memory metrics, featuring:

- Bar charts that show query memory usage for customizable time intervals (MB/GB).
- Trend analysis to quickly identify spikes, bottlenecks, or patterns in resource use.

## Query-Level Insights

Administrators can drill down into:

- **Query text**: Instantly copy details for further analysis in Fabric’s editor, VS Code, or SQL Server Management Studio.
- **Execution counts**: View how often queries run and analyze for anomalies or changes over time.
- **Comparative Analysis**: Directly compare resource consumption between time intervals for optimization purposes.

## Getting Started

- The memory metrics feature is still rolling out and may become available to all customers within a month.
- To access, navigate to your SQL Database in Fabric, use the Performance summary button, and open the Performance dashboard.
- Further details are in the [official documentation](https://learn.microsoft.com/fabric/database/sql/performance-dashboard).

## Feedback

Microsoft Fabric welcomes feedback and suggestions via the [Fabric Ideas page](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas), enabling ongoing improvements to monitoring tools and user experience.

---

By introducing fine-grained memory metrics, Microsoft Fabric empowers database administrators and data engineers to maintain performance, proactively detect inefficiencies, and make data-driven decisions about resource allocation.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/memory-consumption-metrics-now-available-for-fabric-sql-database/)
