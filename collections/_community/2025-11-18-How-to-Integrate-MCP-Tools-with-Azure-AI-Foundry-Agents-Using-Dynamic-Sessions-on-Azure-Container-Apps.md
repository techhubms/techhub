---
layout: post
title: How to Integrate MCP Tools with Azure AI Foundry Agents Using Dynamic Sessions on Azure Container Apps
author: jeffmartinez
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-to-add-an-mcp-tool-to-your-azure-foundry-agent-using-dynamic/ba-p/4468844
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-18 06:14:52 +00:00
permalink: /ai/community/How-to-Integrate-MCP-Tools-with-Azure-AI-Foundry-Agents-Using-Dynamic-Sessions-on-Azure-Container-Apps
tags:
- AI Agents
- API Authentication
- ARM Template
- Azure Container Apps
- Azure Foundry
- Azure Portal
- Cloud Automation
- Dynamic Sessions
- EnvironmentId
- MCP
- Preview Features
- Python Containers
- Remote Execution
- Resource Group
- Session Pool Deployment
- Shell Session Pools
section_names:
- ai
- azure
---
jeffmartinez explains step-by-step how to leverage Azure Container Apps dynamic sessions with MCP capabilities, integrating them as remote tools in Azure AI Foundry agents for secure, interactive shell command execution.<!--excerpt_end-->

# How to Integrate MCP Tools with Azure AI Foundry Agents Using Dynamic Sessions on Azure Container Apps

Author: jeffmartinez

## Introduction

This tutorial guides you through enabling Model Context Protocol (MCP) server support for dynamic session pools in Azure Container Apps (preview feature) and wiring them up as tools for Azure AI Foundry agents. These steps empower AI agents to execute code, run shell commands, and perform advanced tasks remotely—all inside ephemeral, secure containers.

## Prerequisites

- Azure subscription
- Azure CLI installed and authenticated
- Access to Azure Foundry portal

---

## Step 1: Deploy a Shell Dynamic Session Pool Resource with MCP Enabled

Use an ARM template to define and deploy your session pool resource.

**Prepare your environment:**

```sh
az login
SUBSCRIPTION_ID=$(az account show --query id --output tsv)
RESOURCE_GROUP=<RESOURCE_GROUP_NAME>
SESSION_POOL_NAME=<SESSION_POOL_NAME>
LOCATION=<LOCATION>
az group create --name $RESOURCE_GROUP --location $LOCATION
```

**ARM template content (deploy.json):**

```json
{
  "$schema": "http://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#",
  "contentVersion": "1.0.0.0",
  "parameters": {
    "name": { "type": "String" },
    "location": { "type": "String" }
  },
  "resources": [
    {
      "type": "Microsoft.App/sessionPools",
      "apiVersion": "2025-02-02-preview",
      "name": "[parameters('name')]",
      "location": "[parameters('location')]",
      "properties": {
        "poolManagementType": "Dynamic",
        "containerType": "Shell",
        "scaleConfiguration": { "maxConcurrentSessions": 5 },
        "sessionNetworkConfiguration": { "status": "EgressEnabled" },
        "dynamicPoolConfiguration": {
          "lifecycleConfiguration": {
            "lifecycleType": "Timed",
            "coolDownPeriodInSeconds": 300
          }
        },
        "mcpServerSettings": { "isMCPServerEnabled": true }
      }
    }
  ]
}
```

**Deploy the template:**

```sh
az deployment group create --resource-group $RESOURCE_GROUP --template-file deploy.json --name $SESSION_POOL_NAME --location $LOCATION
```

---

## Step 2: Retrieve MCP Server Endpoint and Credentials

**Get Endpoint:**

```sh
az rest --method GET --uri "/subscriptions/$SUBSCRIPTION_ID/resourceGroups/$RESOURCE_GROUP/providers/Microsoft.App/sessionPools/$SESSION_POOL_NAME?api-version=2025-02-02-preview" --query "properties.mcpServerSettings.mcpServerEndpoint" -o tsv
```

**Fetch API Key:**

```sh
az rest --method POST --uri "/subscriptions/$SUBSCRIPTION_ID/resourceGroups/$RESOURCE_GROUP/providers/Microsoft.App/sessionPools/$SESSION_POOL_NAME/fetchMCPServerCredentials?api-version=2025-02-02-preview" --query "apiKey" -o tsv
```

Store these values for upcoming agent integration.

---

## Step 3: Set Up Azure Foundry Project and Agent

1. In the Azure Foundry portal, activate the New Foundry interface.
2. Create a project by navigating to the left nav bar and selecting **Create new project**. Fill in Name, Foundry resource, Subscription, Region, and Resource group, then submit.
3. Build an agent: Access the **Build** tab, go to **Agents**, hit **Create agent**, enter a name, and submit.

---

## Step 4: Connect the MCP Server as a Tool for Your Agent

1. In the agent's Tools section, choose **Connect a tool**.
2. In the **Custom** tab, pick **Model Context Protocol (MCP)** and click **Create**.
3. Complete the form:
   - Name: your-unique-name
   - Remote MCP server endpoint: your-mcp-server-endpoint
   - Authentication: Key-based
   - Credential: Key: "x-ms-apikey", Value: "your-api-key"
4. On creation, use the **Use in an agent** option to link this tool to your newly created agent.

---

## Step 5: Run Remote Shell Commands from Your Agent

You can now use the Foundry agent's playground to prompt your agent to launch tasks inside secure remote shell environments.

**Example prompt:**
> Create a hello world flask app in a new remote environment. Then send a request to the app to show that it works.

Workflow:

- The MCP tool spins up a shell session (tracked by an environmentId GUID).
- The agent requests step-by-step approval for code execution (such as installing Python and Flask).
- On completion, a curl request to the app returns "Hello, World!"

---

## Summary

By following these steps, your AI agents in Azure Foundry can securely execute remote shell commands using MCP-enabled dynamic sessions on Azure Container Apps. This approach opens new capabilities for interactive, action-driven agent solutions in the Azure ecosystem.

---

For more details, refer to [Azure Container Apps: Dynamic Sessions Documentation](https://learn.microsoft.com/azure/container-apps/sessions).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-to-add-an-mcp-tool-to-your-azure-foundry-agent-using-dynamic/ba-p/4468844)
