---
external_url: https://blog.fabric.microsoft.com/en-US/blog/openrowset-and-external-tables-for-fabric-sql-databases/
title: Data Virtualization and External Tables in Fabric SQL Databases (Preview)
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-11-24 10:00:00 +00:00
tags:
- ABFS
- Azure Data Lake Gen2
- Azure SQL Database
- BULK Operations
- Copilot
- CSV
- Data Virtualization
- External Data Sources
- External Tables
- Fabric SQL Database
- JSON
- JSON Functions
- Microsoft Fabric
- OneLake
- OPENROWSET
- Parquet
- Real Time Analytics
- Reverse ETL
- Schema Abstraction
- SQL Server
- T SQL
- AI
- Azure
- ML
- News
- Machine Learning
section_names:
- ai
- azure
- ml
primary_section: ai
---
Microsoft Fabric Blog details new Data Virtualization features for Fabric SQL Databases, enabling real-time analytics and schema abstraction using OPENROWSET, External Tables, and Copilot integration.<!--excerpt_end-->

# Data Virtualization and External Tables in Fabric SQL Databases (Preview)

Microsoft Fabric Blog announces Data Virtualization (Preview) for Fabric SQL Databases, empowering users to query, analyze, and ingest data directly from OneLake (CSV, Parquet, JSON) without the need for data movement or duplication.

## Key Features

- **Real-time Analytics:** Query external data sources instantly. No ingestion or data duplication required.
- **Structured & Semi-structured Data Support:** Use OPENROWSET and External Tables to handle CSV, Parquet, and JSON files. Advanced scenarios are made possible using JSON functions such as JSON_VALUE and OPENJSON.
- **Cross-platform Parity:** Capabilities align with Azure SQL Database, Azure SQL Managed Instance, and SQL Server, providing a consistent experience.
- **Secure Connectivity:** Connections to OneLake use EntraID for secure access with minimal configuration. Shortcuts allow seamless access to Azure Blob Storage, Azure Data Lake Gen2, S3-compatible storage, and SharePoint.
- **Reverse ETL Workflows:** Combine OPENROWSET and External Tables with BULK operations to support reverse ETL ingestion scenarios.
- **Schema Abstraction:** External Tables manage schema info in Fabric SQL Database, letting applications access external data as if it were a regular SQL table with minimal code changes.
- **Copilot Integration:** Generate quick insights in Fabric SQL Database using Copilot, without needing to load data first.

## Getting Started

Sample T-SQL for querying Parquet file:

```sql
SELECT TOP 100 * FROM OPENROWSET (
  BULK 'abfss://<workspaceID>@<tenant>.dfs.fabric.microsoft.com/<lakehouseid>/Files/contoso/store.parquet',
  FORMAT = 'parquet'
) AS STORE;
```

For external data sources:

```sql
CREATE EXTERNAL DATA SOURCE [Cold_Lake]
WITH ( LOCATION = 'abfss://<workspaceID>@<tenant>.dfs.fabric.microsoft.com/<lakehouseid>/Files/' );

-- Parse JSON data
SELECT * FROM OPENROWSET(
  BULK 'JSON/sample_user_profile.json',
  DATA_SOURCE = 'Cold_Lake',
  SINGLE_CLOB
) AS JSONData
CROSS APPLY OPENJSON(JSONData.BulkColumn);
```

To create and query external tables:

```sql
CREATE EXTERNAL FILE FORMAT Parquetff WITH (FORMAT_TYPE=PARQUET);

CREATE EXTERNAL TABLE [ext_product] (
  [ProductKey] [int] NULL,
  [ProductCode] [nvarchar](255) NULL,
  [ProductName] [nvarchar](500) NULL,
  [Manufacturer] [nvarchar](50) NULL,
  [Brand] [nvarchar](50) NULL,
  [Color] [nvarchar](20) NULL,
  [WeightUnit] [nvarchar](20) NULL,
  [Weight] DECIMAL(20, 5) NULL,
  [Cost] DECIMAL(20, 5) NULL,
  [Price] DECIMAL(20, 5) NULL,
  [CategoryKey] [int] NULL,
  [CategoryName] [nvarchar](30) NULL,
  [SubCategoryKey] [int] NULL,
  [SubCategoryName] [nvarchar](50) NULL
) WITH (
  LOCATION = '/product.parquet',
  DATA_SOURCE = [Cold_Lake],
  FILE_FORMAT = Parquetff
);

SELECT * FROM [ext_product];
```

### Practical Advantages

- Access and analyze data in OneLake and other external sources directly from SQL.
- Seamless integration with Microsoft data ecosystem (Fabric, Azure SQL, SQL Server).
- Unified workflows for real-time analytics, data ingestion, reporting, and schema management.
- Leverage Copilot to gain data insights on external datasets without moving information into SQL tables.

## References and Documentation

- [Data Virtualization (preview) documentation](https://aka.ms/fabricsqldv)
- [Fabric Blog: OPENROWSET and External Tables for Fabric SQL Databases](https://blog.fabric.microsoft.com/en-us/blog/openrowset-and-external-tables-for-fabric-sql-databases/)

---

Author: Microsoft Fabric Blog

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/openrowset-and-external-tables-for-fabric-sql-databases/)
