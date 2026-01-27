---
external_url: https://devblogs.microsoft.com/visualstudio/agent-mode-is-now-generally-available-with-mcp-support/
title: Agent Mode Now Generally Available in Visual Studio with MCP Support
author: Rhea Patel, Filisha Shah, Allie Barry
feed_name: Microsoft DevBlog
date: 2025-06-17 18:24:08 +00:00
tags:
- .NET
- Agent Mode
- Agents Toolkit
- AI Assisted Development
- Automation
- Copilot
- Copilot Chat
- Gemini 2.5 Pro
- GPT 4.1
- MCP
- Prompt Sharing
- Tool Calling
- VS
section_names:
- ai
- coding
- github-copilot
primary_section: github-copilot
---
Authored by Rhea Patel, Filisha Shah, and Allie Barry, this piece covers the Visual Studio June update featuring general availability of Copilot agent mode with MCP integration, enabling sophisticated autonomous AI development support in Visual Studio.<!--excerpt_end-->

# Copilot Agent Mode Now Generally Available with MCP Support

*By Rhea Patel, Filisha Shah, and Allie Barry*

## Overview

The latest Visual Studio June update introduces the next evolution in AI-assisted development: **Copilot agent mode**. Now generally available, this feature transforms GitHub Copilot from a conversational assistant into a fully autonomous pair programmer, capable of executing multi-step development tasks from start to finish within Visual Studio.

## Key Features of Agent Mode

- **End-to-End Task Automation:** Agent mode can analyze your codebase, generate detailed plans, apply code changes, respond to build and lint errors, run command-line operations, and self-correct as needed, iterating through tasks until completion.
- **Active Autonomy:** Unlike Ask Mode, where developers guide Copilot interactively, Agent Mode executes entire goals independently, while still providing editable previews, undo options, and a live action feed for user oversight.
- **Multi-Context Understanding:** The more detailed the prompt, the more effective agent mode becomes. It works best when provided with clear goals and full contextual information.

### Example Use Cases

Agent mode is ideal for:

- Adding new features such as, “Add ‘buy now’ functionality to my product page.”
- Implementing robust patterns, e.g., “Add a retry mechanism to this API call with exponential backoff and a unit test.”
- Generating boilerplate code or scaffolding applications, such as, “Create a new Blazor web app that does X.”

## How to Access Agent Mode in Visual Studio

1. Open **Copilot Chat**.
2. Click the **Ask** button.
3. Switch to **Agent** mode.

[Video link: Introduction to Agent Mode](https://youtu.be/iqiR7uQJe7A)

## Tool Calling and Ecosystem Expansion

At the core of agent mode is **tool calling**:

- Agent mode selects and executes from a growing set of tools directly inside Visual Studio, accessible via a tool dropdown menu in Copilot Chat.
- The system is extensible; you can add custom tools to further expand its capabilities with the Model Context Protocol (MCP). [Learn about adding MCP tools](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2025/06/mcptools.png).

## Model Context Protocol (MCP) Integration

MCP offers a standardized, open-source protocol for connecting AI agents with external tools and services:

- **Native support in Visual Studio** through the `mcp.json` configuration file, also compatible with environments like `.vscode/mcp.json`.
- Agents can now access resources beyond code—such as GitHub repositories, CI/CD pipelines, telemetry/monitoring systems, and more, using MCP bridge servers.
- [Official MCP server repository](https://github.com/modelcontextprotocol/servers).

### Practical Examples of MCP Use

- With the GitHub MCP server, the agent can manage issues, review repo history, and perform searches programmatically.
- Integrating with Figma via MCP enables the agent to access design mockups within your workflow.
- Support for other external tools and systems is possible, making agent mode highly adaptable.

## Empowering Developers with AI

Agent mode is designed to improve real-world productivity:

- Developers remain in full control, with Copilot providing suggestions, previews, and the ability to revert changes.
- Enhanced autonomy empowers teams to move faster while maintaining code quality and oversight.
- Continuous feedback and logs are encouraged to further enhance agent mode’s evolution ([Share feedback](https://developercommunity.visualstudio.com/home)).

## Additional AI Features in the Visual Studio June Release

Alongside agent mode, several new AI-related capabilities debut in this update:

- **Prompt File Sharing:** Teams can now create and use reusable markdown prompt files directly in their repositories, streamlining prompt reuse within Copilot Chat.
- **Access to New AI Models:** Gemini 2.5 Pro and GPT-4.1 models are now supported, offering advanced reasoning and code generation capabilities.
- **Output Window Referencing:** Integrate the Output Window context to facilitate more effective runtime troubleshooting.
- **Copilot Usage Monitoring:** Directly track your Copilot usage within Visual Studio.
- **Agents Toolkit 17.14 GA:** General availability for this toolkit brings improvements for building Microsoft 365 apps and intelligent agents.

---

For further reading, see the official [Visual Studio Blog post](https://devblogs.microsoft.com/visualstudio/agent-mode-is-now-generally-available-with-mcp-support/). Learn more about [MCP servers](https://learn.microsoft.com/en-us/visualstudio/ide/mcp-servers?view=vs-2022) and compatible developer workflows.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/agent-mode-is-now-generally-available-with-mcp-support/)
