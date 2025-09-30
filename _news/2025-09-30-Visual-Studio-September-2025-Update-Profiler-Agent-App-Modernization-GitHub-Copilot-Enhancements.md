---
layout: "post"
title: "Visual Studio September 2025 Update: Profiler Agent, App Modernization, GitHub Copilot Enhancements"
description: "This update introduces significant new features in Visual Studio 2022 v17.14, including deep integration with GitHub Copilot and AI-powered tools for profiling, modernization of .NET applications, smarter code reviews, agentic workflows, support for Mermaid diagrams, and enhanced Model Context Protocol support. Developers benefit from improved performance analysis, seamless migration to Azure, and richer chat-based development experiences."
author: "Simona Liao"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/visualstudio/visual-studio-september-update/"
viewing_mode: "external"
feed_name: "Microsoft VisualStudio Blog"
feed_url: "https://devblogs.microsoft.com/visualstudio/feed/"
date: 2025-09-30 13:30:50 +00:00
permalink: "/2025-09-30-Visual-Studio-September-2025-Update-Profiler-Agent-App-Modernization-GitHub-Copilot-Enhancements.html"
categories: ["AI", "Azure", "Coding", "DevOps", "GitHub Copilot"]
tags: [".NET Modernization", "Agent", "Agent Mode", "AI", "Azure", "Azure Migration", "BenchmarkDotNet", "C#", "Code Review", "Code Reviews", "Coding", "Copilot", "Copilot Chat", "Debug", "Developer Productivity", "DevOps", "GitHub Copilot", "MCP", "Mermaid", "Mermaid Diagrams", "Modernization", "News", "Performance Profiling", "Productivity", "Profiler Agent", "Prompt Engineering", "Source Control", "VS"]
tags_normalized: ["dotnet modernization", "agent", "agent mode", "ai", "azure", "azure migration", "benchmarkdotnet", "csharp", "code review", "code reviews", "coding", "copilot", "copilot chat", "debug", "developer productivity", "devops", "github copilot", "mcp", "mermaid", "mermaid diagrams", "modernization", "news", "performance profiling", "productivity", "profiler agent", "prompt engineering", "source control", "vs"]
---

Simona Liao details the major features in the September 2025 Visual Studio update, including AI-powered Profiler Agent, app modernization via Copilot, smarter code reviews, and new developer workflows.<!--excerpt_end-->

# Visual Studio September 2025 Update: Profiler Agent, App Modernization, and Enhanced Copilot Integration

**By Simona Liao**

The Visual Studio 2022 (v17.14) September update brings numerous productivity and modernization features centered around AI, GitHub Copilot integrations, and performance improvements for developers.

## Profiler Agent: AI-Powered Performance Insights

The Profiler Agent is now fully integrated into Visual Studio, providing developers with an AI-driven companion for analyzing and optimizing application performance. Key features include:

- **Works with GitHub Copilot**: Seamlessly integrates Copilot chat for performance analysis and troubleshooting.
- **Identifies bottlenecks**: Analyzes CPU usage, memory, and runtime behavior.
- **BenchmarkDotNet integration**: Can create and optimize benchmarks, suggest fixes, and validate improvements.
- **Conversational commands**: Use Copilot chat to ask targeted questions (e.g., "@profiler Why is my app slow?").
- **Faster workflow**: Reduces time spent on profiling and pinpointing performance issues, supporting .NET object allocations and high CPU usage scenarios.

## .NET Application Modernization

Visual Studio introduces the GitHub Copilot app modernization agent, which:

- **Simplifies upgrading/migrating .NET apps**: Helps transition to newer frameworks and migrate workloads to Azure.
- **Integrated with Copilot Chat**: Invoke via Solution Explorer or Copilot commands (@modernize).
- **Azure focus**: Designed to ease cloud migration and keep apps up-to-date with the latest Azure capabilities.

## Copilot Agent Mode Improvements

Workflow improvements to Copilot Agent Mode include:

- **Reliability and Performance**: More informative progress indicators and improved prompt caching.
- **Working Set Management**: Automatic hiding and clean-up of files for a streamlined workspace.
- **Prompting/Planning Tools**: New ways to decompose complex requests with live-updating markdown tracking and improved formatting for GPT-5 models.
- **Build and Tool Control**: Option to temporarily disable auto-builds for manual oversight.
- **General Fixes**: Improved request cancellations and reduced risk of execution stalls.

## Model Context Protocol (MCP): Prompts, Resources, and Sampling

- **Prompts and Templates**: Structured instructions and templates for guiding code and tooling scenarios.
- **Resource Insertion**: Reference files and schemas within chat using URIs, enriching Copilot responses.
- **Agentic Sampling**: Servers can now initiate LLM actions natively within Visual Studio, leading to more dynamic model workflows.

For more details and examples, check the [Visual Studio blog post on MCP](https://devblogs.microsoft.com/visualstudio/mcp-prompts-resources-sampling/).

## Visualize Workflows with Mermaid Diagrams

Visual Studio's Markdown editor now supports **Mermaid** diagrams, generated by Copilot or user syntax, making it easier to visualize architecture, relationships, and workflows directly in the IDE. Example prompts include sequence diagrams for API calls and onboarding overviews for new projects. Copilot can contextually generate diagrams based on attached files, and these are rendered in real-time in a markdown preview.

## Smarter Code Reviews Powered by Copilot

The local code review process in Visual Studio now leverages GitHub Copilot's advanced models, delivering:

- **In-depth analysis**: Code review comments generated with higher precision and relevance.
- **Customization**: Add `.github/copilot-instructions.md` to control comment style and review focus.
- **Feature Activation**: Enable via Preview Features settings and the GitHub Copilot integration menu.
- **Developer control**: Inline suggestions directly in the Git Changes window for a seamless review workflow.

## Additional Updates

- **Visual Studio Hub**: Access release notes, videos, updates, and discussions in one place.
- **Feedback channels**: Ongoing enhancement driven by community insights and developer feedback.

---

*For step-by-step usage, direct links, and visual guides, refer to the full [Visual Studio September 2025 update post](https://devblogs.microsoft.com/visualstudio/visual-studio-september-update/).*

This post appeared first on "Microsoft VisualStudio Blog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/visual-studio-september-update/)
