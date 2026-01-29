---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-early-preview-byo-remote-mcp-server-on-azure/ba-p/4445317
title: 'Early Preview: Host Your Own Remote MCP Server on Azure Functions'
author: lily-ma
feed_name: Microsoft Tech Community
date: 2025-08-19 02:35:21 +00:00
tags:
- .NET
- API Management
- Authentication
- Azure Developer CLI
- Azure Functions
- BYO MCP Server
- CI/CD
- Flex Consumption Plan
- Func Start
- MCP
- MCP SDK
- Node.js
- Python
- Remote Server Hosting
- Serverless
- Serverless Deployment
- Stateless Servers
- Streamable HTTP
- VS Code
- Azure
- Coding
- Community
section_names:
- azure
- coding
primary_section: coding
---
lily-ma presents the early preview of hosting bring-your-own Remote MCP servers on Azure Functions, making it easier for developers to deploy existing MCP SDK-based servers to a highly scalable serverless environment.<!--excerpt_end-->

# Early Preview: Host Your Own Remote MCP Server on Azure Functions

**Author:** lily-ma

## Overview

Microsoft has announced an early preview for deploying your own (BYO) Model Context Protocol (MCP) servers on Azure Functions, designed to make it seamless for developers to host existing MCP SDK-based servers in Python, Node.js, or .NET on a hyperscale, serverless platform.

## Key Features

- **Minimal Code Changes:** Existing MCP server codebases (Python, Node.js, .NET) can be hosted with as little as a single line change.
- **Serverless Scale:** Azure Functions on the Flex Consumption plan provides automatic, event-driven scaling—including scaling to zero for cost efficiency.
- **Security:** Endpoints include built-in function key protection and can integrate with Azure API Management for enhanced authorization.

## BYO Remote MCP Server vs Functions MCP Extension

There are two main approaches for remote MCP server hosting on Azure Functions:

- **Functions MCP Extension:** Build stateful MCP servers with triggers and bindings, fully integrated into the Functions model. SSE is supported now; streamable HTTP is coming soon.
- **Bring Your Own (BYO):** Deploy existing MCP SDK-based servers directly on Functions, keeping original code structure and ergonomics.

Both options deliver:

- Secure authentication
- Elastic serverless scaling (burst traffic handling)
- Pay-for-what-you-use cost model

## Current Supported Capabilities

- **SDKs:** Python, Node.js, and .NET MCP SDKs
- **Local Debugging:** Use `func start` within Visual Studio or VS Code
- **Deployment:** Rapid deployment with Azure Developer CLI (`azd up`)
- **Server Types:** Stateless servers (using streamable HTTP transport); stateful support/documentation is forthcoming
- **Hosting:** Azure Functions Flex Consumption plan

## Quick Start Samples

- **Python Sample:** [mcp-sdk-functions-hosting-python](https://github.com/Azure-Samples/mcp-sdk-functions-hosting-python)
- **Node.js Sample:** [mcp-sdk-functions-hosting-node](https://github.com/Azure-Samples/mcp-sdk-functions-hosting-node)
- **.NET Sample:** [mcp-sdk-functions-hosting-dotnet](https://github.com/Azure-Samples/mcp-sdk-functions-hosting-dotnet)

Each repository contains:

- Sample "weather" MCP server implementation
- Step-by-step local debugging (Azure Functions Core Tools)
- `azd up` deployment to Azure Functions
- Usage examples with MCP clients (VS Code, Claude, or others)

## Get Involved

Feedback is invited—share your needs regarding identity flows, diagnostics, supported languages, and more to help shape the feature's evolution.

## References and Documentation

- [Azure Functions Flex Consumption plan](https://learn.microsoft.com/en-us/azure/azure-functions/flex-consumption-plan)
- [Build AI agent tools using Remote MCP with Azure Functions](https://techcommunity.microsoft.com/blog/appsonazureblog/build-ai-agent-tools-using-remote-mcp-with-azure-functions/4401059)

---

*Published by lily-ma on the Apps on Azure Blog. Version 1.0. August 19, 2025.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-early-preview-byo-remote-mcp-server-on-azure/ba-p/4445317)
