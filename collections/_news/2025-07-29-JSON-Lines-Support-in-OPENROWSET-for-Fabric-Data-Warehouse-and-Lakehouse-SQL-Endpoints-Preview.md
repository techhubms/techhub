---
external_url: https://blog.fabric.microsoft.com/en-US/blog/public-preview-json-lines-support-in-openrowset-for-fabric-data-warehouse-and-lakehouse-sql-endpoints/
title: JSON Lines Support in OPENROWSET for Fabric Data Warehouse and Lakehouse SQL Endpoints (Preview)
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-07-29 11:30:00 +00:00
tags:
- Data Analytics
- Data Lake
- Data Warehouse
- External Data
- JSON Lines
- JSONL
- Lakehouse
- Microsoft Fabric
- OPENROWSET
- Semi Structured Data
- SQL Endpoints
- T SQL
section_names:
- azure
- ml
---
Microsoft Fabric Blog introduces preview support for JSON Lines (JSONL) in OPENROWSET for Data Warehouse and Lakehouse SQL endpoints, simplifying access to semi-structured data.<!--excerpt_end-->

## JSON Lines Support in OPENROWSET for Fabric Data Warehouse and Lakehouse SQL Endpoints (Preview)

**Author:** Microsoft Fabric Blog

### Overview

Microsoft Fabric has announced the preview of JSON Lines (also known as JSONL) support in the `OPENROWSET(BULK)` function for both Fabric Data Warehouse and SQL endpoints for Lakehouses. This functionality lets users query external, semi-structured data in JSONL format directly from the data lake using standard T-SQL syntax.

### Key Features

- **JSON Lines Support:** The new update enables querying files in JSONL format, a widely used way of storing and exchanging semi-structured data. JSONL files consist of individual JSON objects, separated by newline characters, making them suitable for large-scale analytics and ingestion scenarios.
- **Expanded Data Format Coverage:** Prior to this update, `OPENROWSET(BULK)` supported several formats. The inclusion of JSONL broadens the spectrum of source data organizations can analyze within Microsoft Fabric.
- **Seamless T-SQL Integration:** Users familiar with SQL can now use the same `OPENROWSET(BULK)` syntax to load and query JSONL files as they do with CSV or Parquet, easing the barrier for working with non-relational data stored in the data lake.

### Benefits

- **Simplifies Data Ingestion:** Users can more easily ingest semi-structured data into analytics solutions without requiring format conversions.
- **Speeds up Analytics:** Directly query JSONL data where it resides, eliminating the need for intermediate ETL processes.
- **Broader Data Lake Integration:** Organizations can store data in their preferred format and still leverage the powerful querying capabilities of Fabric Data Warehouse and Lakehouse SQL endpoints.

### Use Case Example

A typical use case might involve large event logs or telemetry datasets stored as JSONL. Using the new preview feature, analysts can write T-SQL queries to extract, filter, and aggregate insights directly from JSONL event files.

### Getting Started

To learn more about this preview and access implementation details, visit [the official Microsoft Fabric announcement](https://blog.fabric.microsoft.com/en-us/blog/public-preview-json-lines-support-in-openrowset-for-fabric-data-warehouse-and-lakehouse-sql-endpoints/).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/public-preview-json-lines-support-in-openrowset-for-fabric-data-warehouse-and-lakehouse-sql-endpoints/)
