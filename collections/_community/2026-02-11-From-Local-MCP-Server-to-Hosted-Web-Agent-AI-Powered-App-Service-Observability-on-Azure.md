---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/from-local-mcp-server-to-hosted-web-agent-app-service/ba-p/4493241
title: 'From Local MCP Server to Hosted Web Agent: AI-Powered App Service Observability on Azure'
author: jordanselig
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-02-11 15:47:24 +00:00
tags:
- AI
- AI Agent
- App Service Plan
- Authentication
- Azure
- Azure App Service
- Azure OpenAI
- Chat UI
- Community
- Deployment
- DevOps
- Express.js
- KQL
- Log Analytics
- Managed Identity
- MCP
- Microsoft Entra ID
- Node.js
- Observability
- VNet Integration
section_names:
- ai
- azure
- devops
---
jordanselig demonstrates how the App Service Observability MCP Server has expanded from a local IDE tool to a scalable, AI-powered web solution on Azure App Service, leveraging Azure OpenAI and robust DevOps integration.<!--excerpt_end-->

# From Local MCP Server to Hosted Web Agent: App Service Observability, Part 2

*Authored by jordanselig*

## Introduction

This article continues the journey from [Part 1](https://techcommunity.microsoft.com/blog/appsonazureblog/chat-with-your-app-service-logs-using-github-copilot/4491573), introducing how the App Service Observability MCP Server can be delivered as a multi-user web application with AI-driven investigation, hosted on Azure App Service. The new approach opens observability to entire teams with minimal configuration, browser-ready access, and deeper integration with Azure cloud resources.

## Quick Recap: Local MCP Server

- Provides 15+ tools (Log Analytics, Kudu logs, HTTP error analysis, deployment correlation, logging configs)
- Runs locally via Node.js, using stdio and your Azure credentials (`az login`)
- Targets individual developer workflows within VS Code or similar IDEs

## The Challenge

- Local/server approach is limited to single-user setups
- Collaboration for on-call engineers, PMs, and support needs a shared solution
- Consistency and access for team investigations were needed

## Solution: Hosted Web Agent on Azure

- The MCP server is deployed to **Azure App Service**; now accessible to any authorized browser user
- Features a React-based chat UI and a built-in Azure OpenAI agent (e.g., GPT-5-mini)
- Adds managed identity authentication, VNet/private endpoints, and zero API keys needed

### Hosted vs. Local Agent

| Feature                  | Local MCP Server                           | Hosted Web Agent                                |
|-------------------------|--------------------------------------------|------------------------------------------------|
| Deployment              | Local/IDE, manual steps                    | `azd up` with Azure Developer CLI               |
| Interface               | IDE plugin, CLI, any MCP client            | Browser-based chat UI                          |
| AI Model                | Existing assistant (Copilot/Claude)        | Azure OpenAI (private endpoint)                 |
| Authentication          | Local dev credentials (`az login`)         | Managed Identity + Entra ID (Easy Auth)        |
| Azure Resources Needed  | None beyond `az login`                     | App Service, Azure OpenAI, VNet integration     |
| Best For                | Individuals troubleshooting locally        | Teams with shared, instant access               |

## Architecture Overview

```
Web Browser
   └─ React Chat UI (resource selectors, tool steps, markdown responses)
       └─ HTTP (REST API)
           └─ Azure App Service (Node.js 20)
               └─ Express Server (API endpoints: /api/chat, /api/set-context, etc.; /mcp MCP protocol)
                   └─ VNet Integration
                       ├─ Azure OpenAI (GPT-5-mini, Private EP)
                       ├─ Log Analytics / KQL Queries
                       └─ ARM API / Kudu (app metadata, logs)
```

The server returns both the chat interface and direct MCP endpoints for alternative clients, letting the same tooling power both web and IDE/CLI experiences.

## Demo: AI-Powered Investigations

- Users can investigate applications by chatting with the built-in AI agent, selecting resources, and reviewing the tools called during the session
- AI agent correlates errors to deployments, reviews logs, and provides step-by-step instructions with markdown formatting

## Infrastructure-as-Code Deployment

- Everything defined with Bicep; one-command deployment using `azd up`
- Provisions:
  - **App Service Plan** with App Service (Node.js 20 LTS, VNet integrated)
  - **Azure OpenAI resource** (GPT-5-mini) + private endpoint/DNS
  - **VNet** with subnets for app and private endpoints
  - **Managed Identity** (Entra ID, RBAC for resource and analytics APIs)
- Strong focus on security: no API keys, fully internal traffic for AI queries

## Security

- Recommend enabling authentication via Microsoft Entra ID (formerly Azure AD) using Easy Auth
- Fine-grained RBAC roles control access to Azure resources, logs, and OpenAI
- Managed identity used exclusively for backend calls

## Developer Experience (Chat UI)

- Resource pickers auto-populate based on your Azure subscription
- Live tool-call tracing with explanations and runtime details
- Session management, markdown output, and easy resets

## Extensibility: Beyond Web Apps

- MCP tools can enable Teams bots, Slack integrations, CLI agents, automated monitors, Azure Portal extensions, mobile debugging, and beyond
- Single codebase and tooling standard (MCP protocol) promote reusability and interoperability across platforms

## Try It Yourself

**Open Source:**

- Local MCP: [github.com/seligj95/app-service-observability-agent](https://github.com/seligj95/app-service-observability-agent)
- Hosted Agent: [github.com/seligj95/app-service-observability-agent-hosted](https://github.com/seligj95/app-service-observability-agent-hosted)

**Deploy:**

```bash
git clone https://github.com/seligj95/app-service-observability-agent-hosted.git
cd app-service-observability-agent-hosted
azd up
```

## What's Next?

- Developing more tools (resource health, autoscale, certificate checks, network diagnostics)
- Multi-app correlation and proactive monitoring/alerting
- Potential for deeper integration with App Service platform and Azure Portal
- Community feedback and PRs encouraged!

---

*Updated Feb 09, 2026
Version 1.0*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/from-local-mcp-server-to-hosted-web-agent-app-service/ba-p/4493241)
