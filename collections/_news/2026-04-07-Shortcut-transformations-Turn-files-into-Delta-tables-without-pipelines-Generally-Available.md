---
section_names:
- azure
- ml
primary_section: ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/shortcut-transformations-and-turn-files-into-delta-tables-without-pipelines-generally-available/
date: 2026-04-07 10:00:00 +00:00
feed_name: Microsoft Fabric Blog
author: Microsoft Fabric Blog
tags:
- Amazon S3
- Azure
- Azure Blob Storage
- Azure Data Lake Storage
- CSV
- Data Ingestion
- Delta Lake
- Delta Tables
- ETL
- Google Cloud Storage
- Incremental Sync
- JSON
- Lakehouse
- Microsoft Fabric
- ML
- News
- OneLake
- Parquet
- Power BI
- Schema Evolution
- Schema Inference
- Semi Structured Data
- Shortcut Transformations
- Spark
- SQL
title: 'Shortcut transformations: Turn files into Delta tables without pipelines (Generally Available)'
---

Microsoft Fabric Blog announces the general availability of Shortcut transformations, a Fabric/OneLake feature that turns files (CSV, Parquet, JSON) referenced via shortcuts into continuously synchronized Delta tables—without building ETL pipelines or writing code.<!--excerpt_end-->

## Shortcut transformations: Turn files into Delta tables without pipelines (Generally Available)

Organizations often store data across different systems and file formats like CSV, Parquet, and JSON. Turning those files into analytics-ready tables usually means creating and maintaining ETL pipelines.

Shortcut transformations in Microsoft Fabric aim to remove that work by converting structured files referenced through OneLake shortcuts into Delta tables without building pipelines or writing code.

AI-powered transformations are also mentioned as available in public preview.

## Why this matters

Traditional data preparation for analytics typically involves:

- Building ingestion pipelines
- Managing compute jobs
- Orchestrating refresh schedules

Shortcut transformations change this by letting you:

- Reference data in-place using OneLake shortcuts
- Have Fabric handle ingestion, transformation, and synchronization
- Keep Delta tables continuously synchronized with the source data (incremental changes, no scheduled jobs required)

The claimed outcome is simpler architecture and lower operational overhead, with a faster path from raw files to analytics.

## Key capabilities

- **No pipelines or code required**
  - Converts files into analytics-ready Delta tables using a fully managed ingestion + sync experience.

- **Always in sync with source data**
  - Detects changes and applies them incrementally.

- **Support for nested folder structures**
  - Detects and processes files across hierarchical folders.

- **Automatic schema handling**
  - Infers schema and evolves tables as new columns appear.
  - Includes support for semi-structured data such as nested JSON.

- **Native Delta Lake output**
  - Outputs Delta tables that can be used across Fabric, including **SQL**, **Spark**, and **Power BI**.

- **Improved cost efficiency**
  - Avoids always-on pipelines and unnecessary compute.
  - Runs transformations when changes are detected.

## Getting started

### 1) Create a new table shortcut

In your **Lakehouse**:

- Select **New Shortcut** under the **Tables** section.
- For Lakehouses with schema, select **New table Shortcut** for a particular schema.

### 2) Connect to your data source

Supported sources mentioned include:

- Azure Data Lake Storage
- Azure Blob Storage
- Amazon S3
- Google Cloud Storage
- Dataverse
- SharePoint
- OneDrive

### 3) Select files and configure the transformation

In the Fabric UI, configure how data should be interpreted (no code required), for example:

- Delimiter (comma, semicolon, pipe, tab, etc.)
- Whether the first row contains headers
- Table name

![Screenshot of a data transformation step showing options to convert CSV data to Delta format. Key elements include delimiter selection set to comma, a toggle for using the first row as headers, connection credentials setup with a new connection option, and navigation buttons for previous and next steps.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/screenshot-of-a-data-transformation-step-showing.png)

*Figure: Auto-transform applied during shortcut creation, converting CSV data to Delta.*

### 4) Create the table

After creation, Fabric transforms the selected files into a Delta table in the Lakehouse **/Tables** folder.

## Resources

- Shortcut transformations documentation: https://learn.microsoft.com/fabric/onelake/shortcuts/transformations


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/shortcut-transformations-and-turn-files-into-delta-tables-without-pipelines-generally-available/)

