---
layout: "post"
title: "What’s New in Fabric Warehouse – July 2025: AI, Performance, and Modern Data Engineering"
description: "This July 2025 recap details the latest Microsoft Fabric Warehouse updates, covering data ingestion, security enhancements, SQL innovations, AI and Copilot integrations, and modern developer tooling. The post highlights migration tools, Cosmos DB preview, Databricks integrations, Python notebooks, audit governance, and roadmap features for data professionals."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-fabric-warehouse-july-2025/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-08-05 11:00:00 +00:00
permalink: "/2025-08-05-Whats-New-in-Fabric-Warehouse-July-2025-AI-Performance-and-Modern-Data-Engineering.html"
categories: ["AI", "Azure", "ML", "DevOps", "Security"]
tags: ["Agentic AI", "AI", "Audit Logging", "Azure", "Azure AI Foundry", "Copilot Studio", "Cosmos DB", "Data Warehouse", "Databricks", "DevOps", "Governance", "Microsoft Fabric", "ML", "MSSQL Extension", "News", "OneLake", "Performance Tuning", "Python Notebooks", "Security", "SQL Analytics", "Synapse Migration", "Terraform", "VS Code"]
tags_normalized: ["agentic ai", "ai", "audit logging", "azure", "azure ai foundry", "copilot studio", "cosmos db", "data warehouse", "databricks", "devops", "governance", "microsoft fabric", "ml", "mssql extension", "news", "onelake", "performance tuning", "python notebooks", "security", "sql analytics", "synapse migration", "terraform", "vs code"]
---

This comprehensive recap from the Microsoft Fabric Blog details July 2025’s new features and enhancements for Fabric Warehouse, including AI integration, security, performance improvements, and developer tools, making it relevant for data engineers and architects.<!--excerpt_end-->

## What’s New in Fabric Warehouse – July 2025 Recap

**Author:** Microsoft Fabric Blog

### Introduction

The July 2025 Fabric Warehouse update highlights quality improvements, major performance enhancements, productivity boosts, and security investments that serve data professionals. Whether you’re migrating from Synapse, optimizing workloads, leveraging SQL in VS Code, or exploring new APIs, these updates cover key advancements, best practices, and future roadmap insights.

---

### 1. What’s New

#### Ingest, Load, and Sync Data

- **Fabric Data Warehouse Migration Assistant:** AI-powered migration from Synapse/SQL Server, with improved summaries, diagnostics, and fix suggestions.
- **Copy Job Enhancements:** Supports data ingestion from database views, built-in sample datasets, and connectors like MongoDB Atlas and Azure AI Search.
- **Cosmos DB Preview:** Native Cosmos DB integration brings NoSQL support to Fabric Warehouse, enabling real-time analytics and mirroring to OneLake.
- **Databricks Unity Catalog Integration (GA):** Seamless access to Databricks-managed datasets and Power BI compatibility.
- **Delta Lake as Iceberg in OneLake:** Supports automatic virtualization for better cross-platform analytics compatibility.
- **SQL Analytics Endpoint Metadata REST API (GA):** Automate metadata refresh for Lakehouse and mirrored sources.
- **COPY INTO and OPENROWSET from OneLake (Preview):** Directly ingest and query CSV/Parquet files from OneLake Lakehouse folders.

#### Query, Transform, and Optimize

- **SQL Analytics Endpoint Updates:** Case-insensitive collation, performance, reliability improvements, and private previews.
- **Inline Scalar UDFs (Preview):** Enables high-performance, modular T-SQL logic.
- **Optimizing Inlineable UDFs:** Guidance on refactoring UDFs for query speed.
- **Performance Guide:** Best practices for ingestion, indexing, concurrency, and optimization.
- **Result Set Caching (Preview):** Faster query execution via result caching.
- **Intelligent Garbage Collection:** Automated clean-up of stale/redundant data.
- **Python Notebook Support:** Use T-SQL in Python notebooks for interactive data workflows and visualization.

#### Secure, Govern, and Manage

- **Core Security Capabilities:** Entra ID, conditional access, object/column/row-level security, data masking, and compliance. Follows Zero Trust principles. Workspace private links coming soon.
- **Phasing Out Default Semantic Models:** Enhances governance, transparency, customizability.
- **Improved Creation Flows:** Streamlined setup for warehouses/lakehouses and semantic models.
- **Visual SQL Audit Logs:** UI-driven configuration with flexible retention for compliance.
- **Unified Audit Operations:** Standardized audit actions and events to improve traceability.

#### Enable AI, Copilot & Agentic Workflows

- **AI-Powered OneLake Transformations:** Summarization, sentiment analysis, and PII detection on unstructured text.
- **Expanded Data Agent Support:** Better AI-powered data and semantic modeling for large sources.
- **Data Agents & Copilot Studio:** Multi-agent orchestration and Model Context Protocols (MCP) in preview.
- **Integration with Azure AI Foundry:** Enables building of domain-specific, enterprise-ready AI agents that query across warehouses and endpoints.

#### Build, Develop & Deploy with Modern Tooling

- **Terraform Provider for Fabric (v1):** Infrastructure-as-code rollout including CLI/App registration for Fabric capacities and support for industry-leading CI/CD.
- **MSSQL Extension for VS Code:** v1.29.0 brings enhanced UI and expanded schema management, supporting local/cloud development, and preview support for Fabric Warehouse.

---

### 2. Docs Updates (July 2025)

- **What’s New in Fabric**: Central listing of features and enhancements.
- **Migration Assistant Guide:** Stepwise migration instructions, troubleshooting, and DACPAC support.
- **SQL Analytics Endpoint Metadata Sync API:** Details for programmatically refreshing SQL endpoint metadata.
- **Security Fundamentals:** Inclusion of row-level security, IP firewalls, customer-managed keys.

---

### 3. Roadmap Updates

**Upcoming features for Fabric Warehouse and SQL Endpoints include:**

- Copilot enhancements
- Custom SQL pools
- Data clustering
- IDENTITY columns
- Incremental/proactive statistics refresh
- MERGE support
- Migration Assistant improvements
- OneLake security
- Outbound access protection
- Warehouse shortcuts
- Workspace-level private links

---

### Next Steps

- **Try Microsoft Fabric for Free:** https://aka.ms/tryfabric
- **Community & Feedback:** Join the [Fabric Community](https://community.fabric.microsoft.com/) or Reddit ([/r/MicrosoftFabric](https://www.reddit.com/r/MicrosoftFabric)) to share ideas.

---

#### For full documentation and feature links, refer to the original [Microsoft Fabric Blog post](https://blog.fabric.microsoft.com/en-us/blog/whats-new-in-fabric-warehouse-july-2025/).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-fabric-warehouse-july-2025/)
