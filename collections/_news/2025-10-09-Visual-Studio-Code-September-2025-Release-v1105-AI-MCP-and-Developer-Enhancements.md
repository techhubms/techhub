---
external_url: https://code.visualstudio.com/updates/v1_105
title: 'Visual Studio Code September 2025 Release (v1.105): AI, MCP, and Developer Enhancements'
author: Visual Studio Code Team
viewing_mode: external
feed_name: Visual Studio Code Releases
date: 2025-10-09 17:00:00 +00:00
tags:
- Accessibility
- Agent Tools
- AI Assisted Development
- Authentication
- Chat Sessions
- Code Coverage
- Developer Productivity
- DevOps Tools
- Extensions
- MCP
- MCP Marketplace
- Merge Conflict Resolution
- OS Notifications
- Pull Requests
- Python
- Terminal Commands
- Testing
- VS Code
- VS Code 1.105
section_names:
- ai
- coding
- devops
- github-copilot
---
Visual Studio Code Team presents the September 2025 release (v1.105), introducing advanced AI integration, GitHub Copilot updates, agent tool improvements, and a new MCP marketplace to boost developer productivity and collaboration.<!--excerpt_end-->

# Visual Studio Code September 2025 Release (v1.105)

**Release Date:** October 9, 2025  
**Author:** Visual Studio Code Team

## Overview

The September 2025 update for Visual Studio Code (v1.105) delivers a broad range of new features and improvements focused on AI-assisted development, chat agents, developer productivity, authentication, testing, DevOps flows, and accessibility. Whether you're writing code, resolving merge conflicts, automating DevOps pipelines, or extending VS Code, the new capabilities in this version are designed to help you and your team work smarter and more efficiently.

For full details, visit the [official release notes](https://code.visualstudio.com/updates/v1_105).

---

## Major Highlights

### AI and Chat Improvements

- **AI-Powered Merge Conflict Resolution**: Use chat agents to resolve git merge conflicts directly in the editor with guidance and automation.
- **Enhanced Chat Sessions**: Improvements to managing multiple chat sessions with both local and remote coding agents, including session history, UI streamlining, and more robust OS notifications for responses and task completions.
- **Chat Mode Customization:** Support for nested `AGENTS.md` files for customized chat agent behavior in different workspace folders.
- **Chain-of-Thought Debugging:** Experimental feature shows reasoning tokens for AI chat responses, aiding transparency and debugging.
- **Improved Edit Tools for Custom Models:** Greater support for BYOK (Bring Your Own Key) OpenAI-compatible models and tailored tool sets in chat.

### GitHub Copilot & Agent Tools

- **GitHub Copilot Enhancements:** Expanded capabilities for Copilot Chat, custom model configuration (`github.copilot.chat.customOAIModels`), and support for Apple account sign-in.
- **Disallow Whitespace-only Edit Suggestions:** Avoid trivial next edit suggestions in Copilot (

`github.copilot.nextEditSuggestions.allowWhitespaceOnlyChanges`).

### Model Context Protocol (MCP) Marketplace

- **MCP Marketplace (Preview):** New built-in marketplace lets developers browse and install MCP servers from the Extensions view, powered by the GitHub MCP registry.
- **Autostart and Improved MCP server handling:** Automatic server startup and new capabilities for icons and tool/resource representations via MCP specification updates.

### DevOps and Source Control

- **Source Control Integration:** Improved file context for chat, better PR and issue support with the GitHub Pull Requests extension, and AI-powered merge resolution.
- **Terminal Improvements:** Enhanced terminal profile configuration, agent-specific shells, and auto-reply capabilities to speed up automated flows.
- **Task and Notification Upgrades:** OS notifications for long-running task completion, improved terminal title persistence, and easier navigation with shortcuts.

### Testing & Language Support

- **Run Tests with Code Coverage:** Chat agents can now access code coverage data via the `runTests` tool, allowing for comprehensive testing and validation.
- **Python-Test Enhancements:** New UI for copying test IDs, improving Python workflow within the editor.

### Security and Authentication

- **macOS Native Broker for Microsoft Authentication:** Modernizes sign-in experience for ARM-based Macs and Intune-enrolled devices, with fallback options and extended MSAL support.
- **GitHub PKCE Support:** Secure authentication flow for GitHub sign-in.
- **MFA Enforcement API:** Support for WWW-Authenticate claims challenges for extensions interacting with Azure resources.

### Accessibility, Customization, and Extensions

- **Screen Reader Support:** Better accessibility with improved announcements and view persistence; shell integration enhancements for `pwsh` under screen readers.
- **Quick Input Shortcut Customization:** Reassign keybindings for Quick Pick and Input Box controls.
- **Secret Storage API Enhancements:** Extensions can now list and manage secret storage keys programmatically.

---

## Detailed Changes

### AI-Powered Productivity

- Merge conflict resolution, task completion, and code reviews are now more efficient with automated AI and agent capabilities.
- Copilot Chat continues to expand with more customization, agentic flows, and direct support for multiple language models (

GPT-5-Codex, Claude Sonnet 4.5).

- Chain-of-thought reasoning and session-based chat history simplify code understanding and debugging.

### DevOps and Automation

- Better CI/CD integration with improved task completion feedback, agent loops, and code/action automations.
- Terminal settings now allow for separation of agent shells and user environments.
- Source control graph and testing interfaces are enhanced for rapid feedback and traceability.

### MCP and Extension Ecosystem

- Extension developers gain new APIs for secret storage, MFA claims challenges, chat prompt/instructions contributions, and better test/telemetry integration.
- MCP servers can now expose custom icons, provide default values for elicitation, and be more seamlessly deployed across teams through the new marketplace preview.

### Contributor Acknowledgments

- The team recognizes dozens of external contributors for pull requests across VS Code and its critical extensions, reflecting a vibrant, collaborative open-source community.

---

## Learn More & Resources

- [VS Code 1.105 Release Notes](https://code.visualstudio.com/updates/v1_105)
- [Copilot in VS Code documentation](https://code.visualstudio.com/docs/copilot/overview)
- [GitHub MCP registry](https://github.com/mcp)
- [Azure Resources extension guide](https://github.com/microsoft/vscode-azureresourcegroups)
- [VS Code Issue Tracker](https://github.com/microsoft/vscode/issues)

Stay updated by exploring prior release notes at [Updates](https://code.visualstudio.com/updates).

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/updates/v1_105)
