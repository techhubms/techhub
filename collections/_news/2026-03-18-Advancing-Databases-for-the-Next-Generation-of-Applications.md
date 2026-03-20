---
title: Advancing Databases for the Next Generation of Applications
primary_section: github-copilot
tags:
- Agentic Control Plane
- AI
- Azure
- Azure Arc
- Azure Cosmos DB
- Azure SQL
- Azure SQL Database
- CI/CD
- Collation
- Customer Managed Keys
- Database Hub
- DevOps
- DiskANN
- Dynamic Data Masking
- GitHub Copilot
- GitHub Copilot CLI
- GitHub Copilot in SSMS
- Governance
- Hyperscale
- Microsoft Fabric
- Migration Assistant
- Mirroring
- ML
- News
- Observability
- OneLake
- Private Endpoints
- Private Link
- REST API
- Security
- SQL Auditing
- SQL Database in Fabric
- SQL Server
- SSMS 22
- T SQL
- Vector Indexing
- Vector Search
- VNET
date: 2026-03-18 05:47:11 +00:00
author: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/advancing-databases-for-the-next-generation-of-applications/
section_names:
- ai
- azure
- devops
- github-copilot
- ml
- security
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog summarizes FabCon | SQLCon announcements focused on Microsoft Fabric and Azure databases: a new Database Hub for unified estate management, new SQL database in Fabric capabilities (migration, security, configuration), Cosmos DB mirroring updates, and developer tooling like GitHub Copilot in SSMS 22.<!--excerpt_end-->

# Advancing Databases for the Next Generation of Applications

If you haven’t already, see Arun Ulag’s overview post: [FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news).

## New announcements from FabCon | SQLCon

The post argues that AI-driven applications increasingly need a **unified data foundation** where transactional and analytical data can be combined securely, in near real time, and at scale.

Microsoft’s framing is:

- A **single SQL engine** spanning on-premises, PaaS, and SaaS
- **Databases built natively into Microsoft Fabric**
- A focus on modernizing SQL, unifying the data estate, and enabling “AI-native” apps

## Introducing the Database Hub in Fabric (early access)

Microsoft is introducing the **Database Hub in Fabric** (available in [early access](https://aka.ms/database-hub)) as a unified database management experience.

It aims to provide a single place to explore/observe/govern/optimize database estates that can include:

- Azure SQL
- Azure Cosmos DB
- Azure Database for PostgreSQL
- SQL Server (enabled by Azure Arc)
- Azure Database for MySQL
- Fabric databases

![Dashboard screenshot displaying estate overview and alerts for database management. Key sections include total items (571), security metrics with authentication at 70%, encryption at 88%, compliance at 95%, capacity utilization showing 16 monitored CPU usage, and alerts highlighting memory pressure, blocking, long-running queries, and replication lag with specific durations and responsible agents. AI-generated content may be incorrect.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/dashboard-screenshot-displaying-estate-overview-an.png)

The Database Hub is positioned as an **agent-assisted, human-in-the-loop** operational model:

- Agents analyze estate-wide signals to surface changes and recommend actions
- Built-in observability, delegated governance, and “Copilot powered insights”
- Aggregate health views, trend analysis, and common performance categories across services

Sign-up link: [Sign up for early access](https://aka.ms/database-hub).

## SQL database in Fabric

**SQL database in Fabric** is described as a SaaS-native experience using the Microsoft SQL engine, extending Azure SQL Database so teams can connect operational data to analytics and AI when ready.

A customer example (Origence) is described as moving legacy SQL Server to SQL database in Fabric, simplifying ETL with OneLake, enabling real-time analytics, lowering costs, and preparing for AI workloads.

> “SQL database in Fabric gives us a modern SQL foundation for high-volume operational data and real-time analytics—helping us move faster without compromising enterprise‑grade security or control.”
>
> — Jeff Shood, CTO, Origence

### Migration Assistant for SQL databases (Preview)

A new **Migration Assistant** (Preview) is announced to:

- Assess readiness
- Identify compatibility considerations
- Guide schema migration from SQL Server into Fabric

![Screenshot of a software interface showing migration options under "Migrate to Fabric," including migrating data engineering items, semantic models, and databases. The "Migrate" button is highlighted, with a focus on the "SQL Server (Preview)" option described as suitable for workloads needing full T-SQL support, low latency, high concurrency OLTP, and smaller analytics without MPP.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-software-interface-showing-migrati.png)

### Autonomous by default, configurable where it matters

The post describes SQL database in Fabric as “autonomous by default” with new configuration options. Announced/mentioned enhancements include:

- **Expanded enterprise security controls**
  - SQL auditing
  - Customer-managed keys (CMK) for encryption control
  - Dynamic data masking
  - Enhanced availability zone support
- **Expanded SQL compatibility**
  - More database compatibility levels
  - Additional T-SQL features
  - Full-text search
  - `ALTER DATABASE` settings
- **Full collation support enabled**
  - All Azure SQL collations usable in Fabric
  - Collation can be specified at database creation via REST API
- **Configurable compute limits**
  - Database-level compute caps to control vCore usage

![Screenshot of a database configuration panel for managing performance and capacity by setting max vCore limits. It shows a dropdown menu set to "4 vCores," a warning about potential temporary connectivity disruptions when changing settings, and buttons labeled "Save" and "Discard."](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-database-configuration-panel-for-m.png)

- **Improved backup and recovery**
  - Deleted databases move to a soft-deleted Recycle Bin
  - Configurable retention: 1–35 days
  - Restore to any point within retention
- **Pre/post-deployment script support**
  - Deployment pipelines can run additional T-SQL scripts
  - Intended for branching, provisioning, and static data management
  - Stated as compatible with CI/CD tooling

### Built for analytics and AI

A key differentiator presented is connectivity into Fabric:

- Built-in **mirroring to OneLake** to make SQL data “analytics-ready”
- Example scenarios mentioned: real-time insights, vector search, AI-powered apps

New features called out:

- **Finer control over mirroring to OneLake**
  - Select which tables are mirrored
  - Manage mirroring via REST API
- **Enhanced vector indexing for AI workloads**
  - **DiskANN Vector Indexes** support:
    - Advanced quantization
    - Full DML operations
    - Iterative filtering
    - Real-time index maintenance

Docs: [SQL database in Fabric documentation](https://learn.microsoft.com/fabric/database/sql/overview)

## Cosmos DB in Fabric updates

### Private network configurations for Cosmos DB mirroring in Fabric (GA)

General availability announcement:

- **Private network support** for **Azure Cosmos DB mirroring in Microsoft Fabric**
- Supports Cosmos DB accounts secured with:
  - Private Endpoints
  - VNETs
- Goal: keep network isolation/compliance controls while enabling near real-time analytics and AI workloads in Fabric via OneLake

![Screenshot of replication status for a Cosmos database in Fabric. Key elements include database details, source connection info, status marked as "Running," rows replicated count, last completion timestamp, and options to create a semantic model or query data, with a green "Query is 1.5x" button highlighted.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-replication-status-for-a-cosmos-data.png)

### Azure Cosmos DB Agent Kit (for AI coding assistants)

The post introduces the **Azure Cosmos DB Agent Kit** as an open-source collection of “agent skills” intended to extend AI coding assistants with Cosmos DB best practices (data modelling, partitioning, resilient distributed app patterns).

It states the kit works with:

- GitHub Copilot CLI
- Claude Code
- Any other “agent skills-compatible” tool

Install command shown:

```bash
npx skills add AzureCosmosDB/cosmosdb-agent-kit
```

Docs: [Azure Cosmos DB Agent Kit documentation](https://learn.microsoft.com/azure/cosmos-db/gen-ai/agent-kit)

## Other announcements mentioned

- **SQL Server 2025 innovations**
  - Semantic and full-text search for AI scenarios
  - Native JSON and REST APIs
  - Change event streaming
  - Security/performance/reliability improvements
- **Azure SQL Database Hyperscale**
  - Independent read scaling
  - Higher vCore options
  - Vector search
  - “Secure AI integration” (as stated)
- **Azure Arc enabled SQL Server migration**
  - Discovery, readiness assessments, guided modernization to Azure
- **Developer productivity**
  - **GitHub Copilot in SSMS 22** is now generally available

Related post link: [Azure Databases update](https://www.microsoft.com/en-us/sql-server/blog/2026/03/18/advancing-agentic-ai-with-microsoft-databases-across-a-unified-data-estate/)

## Calls to action and event links

- Agenda: [FabCon agenda](https://fabriccon.com/program/agenda)
- Database Hub: [early access](https://aka.ms/database-hub)
- Exam info: [Get started today](https://aka.ms/dp800/exam)
- Europe event registration: [Register](https://www.sharepointeurope.com/european-microsoft-fabric-community-conference-tickets/)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/advancing-databases-for-the-next-generation-of-applications/)

