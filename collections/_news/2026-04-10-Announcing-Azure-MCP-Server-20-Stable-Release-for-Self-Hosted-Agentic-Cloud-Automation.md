---
feed_name: Microsoft Azure SDK Blog
author: Sandeep Sen
date: 2026-04-10 17:16:10 +00:00
external_url: https://devblogs.microsoft.com/azure-sdk/announcing-azure-mcp-server-2-0-stable-release/
primary_section: ai
tags:
- Agentic Workflows
- Agents
- AI
- AI Agents
- Authentication
- Automation Pipelines
- Azure
- Azure China (21vianet)
- Azure MCP Server
- Azure Resources
- Azure SDK
- Azure US Government
- CI/CD
- Container Images
- DevOps
- Devtools
- Docker
- GitHub Copilot CLI
- IDE Integrations
- LLM
- Managed Identity
- MCP
- ML
- News
- On Behalf Of Flow
- OpenID Connect
- Remote MCP Server
- Security
- Security Hardening
- Self Hosted
- Sovereign Cloud
- VS
- VS Code
section_names:
- ai
- azure
- devops
- security
title: Announcing Azure MCP Server 2.0 Stable Release for Self-Hosted Agentic Cloud Automation
---

Sandeep Sen announces Azure MCP Server 2.0’s stable release, focusing on self-hosted remote MCP servers, authentication options (managed identity and OBO), security hardening, and operational improvements to support agentic workflows that automate and manage Azure resources.<!--excerpt_end-->

# Announcing Azure MCP Server 2.0 Stable Release for Self-Hosted Agentic Cloud Automation

Azure MCP Server 2.0 is now generally available. It’s open-source software that implements the **Model Context Protocol (MCP)** and lets **AI agents and developer tools** interact with **Azure resources** via a consistent, standardized tool interface.

Azure MCP includes **276 MCP tools across 57 Azure services**, aimed at end-to-end scenarios like provisioning, deployment, monitoring, and operational diagnostics inside AI-assisted workflows.

## What is Azure MCP Server?

Azure MCP Server is an **MCP-compliant server** that exposes Azure capabilities as **structured, discoverable tools** that agents can select and invoke.

It’s designed to fit into developer workflows and can be used:

- Locally on a single machine
- Via IDE/tool integrations
- As a centrally managed deployment
- As a **self-hosted remote MCP server** for team/enterprise scenarios (the primary focus of the 2.0 release)

This enables scaling from ad-hoc local use to centrally managed deployments with consistent policy, security controls, and configuration.

## Key updates in Azure MCP Server 2.0

### Self-hosted remote MCP server

Azure MCP Server 2.0 adds/strengthens **remote hosting** support:

- Strengthened **HTTP-based transport** to support authentication scenarios and safer remote operations
- Enables deploying Azure MCP as a **centrally managed internal service** with consistent configuration and governance

#### Authentication approaches

Remote Azure MCP supports multiple authentication options, including:

- **Managed identity** (example: when running alongside Microsoft Foundry)
  - Reference: https://aka.ms/azmcp/self-host/foundry
- **On-Behalf-Of (OBO) flow** (OpenID Connect delegation)
  - Securely calls Azure APIs using the **signed-in user context**
  - Reference: https://aka.ms/azmcp/self-host/obo

#### Common scenarios

- Shared Azure MCP access for developers and internal agent systems
- Operating within enterprise network and policy boundaries
- Central management of configuration (tenant context, subscription defaults, telemetry policies)
- Integrating MCP-powered workflows into **CI/CD and automation pipelines**

### Security hardening and operational safeguards

2.0 emphasizes improved security and operational safety for both local and remote setups, including:

- Endpoint validation
- Protections against common **injection patterns** for query-oriented tools
- Tighter isolation controls for development environments

The intent is to make Azure MCP safer for local use and more appropriate as a shared remote service.

### Client compatibility and distribution options

Azure MCP Server 2.0 supports a range of environments and agent platforms:

- IDE-based workflows
- CLI-based interaction
- Standalone server operation

It also expands distribution options to improve portability and onboarding across MCP-compatible tools.

### Performance and reliability improvements

- Reliability and responsiveness improvements, especially when multiple MCP toolsets are involved
- Container distribution updates to:
  - Reduce image size
  - Improve efficiency for containerized deployment

### Sovereign cloud readiness

Azure MCP Server can be configured for **sovereign cloud** environments, including:

- Azure US Government
- Azure operated by 21Vianet (Azure in China)

Reference: https://aka.ms/azmcp/sovereign-cloud

This supports regulated deployments that require sovereign endpoints and stronger boundary controls.

## Under the hood

Azure MCP continues evolving the tool ecosystem with changes aimed at:

- Clearer tool descriptions (better agent selection accuracy)
- More consistent validation logic
- Consolidating redundant operations when it improves discoverability

The goal is a practical, consistent “code-to-cloud” operational interface that avoids per-service integration patterns.

## Get started

- GitHub repo: https://aka.ms/azmcp
- Docker image: https://aka.ms/azmcp/download/docker
- Create an issue: https://aka.ms/azmcp/issues

## Choose your experience

### IDE integrations

Use Azure MCP as an IDE extension:

- Visual Studio Code: https://aka.ms/azmcp/download/vscode
- Visual Studio: https://aka.ms/azmcp/download/vs
- IntelliJ: https://aka.ms/azmcp/download/intellij
- Eclipse: https://aka.ms/azmcp/download/eclipse
- Cursor: https://aka.ms/azmcp/download/cursor

### Agent tools / CLI scenarios

- GitHub Copilot CLI: https://aka.ms/azmcp/download/github-copilot-cli
- Claude Code (mentioned as an example for command-line agentic scenarios)

### Standalone and self-hosted

- Run as a standalone local server for a simple setup
- Self-host as a remote MCP server for shared access, centralized configuration, and enterprise controls:
  - https://aka.ms/azmcp/self-host


[Read the entire article](https://devblogs.microsoft.com/azure-sdk/announcing-azure-mcp-server-2-0-stable-release/)

