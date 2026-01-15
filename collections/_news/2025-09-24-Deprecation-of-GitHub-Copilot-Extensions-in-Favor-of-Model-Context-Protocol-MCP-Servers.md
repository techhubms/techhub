---
layout: post
title: Deprecation of GitHub Copilot Extensions in Favor of Model Context Protocol (MCP) Servers
author: Allison
canonical_url: https://github.blog/changelog/2025-09-24-deprecate-github-copilot-extensions-github-apps
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-09-24 15:00:18 +00:00
permalink: /github-copilot/news/Deprecation-of-GitHub-Copilot-Extensions-in-Favor-of-Model-Context-Protocol-MCP-Servers
tags:
- AI
- AI Agent Integration
- Copilot
- Copilot Marketplace
- Cross Platform Compatibility
- Developer Workflow
- DevOps
- GitHub Apps
- GitHub Copilot
- GitHub Copilot Extensions
- IDE Integration
- MCP
- News
- Open Standard
- Plugin Architecture
- Retired
- Server Side Extensions
- Sunset Notice
- Tool Invocation
- VS Code
section_names:
- ai
- devops
- github-copilot
---
Allison outlines the upcoming deprecation of GitHub Copilot Extensions as GitHub moves toward the Model Context Protocol (MCP), offering guidance for developers on transitioning to this universal standard.<!--excerpt_end-->

# Deprecation of GitHub Copilot Extensions: Moving to MCP Servers

**Author: Allison**

## What’s Changing

GitHub is officially deprecating Copilot Extensions (built as GitHub Apps) on **November 10, 2025**. Instead, the platform is adopting [Model Context Protocol (MCP)](https://modelcontextprotocol.io/), an open standard designed for integrating AI agents across environments. This standard allows developers to build an MCP server once for compatibility with any MCP-capable agent, including GitHub Copilot and others.

## Who Is Affected

- **Developers** who have created or are using Copilot Extensions as GitHub Apps.
- **Unaffected:**
  - Client-side VS Code Copilot Extensions (they remain fully supported)
  - Standard GitHub Apps without Copilot Extension features

## Why This Change Matters

GitHub Copilot Extensions only work inside Copilot chat, which required developers to build platform-specific solutions. MCP introduces:

- Universal, open integration standard
- Modular, composable architecture
- Cross-platform compatibility
- Easier maintenance and improved performance

With MCP, developers can focus on building their tool once and using it across multiple AI assistant platforms.

## Action Items for Developers

- **Existing Copilot Extensions:**
  - Plan your replacement strategy. Extensions will stop working after November 10, 2025.
- **Hybrid Apps:**
  - Disable the Copilot Extension config in your App settings before sunset if you want it to remain listed on the Marketplace.
- **Start with MCP:**
  - Explore servers in the [GitHub MCP Registry](https://github.com/mcp)
  - Read [MCP documentation](https://modelcontextprotocol.io/docs/develop/build-server) to learn about building new servers.
- **No New Extensions:**
  - Creation of new Copilot Extensions (server-side) is blocked after September 24, 2025.

## Timeline of Key Dates

- **September 24, 2025:** New Copilot Extension creation blocked
- **November 3–7, 2025:** Brownout testing (temporary interruptions)
- **November 10, 2025:** All Copilot Extensions (GitHub Apps) are disabled

## Frequently Asked Questions

### Why is GitHub making this change?

GitHub aims to unlock more ecosystem value and streamline development through MCP, allowing for greater compatibility and easier developer experience.

### What happens after November 10, 2025?

- Copilot Extension functionality will be fully disabled.
- Marketplace:
  - Copilot Extension-only apps: removed
  - Hybrid apps: must disable Copilot Extension config to remain in Marketplace
  - @mentions of retired extensions will be treated as plain text; VS Code extensions remain available

### Can I convert my Copilot Extension to a regular GitHub App?

If your app combines Copilot Extension and classic GitHub App features, only the GitHub App features continue after the sunset. Review your Marketplace settings before the deadline.

### What about private/internal Copilot Extensions?

They follow the same sunset timeline.

### How do I migrate to MCP?

- Study the [MCP developer docs](https://modelcontextprotocol.io/docs/develop/build-server)
- MCP requires new architecture, not direct migration

### Is there feature parity between Extensions and MCP?

Much functionality is replicated, but architectural differences exist, especially for agent-based scenarios.

### Where can I get support?

Consult documentation and community forums. Guides address common MCP scenarios.

### What about the Copilot Extensions Marketplace category?

It will be removed after November 10, 2025. Hybrid apps must disable their Extension configuration to stay listed as GitHub Apps.

### How will users find MCP servers?

The [GitHub MCP Registry](https://github.com/mcp) acts as a global directory.

### Will there be admin controls?

MCP supports organization-level enable/disable/allowlist policies, with gradual IDE rollout for per-server controls.

## Summary

Developers should begin planning the move to MCP now for continued compatibility with Copilot and other AI assistants. GitHub Copilot’s plugin system is evolving to an open, industry-standard approach.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-24-deprecate-github-copilot-extensions-github-apps)
