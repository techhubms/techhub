---
external_url: https://devblogs.microsoft.com/dotnet/nuget-mcp-server-preview/
title: 'Announcing the NuGet MCP Server Preview: Real-Time NuGet Package Management with AI Integration'
author: Jeff Kluge
feed_name: Microsoft .NET Blog
date: 2025-08-14 17:00:00 +00:00
tags:
- .NET
- AI Powered Development
- Automation
- Continuous Integration
- Copilot
- Dependency Management
- MCP
- NuGet
- NuGet MCP Server
- NuGetSolver
- Package Management
- Package Vulnerabilities
- VS
- VS Code
section_names:
- ai
- coding
- devops
- github-copilot
primary_section: github-copilot
---
Jeff Kluge announces the preview of the NuGet MCP Server, highlighting its integration into AI-powered development workflows, advanced package management features, and support for Visual Studio, VS Code, and GitHub Copilot.<!--excerpt_end-->

# Announcing the NuGet MCP Server Preview: Real-Time NuGet Package Management with AI Integration

Jeff Kluge introduces the NuGet MCP Server in a new preview, extending Microsoft .NET development workflows with real-time NuGet package management leveraging AI. This server is designed to connect AI assistants and development environment tools like GitHub Copilot Coding Agent, Visual Studio, and VS Code, with live NuGet package information, advanced update logic, and automation capabilities.

## What is an MCP Server?

The **Model Context Protocol (MCP)** is an open standard, enabling AI assistants to securely interface with external tools and data sources. MCP servers provide a bridge between AI-powered agents and services such as NuGet, making up-to-date information and management capabilities accessible via automated assistants.

## Why a NuGet MCP Server?

With the constant evolution of the NuGet ecosystem, language models and code assistants can become outdated quickly. The NuGet MCP Server delivers:

- **Real-time package version discovery**
- **Automated security vulnerability remediation** (updating to the lowest compatible secure version)
- **Framework-aware package updating** (ensuring updates don’t break projects)
- **Support for both public and private NuGet feeds**
- **Integration with the NuGetSolver algorithm** (for automatic dependency conflict resolution)

## Getting Started

**Prerequisites:**

- .NET 10 Preview 6 (available from the [official .NET download page](https://dotnet.microsoft.com/download/dotnet/10.0))

**Installation:**

- Available as a [NuGet package](https://www.nuget.org/packages/NuGet.Mcp.Server)
- Add the following configuration for the MCP client:

```json
{
  "servers": {
    "nuget": {
      "type": "stdio",
      "command": "dnx",
      "args": [
        "NuGet.Mcp.Server",
        "--prerelease",
        "--yes"
      ]
    }
  }
}
```

- To use a specific version:

```json
{
  "servers": {
    "nuget": {
      "type": "stdio",
      "command": "dnx",
      "args": [
        "NuGet.Mcp.Server@0.2.0-preview",
        "--yes"
      ]
    }
  }
}
```

## Current Capabilities of NuGet MCP Server

- **Package Version Discovery**: Find latest available versions from any configured feed
- **Security Updates**: Update insecure packages to the lowest fixed version, minimizing breaking changes
- **Version Updates**: Update to highest compatible version considering project’s target frameworks
- **Integration with private feeds** for broader package discovery

## Integration with Development Tools

### Visual Studio

- Add MCP server config either next to a solution or in `%UserProfile%\.mcp.json`
- [More info on MCP server file locations](https://learn.microsoft.com/visualstudio/ide/mcp-servers?view=vs-2022#file-locations-for-automatic-discovery-of-mcp-configuration)
- Visual Studio auto-launches the server when configuration is detected

### VS Code

- One-click install for insiders and stable builds
  - [Install in VS Code](https://insiders.vscode.dev/redirect/mcp/install?name=nuget&amp;config=%7B%22type%22%3A%20%22stdio%22%2C%22command%22%3A%20%22dnx%22%2C%22args%22%3A%20%5B%22NuGet.Mcp.Server%22%2C%22--prerelease%22%2C%22--yes%22%5D%7D)
- `.vscode/mcp.json` manual config support

### GitHub Copilot Coding Agent

- Automates package management in repositories
- Requires .NET 10 SDK Preview 6+
- Example workflow file for setup:

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

- Configure MCP server in repository settings under Copilot -> Coding agent

## Preview Status and Feedback

This is an **early preview release**. The team encourages feedback, feature requests, and bug reports via [NuGet/Home GitHub Issues](https://github.com/NuGet/Home/issues/new?template=MCPSERVER.yml) to help guide future development.

---

*For more detailed guides, sample configurations, and additional information, refer to the linked documentation and blog posts cited throughout this announcement.*

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/nuget-mcp-server-preview/)
