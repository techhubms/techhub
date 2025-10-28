---
layout: "post"
title: "Custom Agents for .NET Developers: C# Expert and WinForms Expert"
description: "This article introduces experimental custom GitHub Copilot agents tailored for .NET developers: C# Expert and WinForms Expert. It details their specific features, guidance for integration, improvements they bring to the coding experience, and practical steps for usage within Visual Studio, VS Code, and the Copilot ecosystem."
author: "Wendy Breiding (SHE/HER)"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/introducing-custom-agents-for-dotnet-developers-csharp-expert-winforms-expert/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2025-10-28 22:05:00 +00:00
permalink: "/2025-10-28-Custom-Agents-for-NET-Developers-C-Expert-and-WinForms-Expert.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: [".NET", "Agents", "AI", "Async Programming", "C#", "Coding", "Copilot", "Copilot CLI", "Custom Agents", "Designer File Protection", "Developer Tools", "Event Driven Programming", "GitHub Copilot", "MVP", "MVVM", "News", "TDD", "Unit Testing", "VS", "VS Code", "WinForms"]
tags_normalized: ["dotnet", "agents", "ai", "async programming", "csharp", "coding", "copilot", "copilot cli", "custom agents", "designer file protection", "developer tools", "event driven programming", "github copilot", "mvp", "mvvm", "news", "tdd", "unit testing", "vs", "vs code", "winforms"]
---

Wendy Breiding presents experimental custom GitHub Copilot agents for .NET developers—C# Expert and WinForms Expert—designed to enhance code quality, architecture, and workflow in C# and WinForms projects.<!--excerpt_end-->

# Custom Agents for .NET Developers: C# Expert and WinForms Expert

**Author:** Wendy Breiding

GitHub Copilot has introduced experimental custom agents to further automate and streamline .NET development. Specifically, the new _C# Expert_ and _WinForms Expert_ agents enhance Copilot’s ability to provide best-in-class support for C# and Windows Forms applications.

## C# Expert Agent

- **Core C# Development:** Adheres to modern, idiomatic C# coding standards, taking into account repository preferences.
- **Code Integrity:** Suggests efficient, minimal code changes, prioritizing async/await usage, reliable cancellation, and exception handling.
- **Testing Support:** Encourages behavior-driven unit testing, integration tests, and TDD workflows.

## WinForms Expert Agent

- **UI Design Patterns:** Implements proven UI architectures like MVVM and MVP, promoting maintainability.
- **Event Handling:** Assists with complex event management and robust state management.
- **Tool Integrity:** Protects `.Designer.cs` files, ensuring continued compatibility with Visual Studio Designer after Copilot's changes.

## Getting Started

1. Download `CSharpExpert.agent.md` and `WinFormsExpert.agent.md` from [@github/awesome-copilot](https://github.com/github/awesome-copilot).
2. Add them to your repository’s `.github/agents` folder.

## How to Use Custom Agents

- **Assigning Tasks:** Use Copilot’s issue assignment to select the C# or WinForms Expert agent for specialized advice.
- **Copilot CLI:** Support for `/agent` command (coming soon).
- **Visual Studio Code:** Available in Insiders edition under the Agent dropdown.
- **Visual Studio:** Automatic agent suggestions for your project (coming in v17.14.21, behind a feature flag).

## Improvements and Developer Feedback

- **Reduced Unused Code:** C# Expert reduces unnecessary interfaces and parameters, cutting down on technical debt.
- **Designer File Safety:** WinForms Expert guards against breaking `.Designer.cs` files, preserving workflow with the Visual Studio Designer.

## Learn More

Explore more sample agents and provide feedback via [@github/awesome-copilot](https://github.com/github/awesome-copilot) and [GitHub Community](https://github.com/orgs/community/discussions/177930). Official documentation is available at [gh.io/customagents](https://gh.io/customagents).

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/introducing-custom-agents-for-dotnet-developers-csharp-expert-winforms-expert/)
