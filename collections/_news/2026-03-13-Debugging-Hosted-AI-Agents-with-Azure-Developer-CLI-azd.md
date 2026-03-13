---
layout: "post"
title: "Debugging Hosted AI Agents with Azure Developer CLI (azd)"
description: "This article introduces new Azure Developer CLI (azd) commands for diagnosing failures in hosted AI agents, allowing developers to display container status and stream live logs directly from the terminal. It covers the capabilities of the azure.ai.agents extension and practical usage for troubleshooting AI agent deployments."
author: "PuiChee (PC) Chan"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/azure-sdk/azd-ai-agent-logs-status/"
viewing_mode: "external"
feed_name: "Microsoft Azure SDK Blog"
feed_url: "https://devblogs.microsoft.com/azure-sdk/feed/"
date: 2026-03-13 19:19:52 +00:00
permalink: "/2026-03-13-Debugging-Hosted-AI-Agents-with-Azure-Developer-CLI-azd.html"
categories: ["AI", "Azure", "DevOps"]
tags: ["Agent Health", "AI", "AI Agents", "Azd", "Azure", "Azure Developer CLI", "Azure SDK", "Azure.ai.agents", "CLI Debugging", "Container Monitoring", "Developer Tools", "DevOps", "DevOps Tools", "Error Diagnosis", "Extension Upgrade", "Log Streaming", "News", "Terminal Commands"]
tags_normalized: ["agent health", "ai", "ai agents", "azd", "azure", "azure developer cli", "azure sdk", "azuredotaidotagents", "cli debugging", "container monitoring", "developer tools", "devops", "devops tools", "error diagnosis", "extension upgrade", "log streaming", "news", "terminal commands"]
---

PuiChee (PC) Chan explains how to use new Azure Developer CLI (azd) commands to diagnose and debug hosted AI agent failures, providing developers with direct access to container status and live logs from the terminal.<!--excerpt_end-->

# Debugging Hosted AI Agents with Azure Developer CLI (azd)

When a hosted AI agent crashes, `azd` now shows you the status and streams live logs—right from the CLI.

## What’s New?

The [`azure.ai.agents` extension](https://learn.microsoft.com/azure/developer/azure-developer-cli/extensions/azure-ai-foundry-extension) for the Azure Developer CLI introduces two key commands:

- `azd ai agent show`: Displays your agent’s container status, health, and error details.
- `azd ai agent monitor`: Streams container logs in real time, helping you track issues as they happen.

These commands centralize agent container health, replica state, and detailed error logs, eliminating the need to search through different portals or APIs.

## Why This Matters

Previously, debugging hosted AI agent failures required jumping between different Azure surfaces and tools. Now, you can:

- Instantly see failure causes, container states, and crash details.
- Access all relevant information in your terminal, minimizing tool switching.

## How To Use the Commands

To check container status and health:

```bash
azd ai agent show --name my-agent --version 1

# View status, health, and error details
```

To stream logs in real time:

```bash
azd ai agent monitor --name my-agent --version 1

# Fetch recent logs

azd ai agent monitor --name my-agent --version 1 -f

# Follow logs live

azd ai agent monitor --name my-agent --version 1 --type system

# View system logs
```

Both commands accept `--name` and `--version` flags to target specific AI agent deployments.

## Getting Started

These features are available starting with the `azure.ai.agents` extension **v0.1.12-preview**, included in `azd` **1.23.7** and later.

- Already using `azd`? Upgrade your extension:

```bash
azd extension upgrade azure.ai.agents
```

- New to `azd`? [Install it](https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd) and run:

```bash
azd ai agent init
```

to get started with hosted AI agents.

## Learn More and Feedback

- [File issues or start discussions on GitHub](https://github.com/Azure/azure-dev).
- [Sign up for azd user research](https://aka.ms/azd-user-research-signup) to help shape future improvements.

This feature was introduced in [PR #6895](https://github.com/Azure/azure-dev/pull/6895).

*Article by PuiChee (PC) Chan, originally published on the [Azure SDK Blog](https://devblogs.microsoft.com/azure-sdk/azd-ai-agent-logs-status/).*

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azd-ai-agent-logs-status/)
