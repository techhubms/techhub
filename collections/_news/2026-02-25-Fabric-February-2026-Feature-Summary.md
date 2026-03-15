---
external_url: https://blog.fabric.microsoft.com/en-US/blog/fabric-february-2026-feature-summary/
title: Fabric February 2026 Feature Summary
author: Microsoft Fabric Blog
primary_section: ai
feed_name: Microsoft Fabric Blog
date: 2026-02-25 10:00:45 +00:00
tags:
- Adaptive Performance Tuning
- AI
- Azure
- CI/CD
- Customer Managed Key
- Data Engineering
- Data Factory
- Data Science
- Data Warehouse
- DevOps
- Entra ID Authentication
- Eventstream Connectors
- Git Integration
- GraphQL API
- Lakehouse
- Microsoft Fabric
- Migration Assistant
- ML
- Modern Evaluator
- News
- ODBC Driver
- OneLake Catalog
- Power BI
- Private Link
- Python Notebooks
- Real Time Intelligence
- Semantic Link
- SQL Pool Insights
- VS Code Extension
- .NET
section_names:
- ai
- azure
- dotnet
- devops
- ml
---
Microsoft Fabric Blog presents the February 2026 feature summary, authored by the Fabric engineering team. This update introduces major developer, data engineering, AI/ML, and platform improvements for the Fabric analytics ecosystem.<!--excerpt_end-->

# Fabric February 2026 Feature Summary

The Microsoft Fabric Blog's February 2026 release introduces significant platform-wide improvements for data and analytics professionals using Microsoft Fabric.

## Highlights

- **OneLake Catalog Enhancements:**
  - Workspace Apps integration
  - Streamlined item details and unified design
  - Centralization of business-ready and technical content discovery
  - Improved organization and metadata for items

- **Developer Experience Improvements:**
  - New horizontal tab display settings for better multitasking
  - Deep VS Code extension integration (item browsing, editing, and Fabric MCP server support)
  - Enhanced integration with Git, deployment pipelines, source control, and CI/CD flows
  - Modern Evaluator for Dataflow Gen2 (now GA; .NET 8 powered)

- **Data Engineering Updates:**
  - Notebook version history from multiple sources (UI, Git, CI/CD, VS Code)
  - Python notebooks gain `%run` support for modular development
  - Full-size editing mode for complex or long notebook cells
  - Microsoft ODBC Driver for Fabric Data Engineering (Preview)
  - Tenant-level identity limit controls for governance
  - Customer Managed Key encryption for notebooks (Azure Key Vault integration)

- **Data Science and AI Features:**
  - Semantic Link 0.13.0 with extended workspace, model, and API management
  - Real-time scoring endpoint monitoring
  - Fabric MCP server and GitHub Copilot Chat available in VS Code for AI and analytics development

- **Data Warehouse and Migration:**
  - Export migration summaries (Excel, CSV)
  - SQL Pool Insights with pool-level telemetry and new monitoring views

- **Real-Time Intelligence (RTI):**
  - Unified data connection experience
  - Streaming connectors for private networks (vNet, on-prem, hybrid)
  - Dramatically improved dashboard performance for large datasets and live visuals

- **Data Factory Enhancements:**
  - 'Recent Data' module for fast, frequent data access in Dataflow Gen2
  - Improved integration and unlimited variables in variable libraries
  - Relative references for easier CI/CD with Fabric connectors
  - Just-in-time publishing for simplified refreshes and deployments
  - Connector expansion (broad SaaS, cloud, and on-prem support)
  - Incremental copy improvements (CDF, watermark, RowVersion for SQL)
  - Service Principal and workspace identity authentication added to Copy jobs
  - Parallel read support for large CSV datasets
  - Adaptive Performance Tuning for intelligent optimization

## Security and Governance

- **Customer Managed Keys for workspace encryption**
- **Private Link support for Fabric's GraphQL API**
- **Entra ID authentication support**
- Enhanced compliance and admin controls

## Full Details

The update spans rich, actionable details for engineers, architects, and data professionals:

- [Workspace Apps in OneLake Catalog](https://learn.microsoft.com/fabric/governance/onelake-catalog-overview)
- [Notebook Version History](https://learn.microsoft.com/fabric/data-engineering/how-to-use-notebook#version-history)
- [Python `%run` support in Notebooks](https://learn.microsoft.com/fabric/data-engineering/author-execute-notebook#reference-run-a-notebook)
- [Private Link and CI/CD for GraphQL API](https://aka.ms/graphql-privatelink)
- [Semantic Link 0.13.0](https://pypi.org/project/semantic-link/0.13.0/#description)
- [ODBC Driver for Fabric](https://learn.microsoft.com/fabric/data-engineering/spark-odbc-driver)
- [Modern Evaluator in Dataflow Gen2](https://learn.microsoft.com/fabric/data-factory/dataflow-gen2-modern-evaluator)
- [Adaptive Performance Tuning](https://learn.microsoft.com/fabric/data-factory/copy-data-activity#configure-your-other-settings-under-settings-tab)
- ...and much more throughout the Data Engineering, Data Science, Data Warehousing, Data Factory, and security sections.

The release is especially relevant for those building or operating analytics platforms, automating data pipelines, implementing ML and AI solutions, or managing Fabric-based infrastructure in the enterprise.

> For full demos, documentation, and links to each new feature, see the original [February 2026 update announcement](https://blog.fabric.microsoft.com/en-us/blog/fabric-february-2026-feature-summary/).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/fabric-february-2026-feature-summary/)
