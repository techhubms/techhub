---
external_url: https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-fabric-warehouse-august-2025/
title: 'What’s New in Fabric Warehouse: August 2025 Recap'
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-09-03 10:30:00 +00:00
tags:
- AzCopy
- Azure AI Foundry
- CI/CD
- Copilot Studio
- Data Agents
- Data Ingestion
- Database Projects
- ETL
- Fabric Warehouse
- JSON Lines
- Microsoft Fabric
- NuGet
- Private Link
- Scheduler
- SQL Analytics Endpoint
- T SQL
- VS Code
- AI
- Azure
- Coding
- DevOps
- Machine Learning
- Security
- News
section_names:
- ai
- azure
- coding
- devops
- ml
- security
primary_section: ai
---
This recap by the Microsoft Fabric Blog team highlights August 2025 updates in Fabric Warehouse, including enhanced data ingestion, automation, security, and AI-driven workflows for developers and data engineers.<!--excerpt_end-->

# What’s New in Fabric Warehouse – August 2025 Recap

## Introduction

The Microsoft Fabric Blog team presents the August 2025 highlights for Fabric Warehouse, focusing on substantial product improvements despite the summer slowdown. Updates include data ingestion, security, automation, developer tooling, and the infusion of AI and agentic workflows.

## Quick Roundup of What’s New

### Ingest, Load, and Sync Data

- **JSON Lines Support in OPENROWSET**: Now in public preview, Fabric Data Warehouse and SQL Analytics Endpoint support querying external JSONL files using OPENROWSET, enabling easier analysis of semi-structured data with familiar T-SQL.
- **Copy Job Enhancements**: Features like Reset Incremental Copy and JSON Format Support provide granular ETL control and simplify workloads. Copy job activity can now be orchestrated within pipelines with enhanced visibility.
- **AzCopy and Secure Transfers**: AzCopy now supports copying data from firewall-enabled Azure Storage accounts directly into Microsoft OneLake, resolving previous networking and security obstacles for large-scale data movement.

### Query, Transform, and Optimize for Insights

- **SHOWPLAN_XML GA**: SHOWPLAN_XML set statement is now generally available, giving deep insights into T-SQL query plans for detailed performance investigation.
- **OBJECT_SCHEMA_NAME Functionality**: Mirroring SQL Server behavior, this function increases script compatibility and metadata management within Fabric Warehouse.
- **Item History Page**: Capacity admins can now analyze 30-day compute usage with new interactive visuals for better workload optimization and forecasting.

### Secure, Govern, and Manage with Confidence

- **Private Link at Workspace Level**: Preview feature empowers organizations to implement fine-grained network isolation for Fabric workspaces, restricting access via private networking and strengthening security posture.
- **Custom Semantic Model Controls**: Default semantic models are being sunset in favor of user-managed models, providing transparency, governance, and flexibility over data assets.
- **Workspace Collation Settings**: Enables consistent migrations and easier data management, with enhanced control over collation settings at both workspace and item levels.

### Enable AI, Copilot & Agentic Workflows

- **Copilot & Data Agent Integration**: Warehouses now offer AI-powered Copilot assistance and data agent entry directly from the SQL editor, simplifying schema building, SQL generation, and contextual insights.
- **Multi-Agent Orchestration with Copilot Studio**: Data agents connect with Copilot Studio for richer, collaborative responses powered by Model Context Protocols (MCP).
- **Azure AI Foundry Integration**: Fabric Data Agents integrate with Azure AI Foundry, allowing organizations to build tailor-made, domain-specific AI agents grounded in enterprise data.

### Build, Develop & Deploy with Modern Tooling

- **SqlPackage & SDK-Style Database Projects**: Scalar Functions are now supported, and developers can import/export functions from VS Code and other client tools for richer CI/CD scenarios.
- **Multiple Scheduler & CI/CD Support**: Multiple schedulers per T-SQL notebook and built-in CI/CD integration promote automation, versioning, and operational efficiency.

## Documentation Updates

- **Performance Guidelines**: Updated best practices for Fabric Data Warehouse, including ingestion, table design, query optimization, and workload scaling.
- **Security Fundamentals**: Documentation reflects new features like workspace-level Private Link.

## Next Steps

Look forward to major announcements at FabCon Vienna and continued innovation in Fabric Warehouse. Feedback is welcomed at <https://aka.ms/fabricideas>.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-fabric-warehouse-august-2025/)
