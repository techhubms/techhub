---
external_url: https://devblogs.microsoft.com/devops/remote-mcp-server-preview-in-microsoft-foundry/
author: Dan Hellem
primary_section: ai
section_names:
- ai
- azure
- devops
date: 2026-03-19 17:48:10 +00:00
title: Remote MCP Server preview in Microsoft Foundry
tags:
- '#devops'
- Agent Orchestration
- Agent Tools
- AI
- AI Agents
- Automated Workflows
- Azure
- Azure & Cloud
- Azure AI Foundry
- Azure DevOps
- Azure DevOps MCP Server
- Azure Integration
- Community
- Deployment
- Developer Workflows
- DevOps
- Evaluation
- MCP
- MCP Server
- Microsoft Foundry
- News
- Public Preview
- Remote MCP Server
- Tool Catalog
feed_name: Microsoft DevOps Blog
---

Dan Hellem announces that the Azure DevOps MCP Server (public preview) can now be used from Microsoft Foundry, explaining what Foundry is and showing the basic steps to add the tool to an agent, connect to an Azure DevOps organization, optionally limit available tools, and test via chat prompts.<!--excerpt_end-->

# Remote MCP Server preview in Microsoft Foundry

Earlier this week, Microsoft released the public preview of the Azure DevOps MCP Server:

- Public preview announcement: https://devblogs.microsoft.com/devops/azure-devops-remote-mcp-server-public-preview

Now, the Azure DevOps MCP Server is available to use in Microsoft Foundry.

## What Microsoft Foundry is

Microsoft Foundry is described as a unified platform for building and managing AI-powered applications and agents at scale. It brings together:

- Model access
- Orchestration
- Evaluation
- Deployment

…in a single environment.

It’s intended to help teams:

- Develop intelligent solutions such as copilots and automated workflows
- Connect AI to real-world tools and services
- Move projects from experimentation to secure, production-ready systems on Azure

More info: https://ai.azure.com

## Add the Azure DevOps MCP Server to an agent

Adding the Azure DevOps MCP Server to your agent is presented as a straightforward setup.

1. In Foundry, go to **Add Tools** > **Catalog**.
2. Search for **“Azure DevOps.”**
3. Select **Azure DevOps MCP Server (preview)** from the results.
4. Click **Create**.

![Foundry catalog showing Azure DevOps MCP Server (preview)](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2026/03/foundry-catalog.webp)

## Connect to your Azure DevOps organization

1. Enter your **organization name**.
2. Click **Connect**.

![Foundry connect screen for Azure DevOps MCP Server](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2026/03/foundry-connect.webp)

## Limit which tools the agent can access (optional)

After connecting, you can start using the Azure DevOps MCP Server tools. The post also notes you may want to restrict access by choosing only a subset of tools, so you can control exactly what the agent can use.

![Foundry configuration screen for selecting a subset of tools](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2026/03/foundry-config-tools.webp)

## Test the agent

To test:

- Create a new chat
- Write a prompt
- Run it

![Foundry chat test screen](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2026/03/foundry-test.webp)

## Summary

This preview integration aims to make it easier to connect AI agents to Azure DevOps and build workflows that help developers and product managers be more productive.

[Read the entire article](https://devblogs.microsoft.com/devops/remote-mcp-server-preview-in-microsoft-foundry/)

