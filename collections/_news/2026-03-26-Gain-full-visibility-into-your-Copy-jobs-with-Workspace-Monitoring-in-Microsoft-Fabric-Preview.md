---
primary_section: ml
title: Gain full visibility into your Copy jobs with Workspace Monitoring in Microsoft Fabric (Preview)
tags:
- Audit Trail
- Bulk Copy
- Capacity Planning
- Change Data Capture
- Copy Job
- Data Activator
- DirectQuery
- Fabric Data Factory
- Fabric Lakehouse
- Incremental Copy
- KQL
- Kusto Query Language
- Microsoft Fabric
- ML
- Monitoring Eventhouse
- News
- Observability
- Power BI Desktop
- Power BI Report Template
- Semantic Model
- Throughput
- Workspace Monitoring
external_url: https://blog.fabric.microsoft.com/en-US/blog/gain-full-visibility-into-your-copy-jobs-with-workspace-monitoring-in-microsoft-fabric-preview/
author: Microsoft Fabric Blog
date: 2026-03-26 09:00:00 +00:00
feed_name: Microsoft Fabric Blog
section_names:
- ml
---

Microsoft Fabric Blog explains how Workspace Monitoring (preview) adds end-to-end observability for Fabric Data Factory Copy jobs by streaming execution logs into a Monitoring Eventhouse (KQL database), enabling failure analysis, performance tracking, and reporting via KQL/SQL and Power BI.<!--excerpt_end-->

# Gain full visibility into your Copy jobs with Workspace Monitoring in Microsoft Fabric (Preview)

## Introduction

[Copy job](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job) in **Microsoft Fabric Data Factory** is used for data movement across multiple clouds. It supports:

- Bulk copy
- Incremental copy
- Change data capture (CDC) replication

[Workspace monitoring](https://learn.microsoft.com/fabric/fundamentals/workspace-monitoring-overview) is a built-in Fabric capability that provides log-level visibility across all items in a workspace.

When enabled:

- Fabric streams execution logs to a **Monitoring Eventhouse**.
- The Eventhouse is a secure, read-only **KQL database** in your workspace.
- Logs are queryable using **KQL (Kusto Query Language)** or **SQL**.
- Access is restricted to workspace users with at least the **Contributor** role, keeping monitoring data within your governance boundary.

This article focuses on how Workspace Monitoring for **Copy job** provides end-to-end observability for data movement scenarios—from ad-hoc copies to large-scale ingestion.

![Figure Screenshot of PBI Report against Fabric Workspace Monitoring metric from Copy job](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/figure-screenshot-of-pbi-report-against-fabric-wor.png)

*Figure: Screenshot of PBI Report against Fabric Workspace Monitoring metric from Copy job.*

## Why Workspace Monitoring for Copy job matters

As Copy jobs scale from a handful to hundreds/thousands, monitoring and diagnosing data movement becomes essential. Workspace Monitoring helps with:

- **Centralized observability**
  - A single, queryable log of every Copy job run in the workspace.
  - Aggregate/filter/trend across runs without inspecting jobs one by one.

- **Root-cause analysis for failures**
  - Logs capture **ErrorCode** and **FailureType** per activity run.
  - Source/destination details and timing help identify issues like connectivity, schema mismatch, timeouts, or capacity.

- **Performance and throughput analysis**
  - Metrics include **ThroughputBytesPerSec**, **DurationMs**, **DataReadKB**, **DataWrittenKB**.
  - Helps spot bottlenecks and correlate throughput with time windows and source types.

- **Historical audit trail**
  - Eventhouse retains historical logs for compliance, auditing, and capacity planning.

- **Cross-item correlation**
  - Since Workspace Monitoring spans multiple Fabric item types, you can correlate across the data lifecycle.
  - Examples:
    - “Did my Copy job CDC replication complete before the Power BI semantic model refreshed?”
    - “How does ingestion latency into Fabric Lakehouse correlate with Copy job throughput in the same time window?”

- **Proactive alerting**
  - Use **Data Activator** to build rules for notifications (job failures, throughput drops, SLA latency issues).

## What gets logged from Copy job in Workspace Monitoring

A Copy job can include multiple source-to-destination mappings (tables or files). Each mapping produces a separate activity-run record.

Example: if a Copy job moves three source tables to three destination tables, you’ll see **three activity-run records per execution**.

Each activity-run log entry includes:

| Category | Fields |
| --- | --- |
| Identity & context | ItemId, ItemName, WorkspaceId, WorkspaceName, CapacityId, Region |
| Run details | CopyJobRunId, RunId, ScheduledTime, StartTime, EndTime, DurationMs, Status |
| Source & destination | SourceType, SourceName, SourceConnectionType, DestinationType, DestinationName, DestinationConnectionType |
| Data movement metrics | RowsRead, RowsWritten, FilesRead, FilesWritten, DataReadKB, DataWrittenKB, ThroughputBytesPerSec |
| Error diagnostics | ErrorCode, FailureType |

*Table: Copy job runs log for workspace monitoring.*

## How to enable Workspace Monitoring and build reports for Copy job

### Step 1: Enable Workspace Monitoring

1. In your Fabric workspace, open **Workspace settings**.
2. Select the **Monitoring** tab.
3. Turn on **Log workspace activity**.

Fabric creates a **Monitoring Eventhouse** and a read-only **KQL database** in your workspace, then streams all execution logs automatically.

![Screenshot of workspace settings showing the option to toggle on workspace monitoring.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-workspace-settings-showing-the-optio.png)

*Figure: Screenshot of enabling workspace monitoring.*

### Step 2: Run your Copy jobs

After enabling workspace monitoring, Copy job runs automatically write to the **CopyJobActivityRunDetailsLogs** table.

- No per-job setup is required.

### Step 3: Query your logs with KQL

Open the Monitoring KQL database and explore Copy job logs.

- Example queries: [Workspace Monitoring for Copy Job in Microsoft Fabric](https://learn.microsoft.com/fabric/data-factory/copy-job-workspace-monitoring#example-kql-queries-for-copyjobactivityrundetailslogs)

### Step 4 (optional): Build a Power BI report

You can turn monitoring data into dashboards for the team.

- Example template: [Power BI report template (.pbit)](https://github.com/microsoft/fabric-toolbox/tree/FWM-2026.2.1/monitoring/workspace-monitoring-dashboards)
- It connects to your Monitoring Eventhouse and includes visuals for Copy job and other Fabric workloads.

To deploy the template:

1. Download **Fabric Workspace Monitoring Report.pbit** from the [fabric-toolbox repository](https://github.com/microsoft/fabric-toolbox/tree/FWM-2026.2.1/monitoring/workspace-monitoring-dashboards).
2. Open the template in **Power BI Desktop**.
3. Set the **Query URI** parameter to your workspace Monitoring Eventhouse KQL database endpoint.
4. Publish the report to your Fabric workspace.

To build your own report:

- Connect to the Monitoring KQL database via **DirectQuery**.
- Build visuals on **CopyJobActivityRunDetailsLogs**.

## Learn more

- [Workspace monitoring for Copy job documentation](https://learn.microsoft.com/fabric/data-factory/copy-job-workspace-monitoring)
- [Enable workspace monitoring in Microsoft Fabric](https://learn.microsoft.com/fabric/fundamentals/enable-workspace-monitoring)
- [Workspace monitoring overview](https://learn.microsoft.com/fabric/fundamentals/workspace-monitoring-overview)
- [Fabric Workspace Monitoring Report Templates (fabric-toolbox)](https://github.com/microsoft/fabric-toolbox/tree/FWM-2026.2.1/monitoring/workspace-monitoring-dashboards)
- [How to monitor a Copy job](https://learn.microsoft.com/fabric/data-factory/monitor-copy-job)

Feedback/community:

- Submit feedback on Fabric Ideas: Fabric Ideas (Data Factory | Copy job)
- Join the discussion in the Fabric Community: the Fabric Community (Copy job)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/gain-full-visibility-into-your-copy-jobs-with-workspace-monitoring-in-microsoft-fabric-preview/)

