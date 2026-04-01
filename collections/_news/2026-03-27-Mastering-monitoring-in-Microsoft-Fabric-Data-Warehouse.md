---
date: 2026-03-27 08:00:00 +00:00
title: Mastering monitoring in Microsoft Fabric Data Warehouse
section_names:
- ml
tags:
- Capacity Metrics App
- Capacity Throttling
- Concurrency Pressure
- CU Utilization
- Distributed Statement ID
- DMVs
- Dynamic Management Views
- Fabric Data Warehouse
- Microsoft Fabric
- ML
- Monitoring
- Monitoring UX
- News
- Observability
- Performance Troubleshooting
- Query Activity
- Query Hash
- Query Insights
- SQL Performance
- SQL Pool Insights
external_url: https://blog.fabric.microsoft.com/en-US/blog/mastering-monitoring-in-microsoft-fabric-data-warehouse/
author: Microsoft Fabric Blog
primary_section: ml
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog walks through how to troubleshoot and explain performance issues in Microsoft Fabric Data Warehouse using first-party monitoring tools like DMVs, Query Insights, SQL Pool Insights, and the Capacity Metrics app.<!--excerpt_end-->

## Overview

Modern analytics workloads need **observability** in addition to fast queries. Microsoft Fabric Data Warehouse (DW) includes first-party tools to help you investigate production questions like:

- Why is my query slow right now?
- Which SQL query is slowing a Power BI report?
- Who is consuming the most capacity?
- What caused a performance incident?
- How do I identify and explain capacity throttling?

A related roundup of broader announcements is linked here: [FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news).

## Monitoring tools in Fabric DW

### Real-time troubleshooting with Dynamic Management Views (DMVs)

DMVs are for answering “what’s happening **right now**” by inspecting live requests, sessions, and connections.

Common views:

- `sys.dm_exec_requests`
- `sys.dm_exec_sessions`
- `sys.dm_exec_connections`

Learn more: [Monitor Microsoft Fabric Data Warehouse using DMVs](https://learn.microsoft.com/fabric/data-warehouse/monitor-using-dmv?source=recommendations)

### Historical analysis with Query Insights (QI)

Query Insights captures **completed queries over time** and supports:

- Long-running query patterns
- Frequently executed queries
- Historical performance comparison
- Query attribution by user

Key views:

- `queryinsights.exec_requests_history`
- `queryinsights.long_running_queries`
- `queryinsights.frequently_run_queries`
- `queryinsights.sql_pool_insights`

Learn more: [Query Insights in Microsoft Fabric Data Warehouse](https://learn.microsoft.com/fabric/data-warehouse/query-insights)

### Capacity health and throttling with the Capacity Metrics app

The **Capacity Metrics app** shows Fabric capacity utilization, saturation, throttling events, and system behavior. It helps you determine whether slowness is caused by **capacity pressure** rather than query logic.

Learn more:

- [Capacity Metrics app overview](https://learn.microsoft.com/fabric/enterprise/metrics-app)
- [Capacity Metrics app: Compute page](https://learn.microsoft.com/fabric/enterprise/metrics-app-compute-page)
- [Capacity Metrics app: Timepoint summary page](https://learn.microsoft.com/fabric/enterprise/metrics-app-timepoint-summary-page)

### Code-free Monitoring UX

The Monitoring UX (Query Activity tab) provides UI-based capabilities to:

- View running and completed queries
- Filter by user, status, or time
- Cancel long-running queries (with permissions)

Learn more: [Use Query Activity (Monitoring UX) in Microsoft Fabric Data Warehouse](https://learn.microsoft.com/fabric/data-warehouse/query-activity)

## Scenario 1: Trace a capacity spike back to the exact query

### 1) Notice capacity usage spikes

Start in the **Fabric Capacity Metrics app**. The **CU % over time** chart highlights elevated or sustained compute usage.

![Fabric Capacity Metrics app - CU % over time chart](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/fabric-capacity-metrics-app-cu-over-time-chart.png)

### 2) Identify the operations driving the spike

In **Background operations for time range**:

- Filter to **Warehouse** operations
- Sort by **Total CU (s)**

This narrows the investigation to the highest-impact operations in the spike window.

![Get the OperationId in the drill through experience](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/get-the-operationid-in-the-drill-through-experienc.png)

### 3) Use Operation ID as the distributed statement ID

For Data Warehouse workloads, the Capacity Metrics app **Operation ID** corresponds to the **distributed statement ID**, which uniquely identifies a distributed SQL statement execution. This is the bridge between capacity telemetry and query-level diagnostics.

### 4) Pivot into Query Insights to get the full query context

Use the distributed statement ID in `queryinsights.exec_requests_history` to retrieve:

- SQL text
- Execution duration
- Start/end time
- Historical behavior

This connects a capacity spike to the specific query execution that caused it.

![Correlate operation id from Capacity Metrics App with details QI](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/correlate-operation-id-from-capacity-metrics-app-w.png)

## Scenario 2: Identify a long-running query and compare performance over time

### Identify long-running queries (live) with DMVs

Use `sys.dm_exec_requests` to find active requests consuming time/CPU.

```sql
SELECT
  r.session_id,
  r.start_time,
  r.status,
  r.total_elapsed_time AS elapsed_time_ms,
  s.login_name,
  s.program_name,
  r.query_hash
FROM sys.dm_exec_requests AS r
JOIN sys.dm_exec_sessions AS s
  ON r.session_id = s.session_id
WHERE r.status = 'running'
ORDER BY r.total_elapsed_time DESC;
```

Use this when:

- Users report slowdowns right now
- You need to identify blocking/runaway executions

Important limitation: DMVs show **live state only**; after completion, DMVs won’t keep the query’s execution detail.

### Check pool health before tuning the query (SQL Pool Insights)

Before tuning the SQL, verify whether the **SQL pool was under pressure**.

```sql
SELECT sql_pool_name, timestamp, is_pool_under_pressure
FROM queryinsights.sql_pool_insights
WHERE sql_pool_name = 'SELECT'
  AND timestamp >= DATEADD(hour, -24, GETDATE())
  AND is_pool_under_pressure = 1
ORDER BY timestamp DESC;
```

### Compare historical executions using Query Insights + query_hash

The `queryinsights.exec_requests_history` view stores completed-query history including CPU and data scanned. `query_hash` represents the logical shape of the query (helpful even when literal values differ).

```sql
SELECT
  start_time,
  total_elapsed_time_ms,
  allocated_cpu_time_ms,
  data_scanned_disk_mb,
  data_scanned_memory_mb,
  data_scanned_remote_storage_mb,
  row_count
FROM queryinsights.exec_requests_history
WHERE query_hash = '<query_hash_from_previous_step>'
ORDER BY start_time DESC;
```

How to interpret differences:

- **Higher data scanned** → data growth, missing filters, or plan changes
- **Higher CPU with similar data scanned** → inefficient operators or joins
- **Higher elapsed time without higher CPU** → contention or capacity pressure

## Scenario 3: Evaluate pool pressure before tuning queries (SQL Pool Insights)

Query slowness can be caused by the environment (concurrency, resource contention), not necessarily the query.

### Check whether the pool was under pressure

```sql
SELECT
  sql_pool_name,
  timestamp,
  is_pool_under_pressure,
  max_resource_percentage,
  current_workspace_capacity
FROM queryinsights.sql_pool_insights
WHERE sql_pool_name = 'SELECT'
  AND timestamp >= DATEADD(hour, -24, GETDATE())
  AND is_pool_under_pressure = 1
ORDER BY timestamp DESC;
```

`sql_pool_insights` records event-based telemetry when:

- Pool pressure persists for **>= 1 minute**
- Pool configuration changes
- Workspace capacity SKU changes

If pressure events align with slowdowns, the issue is likely **environmental**.

### Distinguish concurrency pressure vs bad-actor queries

- **High concurrency pressure** → many queries competing
- **Bad actor pressure** → specific queries monopolizing resources

### Correlate pressure windows with executed queries

```sql
WITH PoolPressure AS (
  SELECT DISTINCT timestamp
  FROM queryinsights.sql_pool_insights
  WHERE is_pool_under_pressure = 1
)
SELECT
  erh.distributed_statement_id,
  erh.query_hash,
  erh.command,
  erh.start_time,
  erh.end_time,
  erh.total_elapsed_time_ms,
  erh.allocated_cpu_time_ms,
  erh.data_scanned_disk_mb,
  erh.data_scanned_memory_mb,
  erh.data_scanned_remote_storage_mb
FROM queryinsights.exec_requests_history AS erh
JOIN PoolPressure p
  ON p.timestamp BETWEEN erh.start_time AND erh.end_time
ORDER BY erh.total_elapsed_time_ms DESC;
```

## Closing: an end-to-end troubleshooting flow

A practical sequence to move from symptom to root cause:

- **Detect slowness:** DMVs or Query Activity
- **Evaluate pool health:** SQL Pool Insights
- **Trace impact:** Capacity Metrics → distributed statement ID → Query Insights
- **Explain regression:** Query Insights + `query_hash` comparison over time

The post ends by inviting feedback/questions in the comments.

[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/mastering-monitoring-in-microsoft-fabric-data-warehouse/)

