---
layout: post
title: Visual Studio Code March 2025 (v1.99) Release Notes
author: Visual Studio Code Team
canonical_url: https://code.visualstudio.com/updates/v1_99
viewing_mode: external
feed_name: Visual Studio Code Releases
feed_url: https://code.visualstudio.com/feed.xml
date: 2025-04-03 17:00:00 +00:00
permalink: /ai/news/Visual-Studio-Code-March-2025-v199-Release-Notes
tags:
- Accessibility
- Agent Mode
- AI Coding
- Chat
- Copilot
- Extension API
- Jupyter Notebook
- MCP
- MCP Servers
- Next Edit Suggestions
- Pylance
- Remote Development
- Source Control
- Terminal IntelliSense
- VS Code
section_names:
- ai
- coding
---
Visual Studio Code Team details the March 2025 (v1.99) release, showcasing features like agent mode with AI-assisted coding, unified chat, advanced notebook tools, and major improvements to developer productivity.<!--excerpt_end-->

# Visual Studio Code March 2025 (version 1.99) Release Notes

## Overview

The March 2025 update for Visual Studio Code delivers several significant enhancements, focused on AI-powered coding assistance, improved chat interfaces, richer terminal features, and wide-ranging updates to developer and extension tooling. Below are the key highlights from this release.

---

## Key Highlights

### Agent Mode General Availability

- **Agent Mode** is now stable, letting developers enable autonomous, AI-assisted coding workflows.
- Enable via the `chat.agent.enabled` setting, with rollout to all users in progress.
- Model Context Protocol (MCP) server tools let agent mode interact with external apps, databases, and more for dynamic, context-aware coding.

### Model Context Protocol (MCP) Server Support

- Configure MCP servers in workspace settings or `mcp.json`, using environment variables for secure configuration.
- Setup via command palette, with AI-assisted setups supported for servers published to Docker, npm, or PyPI.
- MCP server integration enables chat mode to run developer tools and fetch web data during code assistance sessions.

### AI Coding Tools & Chat Experience

- Unified Chat View merges 'Ask', 'Edit', and 'Agent' coding conversations into a single UI.
- Support for switching seamlessly between brainstorming, code edits, and agentic workflows.
- Enhanced chat with support for user-created prompts, prompt file autocompletion, and advanced search (semantic search with #searchResults tool).
- 'Bring Your Own Key' allows developers to provide API keys for models from Azure, OpenAI, Anthropic, Gemini, and others.
- AI notebook editing is improved with support for notebook creation, cell-wise editing, and reviewing/undoing AI edits.

### Developer Productivity Enhancements

- **Next Edit Suggestions** (NES) now generally available for code edits and suggestions.
- Inline suggestion syntax highlighting is enabled by default, aiding faster code reviews.
- Tree-sitter based highlighting for CSS and regex in TypeScript (preview).
- Source control enhancements: improved git reference picker and repository status bar info.
- Terminal improvements: richer IntelliSense with subcommand suggestions, auto-refresh for new global CLI tools, and improved shell integration.
- VS Code extensions can now provide language model tools usable in agent mode and get better APIs for attaching images and monitoring task matchers.

### Notebooks and Data Science

- AI-powered editing for Jupyter notebooks is now stable; developers can edit code and markdown cells, insert cells, and manage outputs with AI assistance.
- Output and errors from notebook cells can be directly referenced in chat, improving context sharing and debugging.

### Platform, Enterprise, and Accessibility

- Enterprise device management support for macOS (alongside Windows).
- Accessibility improvements in agent mode and chat, including tool invocation notifications for screen readers and auditory signals for AI edits.

---

## Sample Configurations and Commands

- **Enable agent mode:**
  - `chat.agent.enabled`
- **Add MCP server:**
  - Call "MCP: Add Server" from command palette.
- **Notebook AI editing:**
  - Use chat mode `/newNotebook` or enable edit mode with `chat.edits2.enabled`.
- **Terminal improvements:**
  - Try new IntelliSense for the `code` CLI and other shell integrations.

---

## Further Reading

- [Full Release Notes](https://code.visualstudio.com/updates/v1_99)
- [Agent Mode Documentation](https://code.visualstudio.com/docs/copilot/chat/chat-agent-mode)
- [MCP Server Documentation](https://code.visualstudio.com/docs/copilot/chat/mcp-servers)
- [Unified Chat Experience](https://code.visualstudio.com/docs/copilot/chat/unified-chat)
- [Remote Workspace Indexing](https://code.visualstudio.com/docs/copilot/reference/workspace-context)

---

## About the Author

This update is brought to you by the Visual Studio Code Team.

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/updates/v1_99)
