---
layout: "post"
title: "Microsoft and Snowflake: Simplified Interoperability with No Data Movement"
description: "This official news update from Microsoft Fabric Blog details the latest advancements in Microsoft and Snowflake interoperability, focusing on seamless analytics and AI workloads via Microsoft OneLake. The collaboration prioritizes data access, integration, and management through open standards like Apache Iceberg and Parquet, eliminating the need for data duplication and complex setup. New intuitive UI features in both platforms further streamline bidirectional data sharing, enabling unified, cross-platform experiences for enterprise data projects."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/microsoft-and-snowflake-simplified-interoperability-with-no-data-movement/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-11-18 08:00:00 +00:00
permalink: "/2025-11-18-Microsoft-and-Snowflake-Simplified-Interoperability-with-No-Data-Movement.html"
categories: ["AI", "Azure", "ML"]
tags: ["AI", "AI Workloads", "Analytics", "Apache Iceberg", "Azure", "Cross Platform Integration", "Data Architecture", "Data Interoperability", "Data Lake", "Data Sharing", "Delta Lake", "Enterprise Data Management", "Fabric Shortcut", "General Availability", "Machine Learning", "Microsoft Fabric", "ML", "News", "OneLake", "Parquet", "Preview Features", "Snowflake", "Unified Data Access"]
tags_normalized: ["ai", "ai workloads", "analytics", "apache iceberg", "azure", "cross platform integration", "data architecture", "data interoperability", "data lake", "data sharing", "delta lake", "enterprise data management", "fabric shortcut", "general availability", "machine learning", "microsoft fabric", "ml", "news", "onelake", "parquet", "preview features", "snowflake", "unified data access"]
---

Microsoft Fabric Blog announces major enhancements in Microsoft and Snowflake interoperability, authored by Microsoft Fabric Blog, with a focus on seamless data analytics, AI, and unified data management.<!--excerpt_end-->

# Microsoft and Snowflake: Simplified Interoperability with No Data Movement

Data is spread across apps, services, and clouds, creating a complex web of systems. In the age of AI, unifying this distributed data is critical for building agentic systems and unlocking value at scale. Microsoft and Snowflake are enhancing open, cross-platform integration to provide seamless access, analytics, and sharing, with no data duplication.

## Key highlights of the integration

- **Open standards**: Built around Apache Iceberg and Parquet, enabling a single copy of data to be used across both platforms.
- **Microsoft OneLake Integration**: Allows Snowflake and Microsoft customers to access and analyze data without moving or duplicating it. New UI improvements in Microsoft Fabric and Snowflake make setup intuitive, including:
  - Snowflake-branded items in OneLake (preview)
  - Use any Fabric workload (analytics, AI, visualization) directly on Snowflake data
- **Native storage and management**: Snowflake introduces UI features for OneLake to act as the storage location for Snowflake data, such that all enterprise data can live in OneLake while leveraging Snowflake engines.

## Interoperability Features Already Delivered

- **General Availability**   
  - Bidirectional data sharing between Snowflake and OneLake (no duplication)
  - Automatic translation of Iceberg metadata to Delta Lake metadata for all Fabric engines
  - Shortcut Snowflake Iceberg data (Azure, Amazon S3, GCS) into OneLake
- **Preview Features**   
  - Native storage of Snowflake Iceberg data in OneLake
  - Automatic conversion of Fabric data into Iceberg format for use in Snowflake
  - New OneLake table APIs supporting Snowflake catalog-linked databases

## Looking Ahead

- Microsoft aims to make unified, cross-platform data access generally available, allowing mission-critical workloads to leverage direct interoperability with streamlined management.
- Ongoing commitment to removing platform barriers, providing optionality and flexibility for enterprise data projects.

## Further Resources

- [Microsoft OneLake overview](https://learn.microsoft.com/fabric/governance/onelake-catalog-overview)
- [Delivering openness and interoperability blog post](https://www.microsoft.com/microsoft-fabric/blog/2025/09/16/microsoft-and-snowflake-delivering-on-the-promise-of-openness-and-interoperability)
- [Get started with new UI features](https://aka.ms/fabricsnowflakedbdocs)
- [Ask Me Anything: Fabric and Snowflake Interoperability webinar](https://developer.microsoft.com/reactor/events/26480/)
- [Snowflake guide: Iceberg in OneLake](https://www.snowflake.com/en/developers/guides/getting-started-with-iceberg-in-onelake/)

---

Data professionals can now expect streamlined, AI-driven analytics and machine learning workloads on Microsoft Fabric and Snowflake, all powered by open standards, unified storage, and simplified cross-platform workflows.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/microsoft-and-snowflake-simplified-interoperability-with-no-data-movement/)
