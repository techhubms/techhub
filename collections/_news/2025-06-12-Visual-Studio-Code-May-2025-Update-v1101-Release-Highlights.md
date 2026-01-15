---
layout: post
title: 'Visual Studio Code May 2025 Update: v1.101 Release Highlights'
author: Visual Studio Code Team
canonical_url: https://code.visualstudio.com/updates/v1_101
viewing_mode: external
feed_name: Visual Studio Code Releases
feed_url: https://code.visualstudio.com/feed.xml
date: 2025-06-12 17:00:00 +00:00
permalink: /github-copilot/news/Visual-Studio-Code-May-2025-Update-v1101-Release-Highlights
tags:
- Accessibility
- Agent Mode
- AI
- Authentication
- Chat AI
- Coding
- DevOps
- Electron 35
- Extension Authoring
- GitHub Copilot
- Jupyter
- MCP
- MCP Servers
- News
- Notebook
- Pull Requests
- Python
- Remote Development
- Semantic Search
- Source Control
- V1.101
- VS Code
- VSCE
section_names:
- ai
- coding
- devops
- github-copilot
---
The Visual Studio Code Team shares the May 2025 v1.101 release, introducing MCP advances, agent coding flow upgrades, enhanced GitHub Copilot Coding Agent integration, new Python developer tools, and more impactful improvements.<!--excerpt_end-->

# Visual Studio Code May 2025 (v1.101) Release Notes

**Release Date:** June 12, 2025  
**Author:** Visual Studio Code Team

Learn more at [code.visualstudio.com/updates/v1_101](https://code.visualstudio.com/updates/v1_101)

## Key Highlights

### MCP and Agent Mode Enhancements

- **Model Context Protocol (MCP):** Expanded agent mode with support for prompts, resources, and sampling. Authenticate with external MCP servers, debug in development mode, and publish servers via extension APIs.
- **Tool Sets:** Define custom tool sets for agent mode chat, making specialized automation and chat-driven workflows easier to configure and use.
- **Resource Support:** Attach, browse, and manage resources from MCP servers directly within the chat.
- **Authentication Upgrades:** MCP servers now support advanced authentication including GitHub and Entra integration with user account management and device code flow.
- **Development Mode:** Debug Node.js/Python MCP servers with restart-on-change and attachable debugging.

### AI, GitHub Copilot, and Chat Innovations

- **Chat Tooling:** Define and use tool sets; improved UX for chat context, undo, and message role distinction. Add source control history into chat, customize context more easily, and navigate new attachment features.
- **Copilot Coding Agent:** Assign and track work for the Copilot agent, view session progress, and manage PRs via VS Code.
- **Next Edit Suggestions (NES):** Now supports automatic import suggestions in Python, JavaScript, and TypeScript with improved suggestion flow.
- **AI-Powered Settings Search:** Experimental semantic search for settings and search suggestions finds results by meaning, not just string matching.

### Python and Notebook Developer Experience

- **Python Chat Tools:** Integrated environment and package tool commands, environment configuration, and enhanced environment managers (pyenv, poetry) for rapid setup and scripting.
- **Notebook Agent Mode:** Agent has improved cell execution tracking, summary tools, and integration for seamless complex workflow management.
- **Project Templates:** Create Python projects with ready-to-go environments and scaffolding directly from the Python Environments extension.

### DevOps, Source Control, and Remote Development

- **Source Control Improvements:** Graph view enhancements, direct context addition from history, multi-file diff, and list/tree views.
- **Pull Requests Extension:** Copilot agent integration, notifications, direct PR/issue work, and in-app image/file viewing for private repos.
- **Remote Development:** SSH pre-connection scripting and streamlined explorer experiences for containerized/cloud development workflows.

### Accessibility and Editor Innovations

- **Sound Cues:** New audio signals for code actions and chat-required user actions.
- **Accessible Dialogs:** Richly labeled confirmation dialogs and alerts for screen reader users in agent mode.
- **Find-As-You-Type:** User-toggled setting for instant or manual search in the editor.
- **EditContext API:** Improved IME support and versatile input experiences now enabled on Stable builds.

### Platform and Extension Authoring

- **Secret Scanning:** VSCE now scans for sensitive information before extension publication, with customizable allowed secrets.
- **Web Environment Detection:** Updated extension host Node.js version with smart global navigator object migration tools for authors.
- **Extension APIs:** New MCP APIs for publishing, discovering, and authenticating MCP server collections.

### Engineering Updates

- **Electron 35 Update:** Brings the latest Chromium and Node.js upgrades for enhanced security, performance, and compatibility.
- **ESM Adoption:** Guidance and examples for adopting ECMAScript modules in extensions targeting NodeJS hosts.

### Notable Fixes & Contributor Acknowledgements

- Fixes include improvements to syntax highlighting, extension host reliability, PowerShell integration, notebook cell operations, and GitHub pull request workflows.
- [Full contributor credits and PR links in the release notes](https://code.visualstudio.com/updates/v1_101).

---

## Learn More & Download

- [Read the full release notes](https://code.visualstudio.com/updates/v1_101)
- Download the latest version for [Windows](https://update.code.visualstudio.com/1.101.2/win32-x64-user/stable), [Mac](https://update.code.visualstudio.com/1.101.2/darwin-universal/stable), and [Linux](https://update.code.visualstudio.com/1.101.2/linux-deb-x64/stable).

## About Visual Studio Code Team

The VS Code Team regularly delivers updates and innovations for developer productivity, AI integration, remote workflows, and extension building.

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/updates/v1_101)
