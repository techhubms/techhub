---
layout: post
title: 'GitHub Copilot CLI: Multiline Input, MCP Enhancements, and Haiku 4.5 Release'
author: Allison
canonical_url: https://github.blog/changelog/2025-10-17-copilot-cli-multiline-input-new-mcp-enhancements-and-haiku-4-5
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-10-17 18:29:47 +00:00
permalink: /github-copilot/news/GitHub-Copilot-CLI-Multiline-Input-MCP-Enhancements-and-Haiku-45-Release
tags:
- AI
- Anthropic Claude Haiku 4.5
- Authentication
- Client Apps
- Coding
- Copilot
- Debugging Tools
- Developer Tools
- Environment Variables
- GitHub Copilot
- GitHub Copilot CLI
- Improvement
- MCP Enhancements
- Multiline Input
- News
- Noninteractive Workflows
- PowerShell Support
- Session Management
- VS Code Terminal
section_names:
- ai
- coding
- github-copilot
---
Allison reports on GitHub Copilot CLI's latest weekly updates, highlighting multiline input, server setup improvements, enhanced noninteractive workflows, and support for Anthropic Claude Haiku 4.5.<!--excerpt_end-->

# GitHub Copilot CLI: Multiline Input, MCP Enhancements, and Haiku 4.5 Release

Author: Allison

Stay updated on the ongoing improvements to GitHub Copilot CLI, directly influenced by developer feedback.

## Highlights This Week

### Multiline Input Support

- Copilot CLI now allows multiline command input using Shift+Enter in terminals supporting the Kitty protocol.
- In Visual Studio Code (VSCode) and compatible forks, enable multiline input via `/terminal-setup`.

### Anthropic Claude Haiku 4.5 Model Support

- The CLI integrates with the new Anthropic Claude Haiku 4.5 AI model, with a playful changelog note marking its debut.

### MCP (Multi-Command Proxy) Server Enhancements

- Easier server setup: The command field now accepts full shell commands, replacing the older comma-separated value format.
- Environment variables can now be entered using common shell syntax (`${VARIABLE_NAME}`), improving the developer experience and consistency with other CLI tools.
- Bug fixes boost reliability for tool calls in MCP sessions.

### Expanded Noninteractive Workflow Capabilities

- PAT and `gh` authentication now respect the `GH_HOST` environment variable, making it possible for GitHub Enterprise Cloud customers with data residency requirements to use noninteractive authentication seamlessly.
- Permissions requests are better handled, and a new `--allow-all-paths` flag is available for granting broad permissions in noninteractive mode.

### Stability Improvements and Forward-Looking Changes

- Session storage and management have been refactored for clarity, conciseness, and scalability.
- The CLI now cleanly separates data storage logic from the display, setting the stage for future integrations and features.
- Model premium request multipliers are now more visible.
- PowerShell support is significantly improved and no longer considered experimental.
- Debugging and logging are enhanced: Stack traces, Copilot API request IDs, and additional debug information are easier to extract. The log level can be set persistently in the config file.

## How to Update and Provide Feedback

- Update Copilot CLI by running:

  ```sh
  npm install -g @github/copilot@latest
  ```

- Contribute with feedback using the `/feedback` command or by opening issues in the GitHub Copilot CLI public repository.

Developer input continues to shape Copilot CLI, bringing quality-of-life upgrades and new features that make command-line development smoother.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-17-copilot-cli-multiline-input-new-mcp-enhancements-and-haiku-4-5)
