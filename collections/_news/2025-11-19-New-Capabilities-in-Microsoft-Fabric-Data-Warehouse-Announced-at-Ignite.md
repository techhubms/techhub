---
external_url: https://blog.fabric.microsoft.com/en-US/blog/fabric-data-warehouse-goes-all-in-on-enterprises-at-ignite/
title: New Capabilities in Microsoft Fabric Data Warehouse Announced at Ignite
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-11-19 11:00:00 +00:00
tags:
- Analytics
- BI
- Cloud Data Platform
- Cosmos DB
- Data Clustering
- Data Engineering
- Data Warehouse
- ETL
- IDENTITY Columns
- Microsoft Fabric
- Schema Management
- Snapshot Consistency
- SQL Analytics
- VARBINARY(MAX)
- VARCHAR(MAX)
- Warehouse Snapshots
- Azure
- ML
- News
- Machine Learning
section_names:
- azure
- ml
primary_section: ml
---
Authored by Microsoft Fabric Blog, with contributions from Peri Rocha, Ancy Philip, Jovan Popovic, and Twinkle Cyril, this article unveils the latest enhancements to Fabric Data Warehouse supporting advanced analytics and modern data warehousing.<!--excerpt_end-->

# Microsoft Fabric Data Warehouse: Enterprise-Ready Features Announced at Ignite

**Authors:** Microsoft Fabric Blog, Peri Rocha, Ancy Philip, Jovan Popovic, Twinkle Cyril

Microsoft Fabric Data Warehouse has evolved rapidly since general availability, introducing multiple performance and usability enhancements designed for enterprise analytics and modern data management.

## Key Enhancements

### 1. Data Clustering (Preview)

- New clustering feature boosts query performance and efficiency.
- Algorithm organizes similar data together to enable aggressive file pruning and faster queries.
- Optimizes data locality, outperforming legacy index techniques.
- [Documentation – Data Clustering in Fabric Data Warehouse](https://aka.ms/DataClusteringDocs)

### 2. IDENTITY Columns (Preview)

- Automatic surrogate key generation during ingestion.
- Eliminates manual key assignment and risk of duplication.
- Ensures uniqueness, even with parallel data jobs.
- [Documentation – IDENTITY Columns](https://aka.ms/identitydwdocs)

### 3. VARCHAR(MAX) and VARBINARY(MAX) Support

- Support for very large string and binary columns (up to 16MB per cell).
- Enables ingestion and analysis of logs, descriptive text, JSON, spatial data, and more.
- SQL endpoint now reads large objects from source systems without former 8KB truncation.
- Auto upgrade for existing tables as schemas change, preventing corruption (notably for Cosmos DB JSON artifacts).

### 4. Warehouse Snapshots (GA)

- Point-in-time, read-only views for consistency in reporting and ETL.
- Solves issues with “half-loaded” data disrupting reporting and dashboards.
- Enables reliable time-travel queries.
- [More on Warehouse Snapshots](https://aka.ms/WarehouseSnapshotsGABlog)

## Platform Overview

Fabric Warehouse integrates ETL, SQL, BI, AI, and operational apps into a unified analytics platform. New capabilities empower organizations to scale and innovate with performance, scalability, and cost-effectiveness.

## Getting Started & Resources

- [Try Fabric for free](https://aka.ms/tryfabric)
- [Shape the roadmap](https://aka.ms/fabricommunity)
- [Public roadmap](https://aka.ms/fabricroadmap)

## Summary

These enhancements position Microsoft Fabric Data Warehouse as a comprehensive solution for enterprise data warehousing, analytics engineering, and large-scale BI, addressing challenges like performance, consistency, and complex schema management.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/fabric-data-warehouse-goes-all-in-on-enterprises-at-ignite/)
