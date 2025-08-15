---
layout: "post"
title: "Announcing the NuGet MCP Server Preview"
description: "This article introduces the preview release of the NuGet MCP Server, an open-source server built with .NET that bridges AI assistants like GitHub Copilot to up-to-date NuGet package information and advanced dependency management functionality. It explains the Model Context Protocol (MCP), installation instructions, configuration steps for Visual Studio, VS Code, and GitHub Copilot Coding Agent, and details how the NuGet MCP Server streamlines real-time NuGet package management, security updates, and compatibility-aware version updates for the .NET ecosystem."
author: "Jeff Kluge"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/nuget-mcp-server-preview/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2025-08-14 17:00:00 +00:00
permalink: "/2025-08-14-Announcing-the-NuGet-MCP-Server-Preview.html"
categories: ["AI", "Coding", "DevOps", "GitHub Copilot"]
tags: [".NET", ".NET 10 Preview", "AI", "AI Assistants", "Coding", "Copilot", "Copilot Coding Agent", "Dependency Management", "DevOps", "DevOps Automation", "GitHub Copilot", "LLM Integration", "MCP", "Microsoft Research", "Model Context Protocol", "News", "NuGet", "NuGet MCP Server", "NuGetSolver", "Open Source Tools", "Package Management", "Package Version Discovery", "SDK Configuration", "Security Updates", "Visual Studio", "Visual Studio Code", "YAML Workflows"]
tags_normalized: ["net", "net 10 preview", "ai", "ai assistants", "coding", "copilot", "copilot coding agent", "dependency management", "devops", "devops automation", "github copilot", "llm integration", "mcp", "microsoft research", "model context protocol", "news", "nuget", "nuget mcp server", "nugetsolver", "open source tools", "package management", "package version discovery", "sdk configuration", "security updates", "visual studio", "visual studio code", "yaml workflows"]
---

Jeff Kluge announces the preview release of the NuGet MCP Server, detailing how it empowers developers to connect AI assistants like GitHub Copilot with real-time NuGet package management and advanced automation for .NET projects.<!--excerpt_end-->

# Announcing the NuGet MCP Server Preview

**Author: Jeff Kluge**

## Overview

The NuGet MCP Server enables real-time NuGet package information and advanced management capabilities for AI-powered development workflows, including integration with GitHub Copilot and coding agents. Built using .NET and the Model Context Protocol (MCP), this server acts as a bridge between AI assistants and the dynamic NuGet ecosystem, ensuring package details and updates are always current.

## What is an MCP Server?

The Model Context Protocol (MCP) is an open standard that allows AI assistants to securely connect with external data and tools. An MCP server provides a bridge to services like NuGet, feeding up-to-date data to large language models and facilitating extended functionality—such as enhanced package discovery and automated updates.

The NuGet MCP Server addresses the challenge that LLMs can become outdated regarding new or changed packages, by:

- Providing instant information about recently published or updated packages
- Managing private feeds alongside public ones for a complete view
- Using the NuGetSolver algorithm (developed with Microsoft Research) to resolve dependency conflicts automatically

## Getting Started

- **Prerequisites:** .NET 10 Preview 6 or newer
- **Installation:** Available as a [NuGet package](https://www.nuget.org/packages/NuGet.Mcp.Server)

### Example Configuration for Your MCP Client

```json
{
  "servers": {
    "nuget": {
      "type": "stdio",
      "command": "dnx",
      "args": [ "NuGet.Mcp.Server", "--prerelease", "--yes" ]
    }
  }
}
```

Use the `--prerelease` flag to ensure preview version access. For specific versions, substitute the package name, e.g. `"NuGet.Mcp.Server@0.2.0-preview"`.

## Main Features

- **Package Version Discovery:** Check the latest versions of any NuGet package available on configured feeds.
- **Security Updates:** Automatically update packages to the lowest version that resolves security vulnerabilities, minimizing disruption.
- **Version Updates:** Upgrade dependencies to the highest compatible version for your target framework, reducing breakage from incompatible updates.

## Integration with Development Tools

### Visual Studio

Add configuration snippets to your `.mcp.json` file in your solution folder or `%UserProfile%\.mcp.json`. Visual Studio supports auto-discovery of these files—see [docs](https://learn.microsoft.com/visualstudio/ide/mcp-servers?view=vs-2022#file-locations-for-automatic-discovery-of-mcp-configuration) for locations.

### VS Code

Quickly install the NuGet MCP Server using the provided badge links or by manually inserting a configuration snippet to `.vscode/mcp.json`:

```json
{
  "servers": {
    "nuget": {
      "type": "stdio",
      "command": "dnx",
      "args": [ "NuGet.Mcp.Server", "--prerelease", "--yes" ]
    }
  }
}
```

### GitHub Copilot Coding Agent

The NuGet MCP Server integrates with Copilot's coding agent to streamline dependency management within CI/CD workflows. For GitHub repositories:

Create or update `.github/workflows/copilot-setup-steps.yml` as follows:

```yaml
name: "Copilot Setup Steps"
on:
  workflow_dispatch:
  push:
    paths:
      - .github/workflows/copilot-setup-steps.yml
  pull_request:
    paths:
      - .github/workflows/copilot-setup-steps.yml
jobs:
  copilot-setup-steps:
    runs-on: ubuntu-latest
    permissions:
      contents: read
    steps:
      - name: Install .NET 10.x
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: |
            1. x
          dotnet-quality: preview
```

Configure your NuGet MCP Server in Copilot via repository settings:

```json
{
  "mcpServers": {
    "nuget": {
      "command": "dnx",
      "args": [ "NuGet.Mcp.Server", "--prerelease", "--yes" ],
      "tools": ["*"],
      "type": "local"
    }
  }
}
```

## Feedback & Preview Status

This is a preview release—new features are under development, and community feedback is encouraged. Report issues or ideas at [NuGet/Home](https://github.com/NuGet/Home/issues/new?template=MCPSERVER.yml).

---

**References:**

- [Announcing the NuGet MCP Server Preview](https://devblogs.microsoft.com/dotnet/nuget-mcp-server-preview/)
- [MCP documentation](https://modelcontextprotocol.io/docs/getting-started/intro)
- [NuGetSolver announcement](https://devblogs.microsoft.com/dotnet/introducing-nugetsolver-a-powerful-tool-for-resolving-nuget-dependency-conflicts-in-visual-studio/)
- [.NET 10 Preview Download](https://dotnet.microsoft.com/download/dotnet/10.0)

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/nuget-mcp-server-preview/)
