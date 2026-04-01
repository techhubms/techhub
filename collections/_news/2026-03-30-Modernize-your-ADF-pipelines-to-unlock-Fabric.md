---
title: Modernize your ADF pipelines to unlock Fabric
tags:
- AI
- Airflow Job
- Apache Airflow
- Azure
- Azure Data Factory
- Azure Synapse Analytics
- Compatibility
- Copilot
- Copy Job
- Data Integration
- Dbt
- ETL
- Fabric Connections
- Fabric Data Factory
- Fabric Workspace
- Linked Services
- Microsoft Fabric
- Migration Tool
- Mirroring
- ML
- Modernization
- News
- OneLake
- Orchestration
- Pipeline Assessment
- Pipeline Migration
- Preview Feature
- Triggers
- Unified Monitoring
- Variable Libraries
feed_name: Microsoft Fabric Blog
author: Microsoft Fabric Blog
date: 2026-03-30 13:00:00 +00:00
external_url: https://blog.fabric.microsoft.com/en-US/blog/modernize-your-adf-pipelines-to-unlock-fabric/
section_names:
- ai
- azure
- ml
primary_section: ai
---

Microsoft Fabric Blog announces a new (preview) in-product migration experience that helps move Azure Data Factory and Azure Synapse pipelines into Fabric Data Factory, including an assessment-first flow, automatic pipeline conversion steps, and notes on what happens during migration (like triggers being disabled by default).<!--excerpt_end-->

# Modernize your ADF pipelines to unlock Fabric

*Note: The post references Arun Ulag’s related announcement roundup: “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform”.*

Moving your **Azure Data Factory (ADF)** and **Azure Synapse** pipelines to **Fabric Data Factory** is now presented as **fast, easy, and free**.

Microsoft announced a **new migration experience from Azure Data Factory to Fabric (Preview)** that’s **built directly into ADF and Synapse**, aiming to make it easier to modernize existing pipelines by moving them to Fabric Data Factory.

![Screenshot showing Pipeline selection option before migrating](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-showing-pipeline-selection-option-befor-3.png)

*Figure: Initiate the ADF Migration Tool from your pipeline.*

## What the new migration experience does

As organizations standardize on **Microsoft Fabric** for end-to-end analytics, upgrading existing ADF and Synapse workloads is positioned as a critical step.

The migration experience is described as removing manual migration friction by providing an **assessment-first migration flow** so you can understand:

- Readiness
- Compatibility
- Next steps

…before moving any workloads.

It’s integrated into the existing **ADF authoring environment**, and is intended to help you:

- Assess pipelines
- Identify **supported and unsupported activities**
- Plan modernization with visibility and control

## How migration works (high level)

Once you proceed, the experience walks you end-to-end through moving pipelines into Fabric Data Factory.

The post says ADF will:

- Automatically **mount your factory to a Fabric workspace**
- **Migrate selected pipelines**
- Automatically **convert ADF linked services** into **Fabric connections**

Safety/control behavior called out:

- Pipelines are migrated with **triggers disabled by default**, so you can validate and test in Fabric before enabling them.

After migration, the post highlights Fabric-native capabilities you can use while preserving existing pipeline logic, including:

- Variable libraries
- Modern orchestration
- Unified monitoring

The stated outcome is a predictable, low-risk modernization path for mission-critical data integration workloads.

## Why migrate to Fabric Data Factory?

The post frames migration as more than moving pipelines: it’s about modernizing data integration to use what Fabric offers.

Fabric is described as unifying:

- Orchestration
- Data movement
- Transformation
n- Analytics
- AI

…in a single environment built on **OneLake**, aiming to reduce architectural complexity and accelerate insights.

It also claims Fabric enables moving beyond traditional ETL patterns via:

- AI-powered experiences
- Open orchestration
- Native integration across the analytics stack

After selecting **Migrate**, the post claims you can modernize ADF pipelines inside Fabric and unlock features including:

- Copilot
- Copy Job
- Mirroring
- dbt
- Airflow Job
- OneLake

…with “no pipeline rewrites required”.

## Next steps

- Learn more / get started: https://learn.microsoft.com/azure/data-factory/how-to-upgrade-your-azure-data-factory-pipelines-to-fabric-data-factory


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/modernize-your-adf-pipelines-to-unlock-fabric/)

