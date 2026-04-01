---
date: 2026-03-30 10:30:00 +00:00
title: Compute auto-scaling choices to better optimize price-performance for SQL databases in Microsoft Fabric (Preview)
primary_section: ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/compute-auto-scaling-choices-to-better-optimize-price-performance-for-sql-databases-in-microsoft-fabric-preview/
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
section_names:
- ml
tags:
- Auto Scaling
- Azure SQL
- Backward Compatibility
- Capacity Management
- Compute (preview)
- Cost Control
- Fabric SQL Database
- Max Vcores Limit
- Microsoft Fabric
- ML
- News
- Noisy Neighbor
- Price Performance
- Resource Governance
- Shared Capacity
- SQL Databases in Fabric
- Vcores
---

Microsoft Fabric Blog announces a preview Compute setting for SQL databases in Microsoft Fabric that lets admins cap auto-scaling with a Max vCores limit (default 32, optional 4) to improve cost predictability and reduce “noisy neighbor” impact in shared capacities.<!--excerpt_end-->

# Compute auto-scaling choices for SQL databases in Microsoft Fabric (Preview)

One common issue for customers running **SQL databases in Microsoft Fabric** is controlling cost and keeping resource usage predictable, especially in **shared capacities**. Fabric SQL databases already **auto-scale** to meet workload demand, but that can allow a single database to temporarily consume a large portion of capacity.

To address this, Fabric is adding a new **Compute** setting (preview) that lets you **cap the maximum compute** a database can use—without changing current behavior unless you opt in.

## What’s new

SQL databases in Fabric now expose a **Compute** configuration in database settings. It sets an upper **vCore limit** the database can reach while scaling.

- **Safe by default**: existing behavior remains unchanged unless you change the setting.
- **Opt-in cost control**: administrators can cap a database at a lower maximum.
- **Database-level setting**: applies per database within the existing Fabric capacity model.
- **Backward compatible**: designed not to break existing workloads.

## Default behavior stays the same

When available:

- **Existing databases** are unchanged.
- **New databases** default to current behavior.
- The default maximum remains **32 vCores**, matching today’s Fabric SQL experience.

## New compute options

In the **Compute** section of SQL database settings, you’ll see **Max vCores limit**.

Choices described in the announcement:

- **32 max vCore (default)**: database can scale up to 32 vCores (current behavior).
- **4 vCores**: caps the database at a lower maximum to improve cost predictability and reduce capacity impact.

This is a deliberate trade-off: you can limit peak performance to get more predictable costs and protect shared capacity.

![Screenshot of a SQL database settings panel for AdventureWorks showing compute resource configuration. It highlights a dropdown menu set to "32 vCores (default) (current)" with a warning about potential temporary connectivity disruptions when changing this setting.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-sql-database-settings-panel-for-ad.png)

*Figure: SQL database Compute preview settings showing a configurable Max vCores limit, with 32 vCores selected as the default.*

![Screenshot of a SQL database settings panel for AdventureWorks showing Compute configuration options. Dropdown menu displays Max Vcore limit with choices of 4 vCores and 32 vCores (default and current), allowing adjustment of database resource limits.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-sql-database-settings-panel-for-ad-1.png)

*Figure: SQL database Compute preview showing the Max vCores limit dropdown open with 32 vCores selected.*

![Screenshot of a SQL database settings panel for AdventureWorks showing the Compute (preview) tab. It displays an option to set Max vCore limit with a dropdown currently set to 4 vCores, a warning about potential temporary connectivity disruptions, and Save and Discard buttons.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-sql-database-settings-panel-for-ad-2.png)

*Figure: SQL database Compute preview with the Max vCores limit changed to 4 vCores.*

## Why it matters

### Better cost control

Setting **max vCores** puts an upper bound on maximum compute (vCores and memory) a database can consume. This helps control costs and makes it easier to plan and manage Fabric capacity.

The post calls out common cases where a lower cap makes sense:

- **Dev/test databases**
- **Lightweight production workloads**

In these scenarios, limiting the auto-scaling range to **4 max vCores** (instead of **32**) can reduce cost and reduce impact on shared capacity.

### Protection from “noisy neighbors”

In shared Fabric capacities, a busy database can temporarily dominate resources. A compute limit acts as a **guardrail** so no single database overwhelms the rest of the workspace.

## How it works at runtime

- The database continues to **auto-scale** based on workload demand.
- When usage reaches the configured **max vCores** limit, scaling stops at that cap.
- Workloads continue running but may take longer once the cap is reached.
- No interruption to availability during normal operation.

The intent is **graceful degradation under load** rather than failures.

## What happens when you change the setting

Changing the max vCores limit is a configuration update and may involve a short transition while the database applies the new setting.

During this update:

- You may see a **brief connectivity interruption**.
- **Data is preserved**.
- The database resumes with the new compute limit.

If you request a downscale that isn’t compatible with the current database size, the update fails with a **clear error message**.

## Designed for Fabric—familiar to SQL users

The experience is meant to align with how customers manage cost/performance in **Azure SQL**, while staying integrated into the Fabric capacity model:

- Simple controls
- Safe defaults
- Clear trade-offs
- No change unless you opt in

## Getting started

In Microsoft Fabric, open your SQL database settings and look for **Compute (Preview)**. No action is required unless you want to apply limits.

## Summary

With the new Compute settings for SQL databases in Microsoft Fabric:

- Keep today’s scaling behavior by default.
- Configure a **max vCores cap** when needed.
- Improve cost control and capacity governance.
- Reduce resource contention in shared environments.

For current limits and edge cases, see the documentation (to be updated as the feature evolves): https://aka.ms/sqldatabaseinfabricknobs


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/compute-auto-scaling-choices-to-better-optimize-price-performance-for-sql-databases-in-microsoft-fabric-preview/)

