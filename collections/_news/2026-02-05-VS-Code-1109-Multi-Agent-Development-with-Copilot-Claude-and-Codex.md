---
layout: "post"
title: "VS Code 1.109: Multi-Agent Development with Copilot, Claude, and Codex"
description: "The January 2026 release of VS Code marks a major leap for multi-agent development, enabling developers to run Claude and Codex agents alongside GitHub Copilot within a unified workspace. This update introduces enhanced agent session management, parallel subagents, open standards like MCP Apps and Agent Skills, and greater extensibility for AI-driven workflows directly in VS Code."
author: "VS Code Team"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://code.visualstudio.com/blogs/2026/02/05/multi-agent-development"
viewing_mode: "external"
feed_name: "Visual Studio Code Releases"
feed_url: "https://code.visualstudio.com/feed.xml"
date: 2026-02-05 00:00:00 +00:00
permalink: "/2026-02-05-VS-Code-1109-Multi-Agent-Development-with-Copilot-Claude-and-Codex.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["Agent Sessions", "Agent Skills", "AI", "AI Agents", "AI Integration", "AI Workflows", "Anthropic Claude", "Blog", "Claude Agent", "Codex Agent", "Coding", "GitHub Copilot", "MCP Apps", "Multi Agent Development", "News", "OpenAI Codex", "Software Extensions", "Subagents", "VS Code"]
tags_normalized: ["agent sessions", "agent skills", "ai", "ai agents", "ai integration", "ai workflows", "anthropic claude", "blog", "claude agent", "codex agent", "coding", "github copilot", "mcp apps", "multi agent development", "news", "openai codex", "software extensions", "subagents", "vs code"]
---

VS Code Team introduces a powerful multi-agent development experience in version 1.109, letting developers run GitHub Copilot, Claude, and Codex agents together. Discover new tools and open standards that shape collaborative AI workflows.<!--excerpt_end-->

# VS Code 1.109: Multi-Agent Development with Copilot, Claude, and Codex

**Author:** VS Code Team  
**Release Date:** February 5, 2026

---

## Overview

Version 1.109 of Visual Studio Code transforms the coding landscape by making it the unified hub for AI agent-powered development. With the new release, developers can seamlessly run multiple types of agents—including Claude, Codex, and GitHub Copilot—in a single workspace, streamlining coding, research, and review tasks.

---

## Unified Agent Sessions

- **Agent Sessions View:** Manage your local, cloud, and background agents in a single, centralized view. Easily switch between sessions, track running tasks, and delegate as needed.
- **Flexible Agent Management:** Start an agent locally for interactive support or invoke a cloud agent for asynchronous, longer-running processes.

### Agent Types

| Criteria | Local | Background | Cloud |
| --- | --- | --- | --- |
| Where it runs | Your machine | Your machine (CLI) | Remote infrastructure |
| Interaction | Interactive | Unattended (async) | Unattended (async), Autonomous |
| Team visibility | No | No | Yes (PRs/issues) |
| Isolation | No (direct workspace) | Yes (worktrees) | Yes (remote) |

With this release, developers can choose among Claude (via Anthropic's official harness) and Codex agents, in addition to Copilot, with the same Copilot subscription.

- **Claude Agent:** Now supported both locally and as a cloud agent.
- **Codex Agent:** Available as a local agent through the OpenAI Codex VS Code extension.
- **GitHub Copilot:** Existing support enhanced with agent interoperability.

> *Both Claude and Codex agents integrate into the Agent Sessions view for unified management.*

---

## Orchestrating Subagents

- **Parallel Subagents:** Kick off multiple isolated subagent sessions simultaneously for research, code scans, or implementation tasks.
- **Visibility:** Monitor which tasks are running, agent identities, prompts, and outputs for each subagent.
- **Custom Agents:** Extend workflows with specialized subagents (e.g., research, implementation, security), leveraging custom tools, instructions, and AI models.
- **Workflow Handoffs:** Design handoff flows, transitioning work from planning through implementation to review, all orchestrated by your main agent session.

---

## Building on Open Standards

- **MCP Apps:** The new MCP extension lets tool calls in chat return interactive components (dashboards, forms, visualizations) for richer collaboration.
- **Agent Skills:** Anthropic’s open standard for agent capabilities—now generally available—lets extension authors package and share domain-specific skills, like testing strategies or API design.
- **Ecosystem Integration:** Agent Skills can be distributed with extensions via `chatSkills` for enhanced, shareable AI-driven capabilities.

---

## Getting Started

- **To enable Claude Agent:** Adjust `github.copilot.chat.claudeAgent.enabled` in your VS Code settings.
- **To run Codex locally:** Requires Copilot Pro+ subscription and the [OpenAI Codex extension](https://marketplace.visualstudio.com/items?itemName=openai.chatgpt).

---

## Learn More

- [Multi-Agent Sessions Documentation](https://code.visualstudio.com/blogs/2026/02/05/multi-agent-development)
- [Agent Skills Guide](https://code.visualstudio.com/docs/copilot/customization/agent-skills)
- [MCP Apps Overview](https://code.visualstudio.com/blogs/2026/01/26/mcp-apps-support)
- [Custom Agents Documentation](https://code.visualstudio.com/docs/copilot/customization/custom-agents)

**Join the Conversation:**

- Raise feedback or issues on the [VS Code repo](https://github.com/microsoft/vscode/issues)
- Attend [Agent Sessions Day](https://youtube.com/live/tAezuMSJuFs) for live demos

---

## Conclusion

With VS Code 1.109, developers can orchestrate complex multi-agent workflows, maximize productivity with AI-powered coding tools, and take advantage of a growing ecosystem built on open standards. The VS Code Team continues to evolve the platform with community feedback at its core.

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2026/02/05/multi-agent-development)
