---
external_url: https://blog.fabric.microsoft.com/en-US/blog/august-2025-fabric-feature-summary/
title: 'August 2025 Microsoft Fabric Feature Summary: Data Engineering, ML, and Platform Enhancements'
author: Microsoft Fabric Blog
viewing_mode: external
feed_name: Microsoft Fabric Blog
date: 2025-08-27 09:14:00 +00:00
tags:
- Apache Spark
- Audit Logging
- Autoscale Billing
- Azure DevOps
- Copilot
- Data Agent
- Data Engineering
- Data Factory
- Data Science
- Dataflow Gen2
- Deployment Pipelines
- Event Schema Registry
- Fabric APIs
- Livy API
- Microsoft Fabric
- OneLake
- OpenAPI
- Python Notebooks
- Real Time Intelligence
- REST API
- Service Principal
- SQL Analytics
section_names:
- ai
- azure
- coding
- devops
- ml
---
Microsoft Fabric Blog highlights the August 2025 Fabric Feature Summary, revealing core updates for developers and engineers, including new deployment pipeline features, Spark improvements, machine learning endpoints, Python Notebook integrations, and streamlined data engineering led by Microsoft.<!--excerpt_end-->

# August 2025 Microsoft Fabric Feature Summary

_Authored by Microsoft Fabric Blog_

## Overview

The August 2025 Microsoft Fabric Feature Summary introduces a host of new features and upgrades designed to streamline data workflows and boost productivity for developers, engineers, and data scientists on the Fabric platform. Updates span deployment pipelines, APIs, DevOps integration, Spark and ML workloads, SQL analytics, Data Factory, and more.

---

## Highlights

### Platform & Deployment

- **Flat List View in Deployment Pipelines:**
  - Intuitive management with enhanced workspace navigation and a new location column.
  - Persistent view selection across stages.
  - [Documentation](https://review.learn.microsoft.com/fabric/cicd/deployment-pipelines/deploy-content?branch=main&branchFallbackFrom=pr-en-us-9645&tabs=new-ui#flat-list-view)
- **Microsoft Fabric APIs Specification:**
  - Official [fabric-rest-api-specs](https://github.com/microsoft/fabric-rest-api-specs) repository for public REST API specs and OpenAPI documentation for custom integrations.

### DevOps & Automation

- **Service Principal and Cross-Tenant Azure DevOps Integration (Preview):**
  - Automate workspace setup with Fabric CLI and Terraform across tenants.
  - Connect Fabric to Azure DevOps repositories using service principals.
  - [Automate Git Integration](https://learn.microsoft.com/fabric/cicd/git-integration/git-automation)

### Data Engineering & Apache Spark

- **Autoscale Billing for Spark:**
  - Serverless, pay-as-you-go Spark compute, decoupled from Fabric capacity.
- **Job Bursting Control:**
  - Admins can toggle Spark bursting for balance between high throughput and fair resource distribution.
- **Livy API (GA):**
  - Run Spark code via REST, enabling remote, automated Spark workloads.
- **JobInsight Diagnostics Library:**
  - In-depth analysis and insight generation for Spark jobs, including log management and troubleshooting.
- **Enhanced High-Concurrency Monitoring:**
  - Improved debugging and visibility for collaborative Notebook jobs.
- **User Data Functions in Notebooks:**
  - Native Pandas DataFrame and Series support for high-performance analytics, via Apache Arrow.
  - Real-time Notebook snapshots for live debugging.
- **OpenAPI Spec Generation and Testing for UDFs (Preview):**
  - Simplified integration and automated testing before publishing UDFs.

### Data Science & ML

- **Serve Real-Time ML Predictions (Preview):**
  - Secure, online model endpoints with REST API support and scalable deployment.
  - One-click test and deploy for ML workflows.
- **Data Agent Enhancements:**
  - Transparency into query examples influencing agent responses.
  - Downloadable diagnostics for run steps, support for larger data sources.

### Warehouse, Databases & Analytics

- **SQL Analytics Endpoint Sync/Metadata API (GA):**
  - Programmatic endpoint refresh for lakehouse and database changes via new REST API.
- **COPY INTO and OPENROWSET Enhancements:**
  - Ingest and query OneLake files (CSV, Parquet, JSONL) natively from SQL.
- **SQL Audit Log Visualization:**
  - UI-driven, granular audit policy configuration with extended retention.
- **SHOWPLAN_XML Statement (GA):**
  - Advanced plan visualization for query performance tuning.
- **Python Notebooks for SQL:**
  - Bidirectional read/write integration with Fabric SQL databases from Notebooks.

### Real-Time Intelligence & Event Processing

- **Activator Analytics & Queryset Schema Controls:**
  - Enhanced event analysis, schema navigation, and improved sharing/collaboration experience.
- **Event Schema Registry (Preview):**
  - Centrally manage event schemas for type-safe, event-driven pipelines.
- **Eventhouse Accelerated Shortcuts:**
  - Improved management of data freshness via MaxAge and HotWindows settings.

### Data Factory & Pipelines

- **Pipeline Trigger Management:**
  - New UI panel for robust trigger viewing and management.
- **Terminology Simplification:**
  - 'Data pipelines' now displayed as 'pipelines.'
- **Incremental Copy Reset & Auto Table Creation:**
  - Simplified incremental syncing and destination schema provisioning.
- **CI/CD Dataflow Gen2 Creation:**
  - Seamless conversion to CI/CD dataflows and enhanced validation monitoring.
- **Copilot Integration:**
  - Copilot-powered data source discovery and guided setup in Dataflow Gen2 workflows.

---

## Further Resources

For complete release notes, demos, and documentation, visit:

- [Microsoft Fabric Blog](https://blog.fabric.microsoft.com/en-us/blog/august-2025-fabric-feature-summary/)
- [Fabric Documentation](https://learn.microsoft.com/fabric/)

---

Explore how these updates can help you modernize your Microsoft Fabric data estate, optimize workloads, and enable secure, scalable analytics and ML solutions.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/august-2025-fabric-feature-summary/)
