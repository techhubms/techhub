---
layout: "post"
title: "Gain End-to-End Visibility into Data Activity Using OneLake Diagnostics"
description: "This news update from the Microsoft Fabric Blog details the general availability of OneLake diagnostics, a robust monitoring and governance feature for Microsoft Fabric's unified data lake. The post outlines how organizations can use OneLake diagnostics to track and analyze data access across Fabric workspaces, supporting operations, compliance, and security. Key capabilities include capturing diagnostic events in Lakehouses, integration with analytics tools like Spark and Power BI, and adoption best practices for secure and scalable deployment."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/gain-end-to-end-visibility-into-data-activity-using-onelake-diagnostics/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-10-13 11:00:00 +00:00
permalink: "/2025-10-13-Gain-End-to-End-Visibility-into-Data-Activity-Using-OneLake-Diagnostics.html"
categories: ["Azure", "ML", "Security"]
tags: ["Azure", "Azure Blob Storage", "Compliance", "Data Activity", "Data Governance", "Data Lake", "Data Management", "Diagnostics", "JSON Logs", "Lakehouse", "Microsoft Fabric", "Microsoft Purview", "ML", "News", "OneLake", "Power BI", "Security", "Security Monitoring", "Spark", "Workspace Monitoring"]
tags_normalized: ["azure", "azure blob storage", "compliance", "data activity", "data governance", "data lake", "data management", "diagnostics", "json logs", "lakehouse", "microsoft fabric", "microsoft purview", "ml", "news", "onelake", "power bi", "security", "security monitoring", "spark", "workspace monitoring"]
---

Microsoft Fabric Blog introduces the general availability of OneLake diagnostics, empowering organizations with comprehensive visibility into data activity and governance across Fabric workspaces. Authored by the Microsoft Fabric Blog team.<!--excerpt_end-->

# Gain End-to-End Visibility into Data Activity Using OneLake Diagnostics

Microsoft Fabric's data platform now features general availability of **OneLake diagnostics**, enabling organizations to monitor, analyze, and govern data access across all Fabric workspaces.

## What is OneLake and OneLake Diagnostics?

**OneLake** powers Microsoft Fabric as a unified, virtualized data lake, supporting enterprise-scale data management. The new **diagnostics** capability allows visibility into data access patterns—answering who accessed what, when, and how—across all workspaces, similar to Azure Blob Storage monitoring.

### Key Features

- **Unified Analytics**: All data is virtualized within OneLake, enabling robust, enterprise-wide analytics and governance.
- **Shortcuts & Mirroring**: Integrate external and replicated data without complex ETL.
- **Security & Cataloging**: Enforce granular security and discover data assets efficiently.
- **End-to-End Monitoring**: Gain detailed insight into both user and programmatic access via APIs, pipelines, and analytics engines.

## How OneLake Diagnostics Works

- **Enable Diagnostics**: Workspace Admins turn on diagnostics per workspace and select a Lakehouse to store events.
- **Event Storage**: Diagnostic events are streamed as append-only JSON files.
- **Tool Compatibility**: Analyze diagnostics using Spark, SQL, Eventhouse, Power BI, or any JSON ingestion tool.
- **Access Logging**: Both internal (Fabric workloads) and external (API, SDK) data accesses are logged.
- **Workspace Monitoring**: Integrates with Microsoft Purview for federated governance.
- **Diagnostic Data Privacy**: Personal data fields such as executingUPN and callerIpAddress are redacted until tenant controls are released.

## Supported Analytics and Compliance Scenarios

- Build dashboards tracking data access frequency and trends.
- Support operational visibility for data governance and compliance reporting.
- Use consolidated logs to understand and value data products organization-wide.

## Best Practices

- **Dedicated Diagnostics Workspace**: Store diagnostic events separately with restricted permissions.
- **Centralization**: Aggregate events from multiple workspaces for simpler analysis.
- **Leverage Shortcuts**: Compose diagnostics across capacities and regions with shortcuts.

## Consumption and Pricing

- Diagnostic storage and transfer is metered, with costs mirroring Azure Blob Storage diagnostics.
- Operational events and data movement are billed to the specific capacity as per published rates.

## Monitoring Changes

- Use Microsoft 365 security logs and the `ModifyOneLakeDiagnosticSettings` event to track diagnostics configuration changes.

## Getting Started

1. Visit [OneLake diagnostics documentation](https://learn.microsoft.com/fabric/onelake/onelake-diagnostics-overview) for detailed setup steps.
2. Monitor diagnostic events for operational and compliance scenarios.
3. Integrate with analytics tools for maximum insight.

For more information, visit the official [Microsoft documentation for OneLake diagnostics](https://learn.microsoft.com/fabric/onelake/onelake-diagnostics-overview).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/gain-end-to-end-visibility-into-data-activity-using-onelake-diagnostics/)
