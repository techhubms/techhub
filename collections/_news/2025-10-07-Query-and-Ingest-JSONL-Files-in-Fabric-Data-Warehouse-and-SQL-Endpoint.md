---
layout: post
title: Query and Ingest JSONL Files in Fabric Data Warehouse and SQL Endpoint
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/query-and-ingest-jsonl-files-in-data-warehouse-and-sql-endpoint-for-lakehouse-general-availability/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-10-07 11:00:00 +00:00
permalink: /ml/news/Query-and-Ingest-JSONL-Files-in-Fabric-Data-Warehouse-and-SQL-Endpoint
tags:
- Azure
- CREATE TABLE as SELECT
- CTAS
- Data Engineering
- Data Ingestion
- Data Warehouse
- JSONL
- Lakehouse
- Microsoft Fabric
- ML
- News
- OPENROWSET
- Schema Mapping
- Semi Structured Data
- SQL Endpoint
- T SQL
section_names:
- azure
- ml
---
Microsoft Fabric Blog details how OPENROWSET in Fabric Data Warehouse and SQL Endpoint allows querying and ingesting JSONL files, streamlining data analysis and loading workflows.<!--excerpt_end-->

# Query and Ingest JSONL Files in Fabric Data Warehouse and SQL Endpoint

The Microsoft Fabric Blog introduces the general availability of JSONL (JSON Lines) file support in Fabric Data Warehouse and SQL Endpoint for Lakehouse environments. This enhancement utilizes the `OPENROWSET` T-SQL function, empowering users to read, query, and ingest JSONL files as table-like sources without manual transformations or parsing.

## Key Capabilities

- **OPENROWSET for JSONL**: Seamlessly query JSONL files—such as logs, social media streams, ML datasets, and configuration files—using familiar T-SQL syntax.
- **Row-wise Mapping**: Each JSON object in a JSONL file maps to a table row, with all properties exposed as distinct columns.
- **Schema Flexibility**: The `WITH` clause allows users to define schemas and extract nested properties, flattening even complex or deeply nested data structures.
- **Tool Integration**: Query JSONL data using Fabric Query editor, T-SQL Notebook, or SQL tools like SSMS.

## Example Usage

```sql
SELECT * FROM OPENROWSET(BULK '/Files/samples/jsonl/farmers-protest-tweets-2021-2-4.jsonl')
```

- Nested or complex JSON structures can be flattened and selectively extracted for analysis.

## Data Ingestion Workflows

- Use `CREATE TABLE AS SELECT (CTAS)` or `INSERT SELECT` statements to load JSONL data into Fabric Data Warehouse tables.
- Example:

```sql
INSERT INTO OpenRowsetDW.dbo.Tweets
SELECT * FROM OPENROWSET(BULK '/Files/jsonl/farmers-protest-2022-12-04.jsonl')
```

- Enables both initial data loads and ongoing automation for refreshing datasets with new semi-structured data.

## Conclusion

JSONL support through the OPENROWSET function extends the capability of Fabric Data Warehouse and SQL Endpoint for Lakehouse, facilitating enterprise-scale analytics on diverse semi-structured data. This update streamlines previously-complex pipelines—making it easier to explore, transform, and ingest data from varied sources using familiar SQL constructs.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/query-and-ingest-jsonl-files-in-data-warehouse-and-sql-endpoint-for-lakehouse-general-availability/)
