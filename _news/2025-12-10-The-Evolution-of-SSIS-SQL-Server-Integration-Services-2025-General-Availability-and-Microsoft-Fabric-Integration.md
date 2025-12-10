---
layout: "post"
title: "The Evolution of SSIS: SQL Server Integration Services 2025 General Availability and Microsoft Fabric Integration"
description: "This news article explores the release of SQL Server Integration Services (SSIS) 2025, focusing on its modernized security, integration with the SQL Server 2025 engine, and enhanced connectivity with Microsoft Fabric. Key topics include enterprise-grade security improvements, authentication via Microsoft Entra ID, integration workflows targeting Fabric Data Warehouse, migration paths for existing SSIS investments, support for Oracle databases, and advanced change data capture capabilities in Data Factory within Microsoft Fabric. The piece is highly technical and extends beyond simple product updates, providing actionable insight for practitioners managing data integration pipelines, hybrid analytics, and cloud migration."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/the-evolution-of-sql-server-integration-services-ssis-ssis-2025-generally-available/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-12-10 13:32:43 +00:00
permalink: "/2025-12-10-The-Evolution-of-SSIS-SQL-Server-Integration-Services-2025-General-Availability-and-Microsoft-Fabric-Integration.html"
categories: ["Azure", "ML", "Security"]
tags: ["Azure", "Azure Data Factory", "Change Data Capture", "Copy Job", "Data Factory", "Data Integration", "Data Migration", "ETL", "Fabric Data Warehouse", "Hybrid Cloud", "Lakehouse", "Microsoft Entra ID", "Microsoft Fabric", "ML", "News", "Oracle Connector", "Private Preview", "Security", "SQL Server", "SQL Server Integration Services", "SSIS", "T SQL COPY INTO"]
tags_normalized: ["azure", "azure data factory", "change data capture", "copy job", "data factory", "data integration", "data migration", "etl", "fabric data warehouse", "hybrid cloud", "lakehouse", "microsoft entra id", "microsoft fabric", "ml", "news", "oracle connector", "private preview", "security", "sql server", "sql server integration services", "ssis", "t sql copy into"]
---

Microsoft Fabric Blog details the major advancements and integration features of SSIS 2025 as it becomes generally available, authored by the official product team. The article covers data security, connections to Microsoft Fabric, and technical migration strategies for enterprise users.<!--excerpt_end-->

# The Evolution of SQL Server Integration Services (SSIS): SSIS 2025 (Generally Available)

SQL Server Integration Services (SSIS) has long enabled organizations to manage complex data integration, ETL, and transformation tasks. With the release of SSIS 2025, announced at Microsoft Ignite, the platform aligns with SQL Server 2025 and delivers modern capabilities for hybrid and cloud-first architectures.

## Major Features of SSIS 2025

1. **Enterprise Security**
   - SSIS 2025 integrates the modern **Microsoft.Data.SqlClient** for secure data access.
   - Advanced security protocols supported, including **TLS 1.3**.
   - Robust authentication using **Microsoft Entra ID** (formerly Azure Active Directory).
   - **Strict Encryption** with TDS 8.0 default protections for all SSIS network traffic, strengthening data security at rest and in transit.

2. **Integration with Microsoft Fabric**
   - SSIS packages can now be targeted at **Fabric Data Warehouse**, expanding enterprise analytics possibilities.
   - **Authentication**: SSIS must use Microsoft Entra ID credentials; SQL and Windows auth are not supported for Fabric.
   - **Data Ingestion**: Use the T-SQL `COPY INTO` command as the recommended loading method for Fabric Data Warehouse, superseding fast-insert/BCP scripts.
     - *See*: [What’s New in Integration Services in SQL Server 2025](https://learn.microsoft.com/sql/integration-services/what-s-new-in-integration-services-in-sql-server-2025?view=sql-server-2017)
     - *See*: [Tutorial: Integrating SSIS with Fabric Data Warehouse | Microsoft Learn](https://learn.microsoft.com/sql/integration-services/fabric-integration/integrate-fabric-data-warehouse?view=sql-server-ver17)

3. **SSIS Lift & Shift to Fabric Data Factory**
   - Seamless migration for existing SSIS packages from on-premises/Azure Data Factory into Fabric Data Factory without major ETL rework.
   - Reuse existing SSIS logic inside the Fabric ecosystem with consolidated data in OneLake and new features, including Copilot-assisted automation.
   - *Early access available*: [Sign Up Form](https://aka.ms/FabricSSIS)

![Screenshot of SSIS in Fabric Data Factory (Private Preview)](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/12/screenshot-of-ssis-in-fabric-data-factory-private-1.png)

## Modernization Insights: Data Integration in Microsoft Fabric

- **Oracle Integration**: The Microsoft Connector for Oracle and CDC components are now outside SSIS, requiring transition to modern data flows via Data Factory within Fabric.
- **Data Factory Connectivity**:
  - Connect to Oracle databases (on-premises or behind a VNet).
  - Configure copy activity and secure connectivity with the on-premises data gateway.
  - [Data Factory Oracle connector](https://learn.microsoft.com/fabric/data-factory/connector-oracle-database-copy-activity).
  - [Move data from Oracle to Lakehouse](https://learn.microsoft.com/fabric/data-factory/tutorial-move-data-from-oracle-to-lakehouse-pipeline).

## Advanced Change Data Capture (CDC)

- Native CDC in Fabric's Copy job allows efficient, automatic replication of inserts, updates, and deletes from sources (Oracle and more) to destinations like Fabric Lakehouse/Data Warehouse.
  - Zero manual intervention: Change tracking and replication are automated.
  - Optimized performance: Only changed data is processed for minimal latency.
  - Flexible source support: CDC-enabled and non-CDC sources, watermark-based incremental copy.
- *Private Preview available*: [Copy Job Sign Up Form](http://aka.ms/CopyJobCDCSignUp)

## Additional Resources

- [SQL Server Integration Services 2025 General Availability | Microsoft Community Hub](https://techcommunity.microsoft.com/blog/ssis/sql-server-integration-services-2025-general-availability/4471157)
- [Tutorial: Integrating SSIS with Fabric Data Warehouse | Microsoft Learn](https://learn.microsoft.com/sql/integration-services/fabric-integration/integrate-fabric-data-warehouse?view=sql-server-ver17)
- [Oracle database connector overview – Microsoft Fabric | Microsoft Learn](https://learn.microsoft.com/fabric/data-factory/connector-oracle-database-overview)
- [Move data from Oracle to Fabric Lakehouse via pipeline and on-premises data gateway](https://learn.microsoft.com/fabric/data-factory/tutorial-move-data-from-oracle-to-lakehouse-pipeline)
- [Expanded CDC Support for More Sources & Destinations](https://blog.fabric.microsoft.com/blog/simplifying-data-ingestion-with-copy-job-expanded-cdc-support-for-more-sources-destinations?ft=Data-factory:category)

---

**Summary**:
SSIS 2025 delivers data integration modernization by raising security standards, enabling seamless identity management, and offering robust migration/integration options with Microsoft Fabric. Data Factory enhancements, especially for Oracle connectivity and CDC, underscore SQL Server’s evolution into unified cloud analytics. This information is intended for architects, developers, and data engineers modernizing enterprise data infrastructure.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/the-evolution-of-sql-server-integration-services-ssis-ssis-2025-generally-available/)
