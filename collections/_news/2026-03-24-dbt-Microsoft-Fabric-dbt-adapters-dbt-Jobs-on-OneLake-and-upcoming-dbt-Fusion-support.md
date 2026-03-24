---
feed_name: Microsoft Fabric Blog
author: Microsoft Fabric Blog
primary_section: ml
date: 2026-03-24 13:00:00 +00:00
external_url: https://blog.fabric.microsoft.com/en-US/blog/dbt-microsoft-fabric-a-strategic-investment-in-the-modern-analytics-stack/
title: 'dbt + Microsoft Fabric: dbt adapters, dbt Jobs on OneLake, and upcoming dbt Fusion support'
section_names:
- azure
- devops
- ml
tags:
- Analytics Engineering
- API Automation
- Azure
- CI/CD
- Data Warehouse
- Dbt
- Dbt Core Adapter
- Dbt Fusion
- Dbt Jobs
- DevOps
- Execution Logs
- Fabric Lakehouse
- Fabric Pipelines
- Fabric Warehouse
- GitHub Integration
- Governance
- Job Scheduling
- Lakehouse
- Microsoft Fabric
- ML
- News
- Observability
- OneLake
- SQL Transformations
---

The Microsoft Fabric Blog (co-authored by Abhishek Narain) outlines why dbt + Microsoft Fabric matters for modern analytics engineering, covering Fabric Warehouse/Lakehouse dbt adapters, dbt Jobs features (including GitHub integration and OneLake logging), and plans for dbt Fusion support in Fabric in Q2 2026.<!--excerpt_end-->

# dbt + Microsoft Fabric: A strategic investment in the modern analytics stack

*If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a broader set of announcements:* [FabCon and SQLCon 2026](https://aka.ms/FabCon-SQLCon-2026-news)

**Author:** Microsoft Fabric Blog  
**Co-author:** Abhishek Narain

## Why dbt + Microsoft Fabric matters

Modern analytics teams are adopting:

- Open, **SQL-first** data transformation
- Robust **CI/CD** and governance
- Seamless integration across **lakehouse** and **warehouse** platforms

**dbt** is positioned as a standard for analytics engineering, while **Microsoft Fabric** aims to unify data engineering, data science, warehousing, and BI through **OneLake**.

Microsoft frames its investment in **dbt + Fabric integration** as enabling “native dbt workflows” on Fabric.

![Diagram illustrating dbt as a standard for SQL data transformation, showing a workflow from raw data through development, testing, deployment, and datasets integration with BI tools, ML models, and operational analytics. Key metrics highlight over 100,000 active community members and 60,000 teams transforming data, with core capabilities including modular SQL pipelines, automated data quality tests, and enterprise-scale data governance.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/diagram-illustrating-dbt-as-a-standard-for-sql-dat.png)

*Figure 1: dbt-sql-transformations.*

## Adapter investments across the analytics surface area

The post emphasizes adapter *depth* (not just availability). In this context, adapters define:

- Performance characteristics
- Supported SQL semantics
- Materialization behavior

Microsoft highlights investment in **dbt core adapters** aligned with Fabric workloads:

- **Fabric Warehouse dbt core adapter**: intended to enable SQL-first analytics engineering on enterprise warehouses
- **Coming soon:** **Fabric Lakehouse dbt core adapter (GA)**: intended to enable analytics engineering directly on lakehouse tables in **OneLake**

The adapters are described as designed to align with Fabric-native concepts such as:

- **OneLake** storage abstraction
- Separation of compute and storage
- Enterprise governance and security

## Build and operationalize with dbt Jobs on Microsoft Fabric

For teams running dbt at scale, the article positions **dbt Jobs** as the operational control plane for:

- Scheduling
- Retries
- Observability
- Environment promotion

The post states that **public preview for dbt Jobs in Fabric opened in December 2025**, and announces expanded “enterprise-ready” capabilities.

### What’s new with dbt Jobs

- **Public package support**: use public/community dbt packages to reuse transformations and standardize patterns.
- **Native GitHub support**: run dbt Jobs from dbt code stored in **GitHub** (positioned as tighter CI/CD integration).
- **Enterprise-scale logging to OneLake**: stream execution logs to **OneLake** with no size limits (removing a prior 1 MB output log cap) to support troubleshooting, auditing, and long-term observability.
- **Pipeline dbt activity support (coming soon)**: dbt Jobs as a **native activity in Fabric pipelines** with parameterization to orchestrate dbt alongside other data/AI processes.
- **API support for dbt job**: APIs to trigger/monitor/manage job executions for CI/CD, orchestration, and automation.
- **Lakehouse adaptor support (coming soon)**: dbt Jobs supports multiple adapters including Fabric DW today, with Lakehouse adapter support planned.

The post summarizes intended outcomes as:

- Fabric Warehouse and Fabric Lakehouse participating naturally in dbt Jobs workflows
- Teams standardizing on dbt Jobs orchestration while using warehouse/lakehouse for compute and storage

## What’s ahead: dbt Fusion and Microsoft Fabric

![Diagram illustrating upcoming integration between Microsoft Fabric and dbt Labs through dbt Fusion in Fabric. It features logos for Microsoft Fabric on the left and dbt Labs on the right, connected by a central label "dbt Fusion in Fabric" with a "Coming soon" tag above.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/diagram-illustrating-upcoming-integration-between.png)

*Figure 2: dbt-Microsoft Fabric.*

Microsoft says it is actively working to support **dbt Fusion** as part of its Fabric investment, with **dbt Fusion support expected later in Q2 calendar year 2026**.

Planned work called out includes:

- **Warehouse and Lakehouse adapters** that integrate cleanly with dbt Fusion
- Ensuring Fabric workloads can participate in Fusion-powered dbt Jobs
- Aligning **execution, metadata, and observability** between Fabric and dbt’s next-generation architecture

The post frames this as enabling customers to adopt dbt Fusion **without rework or disruption** to existing Fabric analytics platforms.

## FabCon: Where dbt and Fabric come together

The article points to **FabCon** as the place where Microsoft and dbt will share:

- Current state of dbt + Fabric integration
- Real-world patterns for warehouses and lakehouses
- Forward-looking direction around dbt Fusion

## Resources

- [dbt Labs Expands dbt Fusion Engine Ecosystem with Microsoft Fabric Integration](https://www.getdbt.com/blog/dbt-labs-integrates-dbt-fusion-engine-in-microsoft-fabric)
- [dbt Job in Microsoft Fabric: Ship Trustworthy SQL Models Faster](https://blog.fabric.microsoft.com/blog/dbt-job-in-microsoft-fabric-ship-trustworthy-sql-models-faster-preview/)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/dbt-microsoft-fabric-a-strategic-investment-in-the-modern-analytics-stack/)

