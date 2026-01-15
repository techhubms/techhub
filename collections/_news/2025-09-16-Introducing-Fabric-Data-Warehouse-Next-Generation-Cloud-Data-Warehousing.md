---
layout: post
title: 'Introducing Fabric Data Warehouse: Next-Generation Cloud Data Warehousing'
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/welcome-to-fabric-data-warehouse/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-09-16 00:30:00 +00:00
permalink: /ml/news/Introducing-Fabric-Data-Warehouse-Next-Generation-Cloud-Data-Warehousing
tags:
- Audit Logs
- Azure
- Coding
- Copilot
- Customer Managed Keys
- Data Engineering
- Data Migration
- Dbt Connector
- Enterprise Data Warehousing
- Fabric Data Warehouse
- Lakehouse Architecture
- Microsoft Fabric
- Migration Assistant
- ML
- News
- OPENROWSET(JSONL)
- Performance Benchmarking
- PrivateLink
- Serverless Data
- SQL Optimization
- SSMS Integration
section_names:
- azure
- coding
- ml
---
This post from Microsoft Fabric Blog introduces Fabric Data Warehouse and its architectural, performance, and security advances for data professionals and developers, with contributions and insights from the Microsoft Fabric team.<!--excerpt_end-->

# Introducing Fabric Data Warehouse: Next-Generation Cloud Data Warehousing

Fabric Data Warehouse is a recent addition to Microsoft's data platform ecosystem, developed from the ground up for the 2020s with a next-generation, lakehouse-based architecture. This news post details key innovations, enterprise-focused features, developer tooling, and migration aids, backed by real customer outcomes announced at FabCon Europe.

## Architectural Overview and Performance

- **Lakehouse Architecture:** Fully serverless, scales to hundreds of petabytes, and tightly integrated with the broader Fabric data platform.
- **Performance Gains:** 40+ performance improvements delivered in early 2025 yielded a 36% improvement in standard benchmarks. For example, Willis Tower Watson reduced a query from 64 hours for 80 TB on Azure Synapse to 29 hours for over 400 TB on Fabric—processing more data in less time.
- **Continuous Enhancements:** Upcoming features include incremental/proactive stats refresh and clustering for further optimization.

## Enterprise-Grade Features

- **Resiliency:** Automatic failover across zones and node-level query resiliency.
- **Security and Compliance:** Support for [PrivateLink](https://blog.fabric.microsoft.com/blog/fabric-workspace-level-private-link-preview/), customer-managed encryption keys, outbound network controls, and streamlined audit log analysis.
- **Flexible Data Types:** Native support for `varchar(max)` and `varbinary(max)` for large data values.
- **Advanced SQL:** `MERGE` operations (combined insert, update, delete), `OPENROWSET(JSONL)` for querying JSON directly in OneLake/ADLS, and soon, identity columns.
- **Collation:** [Workspace-level collation settings](https://blog.fabric.microsoft.com/blog/streamlining-data-management-with-collation-settings-in-microsoft-fabric-warehouse-sql-analytics-endpoint//) enable configuration for case sensitivity.

## Developer and Migration Tooling

- **Data Agent and Data-driven alerts:** Trigger actions based on query results.
- **Integration with SSMS:** Familiar tooling for database professionals.
- **[dbt Connector](https://github.com/Microsoft/dbt-fabric):** Enhanced support for analytics engineers.
- **Copilot for SQL:** Integrated assistant for SQL optimization, documentation lookup, chat-based assistance, and improved SQL generation.
- **Warehouse Migration Assistant:** AI-powered, self-service migration tool with seamless transition from Azure Synapse and dedicated pools, as shared by Kantar (reduced costs by 50% and quadrupled environments).

## Getting Started

- **Join the Community:** Over 20,000 warehouse customers and hundreds of thousands of developers and data professionals are embracing Fabric Data Warehouse.
- **Try for Free:** [Start with two months free](https://aka.ms/tryfabric) (no credit card required).
- **Product Roadmap:** Review the [public roadmap](https://blog.fabric.microsoft.com/blog/announcing-the-fabric-roadmap?ft=All) and contribute ideas via the [Ideas Portal](https://experience.dynamics.com/ideas/categories/list/?category=556c6fe0-a302-e711-8106-5065f38a3bb1&amp;forum=515617a5-dedb-e911-a814-000d3a4f1244).

## Customer Success Highlights

- Willis Tower Watson: Achieved 47% performance improvement processing 5x more data compared to equivalent Synapse workloads.
- Kantar: Migrated seamlessly, quadrupled development environments, reduced costs by half, and leveraged familiar T-SQL capabilities.

## Key Takeaways for Practitioners

- Fabric Data Warehouse offers significant advancements for cloud-scale data warehousing, with a strong focus on both enterprise needs and development modernity.
- Security, management, and developer tooling are core priorities, supporting flexible integration and high productivity.
- Migration tools and compatibility features lower adoption friction for organizations moving from Azure Synapse or other enterprise data warehouse products.

For detailed technical documentation and ongoing announcements, follow the [Microsoft Fabric Blog](https://blog.fabric.microsoft.com/).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/welcome-to-fabric-data-warehouse/)
