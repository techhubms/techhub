---
title: 'FabCon and SQLCon 2026: Unifying databases and Fabric on a single platform'
feed_name: Microsoft News
author: stclarke
date: 2026-03-18 13:38:44 +00:00
section_names:
- ai
- azure
- devops
- github-copilot
- ml
- security
tags:
- AI
- Analysis Services
- Apache Spark 4
- Azure
- Azure Arc
- Azure Cosmos DB
- Azure Data Factory Migration
- Azure Database For MySQL
- Azure Database For PostgreSQL
- Azure SQL
- Azure Synapse Migration
- Change Data Feed
- CI/CD
- Column Level Security
- Company News
- DACPAC
- Data Governance
- Database Hub
- Delta Lake
- DevOps
- Direct Lake
- FabCon
- Fabric Databases
- Fabric Extensibility Toolkit
- Fabric IQ
- Fabric MCP
- Git Integration
- GitHub Copilot
- Graph in Fabric
- KQL
- MCP
- Microsoft Fabric
- Microsoft OneLake
- Mirroring in Fabric
- ML
- News
- PySpark
- Row Level Security
- Runtime 2.0
- Security
- Semantic Models
- Shortcut Transformations
- Spark SQL
- SQL Server
- SQLCon
primary_section: github-copilot
external_url: https://azure.microsoft.com/en-us/blog/fabcon-and-sqlcon-2026-unifying-databases-and-fabric-on-a-single-data-platform/
---

In this Microsoft news post, stclarke outlines what’s new across Microsoft Fabric and SQL at FabCon/SQLCon 2026, including Database Hub, OneLake updates, Fabric analytics runtime improvements, Fabric IQ’s semantic layer, and new developer tooling like Fabric MCP that can connect GitHub Copilot to Fabric.<!--excerpt_end-->

# FabCon and SQLCon 2026: Unifying databases and Fabric on a single platform

Welcome to the third annual **FabCon** and the first **SQLCon** (Atlanta). The event focuses on bringing **Microsoft SQL** and **Microsoft Fabric** together into a single, unified data platform, with nearly 300 workshops and sessions.

## Why Fabric + databases are converging

Microsoft positions the combined platform as a way to unify:

- Transactional data
- Operational data
- Analytical data

…under a single architecture, with an emphasis on getting data estates ready for **AI-powered, multi-agent systems**.

## Introducing the Database Hub in Microsoft Fabric (early access)

**Database Hub in Fabric** is introduced as a unified database management experience across edge, cloud, and Fabric.

It’s described as a single place to **explore, observe, govern, and optimize** a database estate, including:

- **Azure SQL**
- **Azure Cosmos DB**
- **Azure Database for PostgreSQL**
- **SQL Server** (enabled by **Azure Arc**)
- **Azure Database for MySQL**
- **Fabric Databases**

The post also describes an **agent-assisted, human-in-the-loop** approach for database management, with built-in observability, delegated governance, and **Copilot-powered insights**.

- More details: https://aka.ms/database-hub
- Broader database updates: https://aka.ms/FabCon-SQLCon-2026-Shireesh

## Getting your data estate ready for AI with Fabric

The post argues that as orgs adopt multi-agent AI systems, differentiation shifts from “which model” to **context and business intelligence**.

It introduces **Microsoft IQ** as an “intelligence layer” built from:

- Work IQ (productivity signals)
- Foundry IQ (institutional knowledge)
- Fabric IQ (live business data)

It frames an “AI-ready data foundation” as four steps:

1. **Unify** your data estate to reduce silos.
2. **Process and harmonize** data so it’s clean, connected, and structured.
3. **Curate semantic meaning** so agents understand business context.
4. **Enable agents to act** using that context.

## Unifying your data estate with Microsoft OneLake

**Microsoft OneLake** is positioned as a logical data lake that spans clouds, on-premises, and third-party platforms without unnecessary ETL duplication.

### Connecting to more sources

Updates called out:

- Expanded **Mirroring in Fabric** support:
  - SharePoint lists (preview)
  - Dremio (preview)
  - Azure Monitor (coming soon)
  - Oracle (GA)
  - SAP Datasphere (GA)
- “Extended mirroring” capabilities aimed at operationalizing mirrored sources at scale:
  - **Change Data Feed (CDF)**
  - Views on mirrored data (starting with **Snowflake**)
  - These extended capabilities will be a paid option.
- **Shortcut transformations** (GA): automatic shaping as data connects/moves within OneLake.
  - Example mentioned: converting **Excel** to **Delta tables** (preview)
  - Includes AI-powered transformations.

Interoperability investments:

- Native read from OneLake via **Azure Databricks Unity Catalog** (public preview): https://aka.ms/Databricks-Read-FabCon-2026
- OneLake + Snowflake interoperability (GA):
  - https://blog.fabric.microsoft.com/en-us/blog/microsoft-onelake-and-snowflake-interoperability-is-now-generally-available?ft=All

### OneLake security (GA soon)

**OneLake security** is announced as generally available “in the coming weeks”, enabling:

- Role definition
- Row-level and column-level controls
- Unified permission model that follows the data

More:

- https://learn.microsoft.com/en-us/fabric/onelake/security/get-started-security#onelake-security-preview

## Processing and harmonizing data with Fabric analytics

Fabric analytics engines called out:

- Spark
- T-SQL
- KQL
- Analysis Services

New announcements:

- **Runtime 2.0** (preview):
  - Apache Spark **4.x**
  - Delta Lake **4.x**
  - Scala **2.13**
  - Azure Linux Mariner **3.0**
- **Materialized lake views** (GA): simplify medallion architecture in Spark SQL / PySpark, and enable up-to-date pipelines without manual orchestration.
- A new agentic **Copilot experience in notebooks**: deeper workspace context awareness and code generation.

Real-time:

- **Microsoft Fabric Maps** (GA): geospatial context for agents and operations.

More:

- Fabric Analytics announcements: https://aka.ms/FabCon-SQLCon-2026-Bogdan
- Real-Time Intelligence announcements: https://aka.ms/FabCon-SQLCon-2026-Yitzhak

## Creating semantic meaning with Fabric IQ

**Fabric IQ** is described as a semantic layer that unifies analytical and operational data (including telemetry, time series, graph, and geospatial data) into a shared framework of:

- Entities
- Relationships
- Properties
- Rules
- Actions

It also states:

- Fabric IQ ontologies will be accessible via an **MCP server** (preview), enabling agents to discover and use the semantic layer.
- New **planning in Fabric IQ** (announced) for budgets/forecasts/scenarios on top of Fabric semantic models:
  - https://aka.ms/FabCon-SQLCon-2026-Planning

Underlying tech enhancements:

- **Direct Lake on OneLake** (GA): read tables directly from OneLake with native security enforcement.
  - https://aka.ms/Direct-Lake-OneLake
- **Graph in Fabric** (GA soon): visualize/query complex relationships.
  - https://learn.microsoft.com/en-us/fabric/graph/overview

More:

- Fabric IQ announcements: https://aka.ms/FabCon-SQLCon-2026-Yitzhak-FabricIQ
- Power BI announcements: https://aka.ms/FabCon-SQLCon-2026-Mo

## Empowering agents to act with Fabric data and operations agents

Two built-in agent types are highlighted:

- **Fabric data agents** (GA): “virtual analysts” aligned to specific domain data.
  - https://learn.microsoft.com/en-us/fabric/data-science/concept-data-agent
- **Operations agents**: monitor real-time data, detect patterns, and take proactive action.
  - https://learn.microsoft.com/en-us/fabric/real-time-intelligence/operations-agent

The post notes these agents can be used in Fabric and as knowledge sources for tools like Microsoft Foundry, Copilot Studio, and Microsoft 365 Copilot.

## Developer experiences in Fabric

### Fabric Model Context Protocol (MCP)

Two MCP milestones are announced:

- **Fabric local MCP** (GA): open-source local server connecting AI coding assistants (including **GitHub Copilot**) to Fabric.
  - GitHub repo: https://github.com/microsoft/mcp/blob/main/servers/Fabric.Mcp.Server/README.md
  - More: https://aka.ms/Fabric-local-mcp
- **Fabric remote MCP** (public preview): cloud-hosted execution engine for authenticated actions in Fabric.
  - https://aka.ms/Fabric-remote-MCP

### Git integration and CI/CD

Git integration improvements:

- Selective branching
- Improved change comparisons
- New folder relationships between feature workspaces and source workspaces

Docs: https://learn.microsoft.com/fabric/cicd/git-integration/intro-to-git-integration?tabs=azure-devops

### Open-source projects

- **Agent Skills for Fabric**: plugins for using natural language in the GitHub Copilot terminal to work with Fabric.
  - https://github.com/microsoft/skills-for-fabric
- **Fabric Jumpstart**: reference architectures and single-click deployments for sample datasets, notebooks, pipelines, and reports.
  - https://jumpstart.fabric.microsoft.com/

### Fabric Extensibility Toolkit (FET)

- **Fabric Extensibility Toolkit (FET)** (GA), evolving the Workload Development Kit (WDK)
- Adds support for:
  - Full CI/CD
  - Variable library
  - New admin portal management experience

Overview: https://learn.microsoft.com/fabric/extensibility-toolkit/extensibility-toolkit-overview

More platform updates: https://aka.ms/FabCon-SQLCon-2026-Kim

## Migrating existing Azure services to Fabric

Migration assistants (public preview) are announced for:

- **Azure Data Factory**
- **Azure Synapse Analytics**
- **Azure SQL**

Key points:

- ADF/Synapse assistant: move pipelines and artifacts like Spark pools and notebooks with “minimal disruption”, supporting incremental modernization.
  - https://aka.ms/Azure-Data-Factory-Migration
- SQL database migration assistant: move SQL Server into Fabric by importing schemas via **DACPAC**, identifying/solving compatibility issues with AI assistance, and guiding assessment/data copy.
  - https://aka.ms/SQL-Fabric-migration

## More resources

- FabCon/SQLCon event page: https://fabriccon.com/
- Fabric March 2026 feature summary: https://aka.ms/March-2026-Feature-Summary-Blog
- Power BI March 2026 feature summary: https://aka.ms/March-2026-PowerBI-Summary-Blog
- Fabric updates channel: https://blog.fabric.microsoft.com/en-US/blog/
- Fabric free trial: https://aka.ms/try-fabric
- Fabric roadmap: https://aka.ms/Fabric-Roadmap
- Fabric SKU estimator: https://aka.ms/FabricSKUEstimator
- Fabric website: https://microsoft.com/microsoft-fabric
- Fabric community: https://aka.ms/fabric-community

> Microsoft Fabric is helping us evolve our data foundation into a more unified, AI-ready platform. Combined with Power BI and capabilities like Fabric IQ, it enables the enterprise to turn data into intelligence and act on it faster.
> Shekhar Gowda, Vice President of Global Marketing Technologies at The Coca-Cola Company


[Read the entire article](https://azure.microsoft.com/en-us/blog/fabcon-and-sqlcon-2026-unifying-databases-and-fabric-on-a-single-data-platform/)

