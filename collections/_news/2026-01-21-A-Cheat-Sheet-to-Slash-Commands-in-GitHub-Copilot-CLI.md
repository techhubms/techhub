---
external_url: https://github.blog/ai-and-ml/github-copilot/a-cheat-sheet-to-slash-commands-in-github-copilot-cli/
title: A Cheat Sheet to Slash Commands in GitHub Copilot CLI
author: Jacklyn Carroll
feed_name: The GitHub Blog
date: 2026-01-21 17:09:10 +00:00
tags:
- AI & ML
- AI Coding Assistant
- Auditable Actions
- CLI Productivity
- Code Automation
- Context Management
- Copilot CLI
- Developer Tools
- GitHub
- GitHub CLI
- GitHub Copilot CLI
- Model Selection
- Session Management
- Slash Commands
- Terminal Workflow
- Workflow Optimization
section_names:
- ai
- coding
- github-copilot
---
Jacklyn Carroll offers a detailed walkthrough for developers on using slash commands in GitHub Copilot CLI, highlighting time-saving tips, enhanced workflow control, and secure team collaboration.<!--excerpt_end-->

# A Cheat Sheet to Slash Commands in GitHub Copilot CLI

Slash commands in the GitHub Copilot CLI empower developers to manage their coding workflow directly within the terminal, offering speed, transparency, and repeatable actions. If you spend a lot of time in the terminal and seek predictable, context-aware controls over Copilot, mastering these commands can significantly enhance your productivity.

## What Are Slash Commands?

A slash command is a keyboard-driven instruction beginning with `/`, such as `/clear` or `/session`, which triggers specific Copilot actions. These allow you to perform vital tasks—like managing session context, restricting directory access, or configuring AI models—without leaving your terminal environment.

You can use slash commands not only in Copilot CLI, but also across Copilot Chat and agent mode, enabling a consistent experience in multiple development contexts.

## Why Use Slash Commands?

- **Speed & Predictability**: Slash commands avoid the ambiguity of natural language, always producing the same, explicit action.
- **Productivity**: Eliminate context switching by controlling Copilot’s behavior directly in the CLI, running tests, fixing code, or explaining suggestions in place.
- **Clarity & Security**: Commands like `/add-dir` and `/list-dirs` create audit trails for file access, supporting compliance in sensitive environments.
- **Accessibility**: The command-driven interface fits into keyboard-only workflows and is accessible for developers using assistive tech.
- **Trust & Compliance**: Commands make Copilot’s actions traceable and auditable, letting teams manage permissions and monitor usage.
- **Custom Workflows**: Customize Copilot’s integration with automation or CI/CD pipelines via commands like `/delegate`, `/agent`, and `/mcp`.

## Common Slash Commands Grouped by Workflow Need

### Context and Session Management

- `/clear`: Delete session conversation history — ideal for switching tasks or preventing context bleed.
- `/exit`, `/quit`: Close your Copilot CLI session.
- `/session`, `/usage`: See usage statistics for the current session, supporting audits and troubleshooting.

### Directory and File Access

- `/add-dir <directory>`: Grant Copilot access to a directory, scoping suggestions and increasing security.
- `/list-dirs`: List all currently permitted directories.
- `/cwd [directory]`: Display or change working directory.

### Configuration

- `/model [model]`: Select the AI model Copilot should use (useful for testing new models or troubleshooting results).
- `/theme [show|set|list]`: Display, set, or list CLI themes.
- `/terminal-setup`: Enable multiline user inputs in the terminal.
- `/reset-allowed-tools`: Reset external tool permissions.

### Collaboration & External Services

- `/agent`: Choose a custom Copilot agent.
- `/delegate <prompt>`: Auto-generate and submit pull requests using AI.
- `/share [file|gist] [path]`: Export your session for collaboration or documentation.
- `/login` / `/logout`: Authenticate or sign out of Copilot.
- `/mcp [show|add|edit|delete|disable|enable]`: Manage MCP server configurations directly through the CLI.
- `/user [show|list|switch]`: Manage multiple GitHub account workflows.
- `/help`: List all commands and their descriptions.
- `/feedback`: Submit suggestions or bug reports about Copilot CLI.

## Practical Usage Scenarios

- **Switching Between Projects**: Use `/clear` before starting new tasks to remove inherited context.
- **Controlling File Access**: Restrict Copilot’s access with `/add-dir` in sensitive codebases, then audit current access with `/list-dirs`.
- **Model Evaluation**: Quickly compare model performance for your workflow with `/model`.
- **Session Auditing**: Capture a history of actions or share your progress via `/share file [path]`.
- **Configuration Management**: Adjust themes, agents, or tool approvals right from your terminal.

## Quick Reference Table

| Slash Command                          | Purpose                              | Use Case                                    |
|----------------------------------------|--------------------------------------|---------------------------------------------|
| `/clear`                               | Clears context                       | Switching tasks, multirepo workflows        |
| `/add-dir <directory>`                 | Set file access                      | Compliance, security, scoping suggestions   |
| `/model [model]`                       | Change Copilot AI model              | Testing, output optimization                |
| `/session`, `/usage`                   | Show session stats                   | Auditing, troubleshooting                   |
| `/delegate <prompt>`                   | Create a pull request                | Automated code changes                      |
| `/share [file|gist] [path]`            | Export session                       | Documentation, async collaboration          |
| `/login`, `/logout`                    | Sign in/out of Copilot               | Credential management                       |
| `/help`                                | List all available commands          | Onboarding, self-service reference          |

## Getting Started

To try Copilot CLI and these commands:

- [Install GitHub Copilot CLI](https://github.com/features/copilot/cli)
- [Read the documentation](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/use-copilot-cli)
- Explore the [Copilot Chat Cookbook](https://docs.github.com/en/copilot/tutorials/copilot-chat-cookbook) for more workflow ideas

With slash commands in Copilot CLI, you can streamline development, improve repeatability, and ensure clear, traceable automation—all without leaving your terminal.

---
*Written by Jacklyn Carroll*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/github-copilot/a-cheat-sheet-to-slash-commands-in-github-copilot-cli/)
