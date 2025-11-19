---
layout: "post"
title: "Enhancements to Microsoft Fabric Data Factory Copy Job: Truncate Destination, Query-Based Subsets, and Multi-Folder Support"
description: "This article from the Microsoft Fabric Blog presents new features in Microsoft Fabric Data Factory's Copy job. It covers options to truncate destinations before full copies, copy data subsets using database queries, and handle multiple folders in a single job. The update aims to make data ingestion more flexible and efficient for data engineers and architects."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-truncate-destination-queries-and-multiple-folders/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-11-19 13:00:00 +00:00
permalink: "/2025-11-19-Enhancements-to-Microsoft-Fabric-Data-Factory-Copy-Job-Truncate-Destination-Query-Based-Subsets-and-Multi-Folder-Support.html"
categories: ["Azure", "ML"]
tags: ["Azure", "Azure SQL Database", "Bulk Copy", "Change Data Capture", "Cloud Data Integration", "Copy Job", "Data Engineering", "Data Factory", "Data Ingestion", "Data Movement", "Database Queries", "ETL", "Folder Copy", "Incremental Copy", "Microsoft Fabric", "ML", "News", "Truncate Destination"]
tags_normalized: ["azure", "azure sql database", "bulk copy", "change data capture", "cloud data integration", "copy job", "data engineering", "data factory", "data ingestion", "data movement", "database queries", "etl", "folder copy", "incremental copy", "microsoft fabric", "ml", "news", "truncate destination"]
---

Microsoft Fabric Blog details new Copy job capabilities in Fabric Data Factory, with features for truncating destinations, query-based data selection, and copying multiple folders in one operation.<!--excerpt_end-->

# Enhancements to Microsoft Fabric Data Factory Copy Job: Truncate Destination, Query-Based Subsets, and Multi-Folder Support

## Overview

Microsoft Fabric Data Factory's Copy job is a key feature for moving data across clouds, on-premises, or between services. It supports various delivery styles: bulk copy, incremental copy, and change data capture (CDC) replication, making data movement both flexible and streamlined.

## Key Updates

### Truncate Destination Before Full Copy

- **Feature**: Optionally truncate destination data before performing a full copy, ensuring full synchronization without duplicates.
- **How it Works**:
  - The first incremental copy with truncation enabled deletes existing destination data, then loads data.
  - Subsequent incremental copies append or merge records without removing existing ones.
  - Resetting to full copy after incrementals triggers another destination truncation if enabled.
- **Benefits**: Prevents data duplication, delivers clean syncs, and can improve performance for large loads.

### Full & Incremental Copy of Data Subsets With Database Queries

- **Feature**: Copy subsets of data for both full and incremental loads using custom SQL queries.
- **Use Cases**:
  - Load data only for a specific region (using a region column) to support data compliance.
  - Copy top N rows from a table (for testing or sampling).
- **Current Support**: Available now for Azure SQL Database, with more data connectors expected in future updates.
- **Advantage**: Offers more efficient and precise data ingestion based on custom selection logic.

### Copy Multiple Folders in One Copy Job

- **Feature**: Select and copy multiple folders or a mix of folders and files in a single job.
- **Advantage**: Reduces the need for multiple Copy jobs, making development and operation simpler for data engineers.

## Screenshots and Links

- [Microsoft Fabric Copy Job Documentation](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job)
- [Submit Feedback on Fabric Ideas](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/data%20factory%20%7C%20copy%20job)
- [Join the Fabric Community](https://community.fabric.microsoft.com/t5/Copy-job/bd-p/db_copyjob)

## Summary

These enhancements make Microsoft Fabric Data Factory's Copy job more robust for data ingestion, allowing for more customizable, efficient, and reliable workflows.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-truncate-destination-queries-and-multiple-folders/)
