---
external_url: https://devblogs.microsoft.com/visualstudio/visual-studio-february-update/
title: 'Visual Studio February 2026 Update: AI Assistance, GitHub Copilot, and Developer Productivity'
author: Mark Downie
primary_section: github-copilot
feed_name: Microsoft VisualStudio Blog
date: 2026-02-24 22:16:30 +00:00
tags:
- .NET
- AI
- AI Assistance
- BenchmarkDotNet
- Blazor
- C#
- C++
- Debugger
- Debugging And Diagnostics
- Diagnostics
- GitHub Copilot
- Hot Reload
- IEnumerable Visualizer
- Modernization
- MSTest
- News
- NUnit
- Productivity
- Profiler Agent
- Razor
- Roslyn
- Unit Testing
- VS
- WinForms
- xUnit
section_names:
- ai
- dotnet
- github-copilot
---
Mark Downie details the February 2026 Visual Studio update, highlighting GitHub Copilot-powered AI features, smarter test workflows, and productivity improvements for .NET and C++ development.<!--excerpt_end-->

# Visual Studio February 2026 Update: New AI, GitHub Copilot, and Productivity Features

## Introduction

The February 2026 update to Visual Studio continues the platform's focus on accelerating development and maintaining flow. This release brings several developer-focused enhancements, from smarter AI-powered tools to improved testing and modernization workflows.

## Key Features and Improvements

### WinForms Expert Agent

- **Purpose**: Guides developers through WinForms maintenance and modernization, focusing on challenges such as code organization, layout, and property serialization.
- **Modern Patterns**: Updates for .NET 8-10, introducing MVVM with Community Toolkit, async/await, high-DPI support, and nullable reference types.
- **Expert Guidance**: Assists in both design and runtime coding best practices.

### Smarter Test Generation with GitHub Copilot

- **Integration**: Visual Studio now leverages GitHub Copilot to generate and improve unit tests within C# projects.
- **Workflow**: Use `@Test` in Copilot Chat to describe the tests you need; Copilot generates code compatible with xUnit, NUnit, and MSTest.
- **Benefits**: Makes writing and maintaining tests faster without leaving the IDE.
- **Reference**: [GitHub Copilot test generation announcement](https://devblogs.microsoft.com/dotnet/github-copilot-testing-for-dotnet-available-in-visual-studio/)

### Copilot Chat Slash Commands

- **Functionality**: Define and reuse custom prompts in Copilot Chat with slash commands (e.g., `/generateInstructions`, `/savePrompt`).
- **Efficiency**: Enables quicker access to personalized instructions and repeatable workflow patterns.

### C++ App Modernization with Copilot

- **Feature**: Copilot support for modernizing C++ codebases to newer MSVC versions and resolving upgrade issues.
- **Resources**: In-editor guides and [Microsoft Learn documentation](https://learn.microsoft.com/cpp/porting/copilot-app-modernization-cpp?view=msvc-170).
- **Focus**: Targeted at modern C++ workflows and code health.

### DataTips in IEnumerable Visualizer

- **Debugging**: When debugging collections, DataTips are now available in the IEnumerable Visualizer grid.
- **Details**: Hover over any cell to see all object properties, making it easier to inspect complex or nested data structures.

### Analyze Call Stack with Copilot

- **AI-Driven Analysis**: Use Copilot to interpret call stacks during debugging, explaining blocking issues or thread status.
- **Contextual Insight**: Transforms the call stack window into an actionable troubleshooting guide.

### Profiler Agent Support for Unit Tests

- **Performance Testing**: The Profiler Agent now finds and runs unit tests or benchmarks relevant to performance-critical code, generating baseline metrics and suggestions.
- **Support**: Works with .NET and C++ projects (including BenchmarkDotNet and custom tests).

### Razor Hot Reload Enhancements

- **Improvement**: Hot Reload for Razor files is now faster and more reliable via in-process compilation.
- **Enhancements**: Expanded support for live editing, fewer blocked changes, and smarter automatic restarts after edits.
- **Benefit**: Significantly streamlines Blazor development workflows.

---

### Additional Notes

- All features are available in Visual Studio 2026 Stable Channel (version 18.3).
- Updates are designed to help developers understand, test, and modernize codebases more efficiently.
- Feedback is encouraged to shape ongoing improvements ([developercommunity.visualstudio.com](https://developercommunity.visualstudio.com/VisualStudio)).

This post appeared first on "Microsoft VisualStudio Blog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/visual-studio-february-update/)
