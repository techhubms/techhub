---
author: Microsoft Fabric Blog
date: 2026-04-06 15:00:00 +00:00
external_url: https://blog.fabric.microsoft.com/en-US/blog/introducing-migration-assistant-for-sql-database-in-fabric-preview/
tags:
- AI
- Azure OpenAI
- Azure SQL Database Migration
- Compatibility Assessment
- Customer Managed Keys (cmk)
- DACPAC
- Data Factory
- Fabric Copy Jobs
- Fabric Data Gateway
- Microsoft Entra Authentication
- Microsoft Fabric
- Migration Assistant
- ML
- Near Real Time Replication
- News
- OneLake
- Power BI
- Retrieval Augmented Generation (rag)
- Spark Notebooks
- SQL Auditing
- SQL Database in Fabric
- SQL Server Management Studio (ssms)
- SQL Server Migration
- SqlPackage
- T SQL
- Vector Search
- VS Code
feed_name: Microsoft Fabric Blog
primary_section: ai
title: Introducing Migration assistant for SQL database in Fabric (Preview)
section_names:
- ai
- ml
---

Microsoft Fabric Blog introduces the Migration Assistant (Preview) for SQL database in Microsoft Fabric, outlining a wizard-driven process to migrate from SQL Server or Azure SQL Database using DACPAC schema import, compatibility checks, and Fabric Copy Jobs—positioning operational data for OneLake-based analytics and AI workloads.<!--excerpt_end-->

# Introducing Migration assistant for SQL database in Fabric (Preview)

Modernizing legacy SQL Server workloads often gets stuck on migration complexity—schema conversion, ETL work, and re-architecture costs. This announcement introduces the **Migration Assistant (Preview)** for **SQL database in Microsoft Fabric**, a Fabric-native wizard meant to simplify moving SQL Server-based workloads into Fabric.

It follows the general availability announcement of SQL database in Fabric: [Announcing SQL database in Fabric (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/announcing-sql-database-in-microsoft-fabric-public-preview).

## Why migrate to SQL database in Fabric?

The post describes SQL database in Fabric in three pillars:

- **Simple**: Provision databases quickly without complex networking/storage setup.
- **Autonomous and secure**: Serverless scaling plus built-in high availability/disaster recovery, **Microsoft Entra authentication**, **customer-managed keys (CMK)**, and **SQL auditing**.
- **Optimized for AI**:
  - Native **vector search**
  - Support for **Retrieval-Augmented Generation (RAG)**
  - Integration with **Azure OpenAI** and **Microsoft Foundry**

A key differentiator called out is **near real-time replication to OneLake**, so operational data becomes available for:

- Analytics
- **Power BI** reports
- **Spark** notebooks
- AI workloads

## Migration assistant for SQL database in Fabric

The **Migration Assistant** is a wizard-driven experience intended to migrate:

- From **on-premises SQL Server**
- From **Azure SQL Database**

It guides users through **schema import** and **data copy**, aiming to keep the process familiar for SQL teams.

![Screenshot of a software interface showing migration options for data engineering items within a project named "zava-demo-fix." A highlighted section outlines "Migrate to a database" with details about SQL Server (Preview), emphasizing its suitability for workloads requiring full T-SQL support, low-latency/high concurrency OLTP, and smaller analytics without MPP.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-software-interface-showing-migrati-2.png)

*Figure: Migration Assistant for SQL Server in Fabric landing page.*

## Key features

- **DACPAC-based schema migration**: Import schema using **DACPAC** files.
- **Compatibility assessment**: Detect unsupported objects/issues before migrating.
- **Actionable guidance**: Step-by-step recommendations to resolve issues.
- **Built-in data copy workflows**: Use **Fabric Copy Jobs** powered by **Data Factory**.
- **Preserves SQL skills**: Continue using **T-SQL**, **SSMS**, and **Visual Studio Code**.

## How it works: Step-by-step

## Step 1: Launch the Migration wizard

From a Fabric workspace:

- Select **Migrate**
- Choose **Migrate to SQL database in Fabric**

## Step 2: Upload a DACPAC file

Export the source database schema as a DACPAC using one of:

- **SQL Server Management Studio (SSMS)**
- **MSSQL extension for VS Code**
- **SqlPackage**

Upload the DACPAC to the wizard for validation.

## Step 3: Provision the target database

- Name the new SQL database in Fabric
- The wizard provisions it

## Step 4: Deploy schema with compatibility checks

The assistant imports schema and flags compatibility issues when features/objects aren’t supported in SQL database in Fabric.

The post also mentions **Copilot-powered fix suggestions** to help resolve issues faster, with interactive accept/reject.

![Screenshot of a database migration assistant interface showing a metadata migration summary and step-by-step migration process. Key elements include a progress bar with migrated and failed object counts, a sidebar with database queries, and a central panel prompting users to query, preview, or connect data.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-database-migration-assistant-inter.png)

*Figure: Steps to migrate using the Migration Assistant.*

## Step 5: Copy data

After schema deployment:

- Launch a **Fabric Copy Job** to move data.
- Use **Fabric Data Gateway** to securely connect to on-premises sources.

The post suggests using scripts around the load, for example:

- Pre-deployment: disable foreign keys
- Post-deployment: re-enable constraints

## Step 6: Validate and go live

- Review migration results in the Migration Assistant panel in the SQL Editor
- Fix remaining issues
- Validate data
- Go live

## Coming soon

The post says the feature will launch “in a few weeks” and become accessible through the Fabric portal. Feedback is directed to the Fabric Community Forums (links in the original post).


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/introducing-migration-assistant-for-sql-database-in-fabric-preview/)

