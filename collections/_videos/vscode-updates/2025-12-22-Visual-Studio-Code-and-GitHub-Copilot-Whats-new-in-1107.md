---
external_url: https://www.youtube.com/watch?v=Pi57sH7iuKY
title: Visual Studio Code and GitHub Copilot - What's new in 1.107
author: Fokko Veegens
feed_name: Fokko at Work
date: 2025-12-22 18:00:00 +00:00
tags:
- Background Agent
- Chain Of Thought
- Chat Terminal
- Claude Skills
- Code Coverage
- Custom Agent
- Developer Tools
- MCP Marketplace
- Merge Conflicts
- Productivity
- Terminal Profiles
- VS Code
- VS Code 1.107
section_names:
- ai
- github-copilot
---
In VS Code 1.107 features like the new agent sessions integration with chat, better integration with background agents and the introduction of Claude skills were introduced. This video explains the new features by demoing them. Keep in mind that some features might not be available to you due to enterprise policies or a different pricing plan.<!--excerpt_end-->

{% youtube Pi57sH7iuKY %}

## Full summary based on transcript

### Introduction

This video covers what’s new in Visual Studio Code 1.107, focusing on GitHub Copilot. It explains changes to Copilot Chat, new ways to run work asynchronously with agents, and several approval and safety improvements around tools, terminals, and sensitive file edits.

### Highlights

Copilot Chat now integrates agent sessions directly into the chat interface instead of a separate sessions view. You can see recent sessions by default, open a sidebar for full history, and filter or search sessions. VS Code also adds smoother delegation of work to background agents (the Copilot CLI running in VS Code) or cloud agents (coding agent), letting you keep working while Copilot completes tasks in parallel.

A major productivity feature is using Git worktrees for background work. When delegated, a background agent can create an isolated worktree and branch so its edits don’t conflict with changes in your current workspace. You can review changes, merge them back when ready, and remove the worktree afterward. The video also previews sharing custom agents across an organization through a GitHub configuration, and shows that the GitHub MCP server is now optionally bundled into the Copilot Chat extension (disabled by default) to expose GitHub tools in the Tools menu when enabled.

### Integrating agent sessions and chat

The sessions list can appear in a sidebar and adapts to available space. When viewing all sessions, extra controls appear for refresh, full-text search, and filtering by session type (local, cloud, background), completion status, and archived state. Sessions can be archived to reduce clutter and later unarchived. Sessions can also be opened as editor tabs, opened to the side, or opened in a new window, and there’s a setting to change how the sessions sidebar is laid out.

### Continue tasks in background or cloud agents

The video demonstrates sending a request to a background agent so Copilot works asynchronously. Background agent changes can be made in an isolated worktree, appear as separate source control changes, and be reviewed before applying. A keep/apply flow merges worktree edits into the main workspace when you choose to accept them. Worktrees can also be opened in another window for testing or review, with the note that the chat session remains attached to the original workspace window.

### New background agent session and worktree vs workspace

You can start a new background agent session directly and choose whether it works in the current workspace or in a worktree for isolation. The presenter recommends worktrees for tasks that might conflict with ongoing local work, such as urgent fixes while you are in the middle of a refactor. A limitation is noted where the background agent UI may not show agents that lack a model attribute.

### Share custom agents across your GitHub organization

Custom agents can be shared within a repo via a local agents folder, or across a whole company via enterprise AI controls. Organization-wide sharing requires a specific GitHub organization and repository setup with an agents directory containing agent definition files. After committing a new agent, it appears in the enterprise UI, then later in VS Code after a reload. The presenter mentions delays, and a duplication bug where the same agent may appear multiple times, which can be hidden in settings.

### GitHub MCP Server built into Copilot Chat

The GitHub MCP server is now available as part of the Copilot Chat extension as an optional setting. When enabled, GitHub tools show up under Tools and can be used for tasks like listing repository issues. The recommendation is to migrate from manual MCP.json configurations to the integrated server if you use it regularly, since extension updates keep it current.

### Custom agents with background agents and subagents

Custom agents can be selected in background agent sessions to tailor how Copilot analyzes or edits code. The video also introduces a subagent pattern: an orchestrator agent calls specialized subagents in sequence, each with a narrower toolset and possibly a different model, to speed up and structure multi-step workflows. The example is a security-fix process that identifies issues (including fetching OWASP guidance), implements fixes, and then verifies results, while noting that nondeterministic behavior can sometimes deviate from strict instructions.

### Context, skills, and approvals

Background agents can now receive context through the same add-context mechanisms, including attaching files or referencing symbols, instead of manually typing filenames. The video also shows basic support for “skills” files stored in a defined folder structure, allowing Copilot to automatically match and use project-specific knowledge when answering questions.

Several approval improvements are covered. URL and domain auto-approval reduces repeated prompts when Copilot fetches web pages for up-to-date information. Debugging variables can be attached to chat so Copilot can reason about runtime state. Terminal output is now rendered inside chat so it remains visible even if the terminal closes. There is an option to allow all terminal commands for the current session, which speeds workflows but increases risk, and approvals can now be confirmed via a keyboard shortcut. For sensitive files where auto-approval is disabled, approval prompts now show diffs to make review easier. Finally, a setting allows restricting which tools are eligible for auto-approval, forcing manual approval for selected tools such as webpage fetching.

### Closing

The presenter briefly mentions additional individual-license features such as bring-your-own-key model endpoints, but does not demo them, and points viewers to the VS Code updates site for full details before closing with a holiday message and a note about a future VS Code 1.108 video.

Fokko demonstrates each feature with practical examples throughout the video, making it easy to understand how these enhancements integrate into daily development workflows.

[Watch the video on YouTube](https://www.youtube.com/watch?v=Pi57sH7iuKY)
