---
external_url: https://devblogs.microsoft.com/dotnet/maui-team-copilot-tips/
title: How the .NET MAUI Team Uses GitHub Copilot for Productivity
author: Jonathan Peppers
feed_name: Microsoft .NET Blog
date: 2025-07-09 17:15:00 +00:00
tags:
- .NET
- Automation
- C#
- CI/CD
- Copilot Coding Agent
- Copilot Productivity
- Firewall Rules
- MAUI
- MCP Server
- MEAI
- Pull Requests
- Repository Instructions
- VS Code
section_names:
- ai
- coding
- devops
- github-copilot
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
