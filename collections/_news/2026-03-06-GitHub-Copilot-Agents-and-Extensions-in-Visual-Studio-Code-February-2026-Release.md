---
layout: "post"
title: "GitHub Copilot Agents and Extensions in Visual Studio Code February 2026 Release"
description: "This article covers the February 2026 release of Visual Studio Code v1.110, which introduces significant updates to GitHub Copilot, including practical agent workflows for complex tasks, agent lifecycle hooks, enhanced chat features, agent plugins, new CLI integration, and improved context handling across Copilot features. Additional Visual Studio Code productivity improvements are also highlighted."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-03-06-github-copilot-in-visual-studio-code-v1-110-february-release"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-03-06 19:03:06 +00:00
permalink: "/2026-03-06-GitHub-Copilot-Agents-and-Extensions-in-Visual-Studio-Code-February-2026-Release.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["Agent Plugins", "AI", "Auto Approve", "Automation", "Code Review", "Coding", "Coding Assistants", "Context Management", "Copilot", "Copilot CLI", "Developer Tools", "GitHub Copilot", "News", "Productivity", "Programming Agents", "Prompt Engineering", "Slash Commands", "VS Code", "VS Code Extensions"]
tags_normalized: ["agent plugins", "ai", "auto approve", "automation", "code review", "coding", "coding assistants", "context management", "copilot", "copilot cli", "developer tools", "github copilot", "news", "productivity", "programming agents", "prompt engineering", "slash commands", "vs code", "vs code extensions"]
---

Allison details the latest Visual Studio Code update, emphasizing robust GitHub Copilot agent automation, extensibility, CLI integration, and advanced context management for developers.<!--excerpt_end-->

# GitHub Copilot Agents and Extensions in Visual Studio Code February 2026 Release

**Author:** Allison

The February 2026 Visual Studio Code v1.110 update delivers major enhancements for developers leveraging GitHub Copilot. This release focuses on practical agent usage, improved extensibility, and advanced session management to tackle more complex or long-running tasks in your editor.

## Key Features and Improvements

### Program Your Agents

- **Agent Lifecycle Hooks**: Automate workflows and enforce coding policies by triggering custom code at agent lifecycle events (for example, to auto-lint or block risky commands).
- **Conversation Forking**: Branch off from any checkpoint, allowing you to experiment with alternative code paths without losing your original context.
- **Auto-Approve in Chat**: Use `/autoApprove` or `/yolo` straight from chat, paired with terminal sandboxing to control agent command execution.
- **Live Steering**: Send new messages to an agent as it works, dynamically redirecting its path during task execution.

### Extend Your Agents

- **Agent Plugins**: Install bundled skills, tools, hooks, and MCP (Multi-Channel Protocol) servers directly from the Extensions view. Currently experimental, these plugins extend agent capabilities.
- **Slash Command Skills**: Launch agent skills in chat, including those added via your own custom extensions.
- **Agentic Browser Tools**: Enable agents to interact with the integrated browser—navigating, clicking, making screenshots, and verifying code changes (experimental).
- **Create from Chat**: Utilize `/create-*` commands in chat to generate reusable prompts, skills, agents, and hooks.

### Enhanced Copilot CLI Integration

- Copilot CLI is now built into VS Code, with features like diff tabs, trusted folder sync, and the ability to send code snippets via right-click.

### Context and Memory Management

- **Shared Agent Memory**: Copilot now shares and stores context across its coding agent, CLI, and code review, so knowledge is accumulated over time.
- **Plan Memory**: Agent plans persist through conversation turns and compaction to build on prior results.
- **Explore Subagent**: A built-in subagent helps with parallel codebase research, speeding up code analysis for agent planning.
- **Manual Context Compaction**: Trigger compaction and specify which parts to retain using slash commands.
- **Efficient Handling of Large Outputs**: Large outputs are written to disk, ensuring vital details aren't lost.
- **Predictive Edit Suggestions**: Copilot can now suggest edits across your entire file, not just nearby code.

### Additional VS Code Productivity Updates

- Support for Kitty graphics protocol enhances terminal image quality.
- The model picker is now more intuitive, with search, sections, and rich hover data.
- Accessibility improvements offer better screen reader support, navigation, and notifications.
- AI commits attribution clearly marks AI-generated code.

Developers can join further discussion on these updates in the [GitHub Community](https://github.com/orgs/community/discussions/categories/announcements).

---

Stay current with VS Code and Copilot to take advantage of these new developer productivity and AI-powered code collaboration features.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-03-06-github-copilot-in-visual-studio-code-v1-110-february-release)
