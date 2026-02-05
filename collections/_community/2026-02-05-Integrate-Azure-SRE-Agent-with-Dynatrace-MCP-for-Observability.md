---
layout: "post"
title: "Integrate Azure SRE Agent with Dynatrace MCP for Observability"
description: "This guide by dbandaru demonstrates how to connect the Azure SRE Agent to Dynatrace using the official MCP (Microsoft Copilot Platform) server integration. You'll learn how to set up credentials, configure connectors, create subagents, run DQL queries, investigate problems, analyze vulnerabilities, and test full-stack observability solutions on Azure."
author: "dbandaru"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/get-started-with-dynatrace-mcp-server-in-azure-sre-agent/ba-p/4492363"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-05 21:42:26 +00:00
permalink: "/2026-02-05-Integrate-Azure-SRE-Agent-with-Dynatrace-MCP-for-Observability.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Azure", "Azure SRE Agent", "Community", "DevOps", "DQL", "Dynatrace", "Kubernetes Events", "MCP Server", "OAuth", "Observability", "Platform Token", "Problem Investigation", "SaaS Integration", "Security", "Security Analysis", "Server Sent Events", "Timeseries Forecast", "VS Code", "YAML Configuration"]
tags_normalized: ["azure", "azure sre agent", "community", "devops", "dql", "dynatrace", "kubernetes events", "mcp server", "oauth", "observability", "platform token", "problem investigation", "saas integration", "security", "security analysis", "server sent events", "timeseries forecast", "vs code", "yaml configuration"]
---

dbandaru provides a comprehensive step-by-step guide to integrating the Azure SRE Agent with Dynatrace MCP, enabling real-time observability, DQL analytics, and problem/vulnerability investigation directly within Azure environments.<!--excerpt_end-->

# Integrate Azure SRE Agent with Dynatrace MCP for Observability

## Overview

Leverage the integration of Dynatrace's hosted MCP server with Microsoft's Azure SRE Agent to unlock real-time observability in Azure environments. This enables querying logs, problem investigation, vulnerability analysis, and leveraging DQL (Dynatrace Query Language) directly within your SRE workflows.

## Key Capabilities

- **Create DQL queries:** Automatically generate Dynatrace queries from natural language.
- **Explain DQL queries:** Get plain-English explanations of existing queries.
- **Run DQL queries:** Execute analytics on logs and metrics (up to 1000 records).
- **Investigate problems:** Query, detail, and analyze Davis Problems in Dynatrace.
- **Analyze security vulnerabilities:** List and filter vulnerabilities by risk level.
- **Generate timeseries forecasts:** Predict future metrics using statistical models.
- **Kubernetes cluster events:** Streamline platform troubleshooting.

## Prerequisites

- Azure SRE Agent resource in your Azure subscription.
- Dynatrace Platform (SaaS) account.
- Platform Token or OAuth client with appropriate scopes.
- Your Dynatrace environment URL (e.g., `https://abc12345.apps.dynatrace.com`).

## Step-by-Step Integration

### 1. Obtain Dynatrace Credentials

- Create a [Platform Token](https://myaccount.dynatrace.com/platformTokens) with required scopes:
  - `mcp-gateway:servers:invoke`, `mcp-gateway:servers:read` (MCP Gateway)
  - `davis-copilot:nl2dql:execute` (DQL creation)
  - `davis-copilot:dql2nl:execute` (DQL explanation)
  - `storage:buckets:read`, `storage:events:read`, `storage:security.events:read` (data operations)
  - `davis:analyzers:read`, `davis:analyzers:execute` (forecasting)

### 2. Add MCP Connector in Azure SRE Agent

- Go to [sre.azure.com](https://sre.azure.com), select your agent, then **Builder > Connectors** > **Add connector**.
- Choose **MCP server** (user provided connector).
- Configure for Streamable-HTTP:
  - Name: `dynatrace-mcp`
  - Connection type: Streamable-HTTP
  - URL: `https://<your-tenant>.apps.dynatrace.com/platform-reserved/mcp-gateway/v0.1/servers/dynatrace-mcp/mcp`
  - Authentication: Bearer token (your Platform Token)

#### Example mcp.json

```json
{
  "mcpServers": {
    "dynatrace-mcp": {
      "url": "https://<your-tenant>.apps.dynatrace.com/platform-reserved/mcp-gateway/v0.1/servers/dynatrace-mcp/mcp",
      "transport": "streamable-http",
      "headers": { "Authorization": "Bearer YOUR_PLATFORM_TOKEN" }
    }
  }
}
```

### 3. Create a Dynatrace Subagent

- Use **Builder > Subagent builder** > **+ Create** and select **YAML** tab.
- Example configuration:

```yaml
api_version: azuresre.ai/v1
kind: AgentConfiguration
spec:
  name: DynatraceExpert
  system_prompt: |
    You are a Dynatrace observability expert with access to real-time monitoring data via the Dynatrace MCP server.
    ...
  agent_type: Autonomous
  enable_skills: true
```

- Assign relevant tools (MCP Tool, Dynatrace MCP Server Tools).

### 4. Test the Integration

- Start a chat with your SRE Agent.
- Example prompts:
  - “Create a DQL query to find all error logs from the last hour.”
  - “Explain this DQL query: fetch logs | filter loglevel == 'ERROR' | limit 10.”
  - “List active problems in my Dynatrace environment.”
  - “Get vulnerabilities by risk level.”

## Troubleshooting

| Error                | Typical Cause                  | Solution                               |
|----------------------|-------------------------------|----------------------------------------|
| 401 Unauthorized     | Invalid/expired token          | Create a new Platform Token             |
| 403 Forbidden        | Missing token scopes           | Add required scopes                     |
| Could not connect    | Wrong tenant URL               | Verify tenant name in URL               |
| Timeout              | Network/firewall issues        | Ensure access to *.apps.dynatrace.com   |

- Validate your token with curl:

```sh
curl -X GET https://<tenant>.apps.dynatrace.com/platform/management/v1/environment \
  -H "Authorization: Bearer YOUR_PLATFORM_TOKEN" \
  -H "accept: application/json"
```

## Alternate: Open Source (OSS) MCP Server

- Community-supported, provides features like workflow and document management.
- Requires Node.js; configured via Stdio connection in Azure SRE Agent.

## Related Links

- [Dynatrace MCP Documentation](https://docs.dynatrace.com/docs/discover-dynatrace/platform/davis-ai/dynatrace-mcp)
- [Dynatrace Platform Token Reference](https://docs.dynatrace.com/docs/manage/identity-access-management/access-tokens-and-oauth-clients/platform-tokens)
- [DQL Reference](https://docs.dynatrace.com/docs/discover-dynatrace/references/dynatrace-query-language)

---
*Author: dbandaru*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/get-started-with-dynatrace-mcp-server-in-azure-sre-agent/ba-p/4492363)
