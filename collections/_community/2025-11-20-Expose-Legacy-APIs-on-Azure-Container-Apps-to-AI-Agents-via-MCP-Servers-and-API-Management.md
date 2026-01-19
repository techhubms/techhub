---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/exposing-legacy-apis-hosted-on-azure-container-apps-to-ai-agents/ba-p/4470476
title: Expose Legacy APIs on Azure Container Apps to AI Agents via MCP Servers and API Management
author: DeepGanguly
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-11-20 09:37:41 +00:00
tags:
- Agent Tooling
- API Gateway
- API Key
- API Proxy
- Authentication
- Authorization
- Azure API Management
- Azure Container Apps
- Centralized Security
- E Commerce
- Governance
- Legacy API
- MCP
- MCP Server
- Observability
- OpenAPI
- REST API
- Throttling
- VS Code
section_names:
- ai
- azure
- devops
---
DeepGanguly demonstrates a step-by-step process for exposing legacy APIs hosted on Azure Container Apps to AI agents using MCP Servers and Azure API Management, enabling secure, tool-based API access for modern applications.<!--excerpt_end-->

# Exposing Legacy APIs Hosted on Azure Container Apps to AI Agents via MCP Servers

## Overview

Legacy REST APIs form the backbone of many business systems—order management, pricing, inventory, and more. With increased demand for AI-driven automation and integration, it’s critical to expose these services as 'tools' for AI agents while maintaining security, governance, and minimizing backend changes. This guide shows how to achieve this by deploying legacy APIs on Azure Container Apps and exposing them to AI Agents (e.g., GitHub Copilot Agent Mode, Claude, internal copilots) as Model Context Protocol (MCP) servers via Azure API Management.

## Use Case

Suppose your organization runs important business processes on a legacy API hosted in Azure Container Apps. You want AI agents to discover and safely invoke these APIs as standardized tools. The solution leverages Azure API Management and the MCP protocol to provide:

- **Centralized governance** (security, throttling, observability)
- **No backend rewrites required**
- Standards-based API exposure via MCP

A sample e-commerce Store API (products, users, carts, authentication) demonstrates the process.

## Step 1: Deploy Store App to Azure Container Apps

The Store API (REST-based, open source) is deployed on Azure Container Apps with endpoints for realistic e-commerce operations. Use the following Azure CLI commands:

```bash
az group create --name "store-app-rg" --location "East US"
az containerapp env create --name "store-app-env" --resource-group "store-app-rg" --location "East US"
az containerapp create --name "store-app" --resource-group "store-app-rg" --environment "store-app-env" --image "voronalk/fakestoreapi:latest" --target-port 3000 --ingress external --min-replicas 1 --max-replicas 5 --cpu 0.5 --memory 1Gi --env-vars PORT=3000
az containerapp show --name "store-app" --resource-group "store-app-rg" --query "properties.configuration.ingress.fqdn" --output table
```

## Step 2: Create an Azure API Management Instance

API Management acts as the gateway for both governance and exposing MCP server endpoints:

```bash
az group create --name "store-app-rg" --location "East US"
az apim create --name "store-app-apim" --resource-group "store-app-rg" --location "East US" --publisher-email "admin@example.com" --publisher-name "Store App Publisher" --sku-name Developer
```

## Step 3: Import the Storefront Open API Specification

Connect Azure API Management to the legacy API endpoints, creating API proxies for safe and efficient exposure:

```bash
# Get the Container App URL

CONTAINER_APP_URL=$(az containerapp show --name "store-app" --resource-group "store-app-rg" --query "properties.configuration.ingress.fqdn" --output tsv)

# Import the API

az apim api import --resource-group "store-app-rg" --service-name "store-app-apim" --api-id "store-api" --path "/storeapp" --specification-format "OpenApi" --specification-url "https://fakestoreapi.com/docs-data" --display-name "Store API" --protocols https --service-url "https://$CONTAINER_APP_URL"
```

## Step 4: Expose Endpoints as an MCP Server

In API Management, create an MCP server and select which endpoints to expose to AI agents. Configure security (API key required), choose exposed operations, and finalize. Follow [Microsoft documentation](https://learn.microsoft.com/en-us/azure/api-management/export-rest-mcp-server#expose-api-as-an-mcp-server) for detail.

## Step 5: Configure and Invoke via Visual Studio Code

Configure the MCP server endpoint for client apps or for AI agents. Update VS Code settings and configuration files:

- In *settings.json*, set up authentication details for accessing the MCP endpoints.
- In *mcp.json*, specify the MCP server URL and available API operations.
- AI agents, tools, or chat features (such as VS Code Chat) can now invoke MCP endpoints securely with the provided API Key.

## Example VS Code Prompts

- *Show me all the products available in the Store*
- *List the top 10 products from Store*
- *Give me all men’s clothing and sort them by price*
- *Find all carts created between 2020-10-01 and 2020-12-31*

## Benefits and Key Takeaways

- **Centralized API governance**: Authentication, authorization, and throttling handled by Azure API Management.
- **No backend rewrites**: Existing legacy APIs remain untouched.
- **Standardized exposure for AI agents**: MCP protocol simplifies discovery and invocation.
- **Secure integrations**: API key-based access and monitoring ensure safety.

## References

- [Azure Container Apps Documentation](https://learn.microsoft.com/en-us/azure/container-apps/code-to-cloud-options)
- [Azure API Management Documentation](https://azure.microsoft.com/en-us/products/api-management/)
- [Model Context Protocol Documentation](https://modelcontextprotocol.io/docs/getting-started/intro)
- [Expose API as an MCP Server Steps](https://learn.microsoft.com/en-us/azure/api-management/export-rest-mcp-server#expose-api-as-an-mcp-server)

---
**Author:** DeepGanguly

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/exposing-legacy-apis-hosted-on-azure-container-apps-to-ai-agents/ba-p/4470476)
