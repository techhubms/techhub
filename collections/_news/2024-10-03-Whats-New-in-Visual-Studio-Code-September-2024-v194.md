---
external_url: https://code.visualstudio.com/updates/v1_94
title: What's New in Visual Studio Code September 2024 (v1.94)
author: Visual Studio Code Team
feed_name: Visual Studio Code Releases
date: 2024-10-03 17:00:00 +00:00
tags:
- Accessibility
- Account Management
- Code Completion
- Copilot Chat
- Debugging
- Experimental Features
- Extension Management
- GPT 4o
- Notebook Editor
- OpenAI Integration
- Python
- Release Notes
- Semantic Search
- Source Control
- Testing Coverage
- TypeScript 5.6
- V1.94
- VS Code
section_names:
- ai
- coding
- devops
- github-copilot
---
The Visual Studio Code Team presents the September 2024 release (v1.94), packed with significant updates for developers including new features and improvements in GitHub Copilot, coding assistance, DevOps tools, and more. Authored by the Visual Studio Code Team.<!--excerpt_end-->

# What's New in Visual Studio Code September 2024 (v1.94)

**Author:** Visual Studio Code Team

## Overview

The September 2024 release of Visual Studio Code (v1.94) brings major updates for developer productivity, code intelligence, DevOps workflows, and extensibility. This comprehensive set of features includes enhancements to GitHub Copilot, core editing, testing, accessibility, and more.

---

## Highlights

- **Copilot in Native REPL**: Inline chat, code completions, and contextual chat improvements for Copilot.
- **Model Selection in Chat**: Choose language models (including GPT-4o) when using Copilot Chat.
- **Public Code Matching & Referencing**: Display and link to public code matches within Copilot Chat responses.
- **Better File Context in Chat**: Use drag-and-drop, filename suggestions, and improved file path rendering in Copilot.
- **Chat Context Attachments**: Add files and variables as context to improve relevance of AI outputs in chat, notebook, and REPL scenarios.
- **Test Setup and Debugging via Chat**: Experimental/preview commands to help generate and debug tests and launch configurations via Copilot Chat.
- **Accessibility Improvements**: Enhanced navigation, help dialogs, and keyboard support for the Editor and Comments.
- **Source Control Enhancements**: Interactive filtering, branch/tag actions, and repository picking in the Source Control Graph.
- **TypeScript 5.6 Support**: Updated language features, improved diagnostics, and smarter auto-import configuration.
- **Python Testing Coverage**: Run tests with coverage metrics, default problem matcher, and new shell integration in Python REPL.
- **Remote Development**: Attach to Kubernetes over SSH/Tunnel, enhanced Dev Container support, and GPU configuration.
- **Editor and Notebook Improvements**: Multi-cursor editing, improved diff viewing, and notebook serialization in web workers.
- **Engineering Core Updates**: ESM shipping for core VS Code, npm becomes default package manager, and fixes for high CPU and WSL port forwarding.
- **Community Contributions**: Acknowledgment of open-source contributors across VS Code and ecosystem repositories.

---

## GitHub Copilot Feature Updates

### Model Selection

- Select from OpenAI models (including early preview of new o1 models and GPT-4o).
- Enable model driving in chat sessions for tailored responses.

### Chat and Inline Improvements

- GPT-4o now powers Inline Chat for better code generation and explanations.
- Public code referencing: View and explore code suggestions that match public GitHub content.
- File suggestions: Quickly insert file context to Prompts with `#<filename>` auto-complete.
- File path rendering: Workspace file mentions now include icons and interactive context menus.
- Drag-and-drop: Add files as chat context by dragging editor tabs or file explorer items directly into chat.
- Persistent context: File and variable attachments are preserved across chat history, supporting more effective multi-step conversations.
- Python Inline Chat: Native REPL now supports advanced Copilot chat features and completions.

### Experimental and Preview Features

- **/fixTestFailure**: Use chat commands to diagnose and attempt fixes for failing unit tests.
- **/setupTests**: Get recommendations and automated setup for testing frameworks and configurations.
- **/startDebugging**: Generate or update debug configs and start sessions through chat prompts.
- **Semantic Search**: Enable Copilot-powered, semantically relevant code searches in the editor (preview).
- **Temporal Context**: Have Copilot consider recently modified files as part of session context.
- **Custom Instructions**: Use project-based markdown and settings to define code generation behavior (including testing instructions).

---

## Editor and Core Enhancements

- Improved inlay hint control and rendering in code editor.
- New settings for experimental edit context and composition.
- Switch from Yarn to npm as default package manager improves performance/security.
- ESM (ECMAScript Modules) are now used for all VS Code core components.

---

## Source Control Experience

- Expanded filtering, branch/tag creation, cherry-picking, and streamlined UI in the Source Control Graph.
- Badge, paging, and history settings for deeper customization.

---

## Python Tooling

- Run tests with coverage (pytest, coverage.py) and visual reporting in the Test Explorer.
- Default problem matcher for Python enhances task error tracking.
- Terminal shell integration and new Pylance Language Server modes for optimized performance.

---

## TypeScript Support

- Support for TypeScript 5.6 and experimental 5.7 preview.
- Improved diagnostics, commit character behavior, and auto-import exclusion patterns.

---

## Accessibility & Profiles

- Get Started Accessibility walkthroughs and improved focus for Comments and editor commands.
- Manage extension accounts across multiple GitHub/Microsoft identities.
- View and update extensions for all profiles in bulk.

---

## Notebooks & Debugging

- Multi-cursor editing between notebook cells.
- Metadata-aware diff views and performance improvements via serialization in web workers.
- Debug adapter protocol now supports colored/styled output for easier debugging.

---

## Remote Development

- Attach to Kubernetes containers natively via SSH/Tunnel.
- Configure GPU availability and improve WSL forward port handling.

---

## Engineering & Ecosystem

- Removal of V8 sandbox workarounds for extensions.
- Telemetry and documentation updates for affected extension authors.

---

## Open Source Contributions

- Dozens of open and merged PRs across VS Code's codebase and major plugins, with individual contributors acknowledged in the release notes.

---

## Further Reading

- [Full VS Code v1.94 Release Notes](https://code.visualstudio.com/updates/v1_94)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [Python in VS Code](https://code.visualstudio.com/docs/python/python-tutorial)
- [Remote Development in VS Code](https://code.visualstudio.com/docs/remote/remote-overview)

## Conclusion

This release targets improved coding productivity, seamless DevOps workflows, more capable AI assistance, and a stronger open-source ecosystem. Developers leveraging VS Code should explore new chat and AI features, try out experimental test/debugging commands, and take advantage of improved management for extensions, accounts, and workspace profiles.

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/updates/v1_94)
