---
external_url: https://blog.fabric.microsoft.com/en-US/blog/announcing-public-preview-mirroring-for-google-bigquery-in-microsoft-fabric/
title: 'Mirroring for Google BigQuery in Microsoft Fabric: Public Preview Overview'
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-09-24 15:00:00 +00:00
tags:
- Analytics
- Change Data Capture
- Cross Cloud
- Data Governance
- Data Integration
- Data Replication
- Data Synchronization
- ETL
- Google BigQuery
- Microsoft Fabric
- Mirroring
- OneLake
- OPDG
- Power BI
- Real Time Data
- Zero ETL
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog explains their new Mirroring for Google BigQuery feature, enabling near real-time, secure data replication into OneLake for direct analytics, governance, and integration—key value for cross-cloud environments.<!--excerpt_end-->

# Mirroring for Google BigQuery in Microsoft Fabric (Preview)

Microsoft Fabric has introduced a new Mirroring feature in public preview, advancing its zero-ETL (Extract, Transform, Load) strategy for data movement. This feature allows organizations to securely and efficiently replicate data from Google BigQuery into Microsoft Fabric's OneLake with near real-time freshness, reducing complexity and manual pipeline development.

## What is Mirroring in Microsoft Fabric?

Mirroring enables direct replication of operational and warehouse data sources—including BigQuery—into Fabric's OneLake. This process does not require complex ETL pipelines, ensuring data remains up to date for analytics, BI, and AI workloads. Key benefits include:

- Seamless, cross-cloud data movement
- Immediate data availability in analytics tools (e.g., Power BI)
- Support for both operational and warehouse data sources

## Mirroring for Google BigQuery: How It Works

- **Unified Analytics:** Replicate BigQuery tables into OneLake for unified analytics alongside other sources.
- **Direct Query:** Use Power BI, notebooks, or pipelines to query mirrored data without extra transfers.
- **Continuous Sync:** Employs BigQuery's change data capture (CDC), ensuring fresh, near real-time data.
- **Selective Mirroring:** Configure which tables and the frequency of replication.
- **Insert-Only Support:** For tables without primary keys, supports insert-only replication with a reseed mechanism.
- **Security:** OPDG support enables secure, private networking, ensuring compliance with enterprise security policies.
- **Cost and Performance:** Tracks only row-level changes, reducing cost and latency, and scales across varied data environments.

## Why Does This Matter?

- **Cross-Cloud Simplicity:** Organizations using multi-cloud can unify analytics with minimal architecture changes.
- **Operational Efficiency:** Eliminates ETL overhead and simplifies governance within Fabric’s security model.
- **Near Real-Time Insights:** Supports modern analytics and reporting needs for up-to-date business intelligence.
- **Governance and Compliance:** Leverages Fabric’s workspace and security model for streamlined control.

## Looking Forward

The Mirroring for Google BigQuery feature will evolve based on user feedback as Microsoft works toward general availability. Upcoming improvements are focused on user experience, performance monitoring, and scalability.

For more details, see the [official documentation](https://aka.ms/PublicPreviewMirroringforGBQ).

---

*Source: Microsoft Fabric Blog*

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/announcing-public-preview-mirroring-for-google-bigquery-in-microsoft-fabric/)
