---
external_url: https://code.visualstudio.com/blogs/2025/11/04/openSourceAIEditorSecondMilestone
title: VS Code Unifies Copilot AI Features in Open Source Extension
author: The VS Code team
viewing_mode: external
feed_name: Visual Studio Code Releases
date: 2025-11-06 00:00:00 +00:00
tags:
- AI Code Suggestions
- Copilot Chat
- Developer Tools
- Extension Development
- Ghost Text
- Inline Suggestions
- LLM Integration
- Open Source
- OpenAI
- Performance Optimization
- Prompt Engineering
- TypeScript
- VS Code
- VS Code Extensions
section_names:
- ai
- coding
- github-copilot
---
The VS Code Team shares how the GitHub Copilot Chat extension now unifies all Copilot AI code suggestions in a single open source extension, with technical details and contribution opportunities.<!--excerpt_end-->

# Open Source AI Editor: Second Milestone

**By The VS Code Team**

## Introduction

Visual Studio Code (VS Code) is advancing its journey toward becoming a fully open source AI-powered code editor. As of September's [Version 1.105](https://code.visualstudio.com/updates), the Copilot Chat extension now delivers all inline AI code suggestions—previously split between two extensions—in a unified, open source package. This marks the second major milestone in their open source AI initiative.

## Consolidation of Copilot Features

- **Single Extension Experience**: All Copilot functionality—inline suggestions, ghost text, chat, and agent mode—is now available via the Copilot Chat extension.
- **Deprecation Plan**: The old GitHub Copilot extension (previously responsible for inline ghost text) will be deprecated by early 2026 and removed from the VS Code Marketplace.
- **Transparent Transition**: Existing users should experience no major changes. Inline suggestions and chat features have been ported to the new unified extension with ongoing testing and progressive rollout.

## Technical Overview: How Inline Suggestions Work

Developers can now explore and contribute to the [vscode-copilot-chat repository](https://github.com/microsoft/vscode-copilot-chat/tree/main/src/extension/completions-core). Here's how the inline suggestion pipeline operates:

1. **Typing-as-suggested Detection**: Checks if the user is following a previous AI suggestion to avoid unnecessary computation.
2. **Caching**: Utilizes cached suggestions for performance, reducing repeated calls to language models.
3. **Reusing Ongoing Requests**: Leverages still-running LLM requests when possible to minimize latency.
4. **Prompt Construction**: Gathers relevant code and context to formulate prompts for the LLM.
5. **Model Inference**: Requests ghost text and next edit suggestions, prioritizing what's available at the cursor location.
6. **Post-processing**: Refines raw outputs for correct code style, indentation, and syntax.
7. **Multi-line Intelligence**: Determines how many lines to suggest based on context and confidence.

## Performance Improvements

- **Reduced Latency**: Networking optimizations provide faster AI code completions.
- **Quality Validation**: Rigorous experiments ensure no drop in performance or suggestion relevance during the migration.

## Community Involvement

- **Open Contributions**: The extension's codebase invites community PRs and bug reports through its [GitHub repository](https://github.com/microsoft/vscode-copilot-chat/pulls).
- **Iteration Plans**: Progress and upcoming work are available via [iteration plans](https://github.com/microsoft/vscode/issues?q=state%3Aopen%20label%3A%22iteration-plan%22).
- **Troubleshooting Options**: Users uncomfortable with the new unified setup can temporarily revert to prior extension behaviors.

## Future Direction

The next phase will refactor core AI functionality directly into VS Code's core, increasing extensibility and community involvement. Developers can expect ongoing improvements in AI-based development workflows within VS Code.

---

**Explore, contribute, and follow along with VS Code’s open source AI journey!**

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2025/11/04/openSourceAIEditorSecondMilestone)
