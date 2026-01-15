---
layout: post
title: Enabling API Key Authentication for Azure Logic Apps MCP Servers
author: KentWeareMSFT
canonical_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/enabling-api-key-authentication-for-logic-apps-mcp-servers/ba-p/4470560
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-18 18:35:39 +00:00
permalink: /ai/community/Enabling-API-Key-Authentication-for-Azure-Logic-Apps-MCP-Servers
tags:
- Access Token
- Agent Loop
- AI
- API Key Authentication
- API Management
- Az CLI
- Azure
- Azure AI Foundry
- Azure API Center
- Azure Logic Apps
- Community
- Conversational Agents
- Host.json
- Key Expiry
- Logic Apps MCP Server
- Microsoft Azure
- OAuth2
- Primary/Secondary Key
- Regenerate API Key
- REST API
- Security
- Security Configuration
- X API Key
section_names:
- ai
- azure
- security
---
KentWeareMSFT shares a detailed tutorial on configuring and managing API Key authentication for Azure Logic Apps MCP Servers, including setup, key management, and agent usage.<!--excerpt_end-->

# Enabling API Key Authentication for Azure Logic Apps MCP Servers

KentWeareMSFT provides a thorough walkthrough on how to configure authentication settings for Logic Apps Model Context Protocol (MCP) Servers, focusing on the recently added support for API Key authentication alongside OAuth2.

## Overview

Logic Apps MCP Servers now support three authentication types:

- OAuth2 (default)
- API Key (default; new addition)
- Anonymous (opt-in)

API Key authentication is included by default in both the API Center wizard and manual configuration. This makes it easier to integrate with non-Microsoft agent frameworks and secure MCP endpoints.

## Configuring Authentication in Logic Apps

To modify authentication options, update your `host.json` configuration file. The relevant node is:

```json
{
  "version": "2.0",
  "extensionBundle": {
    "id": "Microsoft.Azure.Functions.ExtensionBundle.Workflows",
    "version": "[1.*, 2.0.0)"
  },
  "extensions": {
    "workflow": {
      "McpServerEndpoints": {
        "enable": true,
        "authentication": { "type": "ApiKey" }
      }
    }
  }
}
```

- Leaving out the `type` property means both OAuth2 and API Key are enabled by default.
- Explicitly setting the `type` property will enforce only that scheme (e.g., `"ApiKey"`).

Official documentation: [Set up Logic App as MCP Server](https://learn.microsoft.com/en-us/azure/logic-apps/set-up-model-context-protocol-server-standard#set-up-logic-app-as-mcp-server)

## Prerequisites: Authenticating Requests

You'll need a Bearer token to call the management APIs:

1. Open [Azure Portal](https://portal.azure.com/) and start a Cloud Shell session.
2. Run:

   ```bash
   az account get-access-token --resource https://management.azure.com/
   ```

3. Copy the returned `accessToken`.

## Retrieving MCP Server API Keys

Use a REST client or CLI to call the following endpoint, setting the query parameter `getApikey=true`:

- **REST Endpoint:**

  ```http
  POST https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{RGName}/providers/Microsoft.Web/sites/{LAName}/hostruntime/runtime/webhooks/workflow/api/management/listMcpServers?api-version=2021-02-01&getApikey=true
  ```

  (Replace placeholders with your subscription, resource group, and Logic App names)

- **CLI:**

  ```bash
  az rest --method post --url "https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{RGName}/providers/Microsoft.Web/sites/{LAName}/hostruntime/runtime/webhooks/workflow/api/management/listMcpServers?api-version=2021-02-01&getApikey=true"
  ```

- Optionally supply a request body:

  ```json
  {
    "keyType": "primary",
    "notAfter": "2026-09-04T10:04:24Z"
  }
  ```

  - `keyType`: "primary" or "secondary" (defaults to primary)
  - `notAfter`: expiry for the key (default is 24 hours)

Keys returned apply to the entire Logic App, not individual servers.

## Viewing Preconfigured Tools

The `listMcpServers` endpoint also displays preconfigured tools available for each MCP Server—helpful for discovering integration options.

## Regenerating API Keys

To create a new key:

- **Endpoint:**

  ```http
  POST https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{RGName}/providers/Microsoft.Web/sites/{LAName}/hostruntime/runtime/webhooks/workflow/api/management/regenerateMcpServerAccessKey?api-version=2021-02-01
  ```

- **CLI:**

  ```bash
  az rest --method post --url "https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{RGName}/providers/Microsoft.Web/sites/{LAName}/hostruntime/runtime/webhooks/workflow/api/management/regenerateMcpServerAccessKey?api-version=2021-02-01"
  ```

- Required request body:

  ```json
  {
    "keyType": "primary"
  }
  ```

After regeneration, call `listMcpServers` to obtain the new key value.

## Using API Key Authentication

To connect MCP clients (e.g., Conversational Agents):

1. Build or configure your agent.
2. Select "Add MCP Server (preview)" and choose "Key" for Authentication Type.
3. Provide your MCP Server URL and the API Key.
4. Use "X-API-KEY" as the Key Header Name.
5. Save and test your connection.

Agent Loop support for MCP Servers is also available.

## Demo

See the linked video for a step-by-step demonstration of API Key authentication setup.

## Key Takeaways

- API Key authentication is now a default option for Logic Apps MCP Servers in Azure.
- Setup requires updating host.json and using Azure CLI or REST endpoints for key management.
- Keys are managed at the Logic App level and include custom expiry and regeneration options.
- Works seamlessly with agent integrations for secure and flexible connectivity.

For more details, refer to the official documentation and product updates from KentWeareMSFT.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/enabling-api-key-authentication-for-logic-apps-mcp-servers/ba-p/4470560)
