---
layout: post
title: "What's New in Visual Studio Code June 2025 (v1.102): Copilot Chat, MCP, and More"
author: Visual Studio Code Team
canonical_url: https://code.visualstudio.com/updates/v1_102
viewing_mode: external
feed_name: Visual Studio Code Releases
feed_url: https://code.visualstudio.com/feed.xml
date: 2025-07-09 17:00:00 +00:00
permalink: /github-copilot/news/Whats-New-in-Visual-Studio-Code-June-2025-v1102-Copilot-Chat-MCP-and-More
tags:
- Accessibility
- Agent Mode
- AI Tools
- Chat Mode
- Custom Instructions
- Extension Development
- GitHub Copilot Chat
- MCP Servers
- Open Source
- Pull Requests
- Pylance
- Python
- Release Notes
- Settings Sync
- Terminal Automation
- TypeScript
- VS Code
- VS Code 1.102
section_names:
- ai
- coding
- devops
- github-copilot
---
Visual Studio Code Team highlights the key features of the June 2025 (v1.102) release, including the open sourced GitHub Copilot Chat extension, integrated MCP server management, and advanced AI and coding workflows.<!--excerpt_end-->

# Visual Studio Code June 2025 Release Notes (v1.102)

_Authored by: Visual Studio Code Team_

## Key Highlights

### Open Source GitHub Copilot Chat Extension

- **Copilot Chat is now open source.** The extension's codebase is public, enabling developers to contribute, customize, and understand its AI integrations for VS Code.
- Deep dives into agent mode, inline chat, and MCP integration help advanced users and extension authors build custom AI developer workflows.

### Agent Mode and Coding Automation

- Delegate editor or terminal tasks to Copilot coding agent, which can now handle background operations, manage running tasks, and summarize task statuses.
- Automated approval of terminal commands using allow/deny lists for improved productivity and safety.
- Edit and resubmit previous chat requests with new experimental features.

### MCP Servers General Availability

- **Model Context Protocol (MCP) support is GA**: Easily discover, install, and manage MCP servers using new profile-based configuration. Servers are first-class citizens in VS Code profiles and sync seamlessly across devices.
- Integration with Dev Containers for seamless MCP setup in complex environments.
- Extensive server management UI for starting, stopping, configuration, and resource exploration.

### Enhanced Chat and AI Customization

- Support for custom chat modes, reusable instructions, and prompt import from community repositories (e.g., awesome-copilot).
- Improved editing support with completions, validation, and model-specific diagnostics in chat customization files.
- Generate tailored coding instructions automatically from your workspace for improved AI responses.

### Terminal and Task Improvements

- Tab-based terminal suggestions with multi-command support, enriched discoverability, and better sorting/prioritization.
- Enhanced Git Bash support, improved symlink handling in suggestions, and clearer UI around terminal command execution.
- ‘Snooze’ inline and next edit suggestions to help developers focus.
- Ability to rerun all running tasks and auto-reload `tasks.json`.

### Python and Extension Updates

- Python Environments extension now more deeply integrated and automatically deployed with Python extension rollouts.
- New experimental MCP tools in Pylance for enhanced environment and import analysis.
- GitHub sign-in flow improved for enhanced reliability, including loopback URL support.
- New extension authoring APIs, including ability to open files via `vscode.openFolder` command.

### Additional Features and Fixes

- Accessibility improvements, including editor-based ‘Keep All Edits’ and clearer chat error alerts.
- Code editing enhancements such as middle mouse scrolling, snooze suggestions, and accent color border support on Windows.
- Expanded contributor recognition and numerous bug fixes across the release (see linked issues and PRs for full list).

---

For further information and full details, visit:
[Official Release Notes](https://code.visualstudio.com/updates/v1_102)

## Table of Contents

- [Copilot Chat Goes Open Source](#copilot-chat-goes-open-source)
- [Agent Mode Automation](#agent-mode-automation)
- [MCP Server Enhancements](#mcp-server-enhancements)
- [Chat and Terminal Improvements](#chat-and-terminal-improvements)
- [Python & Extension Updates](#python--extension-updates)
- [Developer Experience and DevOps](#developer-experience-and-devops)
- [Accessibility and Other Improvements](#accessibility-and-other-improvements)
- [Community Contributions and Fixes](#community-contributions-and-fixes)

---

### Copilot Chat Goes Open Source

- The source code for the GitHub Copilot Chat extension is now public ([repo](https://github.com/microsoft/vscode-copilot-chat)), welcoming community collaboration.
- Key components (agent mode, inline chat, MCP integration) are documented and open for pull requests.
- Enhanced ability for developers to:
  - Contribute new features
  - Understand how chat modes and LLM integrations work
  - Import and share custom prompt/config files via community resources (e.g., [awesome-copilot](https://github.com/github/awesome-copilot)).

### Agent Mode Automation

- Delegate repetitive or background tasks directly to the coding agent in chat.
- Support for auto-approval of safe terminal commands, with configurable allow and deny lists.
- Improved awareness of VS Code tasks and terminals, with live progress tracking and summarization.
- Edit and re-submit prior chat context to quickly iterate or correct previous queries.

### MCP Server Enhancements

- **MCP servers (Model Context Protocol) support is now generally available.**
- Install, configure, and manage MCP servers as per workspace or profile, stored separately in `mcp.json`.
- [Developer Guide](https://code.visualstudio.com/api/extension-guides/ai/mcp) provided for building custom MCP servers.
- Enterprise controls now available for MCP policies via GitHub Copilot admin.
- Automatic migration tool detects and updates legacy settings.
- Direct support for Dev Containers (`devcontainer.json`).

### Chat and Terminal Improvements

- Define, edit, and import custom chat modes and instructions for greater coding context.
- Generate suggested instructions from your project's codebase using the agent.
- Terminal suggestion widget improved: supports multi-commands, new symlink icons, better ranking, and Git Bash compatibility.
- Easier access to chat request logs for troubleshooting and AI transparency.

### Python & Extension Updates

- Python Environments extension now bundled as an optional dependency; manage multiple virtualenvs per workspace.
- Pylance extension ships experimental MCP tools for enhanced introspection and environment handling.
- PyREPL compatibility fix for Python 3.13+.
- New APIs and changes for extension authors (file opening support, refined build tooling, new CI checks).

### Developer Experience and DevOps

- GitHub PR extension integrates directly with Copilot coding agent sessions.
- Revamped GitHub authentication UX using reliable loopback URLs.
- Enhanced team and DevOps workflows: Settings Sync integration improves porting custom MCP setups across teams and devices.
- CI build improvements (CSS minification, layer checking, regression testing tools like vscode-bisect).

### Accessibility and Other Improvements

- Editor actions for accepting all edits without leaving the context window.
- Accessibility signals for user-required actions in chat.
- Improved notifications for screen reader users during errors.
- Windows accent color support for editor borders.
- Numerous UI/UX tweaks across the release.

### Community Contributions and Fixes

- Dozens of external contributors improved features, fixed bugs, and enhanced documentation.
- Full list of fixes, pull requests, and notable community members included in the official release notes.

---

For in-depth explanations and troubleshooting, refer to the [official documentation](https://code.visualstudio.com/docs) and changelogs linked throughout.

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/updates/v1_102)
