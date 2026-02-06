---
external_url: https://code.visualstudio.com/blogs/2025/04/07/agentMode
title: Agent Mode Now Available to All VS Code Users with MCP Support
author: Isidor Nikolic
feed_name: Visual Studio Code Releases
date: 2025-04-07 00:00:00 +00:00
tags:
- Agent Mode
- AI Pair Programming
- AI Tools
- AI Workflows
- Autonomous Agents
- Chat UI
- Code Automation
- Code Editing
- Developer Productivity
- Language Server Protocol
- LLM Integration
- MCP
- Terminal Automation
- Tool Extensibility
- VS Code
- VS Code Extensions
- AI
- News
- .NET
section_names:
- ai
- dotnet
primary_section: ai
---
Isidor Nikolic from the VS Code team introduces Agent Mode, now accessible to all users. This update enables developers to automate coding tasks, customize workflows with MCP servers and extensions, and experience streamlined AI integration in Visual Studio Code.<!--excerpt_end-->

# Agent Mode Now Available to All VS Code Users with MCP Support

*Published April 7th, 2025 by Isidor Nikolic ([GitHub](https://github.com/isidorn))*

Agent Mode is now rolling out to all Visual Studio Code users, transforming the way developers interact with code by providing an autonomous AI assistant capable of executing multi-step tasks. This assistant can analyze your codebase, propose file edits, run terminal commands, respond to compile and lint errors, and continually iterate until the task is complete. Developers can now leverage Agent Mode to interact not only with built-in tools but also with tools contributed by MCP servers or VS Code extensions, greatly enhancing extensibility.

## Getting Started with Agent Mode

- Access Agent Mode by opening the Chat view, signing in to GitHub, and enabling the `chat.agent.enabled` setting if it’s not active by default.
- Select **Agent** from the Chat mode dropdown in VS Code. The latest updates may require you to reload VS Code.
- Soon, Agent Mode will be enabled by default for all users.

### Typical Agent Mode Scenarios

- Automating tasks that require multiple steps such as editing code, running commands, monitoring for errors, and self-correcting.
- Automatically determining file and context scope when you’re unsure of what changes are required.
- Integrating with external apps, databases, or cloud services through MCP servers and VS Code extensions.

### Unified Chat and Edit Experiences

- The Chat and Edits views are unified, simplifying workflow management.
- Features include: session history, chat windows, Working Set view improvements, and undo actions that revert changes up to the last tool call.
- Multiple agent sessions are supported in the same workspace, especially beneficial when not editing the same files concurrently.
- The agent can now create/edit notebooks and auto-approve tool calls.

## Extending Agent Mode: MCP Servers and Extensions

Agent Mode’s strength lies in its extensibility:

- **Built-in tools** allow searches, code edits, running commands, capturing errors, fetching data, and more.
- **MCP server tools** offer external integration capabilities using the Model Context Protocol.
- **VS Code extension tools** further expand scenario-specific options.

The [Language Server Protocol (LSP)](https://microsoft.github.io/language-server-protocol/) standardized language server integrations for editors, and now MCP ([Model Context Protocol](https://modelcontextprotocol.io/introduction)) is doing the same for providing context to large language models (LLMs). Agent Mode closes the loop, combining these innovations for developer empowerment.

### Customizing and Controlling Tools

- Manage tool invocation granularly in the UI; individually approve tool use per session, workspace, or all future invocations.
- Use the **Tools** icon in the chat input to add or remove tools and tailor your workflow.
- Use [Dev Containers](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers) to isolate agent operations for security.
- Review the [MCP server documentation](https://aka.ms/vscode-add-mcp) for configuration, integration, and importing servers from other MCP clients like Claude Desktop.
- Find official and community-contributed MCP servers on [GitHub](https://github.com/modelcontextprotocol/servers), or search for language model tool extensions within VS Code using the tag `@tag:language-model-tools`.
- Developers can create new MCP servers or contribute tools through VS Code extensions. Refer to [copilot extensibility docs](https://code.visualstudio.com/docs/copilot/copilot-extensibility-overview) for best practices.

## What’s Next

Upcoming improvements for Agent Mode include:

- Support for custom modes and toolsets
- Faster code application
- Expanded MCP support for prompts and the latest specification updates
- Optimized edit streaming
- Session checkpoints
- Ongoing performance and service quality improvements

Stay up to date by installing [VS Code Insiders](https://code.visualstudio.com/insiders/) and joining the feedback discussions on [GitHub](http://github.com/microsoft/vscode/issues/).

**For more information:** See the [official documentation](https://aka.ms/vscode-copilot-agent).

---

*Happy coding with Agent Mode—now with MCP and even more extensibility!*

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2025/04/07/agentMode)
