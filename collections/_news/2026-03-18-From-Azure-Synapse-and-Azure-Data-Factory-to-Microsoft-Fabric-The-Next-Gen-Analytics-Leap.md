---
title: 'From Azure Synapse and Azure Data Factory to Microsoft Fabric: The Next-Gen Analytics Leap'
primary_section: ai
tags:
- AI
- Apache Spark
- Azure
- Azure Data Factory
- Azure Synapse Analytics
- Compatibility Assessment
- Copilot
- Data Integration
- Data Movement
- Data Warehousing
- Dedicated SQL Pools
- ETL
- Fabric Data Engineering
- Fabric Data Factory
- Fabric Data Warehouse
- High Concurrency
- Lakehouse
- Linked Services
- Microsoft Fabric
- Migration Assistant
- ML
- Native Execution Engine
- News
- Notebooks
- OneLake
- Public Preview
- Query Performance
- Schema Conversion
- Spark Job Definitions
- Spark Pools
- Starter Pools
- Synapse Pipelines
- Triggers
date: 2026-03-18 05:49:01 +00:00
author: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/from-azure-synapse-and-azure-data-factory-to-microsoft-fabric-the-next-gen-analytics-leap/
section_names:
- ai
- azure
- ml
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog (with coauthor Bogdan Crivat referenced in the post) announces public previews and updates to migration assistants that move Azure Data Factory pipelines, Azure Synapse Spark artifacts, and Synapse dedicated SQL pools into Microsoft Fabric, aiming to reduce migration risk and simplify orchestration, Spark operations, and data warehousing in a unified SaaS analytics platform.<!--excerpt_end-->

# From Azure Synapse and Azure Data Factory to Microsoft Fabric: The Next-Gen Analytics Leap

*Source: Microsoft Fabric Blog (coauthor noted in the article: Bogdan Crivat).*

Organizations using **Azure Synapse Analytics** and **Azure Data Factory (ADF)** are being encouraged to migrate to **Microsoft Fabric**, positioning Fabric as a unified SaaS platform for:

- Data integration (ETL/ELT)
- Data engineering (Spark)
- Data warehousing
- BI/analytics
- AI-driven workloads (including Copilot-assisted experiences)

The post announces several **new migration capabilities** intended to make that move more predictable and lower risk.

## Streamlining data pipelines with Fabric Data Factory

Teams with existing **ADF** and **Synapse pipelines** can migrate into **Fabric Data Factory** without rewriting everything.

### Azure Data Factory & Azure Synapse pipelines migration assistant (public preview)

Key points called out in the article:

- **Assessment-first** guided experience (compatibility, supported activities, readiness)
- Automates migration of pipelines into **Fabric Data Factory**
- Converts **linked services** into **Fabric connections**
- **Disables triggers by default** so you can validate before anything runs
- Focus: “predictable, low-risk modernization” while preserving existing pipeline logic

![Screenshot showing Pipeline selection option before migrating](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-showing-pipeline-selection-option-befor.png)

*Figure: Pipeline migration summary in Fabric Data Factory*

Docs linked in the post:

- Migration assistant: https://learn.microsoft.com/azure/data-factory/how-to-upgrade-your-azure-data-factory-pipelines-to-fabric-data-factory
- Migration planning guide: https://learn.microsoft.com/fabric/data-factory/migrate-planning-azure-data-factory

## Modernizing Spark workloads with Fabric Data Engineering

The article highlights migration for **Apache Spark** workloads from Synapse into Fabric, noting Fabric Spark features such as:

- Native Execution Engine
- High concurrency support
- Starter pools

### Synapse Spark to Fabric Data Engineering migration assistant (public preview)

What it migrates (as described):

- Spark pools → Fabric pools/environments
- Notebooks
- Spark job definitions
- Lake databases mapped using **OneLake catalog shortcuts**

Operational detail emphasized:

- **No data is moved during the process**; artifacts are copied while data stays in place
- Existing pipelines can continue running with **zero downtime**
- Produces a **detailed report** of what migrated and what needs attention
- Wizard-driven: choose source Synapse workspace, target Fabric workspace, optional lake database migration, then run
- Flags issues like unsupported runtimes and naming conflicts with actionable guidance

More info:

- https://learn.microsoft.com/fabric/data-engineering/migrate-synapse-overview

![Screenshot of data engineering migration page showing the interface for migration. Steps shown on the left are overview, select a workspace, select a destination and finish and migrate](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-data-engineering-migration-page-show.png)

*Figure: Wizard steps in the migration assistant to Fabric Data Engineering*

## Accelerating query performance with Fabric Data Warehouse

The post positions **Fabric Data Warehouse** as a modern analytical data warehouse and claims superior price/performance compared to **Synapse dedicated SQL pools**.

### Fabric Data Warehouse migration guidance

The article links:

- Migration guide (PDF): https://igniteanalyticspreday.blob.core.windows.net/migrationguides/Synapse%20to%20Fabric%20Migration%20Guide.pdf
- Migration assistant: https://learn.microsoft.com/fabric/data-warehouse/migrate-with-migration-assistant

### Fabric Data Warehouse migration assistant

Capabilities described:

- Built into Fabric
- Converts artifacts from **Synapse dedicated pools** into **Fabric Data Warehouse**
- Helps with **schema conversions** and **data movement**
- Provides an actionable summary of migration results and items needing attention
- **Copilot available** to help resolve issues

Newly announced capability:

- **Connect to a live source instance** and proceed with migration (preview)

![Screenshot of a software workspace interface labeled "DemoWorkspace" showing an empty task flow area with a message prompting to choose from predesigned task flows or add a new task. The interface includes navigation options like "New item," "New folder," "Import," and "Migrate," with a central icon of a paperclip and text indicating no content is present yet.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-software-workspace-interface-label.gif)

*Figure: Connecting to a live source and starting migration in Fabric Data Warehouse*

Additional docs linked:

- https://learn.microsoft.com/fabric/data-warehouse/migration-assistant

## The future of your data estate starts now (closing message)

The post’s recommended path:

- Assess your current Synapse environment
- Explore the Fabric migration assistants
- Plan and execute migration to a unified Fabric analytics platform

It also points readers to Microsoft account teams and the Fabric Community for hands-on support.

## Related announcement roundup

The article points to another post for broader FabCon/SQLCon announcements:

- https://aka.ms/FabCon-SQLCon-2026-news


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/from-azure-synapse-and-azure-data-factory-to-microsoft-fabric-the-next-gen-analytics-leap/)

