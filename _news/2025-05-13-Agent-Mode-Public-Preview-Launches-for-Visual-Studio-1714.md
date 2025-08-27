---
layout: "post"
title: "Agent Mode Public Preview Launches for Visual Studio 17.14"
description: "Visual Studio 17.14 introduces Agent mode in public preview, allowing tasks to be defined in natural language for Copilot to plan, edit, invoke tools, and iterate until complete. The mode integrates autonomous development flows, MCP server support, and now ships on a monthly Copilot update schedule."
author: "Katie Savage, Aaron Yim"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/visualstudio/agent-mode-has-arrived-in-preview-for-visual-studio/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/visualstudio/tag/github-copilot/feed/"
date: 2025-05-13 17:26:51 +00:00
permalink: "/2025-05-13-Agent-Mode-Public-Preview-Launches-for-Visual-Studio-1714.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["Agent Mode", "AI", "AI Agents", "Azure Storage", "Code Refactoring", "Coding", "Copilot", "Copilot Chat", "Cosmos DB", "Documentation Generation", "GitHub Copilot", "MCP", "MCP Servers", "News", "Productivity", "Testing", "Visual Studio 17.14", "VS"]
tags_normalized: ["agent mode", "ai", "ai agents", "azure storage", "code refactoring", "coding", "copilot", "copilot chat", "cosmos db", "documentation generation", "github copilot", "mcp", "mcp servers", "news", "productivity", "testing", "visual studio 17dot14", "vs"]
---

Katie Savage and Aaron Yim introduce Agent mode in Visual Studio 17.14, which empowers developers to define and complete complex tasks in natural language using GitHub Copilot's autonomous planning and execution capabilities.<!--excerpt_end-->

# Agent Mode Arrives in Preview for Visual Studio

**Authors:** Katie Savage, Aaron Yim

## Overview

Agent mode is now publicly available in preview for all users of Visual Studio 17.14. This feature enables developers to define tasks using natural language prompts, leveraging Copilot to autonomously plan, edit code, invoke tools, and iteratively resolve any issues until the prompt is fully addressed.

Unlike Copilot Chat or Edits, Agent mode persists through multiple suggestions or file edits, working iteratively until task completion.

---

## What Is Agent Mode?

Agent mode enhances the development experience by:

- **Autonomously determining context and relevant files** to edit.
- **Suggesting terminal commands** for user approval and execution.
- **Iterating** through the task, continually checking for errors, running builds or tests, and validating results.
- **Invoking trusted tools via MCP (Model Context Protocol) servers**, including linters, test runners, and static analyzers, directly within your environment.

Agent mode is being positioned as the new, more powerful default way to apply code changes in Visual Studio, advancing the Edits functionality in Copilot Chat.

---

## Enabling Agent Mode

Agent mode is off by default during its preview phase. To enable it:

1. Use Visual Studio 17.14 GA or later.
2. Open Feature Search (`Ctrl + Q`).
3. Search for “agent.”
4. Enable: **Copilot Chat: Agent Enabled**.
5. In the Copilot Chat window, switch to “Agent” mode and enter a high-level prompt. The Copilot Chat window can be accessed via the Copilot badge in the IDE’s upper-right corner.

---

## Scenarios and Use Cases

Agent mode can be applied to a variety of development scenarios:

- Creating apps from scratch
- Refactoring code across multiple files
- Writing and running tests
- Generating documentation
- Integrating new libraries
- Implementing complex or terminal-heavy tasks
- Answering questions about complex codebases

### Key Features

| Aspect           | Description                                                                                 |
|------------------|--------------------------------------------------------------------------------------------|
| Edit Scope       | Searches project to autonomously determine the relevant context/files to edit               |
| Task Complexity  | Handles intricate tasks, including tool invocations and terminal commands                   |
| Duration         | May take longer, as it involves multi-step processes and planning                          |
| Self-iteration   | Evaluates and iterates on output, resolving intermediate issues multiple times              |
| Multiple Requests| Can generate several backend requests from a single user prompt                             |

### Example Prompts

- “Add error handling to this API”
- “Convert this project to use environment variables”
- “Write tests for this class and fix anything that fails”
- “Add structured logging with Serilog”
- “Replace HttpClient with IHttpClientFactory”

Providing custom instructions improves Copilot’s responses. The [documentation](https://learn.microsoft.com/visualstudio/ide/copilot-chat-context?view=vs-2022#enable-custom-instructions) guides on how to set these up.

---

## MCP Servers Integration

This release deepens Agent mode’s capabilities by supporting Model Context Protocol (MCP) servers. MCP servers offer enriched context for Copilot, connecting AI applications to tools, data, and resources without the need for custom connections per source.

### Capabilities via MCP Servers

- **Interact with cloud environments:** Official Azure MCP server enables listing, querying, and interaction with Azure Storage, Cosmos DB, Azure CLI, etc.
- **Query and manage databases:** Execute Cosmos DB queries, manage Azure Storage containers/blobs, and fetch metadata.
- **Version control management:** Use GitHub API for bulk issues, batch file updates, code and issue searches across repositories, etc.

You can even build your own MCP servers with supported SDKs (including C#).

---

## Copilot Monthly Release Schedule

Visual Studio is transitioning to a monthly release cadence for Copilot features, ensuring prompt access to the latest updates. Users are encouraged to keep Visual Studio updated to benefit from ongoing improvements.

---

## Resources

- [Visual Studio Downloads](https://visualstudio.microsoft.com/vs/downloads/)
- [Visual Studio Hub](https://visualstudio.microsoft.com/hub/): Central resource for release notes, videos, and community updates.
- [Developer Community](https://developercommunity.visualstudio.com/VisualStudio): Feedback and issue reporting platform.

---

## Closing and Feedback

The Visual Studio team values community feedback as it refines Agent mode and Copilot integration. All users are encouraged to share ideas and issues via the Developer Community.

> Plan, build, test, and fix — all from one prompt. Agent mode aims to make development in Visual Studio more autonomous, productive, and context-aware.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/agent-mode-has-arrived-in-preview-for-visual-studio/)
