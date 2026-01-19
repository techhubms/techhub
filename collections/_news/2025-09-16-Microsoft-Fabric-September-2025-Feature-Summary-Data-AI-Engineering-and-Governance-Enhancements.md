---
layout: post
title: 'Microsoft Fabric September 2025 Feature Summary: Data, AI, Engineering and Governance Enhancements'
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/september-2025-fabric-feature-summary/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-09-16 00:00:00 +00:00
permalink: /ai/news/Microsoft-Fabric-September-2025-Feature-Summary-Data-AI-Engineering-and-Governance-Enhancements
tags:
- Apache Airflow
- CI/CD
- CLI
- Copilot
- Data Agent
- Data Engineering
- Data Governance
- Data Loss Prevention
- Data Wrangler
- Databricks
- Dataflow Gen2
- Entra ID
- Extensibility Toolkit
- Governance APIs
- Lakehouse
- Materialized Lake Views
- Microsoft Fabric
- Mirroring
- Notebook
- OneLake
- Open Source
- Pipelines
- Purview
- Python
- Spark
- Terraform
- VS Code Extension
- Workload Assignment
section_names:
- ai
- azure
- coding
- devops
- ml
- security
---
Compiled by the Microsoft Fabric Blog team, this detailed feature summary explores September 2025 advancements, including new AI, ML, security, and developer capabilities to empower data professionals.<!--excerpt_end-->

# Microsoft Fabric September 2025 Feature Summary

## Overview

This release provides a sweeping upgrade to the Microsoft Fabric platform, including data engineering, governance, AI/ML, developer tooling, and security best practices. Below are the key new features and enhancements covered.

## Certification, Events, and Community

- Special discounted certification exams for Analytics and Data Engineers at FabCon Vienna.
- Power BI DataViz World Championships announced.

## Fabric Platform: Engineering, Management & Governance

- **Parent-Child Hierarchy in OneLake Catalog:** More intuitive navigation and understanding of relationships among Lakehouses, Warehouses, and Eventhouses.
- **Govern Tab & Domains Public APIs:** General availability of data governance tools, including API-driven management of domains for robust access controls and compliance.
- **Microsoft Purview Integration:** Automated sensitivity labels, Data Loss Prevention (DLP) policies, and granular domain-level data protection.
- **Deployment Pipeline Upgrades:** New design for deployment management, with improved variable library integration.
- **Terraform Provider Extensions:** Expanded resource coverage for infrastructure-as-code, supporting standardized and automated deployments.
- **Fabric CLI Open Sourced:** The Fabric CLI has opened to community development, supporting automation and AI-driven data estate management.
- **Fabric Extensibility Toolkit (Preview):** Accelerates creation of custom data apps and integrations.
- **Workspace-level Workload Assignment:** Granular control over workloads at the workspace level.

## Data Engineering

- **User Data Functions (Python):** Now GA with features for business logic processing, pandas/async support, OpenAPI generation, and native integration with Notebooks and Pipelines.
- **Python Notebook Enhancements:** Multiple kernels, advanced Intellisense with Pylance, lakehouse integration, pandas/Polars/Scikit-learn support, semantic links, resource monitoring, multi-source Git/version checkpoints.
- **Materialized Lake Views Improvements:** Optimized refresh, lineage tracking, custom environments, on-demand execution.
- **NotebookUtils API:** CRUD programmatic management and high-performance file operations.
- **Spark Monitoring API and Analysis Tools:** Deep resource metrics, performance tuning, outlier detection for Spark jobs.

## Data Science & ML

- **Data Agent Integrations:** NL2SQL, mirrored databases, and Python SDK provided.
- **LLM-Driven Feedback:** New evaluation tools for example queries and transparency features.
- **Data Wrangler AI:** Apply AI-driven enrichment, code generation, and translation with Copilot; PROSE-based smart suggestions enable low/no-code transformations.

## Data Warehousing/Databases

- **MERGE Transact-SQL:** Complex table operations streamlined for ETL jobs.
- **Migration Assistant:** AI-driven migration assistant for moving from Synapse SQL Pool/SQL Server to Fabric Data Warehouse.
- **MSSQL Extension for VS Code:** Full-provisioning and in-editor development (Preview).
- **Point-in-Time Restore Upgrades:** Recovery window extended to 35 days for more robust operational recovery.
- **Git Integration:** System object references and shared query tracking enabled for collaborative development.
- **Performance Dashboard:** Now tracks memory usage in addition to existing metrics.

## Real-Time Intelligence

- **Maps in Fabric:** Integrated geospatial visualization tools.
- **Azure Monitor Logs:** Native, real-time streaming of Azure diagnostics into Fabric analytics via Eventstream.
- **Eventstream Security:** Workspace Private Link ensures secure, private data ingestion; anomaly detection and scaled-up Activator for 10,000 events/sec.

## Data Integration & Orchestration (Dataflow Gen2, Pipelines, Mirroring, Gateways)

- **Pipelines:** Simplified terminology, enhanced scheduling, and improved Dataflow/Pipeline integration.
- **Dataflow Gen2:** GA for incremental refresh to Lakehouse; CI/CD/param support; Copilot enhancements for code generation and explanation; improved partitioning and modern evaluation engine; extended support for public parameters and new connectors.
- **Pipelines Activities:** Improved integration for scheduling, debugging, and invoking pipelines; full support for On-premises/VNet gateways for secure orchestration.
- **Copy job and Orchestration:** Support for CDC, incremental copy, parameterization, and new connectors; automation with the variable library.
- **Mirroring:** Continuous replication from Google BigQuery, Oracle, Azure SQL MI/DB with firewall and workspace identity support.
- **Gateways:** Secure data movement with VNET Data Gateway for pipeline/copy job.

## Developer Tooling and Extensibility

- **VS Code Extension (GA):** Multi-workspace management, Git source control, integrated SQL access.
- **Open Source Integrations:** Fabric CLI, Extensibility Toolkit, and robust API surface enable script-driven development and deployment.

## Security & Compliance

- **Microsoft Purview:** DLP, default sensitivity labels, and automated security enforcement.
- **OneLake Secure Tab:** Unified view of security roles and user access.
- **Workspace Private Link:** VNET integration secures connections between data sources and Fabric.

## More

- Enhanced connection and role management in connectors for SAP, Salesforce, Snowflake, DB2, and others.
- Support for advanced schema mapping and delta tables.
- General usability improvements, debugging support, new pricing for Dataflow Gen2, and expanded REST APIs

## References

- Documentation embedded in each update section. For community and deeper technical details, refer to the [Microsoft Fabric documentation](https://learn.microsoft.com/fabric/).

---

For specific details, usage guidance, or new feature walkthroughs, refer to the individual sections and follow related documentation links.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/september-2025-fabric-feature-summary/)
