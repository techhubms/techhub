---
external_url: https://blog.fabric.microsoft.com/en-US/blog/announcing-staging-for-mirroring-for-google-bigquery-in-microsoft-fabricmirroring-for-gbq-staging-blog/
title: Announcing Staging for Mirroring for Google BigQuery (Preview)
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-12-04 08:00:00 +00:00
tags:
- Analytics
- Bulk Data
- Cloud Data
- Cross Cloud
- Data Engineering
- Data Ingestion
- Data Replication
- ETL
- Google BigQuery
- Microsoft Fabric
- Mirroring
- Performance Optimization
- Preview Feature
- Staging
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog introduces staging support for Mirroring Google BigQuery, explaining how this enhancement boosts replication speed and reliability for data engineers and analytics teams.<!--excerpt_end-->

# Announcing Staging for Mirroring for Google BigQuery (Preview)

The Microsoft Fabric team has released a significant update: **staging support for Mirroring Google BigQuery** datasets (in Preview). This enhancement dramatically accelerates initial data replication into Microsoft Fabric, optimizing analytics and cross-cloud data scenarios.

## Why Staging Matters

Historically, importing large datasets from BigQuery into Fabric could be slow. With staging enabled, organizations report over 90% performance improvements for the initial sync. For instance, 1.5 TB of data (over 6 billion rows) can now be replicated in 50 minutes instead of taking days.

![Staging Performance Illustration](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/11/staging-1024x587.png)

## How Staging Works

Staging introduces an intermediate data layer that:

- **Optimizes throughput** for large-scale, bulk data ingestion
- **Reduces latency** for first-time connections
- **Increases reliability** by reducing errors during large transfers

## Benefits for Analytics

- **Fast analytics access**: Teams can quickly analyze BigQuery data in Fabric
- **Streamlined workflows**: Reduces or eliminates the need for complex ETL pipelines
- **Reliable cross-cloud analytics**: Improves trust in data replication and integration

## Try It Out

Getting started is simple:

- [Mirroring Google BigQuery in Microsoft Fabric (Preview)](https://learn.microsoft.com/fabric/mirroring/google-bigquery)
- [Tutorial: Set Up Mirroring for Google BigQuery (Preview)](https://learn.microsoft.com/fabric/mirroring/google-bigquery-tutorial)

## Conclusion

This enhancement meets the growing need for fast, scalable analytics and robust cross-cloud data engineering, with native support for BigQuery-to-Fabric scenarios.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/announcing-staging-for-mirroring-for-google-bigquery-in-microsoft-fabricmirroring-for-gbq-staging-blog/)
