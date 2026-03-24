---
feed_name: Microsoft Fabric Blog
author: Microsoft Fabric Blog
primary_section: ml
date: 2026-03-24 13:00:00 +00:00
external_url: https://blog.fabric.microsoft.com/en-US/blog/outstanding-connectivity-for-data-movement-in-fabric-data-factory/
title: Outstanding connectivity for data movement in Fabric Data Factory
section_names:
- ml
tags:
- Amazon RDS
- Auto Partitioning
- Azure Data Explorer
- Azure SQL Database
- Azure SQL Managed Instance
- Azure Synapse Analytics
- Connectors
- Copy Job
- Data Movement
- Fabric Data Factory
- Fabric Data Warehouse
- Fabric Lakehouse
- Google BigQuery
- Google Cloud Storage
- Microsoft Fabric
- ML
- MySQL
- Native Incremental Copy
- News
- ODBC
- Pipelines
- PostgreSQL
- SAP Datasphere
- SharePoint Online
---

Microsoft Fabric Blog outlines new Microsoft Fabric Data Factory connectivity updates, including new/expanded connectors, native incremental copy support, and automatic partitioning to improve large-scale data movement via Copy job and Pipelines.<!--excerpt_end-->

## Overview

In today’s data landscape, success depends not just on analytics, but on reliably moving data across systems. This update focuses on **Microsoft Fabric Data Factory** improvements for enterprise data movement, with enhancements across **Copy job** and **Pipeline**.

> If you haven’t already, see Arun Ulag’s roundup: [FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news)

## What’s new in Fabric Data Factory

Fabric data movement needs to cover patterns from simple bulk copy to orchestrated, enterprise-scale pipelines. The release expands connector coverage across both **Copy job** and **Pipeline**.

### SharePoint Online File Connector (Source & Destination) (GA)

The **SharePoint Online File** connector now supports both **source** and **destination** in Copy job and Pipeline, enabling:

- Ingesting files from SharePoint Online into **Fabric Lakehouse** (or other destinations)
- Publishing data/outputs back to SharePoint Online
- Orchestrating SharePoint-based workflows inside pipelines

### Google BigQuery Connector (Preview)

The **Google BigQuery** connector now supports **destination** writes, enabling outbound data movement into BigQuery for:

- Cross-cloud data publishing
- Hybrid analytics architectures
- Federated data platform strategies
- Scheduled or event-driven sync via Pipelines

![Screenshot of Google BiqQuery connector as destination.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-google-biqquery-connector-as-destina.png)

*Figure: Screenshot of Google BiqQuery connector as destination.*

### MySQL Connector (Preview)

**MySQL** now supports destination writes, enabling bi-directional data movement scenarios.

![Screenshot of MySQL connector as destination.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/word-image-34691-2.png)

*Figure Screenshot of MySQL connector as destination.*

### PostgreSQL Connector (Preview)

**PostgreSQL** now supports destination writes, enabling bi-directional movement between Fabric and PostgreSQL environments.

![Screenshot of PostgreSQL connector as destination.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-postgresql-connector-as-destination-.png)

*Figure: Screenshot of PostgreSQL connector as destination.*

### SAP Datasphere Connector (GA)

A new **SAP Datasphere** connector enables sourcing data directly from SAP’s data warehouse cloud solution.

Capabilities called out for Copy job:

- Move SAP data (bulk, incremental, or CDC) to destinations across clouds, with table/column mapping
- Enterprise security and compliance options (including **VNet gateways** and robust authentication)
- Orchestrate end-to-end movement and transformations into a single pipeline for unified analytics/AI
- Extraction from SAP-managed business data models
- Streamlined ingestion into Fabric for unified analytics
- Reduced friction between SAP estates and Microsoft Fabric

![Screenshot of SAP Datasphere connector in Copy job.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-sap-datasphere-connector-in-copy-job.png)

*Figure: Screenshot of SAP Datasphere connector in Copy job.*

## Native incremental copy (no-code)

Fabric Data Factory has expanded **native incremental copy** in Copy job with a no-code experience. Incremental copy moves only newly added or updated records to:

- Reduce load on source systems
- Improve efficiency
- Minimize costs

The post positions this as enabling scalable **CDC-style** synchronization patterns without complex custom logic.

### New connectors supporting native incremental copy

- Amazon RDS for SQL Server
- Amazon RDS for Oracle
- Azure Data Explorer
- Azure Files
- Google Cloud Storage
- IBM Db2
- ODBC
- Fabric Lakehouse tables / files
- SharePoint Lists
- SharePoint Online Files

## Auto-partitioning for faster large-table loads

For large tables (millions of rows), partitioning can dramatically improve throughput by enabling parallel reads/writes. Traditionally, partitioning required manual setup and re-tuning.

Copy job now supports **automatic partitioning**:

- Detects large datasets
- Analyzes source schema and data characteristics
- Selects an optimal partition column
- Computes balanced boundaries
- Executes parallel reads

### What this means

- **No partition configuration**: no manual column/range/parallelism specification
- **Adaptive throughput**: more partitions for larger tables; avoids overhead for small tables
- **Consistent performance**: applies appropriate strategy across very small to very large tables

### New connectors supporting auto-partitioning

- Amazon RDS for SQL Server
- Azure SQL Database
- Azure SQL Managed Instance
- Azure Synapse Analytics
- Fabric Data Warehouse
- Fabric SQL database
- SQL Server

## Learn more

- [Connector overview](https://learn.microsoft.com/fabric/data-factory/connector-overview)
- [Copy job connectors](https://learn.microsoft.com/fabric/data-factory/copy-job-connectors)
- Fabric roadmap: [Fabric roadmap](https://aka.ms/Fabric-Roadmap)
- Community links: [blogs](https://blog.fabric.microsoft.com/blog), [forums](https://aka.ms/fabric-community), [Ideas channels](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/outstanding-connectivity-for-data-movement-in-fabric-data-factory/)

