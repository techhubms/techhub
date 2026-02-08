---
external_url: https://blog.fabric.microsoft.com/en-US/blog/ingest-files-into-your-fabric-data-warehouse-using-the-openrowset-function/
title: Efficient Data Ingestion in Microsoft Fabric Data Warehouse with OPENROWSET
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-11-03 10:00:00 +00:00
tags:
- Auditing
- Azure Storage
- COPY INTO
- CSV
- Data Ingestion
- Data Warehouse
- ELT
- ETL
- INSERT Statement
- JSONL
- Metadata Extraction
- Microsoft Fabric
- OneLake
- OPENROWSET
- Parquet
- Partitioned Data
- Schema Inference
- T SQL
- Table Creation
- Transformation
- TSV
- Azure
- ML
- News
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog explains the use of OPENROWSET in Microsoft Fabric Data Warehouse for ingesting files with schema inference and transformation, offering guidance on building efficient ETL workflows.<!--excerpt_end-->

# Efficient Data Ingestion in Microsoft Fabric Data Warehouse with OPENROWSET

Data ingestion is a critical operation in building and maintaining Data Warehouse solutions. In Microsoft Fabric Data Warehouse, the **OPENROWSET** function allows you to read and extract data from files located in Fabric OneLake or external Azure Storage accounts. Supported formats include Parquet, CSV, TSV, and JSONL, offering flexibility for ingesting various data types into warehouse tables.

## Overview: OPENROWSET Functionality

- **Versatile Data Reader**: Retrieves file data as structured, tabular rows and columns by specifying the file’s URI.
- **Schema Inference**: Automatically detects column names and data types based on the source file.
- **Partitioned Data Reading**: Supports reading HIVE-partitioned folders and extracts partition values.
- **Metadata Extraction**: Enables retrieval of file names and paths for auditing and lineage.
- **Multiple Format Support**: Parquet, CSV, TSV, JSONL files are directly accessible.
- **Customizable Options**: Allows specification of row/field terminators, character encodings, header row inference/skip, and more.
- **Transformation Capability**: You can apply filters or lightweight data transformations during ingestion for ETL pipelines.

## Using OPENROWSET with INSERT for ETL

OPENROWSET excels when used alongside the INSERT statement to load transformed data. It is especially powerful for:

- Filtering or modifying external data before loading
- Schema inference and ad-hoc ingestion
- Reading partitioned or non-standard file structures

### Recommended Approach

- Use **COPY INTO** for bulk, 1:1 ingestion without transformation (common in ELT scenarios).
- Use **OPENROWSET** with INSERT for ETL workflows that require data transformation or filtering during loading.

## Step-by-Step Guide

### 1. Create the Destination Table

Establish a target table using CTAS (CREATE TABLE AS SELECT) with schema inference based on the file:

```sql
CREATE TABLE [dbo].[bing_covid-19_data] AS SELECT TOP 0 * FROM OPENROWSET(BULK 'https://pandemicdatalake.blob.core.windows.net/public/curated/covid-19/bing_covid-19_data/latest/bing_covid-19_data.parquet')
```

- **TOP 0** ensures an empty table for schema adjustment.
- CTAS infers columns and types from the sample source file.

### 2. Ingest and Transform Data

Insert data into the destination table, applying transformations as needed:

```sql
INSERT INTO [dbo].[bing_covid-19_data]
SELECT * FROM OPENROWSET(BULK 'https://pandemicdatalake.blob.core.windows.net/public/curated/covid-19/bing_covid-19_data/latest/bing_covid-19_data.parquet')
WHERE country_region <> 'Worldwide'
```

- Filters out rows where `country_region` is 'Worldwide'.
- Applies WHERE clause transformations during ingestion.

## OPENROWSET vs COPY INTO: Feature Comparison

| Feature                | OPENROWSET (with INSERT)         | COPY INTO                      |
|------------------------|----------------------------------|-------------------------------|
| Transformations        | Yes (in query execution)         | No (after load only)          |
| Schema Inference       | Source file-based                | Target table-based            |
| Best For               | ETL with filtering/adjustments   | Bulk ELT, high-volume loads   |

## Practical Guidance

- Use **OPENROWSET** for schema inference, filtering, or auditing needs.
- Use for partitioned data sets and when extracting metadata for lineage or auditing.
- COPY INTO is best for high-volume, production-grade data loads with robust error handling.
- Combine these techniques to optimize your data pipelines for both adaptability and performance.

## Further Reading

For more details and documentation, visit: [Ingest data with OPENROWSET function](https://learn.microsoft.com/fabric/data-warehouse/ingest-data-tsql#ingest-data-from-csvparquetjsonl-file).

---

By Microsoft Fabric Blog

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/ingest-files-into-your-fabric-data-warehouse-using-the-openrowset-function/)
