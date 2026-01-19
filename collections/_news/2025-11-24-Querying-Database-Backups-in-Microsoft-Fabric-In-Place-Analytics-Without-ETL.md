---
layout: post
title: 'Querying Database Backups in Microsoft Fabric: In-Place Analytics Without ETL'
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/30438/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-11-24 09:30:00 +00:00
permalink: /ai/news/Querying-Database-Backups-in-Microsoft-Fabric-In-Place-Analytics-Without-ETL
tags:
- Analytics
- Apache Iceberg
- Azure AI Foundry
- Cloud Data Lake
- Compliance
- Database Backup
- Delta Lake
- Eon
- ETL Elimination
- Fabric Data Warehouse
- Fabric Spark
- Governance
- Metadata Virtualization
- Microsoft Entra
- Microsoft Fabric
- Multi Cloud
- OneLake
- Power BI
section_names:
- ai
- azure
- ml
- security
---
Microsoft Fabric Blog details how, in collaboration with Peleg Kazaz and Liore Shai, organizations can leverage Eon's integration to query multi-cloud database backups directly in Microsoft Fabric—empowering analytics, AI, and compliance without ETL or duplication.<!--excerpt_end-->

# Querying Database Backups in Microsoft Fabric: In-Place Analytics Without ETL

**Authors:** Microsoft Fabric Blog (in collaboration with Peleg Kazaz, and Liore Shai)

## Overview

Modern enterprises rely on constant database backups for business operations, analytics, and AI. Historically, data warehouses and analytics platforms have required complex ETL pipelines and duplicated copies to use this backup data. This article introduces an integration between Eon and [Microsoft Fabric](https://www.microsoft.com/microsoft-fabric), allowing direct, governed access to multi-cloud database backups inside [Microsoft OneLake](https://learn.microsoft.com/fabric/onelake/onelake-overview).

## Key Capabilities & Architecture

- **Unified backups:** Integrate backups from Azure, AWS, and Google Cloud into OneLake, accessible via Fabric workloads.
- **No ETL required:** Query backup data in place, avoiding costly duplication and movement.
- **Activation of backup data:** Backups become live, queryable sources for analytics, BI, machine learning, and compliance.
- **Open formats:** Data stored as Apache Iceberg tables; metadata virtualization supports interoperability across engines like Fabric Data Warehouse and Spark.
- **Multi-cloud support:** Access backups from any major cloud, unified in OneLake's Catalog and queried via Table APIs or compatible engines.

## Technical Integration Steps

1. **Backups as Iceberg tables:** Eon manages backups in open formats, stored in Azure Blob Storage, Amazon S3, or Google Cloud Storage.
2. **Enable Fabric access:** Use Eon's console to enable Fabric integration (automatic or on-demand) for selected backups.
3. **Fabric shortcut registration:** Automatic creation of OneLake Shortcuts makes tables instantly discoverable and queryable in Fabric workloads.
4. **Security & access controls:** Authentication via Microsoft Entra service principals, fine-grained dataset-level permissions, and continuous shortcut validation.
5. **Governed analytics:** Administrators control access. Analysts query historical and operational datasets directly in Fabric Data Warehouse, Spark, and Power BI.

## Business Impact

- **Analytics & BI at scale:** Drive reporting and forecasting from historical backup data—all clouds, no restoration needed.
- **Machine Learning & AI:** Train models using full business datasets managed by Eon and integrated in Fabric.
- **Compliance and risk:** Conduct audits, retention checks, and governance directly on backup data.
- **Cyber resilience:** Analyze breach exposure and detect anomalies from backup data.
- **Cost optimization:** Monitor growth, automate tiering, and optimize usage based on real trends.
- **Monitoring and alerts:** Proactive rules across backup datasets enable rapid response via backup-driven signals.

## How to Get Started

- In Eon, go to Integrations → Microsoft Fabric.
- Follow connection wizard to set up automatic or on-demand access.
- Confirm dataset visibility in Fabric under /Tables.
- Connect Power BI or Fabric SQL to start analytics and BI queries.

## Security and Governance Considerations

- Read-only access enforced at the dataset level.
- Authentication leverages Microsoft Entra service principals and least-privilege design.
- Shortcuts validated to ensure storage remains properly governed.

## What’s Next for Fabric and Eon

Ongoing collaboration will expand interoperability, making backup data just as accessible as production data across all major clouds. The goal: more agility, less operational complexity, enhanced security and governance, and lower total cost of analytics.

## Learn More

- [Microsoft Fabric OneLake Documentation](https://learn.microsoft.com/fabric/onelake/onelake-overview)
- [Eon Platform Overview](https://www.eon.io/microsoft-azure)
- [Fabric Data Warehouse](https://learn.microsoft.com/fabric/data-warehouse/)

## About Eon

Eon is a multi-cloud backup platform providing Cloud Backup Posture Management (CBPM), automated compliance, and queryable data lakes for analytics and AI. Its direct integration with Microsoft Fabric and OneLake enables governed access, cost savings, and accelerated analytics from backup data stored across clouds.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/30438/)
