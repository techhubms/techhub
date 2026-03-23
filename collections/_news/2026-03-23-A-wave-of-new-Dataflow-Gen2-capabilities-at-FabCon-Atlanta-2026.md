---
feed_name: Microsoft Fabric Blog
author: Microsoft Fabric Blog
section_names:
- ai
- azure
- ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/a-wave-of-new-dataflow-gen2-capabilities-at-fabcon-atlanta-2026/
title: A wave of new Dataflow Gen2 capabilities at FabCon Atlanta 2026
primary_section: ai
date: 2026-03-23 08:00:00 +00:00
tags:
- ADLS Gen2
- AI
- AI Powered Transforms
- Azure
- Azure Data Lake Storage Gen2
- CI/CD
- Data Destinations
- Data Factory
- Dataflow Gen2
- Diagnostics Logs
- ELT
- Email Alerts
- ETL
- Excel File Destination
- Execute Query API
- Fabric SQL Database
- Git Integration
- Lakehouse
- MCP
- Microsoft Fabric
- ML
- Modern Query Evaluator
- Monitoring
- News
- Parameterization
- Power Query
- Power Query M
- Relative References
- Scheduling
- SharePoint Connector
- Snowflake Destination
- Streaming API
- Variable Libraries
- VNET Data Gateway
- VS Code Integration
- Warehouse
- Workspace Variables
---

Microsoft Fabric Blog summarizes FabCon Atlanta 2026 updates for Dataflow Gen2 in Microsoft Fabric, covering production-ready improvements (performance, destinations, scheduling, portability) and previews like AI-assisted transforms, a streaming Execute Query API, and the Data Factory MCP server for AI-assisted authoring and automation.<!--excerpt_end-->

## Overview

At FabCon Atlanta 2026, Microsoft announced a set of updates for **Dataflow Gen2** in **Microsoft Fabric**. Dataflow Gen2 brings **low-code data transformation** to Fabric using **Power Query**, with workspace-based scheduling, reuse, and governance.

The announcements are split into:

- **Generally Available (GA)**: production-ready capabilities
- **Preview**: early features available for testing and feedback

Related roundup: Arun Ulag’s post, [FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news)

## Generally Available: Production-ready enhancements for Dataflow Gen2

### Modern Query Evaluator: faster, more reliable refresh performance

The **Modern Query Evaluator** improves performance and reliability for Power Query transformations in Dataflow Gen2.

Key improvements:

- Faster refreshes for multi-step shaping (joins, group-bys, type conversions, complex expressions)
- More predictable execution at larger scale and higher-frequency schedules

Docs: [Modern Query Evaluator documentation](https://learn.microsoft.com/fabric/data-factory/dataflow-gen2-modern-evaluator)

### Preview-only steps iterate efficiently without impacting refresh

**Preview Only Steps** let you add steps that run during preview/authoring, but are excluded from the production refresh execution.

Use cases:

- Speed up authoring by sampling/filtering/limiting rows at design time
- Keep experimentation out of scheduled refresh logic

![Screenshot of the Power Query editor in Dataflow Gen2 with the contextual menu of a step showing the enable only in previews option.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-the-power-query-editor-in-dataflow-g-3.png)

Docs: [Preview Only Steps documentation](https://learn.microsoft.com/fabric/data-factory/dataflow-gen2-preview-only-step)

### Fabric Variable Libraries: parameterize once, promote everywhere

Dataflow Gen2 now supports **Fabric Variable Libraries**, enabling runtime resolution of variables based on workspace context.

Benefits:

- Portability across dev/test/prod workspaces
- Reduced configuration drift (avoid hard-coded endpoints/paths/destinations)

![Screenshot of the FIlter rows dialog within Dataflow Gen2 showing the input widget and the option to Select a workspace variable](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-the-filter-rows-dialog-withwithin-datafl-3.png)

![Screenshot of the select variable dialog invoked from within a Dataflow Gen2](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-the-select-variable-dialog-invoked-f-3.png)

Docs: [Fabric Workspace Variables documentation (GA) and Dataflow Gen2 integration](https://learn.microsoft.com/fabric/data-factory/dataflow-gen2-variable-library-integration)

### Relative references: move/copy dataflows with fewer broken connections

**Relative references** allow Dataflow Gen2 to reference Fabric items relative to the current workspace context (rather than hard-coded identifiers).

Benefits:

- Fewer broken connections when copying between workspaces
- Easier template-based reuse

![Screenshot of !(Current Workspace) node in the Lakehouse connector](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-current-workspace-node-in-the-lak.png)

Docs: [Relative references to Fabric items documentation (GA)](https://learn.microsoft.com/fabric/data-factory/dataflow-gen2-relative-references)

### Stay informed with email alerts for failed scheduled refreshes

Dataflow Gen2 can now send **email notifications** when a **scheduled refresh** fails.

Stated outcomes:

- Faster response and time-to-detect
- Less manual monitoring
- More reliable downstream pipelines/reports

### Schedule runs with parameters: use one dataflow for multiple scenarios

You can pass **parameter values** when triggering **scheduled runs**.

Stated advantages:

- Reduce duplication (avoid cloning dataflows for different inputs)
- Standardize transformation logic across workloads
- Combine with variables and relative references for easier environment promotion

### ADLS Gen2 destination (GA)

Dataflow Gen2 supports **Azure Data Lake Storage Gen2 (ADLS Gen2)** as a destination.

Use cases:

- Lake-first ingestion where ADLS is the system of record
- Reuse across Fabric (Spark/SQL) and external systems

Docs: [Data destinations for Dataflow Gen2 – ADLS Gen2 (GA)](https://learn.microsoft.com/fabric/data-factory/dataflow-gen2-data-destinations-and-managed-settings#set-up-file-based-destinations)

### Lakehouse Files destination (GA)

You can write outputs directly into the **Files** area of a Fabric **lakehouse**.

Use cases:

- File-based extracts for downstream notebooks/pipelines/external consumers
- Hybrid patterns (some outputs as tables, some as files)

Docs: [Data destinations for Dataflow Gen2 – Lakehouse Files (GA)](https://learn.microsoft.com/fabric/data-factory/dataflow-gen2-data-destinations-and-managed-settings#lakehouse-files-or-tables)

### Schema-aware destinations (schema targeting)

Destinations support writing into specific **schemas** (where applicable), including:

- Fabric SQL databases
- Lakehouses
- Warehouses

Stated benefits:

- Organize tables by domain (finance, sales, HR)
- Better multi-team publishing and ownership boundaries

![Screenshot of the connect to data destination dialog showing a Warehouse connection leveraging the navigate using full hierarchy](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-the-connect-to-data-destination-dial.png)

Docs: [Data destinations schema support documentation](https://learn.microsoft.com/fabric/data-factory/dataflow-gen2-data-destinations-and-managed-settings#schema-support-for-lakehouse-warehouse-and-sql-databases-preview)

### AI-Powered Transforms (Prompt): generate shaping steps from intent

**AI-Powered Transforms** allow natural language prompting to generate transformation logic.

Stated benefits:

- Faster onboarding for new users learning Power Query
- Faster prototyping for experienced users (then fine-tune generated M)

![Screenshot of the AI Prompt dialog in Dataflow Gen2](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-the-ai-prompt-dialog-in-dataflow-gen-3.png)

Docs: [Fabric AI Prompt in Dataflow Gen2 (Preview)](https://learn.microsoft.com/fabric/data-factory/dataflow-gen2-ai-functions)

### Export Query Results (Power BI Desktop)

You can **export query results** from the Power Query authoring experience in **Power BI Desktop**.

Use cases:

- Troubleshooting by exporting shaped datasets
- Collaboration by sharing snapshots with teammates/business/support

Docs: [Export query results in Power Query documentation](https://learn.microsoft.com/power-bi/transform-model/export-query-results)

### Faster publishing: refreshed UX and parallel validations

Publishing improvements include UI refresh and **parallelized query validations**, aimed at reducing publish time for multi-query dataflows.

Docs: [Publishing Dataflow Gen2 documentation](https://learn.microsoft.com/fabric/data-factory/dataflow-gen2-cicd-and-git-integration#save-replaces-publish)

### Save As upgrades: scheduled refresh policies + new API

Updates to **Save As** for upgrading from Dataflow Gen1 to Gen2:

- Save As now supports **Scheduled Refresh Policies**
- New **Save As API** for automation and bulk migration

![Dialogs for the refresh and scheduling mechanism when using the Save as experience for Dataflow Gen2](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/dialogs-for-the-refresh-and-scheduling-mechanism-w-3.png)

Docs:

- [Save As Dataflow Gen2 documentation](https://learn.microsoft.com/fabric/data-factory/migrate-to-dataflow-gen2-using-save-as#save-a-dataflow-gen1-as-a-new-dataflow-gen2-cicd)
- [Save As API reference](https://learn.microsoft.com/rest/api/power-bi/dataflows/save-dataflow-gen-one-as-dataflow-gen-two)

### Power Automate Dataflows connector: orchestrate Dataflow Gen2 CI/CD items

The Power Automate **Dataflows connector** supports Dataflow Gen2 CI/CD items, enabling workflow orchestration around refreshes and deployments.

Docs: [Dataflows connector in Power Automate](https://learn.microsoft.com/connectors/dataflows/)

## Preview Available: try the latest updates

### SharePoint Site Picker (Modern Get Data)

A new SharePoint site selection experience to help find the correct site faster (especially for large tenants).

- Faster onboarding without needing the exact URL
- Fewer connection errors

![SharePoint Site Picker dropdown.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/word-image-34552-8.gif)

Related connectors:

- [SharePoint folder connector](https://learn.microsoft.com/power-query/connectors/sharepoint-folder)
- [SharePoint list connector](https://learn.microsoft.com/power-query/connectors/sharepoint-list)
- [SharePoint online list (Fabric)](https://learn.microsoft.com/fabric/data-factory/connector-sharepoint-online-list)

### SharePoint Site Picker (Data Destinations)

The same SharePoint Site Picker improvements are coming to the destination selection experience.

- Less manual URL handling
- Consistent UX between sources and destinations

### Recent Tables (Modern Get Data)

**Recent Tables** helps you quickly reconnect to recently used tables.

- Faster iteration across sessions
- Better discoverability in shared/complex sources

### Advanced Edit for destinations

A new destination **Advanced Edit** enables editing underlying **M logic** for destination configuration.

- Parameter-driven destination behavior (schema/table/path/naming conventions)
- More customization than the simplified UI

![Screenshot of the new Advanced editor for data destinations](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-the-new-advanced-editor-for-data-des-3.png)

Docs: [Data Destinations Advanced Edit documentation](https://learn.microsoft.com/fabric/data-factory/dataflow-gen2-advanced-edit-data-destinations)

### New destination: publish directly to Snowflake

Dataflow Gen2 supports **Snowflake** as a destination (Preview).

- Hybrid data estate: consistent transforms while landing data into Snowflake
- Analyst enablement: publish standardized outputs to governed Snowflake targets

Docs: [Snowflake data destination for Dataflow Gen2 documentation](https://learn.microsoft.com/fabric/data-factory/dataflow-gen2-data-destinations-and-managed-settings)

### New destination: write outputs as Excel files (filesystem destinations)

Ability to write outputs as **Excel files** (Preview) for supported filesystem destinations like SharePoint and ADLS Gen2.

Docs: [Excel file data destination documentation](https://review.learn.microsoft.com/fabric/data-factory/dataflow-gen2-data-destinations-excel-advanced)

### Diagnostics download: logs for cloud and VNET gateway dataflows

**Dataflow Diagnostics Download** (Preview) supports both cloud and **VNET gateway** dataflows.

- Download logs/artifacts for root-cause analysis
- Better support for complex networking scenarios

![Screenshot of the recent runs dialog showing the new button at the bottom left of the dialog to Download detailed logs](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-the-recent-runs-dialog-showing-the-n-3.png)

Docs: [Dataflow Gen2 diagnostics documentation](https://learn.microsoft.com/fabric/data-factory/dataflows-gen2-monitor)

### Publish-time destination checks

Publishing includes **data destination validations** (Preview) to catch common issues earlier:

- Missing permissions
- Invalid destination settings
- Naming conflicts

### Execute Query (Streaming) API

The **Execute Query API** (Preview) enables on-demand Power Query execution without a full scheduled refresh cycle.

Use cases:

- Event-driven pipelines
- Near-real-time/streaming scenarios with more frequent execution
- Automation at scale through orchestration tools/scripts
- Faster debugging by re-running targeted queries

Docs: [Execute Query API (Streaming) documentation](https://blog.fabric.microsoft.com/blog/execute-power-query-programmatically-in-microsoft-fabric?ft=All)

### Data Factory MCP

**Data Factory MCP Server** (Preview) exposes Dataflow Gen2 and pipeline capabilities (creation, M authoring, connection management, query execution, refresh orchestration) as tools callable by AI assistants (VS Code, Claude, ChatGPT, Gemini) or CLI.

Why it matters (as stated):

- Create/test/deploy dataflows via natural language
- Use `execute_query` for iterative M development against live data
- MCP Apps provide guided UI forms in chat (connection setup, gateway selection)
- Open source and runs locally; credentials don’t leave your machine

Links:

- GitHub: [microsoft/DataFactory.MCP](https://github.com/microsoft/DataFactory.MCP)
- NuGet: [Microsoft.DataFactory.MCP](https://www.nuget.org/packages/Microsoft.DataFactory.MCP)

## Next steps

Resources referenced in the post:

- [Dataflow Gen2 Overview](https://learn.microsoft.com/fabric/data-factory/dataflows-gen2-overview)
- [Data Factory Roadmap](https://roadmap.fabric.microsoft.com/?product=datafactory)
- Feedback: [Fabric Ideas Forum](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/a-wave-of-new-dataflow-gen2-capabilities-at-fabcon-atlanta-2026/)

