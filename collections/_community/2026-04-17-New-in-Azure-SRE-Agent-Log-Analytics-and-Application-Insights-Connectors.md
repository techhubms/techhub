---
date: 2026-04-17 02:14:28 +00:00
author: Dalibor_Kovacevic
section_names:
- azure
- devops
feed_name: Microsoft Tech Community
tags:
- AKS
- Application Insights
- Az Monitor CLI
- Azure
- Azure MCP Server
- Azure Monitor
- Azure Resource Graph
- Azure SRE Agent
- AzureDiagnostics
- Community
- ContainerLog
- DevOps
- Incident Investigation
- KQL
- KubeEvents
- Kusto Query Language
- Log Analytics
- Log Analytics Reader
- Managed Identity
- MCP
- Monitoring Connectors
- Monitoring Reader
- Observability
- RBAC
- SecurityEvent
- SRE
- Syslog
title: 'New in Azure SRE Agent: Log Analytics and Application Insights Connectors'
primary_section: azure
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/new-in-azure-sre-agent-log-analytics-and-application-insights/ba-p/4509649
---

Dalibor_Kovacevic announces new Azure SRE Agent connectors for Log Analytics and Application Insights, enabling faster, MCP-backed KQL querying with simplified RBAC setup via managed identities.<!--excerpt_end-->

## Overview

Azure SRE Agent now supports **Log Analytics** and **Application Insights** as log providers, backed by the **Azure MCP Server**. After you connect workspaces and App Insights resources, the agent can query them directly during incident investigations.

## Why this matters

Log Analytics and Application Insights are common destinations for Azure operational data such as:

- Container logs
- Application traces
- Dependency failures
- Security events

Previously, the agent could access the same data by shelling out to **`az monitor` CLI** commands (assuming you manually configured RBAC for the agent’s managed identity). That still works, but:

- RBAC setup was manual
- Each query required going through the CLI

With the new connectors:

- Setup is simpler (the product handles RBAC grants)
- Querying is faster (native MCP-backed tools instead of CLI calls)

## What you get

Two new connector types are available in **Builder > Connectors** (and also via the onboarding flow under **Logs**):

- **Log Analytics**
  - Connect a workspace.
  - The agent can query tables including **ContainerLog**, **Syslog**, **AzureDiagnostics**, **KubeEvents**, **SecurityEvent**, and custom tables.
- **Application Insights**
  - Connect an Application Insights resource.
  - The agent can query requests, dependencies, exceptions, traces, and custom telemetry.

You can connect multiple workspaces and App Insights resources. During an investigation, the agent selects the appropriate target based on what’s connected.

## Setup

To enable early access:

- Turn on **Early access to features** under **Settings > Basics**.

Then add connectors using either approach:

- **Through onboarding**
  - Click **Logs** in the onboarding flow
  - Select **Log Analytics Workspace** or **Application Insights** under *Additional connectors*
- **Through Builder**
  - Go to **Builder > Connectors**
  - Add a **Log Analytics** or **Application Insights** connector

Choose the resource from the dropdown and save.

If the resource does not show up via discovery, both connector types support a **manual entry fallback**.

### RBAC granted on save

On save, the system grants the agent’s managed identity these roles on the target resource group:

- **Log Analytics Reader**
- **Monitoring Reader**

If your account can’t assign roles, you can grant them separately.

## Backed by Azure MCP Server

This feature uses the **Azure MCP Server** with the **monitor** namespace. When you save your first connector, the product spins up an MCP server instance automatically.

The agent gets access to read-only tools such as:

- **monitor_workspace_log_query**: run KQL against a workspace
- **monitor_resource_log_query**: run KQL against a specific resource
- **monitor_workspace_list**: discover workspaces
- **monitor_table_list**: list tables in a workspace

Everything is **read-only**: the agent can query data but cannot modify monitoring configuration.

If different connectors use different managed identities, the system handles **per-call identity routing** automatically.

## Example investigation flow

If an alert fires on an **AKS** cluster, the agent can query the connected workspace. Example KQL patterns shown:

```kusto
ContainerLog
| where TimeGenerated > ago(30m)
| where LogEntry contains "error" or LogEntry contains "exception"
| summarize count() by ContainerID, LogEntry
| top 10 by count_

KubeEvents
| where TimeGenerated > ago(1h)
| where Reason in ("BackOff", "Failed", "Unhealthy")
| summarize count() by Reason, Name, Namespace
| order by count_ desc
```

The agent also includes built-in skills for common Log Analytics and Application Insights query patterns, helping it choose relevant tables and queries for typical failure scenarios.

## Things to know

- **Read-only**: query-only access; no changes to alerts, retention, or workspace config
- **Resource discovery needs Reader**: dropdown discovery uses **Azure Resource Graph**; use manual entry if needed
- **One identity per connector**: use separate connectors when different managed identities are required

## Learn more

- Azure SRE Agent documentation: https://sre.azure.com/docs
- Azure MCP Server documentation: https://learn.microsoft.com/en-us/azure/developer/azure-mcp-server/

[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/new-in-azure-sre-agent-log-analytics-and-application-insights/ba-p/4509649)

