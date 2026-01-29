---
external_url: https://blog.fabric.microsoft.com/en-US/blog/announcing-copy-job-activity-now-general-available-in-data-factory-pipeline/
title: Copy Job Activity Now Generally Available in Microsoft Fabric Data Factory Pipelines
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-11-24 11:00:00 +00:00
tags:
- Bulk Data Transfer
- Change Data Capture
- Copy Job Activity
- Data Factory
- Data Integration
- Data Movement
- Data Orchestration
- Data Pipelines
- Email Notification
- ETL
- Incremental Copy
- Microsoft Fabric
- No Code
- Pipeline Automation
- Pipeline Scheduling
- Telemetry
- Azure
- Machine Learning
- News
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog announces the general availability of Copy Job Activity in Data Factory pipelines, highlighting new capabilities for automating and orchestrating data movement workflows.<!--excerpt_end-->

# Copy Job Activity Now Generally Available in Microsoft Fabric Data Factory Pipelines

## Overview

Microsoft Fabric has announced the general availability of the **Copy Job Activity** in Data Factory pipelines—a significant improvement for users needing to move and orchestrate data across various sources and destinations efficiently.

## What is Copy Job Activity?

- **Copy Job Activity** lets users embed Copy jobs directly inside Data Factory pipelines, treating them as native activities.
- Copy jobs were previously managed separately and offered a lightweight, fast approach for moving data. Now, this experience is integrated within pipeline orchestration, supporting both prototyping and complex, automated workflows.

## Key Features

- **No-code simplicity:** Users can select or create Copy jobs from the pipeline canvas without writing code.
- **Flexible orchestration:** Chain Copy jobs with activities like notebooks, dataflows, conditional branches, or Power BI refresh—ideal for advanced workflow scenarios and medallion architecture implementations.
- **Email integration:** Pair Copy Job Activity with Outlook Activity for automated notifications and status updates from your pipelines.
- **Comprehensive data movement:** Supports batch and incremental copy with change data capture (CDC), including monitoring via built-in telemetry.
- **Reuse and reliability:** Logic previously defined in Copy jobs is reusable within pipelines, while new options enable conditional execution, chaining, and retry logic.

## Usage Scenarios

- Automating periodic data ingestion from multiple sources.
- Chaining data movement with downstream analytics or reporting tasks.
- Triggering workflows that involve notifications or multiple delivery styles (bulk, incremental, CDC replication).

## Further Resources

- [Copy job Activity in Data Factory pipelines](https://learn.microsoft.com/fabric/data-factory/copy-job-activity)
- [What is Copy job in Data Factory](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job)
- [Blog: Simplifying Data Ingestion with Copy job](https://blog.fabric.microsoft.com/blog/simplifying-data-ingestion-with-copy-job-incremental-copy-ga-lakehouse-upserts-and-new-connectors)

## Conclusion

Whether you are building quick prototypes or orchestrating complex data pipelines, Copy Job Activity offers a powerful and simplified way to manage data movement within Microsoft Fabric Data Factory.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/announcing-copy-job-activity-now-general-available-in-data-factory-pipeline/)
