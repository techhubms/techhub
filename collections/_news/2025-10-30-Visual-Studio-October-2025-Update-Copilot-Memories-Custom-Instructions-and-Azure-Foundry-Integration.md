---
external_url: https://devblogs.microsoft.com/visualstudio/visual-studio-october-update/
title: 'Visual Studio October 2025 Update: Copilot Memories, Custom Instructions, and Azure Foundry Integration'
author: Jessie Houghton
viewing_mode: external
feed_name: Microsoft VisualStudio Blog
date: 2025-10-30 12:00:36 +00:00
tags:
- Agentic Workflows
- AI Integration
- Azure Foundry
- Chat Management
- Claude Haiku 4.5
- Claude Sonnet 4.5
- Coding Standards
- Copilot Memories
- Custom Instructions
- Developer Tools
- Instruction Files
- Planning Feature
- Project Aware AI
- V17.14
- VS
section_names:
- ai
- azure
- coding
- github-copilot
---
Jessie Houghton details the new features in Visual Studio 2022's October 2025 update, focusing on expanded Copilot capabilities, model choices, planning, and Azure Foundry integration for developers.<!--excerpt_end-->

# Visual Studio October 2025 Update: New Models, Memories, Planning, and More

**Author:** Jessie Houghton  
**Version:** Visual Studio 2022 v17.14

## Overview

The October 2025 Visual Studio 2022 update (v17.14) delivers major enhancements for developers working with AI-assisted tooling, especially through GitHub Copilot. With new model integrations, expanded Copilot features, and Azure compatibility, it targets personal and team productivity in codebases of all sizes.

## New AI Models

In this release, Visual Studio Copilot Chat now includes [Claude Sonnet 4.5](https://developercommunity.visualstudio.com/t/Add-Claude-Sonnet-45-Integration-for-Vi/10974906) and Claude Haiku 4.5 as model choices. These additions expand your options when choosing an AI assistant, supporting more advanced agentic workflows directly within the chat window.

## Copilot Memories: Project-Aware Assistance

Copilot memories enable the AI to learn about your project's coding standards and your team's best practices. As you interact in chat:

- **Detection:** Copilot recognizes when you correct its code, define standards, or make requests for it to remember preferences.
- **Documentation:** It offers to save preferences to:
  - `.editorconfig` (for coding standards)
  - `CONTRIBUTING.md` (guidelines, practices)
  - `README.md` (project overview)
- **Team Impact:** Preferences are shared, helping maintain consistency and improve Copilot responses for the whole team.

## Built-in Planning for Complex Tasks

Copilot Chat now provides built-in planning for multi-step workflows:

- **Automatic Plan Files:** When facing a large problem, Copilot creates a markdown file listing tasks, targeted files, and its intended approach.
- **Real-Time Updates:** Progress, blockers, and strategy adjustments are tracked live.
- **Visibility:** Plans remain transparent and are scoped to your specific development goals. Plans are temporary unless you explicitly save them to the repo.

[Learn more about planning](https://devblogs.microsoft.com/visualstudio/introducing-planning-in-visual-studio-public-preview/)

## Custom Instruction Files

You can now fine-tune Copilot's behavior at the project or folder level using instruction files:

- **Activation:** Enable via Tools > Options > GitHub > Copilot > Copilot Chat.
- **Setup:** Place `.instructions.md` files in `.github/instructions/` and apply to source code subsets using the `applyTo` header with glob patterns (e.g., `applyTo: "src/**/*.cs"`).
- **Benefit:** Specify coding standards or guidance per folder/file hierarchy. Copilot automatically attaches these file references in relevant chat contexts, ensuring consistency across different parts of your codebase.

## Azure Foundry Integration: Bring Your Own Models

The update introduces Azure Foundry integration, letting you use custom AI models with GitHub Copilot Chat for ultimate control over your dev environment.

## Enhanced Chat Management

Two new chat commands simplify conversation history:

- **/clear:** Start a fresh chat by clearing the current thread.
- **/clearAll:** Remove all old threads to declutter chat history.

## Community and Feedback

Stay updated with new features and connect with the Visual Studio community via the [Visual Studio Hub](https://visualstudio.microsoft.com/hub/). Share your experiences or report any issues through the [Developer Community](https://developercommunity.visualstudio.com/VisualStudio).

## Summary

These enhancements reinforce Visual Studio's commitment to supporting developer productivity through deep AI integration, project-aware guidance, and customizable tooling.

This post appeared first on "Microsoft VisualStudio Blog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/visual-studio-october-update/)
