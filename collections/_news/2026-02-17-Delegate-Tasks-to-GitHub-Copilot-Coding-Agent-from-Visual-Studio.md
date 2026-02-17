---
layout: "post"
title: "Delegate Tasks to GitHub Copilot Coding Agent from Visual Studio"
description: "This article introduces the GitHub Copilot coding agent's integration with Visual Studio, which allows developers to delegate coding tasks asynchronously. Users can now initiate background tasks via Copilot Chat, leading to automated pull request creation and cloud-based progress. The feature is available for Copilot Pro, Copilot Business, and Enterprise subscribers and requires Visual Studio 2026 or newer."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-17-delegate-tasks-to-copilot-coding-agent-from-visual-studio"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-17 17:00:11 +00:00
permalink: "/2026-02-17-Delegate-Tasks-to-GitHub-Copilot-Coding-Agent-from-Visual-Studio.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["AI", "Asynchronous Tasks", "Automation", "Cloud Development", "Coding", "Copilot", "Copilot Business", "Copilot Chat", "Copilot Coding Agent", "Copilot Enterprise", "Copilot Pro", "Developer Tools", "GitHub Copilot", "Integrated Development Environment", "News", "Pull Requests", "VS"]
tags_normalized: ["ai", "asynchronous tasks", "automation", "cloud development", "coding", "copilot", "copilot business", "copilot chat", "copilot coding agent", "copilot enterprise", "copilot pro", "developer tools", "github copilot", "integrated development environment", "news", "pull requests", "vs"]
---

Allison details how developers can use the Copilot coding agent in Visual Studio to delegate tasks, automate pull requests, and leverage asynchronous background processing within the GitHub Copilot ecosystem.<!--excerpt_end-->

# Delegate Tasks to GitHub Copilot Coding Agent from Visual Studio

The GitHub Copilot coding agent is an asynchronous, autonomous agent capable of handling delegated tasks for developers. When you assign a task to Copilot, it creates a draft pull request, completes the required work in the background (on the cloud), and notifies you for review after completion.

## Key Highlights

- **Visual Studio Integration**: Developers can now access Copilot coding agent directly from Visual Studio 2026 and newer.
- **Initiating Tasks**: Enter a prompt in Copilot Chat and click the new **Send to Copilot Coding Agent** button. After confirmation, Copilot starts the work and opens a pull request.
- **Supported Subscriptions**: Available to Copilot Pro, Copilot Pro+, Copilot Business, and Copilot Enterprise subscribers. For Copilot Business or Enterprise, an admin must enable the feature on the Policies page.
- **Requirements**:
  - Update Visual Studio to at least the December Update 18.1.0
  - Enable the “Enable Copilot Coding agent (preview)” setting in Visual Studio
- **Documentation Links**:
  - [Copilot Coding Agent Overview](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent)
  - [Enabling Copilot Coding Agent](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/agents/copilot-coding-agent/enabling-copilot-coding-agent)
  - [Visual Studio Release Notes](https://learn.microsoft.com/en-us/visualstudio/releases/2026/release-notes#github-copilot-1)
  - [Copilot Chat and Pull Request Workflow](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/create-a-pr#asking-copilot-to-create-a-pull-request-from-copilot-chat-in-visual-studio-2026)

## How It Works

1. In Visual Studio Copilot Chat, write the prompt describing the coding task.
2. Click **Send to Copilot Coding Agent** (next to Send).
3. Confirm the action when prompted.
4. Copilot coding agent runs your task in the background and creates a draft pull request.
5. You review and merge the code when ready.

## Availability & Permissions

- Ensure you belong to an eligible Copilot plan.
- Copilot Business/Enterprise admins must enable the feature in the organization's Policies settings.
- Latest Visual Studio 2026 required.

This update streamlines developer workflows, reducing manual effort for routine coding tasks and enhancing productivity with cloud-powered automation.

*See the official [GitHub Blog announcement](https://github.blog/changelog/2026-02-17-delegate-tasks-to-copilot-coding-agent-from-visual-studio) for more details.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-17-delegate-tasks-to-copilot-coding-agent-from-visual-studio)
