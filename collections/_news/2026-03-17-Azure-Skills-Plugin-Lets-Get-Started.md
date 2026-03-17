---
title: Azure Skills Plugin – Let’s Get Started!
section_names:
- ai
- azure
- github-copilot
date: 2026-03-17 21:05:08 +00:00
tags:
- AI
- All Things Azure
- Azd
- Azure
- Azure AI Foundry
- Azure CLI
- Azure Developer CLI
- Azure MCP Server
- Azure Resource Groups
- Azure Skills Plugin
- Claude Code
- Context7
- GitHub Copilot
- GitHub Copilot Chat
- GitHub Copilot CLI
- MCP
- MCP Server Definitions
- Microsoft Foundry
- News
- Node.js 18
- Npx
- Plugin Installation
- Troubleshooting
- VS Code
- VS Code Marketplace
external_url: https://devblogs.microsoft.com/all-things-azure/azure-skills-plugin-lets-get-started/
feed_name: Microsoft All Things Azure Blog
primary_section: github-copilot
author: Chris Harris
---

Chris Harris walks through installing and verifying the Azure Skills Plugin across GitHub Copilot CLI, VS Code, and Claude Code, including required prerequisites, MCP server setup, and quick smoke tests to confirm the Azure tools are actually being called.<!--excerpt_end-->

## Overview

This post is focused on getting the **Azure Skills Plugin** installed, verified, and ready to use. It does **not** go deep on architecture or capabilities yet.

The plugin works across multiple agent hosts:

- **GitHub Copilot CLI**
- **VS Code**
- **Claude Code**

Each install should take under a minute.

Plugin repo: https://aka.ms/azure-skills

## Prerequisites (All Hosts)

- **Node.js 18+** — required for MCP servers (run via `npx`)
- **An Azure account** — free tier works for exploring; subscription needed for deployments
- **Azure CLI (`az`)** installed and authenticated:

```bash
az login
```

- **Azure Developer CLI (`azd`)** installed and authenticated:

```bash
azd auth login
```

## Part 1: GitHub Copilot CLI

### Install (inside Copilot CLI)

```bash
# Add the marketplace source (first time only)
/plugin marketplace add microsoft/azure-skills

# Install the plugin
/plugin install azure@azure-skills
```

### Update

```bash
/plugin update azure@azure-skills
```

### After install: reload MCP servers

Copilot CLI does **not** auto-load newly installed MCP servers. Run:

```bash
/mcp reload
```

This loads MCP server definitions from the plugin so the Azure tools become available.

### Verify

```bash
/mcp status
```

You should see the MCP servers (including `azure` and `context7`) listed and running.

### Smoke tests

In Copilot CLI, try:

> “List my Azure resource groups”

If the Azure MCP Server loaded correctly, Copilot should call the resource group tool and return live results.

> “What Azure services would I need to deploy a Python web API?”

If skills loaded, Copilot should reference `azure-prepare` (instead of giving only generic advice).

## Part 2: VS Code

### Additional prerequisites

- **Git CLI** installed (required for the extension)
- **VS Code Stable or Insiders**

### Install

Install the **Azure MCP** extension from the VS Code Marketplace:

- Marketplace: https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azure-mcp-server
- Install in VS Code: `vscode:extension/ms-azuretools.vscode-azure-mcp-server`
- Install in VS Code Insiders: `vscode-insiders:extension/ms-azuretools.vscode-azure-mcp-server`

Or search “Azure MCP” in Extensions (Extension ID: `ms-azuretools.vscode-azure-mcp-server`).

The extension auto-installs a companion extension, **GitHub Copilot for Azure**, which contains the Azure skills.

### Verify

1. Open Copilot Chat (`Ctrl+Shift+I` / `Cmd+Shift+I`)
2. Ensure you’re in **Agent mode** (not Ask or Edit)
3. Open Command Palette (`Ctrl+Shift+P`) → search “MCP” → confirm servers are listed and running

### Smoke tests

> “List my Azure resource groups”

Copilot should call `mcp_azure_mcp_group_list` and return your actual resource groups.

> “What AI models are available in Microsoft Foundry?”

The Azure MCP Server includes Foundry tools, so Copilot should query the model catalog directly.

## Part 3: Claude Code

### Install (from terminal)

```bash
# Add the marketplace source (first time only)
claude plugin marketplace add microsoft/azure-skills

# Install the plugin
claude plugin install azure@azure-skills
```

### Update

```bash
# Update the marketplace
claude plugin marketplace update azure-skills

# Update the plugin
claude plugin update azure
```

## What gets installed

All three hosts install the same plugin payload.

### Skills (`../azure-skills/skills/`) (for Copilot)

The plugin installs **21 curated skill files** (each a `SKILL.md`) that instruct the AI when/how to help with specific Azure scenarios. These are instruction files (they don’t execute code).

Installed path example:

```text
.copilot/installed-plugins/azure-skills/azure/skills/
├── appinsights-instrumentation/SKILL.md
├── azure-ai/SKILL.md
├── azure-aigateway/SKILL.md
├── azure-cloud-migrate/SKILL.md
├── azure-compliance/SKILL.md
├── azure-compute/SKILL.md
├── azure-cost-optimization/SKILL.md
├── azure-deploy/SKILL.md
├── azure-diagnostics/SKILL.md
├── azure-hosted-copilot-sdk/SKILL.md
├── azure-kusto/SKILL.md
├── azure-messaging/SKILL.md
├── azure-prepare/SKILL.md
├── azure-quotas/SKILL.md
├── azure-rbac/SKILL.md
├── azure-resource-lookup/SKILL.md
├── azure-resource-visualizer/SKILL.md
├── azure-storage/SKILL.md
├── azure-validate/SKILL.md
├── entra-app-registration/SKILL.md
└── microsoft-foundry/SKILL.md
```

### MCP servers (`.copilot/installed-plugins/azure-skills/azure/.mcp.json`) (for Copilot)

The plugin configures two MCP (Model Context Protocol) servers:

```json
{
  "servers": {
    "azure": {
      "command": "npx",
      "args": ["-y", "@azure/mcp@latest", "server", "start"]
    },
    "context7": {
      "command": "npx",
      "args": ["-y", "@upstash/context7-mcp@latest"]
    }
  }
}
```

Server summary:

- `azure`: Azure MCP Server (59 tools across 40+ Azure services), including **Microsoft Foundry** tooling (model catalog, deployments, agent management). It can list resources, provision infra, query logs, check prices.
- `context7`: Provides up-to-date, version-specific documentation and code examples directly to the assistant.

### Where files land

Plugin payload location:

```text
your-profile/
├── .copilot/installed-plugins/azure-skills/azure/
│   ├── skills/
│   └── .mcp.json
└── ...
```

## Skill and tool budgets (limits)

Because agent hosts have limits on how much skill/tool content they can include, you can hit budgets—especially if you’re running multiple plugins or MCP servers—causing some skills/tools to be silently excluded.

### GitHub Copilot CLI

- Skill budget: **15,000 tokens by default** (can be increased via an environment variable)
- Context auto-compacts at **95%** of the token limit; long sessions can drop earlier context
- Commands:
  - `/context` to see token usage
  - `/compact` to manually compress context
  - `/skills` to toggle skills on/off

### Claude Code

- Skill budget: **2% of the context window**, with a fallback floor of **16,000 characters**
- `/context` shows warnings if skills are being excluded
- Override budget via environment variable: `SLASH_COMMAND_TOOL_CHAR_BUDGET`

> Tip: If Azure skills don’t seem to activate, check whether you have many other plugins or MCP servers loaded. Reducing the total tool/skill count often resolves it.

## Troubleshooting

Troubleshooting guide: https://github.com/microsoft/azure-skills/blob/main/Troubleshooting.md

### “Skills don’t seem to be loaded”

- **Copilot CLI**: run `/plugin list` to confirm installed
- **VS Code**: check `.github/plugins/azure-skills/` exists; reload window (Command Palette → “Reload Window”); confirm **Agent mode**
- **Claude Code**: run `/skills` to confirm

### “MCP tools aren’t showing up”

- **Copilot CLI**: run `/mcp reload` (required after install), then `/mcp show azure`
- **VS Code**: check `.mcp.json` in workspace root; Command Palette → “MCP: Restart Servers”; check Output panel → “MCP”
- **Claude Code**: run `/mcp` and inspect MCP servers/tools
- **All hosts**: confirm Node.js 18+ (`node --version`)

### “Azure commands fail with auth errors”

- Refresh auth: `az login`
- Deployment auth: `azd auth login`
- Set subscription:

```bash
az account set --subscription <your-sub-id>
```

### “I already had Azure MCP installed separately”

The plugin’s `.mcp.json` uses the same `@azure/mcp` package; you’re effectively adding the **skills layer** on top of your existing tools.

## What’s next

Next in the series: how skills and MCP servers work together—skill activation, orchestrating tool calls, and progressive disclosure for performance.

## Try it yourself

1. Install: https://aka.ms/azure-skills
2. Open Copilot CLI, VS Code, or Claude Code
3. Ask: “List my Azure resource groups”
4. Ask: “What would I need to deploy a Python Flask API to Azure?”

If both work, you’re ready for the rest of the series.


[Read the entire article](https://devblogs.microsoft.com/all-things-azure/azure-skills-plugin-lets-get-started/)

