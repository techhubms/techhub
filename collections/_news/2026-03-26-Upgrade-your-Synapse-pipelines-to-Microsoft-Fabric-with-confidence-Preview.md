---
external_url: https://blog.fabric.microsoft.com/en-US/blog/upgrade-your-synapse-pipelines-to-microsoft-fabric-with-confidence-preview/
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
section_names:
- azure
- ml
date: 2026-03-26 14:00:00 +00:00
primary_section: ml
title: Upgrade your Synapse pipelines to Microsoft Fabric with confidence (Preview)
tags:
- Azure
- Azure Synapse Analytics
- Connection Mapping
- CSV Export
- Data Integration
- Fabric Connections
- Fabric Data Factory
- Linked Services
- Microsoft Fabric
- Migration
- Migration Assessment
- ML
- News
- Phased Migration
- Pipeline Compatibility
- Preview Feature
- Synapse Pipelines
- Triggers
- Workspace Migration
---

Microsoft Fabric Blog explains an upcoming preview migration experience to assess Azure Synapse pipelines, identify compatibility gaps, and migrate supported pipelines into Fabric Data Factory, with a staged flow (assessment, review, migration) and post-migration validation steps.<!--excerpt_end-->

# Upgrade your Synapse pipelines to Microsoft Fabric with confidence (Preview)

To modernize workloads on Microsoft Fabric, migrating existing Azure Synapse Analytics pipelines is a key step. A built-in migration experience (Preview) is being introduced to help you assess readiness, understand compatibility gaps, and migrate supported pipelines into Fabric Data Factory.

If you want the full set of FabCon/SQLCon announcements, see Arun Ulag’s post: [FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news).

## What’s new

The migration experience is designed to make readiness clear before you move any workload.

### Capabilities

- Assess pipeline readiness directly in your Synapse workspace.
- Understand compatibility gaps at both pipeline and activity level.
- Migrate supported pipelines into a Fabric workspace.
- Plan next steps for items that require updates or are not yet supported.

The intent is to keep migrations intentional and lower risk, especially when you want phased validation.

## Prerequisites

Before migrating, you need:

- An existing Synapse workspace that contains pipelines.
- Access to a Microsoft Fabric tenant and a Fabric workspace.

## How the migration experience works

The flow has three stages: assessment, review, and migration.

### 1) Run an assessment

In **Azure Synapse Analytics**, open the workspace you want to assess, then:

- Go to **Integrate**
- Select **Migrate to Fabric (Preview)**
- Choose **Get started**

This evaluates pipelines and activities for migration readiness.

![Screenshot showing Migrate to Fabric button and Get Started button to start migration assessment](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-showing-migrate-to-fabric-button-and-ge.png)

### 2) Review assessment results and select pipelines for migration

Each Synapse pipeline is categorized as one of:

- Ready
- Needs review
- Coming soon
- Unsupported

You can also export a **CSV report** for offline review and remediation planning.

Once you’ve decided on scope, select the Synapse pipelines you want to migrate to a Fabric workspace.

![Screenshot of the assessment results page displaying pipeline readiness statuses and an Export option.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-the-assessment-results-page-displayi.png)

### 3) Review connections and migrate

- Select a **Fabric workspace** as the migration target.
- Map **Synapse linked services** to **Fabric connections**.

If you already created connections in Fabric, you can select them from dropdowns. Otherwise, create new Fabric connections from workspace settings.

Reference: [Data source management](https://learn.microsoft.com/fabric/data-factory/data-source-management).

> Note: Pipelines migrate even if connections aren’t mapped, but activities that use those connections remain deactivated.

![Screenshot of the workspace selection and connection mapping step, showing Synapse linked services on one side and Fabric connections on the other.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-the-workspace-selection-and-connecti.png)

After mapping linked services, select **Confirm**.

![Screenshot showing successful migration of synapse pipelines](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-showing-successful-migration-of-synaps.png)

Pipelines migrate with **triggers disabled by default**, keeping execution under your control.

## Post-migration steps

- Validate connections and credentials.
- Run end-to-end tests to confirm behavior.
- Reconfigure and enable triggers as needed.

## Resources

- [Assess your Azure Data Factory and Synapse pipelines for migration to Fabric](https://learn.microsoft.com/azure/data-factory/how-to-assess-your-azure-data-factory-to-fabric-data-factory-migration)
- Submit feedback: [Fabric Ideas](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/data%20factory%20%7C%20copy%20job)
- Community discussion: [the Fabric Community](https://community.fabric.microsoft.com/t5/Copy-job/bd-p/db_copyjob)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/upgrade-your-synapse-pipelines-to-microsoft-fabric-with-confidence-preview/)

