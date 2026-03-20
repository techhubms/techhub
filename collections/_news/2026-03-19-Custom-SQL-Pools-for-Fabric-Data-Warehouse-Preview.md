---
date: 2026-03-19 08:30:00 +00:00
tags:
- Application Name
- Azure
- Capacity SKU
- Compute Governance
- Custom SQL Pools
- ETL
- Fabric Data Warehouse
- Microsoft Fabric
- ML
- News
- Query Monitoring
- Query Routing
- Queryinsights.sql Pool Insights
- Regex
- Reporting Workloads
- Resource Allocation
- REST API
- Service Principals
- SLA
- SQL Analytics Endpoint
- Throttling
- Workload Isolation
- Workspace Administration
feed_name: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/custom-sql-pools-for-fabric-data-warehouse-preview/
title: Custom SQL Pools for Fabric Data Warehouse (Preview)
author: Microsoft Fabric Blog
primary_section: ml
section_names:
- azure
- ml
---

Microsoft Fabric Blog announces Custom SQL Pools for Fabric Data Warehouse (Preview), a feature that lets workspace admins isolate SQL workloads by allocating compute percentages per pool, routing queries by application name/regex, and monitoring pressure via a built-in insights view.<!--excerpt_end-->

# Custom SQL Pools for Fabric Data Warehouse (Preview)

Microsoft Fabric Data Warehouse now supports **Custom SQL Pools (Preview)**, a capability aimed at giving **workspace administrators** more predictable isolation and control over how SQL compute is allocated across different SQL workloads in a **Fabric Data Warehouse** and its **SQL analytics endpoint**.

The post also points readers to Arun Ulag’s broader roundup of announcements: [FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news).

## What changed vs the previous model

Previously, Fabric Data Warehouse used **two isolated SQL pools** and routed work based on statement type:

- **SELECT** queries
- **NONSELECT** operations (for example, ingestion queries)

That default setup helps keep ETL/ingestion from interfering with reporting. But in workspaces hosting multiple applications, customers asked for **more predictable isolation** and **more direct control**.

![Animation showing the built-in 2-pool 50/50 split transitioning to 3 custom pools split 30%, 60%, and 10%.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/showcasing-the-current-built-in-pool-configuration.gif)

*Figure: Fabric Data Warehouse Pool Isolation*

## With Custom SQL Pools, you can

- Allocate compute using a **percentage of total resources** available.
- Route queries based on **Application Name** or **Application Name Regex**.
- Define a **default pool** so queries can always execute.
- Optimize specific pools for **read-heavy workloads**.
- Keep resources in each pool **isolated** from each other.

## When this is especially useful

- **Multiple applications** share a single Fabric warehouse or SQL analytics endpoint and compete for the same pool resources.
- **Business-critical reporting** gets disrupted by ad-hoc queries, causing missed **SLAs** or deadlines.
- **High capacity utilization** leads to **throttling**, creating a need for tighter control over warehouse consumption for user queries.

## Scaling and configuration options

- Custom SQL Pools **scale automatically** when the **capacity SKU** changes, while preserving the **relative percentage allocations** you defined.
- Configuration is available via:
  - the **Fabric UI**
  - **REST APIs** (including support for **service principals**)

![Screenshot of Fabric workspace settings showing SQL pool configuration with 30% Pipelines, 60% PowerBI (optimized for reads), and 10% Adhoc.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-workspace-settings-panel-showing-s.png)

*Figure: Custom SQL Pool Configuration*

## Monitoring pool pressure

Admins can use the **`queryinsights.sql_pool_insights`** view to:

- monitor SQL pool configurations
- identify pools that are frequently under pressure
- adjust resource percentages proactively to improve end-user query performance

## References

- Microsoft Learn: [Custom SQL Pools – Microsoft Fabric](https://aka.ms/CustomSQLPoolsDocs)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/custom-sql-pools-for-fabric-data-warehouse-preview/)

