---
layout: "post"
title: "Integrating Datadog MCP Server with Azure SRE Agent for Enhanced Observability"
description: "This guide walks through connecting Azure SRE Agent to Datadog’s observability platform using the official Datadog MCP server. It covers capabilities such as log and metric analysis, APM trace correlation, incident management, security, and troubleshooting, all through a native Azure integration."
author: "dbandaru"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/get-started-with-datadog-mcp-server-in-azure-sre-agent/ba-p/4497123"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-25 16:56:04 +00:00
permalink: "/2026-02-25-Integrating-Datadog-MCP-Server-with-Azure-SRE-Agent-for-Enhanced-Observability.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["APM", "Authentication", "Azure", "Azure SRE Agent", "Cloud Monitoring", "Community", "Connector Setup", "Custom YAML Agent", "Datadog MCP Server", "Datadog Skills", "DevOps", "Incident Management", "Integration", "Log Analysis", "Metric Analysis", "Observability", "RBAC", "Security", "Security Best Practices", "Service Accounts", "Troubleshooting"]
tags_normalized: ["apm", "authentication", "azure", "azure sre agent", "cloud monitoring", "community", "connector setup", "custom yaml agent", "datadog mcp server", "datadog skills", "devops", "incident management", "integration", "log analysis", "metric analysis", "observability", "rbac", "security", "security best practices", "service accounts", "troubleshooting"]
---

dbandaru provides a practical guide for integrating Datadog’s official MCP server with Azure SRE Agent, enabling full-stack observability—including logs, metrics, traces, incidents, and security—all within the Azure platform.<!--excerpt_end-->

# Integrating Datadog MCP Server with Azure SRE Agent for Enhanced Observability

Connecting Azure SRE Agent to Datadog's observability platform empowers teams with comprehensive real-time monitoring, troubleshooting, and automation, directly in Azure.

## Overview

The Datadog MCP server acts as a bridge between your Datadog environment and Azure SRE Agent, exposing powerful tools for:

- Logs: Search, analyze, aggregate (including SQL-based queries)
- Metrics: Real-time queries, metadata exploration, and context discovery
- APM: Full trace retrieval, span analytics, latency analysis, and service mapping
- Monitors & Alerts: Status tracking, configuration validation, group inspection
- Incidents: End-to-end timeline investigation, responder identification
- Dashboards, Hosts, Services, Events, Notebooks, and Real User Monitoring (RUM)

All actions adhere to your Datadog RBAC permissions, emphasizing secure integration.

## Prerequisites

- Azure SRE Agent deployed in Azure
- Datadog organization with an active plan
- Datadog API and Application keys (with MCP Read/Write permissions)
- Your Datadog org allowlisted for MCP Server (currently Preview)

## Step-by-Step Setup

### 1. Create API and Application Keys

- Generate an API key in Datadog under Organization Settings > API Keys
- Create an Application key in Organization Settings > Application Keys and assign `MCP Read` (and optionally `MCP Write`) scopes
- Use a service account for production integration and restrict permissions as needed

### 2. Add the MCP Connector in Azure

- In Azure Portal, navigate to your SRE Agent > Builder > Connectors
- Choose 'Datadog MCP server', and input your keys and the correct regional endpoint
- Authentication keys and connector type fields are pre-populated for streamlined setup
- When connected, MCP tools become available to the agent

### 3. (Optional) Create a Specialized Subagent

- Navigate to Builder > Subagents and add a new agent using provided YAML
- Tailor the system prompt to Datadog observability, granting the agent targeted analytic expertise

### 4. (Optional) Add a Datadog Skill

- Create a skill for best practices in log, metric, and incident workflows
- Reference the new skill in your subagent config for enhanced responses

## Testing & Usage Examples

- Start a new session and use prompts like:
    - "Search for error logs from the payment-api service in the last hour"
    - "Show the current CPU usage across all production hosts"
    - "Find slowest traces for the checkout-service"
    - "Show all monitors currently in Alert status"
- The agent will use Datadog MCP core toolsets for queries and correlation

## Managing Toolsets

- The default 'core' toolset covers logs, metrics, traces, dashboards, etc.
- Enable advanced features (alerting, APM, security, CI/CD, etc.) by appending `?toolsets=core,apm,alerting` to the connector URL
- Only add required toolsets to control agent surface area

## Troubleshooting

- 401/403 errors: Validate key validity and RBAC permissions
- Tool not found: May require non-default toolset; update connector accordingly
- Data missing: Verify permissions or expand your search scope
- Region mismatch: Confirm endpoint URL matches your Datadog org region

## Security and Operations

- All traffic secured by HTTPS/TLS
- Access is scoped by RBAC and key granularity—smallest possible blast radius is recommended
- Audit Trail in Datadog tracks all tool usage

## Limitations

- MCP server is currently Preview and only available to allowlisted organizations
- Some tools/toolsets depend on your Datadog plan and enabled features
- Large trace responses may be truncated for context efficiency

## Resources

- [Datadog MCP Server documentation](https://docs.datadoghq.com/mcp/)
- [Datadog API and Application keys](https://docs.datadoghq.com/account_management/api-app-keys/)
- [Azure SRE Agent documentation](https://azure.microsoft.com/en-us/resources/devops/sre-agent/)

---
*Author: dbandaru — Microsoft community contributor*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/get-started-with-datadog-mcp-server-in-azure-sre-agent/ba-p/4497123)
