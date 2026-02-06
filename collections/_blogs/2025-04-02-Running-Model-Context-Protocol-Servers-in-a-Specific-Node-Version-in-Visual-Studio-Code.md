---
external_url: https://jessehouwing.net/vscode-running-mcp-using-node-version/
title: Running Model Context Protocol Servers in a Specific Node Version in Visual Studio Code
author: Jesse Houwing
feed_name: Jesse Houwing's Blog
date: 2025-04-02 17:56:59 +00:00
tags:
- Azure Pipelines
- Fast Node Manager
- GitHub
- MCP
- MCP Server
- Node Version Management
- Node Version Switcher
- Node.js
- Npx
- Playwright
- Typescript
- VS Code
- DevOps
- GitHub Copilot
- AI
- Blogs
- .NET
section_names:
- devops
- github-copilot
- ai
- dotnet
primary_section: github-copilot
---
In this post, Jesse Houwing details solutions for managing Node versions to run Model Context Protocol (MCP) servers with Visual Studio Code Insiders and Azure Pipelines extensions, sharing practical configurations and troubleshooting insights.<!--excerpt_end-->

# Running Model Context Protocol Servers in a Specific Node Version in Visual Studio Code

*Author: Jesse Houwing*

## Introduction

Managing multiple Node.js versions is a common challenge, especially when developing Azure Pipelines extensions that depend on different Node versions for backwards compatibility. In this post, Jesse Houwing explores how to run Model Context Protocol (MCP) servers in Visual Studio Code Insiders when the extensions and tools have varying Node.js version requirements.

## Context and Background

Jesse maintains several Azure Pipelines extensions, many of which are written in TypeScript and target Node 16 to ensure compatibility. However, while experimenting with Model Context Protocol (MCP) servers in Visual Studio Code Insiders, he encountered a situation where his default system Node version was older than required by the MCP server implementation.

## Managing Node Versions: Fast Node Manager (fnm)

To handle different Node environments, Jesse primarily uses [Fast Node Manager (fnm)](https://github.com/Schniz/fnm?ref=jessehouwing.net). Fnm streamlines environment variable setup, modifies the system PATH, and uses symlinks, making it easy to switch between Node versions. His typical workflow for starting development looks like this:

```bash
cd azure-pipelines-variables-tasks
fnm use v16
code .
```

This sequence ensures that Visual Studio Code starts with the desired Node.js version active.

## The MCP Server and Version Challenges

MCP servers can be implemented in various programming languages, but many are distributed as npm packages. Recent packages often require newer Node versions, which can lead to incompatibility with older environments. Specifically, the `@playwright/mcp@latest` package used to enable GitHub Copilot UI interaction required a newer Node version than Jesse’s default.

Installation of the Playwright MCP server results in entries like this in `settings.json`:

```json
"mcp": {
  "servers": {
    "playwright": {
      "command": "npx",
      "args": [ "-y", "@playwright/mcp@latest" ]
    }
  }
}
```

This relies on the `npx` and `node` executable versions present in PATH. However, if these are out-of-date, the MCP server fails to launch correctly.

## Debugging with fnm

Attempting to launch npx using a newer Node version with fnm:

```bash
fnm exec --using v22 npx -y @playwright/mcp@latest
```

did not resolve the issue. Fnm failed to find `npx` in this specific use case—a known [issue](https://github.com/Schniz/fnm/issues/1406?ref=jessehouwing.net) according to the project’s tracker. The resultant error:

```
2025-04-02 17:34:19.103 [info] Starting server from LocalProcess extension host
2025-04-02 17:34:19.120 [info] Connection state: Starting
2025-04-02 17:34:19.120 [info] Connection state: Running
2025-04-02 17:34:19.160 [warning] [server stderr] error: Can't spawn program: program not found
2025-04-02 17:34:19.160 [warning] [server stderr] Maybe the program npx does not exist or is not available in PATH?
2025-04-02 17:34:19.164 [info] Connection state: Error Process exited with code 1
```

## Node Version Switcher (nvs) as an Alternative

Jesse found that [Node Version Switcher (nvs)](https://github.com/jasongin/nvs?ref=jessehouwing.net) also provides an `exec` option for running commands in a specified Node version. To install and set up a specific newer Node version:

```bash
winget install jasongin.nvs

# restart shell

nvs install 22.14.0
```

With nvs, the Visual Studio Code MCP server configuration was updated as follows:

```json
"mcp": {
  "servers": {
    "playwright": {
      "command": "nvs",
      "args": ["exec", "22.14.0", "npx", "-y", "@playwright/mcp@latest"]
    }
  }
}
```

This allows the build terminal to use Node 16 (for existing pipelines) while the Playwright MCP server runs under Node 22, eliminating version conflicts.

## Final Thoughts

Jesse notes that, for now, it appears possible to use both fnm and nvs together, but he is considering migrating existing scripts entirely to nvs to avoid further compatibility issues. This approach provides a robust solution for developers dealing with rapidly evolving tools and differing runtime requirements within Visual Studio Code and similar environments.

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/vscode-running-mcp-using-node-version/)
