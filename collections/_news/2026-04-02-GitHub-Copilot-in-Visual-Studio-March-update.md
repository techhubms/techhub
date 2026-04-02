---
tags:
- .agent.md
- .NET
- Agent Skills
- AI
- Allowlist Policies
- C#
- C++
- Copilot
- Copilot Agents
- Custom Agents
- Debugging
- Dependency Vulnerabilities
- Find Symbol
- GitHub Copilot
- Language Server Protocol
- Live Profiling
- LSP
- MCP
- MCP Governance
- News
- NuGet
- PerfTips
- Profiling Agent
- Razor
- Solution Explorer
- Test Explorer
- TypeScript
- Visual Studio
- Vulnerability Remediation
- Watch Window
external_url: https://github.blog/changelog/2026-04-02-github-copilot-in-visual-studio-march-update
primary_section: github-copilot
section_names:
- ai
- dotnet
- github-copilot
title: GitHub Copilot in Visual Studio — March update
feed_name: The GitHub Blog
date: 2026-04-02 15:00:31 +00:00
author: Allison
---

Allison summarizes the March 2026 update for GitHub Copilot in Visual Studio 2026, focusing on new extensibility features like custom agents and agent skills, plus debugging, profiling, and dependency-vulnerability fixes that integrate directly into the IDE.<!--excerpt_end-->

# GitHub Copilot in Visual Studio — March update

March 2026 brought a major step forward for GitHub Copilot extensibility in Visual Studio, with custom agents, agent skills, and new tools that make the agent smarter and more capable.

## Highlights

Here’s what’s new with GitHub Copilot in the March update of Visual Studio 2026:

- **Build your own custom agents**
  - Define specialized Copilot agents as `.agent.md` files in your repository.
  - Custom agents get access to:
    - Workspace awareness
    - Code understanding
    - Tools
    - Your preferred model
    - MCP connections to external knowledge sources
  - Agents appear in the agent picker for team use.

- **Enterprise MCP governance**
  - MCP server usage now respects allowlist policies set through GitHub.
  - Admins can specify which MCP servers are allowed within organizations to help control sensitive data and align with security policies.

- **Use agent skills**
  - Agent skills are reusable instruction sets that teach agents how to perform specific tasks.
  - Define skills in your repository or your user profile.
  - Copilot automatically discovers and applies them.
  - Community examples: [awesome-copilot](https://github.com/github/awesome-copilot)

- **`find_symbol` tool for agent mode**
  - Language-aware symbol navigation, including:
    - Finding all references
    - Accessing type metadata
    - Understanding declarations and scope
  - Supported for **C++**, **C#**, **Razor**, **TypeScript**, and any language with an **LSP** extension.

- **Profile tests with Copilot**
  - New **Profile with Copilot** command in **Test Explorer**.
  - Uses the **Profiling Agent** to run a specific test and analyze CPU + instrumentation data.
  - Produces actionable performance insights.

- **PerfTips powered by live profiling**
  - Debug-time **PerfTips** now integrate with the **Profiler Agent**.
  - Click an inline performance signal during debugging and Copilot analyzes:
    - Elapsed time
    - CPU usage
    - Memory behavior
  - Copilot suggests targeted optimizations.

- **Smart Watch suggestions**
  - Context-aware expression suggestions in **Watch** windows during debugging to help identify meaningful runtime values faster.

- **Fix vulnerabilities with Copilot**
  - Copilot can fix **NuGet package vulnerabilities** directly from **Solution Explorer**.
  - When a vulnerability is detected, click **Fix with GitHub Copilot** and Copilot recommends targeted dependency updates.

## Further reading

- Original post: [GitHub Copilot in Visual Studio — March update](https://github.blog/changelog/2026-04-02-github-copilot-in-visual-studio-march-update)
- Visual Studio blog post: [Visual Studio March update: Build your own custom agents](https://devblogs.microsoft.com/visualstudio/visual-studio-march-update-build-your-own-custom-agents/)
- Visual Studio 2026 release notes: [Release notes](https://learn.microsoft.com/visualstudio/releases/2026/release-notes)

## What’s next for Copilot in Visual Studio

Follow the [Visual Studio blog](https://devblogs.microsoft.com/visualstudio) for roadmap updates and opportunities to share feedback.


[Read the entire article](https://github.blog/changelog/2026-04-02-github-copilot-in-visual-studio-march-update)

