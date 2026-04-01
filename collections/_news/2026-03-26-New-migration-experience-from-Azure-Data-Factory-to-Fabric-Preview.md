---
feed_name: Microsoft Fabric Blog
date: 2026-03-26 08:00:00 +00:00
external_url: https://blog.fabric.microsoft.com/en-US/blog/new-migration-experience-from-azure-data-factory-to-fabric-preview/
title: New migration experience from Azure Data Factory to Fabric (Preview)
section_names:
- azure
- ml
primary_section: ml
tags:
- ADF Pipelines
- Azure
- Azure Data Factory
- Compatibility Gaps
- CSV Export
- End To End Testing
- Fabric Connections
- Fabric Data Factory
- Fabric Workspace
- Global Parameters
- Linked Services
- Microsoft Fabric
- Microsoft Learn
- Migration Assessment
- Migration Best Practices
- Migration Planning
- ML
- Modern Get Data Experience
- News
- Pipeline Migration
- Triggers Disabled
- Variable Libraries
author: Microsoft Fabric Blog
---

Microsoft Fabric Blog announces a preview migration experience that assesses Azure Data Factory (ADF) pipelines and then helps move supported pipelines into Fabric Data Factory, including compatibility reporting, linked service-to-connection mapping, and recommended post-migration validation steps.<!--excerpt_end-->

## Overview

As more organizations standardize on Microsoft Fabric for end-to-end analytics, upgrading existing Azure Data Factory (ADF) workloads is a key modernization step. This post introduces a **built-in migration experience (Preview)** to help you **assess, plan, and migrate ADF pipelines to Fabric Data Factory** in a more predictable way.

> Note: The post references Arun Ulag’s “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” announcement roundup.

## What’s new

The migration experience is **assessment-first** so you can understand readiness before moving workloads.

Capabilities:

- Assess pipeline readiness directly in **Azure Data Factory**.
- Identify compatibility gaps at the **pipeline and activity** level.
- Migrate supported pipelines into a **Fabric workspace**.
- Plan next steps for items that need updates or are marked **Coming soon**.

## Prerequisites

Before starting, ensure you have:

- An existing **Azure Data Factory** with pipelines.
- Access to a **Microsoft Fabric tenant**.
- A **Fabric workspace** where pipelines will migrate.

## How the migration experience works

The flow is integrated into the **Azure Data Factory authoring experience** and guides you step by step.

## Step 1: Run an assessment

In ADF, select **Migrate to Fabric (Preview)**, then choose **Get started** to evaluate pipelines and activities.

![Screenshot showing the Migration entrypoint from Azure Data Factory Pipeline editor canvas](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-showing-the-migration-entrypoint-from-a.png)

## Step 2: Review assessment results

Each pipeline is categorized as one of:

- **Ready**
- **Needs review**
- **Coming soon**
- **Unsupported**

You can also **export a CSV report** for offline review and remediation planning.

![Screenshot showing migration assessment results for the Azure Data Factory chosen for Migration to Fabric](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-showing-migration-assessment-results-fo.png)

## Step 3: Mount your Azure Data Factory to Fabric

After reviewing the assessment, select **Next** to mount your Azure Data Factory to a Fabric workspace, then continue the migration flow in Fabric.

![Screenshot showing Fabric workspace selection dropdown](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-showing-fabric-workspace-selection-drop.png)

![Confirmation message showing successful mounting of the chosen Azure Data Factory to Fabric](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/confirmation-message-showing-successful-mounting-o.png)

## Step 4: Migrate pipelines into Fabric Data Factory

From the Fabric UX, select **Migrate to Fabric (Preview)**, then:

- Choose the pipelines to migrate.
- Map **ADF linked services** to **Fabric connections**.
  - You can select existing connections or create new ones using the **modern get-data experience**.
- Select **Confirm**.

![Screenshot showing the entry point button for Migration in Fabric](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-showing-the-entry-point-button-for-migr.png)

![Screenshot showing Pipeline selection option before migrating](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-showing-pipeline-selection-option-befor-1.png)

![Screenshot showimg Linked Services to Fabric Connections mapping](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-showimg-linked-services-to-fabric-conne.png)

Safety default:

- Migrated pipelines have **triggers disabled by default**, so you stay in control of execution.

## Post-migration recommendations

After migrating pipelines:

- Validate connections and credentials.
- Update **Global parameters** using **Fabric Variable Libraries**.
- Run end-to-end tests to confirm behavior.
- Re-enable triggers as needed.

## Additional resources

- [Assess your Azure Data Factory and Synapse pipelines for migration to Fabric – Azure Data Factory | Microsoft Learn](https://learn.microsoft.com/azure/data-factory/how-to-assess-your-azure-data-factory-to-fabric-data-factory-migration)
- [Differences between Data Factory in Fabric and Azure – Microsoft Fabric | Microsoft Learn](https://learn.microsoft.com/fabric/data-factory/compare-fabric-data-factory-and-azure-data-factory)
- [Migration Planning for Azure Data Factory to Fabric Data Factory – Microsoft Fabric | Microsoft Learn](https://learn.microsoft.com/fabric/data-factory/migrate-planning-azure-data-factory)
- [Migration Best Practices for Azure Data Factory to Fabric Data Factory – Microsoft Fabric | Microsoft Learn](https://learn.microsoft.com/fabric/data-factory/migration-best-practices)

Feedback and community:

- Fabric Ideas: Submit feedback on the Data Factory / Copy Job label
- Fabric Community: Join the conversation in the Copy job forum


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/new-migration-experience-from-azure-data-factory-to-fabric-preview/)

