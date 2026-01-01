---
layout: "post"
title: "Kick off and Track Copilot Coding Agent Sessions from the GitHub CLI"
description: "This article introduces the new GitHub CLI 2.80.0 features for managing Copilot coding agent tasks. It covers how developers can start, track, and review asynchronous code tasks delegated to Copilot directly from the CLI, using the new `agent-task` commands and related aliases. The content emphasizes workflow automation through code review and background pull request creation."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-09-25-kick-off-and-track-copilot-coding-agent-sessions-from-the-github-cli"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-09-26 09:00:02 +00:00
permalink: "/2025-09-26-Kick-off-and-Track-Copilot-Coding-Agent-Sessions-from-the-GitHub-CLI.html"
categories: ["AI", "Coding", "DevOps", "GitHub Copilot"]
tags: ["AI", "Automation", "Background Tasks", "CLI Tools", "Client Apps", "Code Review", "Coding", "Command Line", "Copilot", "Copilot Coding Agent", "Developer Productivity", "DevOps", "Gh Agent Task", "GitHub", "GitHub CLI", "GitHub Copilot", "News", "Pull Request", "Release V2.80.0", "Workflow Automation"]
tags_normalized: ["ai", "automation", "background tasks", "cli tools", "client apps", "code review", "coding", "command line", "copilot", "copilot coding agent", "developer productivity", "devops", "gh agent task", "github", "github cli", "github copilot", "news", "pull request", "release v2dot80dot0", "workflow automation"]
---

Allison explains how developers can use GitHub CLI 2.80.0 to manage Copilot coding agent sessions, enabling background code automation and review through new command-line integrations.<!--excerpt_end-->

# Kick off and Track Copilot Coding Agent Sessions from the GitHub CLI

The Copilot coding agent is an asynchronous, autonomous background agent provided by GitHub. With this agent, developers can delegate coding tasks to Copilot, which will automatically create a draft pull request, make required code changes in the background, and request a review when ready.

## GitHub CLI 2.80.0 Release Highlights

A significant new feature in [GitHub CLI 2.80.0](https://github.com/cli/cli/releases/tag/v2.80.0) is the introduction of the `agent-task` command set. This enables efficient management of coding agent tasks directly from the command line interface (`gh`).

### Key Commands

- **Start a new task**:

  ```bash
  gh agent-task create "refactor the codebase"
  ```

- **List all your tasks**:

  ```bash
  gh agent-task list
  ```

- **View task details**:

  ```bash
  gh agent-task view 1234
  ```

- **View task logs in real time**:

  ```bash
  gh agent-task view 1234 --log --follow
  ```

### Command Aliases

The `agent-task` feature includes several aliases for added convenience:

- `gh agent-tasks`
- `gh agent`
- `gh agents`

To explore more command-line options, flags, and arguments, use:

```bash
gh agent-task --help
```

## How It Works

1. **Task Delegation**: Assign work to Copilot using the CLI. Copilot handles the coding in the background.
2. **Draft Pull Requests**: As part of the workflow, Copilot opens draft PRs representing the agent’s progress.
3. **Automated Code Changes**: The coding agent applies changes and lets you review them before completing the workflow.
4. **Task Monitoring**: Keep track of ongoing agent activity and view logs at any time from the CLI.

## Why Use Copilot Coding Agent via CLI?

- Automate repetitive or large-scale code changes.
- Streamline the software development and review process.
- Centralize AI-powered code tasks and their progress within your favorite CLI tools.
- Increase developer productivity through seamless background operations.

## Resources

- [About Copilot Coding Agent](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent)
- [GitHub CLI Release Notes](https://github.com/cli/cli/releases/tag/v2.80.0)
- [Official Announcement](https://github.blog/changelog/2025-09-25-kick-off-and-track-copilot-coding-agent-sessions-from-the-github-cli)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-25-kick-off-and-track-copilot-coding-agent-sessions-from-the-github-cli)
