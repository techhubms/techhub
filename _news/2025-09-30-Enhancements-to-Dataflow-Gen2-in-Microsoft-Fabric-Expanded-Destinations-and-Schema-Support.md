---
layout: "post"
title: "Enhancements to Dataflow Gen2 in Microsoft Fabric: Expanded Destinations and Schema Support"
description: "This article details new enhancements to Dataflow Gen2 within Microsoft Fabric, including expanded support for data destinations, improved database schema management, and deeper integration with enterprise platforms like SharePoint, Snowflake, and ADLS Gen2. The update enables data professionals to streamline integration workflows, implement hybrid and collaborative architectures, and support Lakehouse-first data science scenarios."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/new-dataflow-gen2-data-destinations-and-experience-improvements/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-09-30 11:00:00 +00:00
permalink: "/2025-09-30-Enhancements-to-Dataflow-Gen2-in-Microsoft-Fabric-Expanded-Destinations-and-Schema-Support.html"
categories: ["Azure", "ML"]
tags: ["ADLS Gen2", "Azure", "Data Engineering", "Data Governance", "Data Integration", "Data Science", "Database Schema", "Dataflow Gen2", "Enterprise Data", "Hybrid Architecture", "Lakehouse", "Microsoft Fabric", "ML", "News", "Power BI", "SharePoint Integration", "Snowflake", "Spark"]
tags_normalized: ["adls gen2", "azure", "data engineering", "data governance", "data integration", "data science", "database schema", "dataflow gen2", "enterprise data", "hybrid architecture", "lakehouse", "microsoft fabric", "ml", "news", "power bi", "sharepoint integration", "snowflake", "spark"]
---

The Microsoft Fabric Blog outlines significant updates to Dataflow Gen2, led by the Microsoft Fabric team, introducing new data destinations and database schema support to empower data professionals and teams with flexible integration and advanced modeling features.<!--excerpt_end-->

# Enhancements to Dataflow Gen2 in Microsoft Fabric: Expanded Destinations and Schema Support

The latest set of enhancements to Dataflow Gen2 in Microsoft Fabric brings major advances in data integration and workflow collaboration. These updates aim to broaden connectivity, offer flexible new data destinations, enhance schema support, and improve integration with enterprise-grade platforms.

## Expanded Data Destinations

- **Lakehouse Files (Preview):** Directly write CSV files to Lakehouse for further Spark/Python processing. Enables data science workflows without relying on delta lake formats.
- **Snowflake (Sneak Peek):** Soon, organizations can operationalize data pipelines from Fabric to Snowflake, opening up cross-platform integration for data preparation and orchestration.
- **ADLS Gen2 (Preview):** Secure, scalable integration for structured and unstructured data management. Centralizes ingestion pipelines and supports Azure governance models.
- **SharePoint (Generally Available):** Seamless publication of structured data to collaborative environments. Makes low-code scenarios and automation workflows accessible for business users and teams.

## Database Schema Support

- Enhanced ability to define and manage database schemas for Lakehouse Tables, Fabric Warehouse, and Fabric SQL Databases.
- Enables:
  - Grouping related tables
  - Enforcing naming conventions
  - Applying governance and access control at the schema level
  - Modular design and better discoverability
  - Permission management by schema
- For setup, enable the ‘Navigate using full hierarchy’ option during connection configuration.

## Enabling New Data Scenarios

- **Lakehouse-First Data Science:** Feature sets can be landed directly into Lakehouse Files for Spark-based model training and rapid experimentation.
- **Hybrid Architectures:** ADLS Gen2 support allows phased migrations and hybrid analytics by maintaining compatibility with existing Azure data lakes.
- **Collaborative Reporting and Automation:** SharePoint integration empowers business process automation and reporting workflows in Microsoft 365, connecting data engineering with business operations.
- **Enterprise-Grade Modeling:** Schema support paves the way for structured semantic modeling, standardized data architectures, and schema-based access controls for Power BI.

## Summary

These improvements reflect Microsoft Fabric's commitment to versatility, enterprise-readiness, and empowering both technical and business users. Further details can be found in the [official documentation](https://learn.microsoft.com/fabric/data-factory/dataflow-gen2-data-destinations-and-managed-settings).

---

**Feedback and Support:**
Explore these features and provide your feedback to Microsoft. Assistance is available for deeper integration scenarios or specific workflow optimizations.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/new-dataflow-gen2-data-destinations-and-experience-improvements/)
