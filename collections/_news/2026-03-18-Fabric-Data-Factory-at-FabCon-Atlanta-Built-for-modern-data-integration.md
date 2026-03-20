---
title: 'Fabric Data Factory at FabCon Atlanta: Built for modern data integration'
primary_section: ai
tags:
- AI
- Apache Airflow
- Azure Data Factory
- Azure Key Vault
- Change Data Feed (cdf)
- Copy Job
- Data Integration
- Data Orchestration
- Dataflow Gen2
- Dbt Jobs
- DevOps
- ELT
- ETL
- Fabric Data Factory
- Fabric Data Factory MCP Server
- GitHub Integration
- Microsoft Entra ID
- Microsoft Fabric
- Migration Assistant
- Mirroring
- ML
- Model Context Protocol (mcp)
- News
- On Premises Data Gateway
- OneLake
- Open Mirroring
- Outbound Access Protection (oap)
- Power Query (m)
- Private Endpoints
- Security
- Service Principal (spn)
- Slowly Changing Dimension Type 2 (scd2)
- SSIS
- Tumbling Window Trigger
- VNet Gateway
- Workspace Identity (wi)
date: 2026-03-18 05:49:18 +00:00
author: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/fabric-data-factory-at-fabcon-atlanta-built-for-modern-data-integration/
section_names:
- ai
- devops
- ml
- security
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog summarizes FabCon Atlanta announcements for Fabric Data Factory, covering new security controls, OneLake mirroring expansions, Dataflow Gen2 and Copy job upgrades, orchestration improvements, and an MCP server that lets agentic tools integrate with Fabric Data Factory capabilities.<!--excerpt_end-->

# Fabric Data Factory at FabCon Atlanta: Built for modern data integration

*If you haven’t already, check out Arun Ulag’s hero blog “[FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news)” for a complete look at announcements across Fabric and Microsoft’s database offerings.*

At FabCon Atlanta, Microsoft positions **Fabric Data Factory** as the data-integration foundation of **Microsoft Fabric**, built into Fabric and powered by **OneLake**. The article highlights updates across security, unification, transformation, orchestration, multi-cloud movement, AI-assisted experiences, and migration from Azure Data Factory (ADF).

![Fabric Data Factory overview diagram (unify, transform, orchestrate, distribute data)](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/fabric-data-factory-as-the-data-integration-platfo.png)

## Mission critical data integration

Focus: security, compliance, governance, and operational reliability.

- **Outbound Access Protection (OAP)**
  - Ensures **pipelines**, **Dataflows Gen2**, and **Copy jobs** only connect to approved destinations.
  - Intended to reduce data exfiltration risk and strengthen compliance.
  - Works alongside **private endpoints** and **workspace-level policies**.

- **Azure Key Vault integration for VNet Gateway (GA)**
  - Centralizes connection secrets in **Azure Key Vault**.
  - Goal: reduce credential sprawl, support secret rotation, and connect to private/firewalled sources without exposing credentials.

- **Automatic updates for on-premises data gateway**
  - Keeps gateways on the latest supported version (including security patches).
  - Admins can trigger updates without logging into gateway machines, reducing manual maintenance/downtime.

## Unify your data estate with OneLake

Fabric Data Factory’s unification story centers on **mirroring data into OneLake**.

- **Mirroring (GA)**
  - **Oracle** mirroring: GA
  - **SAP Datasphere** mirroring: GA

- **Mirroring (Preview)**
  - **SharePoint lists** mirroring: preview

- **Extended mirroring capabilities**
  - **Change Data Feed (CDF)** generation as part of mirroring into OneLake
  - **Mirroring views**, starting with **Snowflake**
  - Documentation: [extended capabilities in Mirroring](https://aka.ms/MirroringDocumentation)

- **Fabric Open Mirroring partner integrations**
  - Example called out: enabling **Informatica Cloud Data Ingestion and Replication (CDIR)** Open Mirroring support “with a single click”, enabling ingestion from 300+ sources into Fabric mirrored databases.

![Logos of Open Mirroring partners](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-depicts-a-collection-of-various-data-man.png)

## Dataflows: self-service data transformation at scale

The article frames **Dataflow Gen2** as the self-service transformation layer, with new GA and preview capabilities.

### Dataflow Gen2 updates (GA)

- Performance improvements with **Modern Query Evaluator** and **Preview Only Steps**
- **Workspace Variables** and relative references to Fabric items
- Dataflow refresh notification emails
- **Export Query Results in Power BI Desktop**
- **Save As Dataflow Gen2** (migration from Dataflow Gen1)

### Dataflow Gen2 updates (Preview)

- **Recent Tables** for easier access to sources
- New destinations: **Snowflake**, **Excel Files**
- Richer diagnostic download
- **Streaming API** support for query execution
- Selecting the **SharePoint site** when selecting destination
- Validation for data destination during publishing

### Mapping Data Flows coming to Fabric

- Microsoft notes teams rely on **Mapping Data Flows** in **Azure Data Factory** for Spark-scale integration.
- By **June 2026**, Mapping Data Flows are planned to run **natively in Fabric Data Factory**.

![Dataflows Gen2 UI screenshot](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-shows-a-user-interface-for-a-dataflows-g.png)

### dbt jobs enhancements (Preview)

- **GitHub support**: connect Fabric dbt jobs to dbt projects hosted in GitHub
- **Logging enhancement**: streams dbt output to **OneLake** (removes 1 MB log limit)
- **Public package support**: use community dbt packages

## Enterprise-grade orchestration: low code and pro code

Fabric Data Factory is described as supporting both low-code and pro-code orchestration, and the post claims it powers **23+ billion orchestration runs per month**.

- **Identity across pipeline activities (GA)**
  - **Service Principal (SPN)** and **Workspace Identity (WI)** support across all pipeline activities
  - Goal: standardize identity-based access and reduce credential sprawl

- **Orchestration features (Preview)**
  - **SSIS Pipeline Activity**
  - **Lakehouse Maintenance** pipeline activities
  - **Analytics SQL Endpoint refresh** pipeline activity
  - **Interval-based schedules (Tumbling Window Trigger)**
  - New **Airflow REST APIs** for pool and workspace management

## Multi-cloud data distribution

The article emphasizes expansion of **Copy job** for multi-cloud sources/destinations and more incremental/CDC scenarios.

### Incremental copy extensions (GA)

- 10+ new connectors for incremental copy, including:
  - **Google Cloud Storage**, **DB2**, **ODBC**, **Fabric Lakehouse tables/folders**, **Azure Files**
  - **SharePoint Lists**, **SharePoint Online File**
  - **Amazon RDS for SQL Server**, **Amazon RDS for Oracle**
  - **Azure Data Explorer**
- More watermark column type support
- Incremental copy with **ROWVERSION**, date, string
- Ability to specify database queries for full & incremental Copy
- Truncate destination before full copy to avoid duplicates

### Copy job enhancements (Preview)

- **SCD Type 2 (SCD2)**
- Audit columns
- Column mapping improvements for replication
- Workspace Identity & SPN support for copy job activity
- New destinations for multi-cloud distribution: **MySQL**, **PostgreSQL**
- SharePoint file-based incremental copy

## AI-powered data integration and the Fabric Data Factory MCP Server

The post describes built-in Copilot experiences and introduces a developer-focused **Model Context Protocol (MCP)** server.

### AI-powered experiences (GA)

- **Pipeline Expression Copilot**
  - Generate pipeline expressions from natural language.
  - Explain expressions in natural language to improve readability/learning.

- **AI-powered Transform with a prompt** (Dataflow Gen2)
  - Create new columns by describing the transformation in natural language.
  - Uses context from other columns/rows; positioned as “no code required”.

### Fabric Data Factory MCP server (Preview)

- Repo: [Fabric Data Factory MCP](https://github.com/microsoft/DataFactory.MCP)
- Purpose: expose Fabric Data Factory capabilities to “agentic tools” (examples listed: **GitHub Copilot**, **Claude**, and others).

![Screenshot of the Data Factory MCP server interface](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-depicts-the-microsoft-data-factory-mcp.png)

Supported capabilities called out:

- **Authentication**
  - Interactive Azure AD sign-in
  - Service principal auth
  - Session status, token retrieval, sign-out
  - Identity system: **Microsoft Entra ID**

- **Dataflow Gen2**
  - Create dataflows, manage connections, write/update **M (Power Query)** queries, save definitions
  - Trigger background refresh and monitor status
  - Execute query (Preview): run an M expression against live data sources and get results inline in chat

- **Pipeline management**
  - Create pipelines, update activity definitions, start runs
  - Monitor running status
  - Schedule recurring runs with time zone support

- **Gateway management**
  - List on-premises/personal/VNet gateways
  - Retrieve gateway details
  - Inspect gateway status/health

- **Connection management**
  - Discover supported connections
  - Create cloud, on-premises, and VNet connections

## Migration to Fabric Data Factory simplified

The post positions Fabric Data Factory as consolidating **Azure Data Factory** plus **Power Query** into a single experience in Fabric.

- Announces a **Migration Assistant for Data Factory** aimed at migrating from ADF to Fabric.
  - Migration blog: [Migration blog](https://aka.ms/MigrationBlogFabcon)

- Option to evaluate without migrating:
  - Mount existing ADF into a Fabric workspace: [Tutorial: bring Azure Data Factory to Fabric](https://learn.microsoft.com/fabric/data-factory/tutorial-bring-azure-data-factory-to-fabric)

![Animated screenshot of Azure Data Factory UI (migration assistant context)](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-shows-a-visual-representation-of-the-azu.gif)

## Learn more

- [Data Factory in Microsoft Fabric overview](https://learn.microsoft.com/fabric/data-factory/data-factory-overview)
- [Get Started with Microsoft Fabric](https://www.microsoft.com/microsoft-fabric/getting-started)
- [What’s new and planned for Data Factory in Microsoft Fabric](https://roadmap.fabric.microsoft.com/?product=datafactory)
- [Microsoft Fabric Updates Blog](https://blog.fabric.microsoft.com/blog/)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/fabric-data-factory-at-fabcon-atlanta-built-for-modern-data-integration/)

