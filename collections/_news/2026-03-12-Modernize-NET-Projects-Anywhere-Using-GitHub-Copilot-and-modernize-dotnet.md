---
layout: "post"
title: "Modernize .NET Projects Anywhere Using GitHub Copilot and modernize-dotnet"
description: "This article introduces the modernize-dotnet agent, a tool that enables .NET developers to assess, plan, and upgrade their applications across multiple environments. Learn how GitHub Copilot, Visual Studio, VS Code, and GitHub integrations streamline and standardize modernization workflows, produce transparent upgrade artifacts, and support custom organizational skills."
author: "Mika Dumont"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/modernize-dotnet-anywhere-with-ghcp/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2026-03-12 17:05:00 +00:00
permalink: "/2026-03-12-Modernize-NET-Projects-Anywhere-Using-GitHub-Copilot-and-modernize-dotnet.html"
categories: ["AI", "Coding", "DevOps", "GitHub Copilot"]
tags: [".NET", ".NET Modernization", "AI", "ASP.NET Core", "Azure Functions", "Blazor", "C#", "Class Libraries", "Code Transformation", "Coding", "Copilot CLI", "Cross Platform", "Custom Agents", "Developer Tools", "DevOps", "GitHub Copilot", "Migration", "Modernization", "Modernize .NET", "News", "Razor Pages", "Repository Management", "Skills", "Upgrade Planning", "VS", "VS Code", "Web API", "Workflow Automation", "WPF"]
tags_normalized: ["dotnet", "dotnet modernization", "ai", "aspdotnet core", "azure functions", "blazor", "csharp", "class libraries", "code transformation", "coding", "copilot cli", "cross platform", "custom agents", "developer tools", "devops", "github copilot", "migration", "modernization", "modernize dotnet", "news", "razor pages", "repository management", "skills", "upgrade planning", "vs", "vs code", "web api", "workflow automation", "wpf"]
---

Mika Dumont explores how the modernize-dotnet agent empowers developers to modernize .NET projects from Visual Studio, VS Code, the Copilot CLI, or even directly on GitHub, generating structured upgrade plans and supporting custom workflows.<!--excerpt_end-->

# Modernize .NET Projects Anywhere Using GitHub Copilot and modernize-dotnet

Modernizing a .NET application can be complex—it calls for an accurate codebase assessment, analyzing dependencies, identifying breaking changes, and carefully sequencing upgrade steps. Traditionally, such modernization workflows were limited to Visual Studio. Now, with the **modernize-dotnet agent**, these processes are available across Visual Studio, VS Code, GitHub Copilot CLI, and directly within GitHub itself.

## The modernize-dotnet Agent

The [modernize-dotnet](https://github.com/dotnet/modernize-dotnet) agent delivers a consistent modernization workflow regardless of environment:

- **Visual Studio**: Fully integrated — right-click your solution and select Modernize.
- **VS Code**: Use the Copilot modernization extension to invoke the agent.
- **GitHub Copilot CLI**: From the terminal, use plugin commands to assess, plan, and upgrade your project directly.
- **GitHub**: Invoke the agent in a repository and manage artifacts through pull requests and reviews.

## Workflow: Assess → Plan → Execute

Each modernization run produces:

- **Assessment**: Highlights project scope and potential blockers
- **Upgrade Plan**: Lays out stepwise actions for upgrade
- **Upgrade Tasks**: Code changes and tasks required for modernization

These artifacts are versioned alongside source code for traceability and collaboration—a major shift from ad-hoc or opaque one-off upgrades.

## Supported Workloads

The agent assists with upgrades for many .NET project types:

- ASP.NET Core (MVC, Razor Pages, Web API)
- Blazor
- Azure Functions
- WPF
- Class libraries
- Console applications
- Migration from .NET Framework (MVC, Web API, Windows Forms, WPF, Azure Functions)

**Note:** CLI and VS Code workflows are cross-platform; however, .NET Framework migrations require Windows.

## Using the modernize-dotnet Agent

### Copilot CLI Example

1. Add the marketplace plugin: `/plugin marketplace add dotnet/modernize-dotnet`
2. Install: `/plugin install modernize-dotnet@modernize-dotnet-plugins`
3. Select the agent: `/agent` then choose `modernize-dotnet`
4. Prompt: `upgrade my solution to a new version of .NET`

The CLI agent generates and applies assessments, upgrade plans, and tasks—all tracked in your repo for transparency.

### On GitHub

You can invoke the agent within your GitHub repository so modernization proposals and implementation tasks are visible to all collaborators. Custom coding agents can be added to your repo by following [these instructions](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/coding-agent/create-custom-agents).

### In VS Code and Visual Studio

- In VS Code, install the Copilot modernization extension, select modernize-dotnet, and prompt for upgrades.
- In Visual Studio, use Solution Explorer to trigger modernization actions with a right-click.

## Custom Skills Support

Teams can encode internal upgrade frameworks, migration patterns, or architectural standards as **skills**. When the agent runs, these skills are automatically included in the upgrade plan, ensuring consistency with organizational best practices. [Learn more about creating custom skills](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/coding-agent/create-skills#creating-and-adding-a-skill).

## Feedback and Community

To try the agent, run it on a repository you're planning to upgrade and experience modernization within your chosen environment. Share your experiences or report issues in the [modernize-dotnet GitHub repository](https://github.com/dotnet/modernize-dotnet/issues).

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/modernize-dotnet-anywhere-with-ghcp/)
