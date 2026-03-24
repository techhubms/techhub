---
primary_section: ml
feed_name: Microsoft Fabric Blog
tags:
- Dataflow Gen2
- Dataflows
- Diagnostics Download
- Execution Logs
- Fabric Data Factory
- Microsoft Fabric
- ML
- Monitoring
- News
- Observability
- Performance Investigation
- Preview Feature
- Refresh History
- Root Cause Analysis
- Run History
- Runtime Diagnostics
- Troubleshooting
section_names:
- ml
author: Microsoft Fabric Blog
title: Dataflow Gen2 – Dataflow Diagnostics Download (Preview)
external_url: https://blog.fabric.microsoft.com/en-US/blog/dataflow-gen2-dataflow-diagnostics-download-preview/
date: 2026-03-24 10:30:00 +00:00
---

Microsoft Fabric Blog announces a preview feature for Dataflow Gen2 in Microsoft Fabric that lets admins and support teams download per-run diagnostic packages, bundling logs and runtime signals to speed up troubleshooting and performance investigations.<!--excerpt_end-->

# Dataflow Gen2 – Dataflow Diagnostics Download (Preview)

*If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at all FabCon and SQLCon announcements across both Fabric and Microsoft’s database offerings:* https://aka.ms/FabCon-SQLCon-2026-news

This preview capability provides administrators and support teams with a simpler, more consistent way to collect diagnostic information for **Dataflow Gen2** runs on execution environments.

Troubleshooting dataflows often requires detailed execution logs and runtime diagnostics. With this update, you can download diagnostic packages directly for a specific dataflow run, which can make it faster to:

- Investigate failures
- Analyze performance issues
- Collaborate with support teams

## Capabilities

Dataflow Diagnostics Download enables you to:

- Download **run-level diagnostic packages** for Dataflow Gen2
- Collect structured logs and execution details in a single package
- Speed up **root cause analysis** and troubleshooting workflows
- Share diagnostics with support or engineering teams

This is intended to reduce the time and friction involved in gathering the right troubleshooting data and improve overall support efficiency.

## How it works

After a Dataflow Gen2 run finishes, you can open its details page and download the diagnostics package. The package includes the runtime information needed for investigation, such as:

- Runtime metadata
- Execution traces
- Environment signals

### Getting started

- Navigate to your **Dataflow Gen2 run history**
- Select a specific run instance
- Choose **Download diagnostics**
- Share the downloaded package with your admin or support contact if deeper analysis is needed

No additional configuration is required for this preview feature.

![Dataflow Gen2 interface showing the Dataflow Diagnostics Download option, allowing users to export diagnostic logs for troubleshooting and support analysis](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/dataflow-gen2-interface-showing-the-dataflow-diagn.png)

*Figure: Download Dataflow Gen2 diagnostics to export detailed logs and runtime information, helping users and support teams investigate failures and performance issues.*

## What’s next

This update is positioned as a step toward more comprehensive observability and self-service troubleshooting for Dataflow Gen2.

Learn more: https://learn.microsoft.com/fabric/data-factory/dataflows-gen2-monitor?branch=main&branchFallbackFrom=pr-en-us-12258#download-detailed-logs-of-the-refresh


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/dataflow-gen2-dataflow-diagnostics-download-preview/)

