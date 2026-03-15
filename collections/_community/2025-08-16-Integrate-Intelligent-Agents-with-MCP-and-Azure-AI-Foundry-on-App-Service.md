---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/supercharge-your-app-service-apps-with-ai-foundry-agents/ba-p/4444310
title: Integrate Intelligent Agents with MCP and Azure AI Foundry on App Service
author: jordanselig
feed_name: Microsoft Tech Community
date: 2025-08-16 01:15:20 +00:00
tags:
- Agent Service
- AI Agent Integration
- App Deployment
- Automation
- Azure AI Foundry
- Azure App Service
- Azure Developer CLI
- Cloud Hosting
- Developer Guide
- FastAPI
- LLMs
- Managed Identity
- MCP
- MCP Server
- Natural Language Processing
- OpenAPI
- Python
- To Do List App
- Web Applications
- AI
- Azure
- Community
section_names:
- ai
- azure
primary_section: ai
---
jordanselig walks through how developers can connect Azure AI Foundry agents to any application using the Model Context Protocol (MCP) and App Service, with practical examples and deployment guidance.<!--excerpt_end-->

# Integrate Intelligent Agents with MCP and Azure AI Foundry on App Service

## Introduction

Integrating AI into web applications is easier than ever thanks to MCP (Model Context Protocol) support within Azure AI Foundry Agent Service. This open standard enables developers to connect agents with remote tools and services through MCP servers, and with Azure App Service, both agents and MCP servers can be deployed and managed seamlessly—without custom code.

## What is Model Context Protocol (MCP)?

MCP is an open protocol that serves as a standardized bridge between AI agents (such as those powered by large language models) and external applications, APIs, or data sources. With MCP:

- Agents can dynamically discover available tools
- Integration and tool sharing are standardized
- Architecture is scalable for multiple apps and agents
- Setup is simpler versus traditional API integration

Learn more about MCP: [modelcontextprotocol.io](https://modelcontextprotocol.io/).

## Reference Implementation

This solution consists of two sample apps:

### 1. MCP Agent Client Application

- [Repo: Azure-Samples/app-service-mcp-foundry-agent-python](https://github.com/Azure-Samples/app-service-mcp-foundry-agent-python)
- FastAPI app to create/manage Azure AI Foundry agents and connect them to MCP servers
- Features a web chat interface
- Deploys easily to App Service using Azure Developer CLI (`azd`)
- Secure connection via managed identity

### 2. Example MCP Server: To-do List

- [Repo: Azure-Samples/app-service-python-todo-mcp](https://github.com/Azure-Samples/app-service-python-todo-mcp)
- Complete MCP server focused on to-do list management (CRUD, filtering, bulk operations)
- No code changes required to integrate with the AI agent via MCP

## Deployment Guide

### Prerequisites

- Azure subscription
- Azure Developer CLI (azd)
- Python 3.11+

### Steps

1. **Clone and deploy the MCP Agent Client:**

   ```
   git clone https://github.com/Azure-Samples/app-service-mcp-foundry-agent-python.git
   cd app-service-mcp-foundry-agent-python
   azd auth login
   azd up
   ```

   _Note: MCP support is in preview; select a supported region._

2. **Clone and deploy the Example MCP Server (to-do app):**

   ```
   git clone https://github.com/Azure-Samples/app-service-python-todo-mcp.git
   cd app-service-python-todo-mcp
   azd up
   ```

Both apps will be provisioned as independent Azure App Service deployments.

## Using the Solution

- Obtain the MCP endpoint URL from the to-do server app
- Enter it in the agent chat app
- Chat with the agent in natural language to create and manage to-dos
- See your operations reflected in real time

## MCP vs. OpenAPI for Integrating AI Agents

Azure AI Foundry agents also support [OpenAPI-defined tools](https://learn.microsoft.com/azure/ai-foundry/agents/how-to/tools/openapi-spec). If you have legacy apps or lack MCP support, OpenAPI remains an alternative—and you can generate OpenAPI specs with GitHub Copilot if needed.

For more, see: [How it works](https://learn.microsoft.com/azure/ai-foundry/agents/how-to/tools/model-context-protocol#how-it-works) and [App Service integration tutorial](https://learn.microsoft.com/azure/app-service/invoke-openapi-web-app-from-azure-ai-agent-service).

## Expanding the Pattern

MCP means you can layer AI/LLM agent capabilities on nearly any application:

- E-commerce (order management)
- CRM (customer data queries)
- Content management (AI-powered operations)
- Finance (reporting, tracking)

Build an MCP-compatible service that exposes your app's business logic, and connect agents without heavy refactoring.

## Conclusion

The new MCP support in Azure AI Foundry enables rapid, low-effort integration of AI agents into Azure App Service-hosted applications. Follow the reference architecture to:

- Quickly provision agent-connected solutions
- Experiment with both MCP and OpenAPI models
- Scale intelligent workflows across existing apps

For more details and related guidance, visit the [Azure App Service documentation](https://learn.microsoft.com/azure/app-service/overview-ai-integration).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/supercharge-your-app-service-apps-with-ai-foundry-agents/ba-p/4444310)
