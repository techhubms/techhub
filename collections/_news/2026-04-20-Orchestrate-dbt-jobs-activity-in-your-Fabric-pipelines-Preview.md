---
date: 2026-04-20 10:00:00 +00:00
section_names:
- ml
tags:
- Data Pipelines
- Data Transformations
- Dbt
- Dbt Jobs
- Dependency Conditions
- Dynamic Content
- Email Notifications
- Fabric Data Factory
- Fabric Pipelines
- Microsoft Fabric
- ML
- Monitoring
- News
- Parameterized Workflows
- Pipeline Parameters
- Preview Feature
- Run History
- Teams Notifications
- Workflow Orchestration
primary_section: ml
title: Orchestrate dbt jobs activity in your Fabric pipelines (Preview)
external_url: https://blog.fabric.microsoft.com/en-US/blog/orchestrate-dbt-jobs-activity-in-your-fabric-pipelines-preview/
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog announces a Preview feature that adds a dbt job activity to Microsoft Fabric pipelines, letting teams orchestrate dbt transformations with pipeline dependencies, parameters, notifications, and run monitoring in one place.<!--excerpt_end-->

## Overview

Coordinating dbt runs with upstream ingestion and downstream consumption often requires multiple tools. Microsoft Fabric now supports a **dbt job** activity (Preview) directly inside **Fabric pipelines**, so you can orchestrate dbt transformations alongside other pipeline activities.

## Why this matters

- **Run dbt jobs as part of your pipeline**: Chain dbt with other activities using dependency conditions.
- **Create or select dbt jobs inline**: Choose an existing dbt job or create one without leaving the pipeline canvas.
- **Parameterized workflows**: Pass dynamic runtime parameters from your pipeline to the dbt job to enable reusable runs.
- **Notifications on completion**: Send Teams or email notifications after each dbt run.
- **Single place to monitor**: Track dbt job status from the pipeline run history.

## How to use the dbt job activity

1. Open or create a data pipeline in your Fabric workspace.
2. Add the **dbt job** activity from the activity pane.
3. In the **Settings** tab, configure or select a connection to the workspace that contains your dbt job.
   - Use an existing connection, or create a new one via the **Get data** page.
4. Select the target **workspace** and the **dbt job** item.
   - If you don’t have a dbt job yet, select **+ New** to create one inline.

![This pictures show how to use dbt job activity](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/this-pictures-show-how-to-use-dbt-job-activity.png)

*Figure: Add dbt job activity.*

## Parameterized workflows in dbt job

You can use **dynamic content** to parameterize all columns in the **Settings** tab. This means any dbt job activity property can be driven by pipeline runtime values.

Example: pass a parameter to the **Select** field so each pipeline run executes only the models that match the provided rules.

![This screenshot shows the functions of Pipeline parameterizations](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/this-screenshot-shows-the-functions-of-pipeline-pa.png)

*Figure: Configure dynamic content for dbt job.*

## Advanced settings for dbt job

From the **Settings** tab you can configure:

- dbt command settings
- Node selection
- Execution behavior

Supported dbt commands include:

- `build`
- `run`
- `compile`
- `snapshot`
- `test`

You can also set filters such as:

- `select`
- `exclude`
- `full refresh`
- `fail fast`

![This is the screenshot for dbt job activity settings](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/this-is-the-screenshot-for-dbt-job-activity-settin.png)

*Figure: dbt job advanced settings.*

## Next steps

This Preview capability is intended to reduce orchestration complexity while improving control and visibility of dbt execution inside Fabric pipelines.

- Orchestrate dbt jobs in Fabric data pipelines: https://aka.ms/dbtjobactivity
- Monitor pipeline runs in Microsoft Fabric: https://learn.microsoft.com/fabric/data-factory/monitor-pipeline-runs


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/orchestrate-dbt-jobs-activity-in-your-fabric-pipelines-preview/)

