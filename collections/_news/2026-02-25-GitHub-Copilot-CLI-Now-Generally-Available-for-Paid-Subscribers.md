---
layout: "post"
title: "GitHub Copilot CLI Now Generally Available for Paid Subscribers"
description: "This news post announces the general availability of GitHub Copilot CLI, the terminal-native coding agent for paid Copilot subscribers. The update highlights new features such as agentic workflow support, multiple operational modes, built-in specialized agents, plugin extensibility, deep terminal integration, support for various AI models, and cross-platform installation. The post details improvements from public preview, the extensibility ecosystem (e.g., MCP, plugins, hooks), code review and diff tools, session and repository memory, enterprise readiness features, and provides comprehensive links and instructions for getting started."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-25-github-copilot-cli-is-now-generally-available"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-25 17:01:29 +00:00
permalink: "/2026-02-25-GitHub-Copilot-CLI-Now-Generally-Available-for-Paid-Subscribers.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["Agent Skills", "AI", "AI Agent", "Autonomous Coding", "Client Apps", "Code Review", "Coding", "Command Line", "Copilot", "Diff Tool", "Enterprise Policies", "GitHub Copilot", "GitHub Copilot CLI", "Improvement", "Linux", "Macos", "MCP", "News", "Plugin Support", "Proxies", "Session Memory", "Terminal Tools", "Windows", "Workflow Automation"]
tags_normalized: ["agent skills", "ai", "ai agent", "autonomous coding", "client apps", "code review", "coding", "command line", "copilot", "diff tool", "enterprise policies", "github copilot", "github copilot cli", "improvement", "linux", "macos", "mcp", "news", "plugin support", "proxies", "session memory", "terminal tools", "windows", "workflow automation"]
---

Allison announces that GitHub Copilot CLI is now generally available for all paid Copilot subscribers, offering an advanced, agentic, and extensible coding experience directly from the terminal.<!--excerpt_end-->

# GitHub Copilot CLI Now Generally Available for Paid Subscribers

GitHub Copilot CLI—the terminal-native coding agent—has reached general availability for all paid Copilot subscribers, delivering powerful AI-driven productivity enhancements directly to the command line. This release is the culmination of extensive improvements since its public preview, driven heavily by user feedback.

## Key Features and Improvements

### Agentic Development in the Terminal

- **Plan mode**: Copilot analyzes requests, asks clarifying questions, creates a structured implementation plan, and seeks user approval before code execution.
- **Autopilot mode**: Let Copilot autonomously handle tasks end-to-end, running commands and iterating without user intervention.
- **Specialized agents**: Built-in agents like Explore, Task, Code Review, and Plan handle targeted workflows, and can run in parallel.
- **Background delegation**: Use `&` prefix to run coding agents in the cloud, freeing your local terminal. Seamlessly switch between local and remote sessions.

### Model Flexibility

- Choose from leading models, including Claude Opus/Sonnet, GPT-5.3-Codex, Gemini 3 Pro, and more.
- Switch models on the fly using `/model`, with configuration for reasoning effort and visibility.
- GPT-5 mini and GPT-4.1 are included at no additional premium for Copilot subscribers.

### Extensibility and Ecosystem

- **MCP support**: Built-in MCP server enables integration with external tools and services.
- **Plugins**: Install community/custom plugins via simple commands; plugin ecosystem can provide new skills and hooks.
- **Agent Skills**: User-defined, markdown-based workflows that enhance Copilot capabilities across environments.
- **Custom agents and hooks**: Easily extend CLI capabilities and enforce policy or add processing at key workflow points.

### Code Review and Navigation

- Inline diffs and reviewing (`/diff`) allow structured feedback and comparison between session or branch changes.
- Direct session-based code analysis (`/review`) for both staged and unstaged changes.
- Rewind/undo features enable reverting file changes within the session.

### Session and Memory Features

- **Auto-compaction**: Sessions automatically compress near context limit, supporting long-lived conversations.
- **Repository and cross-session memory**: Copilot identifies project conventions and preserves context across sessions for future efficiency.

### Polished Terminal and Platform Support

- Full-screen UI with mouse/keyboard navigation, theme picker, and UNIX keybindings.
- Works on macOS, Linux, and Windows; installable via npm, Homebrew, WinGet, shell scripts, and as a Dev Container feature.
- Accessibility improvements, including screen reader mode and responsive layouts.

### Enterprise Readiness

- Policy controls for organizations, including model availability and network management.
- Support for proxies, authentication options (OAuth device flow, GitHub CLI tokens), and CI/CD use cases.
- Policy enforcement hooks and session controls for compliance.

## Getting Started

1. [Install Copilot CLI](https://docs.github.com/copilot/how-tos/copilot-cli/cli-getting-started) via your preferred method.
2. Authenticate with your GitHub account.
3. Initialize Copilot instructions for your project using `/init`.
4. Start working with Copilot from your terminal.

Copilot CLI is included in Copilot Pro, Pro+, Business, and Enterprise subscriptions. Some features require admin enablement for organization accounts.

## Additional Resources

- [Copilot CLI Documentation](https://docs.github.com/copilot/how-tos/use-copilot-agents/use-copilot-cli)
- [Product Page](https://github.com/features/copilot/cli)
- [Discussion Forum](https://github.com/github/copilot-cli)
- [Best Practices Guide](https://docs.github.com/copilot/how-tos/copilot-cli/cli-best-practices)

The CLI aims to provide a seamless, extensible, and productive tool for developers wanting advanced AI coding assistance directly at their fingertips.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-25-github-copilot-cli-is-now-generally-available)
