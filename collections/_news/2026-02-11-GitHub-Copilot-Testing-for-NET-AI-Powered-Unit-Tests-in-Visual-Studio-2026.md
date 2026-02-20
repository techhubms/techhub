---
external_url: https://devblogs.microsoft.com/dotnet/github-copilot-testing-for-dotnet-available-in-visual-studio/
title: 'GitHub Copilot Testing for .NET: AI-Powered Unit Tests in Visual Studio 2026'
author: McKenna Barlow
primary_section: github-copilot
feed_name: Microsoft .NET Blog
date: 2026-02-11 18:05:00 +00:00
tags:
- .NET
- Agent
- AI
- AI Powered Testing
- C#
- Copilot
- Copilot Chat
- Developer Workflow
- FluentAssertions
- GitHub Copilot
- IDE Integration
- News
- Software Quality
- Test
- Test Agent
- Test Automation
- Test Generation
- Testing
- Unit Testing
- VS
- xUnit
section_names:
- ai
- dotnet
- github-copilot
---
McKenna Barlow announces the general availability of GitHub Copilot testing for .NET in Visual Studio 2026, highlighting how developers can streamline and automate unit test generation with AI integration.<!--excerpt_end-->

# GitHub Copilot Testing for .NET: AI-Powered Unit Tests in Visual Studio 2026

**Author:** McKenna Barlow  
[Read the original post on .NET Blog.](https://devblogs.microsoft.com/dotnet/github-copilot-testing-for-dotnet-available-in-visual-studio/)

---

## Overview

GitHub Copilot testing for .NET is now generally available in Visual Studio 2026 (version 18.3), offering developers a seamless, AI-powered experience for generating, building, fixing, and running unit tests. This release brings richer IDE integration, flexible free-form prompts, and streamlined workflows to help reduce repetitive testing work in C# projects.

## Key Features

- **AI-Driven Unit Testing:** Generate high-quality unit tests using natural language prompts and structured commands in Copilot Chat (`@Test`).
- **Deep IDE Integration:** Launch test generation from Copilot Chat, right-click context menus, or icebreaker prompts directly inside Visual Studio.
- **Flexible Test Scoping:** Target testing efforts at any code level—from an individual member to a class, file, project, or the entire solution. GitHub Copilot adapts tests based on your selection.
- **Iterative Automated Workflow:** The agent can generate, build, run, identify test failures, attempt automatic fixes, and rerun tests to achieve stable results.
- **Automatic Test Project Creation:** If a test project doesn't exist, Copilot can create one and add new tests without manual setup.
- **Summarized Results:** Receive structured summaries detailing:
  - Created or modified test files/projects
  - Coverage improvements and potential testability gaps
  - Pass/fail results, unstable cases, and direct links to generated tests
- **Support for Popular Frameworks:** Define test preferences and conventions (such as xUnit, FluentAssertions) via prompting.

## Getting Started

### Prerequisites

- Visual Studio 18.3
- A C# project or solution that builds without errors
- A paid GitHub Copilot license

### Steps

1. **Open your project** in Visual Studio 18.3.
2. **Start Copilot Chat** and use a prompt beginning with `@Test`. Examples:
   - `@Test generate unit tests for my core business logic`  
   - `@Test class Foo`
   - `@Test write unit tests for my current changes`
   - Structured: `@Test #<target>` where `<target>` is a file, class, etc.
3. **Send the prompt** in Copilot Chat. Copilot will analyze, generate, build, and run tests automatically.
4. **View results** in Test Explorer or the Copilot Chat summary. Review, iterate, and adjust as needed.

### Entry Points

- **Right-click in Editor:** Select _Copilot Actions → Generate Tests_ to launch the agent (scope inferred).
- **Icebreakers in Copilot Chat:** Prompts for common test-related actions populate contextually.

## Developer Feedback and Future Direction

Feedback from early adopters in Visual Studio Insiders has significantly shaped this GA release, particularly in prompt flexibility and workflow integration. Microsoft is continuing to collect feedback, especially as developers tackle larger or more complex testing scopes. Upcoming improvements may include a planning phase for advanced requests, increased control over test generation, and improved ways to review and refine test plans before execution.

Share your experience and help influence the future of GitHub Copilot testing for .NET by [responding to the official survey](https://www.surveymonkey.com/r/QMBPQ7B).

## Useful Resources

- [Getting Started Documentation](https://learn.microsoft.com/visualstudio/test/unit-testing-with-github-copilot-test-dotnet?view=visualstudio)

## Conclusion

GitHub Copilot testing for .NET aims to make unit testing faster and more approachable through AI-powered automation and deep IDE integration, empowering developers to focus on what matters—writing quality software.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/github-copilot-testing-for-dotnet-available-in-visual-studio/)
