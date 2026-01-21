---
external_url: https://blog.fabric.microsoft.com/en-US/blog/microsoft-and-snowflake-simplified-interoperability-with-no-data-movement/
title: 'Microsoft and Snowflake: Simplified Interoperability with No Data Movement'
author: Microsoft Fabric Blog
viewing_mode: external
feed_name: Microsoft Fabric Blog
date: 2025-11-18 08:00:00 +00:00
tags:
- AI Workloads
- Analytics
- Apache Iceberg
- Cross Platform Integration
- Data Architecture
- Data Interoperability
- Data Lake
- Data Sharing
- Delta Lake
- Enterprise Data Management
- Fabric Shortcut
- General Availability
- Microsoft Fabric
- OneLake
- Parquet
- Preview Features
- Snowflake
- Unified Data Access
section_names:
- ai
- azure
- ml
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
