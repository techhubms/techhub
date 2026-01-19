---
layout: post
title: 'Visual Studio Code December 2025 Release (v1.108): New Features and Improvements'
author: Visual Studio Code Team
canonical_url: https://code.visualstudio.com/updates/v1_108
viewing_mode: external
feed_name: Visual Studio Code Releases
feed_url: https://code.visualstudio.com/feed.xml
date: 2026-01-08 17:00:00 +00:00
permalink: /github-copilot/news/Visual-Studio-Code-December-2025-Release-v1108-New-Features-and-Improvements
tags:
- Accessibility
- Agent Skills
- Chat Agents
- Code Editing
- Copilot Customization
- Debugging
- Extension Development
- Git Integration
- Productivity Tools
- Release Notes
- Source Control
- Terminal
- Testing
- TypeScript
- V1.108
- VS Code
section_names:
- ai
- coding
- devops
- github-copilot
---
The Visual Studio Code Team presents the December 2025 (v1.108) update, detailing new agent skill features, Copilot advancements, improved chat and terminal tools, and updates for developers and extension authors.<!--excerpt_end-->

# Visual Studio Code December 2025 Release (v1.108)

**Release date:** January 08, 2026  
**Authors:** Visual Studio Code Team

## Overview

Welcome to the December 2025 release of Visual Studio Code. This release focuses on new features for Copilot and agent skills, notable improvements to chat interfaces, accessibility, source control, and refinements for extension authors and developers. The release includes a major GitHub issue clean-up effort and various productivity boosts for code editing, testing, and debugging.

## Copilot and Agent Skills

### Agent Skills (Experimental)

VS Code now supports **Agent Skills**, allowing developers to teach the coding agent (powered by GitHub Copilot) new capabilities. Skills are stored in folders with an accompanying `SKILL.md` file, which defines their behavior. By storing these in the `.github/skills` directory, specialized domain knowledge and instructions are loaded into Copilot and the chat interface on demand. The settings UI lets you enable this feature ([chat.useAgentSkills](vscode://settings/chat.useAgentSkills)).

For more information, see the [Agent Skills documentation](https://code.visualstudio.com/docs/copilot/customization/agent-skills).

### Improvements to Agent Sessions

- Enhanced keyboard access for session actions
- Grouping and easier management of sessions
- Display of changed files and PRs within each session
- New options to bulk-archive sessions
- General accessibility improvements

## Chat Interface

- The chat Quick Pick now ties closely to agent sessions for better navigation and management.
- Improved session title controls and an updated session restore behavior
- New controls for managing terminal-related actions via chat settings (auto-approve safe actions, manage shell history, and rule exceptions)

## Accessibility

- Streaming chat responses now available in the Accessible View
- MCP server output is excluded from accessible view for clarity
- New dynamic window title variable for accessibility tooling

## Editor, Code Editing, and Terminal

- Drag-and-drop to import settings profiles
- New copyable breadcrumbs paths and customizable separators
- Improved symbol navigation (especially for languages like Rust)
- **Snippet transformations:** New support for snake_case and kebab-case transformations in code snippets
- Terminal IntelliSense UX improvements: now triggered explicitly
- Performance, glyph rendering, and resize overlays improved in the integrated terminal

## Source Control and GitHub Improvements

- New settings for `git blame` (ignore whitespace & disable hovers)
- Worktrees node in Source Control Repositories (experimental)
- Improved authoring commit messages in the editor
- Notable progress on the [GitHub Pull Requests extension](https://marketplace.visualstudio.com/items?itemName=GitHub.vscode-pull-request-github)

## Debugging and Testing

- Breakpoints can now be grouped by file in tree view
- New navigation buttons in the test coverage toolbar

## Extension Authoring

- Quick Pick API updates support more interactive selection interfaces and resource URI-driven icon assignment
- Experimental support for writing extensions directly in TypeScript without build steps

## Engineering and Community

- Major issue clean-up: 5,951 issues closed and 1,203 triaged across repositories, including the Copilot open-sourcing backlog
- Graphs and stats demonstrate ongoing community and contributor growth
- Extensive list of contributor fixes and acknowledgements

## Notable Bug Fixes

- Various reliability and performance improvements, including terminal stability, chat session history, and language support.

## Learn More

For further release notes and details, visit the [official VS Code Updates](https://code.visualstudio.com/updates).

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/updates/v1_108)
