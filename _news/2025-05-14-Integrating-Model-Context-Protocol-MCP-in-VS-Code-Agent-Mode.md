---
layout: "post"
title: "Integrating Model Context Protocol (MCP) in VS Code Agent Mode"
description: "This article, authored by Harald Kirschner, details the addition of Model Context Protocol (MCP) support in Visual Studio Code's agent mode. It explores how MCP enables VS Code to connect AI agents to external tools and services securely, improving extensibility, user control, and ecosystem collaboration."
author: "Harald Kirschner"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://code.visualstudio.com/blogs/2025/05/12/agent-mode-meets-mcp"
viewing_mode: "external"
feed_name: "Visual Studio Code Releases"
feed_url: "https://code.visualstudio.com/feed.xml"
date: 2025-05-14 00:00:00 +00:00
permalink: "/2025-05-14-Integrating-Model-Context-Protocol-MCP-in-VS-Code-Agent-Mode.html"
categories: ["AI", "Coding"]
tags: ["AI", "AI Agents", "Coding", "Developer Tools", "Dynamic Tool Discovery", "Extension Development", "GitHub MCP Server", "MCP", "News", "Playwright MCP Server", "Secrets Management", "Tool Server", "User Control", "VS Code", "Workspace Integration"]
tags_normalized: ["ai", "ai agents", "coding", "developer tools", "dynamic tool discovery", "extension development", "github mcp server", "mcp", "news", "playwright mcp server", "secrets management", "tool server", "user control", "vs code", "workspace integration"]
---

Harald Kirschner explains how integrating Model Context Protocol (MCP) in VS Code empowers AI agents to use external tools, enhancing flexibility and control for developers.<!--excerpt_end-->

# Integrating Model Context Protocol (MCP) in VS Code Agent Mode

*By Harald Kirschner*

Bringing external tools into Visual Studio Code's agent mode just got easier with the integration of the Model Context Protocol (MCP). MCP provides a standardized way for AI agents in VS Code to interact with a broad range of external services in a secure, user-controlled manner.

## Why MCP?

Previously, extending agent mode in VS Code relied on custom extension APIs or configuration files, limiting wide adoption and usability. MCP, akin to HTTP for the web, allows any client—VS Code included—to connect to tool servers such as databases, code search, deployment systems, and more. This dramatically expands the agent mode ecosystem.

Notable servers now available include:

- **Playwright MCP Server** for frontend verification
- **GitHub MCP Server** for repository insights
- **Context7** for advanced API usage

The MCP [server list](https://github.com/modelcontextprotocol/servers) is rapidly growing, reinforcing MCP’s role as foundational in agent-tool integrations.

## Simplicity and Security

VS Code's design around user empowerment shaped its MCP integration:

- **Setup is easy**: Add servers straight from NPM, PyPI, or Docker using the `MCP: Add Server` command, or onboard via _Install in VS Code_ shortcuts.
- **Security first**: Store secrets securely using input variables and reference trusted `.env` files, minimizing risk.
- **User control**: With the tool picker, users decide what tools the agent can access in each session. Server logs and controls simplify development and debugging.

## Powerful MCP Features

With MCP, VS Code gains advanced capabilities:

- **Workspace-awareness**: MCP servers use workspace roots for tailored operations (e.g., monorepo-wide searches, deployment activation).
- **Dynamic tool discovery**: Toolsets adjust on-the-fly, exposing relevant actions as projects evolve.
- **Tool annotations**: Servers provide metadata for better UX (e.g., read-only hints, human-readable names).
- **Streamable HTTP support**: Enhances remote integration and scalability.

VS Code is committed to supporting new MCP features for richer, more flexible AI agent workflows.

## Growing the Ecosystem

Official servers exist from [GitHub](https://github.com/github/github-mcp-server/), [Playwright](https://github.com/microsoft/playwright-mcp), [Azure](https://github.com/Azure/azure-mcp), and [Perplexity](https://github.com/ppl-ai/modelcontextprotocol/), with more joining regularly. The VS Code team contributes to MCP’s specification, fostering collaboration across the AI tooling landscape.

Upcoming work includes improved authorization, prompt handling, resource management, and sampling support.

## Get Started

Explore more about MCP at [ModelContextProtocol.io](https://modelcontextprotocol.io/), check out [VS Code’s docs](https://code.visualstudio.com/docs/copilot/chat/mcp-servers), or experiment with open-source servers.

---

*Happy Coding!*

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2025/05/12/agent-mode-meets-mcp)
