---
feed_name: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/invoke-ssis-package-activity-in-microsoft-fabric-preview/
tags:
- .dtsconfig
- .dtsx
- Azure Data Factory
- Azure SSIS Integration Runtime
- Connection Managers
- Copy Activity
- Data Pipelines
- Dataflow Gen2
- ETL
- Execution Logging
- Fabric Data Factory
- Fabric Data Warehouse
- Hybrid ETL
- Invoke SSIS Package Activity
- Lakehouse
- Lift And Shift Migration
- Microsoft Fabric
- ML
- Monitoring
- News
- Notebook Activity
- OneLake
- Pipeline Parameters
- Property Overrides
- SQL Server Integration Services
- SSIS
- SSISDB Catalog
- Stored Procedure Activity
author: Microsoft Fabric Blog
section_names:
- ml
date: 2026-03-25 09:00:00 +00:00
primary_section: ml
title: Invoke SSIS Package Activity in Microsoft Fabric Data Factory (Preview)
---

Microsoft Fabric Blog announces the Preview “Invoke SSIS Package” activity in Fabric Data Factory, explaining how to run existing SSIS (.dtsx) packages from Fabric pipelines with OneLake storage, pipeline parameter overrides, scheduling, and monitoring—aimed at lift-and-shift and hybrid ETL orchestration scenarios.<!--excerpt_end-->

# Invoke SSIS Package Activity in Microsoft Fabric (Preview)

*If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at announcements across both Fabric and database offerings.*

SQL Server Integration Services (SSIS) has long been used for enterprise data integration and ETL workloads. The **Invoke SSIS Package activity (Preview)** in **Data Factory in Microsoft Fabric** is designed to let you run existing SSIS packages directly from **Fabric pipelines**, bringing SSIS orchestration into Fabric’s SaaS analytics platform.

## Why this matters

Many enterprises have SSIS packages orchestrating complex ETL across on-premises databases, file systems, and cloud services. Previously, running SSIS typically required:

- An on-premises SQL Server, or
- The **Azure-SSIS Integration Runtime** in **Azure Data Factory** (which requires managing additional infrastructure)

The new **Invoke SSIS Package pipeline activity** aims to support a “lift-and-shift” approach:

- Execute SSIS packages from a Fabric pipeline
- **No package rewrite required**
- No separate integration runtime provisioning/management (Fabric provides the compute)

## Key benefits

- **Zero rewrite migration**: Run existing SSIS packages as-is inside Fabric pipelines.
- **Unified orchestration**: Combine SSIS execution with Fabric-native activities in a single pipeline, such as:
  - Copy activity
  - Dataflow Gen2
  - Notebook
  - Stored Procedure
- **OneLake integration**:
  - Use **OneLake** as the package store
  - Write package execution logs into OneLake
- **Simplified management**: No need to provision/manage separate integration runtimes.

## Getting started

1. **Upload packages to OneLake**
   - Move your `*.dtsx` (and optional `*.dtsConfig`) files into a **Lakehouse** via OneLake file explorer or the Fabric portal.
2. **Add the activity to a pipeline**
   - Create/open a pipeline, then add **Invoke SSIS Package** from the Activities pane.

![Screenshot of pipeline canvas, select the "Invoke SSIS Package (Preview)" from the dropdown menu under "Activities" tab.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-pipeline-canvas-select-the-invoke-6.png)

*Figure: Add an Invoke SSIS Package activity.*

3. **Configure package settings**
   - Browse/select your package and configuration files from OneLake.
   - Optionally enable logging to capture execution logs in OneLake.

![Screenshot of Invoke SSIS package activity configuration. It shows options for connection selection, package and configuration paths, encryption password input, logging enablement with level set to Basic, and browsing capabilities for file paths.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-invoke-ssis-package-activity-configu.png)

*Figure: Invoke SSIS package activity configuration.*

4. **Set runtime values**
   - Override connection manager properties or package properties (for example, connection strings and credentials) using:
     - **Connection Managers** tab
     - **Property Overrides** tab
   - Dynamic values can come from expressions, pipeline parameters, or system variables.
5. **Run or schedule**
   - Save the pipeline and run immediately or configure a recurring schedule.
6. **Monitor execution**
   - Track progress in the pipeline **Output** tab or the workspace **Monitor** hub.
   - If logging is enabled, detailed execution logs are written to OneLake.

For full configuration details, see: Invoke SSIS Package activity documentation: https://aka.ms/SSISActivity

## Common scenarios

## Scenario 1: Lift and shift from Azure Data Factory

If you currently use the **Azure-SSIS Integration Runtime** in **Azure Data Factory**, you can migrate SSIS orchestration to Fabric pipelines.

Suggested approach:

- Recreate your ADF pipeline in Fabric
- Replace **Execute SSIS Package** with **Invoke SSIS Package**
- Point to the same **SSISDB catalog**

## Scenario 2: Hybrid ETL orchestration

Combine SSIS packages with Fabric-native capabilities in one pipeline, for example:

1. Use **Copy activity** to ingest raw data from a cloud SaaS source into OneLake.
2. Run **Invoke SSIS Package** to apply existing SSIS transformation logic.
3. Use **Dataflow Gen2** for final shaping before loading into a **Fabric Data Warehouse**.

This is positioned as an incremental modernization path: convert SSIS packages to Fabric-native activities over time while maintaining production workloads.

## Get started (quick checklist)

1. Open Data Factory in Microsoft Fabric: https://learn.microsoft.com/fabric/data-factory/data-factory-overview
2. Create a new Data Pipeline in your workspace.
3. Add **Invoke SSIS Package** and configure it to point to your packages in OneLake.
4. Use the documentation for detailed guidance: https://aka.ms/SSISActivity

The post also asks for feedback via the Fabric community or through your Microsoft account team.

## What’s next

This Preview is described as the first step, with additional work planned, including:

- Expanded package sources
- On-premises and private network connectivity
- Custom or third-party component support

These features are expected to roll out via private previews. Early access sign-up: https://aka.ms/FabricSSIS

## Learn more

- Invoke SSIS Package activity documentation: https://aka.ms/SSISActivity
- Data Factory in Microsoft Fabric overview: https://learn.microsoft.com/fabric/data-factory/data-factory-overview
- Tutorial: Integrate SSIS with SQL database in Microsoft Fabric: https://learn.microsoft.com/sql/integration-services/fabric-integration/integrate-fabric-sql-database
- The Evolution of SQL Server Integration Services (SSIS): SSIS 2025 (Generally Available): https://blog.fabric.microsoft.com/blog/the-evolution-of-sql-server-integration-services-ssis-ssis-2025-generally-available


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/invoke-ssis-package-activity-in-microsoft-fabric-preview/)

