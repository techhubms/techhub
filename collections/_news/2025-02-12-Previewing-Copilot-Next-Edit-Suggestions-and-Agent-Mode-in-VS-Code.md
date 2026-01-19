---
external_url: https://code.visualstudio.com/blogs/2025/02/12/next-edit-suggestions
title: Previewing Copilot Next Edit Suggestions and Agent Mode in VS Code
author: Brigit Murtaugh, Burke Holland
viewing_mode: external
feed_name: Visual Studio Code Releases
date: 2025-02-12 00:00:00 +00:00
tags:
- Agent Mode
- AI Code Completion
- AI Pair Programming
- Code Refactoring
- Copilot Chat
- Editing Assistance
- Editor Automation
- GPT 4o
- Next Edit Suggestions
- Preview Features
- Vision Feature
- VS Code
- VS Code Extensions
section_names:
- ai
- coding
- github-copilot
---
Brigit Murtaugh and Burke Holland introduce developers to the latest AI-driven enhancements in GitHub Copilot for Visual Studio Code, including Next Edit Suggestions, Agent Mode, and Vision capabilities.<!--excerpt_end-->

# Copilot Next Edit Suggestions (preview)

*By Brigit Murtaugh and Burke Holland*

## Overview

February 12, 2025 — This news post presents three preview features for GitHub Copilot in Visual Studio Code: **Next Edit Suggestions (NES)**, **Agent Mode for Copilot Edits**, and **Vision**.

These features help developers by providing AI-powered code completions, context-aware edit recommendations, error correction, and multi-modal capabilities right inside VS Code.

## What’s New in This Release

### 1. Next Edit Suggestions (NES)

GitHub Copilot now expands beyond just autocompleting new code—NES can suggest edits to existing code. It identifies opportunities for fixes, refactoring, and intent-driven changes, ranging from correcting a typo (such as `conts` to `const`) to larger logical mistakes (e.g., catching a flawed conditional statement).

#### How to Enable NES

- Turn on the VS Code setting `github.copilot.nextEditSuggestions.enabled`.
- Restart VS Code for settings to take effect.
- Admin opt-in is required for Business/Enterprise users.

When a suggestion is available, an arrow in the code gutter appears with menu options and keyboard shortcuts. You can navigate and accept suggestions with the Tab key.

#### Example Scenarios

- Catching and correcting coding mistakes, even logic errors (like wrong use of logical operators)
- Adapting code to new intentions (e.g., updating a class structure or method signature)
- Propagating new variable and logic usage throughout your codebase
- Refactoring names and patterns for consistency

Read the [NES documentation](https://aka.ms/gh-copilot-nes-docs) for more examples and details.

### 2. Agent Mode

Agent Mode lets Copilot proactively iterate on code, resolve errors, and run terminal commands. It acts more autonomously, completing tasks beyond just what’s initially requested. Features include:

- Automatic error detection and fixing
- Self-healing behaviors
- Command execution in the integrated terminal

This capability is currently available in VS Code Insiders and will expand to VS Code Stable in future releases. Learn more in the [Agent Mode documentation](https://code.visualstudio.com/docs/copilot/chat/copilot-edits#_use-agent-mode-preview).

### 3. Vision

With Vision, Copilot can now process images as part of your prompts. You can:

- Attach screenshots or images (e.g., UI layouts or error screens) to help Copilot provide better context-aware suggestions
- Enable new workflows like generating UI code from mockups or offering layout corrections
- Supported formats: JPEG/JPG, PNG, GIF, WEBP
- Only available with GPT 4o for now

Vision is live in VS Code Insiders. Paste, drag-and-drop, or attach screenshots to use it.

### Feedback and Resources

- Share your feedback via [GitHub issues](https://github.com/microsoft/vscode/issues)
- Use [VS Code Insiders](https://code.visualstudio.com/insiders/) and pre-release Copilot Chat extension for early access
- [Read the full NES docs](https://aka.ms/gh-copilot-nes-docs) for more information
- Stay up to date via [VS Code's release notes](https://code.visualstudio.com/updates) and on [social media](https://x.com/code)

## Summary

These AI-driven enhancements aim to make Copilot an ever-smarter coding partner in VS Code, not just by suggesting code, but by anticipating developer intent, facilitating edits, and broadening Copilot’s capabilities to visual contexts. Developers are encouraged to try these features, provide feedback, and explore how workflow efficiency and coding quality can improve with this evolving set of Copilot tools.

---

*For more details, see the official [blog announcement](https://code.visualstudio.com/blogs/2025/02/12/next-edit-suggestions).*

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2025/02/12/next-edit-suggestions)
