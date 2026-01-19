---
layout: post
title: 'Accelerating AI Agent Development: Foundry MCP Server Preview Announced'
author: SeokJin Han
canonical_url: https://devblogs.microsoft.com/foundry/announcing-foundry-mcp-server-preview-speeding-up-ai-dev-with-microsoft-foundry/
viewing_mode: external
feed_name: Microsoft AI Foundry Blog
feed_url: https://devblogs.microsoft.com/foundry/feed/
date: 2025-12-03 19:15:11 +00:00
permalink: /ai/news/Accelerating-AI-Agent-Development-Foundry-MCP-Server-Preview-Announced
tags:
- Agent Management
- Agents
- AI Agents
- Azure Entra ID
- Cloud Integration
- Foundry MCP Server
- MCP
- Microsoft Foundry
- Model Benchmarks
- Model Deployment
- Monitoring
- MSIgnite
- OAuth 2.0
- RBAC
- Security Best Practices
- VS
- VS Code
section_names:
- ai
- azure
- coding
- security
---
SeokJin Han details the launch of Foundry MCP Server in preview, highlighting how it simplifies AI agent development and integration, with robust security via Entra ID.<!--excerpt_end-->

# Accelerating AI Agent Development: Foundry MCP Server Preview Announced

**Author: SeokJin Han**

## Overview

The Foundry MCP Server preview introduces a cloud-hosted implementation of the Model Context Protocol (MCP) as part of Microsoft Foundry. MCP provides a standard protocol for AI agents to securely connect with apps, data, and systems, emphasizing interoperability and platform expansion. Foundry Tools and Agent Service streamline the process for developers to build, manage, and integrate AI agents.

## Key Features of Foundry MCP Server

- **Centralized Tool Hub:** Discover, connect, and manage MCP tools securely for seamless integration across more than 1,400 business systems.
- **Cloud-Hosted Service:** Removes local hosting requirements, improving reliability and simplifying setup.
- **Visual Studio Code & Visual Studio Integration:** Use with Copilot and agent workflows for chat-based scenarios and agent creation.
- **Agent Management:** Create, update, clone, list, inspect, and delete agents; designed for chatbots, copilots, and evaluators.
- **Model Evaluation:** Register datasets, execute evaluations, compare results, and retrieve insights, supporting both quality and safety assessments.
- **Model Catalog Access:** List, explore, and compare models based on benchmarks, licensing, and capabilities.
- **Deployment Management:** Deploy models as endpoints, monitor usage, latency, quotas, and manage deprecations.

## Example Scenarios

### Building a New Agent from Scratch

1. **Model Selection:** Use model catalog, recommendations, and benchmarks.
2. **Deployment:** Deploy chosen models as endpoints.
3. **Agent Creation:** Define agent properties including instructions, temperature, safety config.
4. **Evaluation:** Create datasets, run evaluations, and compare agents.

### Optimizing Existing Deployment

- Access monitoring metrics, quota checks, recommend alternatives, benchmark candidates, swap deployments, and deprecate old endpoints.

## Integration with Developer Tools

- **VS Code Workflow:** Add MCP Server in agent mode, integrate Copilot, authenticate via Azure Entra ID, and chat using custom models.
- **Visual Studio Workflow:** Installation via Insiders version, tool addition, and secure chat with agents.
- **Foundry Platform:** Add tools through the Foundry UI, establish agent and MCP connections, and interact with agents in a conversational manner.

## Security Model

- **Authentication:** Entra ID with OAuth 2.0 (on-behalf-of tokens)
- **Access Control:** All actions respect Azure RBAC permissions; activity logging and audit trails are enforced.
- **Conditional Access:** Tenant admins govern token retrieval and access through Azure Policy.

## Documentation & Resources

- [Getting Started with Foundry MCP Server](https://learn.microsoft.com/azure/ai-foundry/mcp/get-started?view=foundry&tabs=user)
- [Sample Prompts and Tool Use](https://learn.microsoft.com/en-us/azure/ai-foundry/mcp/available-tools?view=foundry)
- [Security Guidance](https://learn.microsoft.com/azure/ai-foundry/mcp/security-best-practices?view=foundry)

## Conclusion

Foundry MCP Server simplifies and secures AI agent development, offering robust integration paths for enterprise environments and developer toolchains. Explore, create, and monitor agents with best-in-class security and cloud scalability.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/announcing-foundry-mcp-server-preview-speeding-up-ai-dev-with-microsoft-foundry/)
