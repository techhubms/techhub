---
external_url: https://code.visualstudio.com/updates/v1_117
title: Visual Studio Code 1.117 (April 2026) release notes
feed_name: Visual Studio Code Releases
section_names:
- devops
primary_section: devops
tags:
- Agent Host
- Agent Host Protocol
- Agents App
- Autopilot Permission Mode
- Branch Naming
- Copilot CLI
- Dependency Hover
- DevOps
- Git Isolation
- Insiders
- JavaScript
- JSDoc
- Macos
- News
- Package.json
- Release Notes
- Terminal Integration
- TypeScript
- VS Code
- VS Code 1.117
- Worktrees
author: Visual Studio Code Team
date: 2026-04-16 18:35:16 +00:00
---

The Visual Studio Code Team summarizes what’s new in Visual Studio Code 1.117, including improvements to agent sessions, terminal capture, and Git worktree isolation, plus editor UX fixes for package.json and JSDoc rendering.<!--excerpt_end-->

# Visual Studio Code 1.117 (Insiders)

*Last updated: April 16, 2026*

Welcome to the 1.117 release of Visual Studio Code.

## Highlights

This release includes updates across agent-based workflows, terminal integration, Git/worktree isolation, and a few editor UX improvements.

## April 15, 2026

- Add support for self-updating the Agents app on macOS. [#308646](https://github.com/microsoft/vscode/issues/308646)
- Copilot CLI sessions indicate whether they were created by VS Code or externally. [#308543](https://github.com/microsoft/vscode/issues/308543)
- In `package.json` files, the dependency hover now shows the currently installed version alongside the latest published version of a package. [#307648](https://github.com/microsoft/vscode/issues/307648)
- Images in JSDoc comments, including `<img>` HTML tags, now render correctly in hovers, completion details, and signature help for JavaScript and TypeScript files. [#231792](https://github.com/microsoft/vscode/issues/231792)

## April 14, 2026

- The Autopilot permission mode now persists across sessions. You can configure the default permission level with the `chat.permissions.default` setting. [#309562](https://github.com/microsoft/vscode/issues/309562)
- When an agent sends input to a terminal, the terminal output is now automatically included in the result after a brief delay, saving an extra agent turn. [#309509](https://github.com/microsoft/vscode/issues/309509)
- Agent Host now supports auto-approve session configuration with three modes: Default Approvals, Bypass Approvals, and Autopilot (Preview). [#309337](https://github.com/microsoft/vscode/issues/309337)
- You can now switch back to the main window from auxiliary (floating) windows. [#306571](https://github.com/microsoft/vscode/issues/306571)
- Agent Host Protocol now supports subagents and agent teams. [#305755](https://github.com/microsoft/vscode/issues/305755)

## April 13, 2026

- Agent Host sessions now support worktree and git isolation. [#305325](https://github.com/microsoft/vscode/issues/305325)
- Copilot CLI, Claude Code, and Gemini CLI are now recognized as shell types in the terminal. [#290830](https://github.com/microsoft/vscode/issues/290830)

## April 12, 2026

- Copilot CLI now generates meaningful branch names based on the user's prompt when creating worktrees for background agent sessions. [#306191](https://github.com/microsoft/vscode/issues/306191)

## Links

- Full article: https://code.visualstudio.com/updates/v1_117

We really appreciate people trying new features as soon as they are ready, so check back often and learn what's new.

[Read the entire article](https://code.visualstudio.com/updates/v1_117)

