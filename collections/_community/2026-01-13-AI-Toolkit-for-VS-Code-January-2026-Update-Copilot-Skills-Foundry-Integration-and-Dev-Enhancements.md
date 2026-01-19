---
layout: post
title: 'AI Toolkit for VS Code: January 2026 Update — Copilot Skills, Foundry Integration, and Dev Enhancements'
author: junjieli
canonical_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/ai-toolkit-for-vs-code-january-2026-update/ba-p/4485205
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2026-01-13 08:34:38 +00:00
permalink: /github-copilot/community/AI-Toolkit-for-VS-Code-January-2026-Update-Copilot-Skills-Foundry-Integration-and-Dev-Enhancements
tags:
- Agent Framework
- Agent Skills
- AI Toolkit
- Anthropic
- Bug Fixes
- Claude
- Code Generation
- Copilot Chat
- Entra Auth
- Evaluation Tools
- Microsoft Foundry
- Model Catalog
- Performance Tuning
- VS Code
- VS Code Extension
- Windows ML
section_names:
- ai
- azure
- coding
- github-copilot
---
junjieli shares a major January 2026 update for the AI Toolkit in VS Code, spotlighting Copilot Agent Skills, Microsoft Foundry integration, enterprise auth improvements, and developer-focused local model tools.<!--excerpt_end-->

# AI Toolkit for VS Code: January 2026 Update

## Overview

This release streamlines AI agent development in Visual Studio Code by adopting the latest GitHub Copilot standards, integrating tightly with Microsoft Foundry, and rolling out tools and optimizations for enterprise and local ML development. Authored by junjieli, the post addresses agent skill migration, authentication upgrades, and performance enhancements for technical practitioners.

## Copilot Agent Skills Migration

- Transition from **Copilot Instructions** to **Copilot Skills** in v0.28.1, following [VS Code Copilot standards](https://code.visualstudio.com/docs/copilot/customization/agent-skills).
- AI Toolkit Copilot Tools now leverage Agent Skills for richer integration with GitHub Copilot Chat.
- Automatic migration cleans up old instructions on upgrade to v0.28.1, streamlining upgrades for users.
- **Enhanced AIAgentExpert**: Better workflow code generation and evaluation planning/execution.

## Major Enhancements

### Authentication & Agent Development

- **Entra Auth (Microsoft Entra ID)** and Anthropic model support added for both Agent Builder and Playground, providing enterprise-grade authentication for Claude models in the Agent Framework.
- Prioritization of the **Microsoft Foundry** ecosystem for robust agent development:
  - **Foundry v2** is now the default for code generation.
  - **Evaluation Tool** allows generating and running evaluation code directly in Foundry.
  - Improved Model Catalog loads and prioritizes Foundry models for better user experience.

### Developer Experience and Local ML Support

- **Profiling for Windows ML** models is now integrated, enabling developers to monitor model performance and resources within VS Code (v0.28.0+).
- The Windows AI API tab is hidden on Linux/macOS, simplifying the UI and avoiding platform confusion.

## Bug Fixes and UI Improvements

- GitHub Codespaces crash fixed when selecting images in the Playground.
- Immediate update of newly added models in "My Resources" view.
- Claude model compatibility improved for use with AI Toolkit in Copilot workflows.

## Getting Started

- **Download** the AI Toolkit via the [VS Code Marketplace](https://aka.ms/AIToolkit)
- **Explore documentation**: [AI Toolkit Documentation](https://aka.ms/AIToolkit/doc)
- **Changelog**: [See v0.24.0 and beyond](https://github.com/microsoft/vscode-ai-toolkit/blob/main/WHATS_NEW.md#version-0240)
- **Join the conversation**: Report issues and request features on the [GitHub repository](https://github.com/microsoft/vscode-ai-toolkit/issues)

**Happy Coding!**

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/ai-toolkit-for-vs-code-january-2026-update/ba-p/4485205)
