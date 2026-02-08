---
external_url: https://blog.fabric.microsoft.com/en-US/blog/mirroring-sql-server-postgres-db-cosmos-db-and-updates-to-snowflake-mirroring-now-ga/
title: 'Mirroring Operational Databases to Microsoft Fabric OneLake: SQL Server, PostgreSQL, Cosmos DB, Snowflake & Qlik Integrations Now GA'
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-11-19 09:00:00 +00:00
tags:
- Azure Cosmos DB
- Azure PostgreSQL
- BI
- Data Engineering
- Delta Tables
- Governance
- Hybrid Cloud
- Iceberg Tables
- Microsoft Fabric
- Mirroring
- Multi Cloud
- OneLake
- Operational Data
- Qlik
- Real Time Analytics
- Schema Evolution
- Snowflake
- SQL Server
- T SQL Endpoint
- Zero ETL
- AI
- Azure
- ML
- News
section_names:
- ai
- azure
- ml
primary_section: ai
---
Microsoft Fabric Blog details how Mirroring, now generally available, enables organizations to replicate operational data from SQL Server, PostgreSQL, Cosmos DB, Snowflake, and Qlik sources into OneLake for real-time analytics. Authored by the Microsoft Fabric team.<!--excerpt_end-->

# Mirroring Operational Databases to Microsoft Fabric OneLake: SQL Server, PostgreSQL, Cosmos DB, Snowflake & Qlik Integrations Now GA

Data silos frequently slow down innovation in organizations. Microsoft Fabric's Mirroring feature now removes these barriers by enabling operational data—including from SQL Server, Azure PostgreSQL, Cosmos DB, Snowflake, and Qlik-connected sources—to be brought seamlessly into OneLake, eliminating the need for complex ETL pipelines.

## Key Features and General Availability Announcements

### SQL Server Mirroring (2016–2022 & 2025)

- Native support for SQL Server versions 2016–2022 & SQL Server 2025 now generally available.
- Secure replication into OneLake, transforming operational data to Delta tables in an analytics-ready format.
- Automatic SQL analytics endpoint for familiar T-SQL access to read-only, mirrored data—no ETL required.
- Supported environments: on-premises, Azure VMs, and non-Azure clouds; secure connectivity via on-premises data gateway.
- Learn more: [Mirroring SQL Server](https://learn.microsoft.com/fabric/mirroring/sql-server)

> “With Fabric Mirroring in SQL Server 2025, ExponentHR can effortlessly mirror numerous datasets to fabric, enabling near real-time analytics. This technology has alleviated the need for expensive and complex ETL operations and enables more productivity for our customers. Thanks to SQL Server 2025’s built-in cloud connectivity, we can directly process large amounts of data efficiently and overcome traditional bottlenecks.”
> Brent Carlson, IT Manager, ExponentHR

### Snowflake Mirroring for Iceberg Tables

- Snowflake Mirroring now supports Apache Iceberg tables (as well as managed tables).
- Shortcut-based mirroring brings Iceberg datasets from ADLS Gen2, AWS S3, Google Cloud Storage, and S3-compatible sources into OneLake for high-performance, AI- and analytics-ready workloads.
- Manage and analyze all mirrored tables centrally; gain open format interoperability and reduced costs.
- More info: [Snowflake Mirroring Tutorial](https://learn.microsoft.com/fabric/mirroring/snowflake-tutorial)

### Azure PostgreSQL Mirroring

- Mirroring for Azure Database for PostgreSQL Flexible Server is now generally available.
- Enables continuous real-time replication to OneLake, converting transactional data to Delta tables.
- Supports schema evolution and T-SQL analytics endpoint read access.
- Use cases: customer analytics, inventory optimization, real-time reporting.
- More info: [PostgreSQL Mirroring Docs](https://learn.microsoft.com/fabric/mirroring/azure-database-postgresql)

### Cosmos DB Mirroring

- General availability for Cosmos DB Mirroring brings globally distributed NoSQL data into OneLake.
- Continuous change capture converts mirrored containers into Delta tables; automatic schema inference; supports nested JSON formats.
- Use cases include real-time personalization, fraud detection, IoT telemetry analytics.
- More info: [Cosmos DB Mirroring Docs](https://learn.microsoft.com/fabric/mirroring/azure-cosmos-db) |
  [Cosmos Fabric GA Blog](https://aka.ms/cosmos-fabric-ga-blog)

### Qlik Open Mirroring Ecosystem Integration

- Qlik’s Open Mirroring supports more than 40 sources including SAP and DB2, automating the streaming of operational data into OneLake.
- Reduces ETL complexity, minimizes source impact, and delivers near real-time insights for BI, AI, and data science workloads.
- Tested effectiveness: For example, Eastman improved their SAP operational mirroring with reduced latency and increased stability.
- More info: [Qlik and Microsoft Fabric Blog](https://www.qlik.com/blog/qlik-microsoft-fabric-open-mirroring-the-fast-track-to-real-time-data)

> “Qlik Replicate’s Open Mirroring integration has dramatically reduced end-to-end latency from over 10 minutes to under one minute and moved us from daily operational interventions to sustained, long-term stability. This combination has transformed our ability to deliver real-time insights to Microsoft Fabric targets with the reliability and performance our teams require.”
> Ben Hobbs, Senior Data Architecture Administrator at Eastman

## Why This Changes the Game

- **Accelerated Insights**: Analytics-ready data instantly, with zero ETL.
- **Simplified Governance**: Unified data management under OneLake.
- **Reduced Cost and Complexity**: Works in hybrid and multi-cloud environments.

## What’s Next

- More connectors, performance optimizations, and deeper integrations are coming.
- Watch for roadmap updates and private preview opportunities.
- Engage with Microsoft Fabric Ideas and provide feedback.
- [Try Mirroring in Fabric – Sign up for a free trial](https://aka.ms/try-fabric)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/mirroring-sql-server-postgres-db-cosmos-db-and-updates-to-snowflake-mirroring-now-ga/)
