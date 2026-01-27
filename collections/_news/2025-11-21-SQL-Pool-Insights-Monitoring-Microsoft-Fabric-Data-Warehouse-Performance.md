---
external_url: https://blog.fabric.microsoft.com/en-US/blog/introducing-sql-pool-insights-in-microsoft-fabric-data-warehouse/
title: 'SQL Pool Insights: Monitoring Microsoft Fabric Data Warehouse Performance'
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-11-21 08:45:00 +00:00
tags:
- Analytics Workloads
- Capacity Planning
- Data Engineering
- Data Warehouse
- Event Logging
- Fabric Data Warehousing
- Historical Data
- Microsoft Fabric
- Performance Monitoring
- Pressure Events
- Query Insights
- Resource Utilization
- SELECT Pool
- SQL Pool Insights
- SQL Pools
- Telemetry
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog presents an in-depth look at SQL Pool Insights, offering developers and analytics engineers new tools for monitoring SQL pool performance and optimizing resource usage in Fabric Data Warehouse.<!--excerpt_end-->

# SQL Pool Insights: Monitoring Microsoft Fabric Data Warehouse Performance

Microsoft Fabric Data Warehouse now includes **SQL Pool Insights**, an advanced telemetry feature designed to help analytics teams monitor SQL pool health, resource allocation, and query performance. As part of the **Query Insights (QI) schema**, SQL Pool Insights introduces pool-level visibility to support efficient capacity planning and fast troubleshooting for mission-critical workloads.

## Why SQL Pool Insights Matters

Modern analytics demand transparency and control. Previously, understanding how SELECT and NON SELECT pools handled pressure was challenging. SQL Pool Insights delivers:

- **Real-time monitoring** of pool pressure and health
- **Historical records** for pool configuration and capacity changes
- **Event logging** for sustained pressure events
- **Validation of resource isolation** between pools

These capabilities enhance troubleshooting, optimize performance, and assist with strategic capacity adjustments.

## Key Capabilities

- **Out-of-the-box Pool Coverage:** Monitor SELECT and NON SELECT pools, with upcoming support for custom pools
- **Granular Schema:** Analyze fields such as `sql_pool_name`, `timestamp`, `max_resource_percentage`, and `is_pool_under_pressure`
- **Correlated Insights:** Integrate SQL Pool Insights data with Query Insights views like `exec_requests_history` and `long_running_queries` for comprehensive monitoring

## How SQL Pool Insights Works

SQL Pool Insights adds a new system view to [Query Insights](https://learn.microsoft.com/fabric/data-warehouse/query-insights):

```sql
SELECT * FROM queryinsights.sql_pool_insights;
```

Event logging occurs when:

- Pool configuration changes
- Workspace capacity SKU changes
- Pressure state persists longer than one minute

## Example Scenarios

**Identify Pool Pressure Periods:**

```sql
SELECT sql_pool_name, timestamp, is_pool_under_pressure
FROM queryinsights.sql_pool_insights
WHERE sql_pool_name = 'SELECT'
  AND timestamp >= DATEADD(hour, -24, GETDATE())
  AND is_pool_under_pressure = 1
ORDER BY timestamp DESC;
```

**Visualize Pressure Trends:**

```sql
SELECT sql_pool_name, timestamp, is_pool_under_pressure,
    LAG(timestamp) OVER (PARTITION BY sql_pool_name ORDER BY timestamp) AS previous_event,
    DATEDIFF(minute, LAG(timestamp) OVER (PARTITION BY sql_pool_name ORDER BY timestamp), timestamp) AS minutes_since_last_event
FROM queryinsights.sql_pool_insights
WHERE sql_pool_name = 'SELECT'
ORDER BY timestamp;
```

**Correlate Pressure with Query History:**

```sql
WITH Pool_Pressure_Events AS ( SELECT DISTINCT timestamp FROM queryinsights.sql_pool_insights WHERE is_pool_under_pressure = 1 )
SELECT *
FROM queryinsights.exec_requests_history AS erh
JOIN Pool_Pressure_Events AS ppe ON ppe.timestamp BETWEEN erh.start_time AND erh.end_time
```

## Use Cases for SQL Pool Insights

- Diagnosis of performance issues and bottlenecks
- Validation of resource isolation for different workloads
- Planning capacity upgrades or adjustments using historical pool data
- Troubleshooting slow queries by correlating pressure and request history

## Learn More

Access [Query Insights in Fabric Data Warehousing](https://learn.microsoft.com/fabric/data-warehouse/query-insights) to explore documentation and integration options for your analytics platform.

---

*Authored by Microsoft Fabric Blog*

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/introducing-sql-pool-insights-in-microsoft-fabric-data-warehouse/)
