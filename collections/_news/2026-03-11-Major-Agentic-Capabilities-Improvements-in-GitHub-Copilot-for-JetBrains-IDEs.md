---
external_url: https://github.blog/changelog/2026-03-11-major-agentic-capabilities-improvements-in-github-copilot-for-jetbrains-ides
title: Major Agentic Capabilities Improvements in GitHub Copilot for JetBrains IDEs
author: Allison
primary_section: github-copilot
feed_name: The GitHub Blog
date: 2026-03-11 16:40:03 +00:00
tags:
- Agent Hooks
- Agentic Capabilities
- AI
- Auto Model Selection
- Chat Panel
- Copilot
- Custom Agents
- Developer Tools
- GitHub Copilot
- IDE Extensions
- Improvement
- Instruction Files
- JetBrains IDEs
- MCP
- Model Reasoning
- News
- Plan Agent
- Sub Agents
- User Experience
section_names:
- ai
- github-copilot
---
Allison details major improvements in GitHub Copilot for JetBrains IDEs, focusing on new agentic capabilities, user experience enhancements, and advanced customization for developers.<!--excerpt_end-->

# Major Agentic Capabilities Improvements in GitHub Copilot for JetBrains IDEs

**Author:** Allison

GitHub Copilot for JetBrains IDEs receives a substantial update centered on agentic capabilities, customization features, user experience upgrades, and workflow quality improvements.

## Core New Features

### Agentic Capabilities

- **Custom Agents, Sub-agents, and Plan Agent:**
  - Now generally available, allowing users to tailor Copilot's behavior to their development workflows within JetBrains IDEs.
  - These agents support collaboration and specialized roles directly in the IDE.
  - [Learn more about creating custom agents in JetBrains IDEs](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/create-custom-agents#creating-a-custom-agent-profile-in-jetbrains-ides).

- **Agent Hooks in Preview:**
  - Lets developers run custom commands at key agent lifecycle events (such as `userPromptSubmitted`, `preToolUse`, `postToolUse`, `errorOccurred`).
  - Hooks are defined in a `hooks.json` file placed in `.github/hooks/`:
    - Automate tasks, enforce policies, and integrate other tools directly into Copilot agent sessions.
    - [Agent hooks documentation](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/use-hooks).

- **Auto-Approve for MCP:**
  - Configure auto-approval at the server/tool level to reduce interruptions in agent chat sessions.
  - Setup via **Settings > GitHub Copilot > Chat > MCP Server and Tool Auto-approve Configuration** in JetBrains.

### Customization and Instruction Files

- **Instruction File Support:**
  - Support for `AGENTS.md` and `CLAUDE.md` allows easy customization of agent behavior per project.
  - Copilot discovers and applies instruction files from the workspace or globally.
  - [Generate initial instruction files or manage them in settings](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/create-custom-agents#creating-a-custom-agent-profile-in-jetbrains-ides).

- **`/memory` Slash Command:**
  - Opens settings for agent instruction files for faster management.

### Model Selection and Reasoning

- **Auto Model Selection:**
  - Copilot optimizes model choice based on real-time performance and availability, now GA for all plans.

- **Thinking Panel:**
  - Dedicated UI for tracing extended-reasoning models (e.g., Codex), configurable with Anthropic thinking budgets.

- **Context Window Usage Indicator:**
  - Visual feedback on context utilization during conversations in the chat panel.

## User Experience Improvements

- Streamlined sign-in and chat panel access for reduced friction.
- Enhanced layout, context handling, and approval workflow for better usability.
- Improved support for Windows ARM and refined UI behavior for notifications.

## Quality and Stability Enhancements

- Better handling of terminal output and file update reliability.
- Resolved UI inconsistencies (e.g., lingering actions, blank panels, MCP code vision issues when signed out).
- Visual and interaction improvements to reduce flicker and glitches.

## Deprecation Notice

- **Edit mode** in the chat mode dropdown has been deprecated.

## Get Started and Provide Feedback

- [Try the latest GitHub Copilot plugin for JetBrains IDEs](https://plugins.jetbrains.com/plugin/17718-github-copilot--your-ai-pair-programmer/versions).
- Share feedback using in-product features or via the [issues repository](https://github.com/microsoft/copilot-intellij-feedback/issues).

These updates aim to make GitHub Copilot more flexible, transparent, and convenient for development workflows in JetBrains environments.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-03-11-major-agentic-capabilities-improvements-in-github-copilot-for-jetbrains-ides)
