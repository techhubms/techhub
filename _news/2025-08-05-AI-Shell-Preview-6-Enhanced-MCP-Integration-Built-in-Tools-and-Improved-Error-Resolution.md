---
layout: "post"
title: "AI Shell Preview 6: Enhanced MCP Integration, Built-in Tools & Improved Error Resolution"
description: "This post details the Preview 6 release of AI Shell, introducing MCP client integration, enhanced error resolution, new built-in tools, and ways to streamline AI-powered workflows within PowerShell. The update aims to enhance usability, automation, and developer productivity in shell environments."
author: "Steven Bucher"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/powershell/preview-6-ai-shell/"
viewing_mode: "external"
feed_name: "Microsoft PowerShell Blog"
feed_url: "https://devblogs.microsoft.com/powershell/feed/"
date: 2025-08-05 18:40:38 +00:00
permalink: "/2025-08-05-AI-Shell-Preview-6-Enhanced-MCP-Integration-Built-in-Tools-and-Improved-Error-Resolution.html"
categories: ["AI", "Coding"]
tags: ["AI", "AI Agent", "AI Shell", "Automation", "Azure OpenAI", "Coding", "Command Line", "Copilot in Azure", "Error Handling", "MCP", "News", "OpenAI", "PowerShell", "Resolve Error", "Terminal Tools"]
tags_normalized: ["ai", "ai agent", "ai shell", "automation", "azure openai", "coding", "command line", "copilot in azure", "error handling", "mcp", "news", "openai", "powershell", "resolve error", "terminal tools"]
---

Author Steven Bucher shares the latest on AI Shell Preview 6, highlighting richer MCP integration, built-in tools for PowerShell, and improved error resolution features. Learn how these updates foster a more productive, AI-enhanced shell environment.<!--excerpt_end-->

# AI Shell Preview 6 Release!

*By Steven Bucher*

The AI Shell team is excited to announce the release of Preview 6, focusing on improved user experiences, robust error handling, tighter integration with Model Context Protocol (MCP) tools, and more automation capabilities inside your PowerShell workflows.

## What's New at a Glance

- **MCP client integration:** AI Shell can now act as an MCP client, connecting to a variety of MCP servers for extended functionality.
- **Built-in tools:** A suite of integrated commands enhances automation and enables richer context-awareness for the AI agent.
- **Resolve-Error improvements:** The command is now smarter in identifying and resolving previous command errors.
- **Aliases and workflows:** Stay productive within your terminal using intuitive shortcuts for AI Shell commands.

---

## MCP Integration

AI Shell now works as an MCP client. This allows you to add any MCP server to boost AI Shell’s capabilities with server-specific tools and broader data access.

**How to Add MCP Servers:**

- Create an `mcp.json` file in your `$HOME\.aish\` folder.
- Example configuration for two MCP servers ("everything" and "filesystem"):

  ```json
  {
    "servers": {
      "everything":{
        "type":"stdio",
        "command":"npx",
        "args":["-y", "@modelcontextprotocol/server-everything"]
      },
      "filesystem": {
        "type": "stdio",
        "command": "npx",
        "args": [ "-y", "@modelcontextprotocol/server-filesystem", "C:/Users/username/" ]
      }
    }
  }
  ```

- For remote servers, set `type` to `https`.
- View server registration in the AI Shell UI and verify with the `/mcp` command.
- `/mcp` lists all MCP Servers and their available tools.

**Requirements:**

- Node.js or `uv` should be installed for servers requiring them.

**Standalone Experience:**

- Use servers like [`@simonb97/server-win-cli`](https://github.com/SimonB97/win-cli-mcp-server?tab=readme-ov-file) to run PowerShell, CMD, Git Bash, or other shell commands.
- Define permitted commands and operations.

**Note:** Community MCP servers are not officially maintained by Microsoft; evaluate them before use. Additional servers can be found at:

- [DesktopCommander](https://github.com/wonderwhy-er/DesktopCommanderMCP)
- [Other community MCP Servers](https://mcpservers.org/)

---

## Built-in Tools for AI Shell

A new lineup of built-in tools is now available to the AI agent, improving context-awareness and enabling new automations. These complement existing MCP Server tools, but are exclusive to the base AI Shell environment.

| Tool Name                | Description                                                                                                         |
|-------------------------|---------------------------------------------------------------------------------------------------------------------|
| get_working_directory   | Returns the current working directory, provider name, and path of the PowerShell session.                            |
| get_command_history     | Fetches up to 5 of the latest commands executed in the session.                                                      |
| get_terminal_content    | Retrieves all terminal output currently displayed.                                                                   |
| get_environment_variables| Lists environment variables (with sensitive values redacted).                                                       |
| copy_text_to_clipboard  | Copies provided text/code to the system clipboard.                                                                  |
| post_code_to_terminal   | Inserts code into the prompt for user review before execution.                                                      |
| run_command_in_terminal | Executes shell commands in a persistent session, retaining context across commands.                                 |
| get_command_output      | Retrieves output from commands launched via `run_command_in_terminal`.                                              |

*Usage Notes:* These tools work with a connected PowerShell session, providing detailed context for AI-driven interactions.

**Demos:**

- Run shell commands via AI (`run_command_in_terminal` tool)
- Use `get_terminal_content` for richer context to AI
- Enhance agent responsiveness to session state changes

---

## Resolve-Error Command Improvements

The `Resolve-Error` command is now more intelligent:

- Identifies which command to troubleshoot based on error state and command history.
- Smart detection based on `$LastErrorCode`, `$?`, and analysis of terminal content.
- Reduces need for immediate AI intervention after errors; now, AI can effectively help resolve lingering or earlier issues.

---

## Staying in Your Shell: Commands and Aliases

- **Invoke-AIShell:** Main entry point, now with the `askai` alias.
- **Resolve-Error:** Enhanced troubleshooting, now with the `fixit` alias.

This setup enables continued workflow within PowerShell without switching context, streamlining access to AI-powered assistance.

| Command Name         | Alias   |
|---------------------|---------|
| Invoke-AIShell      | askai   |
| Resolve-Error       | fixit   |

---

## Conclusion

This release is designed to empower users through better automation, more flexible error resolution, and contextually-aware AI assistance directly in PowerShell. For feedback and feature requests, please submit issues at [AI Shell GitHub repository](https://github.com/PowerShell/AIShell).

Thank you for your continued input and support!

*AI Shell Team*

---

**Authors:**
Steven Bucher & Dongbo Wang

This post appeared first on "Microsoft PowerShell Blog". [Read the entire article here](https://devblogs.microsoft.com/powershell/preview-6-ai-shell/)
