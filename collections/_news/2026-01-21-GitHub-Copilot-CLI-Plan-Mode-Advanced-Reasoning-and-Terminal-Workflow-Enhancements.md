---
layout: "post"
title: "GitHub Copilot CLI: Plan Mode, Advanced Reasoning, and Terminal Workflow Enhancements"
description: "This news update introduces major feature improvements to GitHub Copilot CLI, including collaborative plan mode, configurable reasoning with the latest GPT-5.2-Codex model, advanced terminal workflow controls, context management, memory, and a streamlined permission experience. Developers now have increased agency and transparency within their coding terminal."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-01-21-github-copilot-cli-plan-before-you-build-steer-as-you-go"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-01-21 21:44:44 +00:00
permalink: "/2026-01-21-GitHub-Copilot-CLI-Plan-Mode-Advanced-Reasoning-and-Terminal-Workflow-Enhancements.html"
categories: ["AI", "GitHub Copilot"]
tags: ["Agentic AI", "AI", "Automation", "Client Apps", "Code Review", "Coding Agent", "Command Line", "Context Management", "Copilot", "Developer Tools", "Feedback", "GitHub CLI", "GitHub Copilot", "GitHub Copilot CLI", "GPT 5.2 Codex", "Improvement", "News", "Permissions", "Plan Mode", "Reasoning Models", "Session Memory", "Shell Mode", "Terminal Workflow"]
tags_normalized: ["agentic ai", "ai", "automation", "client apps", "code review", "coding agent", "command line", "context management", "copilot", "developer tools", "feedback", "github cli", "github copilot", "github copilot cli", "gpt 5dot2 codex", "improvement", "news", "permissions", "plan mode", "reasoning models", "session memory", "shell mode", "terminal workflow"]
---

Allison details new features in GitHub Copilot CLI, including plan mode, advanced reasoning controls, and terminal workflow improvements, giving developers more power and flexibility in their coding environment.<!--excerpt_end-->

# GitHub Copilot CLI: Plan before you build, steer as you go

*Author: Allison*

GitHub Copilot CLI continues to push agentic AI assistance to new levels within the developer's terminal. With this release, the CLI brings a collection of features focused on collaborative planning, intelligent workflow control, transparency, and developer productivity.

## Plan Mode

Plan mode introduces a collaborative experience before actual code generation. By toggling plan mode (Shift + Tab), developers can:

- Align on task scope and requirements with Copilot's clarifying questions (using the new `ask_user` tool)
- Get a structured, reviewable implementation plan
- Stay in control before any code is written

## Advanced Reasoning Models

- **GPT-5.2-Codex**: Now available for code generation and understanding. Choose with `/model` or `--model gpt-5.2-codex`.
- **Configurable Reasoning Effort**: Adjust how hard the model thinks per prompt for a balance of speed and depth.
- **Reasoning Visibility**: Press Ctrl + T to show/hide reasoning steps during completion.

## Steer the Conversation

- **Interact During Processing**: Enqueue messages while Copilot is thinking. Steer discussions or provide follow-ups in real-time.
- **Inline Feedback**: Reject tool permissions and provide feedback directly, allowing Copilot to adapt without stopping the session.

## Background Delegation

Prefix prompts with `&` (or use `/delegate`) to send requests to the Copilot coding agent in the cloud. Continue working locally while Copilot handles asynchronous tasks.

## Automatic Context Management

- **Auto-Compaction**: Conversation history is compressed near token limits in the background.
- **Manual Compaction**: Use `/compact` as needed.
- **Token Usage Visualization**: `/context` command surfaces detailed usage stats.

## Enhanced Permissions Experience

- **Parallel Approval**: Approving for a session will auto-approve similar pending requests.
- **Convenience Flags**: Use `--allow-all` and `--yolo` for faster permission management.

## Code Review

Review your code right in the terminal using the new `/review` command. Get instant feedback on staged or unstaged changes from Copilot before you commit.

## Repository Memory

Copilot now remembers important codebase facts across sessions, learning your standards and preferences for more personalized assistance. Learn more in [Copilot’s engineering blog post on memory](https://github.blog/ai-and-ml/github-copilot/building-an-agentic-memory-system-for-github-copilot/).

## Shell Mode Improvements

- **History Filtering**: Quickly find previous specific commands (e.g., `!git`)
- **Cleaner Environment**: Commands issued via Copilot aren't saved to shell history files.

## Developer Quality-of-Life Improvements

- `/resume` to switch session modes
- `/cd` alias for changing directories
- Streamlined session messages and tab titles
- All custom instruction files now combine for consistency
- Redesigned CLI header

## Easier CLI Access

Getting started is simpler via `gh copilot`. This also integrates with GitHub Actions for hands-free setup.

## Feedback and Updates

Update Copilot CLI with your package manager or npm, and join discussions in [the public repository](https://github.com/github/copilot-cli).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-21-github-copilot-cli-plan-before-you-build-steer-as-you-go)
