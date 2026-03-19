---
feed_name: Microsoft Fabric Blog
author: Microsoft Fabric Blog
date: 2026-03-19 10:15:00 +00:00
external_url: https://blog.fabric.microsoft.com/en-US/blog/whats-new-and-improved-for-sql-database-in-fabric-generally-available/
title: What’s new and improved for SQL database in Fabric (Generally Available)
section_names:
- ai
- azure
- devops
- ml
- security
tags:
- AI
- Approximate Nearest Neighbor (ann)
- Azure
- Azure Key Vault
- Azure SQL
- Backup Retention
- CI/CD
- Collation
- Customer Managed Keys
- DACPAC
- Database Compatibility Level
- Database Mirroring
- Deployment Pipelines
- DevOps
- DiskANN
- Dynamic Data Masking
- Fabric REST API
- Full Text Search
- KNN
- Microsoft Fabric
- Migration Assistant
- ML
- News
- OneLake
- Point in Time Restore
- Private Link
- Security
- Source Control
- SQL Auditing
- SQL Database in Fabric
- SQL Server Migration
- T SQL
- Vector Search
primary_section: ai
---

Microsoft Fabric Blog summarizes the latest GA and preview improvements for SQL database in Microsoft Fabric, including migration help for SQL Server/Azure SQL, new operational controls, Fabric REST API scenarios, security features like CMK with Azure Key Vault, and performance upgrades for vector search.<!--excerpt_end-->

## Overview

Following the GA announcement for SQL database in Microsoft Fabric at Microsoft Ignite (November 2025), this post describes new and improved capabilities based on customer feedback across three pillars:

- Simplified
- Autonomous & Secure
- Optimized for AI

It also links to a broader roundup: [FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news).

## Modernize with confidence – Migration Assistant (Preview)

A new **Migration Assistant** helps move **SQL Server** and **Azure SQL** workloads into Fabric.

Key points:

- Imports schema via **DACPACs**.
- Detects **compatibility issues**.
- Provides **actionable guidance** prior to migration.
- Includes built-in assessment and data copy workflows to reduce manual work and support cutover.

Learn more: Introducing Migration Assistant to SQL database in Fabric (link provided in source; destination appears to be an internal SharePoint URL).

![Screenshot of a software interface showing migration options for SQL database in Fabric. A highlighted section details migrating to a SQL Server (Preview) with features like full T-SQL support, low-latency/high concurrency OLTP, and suitability for smaller analytics](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-software-interface-showing-migrati-1.png)

*Figure: Migration options for SQL databases in Microsoft Fabric.*

![Screenshot of a database migration tool interface showing a Migration assistant tab with a metadata migration summary panel on the right. The panel highlights migration status with green and red indicators for migrated objects and objects to fix, alongside a four-step migration process including copy metadata, fix script errors, prepare for copy, and finalize copy.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-database-migration-tool-interface.png)

*Figure: Migration assistant showing overall status and progress across migration steps.*

## Autonomous by default, configurable by design (Preview)

SQL database in Fabric is positioned as **SaaS-first**, minimizing day-to-day management while adding controls where needed.

New capability:

- **Database-level compute limits** to cap maximum **vCore** usage.
- Intended to control scaling behavior and cost on shared Fabric capacities.
- Opt-in control that keeps safe defaults (preserves existing behavior).

Learn more: [SQL database in Fabric compute limits](https://aka.ms/FabricSQL-ComputeSettings-blog).

![Screenshot of a database configuration interface allowing users to manage performance and capacity by setting a maximum vCore limit, currently set to 4 vCores to a max of 32 vCores. The interface includes a warning about potential temporary connectivity disruptions when changing settings, along with Save and Discard buttons for applying or canceling changes.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-database-configuration-interface-a.png)

*Figure: Database settings page for configuring maximum vCore capacity limits.*

Additional platform compatibility improvements mentioned:

- Expanded support for **database compatibility levels**
- Additional **T-SQL** capabilities
- **Full-text search**
- Expanded support for relevant **ALTER DATABASE SET** options

Goal: bring existing applications to SQL database in Fabric with minimal/no code changes and maintain parity with Azure SQL Database.

## Full collation support (Preview)

You can now use all **Azure SQL Database collations** when creating databases, to improve compatibility for global scenarios.

Resources:

- YouTube demo: https://www.youtube.com/watch?v=YiMmhcYm5mk
- Sample repo for creating a SQL database with a specific collation via the **Fabric REST API**: https://github.com/Microsoft/fabric-toolbox/tree/main/samples/notebook-create-sql-database
- Note: Fabric SQL Database collation does **not** affect replicate data collation in the SQL analytics endpoint.
- For SQL analytics endpoint collation changes: [Data Warehouse collation – Microsoft Fabric](https://learn.microsoft.com/fabric/data-warehouse/collation).

## Restore deleted databases

Deletion/restore behavior improvements:

- Deleted databases go into a **soft-deleted** state in the Fabric workspace **Recycle Bin**.
- You can recover while within the configured retention.
- Separate **backup retention period** is configurable from **1–35 days**.
- Even if a database is hard deleted from the Recycle Bin, backups remain available for the configured retention period.
- You can restore a backup into a new database to **any point in time** within the restorable period.

Learn more: [Automatic backups in SQL database – Microsoft Fabric](https://learn.microsoft.com/fabric/database/sql/backup)

## Pre-deployment and post-deployment scripts

Fabric’s source control integration and deployment pipelines now support running additional **T-SQL** as part of the database definition.

How it works:

- Add a **Shared Query** to the database.
- Mark it as a **pre-deployment** or **post-deployment** script.

Use cases mentioned:

- Branching and provisioning enhancements
- Static data management
- Customization steps

The post notes the SQL projects database definition remains compatible with broader database CI/CD tooling.

Learn more: [Source control and deployment pipelines for SQL database in Fabric](https://learn.microsoft.com/fabric/database/sql/source-control).

## Manage built-in Mirroring (Preview)

Built-in mirroring to **OneLake** is intended to make operational data available for analytics and AI with zero ETL.

New controls:

- Selectively manage which **tables** are mirrored.

Docs and APIs:

- Overview: https://learn.microsoft.com/fabric/database/sql/mirroring-overview
- Start/restart mirroring via REST API: https://learn.microsoft.com/fabric/database/sql/start-stop-mirroring-api?tabs=5dot1

Related security networking link:

- Workspace-level private links setup: https://learn.microsoft.com/fabric/security/security-workspace-level-private-links-set-up?tabs=fabric-portal

## Enterprise readiness

The post lists enterprise-oriented capabilities including:

- Support for 5,000+ Azure SQL Database collations
- SQL auditing
- Customer-managed keys (CMK)
- Expanded availability zone support

### Customer-Managed Keys (CMK) (Generally Available)

CMK allows encrypting Fabric SQL database data with customer-owned keys stored in **Azure Key Vault**.

Benefits listed:

- Ownership and rotation of encryption keys
- Granular access control
- End-to-end auditability of key usage
- Support for industry-specific compliance needs

Resources:

- Video: https://youtu.be/1ffSH5g1t-Y
- Docs: [Data encryption in SQL database in Fabric](https://learn.microsoft.com/fabric/database/sql/encryption)

### Auditing (Generally Available)

Auditing provides logging for database activities to understand access and changes.

Why it matters (as described):

- Compliance requirements
- Investigating suspicious activity
- Forensic analysis via an immutable audit trail

Learn more: [Auditing for Fabric SQL Database](https://learn.microsoft.com/fabric/database/sql/auditing)

### Dynamic Data Masking (DDM) (Generally Available)

Dynamic Data Masking reduces exposure to sensitive data by applying masking functions on selected table columns.

Behavior:

- Users without appropriate privileges receive masked results.
- Underlying data remains unchanged.
- Designed to help protect sensitive data during development, analytics, and support scenarios without requiring app changes.

Learn more: [Dynamic Data Masking – SQL Server](https://learn.microsoft.com/sql/relational-databases/security/dynamic-data-masking?view=sql-server-ver17)

## Optimized for AI: Vector Index improvements in SQL database in Fabric

The post highlights improvements to **DiskANN Vector Indexes** for vector search.

Enhancements called out:

- Advanced vector quantization techniques for better storage efficiency and faster query performance (transparent to users; no code changes).
- Full DML support so vector-indexed tables are no longer read-only:
  - INSERT
  - UPDATE
  - DELETE
  - MERGE
  - Automatic, real-time index maintenance
- Iterative filtering to apply predicates during the search instead of post-filtering, to avoid over-fetching and keep consistent result counts.

The post also notes integration with the query optimizer to switch between:

- Exact **KNN** searches
- Approximate **ANN** searches

Reference: [VECTOR_SEARCH (Transact-SQL)](https://learn.microsoft.com/sql/t-sql/functions/vector-search-transact-sql?view=sql-server-ver17)

## What’s next

The post frames these changes as ongoing work toward making SQL database in Fabric simpler, more autonomous/secure, and AI-ready, with continued iteration based on customer feedback.

Further reading:

- [SQL database in Fabric overview and tutorials](https://learn.microsoft.com/fabric/database/sql/overview)
- Fabric Community Forums link provided in source: https://blog.fabric.microsoft.com/blog/announcing-sql-database-in-fabric-is-now-generally-available-ga?ft=All


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/whats-new-and-improved-for-sql-database-in-fabric-generally-available/)

