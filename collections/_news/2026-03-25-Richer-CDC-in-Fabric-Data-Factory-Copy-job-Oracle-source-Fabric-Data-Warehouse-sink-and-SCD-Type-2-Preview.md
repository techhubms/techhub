---
feed_name: Microsoft Fabric Blog
author: Microsoft Fabric Blog
section_names:
- ml
primary_section: ml
title: 'Richer CDC in Fabric Data Factory Copy job: Oracle source, Fabric Data Warehouse sink, and SCD Type 2 (Preview)'
tags:
- Azure Data Factory
- Bulk Copy
- CDC Replication
- Change Data Capture
- Copy Job
- Data Movement
- Data Warehousing
- Dimensional Modeling
- Fabric Data Factory
- Fabric Data Warehouse
- Incremental Copy
- Mapping Data Flows
- MERGE INTO
- Microsoft Fabric
- ML
- News
- Oracle
- SCD Type 2
- Slowly Changing Dimension
- Soft Delete
- Stored Procedures
- T SQL
date: 2026-03-25 08:00:00 +00:00
external_url: https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-movement-across-multiple-clouds-with-richer-cdc-in-copy-job-in-fabric-data-factory-oracle-source-fabric-data-warehouse-sink-and-scd-type-2-preview/
---

Microsoft Fabric Blog introduces new CDC capabilities in Fabric Data Factory Copy job, including Oracle as a CDC source, Fabric Data Warehouse as a sink, and a one-toggle SCD Type 2 option (with soft deletes) aimed at simplifying history-tracking replication without custom code.<!--excerpt_end-->

## Overview

[Copy job](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job) in **Microsoft Fabric Data Factory** is positioned as a no-code way to move data across clouds using:

- Bulk copy
- Incremental copy
- Change data capture (CDC) replication

This update adds richer CDC replication scenarios aimed at reducing custom engineering for deletes and history handling.

## What’s new

### Oracle CDC source

Copy job now supports **Oracle as a CDC source**, capturing:

- Inserts
- Updates
- Deletes

…and continuously replicating those changes to supported destinations.

### Fabric Data Warehouse sink

CDC data can now be replicated directly into **Fabric Data Warehouse**, described as:

- Fully managed analytical SQL engine
- **T-SQL** query support
- Stored procedures
- Enterprise-grade security

### SCD Type 2 (Preview): history tracking + soft delete

Copy job adds **Slowly Changing Dimension Type 2 (SCD Type 2)** support, paired with built-in **soft delete** behavior:

- Updates create a new version of the row instead of overwriting history
- Deletes from the source are represented by marking the destination row inactive (closing out the current version), rather than physically removing it

Supported connectors listed for this capability:

- Azure SQL DB
- Azure SQL Managed Instance
- SQL Server
- Fabric SQL Database
- Fabric Lakehouse table

## Why SCD Type 2 matters

[SCD Type 2](https://en.wikipedia.org/wiki/Slowly_changing_dimension#Type_2:_add_new_row) preserves full change history by creating multiple versions of a dimension record.

Example (customer changes state):

| **CustomerKey** | **CustomerID** | **Name** | **State** | **Valid_From** | **Valid_To** | **Is_Current** |
| --- | --- | --- | --- | --- | --- | --- |
| 1001 | C-123 | Acme Corp | CA | 2023-01-15 | 2026-02-20 | No |
| 1002 | C-123 | Acme Corp | NY | 2026-02-20 | 9999-12-31 | Yes |

Soft delete, as described here, closes out the current version on delete events by setting `ValidTo` to the deletion timestamp and switching `IsCurrent` to `false`, keeping the row available for auditing and point-in-time analysis.

## Why SCD Type 2 is usually hard (code-based approach)

The article outlines typical steps required when implementing SCD Type 2 via custom logic:

1. Detect changed source rows by comparing CDC input to the current destination version
2. Close the current version (`ValidTo`, `IsCurrent = false`)
3. Insert a new version (`ValidFrom`, high-date `ValidTo`, `IsCurrent = true`)
4. Handle deletes via soft delete logic (close-out instead of physical delete)

## How to enable SCD Type 2 in Copy job

The described workflow is a configuration toggle rather than a custom pipeline:

1. Create a Copy job
2. Select a CDC-supported source
3. Select a supported destination
4. Turn on **SCD Type 2** (write method)

![Screenshot of enabling SCD2 in Copy job](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-enabling-scd2-in-copy-job.png)

## Comparison: code-based platforms vs Copy job (as presented)

The post contrasts effort and maintenance:

- Setup time: days/weeks (code) vs minutes (toggle)
- Per-table MERGE and SCD logic: hand-written vs generated
- Scaling to many tables: custom framework vs bulk selection
- Maintenance: ongoing code/framework upkeep vs “zero code to maintain”

## Coming from Azure Data Factory (ADF)

The article notes that **Azure Data Factory** doesn’t have built-in one-step SCD Type 2 support. It references ADF’s approach using Mapping Data Flows with transformations such as:

- Conditional split
- Derived column
- Alter row
- Sink transformation configured for upsert

It positions Fabric Copy job’s SCD Type 2 toggle as a simpler alternative and links to an upgrade path:

- [ADF to Fabric migration guide](https://learn.microsoft.com/fabric/data-factory/upgrade-paths)

## Learn more

- [What is Copy job](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job)
- [Change data capture (CDC) in Copy job](https://learn.microsoft.com/fabric/data-factory/cdc-copy-job)
- Feedback: [Fabric Ideas](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/data%20factory%20%7C%20copy%20job)
- Discussion: [Fabric Community – Copy job](https://community.fabric.microsoft.com/t5/Copy-job/bd-p/db_copyjob)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-movement-across-multiple-clouds-with-richer-cdc-in-copy-job-in-fabric-data-factory-oracle-source-fabric-data-warehouse-sink-and-scd-type-2-preview/)

