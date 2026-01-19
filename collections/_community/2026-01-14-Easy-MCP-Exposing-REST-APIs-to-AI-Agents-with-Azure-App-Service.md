---
layout: post
title: 'Easy MCP: Exposing REST APIs to AI Agents with Azure App Service'
author: jordanselig
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/app-service-easy-mcp-add-ai-agent-capabilities-to-your-existing/ba-p/4484513
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2026-01-14 19:41:29 +00:00
permalink: /ai/community/Easy-MCP-Exposing-REST-APIs-to-AI-Agents-with-Azure-App-Service
tags:
- AI Agents
- API Gateway
- API Integration
- App Modernization
- Azure API Management
- Azure App Service
- Azure Developer CLI
- Cloud Migration
- Easy MCP
- Legacy Modernization
- MCP
- No Code Changes
- OpenAPI
- REST API
section_names:
- ai
- azure
---
jordanselig describes how Easy MCP enables seamless integration of existing REST APIs with AI agents like GitHub Copilot on Azure App Service—with zero code changes required.<!--excerpt_end-->

# Easy MCP: Exposing REST APIs to AI Agents with Azure App Service

**Author: jordanselig**

## Overview

Easy MCP is an open source toolkit that bridges the gap between traditional REST APIs and modern AI agents. By providing an OpenAPI-to-MCP gateway, it enables AI tools (such as GitHub Copilot and others) to invoke your APIs automatically—without requiring any code modifications to your existing applications.

## The Challenge

Many organizations rely on mature REST APIs that encapsulate core business workflows. However, AI agents use discovery protocols like [Model Context Protocol (MCP)](https://modelcontextprotocol.io) rather than custom APIs. Historically, making APIs available to AI agents meant:

- Learning the MCP SDK
- Writing additional server code
- Deploying and maintaining new infrastructure

This is a high barrier for teams looking to leverage AI quickly.

## Solution: Easy MCP

**Easy MCP** serves as a translation layer between APIs described with OpenAPI (Swagger) and the MCP format used by AI clients. The approach allows you to:

1. Point the Easy MCP gateway to your API's base URL
2. Autodetect the OpenAPI specification
3. Instantly generate MCP tools for your endpoints
4. Obtain a ready-to-use MCP endpoint for any supported AI agent

If your API is running (e.g., on Azure App Service) and exposes an OpenAPI spec, you can make it MCP-compatible in minutes.

## Example Integration Workflow

Suppose your API is hosted at `https://my-todo-app.azurewebsites.net`:

1. Open the Easy MCP web UI
2. Enter your API URL
3. Click "Detect" to locate your OpenAPI specification
4. Click "Connect" to generate MCP interfaces

Configure your AI client (e.g., VS Code with GitHub Copilot) to use the generated MCP endpoint:

```json
{
  "servers": {
    "my-api": {
      "type": "http",
      "url": "https://my-gateway.azurewebsites.net/mcp"
    }
  }
}
```

Your AI assistant can now perform actions such as retrieving todos, adding items, or marking tasks as complete—all through your existing API infrastructure.

## Modernization Strategy with Azure App Service

This approach fits naturally into the broader Azure app modernization strategy:

- **Migrate** legacy apps to Azure App Service with Managed Instance (no code changes)
- **Expose** APIs to AI agents using Easy MCP Gateway
- **Enable** new AI-powered workflows for business users and developers

This pathway supports modernization without rewriting core logic.

## Deployment Options

- **Get the code**: [seligj95/app-service-easy-mcp on GitHub](https://github.com/seligj95/app-service-easy-mcp)
- **Deploy to Azure with Azure Developer CLI**:

  ```sh
  azd auth login
  azd init
  azd up
  ```

- **Run locally for testing**:

  ```sh
  npm install
  npm run dev
  # Open http://localhost:3000
  ```

If you need a sample app, Microsoft provides a [tutorial for integrating Azure AI agents with .NET](https://learn.microsoft.com/azure/app-service/tutorial-ai-integrate-azure-ai-agent-dotnet).

## Looking Ahead: Platform Integration

Microsoft is considering ways to build Easy MCP-like capabilities natively into Azure App Service. That would allow developers to enable AI agent access from the Azure Portal, with built-in security and scalability—no extra gateways or deployments required. Azure API Management now also offers [REST-to-MCP export functionality](https://learn.microsoft.com/en-us/azure/api-management/export-rest-mcp-server) you can leverage.

## Summary

Easy MCP is a proof of concept making it easier to bring AI agent integration to existing REST APIs and applications—especially those migrated to Azure. The AI modernization journey is accelerated with open tooling and future platform enhancements.

For feedback or feature requests, see the GitHub repository or follow updates on Azure App Service developments.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/app-service-easy-mcp-add-ai-agent-capabilities-to-your-existing/ba-p/4484513)
