---
feed_name: Microsoft Fabric Blog
author: Microsoft Fabric Blog
date: 2026-03-19 10:00:00 +00:00
external_url: https://blog.fabric.microsoft.com/en-US/blog/audit-columns-in-copy-job-in-fabric-data-factory-every-row-is-traceable-for-data-lineage-and-compliance/
title: Audit columns in Copy job in Fabric Data Factory—Every row is traceable for data lineage and compliance
section_names:
- ml
tags:
- Audit Columns
- Bulk Copy
- Change Data Capture
- Compliance
- Connectors
- Copy Job
- Data Engineering
- Data Freshness
- Data Lineage
- Data Movement
- Fabric Data Factory
- Incremental Copy
- Ingestion SLA Monitoring
- Job Run ID
- KQL
- Microsoft Fabric
- ML
- News
- Power BI
- Regulatory Reporting
- Row Level Lineage
- Workspace ID
primary_section: ml
---

Microsoft Fabric Blog explains how to enable audit columns in Microsoft Fabric Data Factory Copy job, adding row-level metadata (extraction time, file path, workspace/job/run IDs, incremental bounds, and custom values) to improve lineage, compliance reporting, and data quality debugging.<!--excerpt_end-->

## Overview

[Copy job](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job) in **Microsoft Fabric Data Factory** is used for data movement across multiple clouds, including:

- Bulk copy
- Incremental copy
- Change data capture (CDC) replication

A common operational/compliance question after loading data is:

- Where did this row come from?
- When did it get here?

To avoid custom engineering for this, **audit columns** can now be enabled in Copy job to automatically append data-movement metadata to *every row* written to the destination.

## What are audit columns?

Audit columns are platform-generated metadata columns added by Copy job to describe the copy operation rather than the source data.

### Audit column list

| Audit column | What it captures |
| --- | --- |
| Data extraction time | Timestamp when the row was extracted from the source by a Copy job run |
| File path | Source file path the row was read from (file-based sources only) |
| Workspace ID | Fabric workspace ID where the Copy job resides |
| Copy job ID | Unique identifier of the Copy job item |
| Copy job run ID | Unique identifier of a specific Copy job execution |
| Copy job name | Name of the Copy job that moved the row |
| Lower bound | Lower bound value of the incremental window for the current run |
| Upper bound | Upper bound value of the incremental window for the current run |
| Custom | User-defined static value (for example: source server name) |

With these enabled, you can answer (per row):

- **When was this data extracted?** Exact extraction timestamp.
- **Where did it come from?** File path / source store context.
- **Which job moved it?** Workspace + job + specific run (IDs and names).
- **What was the incremental scope?** Lower/upper bound slice for the run.

The key point is that this is **automatic** (no custom code, no expressions) and you can add as many audit columns as needed.

### Supported connectors

Audit columns are supported on all Copy job connectors **except**:

- Snowflake
- Office 365
- Databricks Delta Lake

## Why audit columns matter

### Row-level data lineage

Traditional lineage often works at a job level (Job X wrote to Table Y). Audit columns embed provenance into the dataset so you can determine:

- which run produced a specific row
- when that row was written
- whether it came from this run or a previous one

### Compliance and regulatory reporting

For regulated industries (financial services, healthcare, insurance, government), audit columns help answer auditor questions directly from the destination table, such as:

- When was this customer record last updated in analytics?
- Can you prove a record came from a specific production system?
- Which job/run brought a record into the warehouse, and when?

Without this, teams often need to correlate monitoring logs with table contents.

### Data quality and debugging

When investigating duplicates, stale data, or missing records, audit columns let you see arrival time and origin per row without cross-referencing external logs.

### Downstream analytics and freshness tracking

Audit columns can support analytics use cases such as:

- **Source file traceability** via *File path*
- **Ingestion SLA monitoring** by comparing *Data extraction time* against schedules
- **Incremental window auditing** using *Lower bound* and *Upper bound* to confirm slices were processed

## Quick guide: enabling audit columns

### Step 1: Create or open a Copy job

In your Fabric workspace, create a new Copy job or open an existing one and select source tables as usual.

### Step 2: Add audit columns

In the Copy job setup (after selecting source tables/folders), enable **audit columns** to append metadata columns to every destination table in the job.

![Configure audit columns](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/configure-audit-columns.png)

### Step 3: Run the Copy job

Run the job. Each execution writes rows with audit metadata such as extraction time, workspace ID, job/run IDs, and any custom values.

### Step 4: Query data and build reports

Because audit columns are standard table fields, you can query them alongside business data and use them in tools including:

- Power BI
- KQL queries

This supports dashboards for freshness, ingestion SLA monitoring, and compliance lineage reporting without relying on external metadata stores.

![Show audit columns in destination table](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/show-audit-columns.png)

## Learn more

- [What is Copy job in Data Factory for Microsoft Fabric](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job)
- [How to create a Copy job](https://learn.microsoft.com/fabric/data-factory/create-copy-job)
- [Connector overview for Data Factory in Fabric](https://learn.microsoft.com/fabric/data-factory/connector-overview)
- [ADF: Add additional columns during copy](https://learn.microsoft.com/azure/data-factory/copy-activity-overview#add-additional-columns-during-copy)

Community links:

- Fabric Ideas: Submit feedback on Fabric Ideas
- Fabric Community: Join the conversation in the Fabric Community


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/audit-columns-in-copy-job-in-fabric-data-factory-every-row-is-traceable-for-data-lineage-and-compliance/)

