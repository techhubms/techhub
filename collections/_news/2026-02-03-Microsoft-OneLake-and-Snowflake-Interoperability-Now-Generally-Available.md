---
layout: "post"
title: "Microsoft OneLake and Snowflake Interoperability Now Generally Available"
description: "This announcement details the general availability of interoperability between Microsoft OneLake and Snowflake, enabling seamless data sharing, management, and analytics across both platforms. It introduces key features such as Iceberg table compatibility, cross-platform data access, native storage options, and new UI integrations, empowering data teams to eliminate silos and optimize their data architectures for analytics and AI workloads."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/microsoft-onelake-and-snowflake-interoperability-is-now-generally-available/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2026-02-03 00:00:00 +00:00
permalink: "/2026-02-03-Microsoft-OneLake-and-Snowflake-Interoperability-Now-Generally-Available.html"
categories: ["Azure", "ML"]
tags: ["AI", "AI Workloads", "Azure", "BI Development", "Cross Platform Analytics", "Data Integration", "Data Lake", "Enterprise Data", "FabCon", "Iceberg Tables", "Interoperability", "Microsoft Fabric", "Microsoft OneLake", "ML", "News", "OneLake APIs", "Real Time Analytics", "Snowflake", "SQLCon"]
tags_normalized: ["ai", "ai workloads", "azure", "bi development", "cross platform analytics", "data integration", "data lake", "enterprise data", "fabcon", "iceberg tables", "interoperability", "microsoft fabric", "microsoft onelake", "ml", "news", "onelake apis", "real time analytics", "snowflake", "sqlcon"]
---

Microsoft Fabric Blog introduces the general availability of interoperability between Microsoft OneLake and Snowflake, highlighting new cross-platform data sharing capabilities and Iceberg table integration for data and analytics teams.<!--excerpt_end-->

# Microsoft OneLake and Snowflake Interoperability Now Generally Available

Microsoft and Snowflake have announced the general availability (GA) of interoperability between Microsoft OneLake and Snowflake, a major step forward for data teams seeking unified access to analytics and AI capabilities across cloud platforms.

## Key Announcements

- **Interoperable Data Storage**: You can now natively store Snowflake-managed Iceberg tables in Microsoft OneLake. This enables bidirectional access to data from both platforms, removing the need for duplicate copies and reducing data management complexity.
- **Automatic Data Conversion**: Fabric data can be automatically converted into Iceberg format, allowing direct access from Snowflake and enabling seamless analytics workflows.
- **Unified User Experience**: Both platforms introduce new UI elements. OneLake now features a Snowflake item for rapid access to Snowflake data, while Snowflake adds functionality to push managed Iceberg tables into Fabric for discoverability as OneLake items.
- **Real-Time Synchronization**: A single copy of data is maintained. Any changes made on one platform are immediately reflected on the other, ensuring real-time consistency for analytics and operational workloads.

## Why Is This Important?

- **Eliminates Silos**: Data teams gain freedom to choose the best architecture, storage, and analytics engine for each project without being locked into proprietary formats or workflows.
- **Optimized for Analytics and AI**: This integration supports advanced analytics scenarios, real-time insights, and AI workloads, leveraging the strengths of both Microsoft Fabric and Snowflake ecosystems.
- **Reduced Operational Overhead**: Centralized management and elimination of unnecessary data duplication increase agility and lower maintenance effort.

## Getting Started & Resources

- [Snowflake’s BUILD Conference Keynote](https://www.snowflake.com/en/build/london-keynote-broadcast/)
- [Quickstart Guide: Iceberg in OneLake](https://quickstarts.snowflake.com/guide/getting_started_with_iceberg_in_oneLake/index.html)
- [Microsoft Learn: Snowflake with Iceberg tables in OneLake](https://learn.microsoft.com/fabric/onelake/onelake-iceberg-snowflake)
- [OneLake Developer Guidance](https://learn.microsoft.com/fabric/onelake/onelake-access-api)
- [Walkthrough Video](https://www.youtube.com/watch?v=pCOAiBPgnrA)
- Join the upcoming [FabCon & SQLCon 2026](https://aka.ms/FabCon) in Atlanta for in-depth guidance, demos, and expert sessions on leveraging these new capabilities.

---

For a deeper dive into the announcement and integration, [watch the fireside chat](https://aka.ms/Arun-Christian-Fireside-Chat) between Microsoft and Snowflake executives at the BUILD conference and explore live demos of the unified UI and data management experience.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/microsoft-onelake-and-snowflake-interoperability-is-now-generally-available/)
