---
external_url: https://code.visualstudio.com/updates/v1_93
title: Visual Studio Code August 2024 (v1.93) Release Highlights
author: Visual Studio Code Team
feed_name: Visual Studio Code Releases
date: 2024-09-05 17:00:00 +00:00
tags:
- Accessibility
- August Release
- Code Generation
- Debugging
- Django
- ECMAScript Modules
- Extensions
- Git
- IntelliSense
- JavaScript
- Python
- Remote Development
- Settings
- Source Control
- Terminal Integration
- TypeScript 5.6
- Unit Testing
- V1.93
- VS Code
- AI
- Coding
- DevOps
- GitHub Copilot
- News
section_names:
- ai
- coding
- devops
- github-copilot
primary_section: github-copilot
---
The Visual Studio Code Team details the August 2024 (v1.93) update, highlighting new features in GitHub Copilot, DevOps workflows, code editing, and community contributions.<!--excerpt_end-->

# Visual Studio Code August 2024 (v1.93) Release Highlights

**Author:** Visual Studio Code Team

[Full release notes](https://code.visualstudio.com/updates/v1_93)

---

## Overview

The August 2024 (v1.93) update for Visual Studio Code provides a host of improvements relevant to software development, DevOps, and AI-assisted coding.

### Key Feature Highlights

- **GitHub Copilot Enhancements:**
  - Improved test and documentation generation via Copilot.
  - More intuitive chat history and AI-generated naming.
  - Context attachment in Quick Chat for smarter completions.
  - Experimental settings for custom code-generation instructions and start debugging from chat.
  - Recent files can now be used as context for inline chat, increasing code suggestion relevance.
- **Accessibility:**
  - Resize editor columns by keyboard.
  - Enhanced screen reader support for chat.
  - Improved debug accessibility dialogs.
- **Editor Experience:**
  - Profiles editor now generally available for managing developer profiles.
  - Lightbulb and code action improvements.
  - Sort and manage comments by position or date.
- **DevOps and Source Control:**
  - Source Control Graph moved to a dedicated view.
  - Support for new Git 'reftable' storage backend.
  - Reverse sort order in Explorer.
  - SCM compact folders setting for clearer changes view.
- **Language Support:**
  - Django unit test discovery and execution for Python via Test Explorer.
  - Renamed SQL to 'MS SQL' in recognition of Microsoft SQL Server dialects.
  - Enhanced IntelliSense for TypeScript and JavaScript, improved project-wide navigation, and full package typings on vscode.dev.
- **Remote and Terminal:**
  - Expanded Remote Development templates/compatibility.
  - Linux window control overlays support.
  - Julia and NuShell terminal integration support.
- **Debugging & Testing:**
  - Experimental JavaScript Network view.
  - Stack traces on test failure messages and fast navigation from test errors.
  - Caching last input values for launch configs.
- **Extension Authoring & APIs:**
  - New terminal shell integration API for richer extension interaction.
  - Authentication account API improvements for managing multiple accounts.
  - Preview and Proposed APIs for language models, testing, and chat participants.
- **Community and Fixes:**
  - Recognition of key contributors.
  - Numerous bug fixes and enhancements across all major features.

## GitHub Copilot & AI Features

- More robust **test and documentation generation** directly from the editor using Copilot's updated commands.
- Custom instructions can be specified for Copilot, influencing coding pattern generation.
- Faster access to chat history, including session labeling and date grouping.
- Experimental detection of chat participants and use of recent code files for context.
- New debugging integration: Start debugging sessions from chat prompts.
- Enhanced feedback collection for chat responses, streamlining Copilot improvement.

## DevOps, Source Control, and Coding Experience

- Source Control Graph view separates commit and branch visualization for clarity.
- Enhanced Git integration with reftable support and Git 2.45+, including easier repository migration.
- Improvements in Terminal usability, including Julia/NuShell support and moveable terminal groups.
- Full ECMAScript Modules (ESM) transition for VS Code Core, providing faster and more modern extension development.

## Python & Django Unit Testing

- Python extension now supports Django unit test discovery and execution through the Test Explorer.
- Improved native REPL and debugging experiences with Pylance integrations.
- Fast navigation between type definitions and code via inlay hints.

## Remote Development & OS-specific Features

- Expanded Dev Container template paths for improved customization.
- Enhanced compatibility for various remote OS environments via SSH.
- Linux window control overlay functionality empowers a more native look and feel.

## User Experience and Accessibility

- Significant upgrades to accessibility, especially for keyboard, screen reader, and debug workflows.
- Copy direct settings URLs and sort comments in the UI for stronger navigation and collaboration.

## Extension & API Updates

- Shell integration API: Extensions gain capabilities to listen, control, and gather terminal command output.
- Authentication APIs for managing multiple account sessions, particularly Microsoft and GitHub logins.
- Proposed APIs for language model tool integration and code-to-test associations.

## Additional Features & Fixes

- Better notebook diffing: Hide unchanged cells for focused review.
- TypeScript 5.6 preview support through dedicated extension.
- Conpty backend for improved terminal reliability on Windows.
- Full changelog includes extensive bug fixes and minor enhancements contributed by the global open source community.

## Community Contributions

A large number of external contributors participated in bug fixes, extension improvements, and language support updates—see the release notes' “Thank you” section for details and attributions.

---

For more details on all changes, visit the [official release notes](https://code.visualstudio.com/updates/v1_93).

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/updates/v1_93)
