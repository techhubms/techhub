---
external_url: https://blog.fabric.microsoft.com/en-US/blog/processing-cdc-streams-using-fabric-eventstreams-sql/
title: Processing CDC Streams Using Microsoft Fabric Eventstreams SQL
author: Microsoft Fabric Blog
primary_section: ml
feed_name: Microsoft Fabric Blog
date: 2026-02-24 10:00:00 +00:00
tags:
- Azure
- Azure Data Engineering
- Azure SQL
- Change Data Capture
- Data Transformation
- Debezium
- Eventhouse
- Eventstreams
- Inventory Monitoring
- Lakehouse
- Microsoft Fabric
- ML
- News
- Real Time Data Processing
- SQL
- SQL Query
- Streaming Analytics
section_names:
- azure
- ml
---
Microsoft Fabric Blog showcases how to process real-time CDC streams from databases like Azure SQL using Fabric Eventstreams SQL, making event data analytics-ready with efficient SQL transformations.<!--excerpt_end-->

# Processing CDC Streams Using Microsoft Fabric Eventstreams SQL

Modern applications need to react in real-time to database changes, such as orders, inventory updates, and price changes. Instead of relying on batch pipelines, organizations are turning to real-time processing of change data capture (CDC) events.

## The Need for Real-Time CDC Processing

- Applications can update dashboards, trigger workflows, or feed operational apps instantly.
- Fabric Eventstreams ingests CDC data from data sources like Azure SQL, PostgreSQL, MySQL using Debezium.
- Challenge: Raw CDC events are verbose and require every consumer (analytics, alerts, dashboards) to understand low-level CDC formats and write custom parsing logic.

## Centralized CDC Event Shaping with Eventstreams SQL

- Fabric Eventstreams introduces SQL operators to centralize CDC event shaping.
- Instead of pushing parsing and transformation complexity to each consumer, handle it once in Eventstreams using SQL.

### Example: Grocery Store Inventory and Sales

- Scenario: An Azure SQL database manages sales and inventory across grocery stores.
- Two main tables:
  - `SalesTransactions`: Records sales and returns.
  - `ProductInventory`: Tracks inventory changes.
- Requirement: Monitor inventory in near real-time and alert procurement teams for low stock.

#### CDC Flow

- Connect Azure SQL via CDC connector to Eventstream.
- All CDC events land in a single table in Eventhouse or Lakehouse as raw JSON.
- Consumers then need to filter tables, parse payloads, and normalize events.

#### Solution: Transforming CDC in Eventstreams SQL

- Use SQL statements to filter data-change events, select specific tables (e.g., `ProductInventory`), and flatten nested JSON into structured columns:

```sql
SELECT
    payload.op AS OperationType,
    payload.source.db AS SourceDatabase,
    payload.source.[schema] AS SourceSchema,
    payload.source.[table] AS SourceTable,
    payload.[after].*
INTO [grocerystoreinventory-table]
FROM [grocerystoreinventory-es-stream]
WHERE (payload.[after] IS NOT NULL OR payload.before IS NOT NULL)
  AND payload.source.[table] = 'ProductInventory'
```

- Result: A normalized stream reflecting the source table schema, easing downstream analytics and alerting without custom parsing.

### Benefits

- CDC event complexity handled once in Eventstreams.
- Downstream systems focus on business logic, not low-level details.
- Normalized data streams power analytics (Eventhouse), historical analysis/model training (Lakehouse with Fabric Spark), and workflow triggers (Activator).
- Scaling with more tables and evolving schemas becomes manageable.

### Advanced Use Cases

- Select only required columns using explicit SELECTs.
- Reuse logic with the `WITH` SQL statement.
- Customize processing logic per table easily.

### Learn More

- [Microsoft Fabric Eventstreams Overview](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/overview?tabs=enhancedcapabilities)
- [Add Azure SQL Database CDC source to an eventstream](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/add-source-azure-sql-database-change-data-capture?pivots=extended-features)
- [Process Events Using a SQL Operator](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/process-events-using-sql-code-editor)

## Feedback

If this guide helped you, Microsoft Fabric Blog welcomes your feedback and real-time scenario suggestions at [askeventstreams@microsoft.com](mailto:askeventstreams@microsoft.com) or [Fabric Ideas – Microsoft Fabric Community](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/processing-cdc-streams-using-fabric-eventstreams-sql/)
