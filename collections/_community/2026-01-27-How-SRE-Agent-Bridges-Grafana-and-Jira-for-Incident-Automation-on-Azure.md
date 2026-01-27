---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-sre-agent-pulls-logs-from-grafana-and-creates-jira-tickets/ba-p/4489527
title: How SRE Agent Bridges Grafana and Jira for Incident Automation on Azure
author: dchelupati
feed_name: Microsoft Tech Community
date: 2026-01-27 00:30:14 +00:00
tags:
- App Diagnostics
- Azure Container Apps
- Azure Managed Grafana
- Azure Monitor
- Cloud Native
- DevOps Workflows
- Grafana
- Incident Automation
- Incident Management
- Jira
- LogQL
- Loki
- MCP
- Observability
- PagerDuty
- Root Cause Analysis
- Serverless
- ServiceNow
- SRE Agent
section_names:
- azure
- devops
primary_section: azure
---
In this practical guide, dchelupati demonstrates how SRE Agent can automate incident diagnosis and ticket creation by connecting Azure Managed Grafana and Jira—even without native integrations—using MCP and Azure Container Apps.<!--excerpt_end-->

# How SRE Agent Bridges Grafana and Jira for Incident Automation on Azure

**Author: dchelupati**

Azure SRE Agent provides built-in integrations with tools like PagerDuty, ServiceNow, and Azure Monitor. However, modern teams often use a diverse stack, including Jira for incident tracking and Grafana (with Loki) for dashboards and log analytics—neither of which SRE Agent supports natively.

This post guides you through extending SRE Agent with the Model Context Protocol (MCP):

- **Connect tools like Grafana and Jira via self-hosted MCP servers**
- **Automate workflow orchestration for incident investigation and response**

## The Real-World Scenario

A sample grocery store app runs on Azure Container Apps, using Loki for log storage and Azure Managed Grafana for visualization. When an upstream API enforces rate limits, user-facing errors appear. The challenge: automatically detect, diagnose, and track the incident—even though neither Grafana nor Jira are native SRE Agent integrations.

## Step-by-Step: Extending SRE Agent with MCP

### 1. Deploy the SRE Agent

- Grant the Agent Reader access to your Azure subscription.

### 2. Deploy MCP Servers as Azure Container Apps

- **Grafana MCP Server:** Bridges to Azure Managed Grafana for querying logs.
- **Atlassian MCP Server:** Connects to Jira Cloud for ticket creation.
- Both servers are self-hosted as containers exposing `/mcp` endpoints accessible by SRE Agent.

  ```
  Endpoints format:
  - https://ca-mcp-grafana.<env>.azurecontainerapps.io/mcp
  - https://ca-mcp-jira.<env>.azurecontainerapps.io/mcp
  ```

- Add these as MCP endpoint configurations in SRE Agent.

### 3. Create a Dedicated Incident Diagnosis Sub-Agent

- Enable both Grafana MCP and Atlassian MCP tools.
- Provide focused instructions referencing your app's observability setup (e.g., LogQL queries and Loki label conventions).
- Attach a knowledge file (`loki-queries.md`) to give the agent context on log schema and usage.

### 4. Try It in Action

- Use the SRE Agent chat to describe the issue:
  
    > "My container app ca-api-3syj3i2fat5dm in resource group rg-groceryapp is experiencing rate limit errors from a supplier API when checking product inventory."

- The agent will:
  1. Query Loki via Grafana MCP (`{app="grocery-api"} |= "error"`)
  2. Detect patterns like HTTP 429 (rate limit) errors
  3. Extract the root cause (e.g., SUPPLIER_RATE_LIMIT_429 from FreshFoods Wholesale API)
  4. Automatically create a Jira ticket with findings and remediation steps

### 5. Enhance Results With Knowledge Files

- Adding a context file for log label schemas, field names, and LogQL patterns allows SRE Agent to perform more precise and rapid diagnostics.
- Example knowledge file: [loki-queries.md](https://github.com/dm-chelupati/grocery-sre-demo/blob/main/knowledge/loki-queries.md)

## MCP Integration Details

- **stdio Mode:** Ideal for local commands (`npx @modelcontextprotocol/server-github`)
- **Remote Hosted:** Containerized HTTP endpoint for distributed/cloud-native deployments
- If a prebuilt endpoint isn't available, deploy and expose your own via Azure Container Apps.

## Why This Approach Matters

- Most enterprise environments have fragmented tooling across Azure, other clouds, and SaaS providers.
- SRE Agent, with MCP, enables seamless orchestration—connecting Azure services to third-party tools (like Grafana, Jira) for end-to-end, automated incident management.

## Getting Started Checklist

1. Create Azure SRE Agent
2. Deploy MCP servers ([Grafana](https://github.com/grafana/mcp-grafana), [Atlassian/Jira](https://github.com/sooperset/mcp-atlassian))
3. Attach tools and knowledge files
4. Request diagnostic workflows and review results

## Additional Resources

- [Azure SRE Agent documentation](https://aka.ms/sreagent/docs)
- [Sample project repo](https://github.com/dm-chelupati/grocery-sre-demo)
- [Model Context Protocol specification](https://modelcontextprotocol.io)

*Azure SRE Agent is currently in preview.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-sre-agent-pulls-logs-from-grafana-and-creates-jira-tickets/ba-p/4489527)
