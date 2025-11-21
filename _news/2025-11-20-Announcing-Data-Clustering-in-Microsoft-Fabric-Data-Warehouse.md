---
layout: "post"
title: "Announcing Data Clustering in Microsoft Fabric Data Warehouse"
description: "This article introduces Data Clustering in Microsoft Fabric Data Warehouse—a new storage-level feature that drastically improves query speed and resource efficiency for large-scale data analytics. It explains how clustering works, demonstrates performance gains and provides SQL examples and real-world metrics comparing clustered versus standard tables. Readers learn about clustering syntax, how it impacts query scanning and resource use, and how to use it for optimized big data solutions in Fabric."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/announcing-data-clustering-in-fabric-data-warehouse-preview/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-11-20 14:00:00 +00:00
permalink: "/2025-11-20-Announcing-Data-Clustering-in-Microsoft-Fabric-Data-Warehouse.html"
categories: ["ML"]
tags: ["Analytics", "Big Data", "CLUSTER BY", "Data Clustering", "Data Engineering", "Data Storage", "Data Warehouse", "Exec Requests History", "Microsoft Fabric", "ML", "News", "Performance Tuning", "Preview Feature", "Query Insights", "Query Optimization", "Resource Optimization", "SQL"]
tags_normalized: ["analytics", "big data", "cluster by", "data clustering", "data engineering", "data storage", "data warehouse", "exec requests history", "microsoft fabric", "ml", "news", "performance tuning", "preview feature", "query insights", "query optimization", "resource optimization", "sql"]
---

Microsoft Fabric Blog explains the newly released Data Clustering feature for Fabric Data Warehouse, offering dramatic improvements in query speed and efficiency for analytics workloads.<!--excerpt_end-->

# Announcing Data Clustering in Microsoft Fabric Data Warehouse

Data Clustering in Microsoft Fabric Data Warehouse is a feature designed to significantly improve the performance and efficiency of analytic queries by physically organizing similar records together in storage. This blog post introduces the concept, details its impact on large data sets, and demonstrates real query metrics to underscore its benefits.

## What is Data Clustering?

Data Clustering organizes your data by grouping related rows based on specified columns. This means queries can scan and retrieve only the relevant data, reducing both time and resource usage. The engine automatically maintains clustering tables during ingestion and querying—developers, data engineers, and analysts benefit from improved throughput without additional manual intervention.

## How Data Clustering Improves Query Efficiency

- **Minimizes cold storage access:** Data clustering prunes irrelevant data early, so queries scan less information.
- **Optimizes resource usage:** By skipping non-matching files and row groups, clustered queries consume less compute power and memory.
- **Accelerates query performance:** Highly selective queries that align with clustering columns achieve measured improvements.

### Example: Query Performance Gains

A benchmark compares queries performed on standard versus clustered tables—both having identical data (approx. 60 billion rows, 1TB total):

- Elapsed Time: Clustered query = 1,909 ms, Regular = 45,742 ms (24x faster)
- Allocated CPU Time: Clustered = 39,278 ms, Regular = 2,551,199 ms (65x less CPU)
- Total Data Scanned: Clustered = 3,840 MB, Regular = 106,794 MB (only 3.7% of data scanned)

![Bar chart comparing query performance: clustered vs regular tables](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/11/image-49.png)

## Using Data Clustering Syntax

Data clustering adds the `CLUSTER BY` option to the `CREATE TABLE` statement:

```sql
CREATE TABLE Bands (
  ID BIGINT,
  Name VARCHAR(MAX),
  Genre VARCHAR(75),
  Country VARCHAR(56),
  DateFounded DATE
) WITH (CLUSTER BY (ID, DateFounded))
```

Up to four columns can be used, and Fabric's underlying algorithm efficiently groups rows considering all clustering columns (not just basic sequential sorting).

## Measuring Query Improvements

Leverage the `exec_requests_history` view in Query Insights to compare clustered versus regular table queries:

```sql
SELECT label, row_count, total_elapsed_time_ms, allocated_cpu_time_ms,
  data_scanned_disk_mb + data_scanned_memory_mb + data_scanned_remote_storage_mb AS total_data_scanned_mb
FROM queryinsights.exec_requests_history
WHERE label IN ('Clustered','Regular')
```

This lets teams quantify resource savings and validate clustering effectiveness for specific workloads.

## How Does It Work?

- Storage layout in clustered tables aligns with query predicates, enabling the Fabric engine to skip irrelevant files.
- Highly selective queries benefit most, but all queries that filter on clustering columns see improvement.

## What’s Next for Data Clustering?

Fabric Data Warehouse will keep improving in automated data layout. Clustering is in Preview—feedback is encouraged to help refine this feature. Explore [documentation](https://aka.ms/DataClusteringDocs) and [tutorials](https://aka.ms/DataClusteringDocs) for setup and tips.

## Resources

- [Microsoft Fabric Data Warehouse Documentation](https://aka.ms/DataClusteringDocs)
- [Hands-on Tutorial](https://aka.ms/DataClusteringDocs)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/announcing-data-clustering-in-fabric-data-warehouse-preview/)
