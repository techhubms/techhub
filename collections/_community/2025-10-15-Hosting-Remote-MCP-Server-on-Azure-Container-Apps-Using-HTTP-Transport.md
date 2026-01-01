---
layout: "post"
title: "Hosting Remote MCP Server on Azure Container Apps Using HTTP Transport"
description: "This article by DeepGanguly demonstrates how to deploy and run a Model Context Protocol (MCP) server using the HTTP transport mechanism on Azure Container Apps (ACA). It details building a live forex converter server, integrating external APIs for real-time data, and configuring connectivity for AI assistants like VS Code Chat. The guide covers local development, Azure deployment steps with Bicep, and setup for cloud-based scaling, monitoring, and updates."
author: "DeepGanguly"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/hosting-remote-mcp-server-on-azure-container-apps-aca-using/ba-p/4459263"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-15 04:22:11 +00:00
permalink: "/2025-10-15-Hosting-Remote-MCP-Server-on-Azure-Container-Apps-Using-HTTP-Transport.html"
categories: ["AI", "Azure", "Coding"]
tags: ["AI", "Azure", "Azure Container Apps", "Azure Deployment", "Bicep", "Cloud Scaling", "Coding", "Community", "Containerization", "Currency Conversion", "HTTP Transport", "JSON RPC", "Live Forex Rates", "MCP", "Node.js", "REST API", "Serverless", "VS Code Chat"]
tags_normalized: ["ai", "azure", "azure container apps", "azure deployment", "bicep", "cloud scaling", "coding", "community", "containerization", "currency conversion", "http transport", "json rpc", "live forex rates", "mcp", "nodedotjs", "rest api", "serverless", "vs code chat"]
---

DeepGanguly presents a step-by-step guide to hosting a Model Context Protocol (MCP) server with HTTP transport on Azure Container Apps, enabling integration with external APIs and use in tools like VS Code Chat.<!--excerpt_end-->

# Hosting Remote MCP Server on Azure Container Apps Using HTTP Transport

## About

Continuing from a previous exploration of using Server-Sent Events (SSE) transports in Azure Container Apps (ACA), this article focuses on hosting Remote Model Context Protocol (MCP) Servers in ACA implementing the HTTP transport mechanism.

## Overview

The Model Context Protocol (MCP) changes how AI assistants interface with external data and tools. While local stdio deployments are common, this guide shows how to create and deploy a production-grade MCP server utilizing HTTP transport in Azure Container Apps. As an example, a live forex currency converter server is built that retrieves real-time rates from external APIs, demonstrating how AI assistants can benefit from up-to-date dynamic data via MCP integration.

## What is MCP HTTP Transport?

MCP supports several transport mechanisms:

- **Stdio Transport**: For direct process communication
- **HTTP Transport**: For RESTful API communication, ideal for cloud scenarios

HTTP transport allows efficient cloud deployment, cross-platform compatibility, handling multiple clients, load balancing, third-party API integrations, and real-time external data fetching.

## Building and Testing the MCP Server

1. **Clone the Repository**

   ```bash
   git clone https://github.com/deepganguly/azure-container-apps-mcp-sample.git
   cd azure-container-apps-mcp-sample
   npm install
   ```

2. **Run the MCP Server Locally**

   ```bash
   npm start
   ```

3. **Test the HTTP Endpoint**

   ```bash
   curl -X POST http://localhost:3001/mcp \
    -H "Content-Type: application/json" \
    -d '{"jsonrpc":"2.0","id":1,"method":"tools/list","params":{}}'
   ```

### Server Features

- **Exchange Rate Management**: Caches rates from exchangerate-api.com for 10 minutes, updates as needed, and falls back to hardcoded values on API failures.
- **HTTP Endpoints**: Listens for JSON-RPC 2.0 POST requests at `/mcp`.
  - `tools/list` lists available tools like `convert_currency`.
  - `tools/call` handles currency conversion requests.
- **Currency Conversion Logic**: Uses USD as a base, supports cross-currency calculation via USD, and manages errors gracefully.
- **Responses**: Returns formatted, informative responses including the result, live rate, and error handling.

## Deploying to Azure Container Apps

Use the included Bicep template for one-step deployment or deploy manually with CLI commands:

```bash
# Login to Azure

az login

# Create a resource group

az group create --name mcp-live-rates-rg --location eastus

# Create a Container App environment

az containerapp env create --name mcp-forex-env --resource-group mcp-live-rates-rg --location eastus

# Deploy the container app

az containerapp up --name mcp-live-forex-server --resource-group mcp-live-rates-rg --environment mcp-forex-env --source . --target-port 3001 --ingress external
```

## Connecting MCP Server to VS Code Chat

1. **Get the MCP Server URL:**
   - Example: `https://mcp-live-forex-server.<id>.eastus.azurecontainerapps.io/mcp`
2. **Configure VS Code:**
   - Edit or create `.vscode/mcp.json`:

     ```json
     {
       "servers": {
         "my-mcp-server": {
           "url": "https://mcp-live-forex-server.<id>.eastus.azurecontainerapps.io/mcp",
           "type": "http"
         }
       },
       "inputs": [
         {
           "id": "convert_currency",
           "type": "promptString",
           "description": "Convert {amount} {from} to {to} using live exchange rates"
         }
       ]
     }
     ```

   - Add your server, reload VS Code, and interact with the MCP server via chat.

## Conclusion

By building and deploying a MCP HTTP server on Azure Container Apps, developers gain a robust, scalable, and flexible environment for cloud-based integrations with AI assistants. ACA offers operational benefits like easy monitoring, scaling, and updating through Azure tools and CLI.

---
**Repository:** [azure-container-apps-mcp-sample](https://github.com/deepganguly/azure-container-apps-mcp-sample)

For further details and source code, visit the linked repository.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/hosting-remote-mcp-server-on-azure-container-apps-aca-using/ba-p/4459263)
