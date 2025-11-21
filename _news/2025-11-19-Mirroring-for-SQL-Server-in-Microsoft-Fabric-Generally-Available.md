---
layout: "post"
title: "Mirroring for SQL Server in Microsoft Fabric (Generally Available)"
description: "This article introduces and details the new Mirroring capabilities for SQL Server within Microsoft Fabric, covering all in-market SQL Server versions (2016-2025). It explains how Mirroring enables near real-time data ingestion from SQL Server to Fabric's OneLake, easing analytics, AI, and data science processes. The post highlights architectural diagrams, differences between SQL Server versions, and practical setup guidance for leveraging the feature with advanced analytics tools like Power BI, Lakehouse, and KQL Database."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/mirroring-for-sql-server-in-microsoft-fabric-generally-available/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-11-19 14:00:00 +00:00
permalink: "/2025-11-19-Mirroring-for-SQL-Server-in-Microsoft-Fabric-Generally-Available.html"
categories: ["Azure", "ML"]
tags: ["Analytics", "Arc Agent", "Azure", "Azure SQL", "Change Data Capture", "Change Feed", "Data Engineering", "Data Warehouse", "Delta Format", "Direct Lake Mode", "KQL Database", "Lakehouse", "Microsoft Fabric", "Mirroring", "ML", "News", "On Premises Data Gateway", "OneLake", "Power BI", "SQL Server", "VNet Data Gateway"]
tags_normalized: ["analytics", "arc agent", "azure", "azure sql", "change data capture", "change feed", "data engineering", "data warehouse", "delta format", "direct lake mode", "kql database", "lakehouse", "microsoft fabric", "mirroring", "ml", "news", "on premises data gateway", "onelake", "power bi", "sql server", "vnet data gateway"]
---

Microsoft Fabric Blog provides a technical overview of Mirroring for SQL Server in Microsoft Fabric, describing how near real-time data ingestion enables seamless analytics and business intelligence for enterprise workloads.<!--excerpt_end-->

# Mirroring for SQL Server in Microsoft Fabric (Generally Available)

In today’s AI-driven world, analytics platforms depend heavily on the quality and timeliness of data. Enterprises collect huge volumes of data across applications, databases, and data warehouses, but ingesting this data centrally for analytics and AI is complex and costly. Traditional databases use proprietary formats, making shortcuts impossible and requiring extract, transform, and load (ETL) operations that rarely yield real-time results.

## Introducing Mirroring in Microsoft Fabric

[Mirroring](https://learn.microsoft.com/fabric/database/mirrored-database/overview) now offers a modern method for continuous, near-real-time data access and ingestion from any database or data warehouse into OneLake within Microsoft Fabric. This ensures immediate access to data changes, eliminating stale insights and improving analytics responsiveness.

Mirroring is now Generally Available for all in-market SQL Server releases from 2016 to 2022, with support for SQL Server 2025. The feature simplifies the process: after configuration in the Fabric Portal (connecting your SQL Server databases and choosing what to mirror), an initial snapshot is created in Fabric’s OneLake and kept up to date in near-real-time, reflecting transactions, table changes, and data updates.

## Technical Capabilities

- ## Real-Time Analytics Foundation
  - Immediate synchronization of source SQL Server data into OneLake
  - No complex setup or ETL transformations required
  - User selects eligible tables or mirrors entire databases
- ## Version Support and Architecture Differences
  - **SQL Server 2016-2022:** Relies on [Change Data Capture (CDC)](https://learn.microsoft.com/sql/relational-databases/track-changes/about-change-data-capture-sql-server?view=sql-server-ver16) to snapshot and replicate changes. Requires [on-premises data gateway (OPDG)](https://learn.microsoft.com/power-bi/connect-data/service-gateway-onprem) or [VNet data gateway](https://learn.microsoft.com/data-integration/vnet/overview) for connectivity.
  - **SQL Server 2025:** Uses 'change feed' technology (same as Azure SQL mirroring). [Arc Agent](https://aka.ms/sqlservermirroring) is required for outbound authentication.
- ## Integration with Fabric Analytics and BI Tools
  - Mirrored data in delta format is instantly available for:
    - Power BI (Direct Lake mode)
    - Data Warehouse
    - Data Engineering workflows
    - Lakehouse
    - KQL Database
    - Notebooks
    - Copilots

## Advantages

- Simplifies data movement without complex or costly ETL operations
- Enables near real-time analytics
- Facilitates seamless integration for business intelligence and data science
- Works across hybrid environments (on-premises SQL Server and Azure SQL)

## Setup & Documentation

- Configure mirroring via the Fabric Portal, specifying database connection and table selection
- Detailed step-by-step instructions available in [Mirrored SQL Server documentation](https://aka.ms/sqlservermirroring)

## Table: Key Differences Between SQL Server Sources And Mirroring Methods

| Source               | Change Capture      | Arc Agent         | Data Gateway           |
|----------------------|--------------------|-------------------|------------------------|
| SQL Server 2016-2022 | CDC                | Not required      | Required for writing   |
| SQL Server 2025      | Change Feed        | Required          | Used for control plane |
| Azure SQL            | Change Feed        | System identity   | Required in private nw |

Once data is mirrored, it is immediately consumable across Fabric experiences for advanced analytics, reporting, and machine learning workloads.

## Resources

- [Download SQL Server 2025](https://aka.ms/getsqlserver2025)
- [Learn what's new in SQL Server 2025](http://aka.ms/sqlserver2025blog)
- [Sign up for a Fabric free trial](https://aka.ms/try-fabric)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/mirroring-for-sql-server-in-microsoft-fabric-generally-available/)
