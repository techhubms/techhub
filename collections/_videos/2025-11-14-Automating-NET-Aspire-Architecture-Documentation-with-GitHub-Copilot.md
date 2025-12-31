---
layout: "post"
title: "Automating .NET Aspire Architecture Documentation with GitHub Copilot"
description: "This hands-on Level 300 session demonstrates how to automate the creation and updating of .NET Aspire architecture documentation. It leverages the capabilities of GitHub Copilot Chat and the Copilot Coding Agent to generate living docs—Mermaid diagrams, markdown files, and Aspire Dashboard screenshots—keeping architecture records in sync with your app's evolving state. Attendees will learn how to connect architectural definitions to CI/CD workflows, using tools like VS Code's mcp-mermaid and Playwright MCP, ensuring documentation is always up-to-date for any cloud-native .NET Aspire solution."
author: "dotnet"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=8NoetLolw-0"
viewing_mode: "internal"
feed_name: "DotNet YouTube"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UCvtT19MZW8dq5Wwfu6B0oxw"
date: 2025-11-14 04:00:06 +00:00
permalink: "/videos/2025-11-14-Automating-NET-Aspire-Architecture-Documentation-with-GitHub-Copilot.html"
categories: ["AI", "Coding", "DevOps", "GitHub Copilot"]
tags: [".NET 10 Preview", ".NET Aspire", "AI", "AppHost", "Architecture Documentation", "Aspire Dashboard", "Automation", "CI/CD", "Cloud Native", "Coding", "Copilot Chat", "Copilot Coding Agent", "DevOps", "DevOps Workflow", "GitHub Copilot", "Living Documentation", "Markdown", "Mermaid", "Playwright MCP", "Pull Requests", "Videos", "VS Code"]
tags_normalized: ["dotnet 10 preview", "dotnet aspire", "ai", "apphost", "architecture documentation", "aspire dashboard", "automation", "cislashcd", "cloud native", "coding", "copilot chat", "copilot coding agent", "devops", "devops workflow", "github copilot", "living documentation", "markdown", "mermaid", "playwright mcp", "pull requests", "videos", "vs code"]
---

dotnet presents an in-depth session on using GitHub Copilot and Copilot Coding Agent to automate and maintain .NET Aspire app architecture documentation through code and DevOps workflows.<!--excerpt_end-->

{% youtube 8NoetLolw-0 %}

# Automating .NET Aspire Architecture Documentation with GitHub Copilot

**Presenter:** dotnet

## Session Overview

In this Level 300 hands-on session, you'll learn how to transform .NET Aspire AppHost definitions into continuously updated, accurate architecture documentation. The session focuses on reducing documentation rot and effort by streamlining the process through automation, using GitHub Copilot tools.

---

## Key Topics Covered

- **Generating Diagrams:**
  - Use GitHub Copilot Chat to request ASCII diagrams representing the services and resources declared within .NET Aspire projects.
  - Automatically convert these diagrams to Mermaid format for richer presentation.

- **Preview in VS Code:**
  - Utilize the `mcp-mermaid` extension in Visual Studio Code for immediate visualization of Mermaid diagrams, bridging code and documentation for developers.

- **Automating Docs with Agents:**
  - Employ the Copilot Coding Agent to automate pull request creation, ensuring documentation files are updated whenever your app architecture evolves.

- **Enriching with Dashboards:**
  - Integrate Aspire Dashboard screenshots directly into markdown documentation using Playwright MCP for added clarity and context.

- **CI/CD Integration:**
  - Incorporate the entire workflow into your DevOps pipeline, reducing manual effort and guaranteeing your architecture documentation remains a reliable “source of truth.”

## Tools & Technologies

- **.NET Aspire** (AppHost definitions)
- **GitHub Copilot Chat**
- **Copilot Coding Agent**
- **Mermaid** (for diagrams)
- **VS Code + mcp-mermaid**
- **Playwright MCP** (for dashboard screenshots)
- **CI/CD Pipelines** (automation)
- **.NET 10 Preview**

## Practical Workflow Example

1. **Define architecture in .NET Aspire AppHost.**
2. **Use Copilot Chat** to generate diagrams/ASCII representations of the current architecture.
3. **Transform ASCII to Mermaid** and visualize in VS Code with mcp-mermaid.
4. **Delegate documentation updates** to Copilot Coding Agent, which creates PRs as architecture changes.
5. **Add dashboard screenshots** using Playwright MCP, keeping docs visual and current.
6. **Integrate the full cycle** into your CI/CD pipelines, so documentation always reflects the latest production state.

## Benefits

- Minimize outdated or incomplete docs
- Foster transparency and reproducibility for cloud-native .NET Aspire apps
- Deliver a consistent blueprint for development teams
- Lower manual documentation effort and “debt”

## Prerequisites

- Laptop with .NET 10 Preview installed
- Familiarity with GitHub Copilot
- VS Code and relevant extensions available

For the full session and additional .NET Conf 2025 video resources, visit: [YouTube Playlist – .NET Conf 2025](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oXtIlvq1tuORUtZqVG-HdCt)
