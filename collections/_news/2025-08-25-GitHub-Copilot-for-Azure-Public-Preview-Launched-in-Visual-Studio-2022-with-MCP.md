---
external_url: https://devblogs.microsoft.com/visualstudio/github-copilot-for-azure-preview-launches-in-visual-studio-2022-with-azure-mcp-support/
title: GitHub Copilot for Azure Public Preview Launched in Visual Studio 2022 with MCP
author: Yun Jung Choi
feed_name: Microsoft VisualStudio Blog
date: 2025-08-25 18:36:28 +00:00
tags:
- Agent Mode
- Agents
- Azure CLI
- Azure Container Registry
- Azure Cosmos DB
- Azure Dev CLI
- Azure Functions
- Azure Key Vault
- Azure MCP Server
- Azure SQL Database
- Cloud
- Cloud Development
- Copilot
- Copilot Chat
- Developer Productivity
- LLM
- MCP
- Microsoft
- Visual Studio Extensions
- VS
- AI
- Azure
- Coding
- GitHub Copilot
- News
section_names:
- ai
- azure
- coding
- github-copilot
primary_section: github-copilot
---
Yun Jung Choi brings news of the public preview launch of GitHub Copilot for Azure in Visual Studio 2022, highlighting how developers can now access Azure tools through Copilot Agent Mode with integrated Model Context Protocol support.<!--excerpt_end-->

# GitHub Copilot for Azure (Preview) Launches in Visual Studio 2022 with Azure MCP Support

**Author:** Yun Jung Choi

## Overview

GitHub Copilot for Azure is now available in Public Preview as an extension for Visual Studio 2022 (version 17.14+). This extension integrates a curated set of Azure developer tools, exposed through the Azure Model Context Protocol (MCP) server, directly into Copilot's Agent Mode interface inside Visual Studio. Its seamless installation means the MCP server is automatically managed, enabling convenient access to Azure resources and tools within Copilot Chat.

## Key Features

- **Zero-setup Azure MCP server**: The extension automates installation and updates for the Azure MCP server, requiring no manual setup.
- **Agent Mode integration:** Use Copilot Agent Mode to select appropriate Azure tools for common tasks. You can choose from a toolbox for operations like querying resources, diagnosing issues, fetching logs, deploying with azd, and running Azure CLI commands.
- **Comprehensive Azure coverage:** The extension brings support for a wide range of Azure services, such as:
  - App Configuration
  - Azure Best Practices
  - Azure CLI Extension
  - Azure Container Registry (ACR)
  - Azure Cosmos DB
  - Azure Data Explorer
  - Azure Database for PostgreSQL
  - Developer CLI (azd)
  - Azure Functions
  - Azure Key Vault
  - Azure Kubernetes Service (AKS)
  - Azure SQL Database and Elastic Pools
  - Azure Storage
  - And more (see [full list](https://github.com/Azure/azure-mcp/blob/main/docs/azmcp-commands.md))

## Getting Started

### Prerequisites

- Visual Studio 2022 (v17.14+)
- Active GitHub Copilot subscription with Copilot Chat enabled
- Microsoft account with Azure subscription access

### Installation & Setup

1. Install the [GitHub Copilot for Azure (Preview)](https://marketplace.visualstudio.com/items?itemName=github-copilot-azure.GitHubCopilotForAzure2022) extension.
2. Open Copilot Chat and enable Agent Mode.
3. Select the Azure Extension from the tools pane in Copilot Chat.
4. For best results, include required resource details in your prompts (like subscription or resource group names).

**New to Azure?** [Get started with $200 in free credit](https://azure.microsoft.com/en-us/pricing/purchase-options/azure-account).

### Example Prompts

- "Do I have any webapps in my current subscription?"
- "Look for a WebApp named `<appname>`. Does it have any recent downtime?"
- "Find what tenants I have access to and what I’m currently using."
- "Provide the weburls for these ACA apps."

## Roadmap and Feedback

Microsoft aims to enhance the Azure toolset and deepen Visual Studio integration on top of the MCP foundation. User feedback is encouraged to shape future Copilot for Azure capabilities.

For more on MCP and Agent Mode updates, see:

- [MCP General Availability announcement](https://devblogs.microsoft.com/visualstudio/mcp-is-now-generally-available-in-visual-studio/)
- [Agent Mode with MCP support](https://devblogs.microsoft.com/visualstudio/agent-mode-is-now-generally-available-with-mcp-support/)
- [Customizing Copilot in Visual Studio](https://learn.microsoft.com/en-us/shows/github-copilot-for-visual-studio/customizing-github-copilot-in-visual-studio-with-custom-instructions-miniseries)

## Conclusion

This public preview extension meaningfully streamlines Azure-related development workflows for Visual Studio users, leveraging Copilot Agent Mode and MCP for improved productivity and tool discovery.

This post appeared first on "Microsoft VisualStudio Blog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/github-copilot-for-azure-preview-launches-in-visual-studio-2022-with-azure-mcp-support/)
