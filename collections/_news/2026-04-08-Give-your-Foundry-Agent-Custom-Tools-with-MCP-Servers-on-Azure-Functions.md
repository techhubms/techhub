---
feed_name: Microsoft Azure SDK Blog
primary_section: ai
external_url: https://devblogs.microsoft.com/azure-sdk/give-your-foundry-agent-custom-tools-with-mcp-servers-on-azure-functions/
tags:
- .NET
- Access Tokens
- AI
- AI Agents
- Authentication
- Authorization Code Flow
- Azure
- Azure Functions
- Azure Functions MCP Extension
- Azure SDK
- Best Practices
- Foundry
- Function App
- Function Keys
- Java
- Managed Identity
- MCP
- MCP Server
- Microservices
- Microsoft Entra ID
- Microsoft Foundry
- News
- Node.js
- OAuth 2.0
- OAuth Identity Passthrough
- Python
- Remote Tools
- Scopes
- Security
- Serverless
- Token Endpoint
- Tool Calling
date: 2026-04-08 15:00:50 +00:00
author: Lily Ma
section_names:
- ai
- azure
- security
title: Give your Foundry Agent Custom Tools with MCP Servers on Azure Functions
---

Lily Ma explains how to connect an MCP server hosted on Azure Functions to Microsoft Foundry agents, focusing on setup steps and the trade-offs between key-based auth, Microsoft Entra ID (managed identity), OAuth identity passthrough, and unauthenticated access.<!--excerpt_end-->

## Overview

This post shows how to connect a remote **MCP (Model Context Protocol) server** hosted on **Azure Functions** to **Microsoft Foundry agents**, so the agent can call your custom tools (APIs, database queries, business logic).

![Connecting to Foundry agent through OAuth Identity Passthrough demo](https://devblogs.microsoft.com/azure-sdk/wp-content/uploads/sites/58/2026/04/04-06-foundry_oauth_id_passthrough.gif)

## Connect your MCP server on Azure Functions to Foundry Agent

Azure Functions can host remote MCP servers with scalable infrastructure, built-in authentication options, and serverless billing. The main point: once a Foundry agent is connected, it can **discover your tools**, decide when to call them, and use tool results in responses.

## Why connect MCP servers to Foundry agents?

If you already have an MCP server working with clients like **VS Code**, **Visual Studio**, **Cursor**, or other MCP clients, you can reuse those same tools with a Foundry agent without rebuilding the server. You’re adding another consumer (the agent) rather than rewriting tool logic.

## Prerequisites

1. An MCP server deployed to Azure Functions. Sample repos:
   - Python: https://github.com/Azure-Samples/remote-mcp-functions-python
   - TypeScript: https://github.com/Azure-Samples/remote-mcp-functions-typescript
   - .NET: https://github.com/Azure-Samples/remote-mcp-functions-dotnet
   - Java: https://github.com/Azure-Samples/remote-mcp-functions-java
2. A Foundry project with a deployed model and a Foundry agent:
   - Foundry resources quickstart: https://learn.microsoft.com/azure/foundry/tutorials/quickstart-create-foundry-resources?view=foundry&tabs=portal
   - Agent quickstart: https://learn.microsoft.com/azure/foundry/quickstarts/get-started-code?view=foundry&tabs=portal

## Authentication options

Pick an auth option based on your stage (development vs production) and whether you need per-user identity.

| Method | Description | Use case |
| --- | --- | --- |
| Key-based (default) | Agent passes a shared function access key in the request header. | Development, or when Entra authentication isn’t needed. |
| Microsoft Entra | Agent authenticates using its own identity (*agent identity*) or the Foundry project’s shared identity (*project managed identity*). | Prefer agent identity in production; shared identity is better limited to development. |
| OAuth identity passthrough | Agent prompts users to sign in; the user token is used to authenticate. | Production scenarios requiring per-user identity and persisted user context. |
| Unauthenticated access | Agent calls without auth. | Development, or public-only tool access. |

Function keys reference: https://learn.microsoft.com/azure/azure-functions/function-keys-how-to

## Connect your MCP server to your Foundry agent

If you use key-based auth or unauthenticated access, connecting from Foundry is simpler. **Microsoft Entra** and **OAuth identity passthrough** require extra setup.

Detailed walkthroughs for each auth method:

- https://learn.microsoft.com/azure/azure-functions/functions-mcp-foundry-tools?tabs=entra%2Cmcp-extension%2Cfoundry

### High-level setup steps

1. Enable built-in MCP authentication:
   - Key-based auth is default for HTTP endpoints in Functions.
   - Disable that and enable built-in MCP auth instead.
   - Doc: https://learn.microsoft.com/azure/azure-functions/functions-mcp-tutorial?tabs=mcp-extension&pivots=programming-language-python#enable-built-in-server-authorization-and-authentication
2. Get your MCP server endpoint URL:
   - For MCP extension-based servers:
     - `https://<FUNCTION_APP_NAME>.azurewebsites.net/runtime/webhooks/mcp`
3. Gather credentials for your chosen method:
   - Function key, managed identity config, or OAuth credentials.
4. Add the MCP server as a tool in the Foundry portal:
   - In your agent settings, add a new MCP tool and provide endpoint + credentials.

### Microsoft Entra connection required fields

| Field | Description | Example |
| --- | --- | --- |
| Name | Unique identifier for the MCP server | `contoso-mcp-tools` |
| Remote MCP Server endpoint | MCP endpoint URL | `https://contoso-mcp-tools.azurewebsites.net/runtime/webhooks/mcp` |
| Authentication | Auth method | `Microsoft Entra` |
| Type | Identity type the agent uses | `Project Managed Identity` |
| Audience | Application ID URI of the function app’s Entra registration | `api://00001111-aaaa-2222-bbbb-3333cccc4444` |

### OAuth identity passthrough required fields

| Field | Description | Example |
| --- | --- | --- |
| Name | Unique identifier for the MCP server | `contoso-mcp-tools` |
| Remote MCP Server endpoint | MCP endpoint URL | `https://contoso-mcp-tools.azurewebsites.net/runtime/webhooks/mcp` |
| Authentication | Auth method | `OAuth Identity Passthrough` |
| Client ID | Client ID of the function app Entra registration | `00001111-aaaa-2222-bbbb-3333cccc4444` |
| Client secret | Client secret of the function app Entra registration | `abcEFGhijKLMNopqRST` |
| Token URL | Exchange endpoint for access tokens | `https://login.microsoftonline.com/<TENANT_ID>/oauth2/v2.0/token` |
| Auth URL | User sign-in/consent endpoint | `https://login.microsoftonline.com/<TENANT_ID>/oauth2/v2.0/authorize` |
| Refresh URL | Endpoint to obtain a new token | `https://login.microsoftonline.com/<TENANT_ID>/oauth2/v2.0/token` |
| Scopes | Requested permissions/resource access | `api://00001111-aaaa-2222-bbbb-3333cccc4444/user_impersonation` |

### Testing

After configuring the MCP server as a tool, test in the **Agent Builder playground** by sending a prompt that should trigger one of the MCP tools.

## Closing thoughts

The author highlights “composability”: build your MCP server once and reuse it across clients (VS Code, Visual Studio, Cursor, ChatGPT) and now Foundry agents. Azure Functions is positioned as a practical hosting option for these servers with scale and security.

## What’s next

A follow-up post will cover more MCP topics and new MCP features/developments in Azure Functions.


[Read the entire article](https://devblogs.microsoft.com/azure-sdk/give-your-foundry-agent-custom-tools-with-mcp-servers-on-azure-functions/)

