---
layout: "post"
title: "Simplifying File Access in OPENROWSET: Data Sources and Relative Paths (Preview)"
description: "This guide explains new enhancements to the OPENROWSET function that enable data engineers and analysts to use data sources and relative paths for easier querying of files within Microsoft Fabric Lakehouse environments and Azure Data Lake Storage. It demonstrates how to leverage external data sources for cleaner SQL, improved maintainability, and simplified cross-environment data access."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/simplifying-file-access-in-openrowset-using-data-sources-and-relative-paths-preview/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-10-07 10:00:00 +00:00
permalink: "/news/2025-10-07-Simplifying-File-Access-in-OPENROWSET-Data-Sources-and-Relative-Paths-Preview.html"
categories: ["Azure", "ML"]
tags: ["ADLS", "Azure", "Azure Data Lake Storage", "Data Analytics", "Data Engineering", "Data Integration", "Data Platform", "External Data Source", "GUID Based URI", "Lakehouse", "Microsoft Fabric", "ML", "News", "OPENROWSET", "Relative Paths", "SQL", "SQL Endpoint", "T SQL"]
tags_normalized: ["adls", "azure", "azure data lake storage", "data analytics", "data engineering", "data integration", "data platform", "external data source", "guid based uri", "lakehouse", "microsoft fabric", "ml", "news", "openrowset", "relative paths", "sql", "sql endpoint", "t sql"]
---

Microsoft Fabric Blog details how new data source and relative path features in OPENROWSET simplify file queries in Lakehouse and ADLS, making data engineering and analytics tasks cleaner and more flexible.<!--excerpt_end-->

# Simplifying File Access in OPENROWSET: Data Sources and Relative Paths (Preview)

**Author: Microsoft Fabric Blog**

The latest improvements to the `OPENROWSET` function allow users to utilize external data sources and relative paths, dramatically improving the workflow for querying files within Lakehouse workspaces and Azure Data Lake Storage (ADLS).

## Accessing Lakehouse Files with Relative Paths

With `OPENROWSET`, instead of using complex absolute (GUID-based) file paths, you can now specify file paths relative to the Lakehouse’s root folder. This makes queries cleaner and more maintainable. For example, in a T-SQL notebook, you can query the *sales.csv* file from a nested folder using:

```sql
SELECT *
FROM OPENROWSET(
  BULK 'data/2025/09/sales.csv',
  FORMAT = 'CSV')
```

This approach avoids hardcoding lengthy paths and reduces errors.

## Making Cross-Lakehouse Access Easier

When accessing files across Lakehouses in OneLake, you previously had to use GUID-based URIs referencing both workspace and Lakehouse IDs in every query. Now, by defining an external data source with a root URI for your Lakehouse, you can reference it by name. Example setup:

```sql
CREATE EXTERNAL DATA SOURCE MyLakehouse
WITH (
  LOCATION = 'https://onelake.dfs.fabric.microsoft.com/{wsid}/{lhid}'
);
```

Replace `{wsid}` and `{lhid}` with your specific workspace and Lakehouse identifiers. Once set up, you can write queries like:

```sql
SELECT * FROM OPENROWSET(
  BULK 'data/2025/09/sales.csv',
  DATA_SOURCE = 'MyLakehouse',
  FORMAT = 'CSV')
```

## Referencing ADLS Locations

Define external data sources for remote ADLS accounts to further simplify file access:

```sql
CREATE EXTERNAL DATA SOURCE MyAdls
WITH (
  LOCATION = 'abfss://{mycontainer}@{mystorage}.dfs.core.windows.net'
);
```

Now you can reference ADLS files with relative paths in your queries:

```sql
SELECT * FROM OPENROWSET(
  BULK '/Files/data/2025/09/sales.csv',
  DATA_SOURCE = 'MyAdls',
  FORMAT = 'CSV')
```

## Benefits

- Cleaner, more maintainable SQL code
- Simplified cross-Lakehouse and multi-source data integration
- Eliminates repeated use of absolute URIs
- Streamlines data engineering and analytics workflows

> These features are in preview, and Microsoft plans to further improve the experience in the Fabric UI. Try them out and share feedback.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/simplifying-file-access-in-openrowset-using-data-sources-and-relative-paths-preview/)
