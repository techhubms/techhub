---
external_url: https://devblogs.microsoft.com/dotnet/microsoft-learn-mcp-server-elevates-development/
title: 'Microsoft Learn MCP Server: Next-Level Copilot Integration for Developers'
author: Wendy Breiding (SHE/HER), Eric Imasogie
feed_name: Microsoft .NET Blog
date: 2025-12-08 20:00:00 +00:00
tags:
- .NET
- API References
- C#
- Code Suggestions
- Copilot
- Copilot CLI
- Copilot Coding Agent
- Developer Experience
- Developer Tools
- Docs
- Documentation Integration
- Integration Guide
- Learn
- MCP
- MCP Server
- Microsoft Learn
- VS
- VS Code
section_names:
- ai
- coding
- github-copilot
---
Wendy Breiding and Eric Imasogie explain how the Microsoft Learn MCP server upgrades the GitHub Copilot experience for developers, with step-by-step integration guides across key Microsoft development tools.<!--excerpt_end-->

# Microsoft Learn MCP Server: Next-Level Copilot Integration for Developers

**Authors:** Wendy Breiding (SHE/HER), Eric Imasogie

## Overview

The Microsoft Learn Model Context Protocol (MCP) server delivers up-to-date, scenario-aware Microsoft product documentation, code samples, and learning resources directly to GitHub Copilot. This integration ensures developers get relevant, accurate information as they code, reducing hallucinations and improving productivity in tools like Visual Studio, VS Code, the Copilot CLI, and Copilot Coding Agent.

## Why GitHub Copilot Needs Up-to-Date Context

Copilot's core models are trained on public datasets and may not have knowledge of the newest frameworks, APIs, or features (like Aspire 13 or the Agent Framework). As a result, developers sometimes encounter hallucinations or outdated suggestions. The Learn MCP server bridges this gap by injecting real-time official documentation and examples into Copilot's responses.

## Key Benefits

- **Trusted, Current Suggestions:** MCP server draws directly from Microsoft Learn to reduce stale or incorrect Copilot output.
- **Context-Aware Content:** Dynamically delivers documentation and code samples targeted to your exact scenario and framework version (e.g., .NET 10, Aspire, C# APIs).
- **Workflow Acceleration:** Inline documentation means less time spent searching for guidance, more time writing code.
- **Continuous Learning:** Access to official tutorials and code samples boosts upskilling as you work.

## Practical Scenarios

- **API References:** While building with ASP.NET Core, Copilot offers direct access to Microsoft Identity docs tailored to your project's specifics.
- **Best Practice Surfacing:** As you write MCP server code, Copilot suggests practices and examples from official sources.
- **Onboarding New Tech:** Experimenting with libraries like gRPC or SignalR triggers Copilot to recommend the correct Learn modules and samples.

## Step-by-Step Integration Guides

### Visual Studio

1. Use Visual Studio 2026 or 2022 (v17.14+).
2. Ensure MS Learn MCP server is enabled in Copilot chat settings.

### Visual Studio Code

1. Open VS Code, visit the Extensions view.
2. Install the GitHub Copilot extension.
3. In the MCP Server section, search for "Microsoft Docs" and install.

### Copilot CLI

1. Type `/mcp add` in the Copilot CLI.
2. Name the server "microsoft-docs".
3. Choose HTTP as the connection type.
4. Enter: <https://learn.microsoft.com/api/mcp>
5. Save the configuration.

### Copilot Coding Agent

1. Go to your repository's settings > Copilot > Coding Agent.
2. In Model Context Protocol, add:

   ```json
   {
     "mcpServers": {
       "microsoft-docs": {
         "type": "http",
         "url": "https://learn.microsoft.com/api/mcp",
         "tools": ["*"]
       }
     }
   }
   ```

## Resources

- [MS Learn MCP server documentation](https://learn.microsoft.com/training/support/mcp)
- [MS Learn MCP server GitHub Repo](https://github.com/MicrosoftDocs/mcp)

## Conclusion

With the Microsoft Learn MCP server, Copilot becomes a more reliable, up-to-date, and contextually aware assistant for all .NET and C# development. This means less wasted time chasing documentation, more trustworthy code suggestions, and continuous learning within your favorite Microsoft tools.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/microsoft-learn-mcp-server-elevates-development/)
