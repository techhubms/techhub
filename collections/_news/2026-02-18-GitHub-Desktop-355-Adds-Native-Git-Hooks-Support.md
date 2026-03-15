---
external_url: https://github.blog/changelog/2026-02-18-github-desktop-3-5-5-adds-git-hooks-support
title: GitHub Desktop 3.5.5 Adds Native Git Hooks Support
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-02-18 19:32:27 +00:00
tags:
- Client Apps
- Commit Automation
- Copilot Integration
- Cross Platform Development
- Developer Tools
- DevOps
- Git
- Git Hooks
- GitHub Desktop
- Improvement
- News
- Nvm
- Release 3.5.5
- Repository Management
- Shell Environment
- Terminal Integration
- Version Control
- Windows Development
section_names:
- devops
---
Allison details the major new Git hooks support in GitHub Desktop 3.5.5, along with other development-focused improvements like terminal compatibility, Copilot commit attribution, and crash fixes.<!--excerpt_end-->

# GitHub Desktop 3.5.5 Adds Native Git Hooks Support

**Author:** Allison

GitHub Desktop 3.5.5 introduces built-in support for Git hooks, resolving longstanding challenges developers faced when working with version managers and environment-specific scripts within the app—especially on Windows.

## The Problem

Previously, GitHub Desktop's bundled Git installation meant that hooks relying on a developer's configured shell, version managers (like `nvm` for Node.js), or environment-dependent tools often failed. Output from hooks rendered poorly, and users struggled to distinguish between hook rejections and other Git errors.

## What’s New

- **Native Environment Support:** Hooks now run with full access to your shell environment variables. Configuration files like `.bash_profile` or `.zshrc` are loaded, providing consistency with the terminal experience.
- **User Controls:** Enable hooks via **Settings/Options > Git > Hooks**. You can now easily skip or bypass commit hooks directly in the UI.
- **Readable Output:** Hook execution output is displayed in real time with terminal colors and formatting for better clarity.
- **Improved Error Handling:** The interface clearly differentiates between hook failures and other issues, making troubleshooting more straightforward.

## Additional Improvements in 3.5.5

- **Warp terminal support for Windows**
- Ability to open repositories in alternative editors, independent of your default
- New right-click shortcut to view branches quickly on GitHub
- Crash fixes involving emoji/multibyte character output in Git logs
- Correct handling of repository states while switching branches with submodules
- Copilot-authored commits are now visually attributed with the Copilot avatar

## Getting Started

GitHub Desktop will update automatically over time, but you can manually download version 3.5.5 from [GitHub Desktop v3.5.5](https://github.com/desktop/desktop/releases/tag/release-3.5.5). For feedback or to report issues, visit the [GitHub Desktop Issues page](https://github.com/desktop/desktop/issues/new/choose).

---

*For more details, visit the [full changelog announcement](https://github.blog/changelog/2026-02-18-github-desktop-3-5-5-adds-git-hooks-support/).*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-18-github-desktop-3-5-5-adds-git-hooks-support)
