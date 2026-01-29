---
layout: "post"
title: "Workspace Outbound Access Protection for Data Factory and OneLake Shortcuts (Preview)"
description: "This article from the Microsoft Fabric Blog introduces workspace-level Outbound Access Protection (OAP) for Data Factory and OneLake Shortcuts in Microsoft Fabric. It details the security, compliance, and management enhancements enabled by OAP, discusses supported workloads and configuration guidance, and highlights new capabilities for administrators to control outbound connections at a granular workspace level."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/workspace-outbound-access-protection-for-data-factory/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2026-01-29 15:00:00 +00:00
permalink: "/2026-01-29-Workspace-Outbound-Access-Protection-for-Data-Factory-and-OneLake-Shortcuts-Preview.html"
categories: ["Azure", "Security"]
tags: ["Admin APIs", "Azure", "Azure Data Lake Storage", "Compliance", "Cross Workspace Connections", "Data Factory", "Data Gateway", "Data Security", "Dataflow Gen2", "Lakehouse", "Microsoft Fabric", "Mirrored Databases", "News", "OAP", "OneLake", "Outbound Access Protection", "Security", "SQL Endpoint", "VNET", "Warehouse", "Workspace Admin"]
tags_normalized: ["admin apis", "azure", "azure data lake storage", "compliance", "cross workspace connections", "data factory", "data gateway", "data security", "dataflow gen2", "lakehouse", "microsoft fabric", "mirrored databases", "news", "oap", "onelake", "outbound access protection", "security", "sql endpoint", "vnet", "warehouse", "workspace admin"]
---

Microsoft Fabric Blog explains recent improvements to workspace-level Outbound Access Protection (OAP) for Data Factory and OneLake Shortcuts, describing benefits, supported workloads, and configuration steps for enhanced security and compliance.<!--excerpt_end-->

# Workspace Outbound Access Protection for Data Factory and OneLake Shortcuts (Preview)

The Microsoft Fabric team introduces workspace-level Outbound Access Protection (OAP) for Data Factory and OneLake Shortcuts, now available in preview. OAP complements inbound access control by ensuring that data movement and connections from a Fabric workspace are tightly governed and limited to destinations explicitly allowed by workspace administrators.

## Key Features and Benefits

- **Enhanced Outbound Security:** OAP restricts Data Factory items in a workspace to connect only to destinations approve-listed by workspace admins. Public internet and unauthorized endpoints are blocked by default.
- **Granular Workspace Controls:** Outbound access can be set per workspace, rather than just at the tenant level. This enables differentiated security postures for different teams, environments, or projects.
- **Data Exfiltration Defense:** Combined with inbound protection, OAP helps prevent data from being exfiltrated outside workspace boundaries.
- **Compliance Enablement:** Administrators can enforce policies to keep sensitive data within compliant boundaries, supporting regulatory requirements.

## Supported Scenarios

OAP now covers:

- Dataflow Gen 2 (including CI/CD)
- Pipelines
- Copy jobs
- Mirrored items (such as Mirrored SQL Database, Mirrored Snowflake)
- OneLake Shortcuts (with support for external shortcuts)

Admins can specify allow/deny rules for connection types, endpoints, and VNET/On-Prem Data Gateways. Once a gateway is allowed, workspace users can incorporate those connections into their workflows.

A typical setup involves enabling OAP, then configuring granular data connection rules for the workspace. For example, Dataflows and Pipelines can pull data from allowed sources behind a VNET Data Gateway and push to a Lakehouse. Unapproved outbound connections are blocked.

![OAP for Data Factory Flow diagram](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/10/word-image-28889-1-1.png)

For detailed documentation and configuration steps, see the [Workspace outbound access protection overview](https://learn.microsoft.com/fabric/security/workspace-outbound-access-protection-overview).

## OneLake Shortcuts and OAP

OneLake now supports outbound access protection for external shortcuts. New rules allow admins to specify which outbound data sources are accessible. After allow-listing, users can only read or create shortcuts to those approved locations—for example, specific Azure Data Lake Storage Gen2 accounts.

Additionally, data connection rules enable cross-workspace connections through the Lakehouse connector. This approach allows copy operations and shortcuts to connect between OAP-enabled workspaces without managed private endpoints or private link service. For implementation, see [managing outbound access from OneLake with outbound access protection](https://learn.microsoft.com/fabric/onelake/onelake-manage-outbound-access).

## Roadmap

Tenant-level Admin APIs for Workspace OAP will be available soon. Expanded support for Power BI Semantic Models and Reports is planned. The Fabric team welcomes community feedback at the [Fabric Ideas – Microsoft Fabric Community](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/fabric%20platform%20%7C%20security).

## Summary

Workspace Outbound Access Protection extends granular, administrator-driven network security controls to more Data Factory and OneLake workloads within Microsoft Fabric, helping customers secure, comply, and flexibly scale their data platforms.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/workspace-outbound-access-protection-for-data-factory/)
