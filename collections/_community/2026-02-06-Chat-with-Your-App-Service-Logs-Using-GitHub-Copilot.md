---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/chat-with-your-app-service-logs-using-github-copilot/ba-p/4491573
title: Chat with Your App Service Logs Using GitHub Copilot
author: jordanselig
primary_section: github-copilot
feed_name: Microsoft Tech Community
date: 2026-02-06 16:51:33 +00:00
tags:
- Agent Skill
- AI
- App Debugging
- App Monitoring
- Application Logs
- Azure
- Azure App Service
- Azure CLI
- Community
- Container Logs
- DefaultAzureCredential
- Deployment Diagnostics
- DevOps
- Error Analysis
- GitHub Copilot
- KQL
- Kudu
- Log Analytics
- MCP
- Node.js
- Observability
- RBAC
- VS Code
section_names:
- ai
- azure
- devops
- github-copilot
---
Jordan Selig demonstrates how developers can leverage an MCP server to connect GitHub Copilot with Azure App Service logs, enabling AI-driven diagnostics and troubleshooting directly in the IDE or CLI.<!--excerpt_end-->

# Chat with Your App Service Logs Using GitHub Copilot

**Author:** Jordan Selig

## Overview

This blog introduces an open-source Model Context Protocol (MCP) server that bridges Azure App Service observability logs directly to AI assistants like GitHub Copilot. The result: developers can analyze logs, debug issues, and receive targeted recommendations through natural language interactions without leaving their coding environment.

## The Problem

Azure App Service emits a variety of logs—Kudu container logs, Log Analytics tables, deployment logs, and more. Troubleshooting often requires:

- Navigating multiple Azure Portal blades
- Searching and executing KQL queries
- Correlating data across different sources
- Repeated manual effort

The process can be slow and error-prone under pressure.

## The Solution: App Service Observability MCP Server

The presented proof-of-concept enables AI tools such as GitHub Copilot (in VS Code and CLI) and Claude Code to:

- Query logs from Log Analytics (HTTP, Console, Platform logs)
- Fetch container logs from Kudu automatically
- Analyze HTTP errors by status code and endpoint
- Identify slow requests that exceed latency thresholds
- Diagnose deployment problems by correlating logs with app failures
- Check logging setup recommendations for various runtimes (Python, Node.js, .NET, Java)
- View deployment history and link it to errors
- Investigate and explain container restarts

**Natural Language Debugging:**
Developers can ask questions such as “Why did my app stop?” and receive actionable insights, with Copilot able to access both logs and codebase context for effective debugging.

## Demo Walkthrough

The post shows a real-world scenario where the tool helps:

- Identify and triage early, transient issues
- Detect a configuration bug causing an app crash
- Correlate the error with recent deployments
- Suggest code and configuration fixes, using Copilot’s synthesis of log and code insights

## How It Works

The MCP server translates AI assistant requests into Azure API calls. Some features require Log Analytics diagnostic settings, but key operations like `get_recent_logs` and `get_deployments` work out-of-the-box using Kudu and ARM APIs.

### Security Model

- Uses your existing Azure credentials (`DefaultAzureCredential` via `az login`)
- No stored secrets or elevated service principals
- Access is precisely what you’re already authorized for (RBAC enforced)
- All actions are auditable via Azure activity logs
- For production, follow least-privilege principles (dedicated service principal or managed identity)

## The Agent Skill: Building AI Domain Expertise

The solution includes an Agent Skill markdown file that gives Copilot structured domain knowledge:

- Debugging workflows for common scenarios
- Error pattern recognition (OOM issues, Always On, HTTP 5xx, etc.)
- KQL query templates
- SKU/resource reference
- Best practices for investigations and recommendations

With the MCP tooling and Agent Skill knowledge base, AI assistants can:

- Automate diagnosis (e.g. identify a stopped container due to Always On settings)
- Suggest relevant Azure CLI commands for remediation
- Minimize guesswork by drawing from Azure data and domain expertise

## Installation and Getting Started

**Prerequisites:**

- Node.js 20+
- Azure CLI (`az login`)
- VS Code with GitHub Copilot

**Setup Steps:**

1. Clone the repo: `git clone https://github.com/seligj95/app-service-observability-agent.git`
2. Build: `cd app-service-observability-agent && npm install && npm run build`
3. Configure `.vscode/mcp.json` in your workspace with Azure environment variables
4. Reload VS Code, start the server, and begin querying App Service with Copilot

## Future Directions

The project is a proof-of-concept but envisions that every Azure App Service might one day expose a built-in MCP endpoint for AI-powered observability, usable by your whole team.

## Resources

- [GitHub Repo](https://github.com/seligj95/app-service-observability-agent)
- [SKILL.md Knowledge Base](https://github.com/seligj95/app-service-observability-agent/blob/main/SKILL.md)

## Feedback

You’re invited to give feedback or contribute ideas for additional tools and debugging patterns.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/chat-with-your-app-service-logs-using-github-copilot/ba-p/4491573)
