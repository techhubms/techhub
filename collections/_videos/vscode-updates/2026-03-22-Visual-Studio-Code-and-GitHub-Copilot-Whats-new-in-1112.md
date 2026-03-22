---
external_url: https://www.youtube.com/watch?v=BC35VXggNDc
title: Visual Studio Code and GitHub Copilot - What's new in 1.112
author: Fokko Veegens
feed_name: Fokko at Work
date: 2026-03-22 18:00:00 +00:00
tags:
- Autopilot CLI
- CLI Pending Changes
- Customizations Discovery
- Debug Logs
- Image Analysis
- MCP Server Management
- Message Steering
- Symbol Copy-Paste
- Troubleshoot Command
- VS Code
- VS Code 1.112
- AI
- GitHub Copilot
- Videos
section_names:
- ai
- github-copilot
primary_section: github-copilot
---
Fokko Veegens demos the new features in VS Code 1.112, including message steering and queueing for CLI/background agent sessions, expanded pending changes visibility in CLI sessions, autopilot mode for the CLI, the new /troubleshoot command for diagnosing agent issues, exportable and shareable debug logs, image analysis for agents, symbol copy-paste in chat, and customizations discovery from parent repositories.<!--excerpt_end-->

{% youtube BC35VXggNDc %}

## Full summary based on transcript

Visual Studio Code 1.112 (March 2026) extends several features that were previously limited to regular agent sessions into CLI and background agent sessions, adds a new troubleshooting command for diagnosing Copilot behavior, and introduces image analysis capabilities for agents.

### Message Steering and Queueing for CLI Sessions

Message steering and queueing, introduced in VS Code 1.110 for regular agent sessions, is now also available for background agent and CLI sessions.

**How it works:**

- While a CLI/background agent session is busy, you can type additional requests
- Three options appear:
  - **Steer with message** (default) - the message is handled after the current tool call completes
  - **Add to queue** - the message is sent once Copilot has fully completed
  - **Stop and send** - stops current execution and sends your new message

**Example use case:**

- Start a task like adding a property and calculated field to a model
- While Copilot is working, type "please also add unit tests for any changes you apply"
- Choose "steer with message" to have it picked up during the current session

### CLI Pending Changes Visibility

When starting a CLI or background agent session with pending changes in the workspace, Copilot asks what to do with those changes. This dialog now shows which files are affected.

**Options for pending changes:**

- **Copy changes** - copies the changes into the CLI worktree environment
- **Move changes** - undoes the changes in the current workspace and takes them along into the CLI session
- **Skip changes** - leaves the changes in the workspace and starts the CLI session with a clean worktree

**What's new:**

- The dialog now has an expandable section showing the list of affected files
- Clicking a file currently shows the new version of the file (diff view is expected to be added in a future update)

### Autopilot Mode for CLI

Autopilot mode, introduced in VS Code 1.111 for regular agent sessions, is now available for CLI sessions as well. Autopilot bypasses all approval prompts for tools, MCP servers, and sensitive file edits.

**Enabling autopilot:**

1. Go to settings and search for `chat.autopilot.enabled`
2. Check the box to enable autopilot
3. In the CLI approval menu, a new "autopilot" option appears alongside "default approvals" and "bypass approvals"
4. Selecting it triggers a warning prompt about the risks

**Recommendation:** Run autopilot in a dev container or on GitHub Codespaces to limit the potential impact of unattended operations.

### /troubleshoot Command

A new `/troubleshoot` command helps diagnose why Copilot is not behaving as expected, such as when instructions or skills are not being picked up.

**Prerequisites (two settings must be enabled):**

1. `github.copilot.chat.agentDebugLog.enabled` - enables debug logging
2. `github.copilot.chat.agentDebugLog.fileLogging.enabled` - writes JSONL diagnostic files to disk

**How to use:**

- Use `/troubleshoot` followed by a specific question, e.g., `/troubleshoot why is the instruction for XML comments not picked up?`
- The command analyzes the debug logs and identifies the root cause

**Tips:**

- Be explicit and specific in your troubleshooting question for better results
- Results can vary between sessions depending on phrasing
- Useful for diagnosing issues with instructions, skills, and custom agent behavior

### Debug Log Export and Import

Agent debug logs can now be exported and shared with team members for collaborative debugging.

**Exporting:**

1. Open the agent debug panel via the command palette (search for "agent debug logs")
2. Select a session from the list
3. Export it as a JSON file

**Importing:**

- Import a shared JSON file into the debug panel
- The imported session appears in the session list and can be inspected as if it were your own
- You can view the flowchart and all session details

**Privacy warning:** Debug log exports contain detailed session data that may include personal information. Review the file for sensitive data before sharing.

### Image Analysis for Agents

Agents can now read images and binary content directly from the workspace. This enables visual analysis and code fixes based on screenshots.

**Example use case:**

- Take a screenshot of a UI with overlapping or unreadable text
- Ask Copilot to analyze the screenshot and check for overlapping content
- Copilot identifies the visual issues, locates the relevant code, and applies fixes

**Notes:**

- Results depend on the model used (Claude Sonnet 4.6 identified overlapping content that GPT-4.1 missed in testing)
- You can attach images from the file explorer or reference them in chat

**Related settings:**

- `chat.imageCarousel.enabled` - enables an image carousel for previewing chat image attachments
- `explorer.contextMenu.enabled` - adds "Open in image carousel" to the Windows Explorer right-click menu for image files

### Symbol Copy-Paste in Chat

Copying a symbol from the editor and pasting it into the chat input now automatically creates a symbol reference instead of plain text.

**How it works:**

- Copy a function name, variable, or other symbol from the editor
- Paste it in chat - it becomes a `sym:` reference linked to the symbol's location
- Works with functions, variables, and other code symbols
- To paste as plain text instead, use Ctrl+Shift+V (Windows) or Cmd+Shift+V (macOS)

### Customizations Discovery from Parent Repositories

Custom instructions, agents, skills, and other customizations stored in the `.github` folder of a parent repository are now discovered when you open a subfolder of that repository.

**Enabling the feature:**

1. Go to settings and search for `chat.useCustomizationsInParentRepositories`
2. Enable the setting
3. Reload the window

**Use case:**

- In mono-repositories where developers typically open subfolders rather than the root, customizations defined at the root level are now picked up automatically
- Applies to custom instructions, custom agents, skills, and `copilot-instructions.md`

### MCP Server Management

The new customizations panel provides a unified view of all Copilot customizations, including MCP servers.

**How to access:**

- Open the command palette (F1 / Ctrl+Shift+P) and run `Chat: Open Customizations`
- Shows all customizations: agents, skills, instructions, and MCP servers

**MCP server management:**

- Right-click an MCP server to disable it
- Two disable options: disable for the current session or disable for the workspace
- When disabled, Copilot no longer picks up the MCP server
