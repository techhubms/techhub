---
layout: "post"
title: "Automating .NET Development with GitHub Copilot Coding Agent"
description: "This article demonstrates how the GitHub Copilot Coding Agent streamlines common .NET development tasks. Focusing on real-world scenarios, it walks through automating unit test creation and feature implementation from a Product Requirements Document (PRD) using Copilot in the cloud. Readers will learn how to leverage Copilot Coding Agent to analyze repositories, plan tasks, create issues and pull requests, and ultimately automate repetitive or boilerplate development work. The content highlights best practices for integrating Copilot with GitHub workflows, discusses customizability through AGENTS.md, and emphasizes the importance of human oversight to maintain code quality. Relevant for .NET developers looking to increase productivity using AI-powered tools within their existing workflows."
author: "Bruno Capuano"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/copilot-coding-agent-dotnet/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2025-09-03 17:05:00 +00:00
permalink: "/2025-09-03-Automating-NET-Development-with-GitHub-Copilot-Coding-Agent.html"
categories: ["AI", "Coding", "DevOps", "GitHub Copilot"]
tags: [".NET", "AI", "AI in Development", "Automation", "C#", "Coding", "Continuous Integration", "DevOps", "Eshoplite", "GitHub", "GitHub Copilot", "GitHub Copilot Coding Agent", "MCP Server", "News", "Product Requirements Document", "Pull Requests", "Test Coverage", "Unit Testing", "Workflow Automation"]
tags_normalized: ["dotnet", "ai", "ai in development", "automation", "csharp", "coding", "continuous integration", "devops", "eshoplite", "github", "github copilot", "github copilot coding agent", "mcp server", "news", "product requirements document", "pull requests", "test coverage", "unit testing", "workflow automation"]
---

Bruno Capuano showcases how the GitHub Copilot Coding Agent automates unit tests and feature implementation in .NET projects, demonstrating AI-powered development enhancements for practitioners.<!--excerpt_end-->

# Automating .NET Development with GitHub Copilot Coding Agent

**Author: Bruno Capuano**

The GitHub Copilot Coding Agent is changing the way .NET developers approach day-to-day software development. Instead of just providing inline code completions, the Coding Agent can analyze your repository, plan complex multi-step tasks, create issues, open pull requests, and coordinate changes—all in the cloud. This means developers get to focus more on design and code review, with Copilot handling repetitive, manual work.

## Key Scenarios Demonstrated

### 1. Automating Unit Test Creation

#### Analyze and Propose Unit Tests

- Copilot Coding Agent reviews the project (eShopLite sample) to identify gaps in test coverage.
- Using a structured prompt, Copilot suggests tests with details such as titles, descriptions, rationale, affected files, and test methods.

#### Review and Create Issues

- Developers review Copilot's structured unit test suggestions to ensure alignment with requirements.
- Copilot creates a GitHub issue containing the full list of proposed tests, tracking the work to be done ([Example issue](https://github.com/Azure-Samples/eShopLite/issues/47)).

#### Assign and Automate Test Implementation

- The issue is assigned to Copilot Coding Agent, which generates the test code and submits a pull request for human review ([Example PR](https://github.com/Azure-Samples/eShopLite/pull/48)).
- This process helps standardize and automate the entire unit test workflow.

### 2. Implementing Features from a PRD

#### Generate a Product Requirements Document (PRD)

- Developers prompt Copilot to draft a PRD for a new feature, in this case, a mock payment server.
- The PRD includes purpose, scope, key success criteria, API contracts, data models, testing strategy, and more.

#### Automatic Feature Implementation

- Once the PRD is approved, Copilot Coding Agent proceeds to implement the feature:
    - Generates code changes and configuration updates
    - Creates or updates UI as needed
    - Utilizes tools like Playwright for validation and screenshots
    - Opens pull requests for review ([Example PR](https://github.com/Azure-Samples/eShopLite/pull/49))

## Integrations and Best Practices

- The Copilot Coding Agent seamlessly integrates with GitHub workflows and supports advanced customization using `AGENTS.md` instructions ([Changelog](https://github.blog/changelog/2025-08-28-copilot-coding-agent-now-supports-agents-md-custom-instructions)).
- To maximize effectiveness and maintain project standards, developers should always review Copilot-generated code before merging.

## Benefits for .NET Developers

- **Boosted Productivity:** Automates low-level coding and project setup tasks.
- **Quality and Consistency:** Encourages thorough test coverage and standardized implementation based on requirements.
- **Focus on Higher-Value Work:** Developers spend more time architecting and less time on repetitive activities.

## Cautions

- While Copilot can automate much of the development process, its outputs should always be subject to human oversight for quality, security, and relevance.

## References

- [eShopLite sample repository](https://aka.ms/eShopLite/repo)
- [10 Microsoft MCP Servers to Accelerate Your Development Workflow](https://devblogs.microsoft.com/blog/10-microsoft-mcp-servers-to-accelerate-your-development-workflow)
- [Copilot Coding Agent changelog and customization](https://github.blog/changelog/2025-08-28-copilot-coding-agent-now-supports-agents-md-custom-instructions)

---

By making use of the Copilot Coding Agent, .NET developers can dramatically enhance their development cycle, automating common tasks and enabling a greater focus on creativity and solution quality.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/copilot-coding-agent-dotnet/)
