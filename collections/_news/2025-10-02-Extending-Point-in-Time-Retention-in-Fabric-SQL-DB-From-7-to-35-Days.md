---
external_url: https://blog.fabric.microsoft.com/en-US/blog/extending-point-in-time-retention-in-fabric-sql-db-from-7-to-35-days/
title: 'Extending Point-in-Time Retention in Fabric SQL DB: From 7 to 35 Days'
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-10-02 11:00:00 +00:00
tags:
- Automated Backups
- Backup Retention
- Compliance
- Data Platform
- Database Management
- Database Recovery
- Disaster Recovery
- Enterprise Data
- Fabric SQL DB
- Microsoft Fabric
- PITR
- Point in Time Restore
- Sys.dm Database Backups
- ZRS
- Azure
- ML
- News
- Machine Learning
section_names:
- azure
- ml
primary_section: ml
---
The Microsoft Fabric Blog team details the extension of point-in-time restore retention in Fabric SQL DB from 7 to 35 days, providing step-by-step guidance for database administrators to strengthen data protection strategies.<!--excerpt_end-->

# Extending Point-in-Time Retention in Fabric SQL DB: From 7 to 35 Days

Microsoft Fabric has introduced a significant enhancement to its Fabric SQL DB service: the ability to extend point-in-time restore (PITR) retention from 7 days to up to 35 days. This update strengthens your data resilience, enabling better compliance, operational flexibility, and robust recovery capabilities.

## Why Extended PITR Retention Matters

- **Regulatory Compliance:** Extended retention helps meet industry requirements (e.g., finance, healthcare) for longer data retention windows.
- **Operational Safety Net:** More time to recover from accidental data loss or delayed error discovery.
- **Strategic Alignment:** Enables PITR settings to be consistent with enterprise backup and disaster recovery policies.

## What Is Fabric SQL DB?

Fabric SQL DB is a fully managed, SaaS-style relational database offering within Microsoft Fabric. It provides seamless SQL integration for analytical workloads, Power BI, and OneLake, with simplified management and enterprise features like high availability and security.

## Automatic Backups: How It Works

- **Full backups:** Weekly
- **Differential backups:** Every 12 hours
- **Transaction log backups:** Approx. every 10 minutes
- **Storage:** All backups use Azure zone-redundant storage for durability and high availability

## Point-in-Time Restore (PITR)

PITR lets you restore a database to any state within the configured retention period. Previously limited to 7 days, you can now set up to 35 days for PITR, offering broader coverage for operational and compliance scenarios.

## Configuring Extended Retention

**To change PITR retention:**

1. Sign in to your Fabric workspace.
2. Open your Fabric SQL database.
3. Go to **Settings**.
4. Select **Backup retention policy**.
5. Move the slider or enter a value (1–35 days).
6. Click **Save**.

![](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/09/image-28-1024x497.png)

> **Note:** Extending retention increases storage costs.

## Backup Observability

To monitor backup status and details, use the [`sys.dm_database_backups`](https://learn.microsoft.com/sql/relational-databases/system-dynamic-management-views/sys-dm-database-backups-azure-sql-database?view=azuresqldb-current) dynamic management view. It provides backup type (Full, Differential, Transaction log) and history for compliance and troubleshooting purposes.

![](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/09/image-2-1024x286.gif)

## Summary

Extending PITR retention is a straightforward but powerful configuration for database administrators and engineers building analytics solutions, SaaS apps, or internal tools on Microsoft Fabric. Review your current backup retention policy and consider adjusting to take advantage of the new 35-day window for enhanced data protection.

For more details, visit the official [blog post](https://blog.fabric.microsoft.com/en-us/blog/extending-point-in-time-retention-in-fabric-sql-db-from-7-to-35-days/).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/extending-point-in-time-retention-in-fabric-sql-db-from-7-to-35-days/)
