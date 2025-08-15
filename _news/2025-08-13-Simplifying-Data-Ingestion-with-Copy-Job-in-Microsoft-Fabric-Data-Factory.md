---
layout: "post"
title: "Simplifying Data Ingestion with Copy Job in Microsoft Fabric Data Factory"
description: "This article introduces enhancements to Copy Job in Microsoft Fabric Data Factory, detailing new features such as reset incremental copy, auto table creation, and JSON format support for data migration. It explains how these updates improve data movement across various sources and destinations, aiming to boost workflow efficiency for data engineers."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-reset-incremental-copy-auto-table-creation-and-json-format-support/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-08-13 09:00:00 +00:00
permalink: "/2025-08-13-Simplifying-Data-Ingestion-with-Copy-Job-in-Microsoft-Fabric-Data-Factory.html"
categories: ["Azure", "ML"]
tags: ["Auto Table Creation", "Azure", "Azure SQL", "Azure SQL Managed Instance", "Bulk Copy", "Copy Job", "Data Engineering", "Data Factory", "Data Ingestion", "Data Migration", "Data Movement", "ETL", "Incremental Copy", "JSON Support", "Lakehouse", "Microsoft Fabric", "ML", "News", "Snowflake", "SQL Server"]
tags_normalized: ["auto table creation", "azure", "azure sql", "azure sql managed instance", "bulk copy", "copy job", "data engineering", "data factory", "data ingestion", "data migration", "data movement", "etl", "incremental copy", "json support", "lakehouse", "microsoft fabric", "ml", "news", "snowflake", "sql server"]
---

The Microsoft Fabric Blog team shares new features for Copy Job in Fabric Data Factory, helping data engineers with efficient data movement and ingestion workflows.<!--excerpt_end-->

# Simplifying Data Ingestion with Copy Job in Microsoft Fabric Data Factory

Copy Job is becoming a preferred solution in Microsoft Fabric Data Factory for efficiently moving data between diverse environments—including across clouds, from on-premises sources, or between services. The tool now offers multiple delivery styles like bulk copy, incremental copy, and change data capture (CDC) replication, providing flexibility for a variety of enterprise data scenarios.

## New Enhancements in Copy Job

### Reset Incremental Copy

- Incremental copy boosts efficiency by transferring only new or updated data after the first full load.
- Now, users can reset incremental copy to perform a full copy on the next run, useful for resolving data discrepancies between source and destination.
- Resets can be performed per table, enabling more granular troubleshooting and minimizing disruption.

### Auto Table Creation on Destination

- Copy Job can now auto-create destination tables if they do not exist, automatically setting up the correct schema as part of the data movement process.
- Supported destination types include:
  - SQL Server
  - Azure SQL Database
  - Fabric Lakehouse table
  - Snowflake
  - Azure SQL Managed Instance
  - Fabric SQL database
- This feature streamlines setup and minimizes manual intervention.

### JSON Format Support

- High-throughput binary copying is available for any format.
- Copy Job now supports movement of JSON files alongside familiar formats like CSV and Parquet, widening its usability for modern data exchanges.

### Quick Access to Connection Details

- Instantly view service name and workspace name by hovering over source or destination connections in the Copy Job panel—saving time and improving workflow efficiency.

## Additional Resources

- [Microsoft Fabric Copy Job Documentation](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job)
- [Fabric Ideas Community](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/data%20factory%20%7C%20copy%20job)
- [Technical Documentation](https://aka.ms/FabricBlog/docs)

_Questions or feedback? Join the community and continue the conversation!_

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-reset-incremental-copy-auto-table-creation-and-json-format-support/)
