---
layout: "post"
title: "OneLake Integration with COPY INTO and OPENROWSET Now in Microsoft Fabric Preview"
description: "This news update from the Microsoft Fabric Blog introduces the preview release of integrating OneLake as a direct data source for the COPY INTO and OPENROWSET commands within Microsoft Fabric Data Warehouse. Users can now ingest and query files stored in Lakehouse folders without relying on external staging, storage accounts, or additional IAM configurations. The update streamlines data movement and access, leveraging Fabric’s built-in workspace governance and Entra ID–based security, and paves the way for further improvements in usability and collaboration. Detailed examples and links to official documentation are included for immediate adoption."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/announcing-public-preview-onelake-as-a-source-for-copy-into-and-openrowset/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-07-31 09:00:00 +00:00
permalink: "/2025-07-31-OneLake-Integration-with-COPY-INTO-and-OPENROWSET-Now-in-Microsoft-Fabric-Preview.html"
categories: ["ML"]
tags: ["COPY INTO", "Cross Workspace Data Loads", "CSV", "Data Engineering", "Data Ingestion", "Data Warehouse", "Entra ID", "Lakehouse", "Microsoft Fabric", "ML", "News", "OneLake", "OPENROWSET", "Parquet", "Private Link", "SaaS", "Service Principals", "SQL", "Workspace Permissions"]
tags_normalized: ["copy into", "cross workspace data loads", "csv", "data engineering", "data ingestion", "data warehouse", "entra id", "lakehouse", "microsoft fabric", "ml", "news", "onelake", "openrowset", "parquet", "private link", "saas", "service principals", "sql", "workspace permissions"]
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
