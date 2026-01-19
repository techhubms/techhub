---
layout: post
title: GitHub Copilot CLI Introduces New AI Models, Enhanced Code Search, and Improved Image Support
author: Allison
canonical_url: https://github.blog/changelog/2025-11-18-github-copilot-cli-new-models-enhanced-code-search-and-better-image-support
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-11-18 22:07:34 +00:00
permalink: /github-copilot/news/GitHub-Copilot-CLI-Introduces-New-AI-Models-Enhanced-Code-Search-and-Improved-Image-Support
tags:
- Agent Configuration
- AI Models
- Automation
- Bugfixes
- CLI
- Client Apps
- Code Search
- Copilot
- Developer Tools
- Gemini 3.5 Pro
- GitHub Copilot CLI
- Google AI
- GPT 5.1
- Image Support
- Improvement
- Markdown
- npm
- OpenAI
- PowerShell
- Ripgrep
- Shell Commands
section_names:
- ai
- coding
- github-copilot
---
Allison from GitHub presents a comprehensive update on GitHub Copilot CLI, describing key feature enhancements, expanded AI model support, code search improvements, and upgrades to image handling and automation for developers.<!--excerpt_end-->

# GitHub Copilot CLI: New Models, Enhanced Code Search, and Better Image Support

**Author:** Allison  

GitHub Copilot CLI has received a series of updates focused on improving developer productivity and AI integration.

## Latest AI Models Now Available

- **New Model Support:**
  - GPT-5.1, GPT-5.1-Codex, and GPT-5.1-Codex-Mini from OpenAI
  - Gemini 3.5 Pro from Google
- Developers can utilize the latest advances in generative AI for more accurate and contextual results.

## More Powerful Code Search

- **Enhanced Search Capabilities:**
  - Copilot CLI now integrates `ripgrep` as well as classic `grep` and `glob` tools.
  - Searches are now faster and provide more relevant context for code suggestions.

## Better Image Support

- Images can now be added to Copilot's context directly by pasting or using drag-and-drop, in addition to @-mentioning image files.
- This streamlines workflows that rely on visual context.

## Additional Developer Enhancements

- **Session Sharing:** `/share` command saves chat sessions as Markdown files or GitHub gists.
- **Permission Handling:** Removed unnecessary checks for heredocs and special characters.
- **Shell Command Support:** Improved compatibility with `!` commands in PowerShell.
- **Automation:** Headless `-p` mode now exits with nonzero codes on errors to better support scripting and automation.
- **UI Improvements:** Enhanced output formatting for `/session`, whitespace, and UI styling.
- **Notifications:** Added support for MCP server tool notifications.
- **Long-Running Shell Commands:** Improved performance and stability.

## Reliability and Bug Fixes

- Eliminated memory leaks in long shell sessions.
- Bugfixes around agent configuration.
- Improved Ctrl+C and abort operation handling.
- Removed internal `NODE_ENV` collision in Node projects.
- Fixed Windows Terminal keyboard input problems.
- More informative error messages for unsupported models.
- UI color printing resolved on first launch.

## Getting the Update

- Update CLI: `npm install -g @github/copilot@latest`
- Feedback is encouraged via the `/feedback` command or GitHub issues in the [public repo](https://github.com/github/copilot-cli).

The GitHub team thanks the community for ongoing feedback as Copilot CLI continues to evolve.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-18-github-copilot-cli-new-models-enhanced-code-search-and-better-image-support)
