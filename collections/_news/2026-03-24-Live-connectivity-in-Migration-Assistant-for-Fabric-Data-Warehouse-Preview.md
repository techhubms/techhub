---
date: 2026-03-24 13:30:00 +00:00
feed_name: Microsoft Fabric Blog
author: Microsoft Fabric Blog
title: Live connectivity in Migration Assistant for Fabric Data Warehouse (Preview)
external_url: https://blog.fabric.microsoft.com/en-US/blog/live-connectivity-in-migration-assistant-for-fabric-data-warehouse-preview/
section_names:
- azure
- ml
tags:
- Azure
- Azure SQL Database
- Azure SQL Managed Instance
- Azure Synapse Analytics Dedicated SQL Pool
- DACPAC
- Database Security
- Fabric Data Warehouse
- Functions
- Live Connectivity
- Metadata Migration
- Microsoft Fabric
- Migration Assistant
- ML
- News
- On Premises Data Gateway
- Power BI Gateway
- Preview Feature
- Schema Migration
- SQL Server
- Stored Procedures
- Views
- Workspace
primary_section: ml
---

Microsoft Fabric Blog announces a preview feature in Migration Assistant for Fabric Data Warehouse that adds live connectivity to source systems, enabling metadata migration without generating a DACPAC and streamlining early migration steps.<!--excerpt_end-->

# Live connectivity in Migration Assistant for Fabric Data Warehouse (Preview)

The **live connectivity** feature (preview) in **Migration Assistant for Fabric Data Warehouse** allows you to **connect directly to your source system** and **migrate object metadata** into a new Fabric warehouse. The goal is to speed up the migration process by removing the need to generate and upload a **DACPAC** during the metadata step.

> If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a full view of broader announcements across Fabric and Microsoft database offerings.

## Key benefits

- **Start faster**: Begin metadata migration without DACPAC extraction or prep work.
- **Reduce complexity and risk**: Fewer manual steps and less chance of schema drift.
- **Keep the guided migration experience**: Continue using Migration Assistant’s guided flow to review outcomes and proceed through stages.

## What gets migrated (metadata)

The following object definitions are migrated to the destination warehouse:

- Schemas
- Tables
- Views
- Stored procedures
- Functions
- Security

## How to try it

1. Open the workspace > Select **Migrate**.
2. Select the **source**.
3. Choose **Connect directly to the source system**.
4. Provide **source connection details**.
5. Select the **destination** workspace and warehouse.
6. Select **Migrate**.

![Screenshot of a software workspace interface displaying an empty task flow area with a prompt to choose from predesigned task flows or add a new task. The interface includes buttons for selecting predesigned flows, importing task flows, and options for creating or managing apps, with a message indicating no content is currently present.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-software-workspace-interface-displ.gif)

*Figure 1: Connect directly to the source system.*

## Important considerations

- **Supported sources**: This live connectivity experience supports:
  - **Azure Synapse Analytics Dedicated SQL Pool**
  - **SQL Server database**
  - **Azure SQL Database**
  - **Azure SQL Managed Instance (SQL MI)** used for analytics
  - For on-premises or cloud SQL databases (including Azure SQL DB / Azure SQL MI), select the **SQL Server database** card.
- **Gateway requirements for on-prem SQL Server**: You may need an **on-premises data gateway**.
  - If you already have a **Power BI gateway**, you can leverage it.
- **Permissions affect metadata visibility**: If the credentials used to connect lack permissions to read catalog metadata, you may only see a subset of objects available for migration.

## Learn more

- [Migration Assistant for Fabric Data Warehouse – Microsoft Fabric | Microsoft Learn](https://learn.microsoft.com/fabric/data-warehouse/migration-assistant)
- [Fabric Ideas portal (migration ideas)](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/live-connectivity-in-migration-assistant-for-fabric-data-warehouse-preview/)

