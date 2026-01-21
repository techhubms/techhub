---
external_url: https://code.visualstudio.com/updates/v1_95
title: "What's New in Visual Studio Code October 2024 (v1.95): Copilot, Extensions & Productivity"
author: Visual Studio Code Team
feed_name: Visual Studio Code Releases
date: 2024-10-29 17:00:00 +00:00
tags:
- AI Development
- Copilot Chat
- Copilot Edits
- Copilot Extensions
- Data Analysis
- Electron 32
- ESLint 9
- Mermaid Diagrams
- Open Source
- Pull Requests
- Pylance
- Python
- Semantic Search
- TypeScript 5.7
- V1.95
- VS Code
- VS Code Extensions
- Workspace Indexing
section_names:
- ai
- coding
- devops
- github-copilot
---
The Visual Studio Code Team unveils version 1.95, packed with Copilot improvements, AI-driven code review, extension APIs for LLMs, productivity upgrades, Python tools, and new ways to collaborate through open source contributions.<!--excerpt_end-->

# Visual Studio Code October 2024 (v1.95) Release Notes

## Overview

Visual Studio Code version 1.95 brings key upgrades for AI-assisted development, extensibility, DevOps, and productivity. This release highlights a deepened integration of GitHub Copilot features, enhanced chat workflows, productivity tools, new APIs for language model extensions, and security-focused updates.

---

## Security and Extension Updates

- **Security updates** for important extensions: [ms-python.python](https://marketplace.visualstudio.com/items?itemName=ms-python.python) and [ms-vscode-remote.remote-ssh](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-ssh).
- Updates 1.95.1, 1.95.2, and 1.95.3 address a range of issues ([details](https://github.com/microsoft/vscode/issues?q=is%3Aissue+milestone%3A%22October+2024+Recovery+1%22+is%3Aclosed)).
- Download latest builds for all major platforms (Windows, Mac, Linux) [here](https://code.visualstudio.com/updates).

---

## Key Copilot and AI Features

### GitHub Copilot Improvements

- **Copilot Edits:** Preview AI-powered multi-file code editing sessions. Quickly iterate with conversational chat plus inline suggestions. [Docs](https://code.visualstudio.com/docs/copilot/chat/copilot-edits)
- **Copilot Chat in Secondary Side Bar:** Keep AI chat open and ready, integrated with workspace navigation.
- **Copilot Code Reviews:** Preview AI-based code review on selections or full uncommitted changes—get actionable suggestions right in your editor.
- **Copilot Extensibility:** Showcase new APIs for adding AI-augmented tools (see extension authoring section).
- **Feedback Integration:** Share feedback through GitHub issues directly from the product.

### AI and Data-Centric Extensions

- **Mermaid Diagrams with Copilot:** Use chat to create and refine diagrams based on workspace code context.
- **Data Analysis for Copilot:** Analyze CSVs, generate graphs, run Python code in-browser, and auto-export to notebooks.
- **Vision for Copilot Preview:** Attach and analyze images in AI chat flows; generate/refine alt text for accessibility.

### Workspace and DevOps Enhancements

- **Workspace Indexing** for `@workspace` chat, progressive semantic search, and integration with GitHub Code Search.
- Expanded settings for code semantic search and participant detection in Copilot chats.
- **Multiple GitHub Accounts:** Seamless multi-account login for workflow flexibility (Settings Sync, PRs, Copilot usage).

---

## Editor, Language, and Productivity Updates

- **Pylance Docstring Generation:** Easily generate Python docstrings for documentation and code clarity.
- **Experimental AI Code Actions:** Implement abstract Python classes with Copilot and Pylance.
- **TypeScript 5.7 Support:** Improved preview for new features and automatic import handling on paste.
- **ESLint 9, Electron 32 Update:** Backend and performance improvements across the stack.
- **Workbench UI:** Enhanced icon support for profiles, panel view flexibility, show/hide labels, and new preview indicators for settings.
- **Web Platform:** Now reflects local file changes instantly when using supported browsers ([FileSystemObserver](https://chromestatus.com/feature/4622243656630272)).

---

## Extension Authoring and APIs

- **Language Model Tool API:** Register and share tools callable by LLMs (e.g., code search, Git history, workspace symbol extraction).
- **Participant Detection API:** Enable Copilot and extensions to route conversational requests to the relevant participant.
- **VS Code Speech Extension:** Uses newer Azure Speech SDK for improved speech-to-text AI features.
- Details: See [API docs](https://code.visualstudio.com/api/references/vscode-api).

---

## Notable Community and Open Source Contributions

- Dozens of community contributors fixed issues, added features, improved docs, and helped mature the VS Code ecosystem.
- Security and developer productivity driven by open source involvement—see full contributor lists above for details.

---

## Resources and Downloads

- [Official release notes](https://code.visualstudio.com/updates/v1_95)
- [VS Code download page](https://code.visualstudio.com/Download)
- [All VS Code extensions](https://marketplace.visualstudio.com/vscode)
- [Copilot documentation](https://code.visualstudio.com/docs/copilot/)
- [Extension authoring guides](https://code.visualstudio.com/api)

## Summary

Visual Studio Code 1.95 is a major step forward for AI-powered software development, extensibility, and modern DevOps workflows. From enhanced Copilot integrations to intelligent workspace tooling and open extension APIs, this release empowers developers to build, review, and ship code more productively than ever.

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/updates/v1_95)
