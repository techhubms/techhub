---
external_url: https://github.blog/changelog/2025-10-10-github-copilot-cli-faster-more-concise-and-prettier
title: 'GitHub Copilot CLI: Faster, More Concise, Streamlined Developer Experience'
author: Allison
feed_name: The GitHub Blog
date: 2025-10-10 20:41:42 +00:00
tags:
- AI Coding Assistant
- Bugfixes
- CLI
- Client Apps
- Copilot
- Copilot Updates
- Developer Tools
- GitHub Copilot CLI
- Improvement
- Kitty Protocol
- Linux
- MacOS
- Markdown Rendering
- Node.js
- Performance
- PowerShell
- Prompt Engineering
- Terminal
- Usability
- Windows Support
- AI
- DevOps
- GitHub Copilot
- News
- .NET
section_names:
- ai
- dotnet
- devops
- github-copilot
primary_section: github-copilot
---
Allison discusses the latest updates to GitHub Copilot CLI, emphasizing enhanced speed, improved terminal usability, and expanded Windows support for developers leveraging AI-powered coding assistance.<!--excerpt_end-->

# GitHub Copilot CLI: Faster, More Concise, and Prettier

Authored by Allison, this post shares updates based directly on developer feedback and daily improvements tracked in the public Copilot CLI repository.

## Key Highlights

- **Performance Gains**: Refinements in Copilot's prompts deliver a 15% reduction in required steps, a 17% drop in input tokens, and responses up to 45% faster (median wall clock time).
- **Streamlined UI & Input**: Timeline interface and input experiences have been further polished. Now, diffs appear by default during file edits, markdown color readability is improved, and window rendering has been optimized for narrow terminals.
- **Enhanced Command Workflow**:
  - Slash commands only cycle through tab completions without arguments, and now provide inline hints for arguments.
  - Terminals supporting the Kitty protocol enable Shift+Tab for multiline input, and support for more terminals is underway.
  - Cursor controls (Ctrl+B / Ctrl+F) for navigation have been added.
- **Windows and PowerShell Fixes**: Substantial improvements to PowerShell file writing speed and command execution, especially for users with custom profiles and configurations. Windows parity with Linux/MacOS is the goal.
- **Bugfixes & Features**:
  - Proxy support covers all supported Node.js versions.
  - Persistent screen reader preferences with `--screen-reader`.
  - Bug resolved for prompt truncation during paste operations.
  - `/clear` now correctly resets context tracking.
- **Daily Releases & Feedback Loop**: Continuous improvements are delivered daily. Users are encouraged to update via `npm install -g @github/copilot@latest` and provide further feedback by using the `/feedback` command or opening issues in the [public repo](https://github.com/github/copilot-cli).

## Further Reading

- [Daily copilot-cli release notes](https://github.com/github/copilot-cli/releases)
- [GitHub Blog Changelog](https://github.blog/changelog/2025-10-10-github-copilot-cli-faster-more-concise-and-prettier)

Stay tuned for more frequent updates and contribute feedback to help shape Copilot CLI!

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-10-github-copilot-cli-faster-more-concise-and-prettier)
