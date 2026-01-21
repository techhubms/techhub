---
external_url: https://blog.fabric.microsoft.com/en-US/blog/announcing-public-preview-onelake-as-a-source-for-copy-into-and-openrowset/
title: OneLake Integration with COPY INTO and OPENROWSET Now in Microsoft Fabric Preview
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-07-31 09:00:00 +00:00
tags:
- COPY INTO
- Cross Workspace Data Loads
- CSV
- Data Engineering
- Data Ingestion
- Data Warehouse
- Entra ID
- Lakehouse
- Microsoft Fabric
- OneLake
- OPENROWSET
- Parquet
- Private Link
- SaaS
- Service Principals
- SQL
- Workspace Permissions
section_names:
- ml
---
Microsoft Fabric Blog presents an overview of the new preview capability where OneLake can be used directly as a source for COPY INTO and OPENROWSET, written by the Microsoft Fabric team.<!--excerpt_end-->

# OneLake as a Source for COPY INTO and OPENROWSET (Preview)

**Author:** Microsoft Fabric Blog

The latest update to Microsoft Fabric Data Warehouse introduces a major simplification for data professionals: you can now use OneLake as a source for the COPY INTO and OPENROWSET commands without any external storage or elaborate IAM configuration.

## Key Updates

- **Direct File Access**: Load and query files directly from Lakehouse file folders—no more dependency on external staging storage or SAS tokens.
- **Workspace Permissions**: Operations depend on Fabric workspace permissions and Entra ID security, eliminating the need for external IAM or firewall rules.
- **Supported Files**: COPY INTO supports CSV and Parquet files; OPENROWSET enables ad hoc SQL-based queries.
- **No-Code Pipelines**: There’s no need for Spark or Data Factory pipelines to move or access data.

## New Scenarios Enabled

- Ingest data from a Lakehouse into a Data Warehouse using COPY INTO, supporting formats like CSV, Parquet, and JSON
- Perform cross-workspace loads and queries within a tenant
- Secure operations entirely within Private Link environments
- Automate data pipelines using service principals via Entra ID

## Example Commands

**COPY INTO usage**

```sql
COPY INTO dbo.Sales FROM 'https://onelake.dfs.fabric.microsoft.com/<workspace>/<lakehouse>/Files/Sales.csv'
WITH ( FILE_TYPE = 'CSV', FIRSTROW = 2, FIELDTERMINATOR = ',', ERRORFILE = 'https://onelake.dfs.fabric.microsoft.com/<workspace>/<lakehouse>/Files/Sales_Errors.csv' );
```

**OPENROWSET usage**

```sql
SELECT * FROM OPENROWSET('https://onelake.dfs.fabric.microsoft.com/<workspace>/<lakehouse>/Files/Sales.csv');
```

## Getting Started

All Microsoft Fabric users can try this feature by uploading files into a Lakehouse’s **Files** folder and using COPY INTO or OPENROWSET from any connected Data Warehouse.

## Next Steps

Upcoming enhancements will make it easier to reference Lakehouse and workspace resources using friendly names in scripts, improving readability and collaboration.

## Learn More

- [COPY INTO command](https://learn.microsoft.com/sql/t-sql/statements/copy-into-transact-sql?view=fabric)
- [Browse file content using OPENROWSET function](https://learn.microsoft.com/fabric/data-warehouse/browse-file-content-with-openrowset)
- [Ingest data into the Warehouse](https://learn.microsoft.com/fabric/data-warehouse/ingest-data)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/announcing-public-preview-onelake-as-a-source-for-copy-into-and-openrowset/)
