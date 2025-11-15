---
layout: "post"
title: "Visual Studio Code and GitHub Copilot - What's new in 1.106"
description: "Fokko Veegens provides a comprehensive walkthrough of Visual Studio Code 1.106, released in November 2025, highlighting significant enhancements like plan-mode for guided feature planning, unified Copilot and CLI agent sessions for seamless handoffs, and safer, smarter tooling with open-sourced inline completions and enhanced trust controls."
author: "Fokko Veegens"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=zsCBEVCQ98I"
viewing_mode: "internal"
youtube_id: "8za-H1fWjaM"
date: 2025-11-15 23:00:00 +00:00
permalink: "/2025-11-15-Visual-Studio-Code-and-GitHub-Copilot-Whats-new-in-1106.html"
categories: ["AI", "GitHub Copilot"]
section: "github-copilot"
alt-collection: "vscode-updates"
tags: ["AI", "Chain Of Thought", "Chat Terminal", "Code Coverage", "Developer Tools", "GitHub Copilot", "MCP Marketplace", "Merge Conflicts", "Productivity", "Terminal Profiles", "Videos", "VS Code", "VS Code 1.106"]
tags_normalized: ["ai", "chain of thought", "chat terminal", "code coverage", "developer tools", "github copilot", "mcp marketplace", "merge conflicts", "productivity", "terminal profiles", "videos", "vs code", "vs code 1dot106"]
---

{{ page.description }}<!--excerpt_end-->

{% youtube page.youtube_id %}

## Full summary based on transcript

Visual Studio Code 1.106, released in November 2025, introduces a broad set of improvements centered on GitHub Copilot and multi-agent workflows. The Agent Sessions view is now enabled by default and unifies local agent chats, cloud coding agent sessions that run asynchronously on GitHub Actions, and Copilot CLI sessions. You can position this view on the left or as a single tab inside the chat panel and search long session lists with Ctrl+Alt+F. This consolidates work across devices and contexts while keeping full history accessible.

### Plan mode

Plan mode adds a safe, plan-first companion that analyzes your codebase without writing changes. It generates a concrete plan and either pastes a “start implementation” prompt into Agent mode or opens a Markdown plan file you can reuse as shared context in future sessions. Under the hood, the plan agent uses “handoffs” to delegate follow-up work to other agents with optional auto-send, making multi-step workflows explicit and reproducible.

### Cloud coding agent integration

Cloud coding agent integration has moved from the Pull Requests extension into the Copilot Chat extension. From a repository in the browser you can open an existing coding agent session directly in VS Code and continue the same run locally. Copilot CLI, which is installed via npm, mirrors the chat experience from the terminal and its sessions are visible in the Agent Sessions view. You can start CLI agents in an integrated terminal and even delegate an in-editor or CLI request to the cloud coding agent, which spins up a branch and runs asynchronously while counting as a single premium request.

### Agent Sessions

Agent Sessions now shows per-session edit tracking, reporting lines of code changed so you can assess code churn at a glance. Former “chat modes” have been renamed to custom agents and migrated to .agent.mmd files. Custom agents can be created locally or in a repository folder so collaborators can use them. For coding-agent compatibility, include a shell capability such as “bash” when you want read-only file listing and inspection, since the coding agent does not rely on the local code index. Agent metadata supports target-specific fields, enabling VS Code-only features like handoffs and model selection, or GitHub-side settings such as MCP server lists and the limited tool set available to the coding agent.

### Inline completions open sourced

Copilot Chat has been open-sourced, and its inline completion functionality from the legacy Copilot extension is being unified into the same repository. An experimental setting enables this unified experience now, with full deprecation of the separate Copilot extension planned for early 2026. Day-to-day ergonomics improved as well: you can snooze “next edit” suggestions from the gutter and optionally suppress all inline suggestions for a set period, reducing interruption while editing.

### Trust, safety & data use

Trust and safety features have expanded around external data and tool use. When you add live web context via a URL and the #fetch directive, Copilot shows a pre-ingest preview so you can approve both retrieval and the exact content used, helping prevent prompt injection. Tool approvals can be centrally managed: you can allow specific MCP tools to run without prompting, and separately choose whether to skip post-result review. Terminal actions in agent mode are now parsed by a real command parser that detects redirections and file writes, warning you about hidden write operations before they execute.

### Terminals + chat

The bridge between terminal work and chat is tighter. You can attach any terminal command and its output directly to a chat with a single click beside the command, giving Copilot precise context for troubleshooting. To keep the UI uncluttered, chat tool output can be set to appear inline in the conversation without opening a terminal pane, and any hidden terminals can still be surfaced on demand from a “hidden terminals” entry.

### Prompts & chat ergonomics

Prompt authoring and reuse are easier. The improved /save workflow converts a conversation into a reusable prompt file that you can store under a repository prompts directory and later insert by name. Teams can surface recommended prompt files as welcome buttons in new chats through settings, and a new “Edit prompt file” command opens the referenced Markdown for quick refinement and pull requests.

### File edits & visibility

File visibility during automated edits is more controllable. By default, agent-initiated edits no longer pop open all affected files, keeping your editor focused on what you were viewing. A setting restores the old behavior if you prefer, and the Explorer uses clear badges so you can drill down to changed files quickly even with folders collapsed.

### Reasoning UI

Reasoning UI has broadened beyond a single model. The thinking display, which can show tool calls and reasoning snippets during streaming, is now available for models like GPT-5, GPT-5 Mini, and Gemini 2.5 Pro. You can choose to show no tool use, only read-only tool calls, or all details, and select a thinking style that is fixed scrolling, a collapsible live preview, or fully collapsed with optional expansion.

### Inline Chat v2

Inline Chat v2 is available behind a setting and focuses on single-prompt, code-only edits with optional context. It supports commands such as inline documentation and explanation but intentionally removes iterative rounds to stay tightly scoped to the current file. The main chat view also received a light refresh with a cleaner menu, a left-aligned tool selector, a new chooser to open a chat editor or a separate chat window, and the ability to copy the source for rendered math.

### MCP servers (governance & install)

For MCP server governance, organizations can point developer clients at a curated registry and, where the IDE supports it, enforce allowlists so only approved servers are usable. Support varies by IDE and version, with the latest VS Code providing the most complete path. The Marketplace now clarifies install scope by offering both user-level install and “install in workspace,” the latter creating a .vscode/mcp.json so a server is available only for that project, while user-level MCP servers remain globally accessible via your profile configuration.

Overall, this release emphasizes plan-first development, smooth handoffs between local, CLI, and cloud agents, tighter safety and approvals for tools and external data, and a cleaner editing experience that lets you track edits, control noise, and reuse prompts across your team.

Fokko demonstrates each feature with practical examples throughout the video, making it easy to understand how these enhancements integrate into daily development workflows.

[Watch the video on YouTube](https://www.youtube.com/watch?v={{ page.youtube_id }})
