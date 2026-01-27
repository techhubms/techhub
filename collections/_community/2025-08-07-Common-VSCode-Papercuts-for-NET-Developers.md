---
external_url: https://www.reddit.com/r/dotnet/comments/1mk1592/vscode_paper_cuts_for_net_dev/
title: Common VSCode Papercuts for .NET Developers
author: Sure-Natural-9086
feed_name: Reddit DotNet
date: 2025-08-07 14:11:04 +00:00
tags:
- .NET
- .NET Build
- C#
- C# Dev Kit
- Cross Platform Development
- Debugging
- Developer Experience
- Extension Management
- IDE Comparison
- IntelliSense
- NuGet
- Productivity
- Solution Explorer
- VS Code
section_names:
- coding
- devops
primary_section: coding
---
Sure-Natural-9086 leads a community discussion on the usability gaps ('paper cuts') faced by .NET developers using VSCode, with practical suggestions and peer insights.<!--excerpt_end-->

# Common VSCode Papercuts for .NET Developers

**Author:** Sure-Natural-9086

This community post explores everyday obstacles ('paper cuts') encountered while developing C# and .NET applications in Visual Studio Code (VSCode). Drawing on experience since 2006 with Visual Studio, the author appreciates VSCode's speed and cross-platform utility but identifies areas for improvement when building .NET solutions.

## Key Issues Identified

### 1. NuGet Package Management

- **Visual Studio** provides a comprehensive UI for managing and updating NuGet packages across entire solutions.
- **VSCode:** Currently lacks a robust NuGet UI, making package management more manual and fragmented.
- **Community solution:** [vscode-nuget-gallery](https://marketplace.visualstudio.com/items?itemName=patcx.vscode-nuget-gallery) extension is recommended as a partial fix.

### 2. Solution Explorer Experience

- Icon colors in Solution Explorer are noted as an area needing parity with Visual Studio for quicker file identification.
- See [GitHub issue #1804](https://github.com/microsoft/vscode-dotnettools/issues/1804) for community feedback.

### 3. Build Output Readability

- Building from the UI can result in uncolorized and hard-to-read output, making failure troubleshooting difficult.
- Suggestions include outputting build logs to files and reviewing them within VSCode.

### 4. Extension Availability and Licensing

- Concerns over subscription models for language extensions; broad agreement that essential language support should be free for all developers.
- Issues with extensions being disabled on VSCode forks, impacting developers using alternative editors.

### 5. Personal Workflows and Preferences

- Some developers prefer a fully manual or text-based workflow, eschewing heavy UIs and integrated management tools in favor of speed and customizability.
- Multiple experiences shared about moving away from Visual Studio to avoid unexpected UI behaviors or performance issues.

## Images and Screenshots

- The post includes build output and Solution Explorer screenshots to illustrate UI points (links in the original post).

## Takeaways and Requests

- Call for more feedback to be directed at the VSCode C# extension team via [GitHub](https://github.com/dotnet/vscode-csharp).
- Suggestions and workarounds from the community for current limitations.
- Recognition of ongoing improvements, encouragement to continue iterating based on real developer needs.

---

**Discussion concludes with community reflections** on why some prefer VSCode, what core features are still missing, and where peer-driven tooling can enhance or fill in existing gaps.

This post appeared first on "Reddit DotNet". [Read the entire article here](https://www.reddit.com/r/dotnet/comments/1mk1592/vscode_paper_cuts_for_net_dev/)
