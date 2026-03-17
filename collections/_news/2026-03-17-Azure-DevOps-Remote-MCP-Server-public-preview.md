---
title: Azure DevOps Remote MCP Server (public preview)
external_url: https://devblogs.microsoft.com/devops/azure-devops-remote-mcp-server-public-preview/
tags:
- '#devops'
- AI
- Authentication
- Azure & Cloud
- Azure DevOps
- Azure DevOps Organizations
- Community
- DevOps
- GitHub Copilot
- GitHub Copilot Chat
- GitHub Copilot CLI
- MCP
- MCP Server
- Mcp.json
- Microsoft Entra ID
- News
- OAuth
- Public Preview
- Remote MCP Server
- Security
- Streamable HTTP Transport
- VS
- VS Code
author: Dan Hellem
primary_section: github-copilot
date: 2026-03-17 13:52:29 +00:00
section_names:
- ai
- devops
- github-copilot
- security
feed_name: Microsoft DevOps Blog
---

Dan Hellem announces the public preview of the Remote Azure DevOps MCP Server, a hosted MCP endpoint that connects Azure DevOps data to clients like Visual Studio and VS Code via GitHub Copilot Chat, and explains setup, current client support, and Entra ID authentication requirements.<!--excerpt_end-->

## Overview

When Microsoft released the local [Azure DevOps MCP Server](https://github.com/microsoft/azure-devops-mcp), it enabled customers to connect Azure DevOps data with tools like Visual Studio and Visual Studio Code through GitHub Copilot Chat.

The new **Remote Azure DevOps MCP Server** is a **hosted** version of that server using **streamable HTTP transport**. It aims to:

- Reduce setup/installation compared to the local server
- Enable use with tools/services that only support **remote** MCP servers

The Remote MCP Server preview is available now.

## Getting started

Add the server information to your `mcp.json`:

```json
{
  "servers": {
    "ado-remote-mcp": {
      "url": "https://mcp.dev.azure.com/{organization}",
      "type": "http"
    }
  },
  "inputs": []
}
```

More configuration options are covered in the official documentation:

- https://learn.microsoft.com/en-us/azure/devops/mcp-server/remote-mcp-server

## Supported clients and services

The Remote MCP Server:

- Is hosted on the **Azure DevOps** service
- Uses **Microsoft Entra** for authentication
- Requires your Azure DevOps organization to be **backed by Entra**
  - **Standalone organizations (MSAs) are not supported**

### Supported today (no additional onboarding)

- Visual Studio with GitHub Copilot
- Visual Studio Code with GitHub Copilot

## Coming soon

Some additional clients require **dynamic registration of an OAuth Client ID in Entra** before they can be used with the MCP server. Microsoft notes they’re working with the Entra team to enable this capability.

Examples mentioned:

- GitHub Copilot CLI
- Claude Desktop
- Claude Code
- ChatGPT

Also noted as **not yet available** (separate announcement planned when available):

- Azure AI Foundry
- Microsoft 365 Copilot
- Copilot Studio

## Local MCP Server status

You can continue using the local server for now:

- https://github.com/microsoft/azure-devops-mcp

However, Microsoft plans to **eventually archive** the local MCP Server repo and focus investment on the **Remote MCP Server**, with archiving aligned to the Remote MCP Server reaching **general availability** (no date announced).

## Support

During the public preview, submit issues/questions via the local MCP Server repo issue template:

- https://github.com/microsoft/azure-devops-mcp/issues/new?template=remote-mcp-server-issue.md


[Read the entire article](https://devblogs.microsoft.com/devops/azure-devops-remote-mcp-server-public-preview/)

