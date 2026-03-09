---
layout: "post"
title: "Enhance Coding Agents with .NET Skills: Introducing dotnet/skills"
description: "This news article introduces the dotnet/skills repository, which provides .NET-focused agent skills to boost coding agent workflows. It explains what agent skills are, how they help developers improve productivity with tools like GitHub Copilot CLI and VS Code, and details usage, installation, and the guiding principles of .NET skill development."
author: "Tim Heuer"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/extend-your-coding-agent-with-dotnet-skills/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2026-03-09 20:30:00 +00:00
permalink: "/2026-03-09-Enhance-Coding-Agents-with-NET-Skills-Introducing-dotnetskills.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: [".NET", "Agent Skills", "AI", "AI Development", "C#", "Coding", "Coding Agents", "Coding Productivity", "Copilot", "Copilot CLI", "Debugging", "Dotnet/skills", "GitHub Copilot", "News", "Performance", "Performance Optimization", "Plugin Marketplace", "VS Code"]
tags_normalized: ["dotnet", "agent skills", "ai", "ai development", "csharp", "coding", "coding agents", "coding productivity", "copilot", "copilot cli", "debugging", "dotnetslashskills", "github copilot", "news", "performance", "performance optimization", "plugin marketplace", "vs code"]
---

Tim Heuer introduces the dotnet/skills repository, which equips .NET developers with agent skills to extend coding agent workflows. Discover how these skills boost productivity and can be integrated with GitHub Copilot CLI, VS Code, and other tools.<!--excerpt_end-->

# Extend your coding agent with .NET Skills

**Author:** Tim Heuer

The .NET team is advancing development workflows by introducing the [dotnet/skills](https://github.com/dotnet/skills) repository, a curated set of agent skills aimed at .NET developers. These skills are intended to enhance coding agent experiences—such as with GitHub Copilot CLI, VS Code, and Claude Code—by providing specialized, context-rich abilities to help complete common .NET tasks more reliably.

## What Are Agent Skills?

Agent skills are lightweight packages of specialized knowledge that coding agents can use to improve their task performance. Each skill includes:

- Intent and task-specific context
- Supporting artifacts
- A standard definition (following the [Agent Skills specification](https://agentskills.io))

These skills are compatible with a growing ecosystem of coding agents and can be shared and reused across teams.

## The dotnet/skills Repository

The .NET team is shipping first-party skills that they use internally and in collaboration with engineering partners. These are thoroughly validated—each skill is tested using a light validator measuring if and how the result improves compared to a baseline. Evaluation details are available in the repository so developers can inspect, re-run, and refine them.

## Practical Benefits

- **Better context:** Skills help agents act on tried-and-true patterns from .NET team workflows.
- **Increased reliability:** Measured improvements are prioritized before merging new skills.
- **Transparency:** Skill evaluations are published for visibility and reproducibility.

A customer example highlights how a skill rapidly diagnosed a .NET performance issue, demonstrating real-world productivity gains.

## Installation and Usage

Agent skills are distributed as plugins that can be installed via plugin marketplaces supported by tools like GitHub Copilot CLI and VS Code. The repository is organized by functional plugins. To get started:

```bash
# Add the dotnet/skills marketplace

/plugin marketplace add dotnet/skills

# Browse and install plugins

/plugin marketplace browse dotnet-agent-skills
/plugin install <plugin>@dotnet-agent-skills
```

In VS Code, add the marketplace URL in Copilot extension settings to browse and install plugins through the extension explorer. Skills can then be executed directly from your agent environment.

## Principles and Community Collaboration

The .NET team focuses on:

- Simplicity and task orientation
- Leveraging existing SDK tools where possible
- Community collaboration and support for open skills initiatives (e.g., [github/awesome-copilot](https://github.com/github/awesome-copilot))

The skills are not designed to compete with other marketplaces but to enhance the core .NET development experience.

## The Road Ahead

The AI agent ecosystem is evolving rapidly. The .NET team plans frequent updates and encourages feedback via GitHub to improve available skills and expand their usefulness.

**Explore [dotnet/skills](https://github.com/dotnet/skills), try the skills in your workflows, and share feedback to shape the future of .NET agent development!**

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/extend-your-coding-agent-with-dotnet-skills/)
