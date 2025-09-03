---
layout: "post"
title: "Accelerating Data Ingestion from SQL to Fabric with Fast Copy in Dataflow"
description: "This article from the Microsoft Fabric Blog explains how to leverage the Fast Copy feature in Dataflow Gen2 to significantly improve data ingestion speed and efficiency when importing large volumes of data from SQL databases into Fabric environments. It provides a practical walkthrough, from enabling Fast Copy to monitoring its usage, highlighting bulk loading, parallelism, and operational best practices for modern analytics workloads."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/accelerating-data-movement-by-using-fast-copy-to-unlock-performance-and-efficiency-during-data-ingestion-from-sql-database-in-fabric/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-09-03 09:00:00 +00:00
permalink: "/2025-09-03-Accelerating-Data-Ingestion-from-SQL-to-Fabric-with-Fast-Copy-in-Dataflow.html"
categories: ["Azure", "ML"]
tags: ["Analytics", "Azure", "Bulk Loading", "Data Ingestion", "Data Integration", "Data Movement", "Data Pipelines", "Dataflow Gen2", "ETL", "Fast Copy", "Microsoft Fabric", "ML", "News", "OneLake", "Parallel Processing", "Performance Optimization", "SQL Database"]
tags_normalized: ["analytics", "azure", "bulk loading", "data ingestion", "data integration", "data movement", "data pipelines", "dataflow gen2", "etl", "fast copy", "microsoft fabric", "ml", "news", "onelake", "parallel processing", "performance optimization", "sql database"]
---

Microsoft Fabric Blog outlines practical steps to use Fast Copy in Dataflow for faster data ingestion from SQL databases. The guide, authored by the Microsoft Fabric Blog team, covers feature activation, configuration, and performance checks, empowering organizations to streamline data integration workflows.<!--excerpt_end-->

# Accelerating Data Ingestion from SQL to Fabric with Fast Copy in Dataflow

The Fast Copy feature in Dataflow Gen2 offers a major performance boost for loading large datasets from SQL databases into Microsoft Fabric environments. This article demonstrates step-by-step how to enable and use Fast Copy, enabling organizations to accelerate ETL processes and unlock analytics at scale.

## Key Benefits of Fast Copy

- **Reduced data movement latency**: Parallel processing and bulk loading minimize total transfer time.
- **Optimized resource utilization**: Data is divided into manageable chunks, efficiently using both source and destination compute resources.
- **High-throughput, low-touch migrations**: Suitable for organizations pursuing operational excellence in analytics.

## Step-by-Step Guide

### 1. Create a New Data Flow

- Start by selecting a Dataflow Gen2 item in your Fabric workspace.

### 2. Configure Fast Copy

- Open the options menu in Dataflow.
- Under the **Scale** section, check the box for 'Allow use of fast copy connectors.'

### 3. Select Source and Destination

- Choose an SQL database as the source.
- Set OneLake as the data destination.

### 4. Publish and Run the Dataflow

- Save, publish, and execute your dataflow.

### 5. Automatic and Manual Triggering

- Fast Copy is automatically used when input data exceeds 100MB or 1 million rows.
- To force Fast Copy on smaller datasets, right-click your query and select ‘Require fast copy.’

### 6. Verifying Fast Copy Usage

- Navigate to **Refresh history** in Dataflow.
- Review run details: activities named 'CopyActivity' indicate Fast Copy was applied.
- If disabled, the engine will indicate no Fast Copy usage.

## Operational Notes

- **Fast Copy currently supports SQL database as a source only.**
- Enables organizations to keep up with modern analytics demands by optimizing the speed and efficiency of routine data integrations.

## Additional Resources

- [Read the official documentation for Fast Copy in Dataflow Gen2](https://learn.microsoft.com/fabric/data-factory/dataflows-gen2-fast-copy)

Organizations seeking to modernize their data flows and analytics processes can benefit directly from the outlined workflow for using Fast Copy during SQL-to-Fabric data integrations.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/accelerating-data-movement-by-using-fast-copy-to-unlock-performance-and-efficiency-during-data-ingestion-from-sql-database-in-fabric/)
