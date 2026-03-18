---
title: 'From Lakehouse to boardroom: Analytics and AI for real insights'
primary_section: github-copilot
tags:
- ADO.NET Driver
- Agent Skills For Fabric
- AI
- Apache Spark
- AutoML in Fabric
- Custom Live Pools
- Custom SQL Pools
- DacFx
- Data Freshness SLO
- Delta Lake
- Delta Lake 4.0
- Deployment Pipelines
- Direct Lake
- Eventhouse
- Fabric Analytics
- Fabric Data Agents
- Fabric Data Engineering
- Fabric Data Warehouse
- Git Integration
- GitHub Copilot
- GitHub Copilot CLI
- Incremental Statistics Refresh
- JDBC
- KQL
- Lakehouse
- Managed Identity
- Materialized Lake Views
- Medallion Architecture
- MERGE
- Microsoft Entra ID
- Microsoft Fabric
- Microsoft Purview
- ML
- MLflow
- Multimodal AI Functions
- Native Execution Engine
- Natural Language Querying
- News
- OneLake
- Outbound Access Protection
- Pandas
- Parquet
- Power BI
- Proactive Statistics Refresh
- PySpark
- Resource Profiles
- Runtime 2.0
- Security
- Semantic Model
- Service Principal
- Spark 4.0
- Spark ODBC Driver
- SQL Analytics Endpoint
- T SQL
- TMDL
- Translytical Task Flows
- UDF
date: 2026-03-18 05:45:25 +00:00
author: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/from-lakehouse-to-boardroom-analytics-and-ai-for-real-insights/
section_names:
- ai
- github-copilot
- ml
- security
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog outlines what’s shipping across Fabric Analytics—Spark performance work, Fabric Data Warehouse updates, Power BI semantic modeling improvements, and Fabric Data Agents—plus a new open-source “Agent Skills for Fabric” integration for GitHub Copilot CLI.<!--excerpt_end-->

# From Lakehouse to boardroom: Analytics and AI for real insights

*If you haven’t already, check out Arun Ulag’s blog for a full roundup of FabCon/SQLCon announcements across Fabric and database offerings:* [FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news)

This post gives an end-to-end view of what’s shipping in **Microsoft Fabric Analytics**, with an emphasis on:

- Building an **AI-ready analytics stack** (ingestion → transformation → semantic model → AI experiences)
- Improving **price × performance** across the stack
- Using **agents** and **natural language** to get governed answers where people work (including Microsoft 365)

## What’s new across the Fabric Analytics stack

## Data Engineering

### Speed, scale, and price × performance

Fabric Data Engineering focuses on performance on open formats like **Delta** and **Parquet**, aiming to reduce downstream latency and cost.

Key points:

- Fabric Data Engineering uses **Apache Spark**, with additional Microsoft work and open-source contributions.
- A **Native Execution Engine** provides lower latency for Parquet and Delta workloads with **vectorized execution**.

### Performance for every format

Updates and capabilities called out:

- **Native Execution Engine**: *up to 6× performance boost vs OSS Spark*, with no code changes.
- Acceleration is being extended to more data types (starting with **vectorized CSV parsing**, with plans to extend to **JSON** and **Spark Streaming**).
- **Z-order** and **Liquid Clustering** optimizations supported for reads and writes.
- **Parallel snapshot loading** reduces Delta metadata read time for wide tables with many files/partitions.

![Bar chart comparing TPC-DS 1TB performance results of Apache Spark, Fabric Spark, and Fabric Spark with Native Engine. Fabric Spark shows a 2x speed improvement over Apache Spark, while Fabric Spark with Native Engine achieves a 6x speed increase, highlighted by arrows and labeled multipliers. AI-generated content may be incorrect.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/bar-chart-comparing-tpc-ds-1tb-performance-results.png)

*Figure 1: Price performance of Native execution engine for Fabric Data Engineering compared against OSS Spark.*

### Runtime and compute ergonomics

Runtime and operational improvements:

- **Runtime 2.0** (Preview): brings **Spark 4.0** and **Delta Lake 4.0**.
  - Spark 4.0 includes query planning improvements.
  - Delta Lake 4.0 introduces features like **variant data types**.
- **Resource Profiles**: express job intent and get a recommended set of Spark configurations.
- **Custom Live Pools** (rolling out to Preview): workspace admins can create warm compute pools (node size/count) to reduce startup latency.

![Screenshot of a software interface showing Spark compute configuration settings within a data analytics platform. It displays environment pool selection, node family and size details, compute pool activation toggle, and options for adjusting Spark driver core and memory properties.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-software-interface-showing-spark-c-scaled.gif)

*Figure 2: The new Resource Profiles capability sets you up with the recommended set of Spark Configurations.*

### Materialized Lake Views (GA)

**Materialized Lake Views (MLVs)** are now **Generally Available**. They support:

- Pre-computed views in the lakehouse
- Incremental updates
- Data quality constraints
- Medallion architecture enablement

Additional details mentioned:

- Broader clause support for incremental refresh
- Multi-schedules in a single lakehouse
- In-place updates
- PySpark authoring
- Stronger data quality enforcement

![Screenshot of a data workflow diagram in a software interface showing interconnected nodes representing datasets and processes related to materialized lake views. Nodes are labeled with dataset names and sources, connected by arrows indicating data flow, with a sidebar listing data sources and folders, and a top menu for managing runs and reports.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-data-workflow-diagram-in-a-softwar.gif)

*Figure 3: Materialized lake views make it easier to implement medallion architecture on Fabric and make your pipelines production ready.*

### Connectivity drivers

- **Spark ODBC and ADO.NET drivers** (Preview)
- **JDBC drivers** are GA
- Auth modes include **Azure AD**, **service principals**, and **managed identities**

## AI-assisted engineering

### Copilot in Data Engineering / Data Science (Preview)

The post describes improvements to Copilot for notebook-based work:

- Context-aware by default (understands notebooks, data, environment)
- Helps teams write/understand/debug/optimize notebooks
- Can reference workspace context across Fabric
- Improved **VS Code** experience via a Data Engineering extension

### Data Science / ML features

- **AutoML in Fabric** (GA): automated model selection, feature engineering, hyperparameter tuning; integrated with notebooks, experiments, and **MLflow tracking**.
- **Multimodal AI Functions** (Preview): extends built-in AI Functions beyond text to support images and PDFs; supports AI-powered transformations from **pandas** or **PySpark**.

## Data Warehouse

### Custom SQL Pools (Preview)

**Custom SQL Pools** allow:

- User-defined, isolated compute pools
- Separate pools for ETL, reporting, ad-hoc queries
- **Physical resource isolation** so concurrent workloads don’t interfere

Architecture described:

- Control flow separated from physical execution
- A single SQL front-end routes to pools based on configuration

![Architecture of Custom SQL Pools. Image shows the separate control flow from physical execution.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/architecture-of-custom-sql-pools-image-shows-the.png)

*Figure 4: Custom SQL Pools offer predictable performance at scale.*

### Freshness improvements

The post highlights:

- A new metadata sync for SQL analytics endpoints targeting a **30-second SLO for data freshness** once delta logs are available.

Also becoming GA:

- **Proactive Statistics Refresh**: maintains optimizer stats immediately after data changes.
- **Incremental Statistics Refresh**: updates stats incrementally for large tables.

### Built-in AI functions in T-SQL

New AI functions described as built directly into T-SQL:

- `AI_ANALYZE_SENTIMENT`
- `AI_CLASSIFY`
- `AI_EXTRACT`
- `AI_GENERATE_RESPONSE`
- `AI_SUMMARIZE`
- `AI_TRANSLATE`
- `AI_FIX_GRAMMAR`

Goal: invoke AI capabilities without moving data or standing up separate services.

![Screenshot of an SQL query and its results displaying hotel reviews data, including city, name, review rating, sentiment analysis, category, review date, opinion, and AI-generated polite replies. The query extracts and classifies review text, showing sentiment as positive or negative and providing specific AI responses for each review entry.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-an-sql-query-and-its-results-display.png)

*Figure 5: Built-in AI functions are now directly available in T-SQL.*

### Rules and actions on query results

The post describes creating alerts and actions based on query results, e.g.:

- Send a Teams message when KPIs are out of bounds
- Email ops when a pipeline produces extreme skew

![The image displays a user interface for an SQL query builder or visualization tool, with various options such as reporting, management, and help features. It includes a sample query to calculate total population by city and state, with steps to filter and sort the results. AI-generated content may be incorrect.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-displays-a-user-interface-for-an-sql-que.gif)

*Figure 6: Create rules on SQL query results to detect data issues, monitor KPIs, and automatically trigger alerts or Fabric workflows.*

### SQL fundamentals and enterprise features

Items called out:

- `MERGE` is now **GA**.
- **DacFx integration in web experiences** for Git-based schema management.
- **Cross-warehouse reference support** for dependency-aware development and correct pipeline ordering.
- **Migration Assistant**: live connectivity to sources (e.g., **Azure Synapse**) without DACPAC extraction.
- A new **Monitoring UX**, including:
  - Query comparison across executions
  - Query text visibility
  - SQL pool names per query
  - Live running queries
  - SQL Pool Insights to understand pool pressure
- Additional GA items listed:
  - SQL Audit Logs (Fabric Data Warehouse and SQL Endpoint)
  - Outbound Access Protection
  - `COPY INTO` and `OPENROWSET`
  - **SSMS 22.5.0 integration**

![Screenshot of a monitoring dashboard displaying a line graph and a table of query runs with request counts over time. The graph shows spikes in requests on specific dates, while the table lists query IDs, texts, submission timestamps, durations, and success statuses, with green "Succeeded" labels indicating completed queries.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-monitoring-dashboard-displaying-a-scaled.gif)

*Figure 7: A new Monitoring UX provides a one-stop shop for live and completed queries with performance comparison across executions.*

## Power BI

### Semantic layer focus

The post argues that AI systems need curated semantic context (measures, relationships, hierarchies, time intelligence, definitions), and positions Power BI as the layer that provides that semantic grounding.

### Translytical Task Flows (GA)

**Translytical Task Flows** are **Generally Available**, enabling actions directly from reports:

- Add/update/delete data
- Trigger workflows in other systems

### Report Copilot for mobile

A mobile experience for asking questions about data using voice or text in the Power BI mobile app.

![Screenshot of a sales report dashboard for Fabrikam Company displaying key metrics including sales, sales orders, average order value, units sold, margin, and 50% margin percentage. Includes a line graph showing sales over time from 2021 to 2024.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-sales-report-dashboard-for-fabrika.gif)

*Figure 8: Ask questions about your data using voice or text in the Power BI mobile app and get instant answers or visuals.*

### TMDL View on the web

Power BI web now supports viewing/editing model code directly using **TMDL (Tabular Model Definition Language)**.

![Screenshot of a software interface displaying a code editor with DAX formulas for calculating sales profit margins and total sales, highlighted with red boxes around specific variables like "displayFolder." The interface includes tabs for formatting, data, and model views, with a sidebar showing tables and model elements, emphasizing code structure and semantic roles for data analysis.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-software-interface-displaying-a-co.png)

*Figure 9: TMDL View allows you to view and edit your data model’s code directly in the Power BI web interface.*

### Direct Lake over OneLake (GA)

**Direct Lake over native Delta tables** is now GA, letting Power BI query OneLake data in Delta format without duplication/import steps, aimed at near-real-time analytics and faster access to large datasets.

## AI & Data Agents

### Fabric Data Agents (GA)

Fabric Data Agents are described as “last mile” components that:

- Sit on semantic models and OneLake data
- Use semantic context (measures/relationships) to answer questions
- Expose insights via **natural language** in **M365 Copilot**

Capabilities mentioned:

- Works across various data sources (Lakehouse, Warehouse, Semantic Models, Eventhouse, SQL Databases, etc.)
- Flexible configuration: agent-level and data source–specific instructions + custom example queries
- Sharing/publishing and lifecycle management, including diagnostics, **Git integration**, and **deployment pipelines** (ALM)

![Diagram illustrating Fabric data agents architecture, showing data flow from various consumer applications like Fabric, Foundry, Copilot Studio, Teams, MCP Server, M365, and Endpoint through a central Data Agent to OneLake. OneLake connects to multiple data storage and processing components including Lakehouse, Warehouse, Semantic Model, Eventhouse, Mirrored DB, SQL DB, AI Search, Ontology, and Graph, with color-coded sections and labeled icons representing each element.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/diagram-illustrating-fabric-data-agents-architectu.png)

*Figure 10: Fabric Data Agents can reason over data in OneLake, support deeper analysis, and deliver insights.*

### Security and governance for Data Agents

Enhancements described:

- Integration with **Microsoft Purview** for auditing, eDiscovery, data lifecycle management, communications compliance, and classification by capturing prompt/response telemetry and user context.
- Expansion of **Outbound Access Protection** to help prevent sensitive data exfiltration.

### Source enhancements for Data Agents

- **Graph** as a data source for modeling complex relationships and using them for AI-powered insights.
- Support for **KQL User Defined Functions (UDFs)** for Eventhouse/KQL-backed sources to generate efficient, secure queries from natural language.

![Screenshot of a software interface showing a data source description and instructions form within a RetailExpert application. The layout includes a left sidebar with navigation options, a main content area with text input fields for data source details, and a right panel for testing agent responses with sample questions.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-software-interface-showing-a-data.gif)

*Figure 11: Fabric data agents now support Graph as a data source.*

## The end-to-end vision (Lakehouse → boardroom)

The post describes a unified pipeline:

- **Spark**: ingestion/transformation with vectorized execution, warm pools, auto-tuned configs.
- **Fabric Data Warehouse**: isolated SQL pools, freshness SLOs, AI functions in T-SQL, Git-based CI/CD.
- **Power BI**: semantic layer that makes AI trustworthy.
- **Data Agents**: deliver semantic answers via natural language in M365 Copilot, governed by Purview, secured with outbound access protection.

Everything runs on **OneLake** over open formats with the goal of reducing data movement and keeping consistent governance and security.

## New developer tools: Agent Skills for Fabric

### Agent Skills for Fabric in GitHub Copilot CLI

The post announces **Agent Skills for Fabric** for **GitHub Copilot CLI**:

- Repo: [Agent Skills for Fabric in GitHub Copilot CLI](https://github.com/microsoft/skills-for-fabric)
- Described as open-source plugins that let you use natural language to interact with Fabric end-to-end.
- Example prompts include:
  - “Document my workspace”
  - “Demo NYC Taxi Trip data is available here https://www.nyc.gov/site/tlc/about/tlc-trip-record-data.page. Create a Fabric medallion architecture project for all trips in 2019”

The skills are described as specialized for:

- Spark authoring/consumption
- SQL warehouse authoring/consumption
- Eventhouse authoring/consumption
- Power BI semantic model interaction
- End-to-end medallion architecture orchestration

More info: [Agent Skills for Fabric GitHub repo](https://aka.ms/skills-for-fabric)

![Screenshot of a Windows PowerShell terminal displaying a prompt ready for user input. Text includes creating a Fabric workspace, lakehouse, Spark environment, and writing a Delta table.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-windows-powershell-terminal-displa.gif)

*Figure 12: Windows PowerShell terminal displaying a prompt ready for user input.*

## Getting started (FabCon Atlanta)

The post encourages readers to:

- Attend sessions (deep dives on Data Agents, Data Engineering, Warehousing, Power BI, AI in Fabric): [FabCon agenda](https://fabriccon.com/program/agenda)
- Try features (Custom Live Pools, Custom SQL Pools, Data Agents GA, Agent Skills for Fabric)
- Connect with engineers at “Ask the Experts” booths


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/from-lakehouse-to-boardroom-analytics-and-ai-for-real-insights/)

