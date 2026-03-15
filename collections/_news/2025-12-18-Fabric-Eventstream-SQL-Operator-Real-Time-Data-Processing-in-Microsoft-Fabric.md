---
external_url: https://blog.fabric.microsoft.com/en-US/blog/fabric-eventstream-sql-operator-your-tool-kit-to-real-time-data-processing-in-fabric-real-time-intelligence/
title: 'Fabric Eventstream SQL Operator: Real-Time Data Processing in Microsoft Fabric'
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-12-18 09:59:11 +00:00
tags:
- Aggregation
- Anomaly Detection
- Azure Stream Analytics
- Data Engineering
- Data Pipeline
- Data Transformation
- Event Driven Architecture
- Eventstream
- Low Code
- Microsoft Fabric
- Power BI
- Real Time Intelligence
- SQL Operator
- SQL Query
- Streaming Data
- Azure
- ML
- News
section_names:
- azure
- ml
primary_section: ml
---
The Microsoft Fabric Blog details how the new SQL operator in Fabric Eventstream empowers data engineers and analysts to build powerful real-time data processing and anomaly detection solutions using familiar SQL syntax within the Fabric Real-Time Intelligence suite.<!--excerpt_end-->

# Fabric Eventstream SQL Operator: Your Tool Kit to Real-Time Data Processing in Fabric Real-Time Intelligence

**Author:** Microsoft Fabric Blog

## Introduction

As organizations require faster insights from their data, the Microsoft Fabric team introduces the SQL operator for Eventstream. This feature empowers users to ingest, transform, extract insights, and route streaming data using SQL directly within Fabric Real-Time Intelligence, minimizing response time between data events and analytics.

## Why the SQL Operator Matters

- **Familiarity**: Users can apply standard SQL logic to streaming data, leveraging pre-existing SQL knowledge.
- **Low-Code & Flexibility**: Supports both low-code real-time processing and integration with no-code operators. Runs on the reliable Azure Stream Analytics runtime.
- **Performance**: SQL-based transformations occur upstream, reducing post-processing and ensuring agile and efficient pipelines.

## Capabilities and Built-in Functions

The SQL operator enables rich data transformations on streaming data including:

- **Aggregation**: Compute sums, averages, counts (e.g., per-minute sales totals).
- **Windowing**: Analyze events within set time frames (Tumbling, Sliding windows).
- **Advanced Detects**: Outlier and anomaly detection using statistical logic.
- **Bot Detection**: Identify abnormal event bursts with SQL HAVING clauses.
- **Built-in functions**: Aggregate, analytic, array, geospatial, metadata, record, windowing, and scalar functions. ([Documentation](https://learn.microsoft.com/stream-analytics-query/built-in-functions-azure-stream-analytics))

## Practical Scenario: E-commerce Real-Time Analytics

Imagine an e-commerce platform sending order events to Eventstream. Using the SQL operator, you can:

1. **Calculate per-minute sales totals by city**: Using aggregations to compute order count, total revenue, and average order value—detecting high value spikes.
2. **Detect bot attacks**: Use a sliding window aggregate to find customers placing more than 10 orders in two minutes.
3. **Spot outlier orders**: Flag orders 1.5× above their city's five-minute rolling average.

Key steps include:

- Setting up Eventstream with a custom endpoint.
- Adding SQL operators for each transformation need.
- Using the SQL query editor to create, test, and refine logic.
- Outputting results to destinations like Power BI or dashboards.

## Example SQL Queries

### Summing Sales Per Minute By City

```sql
SELECT System.Timestamp AS WindowEnd, city, COUNT(orderId) AS OrderCount,
SUM(orderAmount) AS TotalRevenue, AVG(orderAmount) AS AvgOrderValue
INTO citySalesAgg
FROM [ECommerceExample-stream]
GROUP BY city, TumblingWindow(minute, 1)
```

### Detecting Bot-Like Order Bursts

```sql
SELECT System.Timestamp AS WindowEnd, customerId, COUNT(orderId) AS OrdersPerCustomer
INTO SuspiciousOrders
FROM [ECommerceExample-stream]
GROUP BY customerId, SlidingWindow(minute, 2)
HAVING COUNT(orderId) >= 10
```

### Identifying Outlier Orders

```sql
WITH CityAverages AS (
    SELECT System.Timestamp AS windowEnd, city,
    AVG(orderAmount) AS RollingAvg
    FROM [ECommerceExample-stream] TIMESTAMP BY eventTime
    GROUP BY city, SlidingWindow(minute, 5)
)
SELECT o.orderId, o.city, o.orderAmount, a.RollingAvg, a.windowEnd,
CASE WHEN o.orderAmount > a.RollingAvg * 1.5 THEN 'Anomaly' ELSE 'Normal' END AS OrderStatus
INTO Activator
FROM [ECommerceExample-stream] o TIMESTAMP BY eventTime
JOIN CityAverages a ON o.city = a.city AND DATEDIFF(second, o, a) BETWEEN 0 AND 60
```

These samples illustrate real-world, scalable streaming analytics within Microsoft Fabric.

## SQL Operator Benefits

- **Centralized logic**: Implement all rules in one SQL node with easy incremental development.
- **Rapid testing**: Built-in editor with test view streamlines debugging and validation.
- **Scalability**: Supports production-grade stream analytics and can route outputs to various systems (BI dashboards, alerts, more).

## Further Learning & Community

- **Documentation**: [Built-in SQL Functions](https://learn.microsoft.com/stream-analytics-query/built-in-functions-azure-stream-analytics)
- **Language Reference**: [MS Learn: Stream Analytics Query Language](https://learn.microsoft.com/en-us/stream-analytics-query/stream-analytics-query-language-reference)
- **Get help/forum**: [Real-Time Intelligence Forum](https://community.fabric.microsoft.com/t5/Get-Help-with-Real-Time/ct-p/da_gethelp)
- **Submit ideas**: [Fabric Ideas – Microsoft Fabric Community](https://aka.ms/Fabric.Kusto.Ideas)

## Summary

With robust SQL support in Eventstream, Microsoft Fabric enables advanced real-time data analytics and engineering directly on streaming pipelines. SQL operators allow data professionals to rapidly build flexible, powerful event-driven architectures for immediate business value.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/fabric-eventstream-sql-operator-your-tool-kit-to-real-time-data-processing-in-fabric-real-time-intelligence/)
