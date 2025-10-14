---
layout: "post"
title: "Azure Synapse Runtime for Apache Spark 3.5 Now Generally Available"
description: "This announcement details the general availability of Azure Synapse Runtime for Apache Spark 3.5, enabling customers to adopt the latest Spark runtime with Delta Lake 3.2 support for their production workloads. It also covers considerations for migration to Microsoft Fabric Spark and highlights advanced features such as the Native Execution Engine, starter pools, unified security, and materialized views that await customers planning a future transition."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/general-availability-azure-synapse-runtime-for-apache-spark-3-5/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-10-14 10:29:13 +00:00
permalink: "/2025-10-14-Azure-Synapse-Runtime-for-Apache-Spark-35-Now-Generally-Available.html"
categories: ["Azure", "ML"]
tags: ["Apache Spark 3.5", "Azure", "Azure Synapse", "Data Engineering", "Delta Lake 3.2", "Lakehouse", "Materialized Views", "Microsoft Fabric", "Migration", "ML", "Native Execution Engine", "News", "Production Workloads", "Release Notes", "Spark Runtime", "Starter Pools"]
tags_normalized: ["apache spark 3dot5", "azure", "azure synapse", "data engineering", "delta lake 3dot2", "lakehouse", "materialized views", "microsoft fabric", "migration", "ml", "native execution engine", "news", "production workloads", "release notes", "spark runtime", "starter pools"]
---

Microsoft Fabric Blog announces the general availability of Azure Synapse Runtime for Apache Spark 3.5, helping customers leverage updated Spark features before migrating to Microsoft Fabric Spark.<!--excerpt_end-->

# Azure Synapse Runtime for Apache Spark 3.5 Now Generally Available

Microsoft has officially released Azure Synapse Runtime for Apache Spark 3.5, making it available for Azure Synapse Spark customers to use for production workloads. This update provides feature enhancements from Apache Spark 3.5 and Delta Lake 3.2, enabling organizations to benefit from the latest updates in Spark performance, reliability, and compatibility.

## Key Announcements

- **General Availability:** Customers can now create and run workloads using Azure Synapse Runtime for Apache Spark 3.5.
- **Core Upgrades:** The release incorporates major improvements from Apache Spark 3.5 and Delta Lake 3.2.
- **Production Ready:** Suitable for mission-critical and production data engineering, ETL, and analytics workloads.
- **Release Notes:** For a comprehensive list of changes and features, refer to the official [release notes for Apache Spark 3.5](https://github.com/microsoft/synapse-spark-runtime/tree/main/Synapse/spark3.5).

## Migration Information

Customers are encouraged to review [migration guidelines between Spark 3.4 and 3.5](https://spark.apache.org/docs/latest/sql-migration-guide.html#upgrading-from-spark-sql-34-to-35) to identify necessary modifications to existing jobs, applications, and notebooks.

## Looking Forward: Transition to Microsoft Fabric Spark

While continuing support for Azure Synapse Spark, Microsoft recommends organizations plan for migration to Microsoft Fabric Spark to take advantage of:

- **Native Execution Engine (NEE):** Enhanced query performance at no extra cost ([Learn more](https://learn.microsoft.com/en-us/fabric/data-engineering/native-execution-engine-overview?tabs=sparksql)).
- **Starter Pools:** Faster Spark session creation ([Details](https://learn.microsoft.com/en-us/fabric/data-engineering/spark-compute)).
- **Unified Lakehouse Security:** Manage Row-Level Security (RLS) and Column-Level Security (CLS) easily.
- **Materialized Views:** Efficient and flexible data options for analytics ([Overview](https://learn.microsoft.com/en-us/fabric/data-engineering/materialized-lake-views/overview-materialized-lake-view)).

To learn more about this runtime, see [Azure Synapse Runtime for Apache Spark 3.5 documentation](https://learn.microsoft.com/en-us/azure/synapse-analytics/spark/apache-spark-35-runtime).

## Summary

The general availability of Azure Synapse Runtime for Apache Spark 3.5 marks a significant update for data engineers and analytics teams using Azure. Current users should plan for eventual migration to Microsoft Fabric Spark to maximize access to the latest performance and security advancements.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/general-availability-azure-synapse-runtime-for-apache-spark-3-5/)
