---
layout: post
title: 'Locking Down MCP: Create a Private Registry on Azure API Center for GitHub Copilot and VS Code'
author: tjsingh85
canonical_url: https://devblogs.microsoft.com/all-things-azure/locking-down-mcp-create-a-private-registry-on-azure-api-center-and-enforce-it-in-github-copilot-and-vs-code/
viewing_mode: external
feed_name: Microsoft All Things Azure Blog
feed_url: https://devblogs.microsoft.com/all-things-azure/feed/
date: 2025-12-04 15:06:03 +00:00
permalink: /github-copilot/news/Locking-Down-MCP-Create-a-Private-Registry-on-Azure-API-Center-for-GitHub-Copilot-and-VS-Code
tags:
- Anonymous Access
- API Management
- API Registry
- Azure API Center
- CORS
- Developer Productivity
- Developer Tools
- Endpoint Configuration
- Enterprise Integration
- GitHub
- MCP
- MCP (model Context Protocol)
- Microsoft Azure
- V0.1 MCP Registry
- VS Code
section_names:
- ai
- azure
- devops
- github-copilot
---
tjsingh85 delivers a hands-on guide for setting up a private MCP registry with Azure API Center, enforcing it for GitHub Copilot and VS Code integration, and troubleshooting common setup errors for developers and DevOps teams.<!--excerpt_end-->

# Locking Down MCP: Create a Private Registry on Azure API Center for GitHub Copilot and VS Code

Many organizations need a private Model Context Protocol (MCP) registry to control which services can be discovered and used by GitHub Copilot and Visual Studio Code. In this step-by-step guide, tjsingh85 covers how to set up a registry on Azure API Center, register MCP servers, configure anonymous access, wire up GitHub settings, and verify enforcement in VS Code.

## Table of Contents

- [Overview](#overview)
- [Option 1: Self-hosted MCP Registry](#option-1-self-hosted-mcp-registry)
- [Option 2: Azure API Center MCP Registry](#option-2-azure-api-center-mcp-registry)
- [Step 1: Create Azure API Center Instance](#step-1-create-azure-api-center-instance)
- [Step 2: Register MCP Servers](#step-2-register-mcp-servers)
- [Step 3: Enable API Center Portal and Anonymous Access](#step-3-enable-api-center-portal-and-anonymous-access)
- [Step 4: Find the Real MCP Registry Endpoint](#step-4-find-the-real-mcp-registry-endpoint)
- [Step 5: Configure GitHub’s MCP Registry URL](#step-5-configure-githubs-mcp-registry-url)
- [Step 6: Test in VS Code](#step-6-test-in-vs-code)
- [Troubleshooting](#troubleshooting)
- [Summary](#summary)
- [Resources](#resources)

---

## Overview

This blog addresses a common enterprise request: how to create and enforce a private MCP registry so GitHub Copilot and VS Code only work with approved MCP servers. The main solution uses Azure API Center's registry features and walks through every step—including CORS requirements, endpoint URL patterns, permissions, and UI quirks.

## Option 1: Self-hosted MCP Registry

- Run your own service that implements the v0.1 MCP Registry specification.
- Full control and flexibility, but requires ongoing infrastructure and CORS configuration.

## Option 2: Azure API Center MCP Registry (Simpler Approach)

- Built-in v0.1 MCP Registry support, CORS pre-configured, and a simple UI.
- No infrastructure hassle.
- For this guide, Azure API Center is used.

## Step 1: Create Azure API Center Instance

1. In the Azure Portal, search for "API Center" and click **+ Create**.
2. Fill in the required info: Subscription, Resource group, Name (e.g. `private-mcp-registry`), Region (e.g. `East US`), SKU (free is fine to start).
3. Deploy, then note your **data-plane endpoint** (e.g. `https://private-mcp-registry.data.eastus.azure-apicenter.ms`).

## Step 2: Register MCP Servers

Two registration paths:

- **Custom MCP server**: Go to Assets → APIs → Register an API, select MCP type, and register your endpoint.
- **Discovery feature**: Use the preview Discovery section to add well-known MCP servers like Microsoft Docs or Atlassian with a click.
- Servers can be assigned to different environments (production, test, etc.).

## Step 3: Enable API Center Portal and Anonymous Access

*Critical for Copilot integration.*

1. In your Azure API Center resource, go to API Center portal → Settings.
2. On the Identity provider tab, run the startup wizard and hit Save + publish.
3. Now in Settings → API visibility tab, toggle **Anonymous access** ON and Save + publish again.

This enables unauthenticated read access for MCP clients (required for Copilot and VS Code). Without this, you’ll hit "401 Unauthorized" errors.

## Step 4: Find the Real MCP Registry Endpoint

Don’t use just the base endpoint—**you must include the workspace path**:

```
https://<your-api-center-endpoint>/workspaces/default/v0.1/servers
```

Test with:

```sh
curl "https://private-mcp-registry.data.eastus.azure-apicenter.ms/workspaces/default/v0.1/servers"
```

You should see a JSON server list. If not, check your workspace path and permissions.

## Step 5: Configure GitHub’s MCP Registry URL

In your GitHub Enterprise admin settings:

1. Go to Enterprise → AI Controls → MCP.
2. For **MCP Registry URL**, use the workspace base URL:
   - Example: `https://private-mcp-registry.data.eastus.azure-apicenter.ms/workspaces/default`
   - **Do not append** `/v0.1/servers`—GitHub handles that automatically.
3. Set your MCP policy (allow all, or restrict to registry).

## Step 6: Test in VS Code

After settings propagate:

- Open VS Code and review MCP servers—only those registered through your Azure API Center registry should appear.
- Disabled servers (not in your approved registry) display a warning.
- In the Output window select ‘Window’ to confirm registry URL is correct.

If the registry isn’t reflected, restart VS Code.

## Troubleshooting

| Issue | Solution |
| --- | --- |
| 401 Unauthorized | Ensure correct user role and anonymous access is enabled |
| Save + Publish Greyed Out | Enable the API portal and complete identity provider setup |
| 404 Not Found | Confirm the workspace segment is present in your endpoint URL |

## Summary

Setting up a private MCP registry with Azure API Center allows you to strictly control which MCP servers are available to GitHub Copilot and VS Code for your enterprise. Key points learned:

- Use the proper workspace path in endpoint URLs.
- Anonymous access must be enabled for Copilot integration.
- Only MCP servers registered in Azure API Center (and whitelisted by policy) become discoverable in VS Code.
- For maximum privacy, a self-hosted registry remains an option but comes with more complexity.

As MCP support matures, expect future options like authenticated endpoints.

## Resources

- [Configure MCP Registry on GitHub Docs](https://docs.github.com/en/enterprise-cloud@latest/copilot/how-tos/administer-copilot/manage-mcp-usage/configure-mcp-registry)
- [Microsoft Azure API Center: MCP Overview](https://learn.microsoft.com/en-us/azure/api-management/mcp-server-overview)
- [Configure MCP Server Access on GitHub](https://docs.github.com/en/enterprise-cloud@latest/copilot/how-tos/administer-copilot/manage-mcp-usage/configure-mcp-server-access)

This post appeared first on "Microsoft All Things Azure Blog". [Read the entire article here](https://devblogs.microsoft.com/all-things-azure/locking-down-mcp-create-a-private-registry-on-azure-api-center-and-enforce-it-in-github-copilot-and-vs-code/)
