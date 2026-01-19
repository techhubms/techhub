---
layout: post
title: Visual Studio Code and GitHub Copilot - What's new in 1.108
author: Fokko Veegens
canonical_url: https://www.youtube.com/watch?v=d6IzQ2y3nuQ
viewing_mode: internal
youtube_id: d6IzQ2y3nuQ
date: 2026-01-15 18:00:00 +00:00
permalink: /github-copilot/videos/Visual-Studio-Code-and-GitHub-Copilot-Whats-new-in-1108
section: github-copilot
alt-collection: vscode-updates
tags:
- Agent Skills
- Agent Sessions
- Auto Approve
- Chat Sessions
- Developer Tools
- NPM
- Productivity
- Terminal
- VS Code
- VS Code 1.108
section_names:
- ai
- github-copilot
---
VS Code 1.108 introduces agent skills support in the GitHub folder, enhanced agent sessions view with grouping and archiving, improved chat picker integration, and expanded terminal auto-approval capabilities. Fokko Veegens demonstrates these features with practical examples showing how skills can automate controller creation and how the new terminal command parsing prevents hidden operations.<!--excerpt_end-->

{% youtube d6IzQ2y3nuQ %}

## Full summary based on transcript

Visual Studio Code 1.108, released in January 2026, brings significant improvements to GitHub Copilot's agent skills, session management, and terminal integration. The update enhances developer productivity through better skill organization, improved session navigation, and smarter terminal command handling.

### Agent Skills (Experimental)

Agent skills, introduced in the previous release, now support placement in the `.github/skills/` folder alongside the existing `.copilot/skills/` location. Skills allow developers to describe specific processes and procedures for GitHub Copilot to follow, and unlike custom instructions, multiple skills can be applied simultaneously in the same session.

Skills are portable across different Copilot environments - they work in VS Code, with the background agent, and on github.com with the coding agent. This portability makes them more versatile than some types of custom instructions.

**Creating a skill:**

- Create a folder in `.github/skills/` or `.copilot/skills/` with your skill name (e.g., `flight-log`)
- Add a `skill.md` file with frontmatter containing:
  - `name`: Descriptive name for the skill
  - `description`: Detailed description using natural language that helps Copilot identify when to use the skill
- Include markdown documentation explaining what the skill does
- Provide step-by-step instructions for implementation

**Example skill structure:**

```markdown
---
name: Create Controller
description: Triggers on requests like "create a new controller for {entity name}"
---

# Controller Creation Skill

This skill automates the creation of API controllers for models that don't have controllers yet.

## Steps:
1. Identify the target model
2. Read the model structure
3. Create a new controller with standard CRUD operations
4. Include logging
5. Add example data
```

**Using skills:**

- Ask Copilot "what skills do you have available" to see all available skills
- The system checks both `.copilot/` and `.github/` folders for skills
- When you make a request matching a skill's description, Copilot automatically recognizes and uses it
- Skills work across local agents, background agents, and the coding agent on github.com

Fokko demonstrates creating a controller for an airfield model in a C# web API project. The skill correctly sets up the controller as an API controller, includes a logger, implements expected methods, and adds example data - all following the skill's instructions.

### Improvements to Agent Sessions View

The Agent Sessions view has received several usability enhancements for better organization and navigation:

**Grouping by date:**

- Sessions are now automatically grouped into "Today," "Last week," and "Older" categories
- Double-click groups to collapse/expand them
- Navigate with arrow keys (up/down)
- Press spacebar to select sessions or collapse/expand groups

**Enhanced metadata:**

- Uncommitted changes display exact counts (e.g., "2 additions, 80 removals")
- Icons indicate session type:
  - Local agent sessions (default icon)
  - Background agent sessions (Git branch icon)
  - Cloud coding agent sessions (cloud icon)

**Bulk operations:**

- Archive all chats in a group with a single click
- Archive individual sessions by pressing the Delete key

**Keyboard shortcuts:**

- Right-click any session to see available commands and keyboard shortcuts
- Rename sessions directly from the context menu
- Mark sessions as read/unread (keyboard shortcut may require custom binding)

### Chat Picker Based on Agent Sessions

The chat picker, accessed by clicking the chat title, now uses the same titles as the agent sessions view for consistency. This quick picker provides:

- Overview of recent chat history
- Visual indicators showing session type (local, background, or cloud)
- Quick switching between sessions by clicking on them
- Consistent naming across all session views

### Open Empty Chat on Restart

VS Code 1.108 changes the default startup behavior to show an empty chat instead of restoring the last session. This provides a fresh start for each coding session.

**Restoring old behavior:**

- Setting: `chat.restore.lastPanelSession`
- Enable this setting to continue where you left off from the previous session

### Terminal Tool Auto Approve Default Rules

Auto-approval for terminal commands helps speed up the coding agent by automatically executing safe commands while requiring confirmation for potentially dangerous operations.

**Configuration:**

- Setting: `enableAutoApprove` (default: off)
- First time you encounter an auto-approval situation, you're prompted to enable it
- Setting: `terminal.autoApprove` - list of commands that can be auto-approved

**New commands in auto-approve list:**

- `git ls-files`
- `rg` (ripgrep)
- Various other safe read-only commands (approximately 6-7 new additions)

**Command approval levels:**

- `true`: Can be auto-approved
- `false`: Cannot be auto-approved (e.g., `rm` command always requires confirmation)

**Transparency improvements:**

- When auto-approval is denied, the system shows which rule blocked it
- Click the rule to navigate directly to the relevant setting
- Example: `rm` command shows "Denied by rule: RM user" and links to the `chat.approve.autoApprove` setting

**NPM/Yarn/PNPM special handling:**

- New setting ensures NPM, Yarn, and PNPM commands can run without approval when:
  - Commands are defined in `package.json`
  - Workspace is trusted (required for agent mode)
- Rationale: Scripts in `package.json` of trusted workspaces are considered safe
- This setting is enabled by default but can be disabled

### Add Session and Workspace Rules for Future Terminal Tool Commands

The auto-approval system now supports more granular control over command execution:

**Approval scopes:**

1. **Session-level approval:**
   - "Allow npm install... in this session"
   - Applies only to the current chat conversation
   - Approval is lost when starting a new chat

2. **Workspace-level approval:**
   - "Allow npm install... in this workspace"
   - Applies to all sessions within the current directory

3. **Always allow:**
   - "Always allow npm install..."
   - Global approval across all workspaces

**Command matching options:**

1. **Wildcard matching:**
   - "Allow npm install..." (with ellipsis)
   - Matches the command and anything after it
   - ⚠️ Warning: Can be risky as it allows piped commands (e.g., `npm install | rm -rf`)

2. **Exact command line:**
   - Matches only the exact command (e.g., only `npm install`, not `npm install package-name`)
   - Available for session, workspace, and always scopes

This granular control helps balance automation speed with security concerns.

### Terminal Tool Preventing Adding to Shell History

A highly requested feature: Copilot terminal commands no longer clutter your shell history by default.

**Benefits:**

- Keeps terminal history clean and relevant to manual commands
- Copilot commands are always available in chat history
- Reduces noise when using arrow keys to recall commands

**Implementation (varies by shell):**

For Bash:

- Sets `HISTCONTROL=ignorespace`
- Prepends commands with a space
- Commands with leading spaces are not added to history

**Configuration:**

- Setting: `chat.tools.preventShellHistory`
- Default: Enabled
- Copilot creates hidden chat terminals for command execution
- Hidden terminals can still be revealed if needed

**Example behavior:**

- Copilot executes `Get-ChildItem` in a hidden terminal
- Pressing arrow-up in your terminal won't show this command
- All Copilot commands remain searchable in chat history

This feature respects the separation between AI-generated commands and developer-initiated commands, making terminal history more useful and less cluttered.

### Additional Notes

The release focuses on refining the agent experience with practical improvements based on user feedback. Many features are experimental and may not be available to all users due to enterprise policies or pricing plans. Fokko provides detailed demonstrations of each feature with timestamps for easy navigation.

[Watch the video on YouTube](https://www.youtube.com/watch?v=d6IzQ2y3nuQ)
