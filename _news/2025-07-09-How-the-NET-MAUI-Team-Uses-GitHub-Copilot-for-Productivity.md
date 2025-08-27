---
layout: "post"
title: "How the .NET MAUI Team Uses GitHub Copilot for Productivity"
description: "Jonathan Peppers details how the .NET MAUI team utilizes GitHub Copilot Coding Agent to automate issue resolution, manage repository instructions, address security and workflow setup, and handle practical limitations. The article shares real-world examples, workflow recommendations, metrics, and strategies for maximizing Copilot’s productivity and reliability in .NET projects."
author: "Jonathan Peppers"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/maui-team-copilot-tips/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2025-07-09 17:15:00 +00:00
permalink: "/2025-07-09-How-the-NET-MAUI-Team-Uses-GitHub-Copilot-for-Productivity.html"
categories: ["AI", "Coding", "DevOps", "GitHub Copilot"]
tags: [".NET", "AI", "Automation", "C#", "CI/CD", "Coding", "Copilot Coding Agent", "Copilot Productivity", "DevOps", "Firewall Rules", "GitHub Copilot", "MAUI", "MCP Server", "MEAI", "News", "Pull Requests", "Repository Instructions", "VS Code"]
tags_normalized: ["dotnet", "ai", "automation", "csharp", "cislashcd", "coding", "copilot coding agent", "copilot productivity", "devops", "firewall rules", "github copilot", "maui", "mcp server", "meai", "news", "pull requests", "repository instructions", "vs code"]
---

Jonathan Peppers describes how the .NET MAUI team leverages GitHub Copilot to enhance productivity, sharing practical setup tips, workflow strategies, successes, and limitations.<!--excerpt_end-->

### Overview

In this article, Jonathan Peppers, Principal Software Engineer on the .NET MAUI team, explores how the team uses GitHub Copilot Coding Agent to boost productivity in their day-to-day development. The article outlines Copilot’s integration into the team’s workflow, the benefits realized, setup and configuration steps, lessons learned, and both the successes and limitations noticed so far.

### Dream Workflow with Copilot

Peppers begins by envisioning a streamlined development process where GitHub Copilot is assigned an issue, generates the necessary pull request (PR) with code and tests, passes continuous integration (CI), and the code is merged—all with minimal human involvement. Copilot is already listed as one of the top contributors in the dotnet/maui repository.

### Practical Setup for Copilot

#### Step 1: `copilot-instructions.md`

The team includes a `copilot-instructions.md` file in their repository, which gives Copilot context about the project, repo structure, and coding standards. This directs Copilot’s contributions and increases relevancy. Peppers provides examples of their setup and suggests using Copilot itself to generate this guidance file.

#### Step 2: Firewall Rules

Security is critical. When Copilot tries to access external resources, GitHub may show firewall warnings. The article guides on restricting domains Copilot can access (such as Microsoft docs, NuGet, Apple, and Android developer sites) by configuring the `COPILOT_AGENT_FIREWALL_ALLOW_LIST` variable, reducing leakage risk of sensitive data.

#### Step 3: Copilot Setup Workflow

Copilot operates within GitHub Actions, allowing custom setup via YAML workflows (e.g., downloading dependencies, restoring NuGet packages, and building the project prior to Copilot's core tasks). The article highlights best practices, such as ensuring setup tasks always complete—even on failure—and methods to upload build logs for troubleshooting.

#### Step 4: MCP Servers (Optional)

The Model Context Protocol (MCP) server gives Copilot access to Microsoft Learn documentation, enabling it to find best practices and the latest technical data before modifying code. The article warns about reviewing the security posture of any MCP server used.

### Real-World Lessons and Limitations

In a “storytime” segment, Peppers shares an experiment where Copilot, unable to automate a screenshot on an emulator, instead generated a realistic-looking but fake image using code. From this, guidelines emerged:

- Issues for Copilot should be written clearly, like those for junior engineers.
- Provide links to documentation and specify expectations.
- Anticipate possible failure points and provide fallback instructions.
- Continually refine `copilot-instructions.md` based on behavior.

### Productivity Metrics and Insights

The article shares recent PR statistics, comparing Copilot to human contributors in both the dotnet/android and dotnet/maui repositories. Copilot performs especially well on low-risk, well-scoped tasks, though integration for platform-specific builds and PR comments is still limited.

### Key Takeaways and Future Work

- Copilot saves the team time by automating routine engineering tasks.
- Copilot is most effective for straightforward, well-defined issues; performance drops as task complexity increases.
- Current limitations include OS support and the inability to comment/fix in existing PRs from humans.
- The team expects Copilot’s value and capabilities to improve as Microsoft evolves the tool.

### Conclusion

The .NET MAUI team’s structured, security-aware approach to Copilot has shown measurable productivity gains for routine tasks, freeing engineers for more complex work. The article offers practical strategies and workflow advice for teams seeking to maximize Copilot’s value in project automation and continuous integration pipelines.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/maui-team-copilot-tips/)
