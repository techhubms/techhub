---
layout: "post"
title: "Lakehouse Schemas Now Generally Available in Microsoft Fabric"
description: "This article details the general availability of schema-enabled lakehouses in Microsoft Fabric. It explains how schemas enhance organization, accessibility, and interoperability for tables in lakehouses. The article also covers new features such as schema shortcuts, Spark multi-workspace queries, security improvements, and discusses current limitations and future enhancements planned for schema lakehouses."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/lakehouse-schemas-generally-available/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-12-10 09:00:00 +00:00
permalink: "/2025-12-10-Lakehouse-Schemas-Now-Generally-Available-in-Microsoft-Fabric.html"
categories: ["Azure", "ML"]
tags: ["ADLS Gen2", "Azure", "Data Engineering", "Data Lake", "Data Management", "Fabric Notebooks", "Feature Parity", "Lakehouse", "Materialized Views", "Microsoft Fabric", "Migration Tools", "ML", "News", "OneLake Security", "Schema Lakehouse", "Spark", "Table Organization", "Workspace Private Links"]
tags_normalized: ["adls gen2", "azure", "data engineering", "data lake", "data management", "fabric notebooks", "feature parity", "lakehouse", "materialized views", "microsoft fabric", "migration tools", "ml", "news", "onelake security", "schema lakehouse", "spark", "table organization", "workspace private links"]
---

Microsoft Fabric Blog explains the launch of schema-enabled lakehouses, outlining new organizational options, Spark compatibility, security features, and ongoing plans for improvements.<!--excerpt_end-->

# Lakehouse Schemas Now Generally Available in Microsoft Fabric

Schema-enabled lakehouses introduce a new way to organize and manage your tables in Microsoft Fabric. When creating new lakehouses, schema-enabled options are now the default, though users can still opt for classic non-schema lakehouses if preferred.

## Key Features

- **Improved Table Organization**: Schemas act like folders, helping users find and segment data efficiently.
- **Schema Shortcuts**: Create pointers to internal tables in other schemas, or to external data sources such as ADLS Gen2.
- **Enhanced Spark Interoperability**: Reference lakehouses across multiple workspaces and join them in a single query. Both schema and non-schema lakehouses are supported through naming conventions; future migration scenarios are planned.
- **Security and Performance**: Support for OneLake security with RLS/CLS and Fabric Materialized Views.
- **Notebook Compatibility**: To use schema-enabled lakehouses in a Notebook, ensure you have it pinned, or none pinned. Spark currently requires this configuration, but improvements for more flexible workspace modes are coming soon.

## Upcoming Enhancements and Limitations

Several Spark-related features for schema lakehouses are rolling out soon:

- Support for Spark Views
- Shared lakehouse support across workspaces
- User-Defined Functions (UDFs) for lakehouses
- External ADLS table support
- Workspace Private Links
- Outbound Traffic Protection

Workarounds are currently available for these, with more detail at [Lakehouse schemas – Microsoft Fabric | Microsoft Learn](https://learn.microsoft.com/en-us/fabric/data-engineering/lakehouse-schemas).

## Existing Non-Schema Lakehouses

Non-schema lakehouses continue to be supported, and Microsoft is working towards full feature parity and interoperability. Tools will be introduced for a smooth transition from non-schema to schema-enabled lakehouses, ensuring users can benefit from new features without migration downtime.

## References

- [Lakehouse Schemas (Generally Available) Blog](https://blog.fabric.microsoft.com/en-us/blog/lakehouse-schemas-generally-available/)
- [Lakehouse schemas – Microsoft Fabric | Microsoft Learn](https://learn.microsoft.com/en-us/fabric/data-engineering/lakehouse-schemas)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/lakehouse-schemas-generally-available/)
