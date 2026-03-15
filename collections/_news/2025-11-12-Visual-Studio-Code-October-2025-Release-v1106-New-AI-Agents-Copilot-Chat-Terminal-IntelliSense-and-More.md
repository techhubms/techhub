---
external_url: https://code.visualstudio.com/updates/v1_106
title: 'Visual Studio Code October 2025 Release (v1.106): New AI Agents, Copilot Chat, Terminal IntelliSense & More'
author: Visual Studio Code Team
feed_name: Visual Studio Code Releases
date: 2025-11-12 17:00:00 +00:00
tags:
- Agent HQ
- AI Agents
- Code Editing
- Copilot Chat
- Copilot CLI
- Custom Agents
- Editor Experience
- Extension Authoring
- Open Source
- Pylance
- Python
- Source Control
- Terminal IntelliSense
- VS Code
- VS Code 1.106
- AI
- DevOps
- GitHub Copilot
- News
- .NET
section_names:
- ai
- dotnet
- devops
- github-copilot
primary_section: github-copilot
---
The Visual Studio Code Team details the October 2025 (v1.106) release, featuring major upgrades to Copilot Chat, the AI agent platform, coding tools, terminal experiences, and extensibility for developers.<!--excerpt_end-->

# Visual Studio Code October 2025 Release (v1.106)

*Author: Visual Studio Code Team*

## Overview

The October 2025 release of Visual Studio Code (version 1.106) delivers a significant suite of new features for developers, especially those leveraging Microsoft's AI, DevOps, and productivity platforms. Highlights include a robust AI agent infrastructure, Copilot Chat and CLI enhancements, richer coding and source control features, and expanded tools for extension and workflow authors.

---

## Major Highlights

### 1. **AI Agents & Copilot Platform**

- **Agent HQ**: Unified dashboard to launch, monitor, and review agent sessions (local, Copilot, Codex, or CLI agents).
- **Plan Agent**: Interactive agent for breaking down and planning complex development tasks before generating code—actions can be iteratively approved.
- **Custom Agents**: Build, manage, and extend custom agent workflows with new frontmatter options for fine-tuned integration across VS Code and GitHub Copilot.

### 2. **Copilot Chat & CLI Integration Enhancements**

- **Copilot CLI**: Now integrates tightly within VS Code. Sessions can be managed as first-class citizens in the chat interface or terminal, with tracking of edits and delegation.
- **Cloud Agent Improvements**: Native support for opening and interacting with Copilot coding agents through cloud features and GitHub Mission Control.
- **Open Source Milestone**: Inline suggestions are now open source, and Copilot Chat/extension experiences are unified for consistency and transparency.
- **Chat Modes Renamed**: 'Chat modes' are now 'Custom Agents'; legacy files automatically migrate.

### 3. **Editor & Productivity Features**

- **Inline Diff Copy**: Select and copy deleted code in diff editors for more flexible code reviews.
- **Refreshed Iconography**: More modern and legible codicon icons.
- **Advanced Settings**: Access to hidden, advanced features and configuration—for expert users and extension authors.
- **Terminal IntelliSense**: Powerful, extensible command suggestions for major shells (PowerShell, bash, zsh, fish). Customizations and integration improvements included.

### 4. **DevOps & Source Control**

- **Source Control Graph**: Visualize and compare incoming/outgoing changes, fold commit messages, and fine-tune repository views for multi-repository setups.
- **Expanded GitHub Pull Requests Support**: AI-generated PR descriptions, enhanced diff navigation, and draft handling.

### 5. **Security & Trust Improvements**

- **Tool Approvals**: Post-approval for tools accessing external data (preventing prompt injections) and support to trust entire servers/extensions.
- **Terminal Tool Security**: Enhanced shell command parsing, detection of file writes, and experimental auto-approval rules.

### 6. **Language & Testing Support**

- **Python**: docstring creation from Copilot summaries, improved virtual environment creation, and explicit import refactoring.
- **Notebooks & Coverage Navigation**: Enhanced search and navigation of uncovered lines in test coverage.

### 7. **Extension Authoring & APIs**

- **Markdown Alerts & Labels**: Render GitHub-style alerts and use markdown in tree view labels for richer UI components.
- **Secondary Sidebar**: Place custom views alongside built-in chat experiences.

---

## Reference Links

- [Full Release Notes](https://code.visualstudio.com/updates/v1_106)
- [Copilot Coding Agent Documentation](https://code.visualstudio.com/docs/copilot/copilot-coding-agent)
- [GitHub Copilot CLI](https://github.com/features/copilot/cli/)
- [Agent Sessions in VS Code](https://code.visualstudio.com/docs/copilot/chat/chat-sessions/#_agent-sessions-view)
- [Open Source Copilot Blog Post](https://code.visualstudio.com/blogs/2025/11/04/openSourceAIEditorSecondMilestone)

---

## Key Upgrades in Detail

### AI & Copilot Chat

- Improved unified UI for engaging with AI-powered chat and agents inside VS Code.
- End-to-end edit tracking for Copilot CLI sessions and agent delegations from both UI and terminal.
- Option to save AI conversations as reusable prompts or edit welcome prompts for consistency in workflow.
- Integration of cloud-based Copilot agents and streamlined delegation for complex, multi-step coding operations.

### Developer Productivity

- Enhanced code navigation, folding, Git commit management, and advanced diff navigation for teams.
- Stability improvements in notebook search, code folding, and command palette accent-insensitive filtering.
- Diagnostic hover copy and multi-file diff navigation.

### DevOps & Source Control

- Intuitive repository selection, visualization, and comparison tools.
- Improved GitHub pull request management, including AI-powered PR description drafts and advanced branch controls.

### Extension & API Surface

- Support for new authentication workflows, Quick Pick enhancements, markdown alerts, and custom icons for refined UI development.
- Tools to manage MCP servers, account preferences, and workspace-specific installations, supporting enterprise policy and multi-user collaboration.

### Security & Admin

- Post-approval controls for agent tools that access external data, trust management for MCP servers and extensions.
- Shell integration timeout consolidation and improved terminal integration settings for safer command execution.

---

## Contributing & Community Recognition

Numerous community contributions are credited, spanning code fixes, feature enhancements, localization, and documentation, reflecting the commitment of Microsoft and the wider OSS community.

> For a complete list of contributors and pull requests: see the original release notes linked above.

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/updates/v1_106)
