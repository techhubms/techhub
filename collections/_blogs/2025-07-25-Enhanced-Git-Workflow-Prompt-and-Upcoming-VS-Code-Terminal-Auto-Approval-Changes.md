---
layout: post
title: Enhanced Git Workflow Prompt and Upcoming VS Code Terminal Auto-Approval Changes
author: Reinier van Maanen
canonical_url: https://r-vm.com/improved-git-workflow-custom-prompt-upcoming-vscode-change-warning.html
viewing_mode: external
feed_name: Reinier van Maanen's blog
feed_url: https://r-vm.com/feed.xml
date: 2025-07-25 00:00:00 +00:00
permalink: /github-copilot/blogs/Enhanced-Git-Workflow-Prompt-and-Upcoming-VS-Code-Terminal-Auto-Approval-Changes
tags:
- Automation
- Branch Protection
- Configuration
- Configuration Changes
- Custom Prompts
- Developer Tools
- Git
- Git Workflow
- GitHub Copilot Chat
- MCP Server
- PowerShell Scripts
- Progress Bars
- Prompt Engineering
- Pull Requests
- Regex Support
- Terminal
- Terminal Auto Approval
- Version Control
- Visual Studio Code Extensions
- VS Code
- Workflow
- Workflow Automation
section_names:
- ai
- coding
- devops
- github-copilot
---
In this post, Reinier van Maanen details his improved Git workflow prompt, VS Code and GitHub Copilot Chat integration, and provides a warning about breaking changes to the terminal auto-approval configuration coming to VS Code.<!--excerpt_end-->

# Enhanced Git Workflow Custom Prompt & Upcoming VS Code Change Warning

_Author: Reinier van Maanen_

Following up on my previous post about automating Git workflows with VS Code and GitHub Copilot Chat, I’ve refined my custom workflow prompt and identified major upcoming changes in Visual Studio Code affecting terminal auto-approval. This write-up provides insights on the improvements and the actions developers need to take to ensure smooth workflows after VS Code updates.

## Table of Contents

- [What’s Changed](#whats-changed)
- [Improved Custom Prompt](#improved-custom-prompt)
  - [Better Terminal Experience](#better-terminal-experience)
  - [Actual Files](#actual-files)
  - [Tips](#tips)
  - [Comparison Table](#comparison-table)
  - [Detailed Improvements](#details)
- [VS Code Terminal Allow List & Auto-Approval](#vs-code-terminal-allow-list--auto-approval)
- [Upcoming VS Code Changes](#upcoming-vs-code-changes)
  - [Configuration Consolidation](#configuration-consolidation)
  - [Configuration Name Change](#configuration-name-change)
  - [Enhanced Regex Support](#enhanced-regex-support)
- [What You Need to Do](#what-you-need-to-do)
- [Timeline](#timeline)
- [References](#references)

---

## What’s Changed

Two significant developments since the previous post:

1. **Custom Prompt Improvements:**
   - Enhanced `/pushall` prompt with better error handling, AI-driven commit message generation, branch safety checks, responsive terminal UI, and scripts migrated to PowerShell.
2. **VS Code Terminal Auto-Approval Changes:**
   - Microsoft is overhauling terminal auto-approval configuration in VS Code, consolidating settings and adding regex flag support. This will impact existing workflows and requires prompt configuration updates.

## Improved Custom Prompt

### Better Terminal Experience

- Implemented a more polished and responsive terminal UI using PowerShell, with progress bars, colored output, adaptive layouts, and real-time countdown timers.
- Enhanced script supports dynamic resizing for a better user experience.

**Examples:**

- ![Enhanced wide terminal UI](/assets/pushall/wide-terminal.png)
- ![Responsive small terminal UI](/assets/pushall/small-terminal.png)

### Actual Files

The updated workflow includes:

- [pushall.prompt.txt (rename to md)](/assets/pushall/pushall.prompt.txt): Main workflow prompt
- [pushall-delay.ps1](/assets/pushall/pushall-delay.ps1): Advanced user confirmation script
- [get-git-changes.ps1](/assets/pushall/get-git-changes.ps1): Git change analysis

_Note: Requires MCP server configuration. Untested outside author’s own environment._

### Tips

Through the prompt, users can pass additional instructions for quick automation. For example:

```
/pushall skip the approval steps, make a new branch, move my changes there, create a PR and assign to Copilot
```

The system smartly interprets these requests, performing multi-step operations autonomously.

### Comparison Table

| Feature                    | Before                                | After                                                      |
|----------------------------|---------------------------------------|------------------------------------------------------------|
| Branch Handling            | Any branch (risky)                    | Main branch protection                                      |
| Change Analysis            | Simple `git --no-pager diff`          | PowerShell + JSON metadata                                  |
| Pull Request Integration   | None                                  | PR creation and Copilot reviews                             |
| User Experience            | 10s delay, basic output                | Multiple confirmations, responsive UI                       |
| Automation                 | Required manual intervention          | Intelligent defaults for unattended operation               |
| Terminal UI                | Text-only                             | Colored, responsive with progress bars                      |
| Script Architecture        | Single Python script                   | Two specialized PowerShell scripts                          |
| Workflow Complexity        | 4 steps                               | 14 detailed steps                                           |
| Conflict Resolution        | Basic instructions                     | Comprehensive handling                                      |

### Details

- **Branch Protection:** Prevents risky operations on main, suggests/switches to a feature branch with user confirmation.
- **Change Analysis:** PowerShell script outputs detailed JSON metadata, detects remote changes, summarizes branch status.
- **Pull Request Integration:** Direct PR creation, update, Copilot reviews via GitHub MCP API.
- **User Experience:** More interactive confirmations, clear guidance, and real-time feedback.
- **Automation:** Defaults enable hands-free operations when possible.
- **Terminal UI:** Enhanced color, responsive layout, and user controls (abort with SPACE, continue with ENTER).
- **Script Architecture:** Modular, maintainable PowerShell scripts for analysis and delay/confirmation.
- **Workflow Complexity:** Structured, multi-step automation for robust operation.
- **Conflict Resolution:** Clear mechanisms to handle remote changes and merge issues.

## VS Code Terminal Allow List & Auto-Approval

To enable seamless automation for these workflows, ensure your VS Code settings allow required commands:

```json
"github.copilot.chat.agent.terminal.allowList": {
  "git": true,
  "pwsh": true
}
```

For MCP actions, you may need to authorize or set workspace-level permissions.

## Upcoming VS Code Changes

### Configuration Consolidation

- **Old:** Separate `allowList` and `denyList` entries.
- **New:** Merged into a single `autoApprove` property.

**Before:**

```json
"github.copilot.chat.agent.terminal.allowList": { "git": true, "echo": true, "ls": true },
"github.copilot.chat.agent.terminal.denyList": { "rm": true, "curl": true, "wget": true }
```

**After:**

```json
"github.copilot.chat.agent.terminal.autoApprove": { "git": true, "echo": true, "ls": true, "rm": false, "curl": false, "wget": false }
```

### Configuration Name Change

The settings will likely be renamed in line with [VS Code Issue #253314](https://github.com/microsoft/vscode/issues/253314):

```json
"chat.agent.terminal.autoApprove": { "git": true, "echo": true, "ls": true, "rm": false }
```

This is part of the terminal auto-approval move from the Copilot extension into the VS Code core ([Issue #252650](https://github.com/microsoft/vscode/issues/252650)).

### Enhanced Regex Support

- **Regex flags** (e.g., `i`, `m`, `s`) are now supported.
- **Case-insensitive and flexible pattern matching** for commands.
- **Examples:**

```json
"chat.agent.terminal.autoApprove": {
  "/^git\s+/i": true,
  "/^Get-ChildItem\b/i": true,
  "/^echo.*/s": true,
  "rm": false,
  "/dangerous/i": false
}
```

These extensions mean configurations can precisely control which terminal commands are auto-approved, even with command name variations or case differences.

## What You Need to Do

1. **Update your settings configuration:** Merge your allow/deny lists into one autoApprove dict and use the new property name.
2. **Leverage new regex features:** Adjust your patterns for case-insensitive and flexible matching, especially for PowerShell workflows.
3. **Monitor VS Code releases:** These features are targeting VS Code v1.103 (expected August 2025), so be prepared to update as you upgrade.

## Timeline

- Changes are merged upstream and will appear in July 2025 release (VS Code v1.103, ships August 2025).
- Old configurations will break once you update to this version.

## References

- [Previous post: Automating my Git workflow in VS Code](/automating-my-git-workflow-vscode-copilot-chat-terminal-auto-approval.html)
- [VS Code Issue #252650](https://github.com/microsoft/vscode/issues/252650)
- [VS Code Issue #253314](https://github.com/microsoft/vscode/issues/253314)
- [VS Code Issue #253472](https://github.com/microsoft/vscode/issues/253472)
- [VS Code Issue #256742](https://github.com/microsoft/vscode/issues/256742)
- [VS Code Pull Request #256725](https://github.com/microsoft/vscode/pull/256725)
- [VS Code Pull Request #256754](https://github.com/microsoft/vscode/pull/256754)
- [VS Code v1.102 Release Notes](https://code.visualstudio.com/updates/v1_102#_terminal-auto-approval-experimental)
- [VS Code July 2025 Milestone](https://github.com/microsoft/vscode/milestone/319)
- [Customize Copilot with Instructions and Prompts](https://code.visualstudio.com/docs/copilot/copilot-customization)

*This article was co-written with GitHub Copilot Chat.*

This post appeared first on "Reinier van Maanen's blog". [Read the entire article here](https://r-vm.com/improved-git-workflow-custom-prompt-upcoming-vscode-change-warning.html)
