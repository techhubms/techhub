---
external_url: https://github.blog/changelog/2026-01-14-github-copilot-cli-enhanced-agents-context-management-and-new-ways-to-install
title: 'GitHub Copilot CLI: Enhanced Agents, Context Management, and Installation Methods'
author: Allison
feed_name: The GitHub Blog
date: 2026-01-14 20:21:16 +00:00
tags:
- AI Agents
- Automation
- CI/CD Authentication
- Client Apps
- Code Analysis
- Code Review
- Context Management
- Copilot
- DevOps Tools
- GitHub Codespaces
- GitHub Copilot CLI
- GPT 4.1
- GPT 5 Mini
- Homebrew
- Improvement
- Installation
- Model Management
- Project Context
- Scripting
- Session Management
- Terminal
- WinGet
- AI
- GitHub Copilot
- News
section_names:
- ai
- github-copilot
primary_section: github-copilot
---
Allison introduces significant improvements to GitHub Copilot CLI, including advanced agents, enhanced context features, improved installation workflows, and better scripting options for developers.<!--excerpt_end-->

# GitHub Copilot CLI: Enhanced Agents, Context Management, and Installation Methods

Allison shares the latest updates for GitHub Copilot CLI, improving the developer experience with new agents, greater context management, and easier installation workflows.

## New Models and Model Management

- **GPT-5 mini** and **GPT-4.1** models are now available for Copilot CLI users. These models are included with Copilot subscriptions and do not consume premium requests on paid plans.
- The CLI provides prompts to enable models directly in the terminal, via the `/model` command, model picker, or `--model` flag, streamlining the user experience for Copilot Pro and Pro+ users.

## Built-In Custom Agents

- **Explore Agent**: Allows fast codebase analysis and lets users query their codebase without cluttering their current session.
- **Task Agent**: Runs tests and builds, providing concise summaries on success and full outputs on failure.
- **Plan Agent**: Generates implementation plans by analyzing dependencies and project structure.
- **Code-review Agent**: Reviews code changes, surfacing only genuine issues and minimizing noise.
- Copilot CLI can leverage multiple agents in parallel as appropriate and integrates with [Agent Skills](https://github.blog/changelog/2025-12-18-github-copilot-now-supports-agent-skills/) for advanced workflows.

## Installation Methods

- **Windows:** Install via WinGet: `winget install GitHub.Copilot`
- **macOS/Linux:** Install using Homebrew: `brew install copilot-cli` or with the install script: `curl -fsSL https://gh.io/copilot-install | bash`
- Copilot CLI is included in GitHub Codespaces and available as a [Dev Container Feature](https://github.com/devcontainers/features).
- Standalone executables are distributed through [GitHub Releases](https://github.com/github/copilot-cli/releases).

## Automation and Scripting

- New CLI flags simplify scripting and CI/CD integration:
  - `--silent`, `--share`, `--share-gist`, `--available-tools`, `--excluded-tools`, `--additional-mcp-config`.
- `GITHUB_ASKPASS` environment variable supports authenticated CI/CD workflows.
- Copilot Spaces enhancements allow project-specific context via the GitHub MCP server.

## Context Management

- Automatic history compaction when token usage nears limit (95%).
- Use `/compact` for manual context compression and `/context` for token usage visualization.
- The `--resume` function enables cycling through local and remote Copilot agent sessions with TAB completion.

## Enhanced Terminal Experience

- Detailed syntax highlighting in diffs, integrated with Git pagers.
- Path autocomplete in directory-related commands.
- Toggle model reasoning visibility with `Ctrl+T` (supported models).
- Agent run commands are now excluded from shell history to keep command logs clean.

## Web Access Controls

- The new `web_fetch` tool retrieves URL content as markdown, with access controlled via configuration patterns in `~/.copilot/config`.
- URL restrictions also apply to shell-based commands like `curl` and `wget`.

## Getting Involved

- Update Copilot CLI with your package manager or run `npm install -g @github/copilot@latest`.
- Feedback and discussion are welcome in the [Copilot CLI’s public repository](https://github.com/github/copilot-cli).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-14-github-copilot-cli-enhanced-agents-context-management-and-new-ways-to-install)
