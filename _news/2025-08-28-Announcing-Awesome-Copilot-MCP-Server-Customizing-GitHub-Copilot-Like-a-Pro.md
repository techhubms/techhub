---
layout: "post"
title: "Announcing Awesome Copilot MCP Server: Customizing GitHub Copilot Like a Pro"
description: "This news article introduces the Awesome Copilot MCP Server, highlighting how developers can easily discover, search, and save custom GitHub Copilot chat modes, instructions, and prompts directly from Copilot Chat. The guide covers installing the server using Docker, integrating with Visual Studio Code, and adopting workflow enhancements for personalized AI-based code assistance."
author: "Justin Yoo"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/blog/announcing-awesome-copilot-mcp-server"
viewing_mode: "external"
feed_name: "Microsoft Blog"
feed_url: "https://devblogs.microsoft.com/feed"
date: 2025-08-28 17:00:18 +00:00
permalink: "/2025-08-28-Announcing-Awesome-Copilot-MCP-Server-Customizing-GitHub-Copilot-Like-a-Pro.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["AI", "AI Customization", "Awesome Copilot", "Chat Modes", "Code Review", "Coding", "Copilot Chat", "Custom Instructions", "Developer Tools", "Docker", "GitHub Copilot", "MCP", "MCP Server", "News", "Prompts", "Repository Management", "VS", "VS Code"]
tags_normalized: ["ai", "ai customization", "awesome copilot", "chat modes", "code review", "coding", "copilot chat", "custom instructions", "developer tools", "docker", "github copilot", "mcp", "mcp server", "news", "prompts", "repository management", "vs", "vs code"]
---

Justin Yoo demonstrates how developers can search, preview, and save custom GitHub Copilot chat modes, instructions, and prompts using the new Awesome Copilot MCP Server, streamlining Copilot customization workflows.<!--excerpt_end-->

# Announcing Awesome Copilot MCP Server: Customizing GitHub Copilot Like a Pro

The Awesome Copilot MCP Server is a new tool designed to help developers discover, customize, and manage GitHub Copilot chat modes, instructions, and prompts directly from Copilot Chat. This solution addresses the growing challenge of finding the right Copilot customization among the vast array of options in the community-driven Awesome Copilot repository.

## What Are Chat Modes, Instructions, and Prompts?

- **Chat Modes** set behavioral boundaries and context for how Copilot Chat interacts with your codebase (e.g., focusing only on front-end development).
- **Custom Instructions** establish rules and guidelines about *how* Copilot should perform certain tasks, such as enforcing coding practices or project requirements.
- **Prompts** provide reusable task directives, like code scaffolding or code review, specifying *what* should be accomplished.

Developers can create and share these customizations, improving AI assistance relevance within their specific projects and teams.

## Installing Awesome Copilot MCP Server

To get started:

1. **Prerequisite:** Ensure Docker Desktop is installed and running, as the MCP server operates in a container.
2. **Easy Install:** Click the provided link to install directly in Visual Studio Code or VS Code Insiders:
   - [Install in VS Code](https://aka.ms/awesome-copilot/mcp/vscode)
   - [Install in VS Code Insiders](https://aka.ms/awesome-copilot/mcp/vscode-insiders)
3. **Manual Configuration:** Alternatively, add this configuration to your MCP server settings:

   ```json
   {
     "servers": {
       "awesome-copilot": {
         "type": "stdio",
         "command": "docker",
         "args": ["run", "-i", "--rm", "ghcr.io/microsoft/mcp-dotnet-samples/awesome-copilot:latest"]
       }
     }
   }
   ```

For alternative installation methods, visit the [Awesome Copilot MCP GitHub repository](https://aka.ms/awesome-copilot/mcp).

## Using the Server to Enhance Copilot

Once installed, developers can interact with the MCP server inside Copilot Chat:

- **Search Customizations:** Use the `#search_instructions` tool or `/mcp.awesome-copilot.get_search_prompt` command to look up specific chat modes, instructions, or prompts by keyword (e.g., "python").
- **Compare and Save:** Results are displayed in a structured table, indicating if a customization already exists in your repo. Pick the desired customization by filename to save it to your project’s `.github` directories.
- **Load and Commit:** The server automatically loads and saves selected files, facilitating easy collaboration and codebase consistency.

### Example Workflow

1. Run `/mcp.awesome-copilot.get_search_prompt` in Copilot Chat.
2. Enter a relevant keyword.
3. Review the list of available chat modes, instructions, and prompts.
4. Select and save customizations directly into your repository.

## Additional Resources

- [Awesome Copilot](https://aka.ms/awesome-copilot)
- [Awesome Copilot MCP Server GitHub](https://aka.ms/awesome-copilot/mcp)
- [Let’s Learn MCP](https://aka.ms/letslearnmcp)
- [MCP Workshop in .NET](https://aka.ms/mcp-workshop/dotnet)
- [MCP Samples](https://github.com/modelcontextprotocol/csharp-sdk/tree/main/samples)

> For further customization examples and details about MCP in .NET, explore the provided resource links.

This post appeared first on "Microsoft Blog". [Read the entire article here](https://devblogs.microsoft.com/blog/announcing-awesome-copilot-mcp-server)
