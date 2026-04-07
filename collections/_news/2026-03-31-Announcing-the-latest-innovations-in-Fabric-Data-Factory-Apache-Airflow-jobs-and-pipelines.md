---
primary_section: ml
date: 2026-03-31 11:30:00 +00:00
author: Microsoft Fabric Blog
section_names:
- ml
tags:
- Airflow APIs
- Airflow DAGs
- Apache Airflow
- Copy Jobs
- Data Pipelines
- Dbt
- ELT
- ETL
- Event Driven Workflows
- Fabric Data Factory
- Interval Based Schedules
- Microsoft Fabric
- ML
- News
- Notebooks
- Orchestration
- Parameterization
- Reusable Templates
- Semantic Models
- Spark Job Definitions
- Tumbling Window Trigger
- Visual Pipeline Designer
- Workflow Automation
title: 'Announcing the latest innovations in Fabric Data Factory: Apache Airflow jobs and pipelines'
feed_name: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/announcing-the-latest-innovations-in-fabric-data-factory-apache-airflow-jobs-and-pipelines/
---

Microsoft Fabric Blog announces new Fabric Data Factory capabilities for Apache Airflow orchestration and pipeline authoring, including native operators to run Fabric items from DAGs, new Airflow job APIs, and preview interval-based scheduling to support common ETL patterns.<!--excerpt_end-->

# Announcing the latest innovations in Fabric Data Factory: Apache Airflow jobs and pipelines

## Introduction

The Microsoft Fabric team has released new updates for **Fabric Data Factory** focused on improving **data integration and orchestration**, especially for teams using **Apache Airflow** and Fabric pipelines.

## What’s new in Fabric Data Factory Apache Airflow jobs?

### Airflow integration: Orchestrate with confidence

**Apache Airflow jobs in Fabric Data Factory** now simplify running various Fabric items with native operator support.

From an Airflow **DAG**, you can execute Fabric artifacts such as:

- Notebooks
- Spark job definitions
- Pipelines
- Semantic Models
- User data functions

Airflow jobs now also support running:

- **Copy jobs**
- **dbt jobs**

![Apache Airflow jobs configuration in Fabric Data Factory showing support for copy and dbt jobs](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/apache-airflow-jobs-in-fabric-data-factory-with-su.png)

You can also add code to run Fabric items using a shortcut: open the context menu and select **Run Fabric Artifact**.

![Context menu shortcut to run a Fabric item from Apache Airflow](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/running-a-fabric-item-from-apache-airflow-using-th.png)

Documentation:

- Run a Fabric item using Apache Airflow DAG: https://learn.microsoft.com/fabric/data-factory/apache-airflow-jobs-run-fabric-item-job

### New Airflow APIs: Elevate your orchestration experience

This release also introduces new **Apache Airflow job APIs** to support workflow integration and automation. The APIs are positioned to help you:

- Programmatically **manage, monitor, and trigger** Airflow DAG runs
- Build integrations from applications and services
- Support scheduled and event-driven orchestration scenarios

Documentation:

- API capabilities for Fabric Data Factory’s Apache Airflow Job: https://learn.microsoft.com/fabric/data-factory/apache-airflow-jobs-api-capabilities

## What’s new in Fabric Data Factory pipelines?

Fabric Data Factory pipelines received updates aimed at making data movement and transformation workflows easier to build and maintain:

- **Enhanced Visual Pipeline Designer**: more intuitive drag-and-drop, with improved ability to build, visualize, and debug flows
- **Parameterization and reusability**: advanced parameterization to support dynamic pipelines and reusable templates for common ETL patterns

### Interval-based schedules (Preview)

A new **interval-based schedule** option (Preview) supports regular, non-overlapping intervals—similar to the **tumbling window trigger** pattern in Azure Data Factory.

This is intended to help automate time-based workflows, ensuring consistent recurring pipeline runs for time-dependent ETL workloads.

![Interval-based schedule configuration for Fabric Data Factory pipelines (Public Preview)](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/interval-based-schedule-configuration-for-fabric-d.png)

## Get started

To explore these features and related guidance:

- Fabric Data Factory documentation hub: https://learn.microsoft.com/fabric/data-factory/

## Join the conversation

The post invites readers to share how they’re using these features and what problems they’re solving via the comments section.

[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/announcing-the-latest-innovations-in-fabric-data-factory-apache-airflow-jobs-and-pipelines/)

