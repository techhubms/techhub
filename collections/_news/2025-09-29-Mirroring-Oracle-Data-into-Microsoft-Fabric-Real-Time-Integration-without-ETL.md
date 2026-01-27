---
external_url: https://blog.fabric.microsoft.com/en-US/blog/mirroring-for-oracle-in-microsoft-fabric-preview/
title: 'Mirroring Oracle Data into Microsoft Fabric: Real-Time Integration without ETL'
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-09-29 17:00:00 +00:00
tags:
- Cross Cloud Analytics
- Data Integration
- Data Lake
- Data Mirroring
- Data Modernization
- Data Warehouse
- ETL Replacement
- Exadata
- Google BigQuery
- Microsoft Fabric
- OneLake
- Oracle Cloud Infrastructure
- Oracle Database
- Power BI
- Predictive Analytics
- Real Time Data
- Replication
- Semantic Models
- Snowflake
- SQL Server
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog outlines how Mirroring for Oracle empowers enterprises to replicate and integrate Oracle data directly into OneLake for immediate analytics, reporting, and machine learning. This article by the Microsoft Fabric Blog emphasizes configuration details, supported scenarios, and business benefits.<!--excerpt_end-->

# Mirroring Oracle Data into Microsoft Fabric: Real-Time Integration without ETL

Microsoft Fabric has introduced Mirroring for Oracle—a preview feature enabling organizations to replicate Oracle data (from on-premises, OCI, or Exadata) directly into Fabric’s OneLake without complex ETL processes.

## Overview of Mirroring in Microsoft Fabric

- **Mirroring** is a continuous, low-latency data replication solution designed to move external data sources (such as Oracle, Google BigQuery, Snowflake, SQL Server) into OneLake, the unified data lake underpinning Microsoft Fabric.
- Unlike traditional ETL pipelines, mirroring requires no transformation or migration steps—keeping external data fresh and instantly queryable within Fabric Data Warehouse and other analytics workloads.

## Technical Capabilities

- **Supported Oracle Platforms**: On-premises Oracle (including VMs and Azure VMs), Oracle Cloud Infrastructure (OCI), Oracle Exadata.
- **Supported Oracle Versions**: 11 and above—with LogMiner enabled.
- **Configuration Requirements**: Database administrators must configure archived redo log files and supplemental logging for successful replication.
- **Exclusions**: Oracle Autonomous databases are not supported during preview.

## Key Features

- **Zero ETL**: Replicate Oracle tables directly into OneLake.
- **Cross-cloud data**: Integrate and query across Oracle, Google BigQuery, Snowflake, and SQL Server sources.
- **Unified semantic models**: Build analytics models spanning multiple data estates.
- **Immediate availability**: Oracle data is accessible for Fabric workloads including Power BI dashboards, AI/ML models, and reporting.
- **Performance and Scalability**: Mirroring handles billions of rows refreshed hourly for enterprise-scale analytics.

## Real-World Impact

> *CSX Transportation aims to leverage Mirroring for Oracle in Microsoft Fabric to stream real-time data from multiple on-premises Oracle systems, enabling unified, scalable data for predictive analytics and improved decision making.* – Chandra Bhowmick, CSX

## Limitations and More Information

- Database permissions and detailed setup steps are outlined in the official [Mirroring for Oracle Documentation](https://aka.ms/MirroringForOracleDocumentation).
- The preview is not yet compatible with Oracle Autonomous databases.
- Enhancements planned for user experience, monitoring, and overall scalability.

## Next Steps for Users

- Read [Mirroring for Oracle Overview](https://aka.ms/MirroringForOracleDocumentation) for understanding mirrored data architecture.
- Review prerequisites and setup guidance for Oracle mirroring.
- Begin replicating Oracle data into Fabric to modernize your analytics stack.

Mirroring for Oracle positions Microsoft Fabric as a central cross-cloud data platform, empowering enterprises to break down data silos, modernize strategy, and leverage real-time data for analytics and machine learning.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/mirroring-for-oracle-in-microsoft-fabric-preview/)
