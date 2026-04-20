---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/introducing-the-plugin-marketplace-for-azure-api-center/ba-p/4512231
section_names:
- ai
- azure
- github-copilot
- security
author: Sreekanth_Thirthala
tags:
- AI
- AI Plugins
- API Governance
- API Inventory
- Authentication
- Azure
- Azure API Center
- Claude Code
- Community
- Copilot CLI
- Data Plane URL
- Developer Workflow
- Enterprise Governance
- GitHub Copilot
- GitHub Copilot CLI
- Marketplace Endpoint
- Marketplace.git
- MCP Servers
- Plugin Marketplace
- Public Preview
- Security
- Skills
feed_name: Microsoft Tech Community
title: Introducing the plugin marketplace for Azure API Center
primary_section: github-copilot
date: 2026-04-17 00:47:14 +00:00
---

Sreekanth_Thirthala announces a public preview feature for Azure API Center: a plugin marketplace endpoint that lets developers discover and install AI plugins (including MCP servers and skills) from tools like Claude Code and GitHub Copilot CLI, while keeping enterprise governance and auth intact.<!--excerpt_end-->

## Introducing the plugin marketplace endpoint for Azure API Center (public preview)

This post introduces a **public preview** capability in **Azure API Center**: a **plugin marketplace endpoint** that helps developers discover and install **AI plugins**—including **MCP servers** and **skills**—directly from developer tools such as **Claude Code** and **GitHub Copilot CLI**.

## The problem being addressed

As AI plugins and MCP servers become more common in developer workflows, teams often lack a single, governed place to:

- Find approved plugins
- Manage and publish plugins in a controlled way

Without this, developers end up searching across documentation, Teams messages, and internal wikis, and platform teams struggle to ensure the right plugins are used.

## What’s new

When you:

- Register plugins in your **API Center inventory**, and
- Enable the **API Center portal**

Azure API Center now automatically provisions a **marketplace.git** endpoint at your API Center **data plane** URL.

### Marketplace endpoint format

Your marketplace endpoint follows this pattern:

`https://<service>.data.<region>.azure-apicenter.ms/workspaces/default/plugins/marketplace.git`

The endpoint acts as a **live, version-controlled catalog** of plugins in your API Center, including:

- Metadata
- Configuration
- Install instructions

This is designed for developer tooling to consume.

## Get up and running (example commands)

Developers can add the organization’s marketplace and start installing plugins with a couple of commands.

### In Claude Code

```text
/plugin marketplace add <url>
/plugin install plugin-name@myapicenter
```

Example URL shown in the post:

- `https://myapicenter.data.eastus.azure-apicenter.ms/workspaces/default/plugins/marketplace.git`

### In GitHub Copilot CLI

```text
/plugin marketplace add <url>
/plugin marketplace browse myapicenter
```

## Enterprise-ready governance and security

The marketplace endpoint is designed to be governed:

- Access to the marketplace endpoint **inherits the same authentication model** configured for the API Center portal.
- Platform teams control what gets published.
- Developers get a trusted source of approved plugins.

## Documentation

- https://learn.microsoft.com/en-us/azure/api-center/enable-api-center-plugin-marketplace

## Post metadata

- Updated: Apr 16, 2026
- Version: 1.0

[Read the entire article](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/introducing-the-plugin-marketplace-for-azure-api-center/ba-p/4512231)

