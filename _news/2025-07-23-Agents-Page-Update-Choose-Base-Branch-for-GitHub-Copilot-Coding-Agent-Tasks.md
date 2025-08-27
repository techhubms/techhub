---
layout: "post"
title: "Agents Page Update: Choose Base Branch for GitHub Copilot Coding Agent Tasks"
description: "GitHub Copilot coding agent now allows users to select a base branch when delegating tasks. This update enables more flexible workflows, especially when working with feature branches or non-default starting points. The feature is accessible via the Agents page and integrates with various GitHub tools."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-07-23-agents-page-set-the-base-branch-for-github-copilot-coding-agent-tasks"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/label/copilot/feed/"
date: 2025-07-23 16:12:28 +00:00
permalink: "/2025-07-23-Agents-Page-Update-Choose-Base-Branch-for-GitHub-Copilot-Coding-Agent-Tasks.html"
categories: ["AI", "DevOps", "GitHub Copilot"]
tags: ["Agents Page", "AI", "Automation", "Base Branch", "Branching Model", "Coding Agent", "DevOps", "Feature Branch", "GitHub CLI", "GitHub Copilot", "News", "Pull Requests", "VS Code"]
tags_normalized: ["agents page", "ai", "automation", "base branch", "branching model", "coding agent", "devops", "feature branch", "github cli", "github copilot", "news", "pull requests", "vs code"]
---

Allison outlines the latest GitHub Copilot coding agent update, letting users designate a base branch for delegated tasks—supporting advanced branching workflows directly from the Agents page.<!--excerpt_end-->

## Agents Page: Set the Base Branch for GitHub Copilot Coding Agent Tasks

**Author:** Allison

With the GitHub Copilot coding agent, you can delegate coding tasks for Copilot to handle in the background while you work on other activities. The latest update introduces the ability to select a base branch when assigning tasks to the Copilot coding agent from the [Agents page](https://github.com/copilot/agents).

### Key Feature

- **Base Branch Selection:** Instead of always initiating work from the repository’s default branch, you can specify which branch Copilot should use as the starting point. Copilot will then create a new branch from your selected base, push its changes, and open a pull request targeting that branch.

### Use Cases

- **Feature Branch Collaboration:** Delegate tasks to Copilot on a feature branch you’re actively developing or reviewing, facilitating seamless collaboration.
- **Branching Model Support:** Supports repositories that follow non-default branch workflows, ensuring Copilot’s automation aligns with custom development processes.

### Availability

Copilot coding agent is available in public preview for:

- Copilot Pro
- Copilot Pro+
- Copilot Business
- Copilot Enterprise (requires administrator-enabled policy)

Learn more in the [official documentation](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/agents/copilot-coding-agent/enabling-copilot-coding-agent).

### Integration

The Agents page is one of several surfaces where you can assign tasks to Copilot. The Copilot coding agent is also integrated with:

- Visual Studio Code
- GitHub Mobile
- GitHub CLI
- GitHub MCP Server

Refer to [the documentation](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/agents/copilot-coding-agent) for further guidance and details.

---

**Source:** [GitHub Blog announcement](https://github.blog/changelog/2025-07-23-agents-page-set-the-base-branch-for-github-copilot-coding-agent-tasks)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-07-23-agents-page-set-the-base-branch-for-github-copilot-coding-agent-tasks)
