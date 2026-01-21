---
external_url: https://code.visualstudio.com/updates/v1_104
title: What's New in Visual Studio Code August 2025 (v1.104)
author: Visual Studio Code Team
feed_name: Visual Studio Code Releases
date: 2025-09-11 17:00:00 +00:00
tags:
- AI Powered Development
- Chat Features
- Coding Agents
- Extension APIs
- JavaScript
- Model Selection
- Productivity Tools
- Python
- Release Notes
- Source Control
- Terminal Improvements
- TypeScript
- VS Code
- VS Code 1.104
section_names:
- ai
- coding
- devops
- github-copilot
---
Visual Studio Code Team's August 2025 release introduces AI-powered features and tighter GitHub Copilot integration, boosting developer productivity and security with expanded chat, coding agents, and model selection improvements.<!--excerpt_end-->

# What's New in Visual Studio Code August 2025 (v1.104)

Visual Studio Code's August 2025 release delivers significant advancements across AI, security, productivity, and developer experience. Below, we highlight the most impactful new features developers and teams will find valuable.

## AI-Powered Development & GitHub Copilot Enhancements

- **Auto Model Selection in Chat**: When using chat, VS Code can now auto-select from a range of advanced models (Claude Sonnet 4, GPT-5, GPT-4.1, Gemini Pro 2.5), optimizing performance and mitigating rate limits. This rollout begins with individual Copilot plans.
- **Improved Copilot Coding Agent Integration**: Enhanced support for delegating TODO comments and larger tasks directly to coding agents in chat, now with richer UI feedback and broader scope leveraging GitHub Copilot's APIs.
- **Contextual Chat with AGENTS.md**: Teams can provide a centralized `AGENTS.md` describing agent context and instructions, auto-loaded for improved multi-agent and workflow-specific chat experiences.
- **Prompt File Improvements**: Custom chat modes can now be referenced from reusable prompt files, enabling granular AI workflow automation across projects.
- **Disable AI Features Setting**: Users can now easily hide or disable built-in GitHub Copilot features, syncing this preference across devices and profiles.

## Security and Trust

- **Sensitive File Edit Confirmations**: Agents invoking edits to critical files now prompt for explicit user approval, mitigating risks from autonomous actions.
- **Terminal Auto Approve Controls**: New warnings, opt-in flows, and rule transparency increase safety when running automated commands; especially important for CI/CD and DevOps workflows.
- **Global Auto Approve Warning**: Enhanced protection with visible prompts and default opt-out, lowering the risk of inadvertently approving sensitive operations globally.

## Productivity & Usability

- **Improved Changed Files Experience**: File diffs and suggested changes are easier to manage, with clear tracking of line-by-line modifications and commit integration.
- **Collaboration Tools**: Chat sessions view and multi-session support help teams coordinate around agentic and Copilot-driven activities.
- **Math Rendering in Chat**: Mathematical equations in chat responses leverage KaTeX for better analysis and scientific coding workflows.
- **Font Configuration in Chat**: Developers can now control font family and size in the chat view for accessibility and comfort.

## Language & Platform Updates

- **JavaScript/TypeScript**: Deprecated Bower IntelliSense; continued npm/yarn focus.
- **Python**: Pipenv environment support, improved environment management, and experimental AI hover summaries for symbols via Pylance and Copilot.
- **Extension Authoring APIs**: Finalized LanguageModelChatProviders API for chat models, improved authentication flows (with support for Azure MFA challenges), and new contribution surfaces like secondary sidebar containers.
- **Terminal and Source Control**: Enhanced terminal features (sticky scroll, IntelliSense, split windows), and streamlined worktree management within source control.

## Engineering & Open Source Recognition

- Contributions from dozens of developers across the VS Code ecosystem, tackling bug fixes, memory leaks, UX improvements, extension enhancements, and documentation.

## Resources

- [Full Release Notes](https://code.visualstudio.com/updates/v1_104)
- [Copilot Model Customization](https://code.visualstudio.com/docs/copilot/customization/language-models)
- [Agent Mode and Security Guidance](https://code.visualstudio.com/docs/copilot/security)
- [AI Toolkit for VS Code](https://aka.ms/AIToolkit)

---

The August 2025 release is a leap forward for developers seeking a smarter, more secure, and customizable coding experience within VS Code. From AI-powered productivity and chat-driven workflows to robust new APIs and expanded language support, this update reflects the continued evolution of development tooling in the age of AI.

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/updates/v1_104)
