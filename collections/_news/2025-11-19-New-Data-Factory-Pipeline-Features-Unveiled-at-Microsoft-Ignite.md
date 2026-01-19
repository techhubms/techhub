---
layout: post
title: New Data Factory Pipeline Features Unveiled at Microsoft Ignite
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/igniting-your-pipelines-new-data-factory-features-announced-at-ignite/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-11-19 11:30:00 +00:00
permalink: /ai/news/New-Data-Factory-Pipeline-Features-Unveiled-at-Microsoft-Ignite
tags:
- Apache Airflow
- Copilot For Data Factory
- Data Factory
- Data Integration
- Data Orchestration
- Delta Tables
- Interval Based Scheduling
- Lakehouse Maintenance
- Microsoft Fabric
- Microsoft Ignite
- Monitor Hub
- Observability
- Optimize Activity
- Parquet
- Pipeline Automation
- Pipeline Expression Builder
- Pipeline Expressions
- Vacuum Activity
section_names:
- ai
- azure
- ml
---
Microsoft Fabric Blog introduces new Data Factory pipeline features launched at Microsoft Ignite, covering code-free orchestration, Airflow DAGs, Copilot-powered expression builder, and advanced scheduling and lakehouse maintenance tools.<!--excerpt_end-->

# New Data Factory Pipeline Features Unveiled at Microsoft Ignite

Microsoft Fabric has introduced a range of innovative enhancements to the Data Factory service at Microsoft Ignite. These features focus on data orchestration, improving usability and productivity for both developer-focused and code-free pipeline users.

## Code-Free and Code-First Data Orchestration

- **Pipelines**: Easily orchestrate data workflows without writing code, streamlining integration processes within Fabric.
- **Airflow Jobs**: For developers preferring code-first approaches, new APIs and UI gestures enhance the experience of building and deploying Airflow DAG-based pipelines.

## Improved Observability

- The updated **Monitor Hub** offers hierarchical views to understand upstream/downstream pipeline activities, facilitating comprehensive impact analysis and simplified troubleshooting.

## Pipeline Expression Builder with Copilot

- Build pipeline expressions using natural language prompts and let **Copilot for Data Factory** generate the required code.
- Supports context-aware prompts, e.g., referencing output from previous activities.
- [Official documentation](https://aka.ms/expressioncopilot) provides more details on usage.

## Upcoming Features

### Interval-Based Schedules

- New scheduling capabilities (inspired by Azure Data Factory's tumbling window triggers) allow pipelines to be executed based on defined intervals (time slices), managed entirely within Fabric.
- [Learn more about pipeline triggers](https://learn.microsoft.com/fabric/data-factory/pipeline-runs).

### Lakehouse Maintenance Activities

- Automate and schedule maintenance of Lakehouse Parquet/Delta tables, ensuring optimal performance and efficient data layout.
- New pipeline activities to vacuum and optimize tables can be set up as part of orchestration flows.
- Further information [here](https://aka.ms/lhmaint).

## Developer Experience Enhancements

- New UI features in the Airflow code editor facilitate file uploads directly to Apache Airflow job projects.
- Expression builder advances aid in workflow efficiency and accessibility for less code-oriented users.

## Community Appreciation

Microsoft expresses gratitude to users and the community, encouraging feedback and success stories to continue shaping Data Factory development.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/igniting-your-pipelines-new-data-factory-features-announced-at-ignite/)
