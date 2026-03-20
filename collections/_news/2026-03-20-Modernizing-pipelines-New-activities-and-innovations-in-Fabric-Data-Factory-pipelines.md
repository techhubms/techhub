---
tags:
- AI
- Copilot
- DataOps
- Expression Builder
- Fabric Data Factory
- Fabric Pipelines
- Generally Available
- Lakehouse
- Lakehouse Maintenance Activity
- Lakehouse Utility Suite
- Microsoft Fabric
- ML
- Natural Language To Expression
- News
- Pipeline Expressions
- Pipeline Orchestration
- Preview Features
- Refresh SQL Endpoint Activity
- SQL Endpoint
section_names:
- ai
- ml
title: 'Modernizing pipelines: New activities and innovations in Fabric Data Factory pipelines'
external_url: https://blog.fabric.microsoft.com/en-US/blog/modernizing-pipelines-new-activities-and-innovations-in-fabric-data-factory-pipelines/
primary_section: ai
feed_name: Microsoft Fabric Blog
date: 2026-03-20 11:45:00 +00:00
author: Microsoft Fabric Blog
---

Microsoft Fabric Blog announces new Fabric Data Factory pipeline capabilities, including Lakehouse maintenance and SQL endpoint refresh activities, plus Copilot-assisted pipeline expression authoring to speed up building and maintaining Fabric Lakehouse workflows.<!--excerpt_end-->

# Modernizing pipelines: New activities and innovations in Fabric Data Factory pipelines

*If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at all of the FabCon and SQLCon announcements across both Fabric and database offerings.*

## Introducing the Lakehouse utility suite in Fabric pipelines (Preview)

The **Lakehouse Utility Suite** in Fabric pipelines introduces purpose-built activities to operationalize key Lakehouse tasks directly inside pipelines.

This update introduces two new activities:

- **Lakehouse Maintenance activity (Preview)**
- **Refresh SQL Endpoint activity (Preview)**

### Lakehouse maintenance activity (Preview)

Keep a Lakehouse healthy with automated upkeep.

![Lakehouse maintenance activity in the Fabric pipeline canvas](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/lakehouse-maintenance-activty.png)

*Figure: The Lakehouse maintenance activity in the Fabric pipeline’s canvas.*

You can use this activity to schedule routine maintenance tasks such as:

- Vacuuming old files
- Optimizing table layouts
- Managing storage and performance upkeep

![Lakehouse Maintenance activity settings in Fabric pipelines](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/lakehouse-maintenance-activity-settings.png)

*Figure: The activity settings for the Lakehouse maintenance activity.*

The post highlights that you can run this nightly or integrate it into a broader **DataOps** workflow to keep performance high and storage costs predictable without relying on manual scripts.

Docs: [https://aka.ms/LakehouseMaintenanceDocs](https://aka.ms/LakehouseMaintenanceDocs)

### Refresh SQL endpoint activity (Preview)

A new activity is available to keep the SQL analytics layer fresh and consistent.

![Refresh SQL Endpoint activity in the Fabric pipeline canvas](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/refresh-sql-endpoint-activity.png)

*Figure: The Refresh SQL endpoint activity in Fabric pipeline’s canvas.*

Use this activity to:

- Refresh your Lakehouse **SQL Endpoint** on demand or on a schedule
- Sync SQL refreshes with ingestion or transformation pipelines
- Ensure BI and reporting scenarios read the latest state

The post notes this is especially useful for downstream users who need reliable SQL performance.

![Refresh SQL Endpoint activity settings in Fabric pipelines](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/refresh-sql-endpoint-activity-settings.png)

*Figure: The activity settings for the Refresh SQL endpoint activity.*

Docs: [https://aka.ms/RSQLDocs](https://aka.ms/RSQLDocs)

## Copilot in the Pipeline expression builder (Generally Available)

**Copilot in the Pipeline expression builder (GA)** lets you describe pipeline expression logic in natural language and have Copilot generate the expression.

![Copilot experience in the pipeline expression builder](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/pipeline-expression-builder-experience.png)

*Figure: The Pipeline expression builder Copilot experience.*

Copilot can help generate expressions for:

- Dynamic paths
- Conditional logic
- String manipulation and parsing
- Parameterization and reusable logic

The GA release is described as improving generation accuracy, speed, and contextual suggestions in the pipeline builder.

Docs: [https://aka.ms/CopilotPipelineExpression](https://aka.ms/CopilotPipelineExpression)

## Summary

These capabilities are positioned as part of Fabric’s effort to make Lakehouse operations more automated and predictable within **Fabric Data Factory pipelines**, supporting scenarios like:

- Nightly DataOps processes
- Reducing manual maintenance
- More reliable downstream analytics

The post invites feedback on which scenarios readers want to optimize next.


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/modernizing-pipelines-new-activities-and-innovations-in-fabric-data-factory-pipelines/)

