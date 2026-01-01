---
layout: "post"
title: "What's New in Visual Studio Code 1.97: GitHub Copilot Enhancements and More"
description: "This article from the Visual Studio Code Team details the January 2025 (version 1.97) release, showcasing key productivity features and improvements for developers. Highlights include advanced GitHub Copilot suggestions and editing, new security enhancements, updates for extension management, improved debugging, Python REPL integration, accessible features, and enhanced logs handling. The release also brings updates for remote development, notable engineering fixes, improvements to code search, and previews of experimental agent mode and vision features for Copilot in VS Code."
author: "Visual Studio Code Team"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://code.visualstudio.com/updates/v1_97"
viewing_mode: "external"
feed_name: "Visual Studio Code Releases"
feed_url: "https://code.visualstudio.com/feed.xml"
date: 2025-02-06 17:00:00 +00:00
permalink: "/2025-02-06-Whats-New-in-Visual-Studio-Code-197-GitHub-Copilot-Enhancements-and-More.html"
categories: ["AI", "Coding", "DevOps", "GitHub Copilot"]
tags: ["Accessibility", "Agent Mode", "AI", "Code Completion", "Coding", "Command Palette", "Compound Logs", "Copilot Edits", "Copilot Next Edit Suggestions", "Copilot Vision", "Debugging", "DevOps", "Extension Security", "Git Blame", "GitHub Copilot", "Logs Export", "Microsoft Authentication", "MSAL", "News", "Notebooks", "Python", "Remote Development", "REPL", "Settings Search", "Source Control", "Terminal", "TypeScript", "Version 1.97", "VS Code", "Workspace Index"]
tags_normalized: ["accessibility", "agent mode", "ai", "code completion", "coding", "command palette", "compound logs", "copilot edits", "copilot next edit suggestions", "copilot vision", "debugging", "devops", "extension security", "git blame", "github copilot", "logs export", "microsoft authentication", "msal", "news", "notebooks", "python", "remote development", "repl", "settings search", "source control", "terminal", "typescript", "version 1dot97", "vs code", "workspace index"]
---

The Visual Studio Code Team covers the 1.97 January 2025 release, with in-depth updates on GitHub Copilot features, editing improvements, DevOps tooling, and developer experience enhancements.<!--excerpt_end-->

# Visual Studio Code January 2025 (Version 1.97) Release Notes

The Visual Studio Code Team introduces significant new features and updates in the January 2025 (version 1.97) release. This version is packed with productivity improvements, Copilot advancements, security features, deeper DevOps integration, and many enhancements for developers. Below are the main highlights:

## Key Highlights

### GitHub Copilot: Smarter Coding Assistance

- **Copilot Next Edit Suggestions (Preview):** Copilot can now predict likely next edits, showing suggestions not just for completions but also for refactoring and evolving code. These suggestions appear dynamically; configuration options let you control display and acceptance behavior.
- **Copilot Edits General Availability:** Copilot Edits is now fully available, allowing AI-assisted refactoring and edits across multiple files, managed directly from Copilot Chat. Acceptance and discarding of suggested changes have been improved, with granular control for individual edits and time-based auto-acceptance options.
- **Apply in Editor:** Users can instantly apply suggested code blocks from Copilot Chat to their workspace files. The workflow now prompts for file creation if the suggested file doesn’t exist.
- **Temporal Context and Workspace Indexes:** Copilot is contextually smarter, using files you've recently interacted with and a local/remote codebase index to provide accurate, relevant suggestions.
- **Model Availability:** Expanded AI model support, including OpenAI's o3-mini and Google's Gemini 2.0 Flash, for improved code intelligence and explanations.
- **Experimental Features:** Agent Mode enables autonomous Copilot operations for editing, error-checking, and terminal commands. Agentic codebase search and Copilot Vision (image understanding in prompts) are in preview in VS Code Insiders.

### Core Editor and Extension Improvements

- **Command Palette and Quick Inputs:** Palette can now be moved and positioned flexibly in the window. Preset and custom layouts are supported.
- **Trusted Extension Publisher Dialog:** Enhanced extension management helps ensure that only trusted publishers’ extensions are installed, bolstering development environment security.
- **Extension Filter Upgrades:** Find outdated or recently-updated extensions more easily with `@outdated` and `@recentlyUpdated` filters.

### Productivity and Debugging Enhancements

- **Compound Logs & Output Panel Filtering:** Aggregate multiple logs into a single view and filter logs by text or severity. Export/import logs for collaboration.
- **Git Blame and GitHub Integration:** Expanded blame information, direct links to GitHub from editor, and visible avatars for repository contributors.
- **Enhanced Notebooks:** Inline values now appear upon cell execution, and Markdown cell rendering supports custom fonts.
- **Terminal Improvements:** Font ligature support, PowerShell and shell completions, sticky scroll visibility for long commands, improved control for closing terminals, and ConEmu progress support.
- **Debug Console and Variables:** Improved history and value searching, plus more reliable content selection and new commands for pretty-printing JavaScript.

### Python & TypeScript Updates

- **Python REPL and Debugging:** Native VS Code REPL launch from terminals, Python no-config debugging, and accelerated code navigation with Go to Implementation via Pylance.
- **Experimental AI Code Action for Python:** Use Copilot and Pylance to generate code symbols from prompts.
- **TypeScript 5.7.3 Recovery Release:** Bug fixes and import regression resolution, along with preview support for TypeScript 5.8 and new Tree-Sitter based syntax highlighting.

### Engineering & Community Contributions

A substantial round of bug fixes, resource optimizations (notably for large TypeScript workspaces), and community pull requests is acknowledged in the release.

## Security and Accessibility

- Security-related bug fixes in recovery updates 1.97.1 and 1.97.2.
- Publisher trust dialog and improved extension trust management.
- Enhanced accessibility sounds, status dialogs, keyboard navigation for Copilot Edits, and more screen reader-friendly notifications.

## Remote Development

- Smoother SSH and Dev Container workflows, migration paths for legacy servers, and WSL improvements.

## For Developers and Extension Authors

- New document paste APIs for extensibility and file comments.
- Proposed terminal completion provider API and support for identifying shell types.

## Learning More

For the full version and technical deep-dive, visit the [VS Code updates page](https://code.visualstudio.com/updates/v1_97).

---

## Acknowledgements

A detailed list of contributors' issue triaging, code commits, and extension support included in this release is available in the full article, with thanks to all community participants and external contributors.

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/updates/v1_97)
