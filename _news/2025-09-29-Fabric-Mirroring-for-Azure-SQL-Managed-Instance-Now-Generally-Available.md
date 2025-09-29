---
layout: "post"
title: "Fabric Mirroring for Azure SQL Managed Instance Now Generally Available"
description: "This article introduces the general availability of Fabric Mirroring for Azure SQL Managed Instance, a feature that allows near real-time data replication from Azure SQL Managed Instance directly to Microsoft Fabric’s OneLake. It highlights the architecture, setup process, advanced analytics capabilities, seamless schema changes, integration with Power BI Direct Lake mode, and details on mirroring pricing and supported scenarios."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/announcing-the-general-availability-ga-of-mirroring-for-azure-sql-managed-instance-in-microsoft-fabric/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-09-29 11:00:00 +00:00
permalink: "/2025-09-29-Fabric-Mirroring-for-Azure-SQL-Managed-Instance-Now-Generally-Available.html"
categories: ["Azure", "ML"]
tags: ["Azure", "Azure SQL Managed Instance", "Data Engineering", "Data Platform", "Data Replication", "Data Science", "Data Warehouse", "ETL Free Data Integration", "Fabric Mirroring Pricing", "KQL Database", "Lakehouse", "Microsoft Fabric", "Mirroring", "ML", "News", "Notebooks", "OneLake", "Power BI Direct Lake", "Real Time Analytics", "Schema Changes"]
tags_normalized: ["azure", "azure sql managed instance", "data engineering", "data platform", "data replication", "data science", "data warehouse", "etl free data integration", "fabric mirroring pricing", "kql database", "lakehouse", "microsoft fabric", "mirroring", "ml", "news", "notebooks", "onelake", "power bi direct lake", "real time analytics", "schema changes"]
---

The Microsoft Fabric Blog team discusses the general availability of Fabric Mirroring for Azure SQL Managed Instance, guiding readers through setup, real-time analytics integration, and cost considerations.<!--excerpt_end-->

# Fabric Mirroring for Azure SQL Managed Instance Now Generally Available

Microsoft Fabric has announced the general availability of Mirroring for Azure SQL Managed Instance, delivering seamless near real-time data replication to Fabric’s OneLake without complex ETL processes.

## What is Mirroring in Fabric?

Mirroring in Fabric is a data integration feature that replicates data from sources like Azure SQL Managed Instance directly into OneLake in near real-time. This ensures your data is always up to date and accessible for analytical and data science workloads with minimal operational overhead. Unlike traditional change data capture (CDC), Mirroring in Fabric supports DDL changes such as adding and dropping columns on active mirrored tables.

## How Fabric Mirroring Works

- Continuous, near real-time ingestion of data from Azure SQL Managed Instance into OneLake.
- Data becomes analytics-ready and can be consumed via Power BI (including the new Direct Lake mode), Data Warehouse, Data Engineering, Lakehouse, KQL Database, and Notebooks.
- Supports seamless schema changes, drastically reducing administrative effort compared to ETL-based pipelines.

![Mirroring Architecture](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/09/Picture1.png)

## Setup, Monitoring, and Schema Adaptation

Setting up mirroring involves connecting your Azure SQL Managed Instance to Microsoft Fabric and configuring which databases to mirror. Once configured, you can:

- Monitor mirroring status and data freshness from the Fabric UI.
- Apply changes to schemas – DDL changes like adding or dropping columns can be made to mirrored tables without interruption.

![Setup Visualization](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/09/Picture2-1024x553.png)

![Animation of Mirroring Setup](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/09/sqlmisamigif.gif)

## Leveraging Your Mirrored Data

Once data from Azure SQL Managed Instance is mirrored into OneLake, it can be:

- Queried and cross-joined with other mirrored sources, warehouses, or lakehouses.
- Utilized across all Microsoft Fabric experiences, including advanced analytics tooling, machine learning via Notebooks, and building Power BI reports using Direct Lake mode and semantic models.

Workloads benefit from always-fresh data and integration with the entire Fabric analytics platform.

## Pricing

Mirroring is offered with free compute and free storage based on your Fabric capacity (e.g., an F64 capacity provides 64 free terabytes for mirroring). Storage charges only apply when usage exceeds your capacity's included storage. See Microsoft Fabric pricing documentation for more.

- [Microsoft Fabric – Pricing](https://azure.microsoft.com/pricing/details/microsoft-fabric/#overview)

## Summary and Supported Scenarios

Mirroring now supports Azure SQL Managed Instance (GA), and is expanding support for other sources like Azure SQL Database and upcoming SQL Server versions. The technology provides:

- Reduced total cost of ownership
- Zero ETL/code requirements
- Immediate access to operational data for analytics

For updates on new features and sources, follow the [Mirroring Roadmap](https://aka.ms/FabricRoadmap).

## Resources

- [Microsoft Fabric Mirrored Databases from Azure SQL Managed Instance](https://learn.microsoft.com/fabric/mirroring/azure-sql-managed-instance)
- [Microsoft Fabric Mirrored Databases from Azure SQL Database](https://learn.microsoft.com/fabric/mirroring/azure-sql-database)
- [Microsoft Fabric Mirrored Databases from SQL Server](https://learn.microsoft.com/fabric/mirroring/sql-server)
- [Get Started with a Fabric free trial](https://aka.ms/try-fabric)
- [Provide feedback on Mirroring via Fabric Ideas](https://aka.ms/fabricideas)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/announcing-the-general-availability-ga-of-mirroring-for-azure-sql-managed-instance-in-microsoft-fabric/)
