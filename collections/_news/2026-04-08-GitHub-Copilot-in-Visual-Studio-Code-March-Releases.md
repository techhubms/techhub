---
date: 2026-04-08 13:44:52 +00:00
primary_section: github-copilot
author: Allison
title: GitHub Copilot in Visual Studio Code, March Releases
external_url: https://github.blog/changelog/2026-04-08-github-copilot-in-visual-studio-code-march-releases
section_names:
- ai
- github-copilot
feed_name: The GitHub Blog
tags:
- Agent Permissions
- Agent Sessions
- AI
- Autopilot
- Browser Debugging
- Chat Customizations
- Chrome Debugging
- Claude Sonnet
- Codebase Tool
- Copilot
- Copilot Chat
- Copilot CLI
- Edge Debugging
- Editor Browser Debug Type
- GitHub Copilot
- GPT
- MCP
- MCP Servers
- Model Picker
- Monorepo
- News
- Plugins
- Reasoning Models
- Sandboxing
- Semantic Search
- Subagents
- Troubleshooting
- TypeScript 6.0
- VS Code
- YAML Frontmatter
---

Allison summarizes the March/early April 2026 GitHub Copilot updates in VS Code (v1.111–v1.115), focusing on more autonomous agent sessions (Autopilot), expanded debugging and multimodal chat capabilities, and improved customization and troubleshooting tools.<!--excerpt_end-->

## GitHub Copilot in Visual Studio Code (v1.111–v1.115): March Releases

VS Code has moved to weekly stable releases. This changelog covers VS Code releases **v1.111 through v1.115** shipped throughout March and early April 2026.

Highlights include:

- **Autopilot** for fully autonomous agent sessions (public preview)
- **Integrated browser debugging**
- **Image and video support** in chat
- A **unified editor** for managing chat customizations

## Control how agents run

### Agent permissions
You can choose a permission level per session to control which actions or decisions require manual intervention. The options are:

- **Default**
- **Bypass Approvals**
- **Autopilot**

This applies to both **local** and **Copilot CLI** sessions.

### Autopilot (preview)
In **Autopilot** mode:

- Agents approve their own actions
- Agents automatically retry on errors
- Agents work autonomously until the task completes
- No manual approvals are required

### Configurable thinking effort
You can control how deeply reasoning models (for example **Claude Sonnet 4.6** and **GPT-5.4**) “think” before responding.

- Set this directly from the **model picker**
- The preference persists across conversations

## Expand what agents can do

### Debug with the integrated browser
VS Code adds integrated browser debugging so you can:

- Set breakpoints
- Step through code
- Inspect variables

This works without leaving VS Code.

A new debug type is introduced:

- `editor-browser`

It works with most existing `chrome` and `msedge` debug configurations.

### Nested subagents
Subagents can now invoke other subagents via:

- `chat.subagents.allowInvocationsFromSubagents`

This is intended to enable more complex multi-step workflows and better task decomposition.

### Work with images and video in chat
Chat now supports:

- Attaching **screenshots** and **videos** to chat messages
- Agents returning **images** or **recordings** of changes

You can review returned media in a carousel with:

- Zoom
- Pan
- Thumbnail navigation

### CLI and Claude agent improvements
- **MCP servers** configured in VS Code now work in:
  - **Copilot CLI**
  - **Claude agent sessions**
- You can **fork any session** to explore alternative approaches while keeping the original conversation intact

## Simplify customization and debugging

### Chat customizations editor (preview)
A unified interface lets you create and manage:

- Instructions
- Custom agents
- Skills
- Plugins

You can also browse **MCP** and **plugin marketplaces** directly from the editor.

### Faster workspace search
The `#codebase` tool now:

- Runs purely **semantic searches**
- Uses a single **auto-managed index**
- Removes the “local vs. remote indexing” split

### Sandbox MCP servers
You can run local MCP servers in a restricted sandbox to limit:

- File access
- Network access

Supported on **macOS** and **Linux**.

### Diagnose with `/troubleshoot`
A new `/troubleshoot` chat command can analyze agent debug logs to help you understand:

- Unexpected behavior
- Why instructions were ignored
- Why responses were slow

This includes analysis of **past sessions**.

### Monorepo customizations
VS Code can now discover instructions, agents, skills, and hooks from parent folders up to the repository root, to make it easier to share team-wide configurations across packages.

### Agent-scoped hooks (preview)
You can attach pre- and post-processing logic to specific custom agents without affecting other chat interactions:

- Enable via `chat.useCustomAgentHooks`
- Define hooks in the YAML frontmatter of `.agent.md` files

## Related VS Code productivity updates

- New default **“VS Code Light”** and **“VS Code Dark”** themes
- **TypeScript 6.0** support (including fixes ahead of the TypeScript 7.0 native rewrite)
- Self-signed certificate support in the integrated browser for local HTTPS development
- Improved browser tab management with **Quick Open**, **Close All**, and a title bar shortcut
- Automatic symbol references when pasting code symbols into chat
- **Copy Final Response** command to copy just the agent’s final answer, skipping thinking steps and tool calls

## Links

- VS Code release notes: https://code.visualstudio.com/updates/v1_115
- GitHub Community announcements discussion: https://github.com/orgs/community/discussions/categories/announcements


[Read the entire article](https://github.blog/changelog/2026-04-08-github-copilot-in-visual-studio-code-march-releases)

