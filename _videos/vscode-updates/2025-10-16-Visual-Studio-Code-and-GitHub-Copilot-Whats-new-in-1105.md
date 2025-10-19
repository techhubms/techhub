---
layout: "post"
title: "Visual Studio Code and GitHub Copilot - What's new in 1.105"
description: "Fokko Veegens provides a comprehensive walkthrough of Visual Studio Code 1.105, released in October 2025, highlighting significant enhancements to GitHub Copilot integration and developer productivity tools. The release introduces AI-powered assistance for complex workflows, improved terminal integration, and enhanced code testing capabilities."
author: "Fokko Veegens"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=zsCBEVCQ98I"
viewing_mode: "internal"
youtube_id: "zsCBEVCQ98I"
date: 2025-10-16 07:12:43 +00:00
permalink: "/2025-10-16-Visual-Studio-Code-and-GitHub-Copilot-Whats-new-in-1105.html"
categories: ["AI", "GitHub Copilot"]
section: "github-copilot"
alt-collection: "vscode-updates"
tags: ["AI", "Chain Of Thought", "Chat Terminal", "Code Coverage", "Developer Tools", "GitHub Copilot", "MCP Marketplace", "Merge Conflicts", "Productivity", "Terminal Profiles", "Videos", "VS Code", "VS Code 1.105"]
tags_normalized: ["ai", "chain of thought", "chat terminal", "code coverage", "developer tools", "github copilot", "mcp marketplace", "merge conflicts", "productivity", "terminal profiles", "videos", "vs code", "vs code 1dot105"]
---

{{ page.description }}<!--excerpt_end-->

{% youtube page.youtube_id %}

## Full summary based on transcript

### AI-Powered Merge Conflict Resolution

GitHub Copilot can now help resolve merge conflicts, a feature particularly useful when conflicts become complex. When a merge conflict occurs, a Copilot sparkle button appears alongside the traditional "Resolve in Merge Editor" option. Clicking this button activates Copilot to analyze both branches and suggest resolutions by examining different file versions and commit hashes. While Copilot can automatically resolve conflicts, developers still need to review the changes carefully as the AI typically chooses one branch's changes over the other.

### Fully Qualified Tool Names

The tool naming convention in agent mode has been updated to use fully qualified names, improving clarity and avoiding conflicts. Tools from extensions and MCP servers now use prefixes like `mspython.python` followed by the specific tool name. This change affects custom chat modes where tools are defined in markdown files. VS Code provides quick fixes to automatically update tool references to the new naming scheme, either through the configure tools interface or via lightbulb quick fix actions.

### OS Notifications for Chat Responses

A new setting `chat.notify on response received` enables system notifications when GitHub Copilot completes a chat response. This feature, available since version 1.103 for confirmation prompts, now extends to all chat responses. When enabled, users receive toast notifications even when working in other applications, allowing them to maintain focus on other tasks while waiting for Copilot's response.

### Chain of Thought

The GPT-5 Codex model introduces chain of thought prompting, allowing developers to see the model's step-by-step reasoning process. A new "thinking style" setting offers options to display these thoughts in collapsed or expanded views. The thinking sections appear as collapsible boxes showing the model's internal reasoning, such as "composing supportive summary message" or "evaluating file path usage." This transparency helps developers understand how Copilot approaches problems, though the feature currently only works with the GPT-5 Codex model.

### Show Recent Chat Sessions

A new `history enabled` setting displays recent chat sessions directly in the chat window, providing quick access to previous conversations without navigating to the separate chat history panel. When enabled, the last few chats appear in the chat interface, with a link to the full chat history also available.

### Keep or Undo Changes During an Agent Loop

During complex agent mode operations that involve multiple files, developers can now keep or undo individual changes as Copilot works. While keeping changes is relatively safe, undoing changes during an active operation can be risky as it might leave the codebase in an inconsistent state. This feature provides granular control over each file modification, allowing developers to accept some changes while rejecting others during the implementation process.

### Keyboard Shortcuts for Navigating Chat Messages

New keyboard shortcuts streamline navigation through chat history. Ctrl+Alt+Up moves to the previous chat message, while Ctrl+Alt+Down moves to the next message. These shortcuts work when the chat window has focus, eliminating the need to scroll through lengthy conversations to review earlier prompts and responses.

### Chat Terminal Profiles

OS-specific terminal settings enable customized terminal configurations for interactive commands executed by GitHub Copilot. Developers can define terminal profiles for Windows, macOS, and Linux in the `settings.json` file, specifying environment variables, shell paths, and command arguments. For example, a Windows PowerShell profile can include custom environment variables that Copilot can access when executing terminal commands. The configuration requires specifying the shell path and can include additional arguments for advanced terminal customization.

### Auto-Reply to Terminal Prompts

The `auto reply to prompts` setting automatically handles terminal confirmation prompts that require simple yes/no responses. When enabled, Copilot automatically inserts "Y" responses for commands like file deletion that request confirmation. With this setting disabled, users must manually confirm these prompts. This feature speeds up workflows but requires careful consideration as it automatically approves destructive operations.

### MCP Marketplace

The MCP (Model Context Protocol) Marketplace improves discoverability of MCP servers through a curated gallery integrated into the VS Code extensions marketplace. An "Enable MCP servers marketplace" button in the extensions view activates this feature by updating settings to display all available MCP servers when filtering for "@mcp". Installation is simplified to a single click, making it easier to extend GitHub Copilot's capabilities with various data sources and tools.

### Add File Commit to Chat Context

GitHub Copilot can now access specific commits from the version control history. Developers can right-click commits in the source control graph and add them to the chat context, enabling comparison of different file versions across commits. This allows asking questions like "what is the difference between these two files" when providing multiple commit versions of the same file, opening new possibilities for working with historical code changes.

### Run Tests with Code Coverage

A new `run tests` tool integrates with testing extensions to execute unit tests and generate code coverage reports directly through GitHub Copilot. This feature requires a testing extension appropriate for the language (such as C# DevKit for C#, or extensions for Java, Python, and JavaScript). When Copilot runs tests, it uses the testing framework in the background and can determine code coverage, displaying results in the test coverage tab. This integration streamlines the testing workflow by eliminating manual terminal commands.

### Additional Features

The release includes several other notable improvements. Apple ID login is now supported alongside existing Google, Microsoft, and GitHub authentication options. Two new AI models are available: GPT-5 Codex with thinking capabilities and Claude Sonnet 4.5, which became generally available shortly before the video recording. The "bring your own key" feature allows individual and enterprise license holders to use locally hosted models or models from cloud providers like Azure, though some features like license blocking for open source code are not available with this option. The dispatch button for sending work to the GitHub coding agent on github.com is now enabled by default, allowing asynchronous work delegation. Finally, nested agents.md files provide hierarchical instruction sets for both the coding agent and local GitHub Copilot agent, though this feature may require experimentation to implement successfully.

Fokko demonstrates each feature with practical examples throughout the video, making it easy to understand how these enhancements integrate into daily development workflows.

[Watch the video on YouTube](https://www.youtube.com/watch?v={{ page.youtube_id }})
