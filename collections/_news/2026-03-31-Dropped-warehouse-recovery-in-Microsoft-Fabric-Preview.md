---
primary_section: ml
date: 2026-03-31 09:30:00 +00:00
author: Microsoft Fabric Blog
section_names:
- ml
tags:
- Accidental Drop Recovery
- Data Resilience
- ETL Pipelines
- Fabric Data Warehouse
- Microsoft Fabric
- ML
- News
- Permissions
- Point in Time Recovery
- Retention Period
- Saved Queries
- Snapshots
- Stored Procedures
- Tenant Configuration
- Views
- Warehouse Restore
- Workspace Administration
- Workspace Recycle Bin
title: Dropped warehouse recovery in Microsoft Fabric (Preview)
feed_name: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/dropped-warehouse-recovery-in-microsoft-fabric-preview/
---

Microsoft Fabric Blog explains how Microsoft Fabric Data Warehouse (Preview) can restore a dropped warehouse from the Workspace Recycle Bin within a tenant-configured retention window, bringing back data, schemas, snapshots, and security without manual rebuilds.<!--excerpt_end-->

# Dropped warehouse recovery in Microsoft Fabric (Preview)

*If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at FabCon and SQLCon announcements across Fabric and Microsoft database offerings: https://aka.ms/FabCon-SQLCon-2026-news*

## When an accidental drop isn’t the end anymore

A common failure mode in fast-moving data platforms is an accidental delete: deployments roll out, ETL changes land, schemas evolve, and suddenly a key analytics artifact disappears.

In this scenario, a user reports they can’t see the **Marketing reports**, and you discover the **Marketing NY Taxi Warehouse** has been dropped from the Fabric workspace.

Instead of a traditional recovery sequence (restore jobs, pipeline replays, and extended downtime), **Fabric Data Warehouse** supports a near-instant restore path.

## What you restore (not just the warehouse “shell”)

After the restore, the warehouse returns with its associated artifacts, including:

- Table schemas and data
- Warehouse snapshots
- Permissions and security
- Saved queries, views, and stored procedures

## Example: warehouse recovery in action

![Animation showing a Fabric workspace where an admin opens the Recycle Bin, selects a dropped warehouse and snapshot, and restores them.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-displays-a-power-bi-new-york-taxi-worksp-scaled.gif)

*Figure: From drop to restore in minutes.*

The article emphasizes what is *not* required:

- No manual rebuilds
- No copy jobs
- No restore pipelines
- No multi-hour outage
- No data loss

## How it works

Fabric doesn’t immediately delete the data warehouse after it is dropped.

Instead, **Microsoft Fabric automatically retains dropped data warehouses** for a retention period. During that window, a **Workspace Administrator** can restore the dropped warehouse.

### Retention window

- Tenant-configured retention window: **7 to 90 days**
- Default retention: **7 days**

Within that configured window, the warehouse can be restored to the **exact state it was in before the drop**.

## Why this matters (including in AI-driven environments)

Accidental drops remain a realistic operational risk because data work changes quickly (ETL updates, schema changes, and deployment mistakes). The key requirement is **fast and predictable recovery**.

The framing here is “resilience by design”: make recovery boring, reliable, and well-defined.

## Learn more

- Manage workspaces documentation: https://learn.microsoft.com/fabric/admin/portal-workspaces


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/dropped-warehouse-recovery-in-microsoft-fabric-preview/)

