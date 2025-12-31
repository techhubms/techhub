---
layout: "post"
title: "Visual Studio Code February 2025 (v1.98) Release Highlights"
description: "The February 2025 (1.98) update to Visual Studio Code brings significant improvements focused on developer productivity and AI-powered workflows. Major advancements include enhanced GitHub Copilot features, expanded agent mode, notebook editing, terminal IntelliSense, TypeScript 5.8 integration, and a smoother authentication experience. The release also delivers UI refinements, accessibility improvements, and a wide range of bug fixes and enhancements for core VS Code workflows. This update enables deeper, more intelligent code assistance and streamlines daily development tasks."
author: "Visual Studio Code Team"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://code.visualstudio.com/updates/v1_98"
viewing_mode: "external"
feed_name: "Visual Studio Code Releases"
feed_url: "https://code.visualstudio.com/feed.xml"
date: 2025-03-05 17:00:00 +00:00
permalink: "/news/2025-03-05-Visual-Studio-Code-February-2025-v198-Release-Highlights.html"
categories: ["AI", "Coding", "DevOps", "GitHub Copilot"]
tags: ["Accessibility", "AI", "Authentication", "Claude 3.7 Sonnet", "Coding", "Copilot Agent Mode", "Copilot Chat", "Copilot Edits", "DevOps", "GitHub Copilot", "GPT 4.5", "News", "Notebook Editing", "Python", "Release Notes", "Remote Development", "Terminal IntelliSense", "TypeScript 5.8", "V1.98", "VS Code"]
tags_normalized: ["accessibility", "ai", "authentication", "claude 3dot7 sonnet", "coding", "copilot agent mode", "copilot chat", "copilot edits", "devops", "github copilot", "gpt 4dot5", "news", "notebook editing", "python", "release notes", "remote development", "terminal intellisense", "typescript 5dot8", "v1dot98", "vs code"]
---

The Visual Studio Code Team delivers the February 2025 (v1.98) release with major updates by introducing advanced GitHub Copilot features, new AI-driven workflows, and core improvements, enhancing developer productivity and coding experience.<!--excerpt_end-->

# Visual Studio Code February 2025 (v1.98) Release Highlights

**Author: Visual Studio Code Team**

Welcome to the February 2025 (version 1.98) release of Visual Studio Code! This update is packed with productivity-enhancing features, powerful AI capabilities, and UX refinements for developers working across languages and platforms.

## Key Feature Highlights

### GitHub Copilot & AI Enhancements

- **Next Edit Suggestions (Preview):** Copilot predicts the next possible code edit based on your current context.
- **Agent Mode (Preview):** Copilot can now autonomously complete multi-step tasks, search your workspace, edit files, and execute terminal commands.
- **Copilot Edits for Notebooks:** Edit Jupyter and other notebooks directly using Copilot's AI-driven suggestions.
- **Copilot Vision (Preview):** Attach images and interact with them in chat prompts, using models like GPT-4o.
- **Model Selection:** Support for new AI models including GPT-4.5 and Claude 3.7 Sonnet, with model selection available for completions and chat.
- **Codebase Search in Chat:** Enhanced search tools help Copilot find and reference relevant files and symbols during chat sessions.
- **Custom Instructions:** Workspace-level `.github/copilot-instructions.md` enables tailored code suggestions and chat responses.
- **Smoother Authentication:** Improved flows for linking GitHub repositories and managing permissions.

### Editor & UX Improvements

- **Terminal IntelliSense:** Smarter command completions and argument suggestions for Bash, Zsh, Fish, and PowerShell terminals.
- **Drag-and-Drop References:** Seamlessly open peek references in new editors.
- **Notebook Inline Diff View:** Preview and compare notebook changes inline.
- **Custom Title Bar on Linux:** Custom title bar is now enabled by default.
- **Accessibility:** Enhanced support with audio signals and a more accessible diff viewer for Copilot Edits.

### Language and Tooling Updates

- **TypeScript 5.8.2:** Integrated for improved language features and support.
- **Python Improvements:** Updates to Pylance for automatic quotation insertion, memory use, and shell integration.
- **Remote Development:** Expanded proxy rules and end-of-life for Linux legacy server.
- **Debugging:** Inline hover for debug values improves variable readability.

### Source Control & DevOps

- **Source Control View Titles:** Shortened and clarified for better usability.
- **Soft-delete for Untracked Files:** Move untracked files to trash instead of deleting.
- **Diagnostics Commit Hook:** Experimental prompt for committing with unresolved diagnostics.

### Contributor Recognition

- Dozens of open-source contributors have improved VS Code core, extensions, and the ecosystem, as recognized in the full release notes.

## Getting Started

- Download and view full release details at [Visual Studio Code Updates](https://code.visualstudio.com/updates/v1_98).
- Explore new experimental features in VS Code Insiders builds for early access.

---

## Detailed Release Breakdown

### 1. GitHub Copilot & Agent Mode

**Agent Mode** allows Copilot to take over more complex developer workflows:

- Finds context in the workspace, edits code, checks for errors, and runs commands.
- Allows stepwise control: you can approve, edit, or undo actions inline.
- Includes notebook file support and now lifts interaction/file limits.
- Additional AI models (such as GPT-4.5 and Claude 3.7 Sonnet) are available, and custom instructions allow deeper personalization.

### 2. Editor and UI Improvements

- **Terminal IntelliSense** supplies more accurate CLI auto-complete for common shells and CLIs, including dynamic completion generators (e.g., `git branch`).
- **Drag & Drop for Peek References:** Quickly navigate code structure.
- **Inline notebook diff view:** Inline and side-by-side comparisons for notebook content.
- **Accessibility:** Copilot Edits provide audio cues and accessible diffs.
- **Custom Title Bar (Linux):** Now default, but configurable.

### 3. Language & Extension Updates

- **TypeScript:** Updated to 5.8.2 with improved type checking and support for Node options.
- **Python (Pylance):** Better memory use, instant shell integration, more flexible test discovery, and debug config.
- **Remote Development:** EOL for Linux legacy server; improved support for proxy scenarios.
- **Debug:** Rich inline value hovers for debugging.

### 4. Source Control & DevOps

- **Soft Delete:** Untracked changes go to trash for easy recovery.
- **Diagnostics Hook:** Prompts about unresolved diagnostics before commit.

### 5. Release Process & Community

- Many developer contributions are acknowledged in bug fixes, documentation, and enhancements across the core and extensions.

---

## Learn More

- [Full Release Notes](https://code.visualstudio.com/updates/v1_98)
- [Known Issues](https://github.com/microsoft/vscode/issues)
- [GitHub Copilot in VS Code](https://code.visualstudio.com/docs/copilot/overview)

Stay updated by joining the VS Code [Insiders](https://code.visualstudio.com/insiders) program for early feature previews and deeper AI integrations.

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/updates/v1_98)
