---
layout: "post"
title: "Making Agents Practical for Real-World Development with VS Code 1.110"
description: "This article details new features in Visual Studio Code 1.110 focused on agent integration for software development workflows. Topics include agent memory, session management, lifecycle hooks, agent skills, integrated browser tools, Copilot CLI integration, and real-world use for complex coding tasks. Developers will learn how agents can become reliable collaborators, how to guide agent behavior, and how to automate or validate typical coding tasks within VS Code."
author: "VS Code Team"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://code.visualstudio.com/blogs/2026/03/05/making-agents-practical-for-real-world-development"
viewing_mode: "external"
feed_name: "Visual Studio Code Releases"
feed_url: "https://code.visualstudio.com/feed.xml"
date: 2026-03-05 00:00:00 +00:00
permalink: "/2026-03-05-Making-Agents-Practical-for-Real-World-Development-with-VS-Code-1110.html"
categories: ["AI", "Coding"]
tags: ["Agent Extensibility", "Agent Memory", "Agent Skills", "AI", "Automation", "Blog", "Browser Tools", "Chat Forking", "Code Review Automation", "Coding", "Coding Productivity", "Context Compaction", "Copilot CLI Integration", "Developer Tools", "Lifecycle Hooks", "News", "Session Management", "Software Development", "VS Code", "VS Code 1.110"]
tags_normalized: ["agent extensibility", "agent memory", "agent skills", "ai", "automation", "blog", "browser tools", "chat forking", "code review automation", "coding", "coding productivity", "context compaction", "copilot cli integration", "developer tools", "lifecycle hooks", "news", "session management", "software development", "vs code", "vs code 1dot110"]
---

VS Code Team presents new agent features in Visual Studio Code 1.110, covering agent memory, skills, extensibility, and session management, aimed at streamlining real-world development workflows.<!--excerpt_end-->

# Making Agents Practical for Real-World Development

**By VS Code Team**

The February 2026 release (1.110) of Visual Studio Code brings major improvements to agent-driven workflows. The update focuses on making agents more practical for real-world development by adding richer controls, better session management, and true workflow integration.

## Giving Agents the Right Context

- **Large outputs**: VS Code now streams large diffs and logs to temporary files, keeping sessions focused and uncluttered. Terminal outputs are organized in collapsible sections.
- **Agent memory**: Agents in VS Code now retain memory across coding, CLI, and code review interactions—architectural decisions and conventions are remembered across sessions.
- **Session compaction**: Sessions can be manually compacted with `/compact`, giving developers control over what information is preserved or discarded, keeping important context in focus for long-running conversations.

## Agent Controls and Guidance

- **Live intervention**: Developers can guide agents mid-response instead of waiting for task completion, reducing wasted cycles and improving feedback loops.
- **Request queuing**: Queue and reorder multiple agent requests, guiding session flow as needed.
- **Session forking**: Fork a chat session with `/fork` or at checkpoints, allowing you to explore alternative solutions while preserving original context.

## Automating and Enforcing Standards with Hooks

- **Lifecycle hooks**: Hooks enforce project conventions and automate checks at key events (e.g., linting, blocking config file changes, running test suites), maintaining standards without repeated manual prompts.
- **Automated commit hooks**: For example, a hook can detect uncommitted changes at session end and trigger a commit/push automatically.

## Agent Extensibility and Skills

- **Slash-command skills**: Invoke agent skills (e.g., `/tests`, `/explain`, `/fix`) directly from chat for repeatable tasks like test generation, code explanation, or targeted fixes. Skills may be built-in, provided by extensions, or project-specific.
- **One-click discovery**: Available skills are shown in the `/` menu for easy reuse.

## Integrated Browser Tools

- **Frontend validation**: Agents can open and interact with integrated browsers within VS Code, implementing UI changes and validating behavior without leaving the editor—a major boost for UI development workflows.

## Workflow Integration: Copilot CLI

- **CLI and editor alignment**: Copilot CLI is now natively integrated; generated changes surface as diffs for review directly inside VS Code. Context is synced between CLI and editor.
- **Typical workflow**: Generate changes via CLI, view diffs in the editor, approve or adjust as needed—all in one place.

## Conclusion and Next Steps

Agents in VS Code are moving towards becoming adaptive, context-aware collaborators that fit naturally into your workflow. With update 1.110, you have more control over agent behavior, deeper workflow integration, and new automation options.

For feedback, join the discussion on the [VS Code GitHub repo](https://github.com/microsoft/vscode/issues) or social media. To see features in action, watch the release livestream on March 19 at 8 AM PST.

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2026/03/05/making-agents-practical-for-real-world-development)
