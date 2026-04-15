---
feed_name: Microsoft VisualStudio Blog
external_url: https://devblogs.microsoft.com/visualstudio/azure-mcp-tools-now-ship-built-into-visual-studio-2022-no-extension-required/
title: Azure MCP tools now ship built into Visual Studio 2022 — no extension required
primary_section: github-copilot
tags:
- .NET
- AI
- AppLens
- ASP.NET Core
- Azd
- Azure
- Azure App Service
- Azure Developer CLI
- Azure Development Workload
- Azure MCP Server
- Azure Storage
- Azure Subscription Permissions
- Developer Tooling
- GitHub Copilot
- GitHub Copilot Chat
- GitHub Copilot For Azure
- IDE Integration
- KQL
- Log Analytics
- MCP Tools
- News
- Resource Health
- Visual Studio
- Visual Studio Installer
- VS
author: Yun Jung Choi
date: 2026-04-15 17:30:28 +00:00
section_names:
- ai
- azure
- dotnet
- github-copilot
---

Yun Jung Choi explains that Azure MCP tools are now built into Visual Studio 2022 via the Azure development workload, letting developers enable an Azure MCP Server inside GitHub Copilot Chat to provision resources, deploy apps, and troubleshoot Azure services without installing a separate extension.<!--excerpt_end-->

# Azure MCP tools now ship built into Visual Studio 2022 — no extension required

Azure MCP tools are now built into Visual Studio 2022 as part of the Azure development workload—no separate extension to find, install, or update. You can enable over 230 tools across 45 Azure services directly in GitHub Copilot Chat and manage Azure resources, deployments, and diagnostics without leaving your IDE.

## What changed

Previously, using Azure MCP tools in Visual Studio 2022 required installing the **“GitHub Copilot for Azure (VS 2022)”** extension from the Visual Studio Marketplace, running the VSIX installer, and restarting Visual Studio. If something went wrong, you often had to uninstall and reinstall the extension.

Starting now:

- Azure MCP tools ship as part of the **Azure development** workload in **Visual Studio 2022**.
- There’s no separate extension to manage.
- The **Azure MCP Server** becomes available directly in **GitHub Copilot Chat** when the workload is installed.
- You enable it once, and it remains enabled across sessions.

Benefits called out:

- Fewer installation steps
- No extension/IDE version mismatches
- Single update path via the **Visual Studio Installer**
- Azure MCP Server updates ride along with regular Visual Studio releases

> **Note:** VS-specific tools available in Visual Studio 2026 are not included in Visual Studio 2022.

## What you get

The Azure MCP Server surfaces **230+ tools** spanning **45 Azure services** through GitHub Copilot Chat.

Key scenarios:

- **Learn** — ask about Azure services, best practices, architecture patterns
- **Design & develop** — get recommendations and configure application code
- **Deploy** — provision resources and deploy directly from the IDE
- **Troubleshoot** — query logs, check health, diagnose production issues

Tools show up in **all tools** mode in GitHub Copilot Chat. You choose which tools to enable, and Copilot calls them automatically when prompts relate to Azure.

## See it in action

Each prompt below triggers one or more Azure MCP tool calls.

### Explore your Azure resources

```text
List my storage accounts in my current subscription.
```

Copilot queries subscriptions and storage accounts and returns details such as name, location, and SKU in the chat window.

### Deploy your app

```text
Deploy my ASP.NET Core app to Azure.
```

Copilot identifies your project, guides you through creating an **App Service** resource, and initiates deployment using **azd**. Progress is shown in chat output.

### Diagnose issues

```text
Help diagnose my App Service resource.
```

Copilot uses tools like **AppLens** and resource health checks to analyze availability and provide recommendations.

### Query your logs

```text
Query my Log Analytics workspace for exceptions.
```

Copilot generates and runs a **KQL** query against **Log Analytics**, returning recent exceptions (timestamps, messages, stack traces). You can iterate with follow-up prompts.

## How to enable Azure MCP tools

Azure MCP tools ship with the **Azure development** workload in **Visual Studio 2022 version 17.14.30+**, but they’re **disabled by default**.

1. **Update Visual Studio 2022**
   - Open **Visual Studio Installer** and ensure you’re on **17.14.30 or higher**.
2. **Install the Azure development workload**
   - In Visual Studio Installer, select **Modify** and check **Azure development**, then apply changes.
3. **Launch Visual Studio 2022**
   - Open or create a project, then open **GitHub Copilot Chat**.
4. **Sign in**
   - Sign in to **GitHub** (Copilot) and **Azure** (resource access).
5. **Enable the Azure MCP Server**
   - In Copilot Chat, select **Select tools** (two wrenches icon).
   - Find **Azure MCP Server** and toggle it on.

[![Visual Studio 2022 showing Select tools with Azure MCP Server enabled](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2026/04/visual-studio-2022-enable-tools.webp)](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2026/04/visual-studio-2022-enable-tools.webp)

Once enabled, Azure MCP tools are available in every Copilot Chat session (you don’t need to re-enable after restarting Visual Studio).

## Things to know

- Azure MCP tools are **disabled by default** and must be enabled in **Select tools**.
- Tools specific to **Visual Studio 2026** are not included in **Visual Studio 2022**.
- Tool availability depends on **Azure subscription permissions** (if you can’t do it in the portal, you can’t do it via MCP tools).
- Requires an active **GitHub Copilot subscription** and an **Azure account**.
- The Azure MCP Server version updates with regular Visual Studio releases.

## Learn more

- [GitHub Copilot for Azure Documentation](https://learn.microsoft.com/en-us/azure/developer/github-copilot-azure/introduction?tabs=vscode)
- [Azure MCP Server Documentation](https://aka.ms/azmcp/docs)
- [Azure MCP Server Repo](https://aka.ms/azmcp)
- Feedback: **Help > Send Feedback** in Visual Studio 2022, or file issues on the [Azure MCP Server GitHub repository](https://github.com/microsoft/mcp)


[Read the entire article](https://devblogs.microsoft.com/visualstudio/azure-mcp-tools-now-ship-built-into-visual-studio-2022-no-extension-required/)

