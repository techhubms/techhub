---
layout: "post"
title: "What's New in Visual Studio Code July 2025 (v1.103): GPT-5, Coding Agents, and DevOps Improvements"
description: "This official release note details the July 2025 update (version 1.103) of Visual Studio Code from Microsoft, highlighting major new features such as integrated GPT-5 support for all paid GitHub Copilot plans, improvements to agent mode, experimental enhancements for chat-based coding, enhanced Azure DevOps repo indexing, UI/UX upgrades, terminal and notebook features, contributor acknowledgments, and more."
author: "Visual Studio Code Team"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://code.visualstudio.com/updates/v1_103"
viewing_mode: "external"
feed_name: "Visual Studio Code Releases"
feed_url: "https://code.visualstudio.com/feed.xml"
date: 2025-08-07 17:00:00 +00:00
permalink: "/2025-08-07-Whats-New-in-Visual-Studio-Code-July-2025-v1103-GPT-5-Coding-Agents-and-DevOps-Improvements.html"
categories: ["AI", "Azure", "Coding", "DevOps", "GitHub Copilot"]
tags: ["Accessibility", "Agent Mode", "AI", "AI Chat", "Azure", "Azure DevOps", "Chat Checkpoints", "Coding", "Coding Agent", "DevOps", "Editor Updates", "Extension Authoring", "GitHub Copilot", "GPT 5", "MCP Server", "Model Management", "News", "Notebook", "Pull Requests", "Python", "Source Control", "Task Automation", "Terminal Tools", "TypeScript 5.9", "VS Code"]
tags_normalized: ["accessibility", "agent mode", "ai", "ai chat", "azure", "azure devops", "chat checkpoints", "coding", "coding agent", "devops", "editor updates", "extension authoring", "github copilot", "gpt 5", "mcp server", "model management", "news", "notebook", "pull requests", "python", "source control", "task automation", "terminal tools", "typescript 5dot9", "vs code"]
---

The Visual Studio Code Team presents the July 2025 (v1.103) update, packed with new features like GPT-5 for Copilot, coding agent improvements, Azure DevOps repo indexing, and numerous productivity enhancements.<!--excerpt_end-->

# Visual Studio Code July 2025 (v1.103) Release Notes

*Release date: August 7, 2025*

## Overview

This release brings a host of new features and updates to Visual Studio Code, including extensive enhancements to AI features, coding experience, and DevOps connectivity. GPT-5 is now available for all paid GitHub Copilot plans. Improvements also cover agent mode, tool management, terminal automation, Azure DevOps indexing, and more—all making developer workflows smarter and more productive.

## Key Highlights

### AI & GitHub Copilot

- **GPT-5**: Now available to all paid Copilot users, offering more advanced coding and chat capabilities than ever before. Also includes GPT-5 mini, a faster, more efficient variant.
- **Chat Improvements**: Enhanced prompt quality, new chat checkpoints for conversation state management, and robust tools for agent-driven task planning.
- **Model Management**: Updated chat provider API enables users to customize available language models and empowers extension ecosystem growth.
- **Experimental Features**: Project scaffolding using Context7, experimental todo/task lists in chat, and inline chat for notebooks and the terminal.

### Coding and Productivity

- **Git Worktree Support**: Check out multiple branches simultaneously with full worktree management.
- **MCP Servers**: Updates include server autostart, authentication improvements via client credentials, server trust dialogues, and adherence to new MCP specs.
- **Terminal and Task Enhancements**: More reliable task running, output polling for longer tasks, advanced terminal auto-approval, and voice dictation support.
- **Chat Session Management**: Dedicated views for managing coding agent sessions and improved integrations with pull requests.
- **Language Updates**: Shell integration support for Python 3.13+, bundled TypeScript 5.9, and UI improvements for JavaScript/TypeScript hover tooltips.

### DevOps and Azure

- **Azure DevOps Indexing**: The codebase tool now supports remote indexes for Azure DevOps repos, bringing near-instant code search for large workspaces.
- **Source Control Improvements**: Enhanced repositories view, improved worktree/submodule management, and pull request integration with the chat experience.

### Extension Authoring

- New activation events for terminals and shell integration.
- APIs for custom chat output rendering (e.g., rich diagrams) and Chat Session Provider integration.
- Improved secret storage management for extensions.

### Accessibility and UX

- Better accessibility for chat and file edits, ARIA notifications for UI changes, and automated accessibility tests using Playwright.
- Improved settings editor search (AI-assisted), customizable side bar visibility, more intuitive context menus, and math support in chat.

### Engineering and Infrastructure

- Upgrade to Electron 37 with Chromium 138 and Node.js 22.17.0.
- Key update for packages.microsoft.com, reducing errors for Linux installs.

### Contributions

- Detailed acknowledgments for contributors to VS Code, its extensions, and related projects.

For more information and links to full details, visit the [VS Code release notes online](https://code.visualstudio.com/updates/v1_103).

---

*Thank you to all community and Microsoft contributors who made this release possible!*

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/updates/v1_103)
