---
layout: post
title: 'Migration Assistant for Fabric Data Warehouse: General Availability Announced'
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/announcing-general-availability-of-migration-assistant-for-fabric-data-warehouse/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-10-06 09:00:00 +00:00
permalink: /ml/news/Migration-Assistant-for-Fabric-Data-Warehouse-General-Availability-Announced
tags:
- ACID Compliance
- Analytics Platform
- Azure
- Azure Synapse Analytics
- Cloud Migration
- Data Engineering
- Data Lake
- Data Migration
- Data Warehouse
- Delta Parquet
- Distributed Computing
- Microsoft Fabric
- Migration Assistant
- ML
- News
- Power BI Integration
- SaaS Analytics
- T SQL
section_names:
- azure
- ml
---
Microsoft Fabric Blog introduces the Migration Assistant for Fabric Data Warehouse, detailing features that ease transition from T-SQL analytics databases like Azure Synapse, authored by the Microsoft Fabric Blog team.<!--excerpt_end-->

# Migration Assistant for Fabric Data Warehouse (General Availability)

The Migration Assistant for Fabric Data Warehouse is now generally available, providing a streamlined approach for organizations migrating their analytical workloads to Microsoft Fabric’s modern, cloud-native data warehouse platform.

## Key Features

- **Broad Database Support:** Migrate from any T-SQL-based analytical database, including Azure Synapse Analytics dedicated SQL pool.
- **Comprehensive Migration:** Handles both metadata and data transfers, with schema conversion and connection re-routing.
- **Modern SQL Surface Compatibility:** Continuously updated to recognize evolving T-SQL patterns and provide robust translations for Fabric Data Warehouse.
- **Guided Experience:** Step-by-step guidance via an intuitive interface:
  - Upload DACPAC files
  - Select target workspace and warehouse
  - Receive real-time migration status and feedback
- **Migration Summary and Issues Panel:** Clear summaries, detailed results, and a “Fix Problems” panel (including Copilot resolutions) help quickly address any blockers.

## Why Move to Fabric Data Warehouse?

Migrating unlocks several benefits:

- **Lake-Native Architecture:** Utilizes the open Delta-Parquet format for seamless integration, interoperability, and ACID compliance.
- **Unified Analytics:** Integrates closely with Power BI and the wider Fabric ecosystem.
- **Autonomous Performance:** Distributed query engine requires no manual tuning.
- **Elastic Compute:** Instantly scale storage and compute independently.
- **Flexible Data Ingestion:** Use pipelines, dataflows, or T-SQL for data movement.
- **Cross-Database Analytics:** Query across multiple warehouses without duplication.
- **Cloud-Native Admin:** Simple governance, embedded security, and built-in monitoring.

Read more on [data warehousing in Microsoft Fabric](https://learn.microsoft.com/fabric/data-warehouse/data-warehousing).

## Getting Started

1. **Access Migration Assistant:**
   - Open a Fabric workspace
   - Use the “Migrate” button and select “Analytical T-SQL warehouse or database”
2. **Follow Step-by-Step Guidance:**
   - Detailed instructions are available in [the migration documentation](https://learn.microsoft.com/fabric/data-warehouse/migrate-with-migration-assistant)

## Need Support?

- Review [Fabric deployment patterns](https://learn.microsoft.com/azure/architecture/analytics/architecture/fabric-deployment-patterns) and the [full migration guide](https://learn.microsoft.com/fabric/fundamentals/migration)
- Contact your Microsoft account team for free personalized guidance and support via the Migration Factory

## Community Involvement

Share migration stories, challenges, and feedback on the [Ideas Portal](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas) to help shape future releases.

---

Ready to modernize your analytics with Microsoft Fabric? The Migration Assistant can help you transition quickly and unlock the advantages of cloud-native data warehousing.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/announcing-general-availability-of-migration-assistant-for-fabric-data-warehouse/)
