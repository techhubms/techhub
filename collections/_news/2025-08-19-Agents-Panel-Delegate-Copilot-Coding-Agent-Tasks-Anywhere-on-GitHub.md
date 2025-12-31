---
layout: "post"
title: "Agents Panel: Delegate Copilot Coding Agent Tasks Anywhere on GitHub"
description: "This article introduces the new Agents panel for GitHub, allowing developers to delegate coding tasks to the Copilot coding agent from any page. The update details how Copilot automates coding workflows, integrates with various GitHub tools, and tracks task progress in real-time."
author: "Tim Rogers"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/news-insights/product-news/agents-panel-launch-copilot-coding-agent-tasks-anywhere-on-github/"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/feed/"
date: 2025-08-19 19:53:14 +00:00
permalink: "/news/2025-08-19-Agents-Panel-Delegate-Copilot-Coding-Agent-Tasks-Anywhere-on-GitHub.html"
categories: ["AI", "GitHub Copilot"]
tags: ["Agentic Workflows", "Agents Panel", "AI", "AI Agents", "Autonomous Developer Agent", "Background Coding Tasks", "Copilot Coding Agent", "Generative AI", "GitHub Copilot", "GitHub Mobile", "MCP", "MCP Server", "News", "News & Insights", "Product", "Pull Requests", "Task Delegation", "VS Code Integration", "Workflow Automation"]
tags_normalized: ["agentic workflows", "agents panel", "ai", "ai agents", "autonomous developer agent", "background coding tasks", "copilot coding agent", "generative ai", "github copilot", "github mobile", "mcp", "mcp server", "news", "news and insights", "product", "pull requests", "task delegation", "vs code integration", "workflow automation"]
---

Tim Rogers explains how the new Agents panel lets developers assign and track Copilot coding agent tasks from anywhere on GitHub, making code automation and review more seamless.<!--excerpt_end-->

# Agents Panel: Delegate Copilot Coding Agent Tasks Anywhere on GitHub

**Author:** Tim Rogers

GitHub has introduced the Agents panel—a new feature that empowers developers to assign coding tasks to the Copilot coding agent from any page on [github.com](http://github.com). This helps automate coding workflows and lets users track task progress without losing their place in their current project.

## What Is the Copilot Coding Agent?

The Copilot coding agent is an asynchronous, autonomous developer agent. Developers can assign an issue or task to Copilot, which then works in the background: planning changes, running builds/tests, and creating draft pull requests for review. This agent operates like a virtual team member and integrates with GitHub Issues, Visual Studio Code, GitHub Mobile, and any tool that supports the Model Context Protocol (MCP).

## Introducing the Agents Panel

The new Agents panel is a lightweight overlay available throughout GitHub. It allows developers to:

- Assign background coding tasks to Copilot from anywhere on GitHub
- Monitor progress and view logs in real-time
- Review and approve generated pull requests
- Launch new tasks using natural language prompts

You can open the full Agents panel, which serves as a dashboard for all recent Copilot tasks, or access it directly via [github.com/copilot/agents](https://github.com/copilot/agents).

### Launching Tasks With the Agents Panel

To start a task:

1. Open the Agents panel from the navigation bar.
2. Describe your goal in plain language (e.g., "Add integration tests for LoginController").
3. Optionally reference issues, pull requests, or multiple parallel tasks.
4. Select a repository and branch.
5. Copilot manages the entire workflow—from planning to draft PR.

Example prompts:

- “Add integration tests for LoginController”
- “Refactor WidgetGenerator for better code reuse”
- “Fix #877 using pull request #855 as an example”

## How Copilot Coding Agent Works

Copilot coding agent tasks can be triggered from GitHub Issues, GitHub Mobile, VS Code, JetBrains IDEs, and any MCP-enabled tool. Thanks to integration with the Model Context Protocol, Copilot can access repository data, test web pages, and connect to custom MCP servers for more advanced scenarios.

The coding agent executes tasks securely in a cloud environment powered by GitHub Actions. Developers get detailed logs, control via PR approval, and the ability to give feedback directly in reviews.

## Availability

Copilot coding agent and the Agents panel are now in public preview for all paid Copilot subscribers. Administrators may need to enable the Copilot coding agent policy for enterprise use. For more, see:

- [Copilot coding agent documentation](https://docs.github.com/en/copilot/concepts/coding-agent/coding-agent)
- [GitHub Copilot info](https://github.com/features/copilot)

## Summary

The Agents panel improves developer workflow flexibility by making it easier to delegate, monitor, and review AI-generated code contributions powered by GitHub Copilot’s autonomous capabilities.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/news-insights/product-news/agents-panel-launch-copilot-coding-agent-tasks-anywhere-on-github/)
