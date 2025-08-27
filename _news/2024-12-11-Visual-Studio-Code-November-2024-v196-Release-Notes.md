---
layout: "post"
title: "Visual Studio Code November 2024 (v1.96) Release Notes"
description: "The November 2024 release of Visual Studio Code (version 1.96) introduces a free GitHub Copilot plan, Copilot Edits improvements, new debugging capabilities, enhanced TypeScript and Python tools, and various productivity updates. This summary highlights Copilot integration, coding enhancements, DevOps workflows, and editor improvements for developers."
author: "Visual Studio Code Team"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://code.visualstudio.com/updates/v1_96"
viewing_mode: "external"
feed_name: "Visual Studio Code Releases"
feed_url: "https://code.visualstudio.com/feed.xml"
date: 2024-12-11 17:00:00 +00:00
permalink: "/2024-12-11-Visual-Studio-Code-November-2024-v196-Release-Notes.html"
categories: ["AI", "Coding", "DevOps", "GitHub Copilot"]
tags: ["Accessibility", "AI", "AI Assisted Development", "Coding", "Copilot Chat", "Copilot Debug", "Copilot Edits", "Debugging", "DevOps", "DevOps Workflows", "Editor Improvements", "Extension Management", "GitHub Copilot", "GitHub Copilot Free", "News", "Notebooks", "Python Environments", "Python Testing", "Remote Development", "Terminal Enhancements", "Test Coverage", "TypeScript 5.7", "VS Code", "VS Code Extension"]
tags_normalized: ["accessibility", "ai", "ai assisted development", "coding", "copilot chat", "copilot debug", "copilot edits", "debugging", "devops", "devops workflows", "editor improvements", "extension management", "github copilot", "github copilot free", "news", "notebooks", "python environments", "python testing", "remote development", "terminal enhancements", "test coverage", "typescript 5dot7", "vs code", "vs code extension"]
---

The Visual Studio Code Team details the November 2024 (v1.96) release, highlighting a new free GitHub Copilot plan, AI-powered coding enhancements, debugging improvements, and productivity features for developers.<!--excerpt_end-->

# Visual Studio Code November 2024 (v1.96) Release Highlights

The November 2024 update to Visual Studio Code brings a broad set of enhancements to improve the developer experience, especially for those using AI-assisted tooling with GitHub Copilot. Below you'll find an overview of key features, Copilot updates, and development productivity highlights from this release.

## GitHub Copilot and AI Features

### GitHub Copilot Free Plan

- A new free tier is available for GitHub Copilot, allowing users with a GitHub account to leverage code completions and chat monthly quotas.
- Sign-up is accessible directly from within VS Code, streamlining onboarding for developers.
- Read more: [Copilot Free plan details](https://docs.github.com/en/copilot/about-github-copilot/subscription-plans-for-github-copilot).

### Copilot Edits Improvements

- Rapidly edit multiple files at once with natural language commands using Copilot Edits (preview feature).
- Enhanced progress visibility, streamlined change review controls, and the ability to move chat sessions into Copilot Edits for batch suggestions.
- Improved file suggestions and working set management, including context addition from explorer and search views.
- Session restore support: edit sessions are now maintained after VS Code restarts.

### Copilot Debugging Support

- The `copilot-debug` terminal command enables Copilot-assisted debugging for any project or language. Example usage: `copilot-debug python foo.py`.
- After execution, easily save or regenerate launch configurations and rerun sessions.
- Includes dynamic pre-launch task generation for compiled languages (Rust, C++, etc.).

### Copilot Chat and Context Features

- Symbols and folders can now be added as chat or edit context with simple drag-and-drop from across the editor.
- Global symbol picker and improved context management for larger projects.

### Copilot Usage Metrics

- New graph in Runtime Status view shows Copilot extension chat usage over time, aiding in extension insight and troubleshooting.

### Custom Instructions for Commit Messages

- Specify format or content guidelines for Copilot-generated commit messages using workspace or user settings.

### Inline and Terminal Chat Enhancements

- Improved hinting, accessibility, and UI in inline chat—including pseudo-code recognition and context suggestions.
- Terminal inline chat now matches the editor chat appearance and usability.

## Editor and Coding Productivity

- New overtype mode in the editor (toggle with Insert key): easier editing of markdown tables and fixed-width content.
- JavaScript/TypeScript "Paste with Imports": automatically adds missing imports when pasting code.
- Persisted editor find history across sessions and workspaces for better search continuity.

## DevOps, Testing, and Extension Authoring

- Attributable test coverage: filter code coverage by individual test, with extension API support.
- Source Control improvements, including experimental Git blame decorations and updated push/pull workflow.
- Configurable extension allow-list for enterprises and support for VS Code installations with preselected extensions.

## Language Support and Tooling

### TypeScript 5.7

- Updated built-in support brings new language features, expanded hover info, and bug fixes.

### Python Updates

- Python Environments extension (preview): graphical UI for managing Python environments and packages.
- Testing rewrite and REPL improvements; dynamic pytest rootdir settings and better test debug session handling.
- Pylance "full" IntelliSense mode for more complete code analysis (with associated performance tradeoffs).

## Remote Development and Accessibility

- Improved experience for remote containers, SSH, and WSL.
- More robust accessibility signals for code actions and REPL execution focus controls.

## Additional Improvements

- Enhanced extension disk usage information, direct download support in the marketplace.
- Terminal ligature support, new link formats, and keyboard shortcuts for notebook editing.
- Start of GPU acceleration groundwork in the VS Code editor, aiming at lower latency and improved performance.
- Deprecation notice for macOS Catalina support alongside the upcoming Electron 33 update.

## Community Contributions

The VS Code team recognizes extensive community contributions to this release, in areas including extension development, bug fixes, Python/Jupyter enhancements, and developer ergonomics.

---
For the complete detailed release notes, visit the [Visual Studio Code 1.96 updates page](https://code.visualstudio.com/updates/v1_96).

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/updates/v1_96)
