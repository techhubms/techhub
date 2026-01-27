---
external_url: https://devblogs.microsoft.com/blog/bringing-work-context-to-your-code-in-github-copilot
title: Bringing Work Context to Your Code with GitHub Copilot SDK
author: Kayla Cinnamon
feed_name: Microsoft Blog
date: 2026-01-23 15:00:30 +00:00
tags:
- Agent Loop
- Architecture Diagrams
- Code Context
- Contextual AI
- Copilot Integration
- Developer Productivity
- Extension Development
- GitHub Copilot CLI
- GitHub Copilot SDK
- MCP
- MCP Server
- Microsoft 365
- Plugin Development
- VS Code
- Work IQ
section_names:
- ai
- coding
- github-copilot
---
Kayla Cinnamon discusses how the new GitHub Copilot SDK allows developers to embed Copilot agent logic into applications, unlocking productivity through deeper work context integration with tools like VS Code and Microsoft 365 Work IQ MCP.<!--excerpt_end-->

# Bringing Work Context to Your Code with GitHub Copilot SDK

*By Kayla Cinnamon*

The launch of the [GitHub Copilot SDK](https://github.blog/news-insights/company-news/build-an-agent-into-any-app-with-the-github-copilot-sdk/) marks a significant evolution in developer tools, enabling the Copilot agent loop from the Copilot CLI to be easily embedded in any application. Drawing from months of improvements and experiments with Copilot CLI, the SDK offers new opportunities to bridge the gap between code and the myriad factors that shape it—such as specs, design docs, meetings, and team knowledge.

## Challenges Developers Face

- Navigating unfamiliar codebases and identifying code owners
- Tracking down requirements, specs, or the reasoning behind architectural decisions
- Sifting through meeting notes, documents, or organization charts to find answers

## Enabling Deep Work Context with Copilot

Traditional tools like [GitHub Copilot CLI](https://github.com/features/copilot/cli/) have been focused primarily on code. The new SDK expands this by allowing Copilot to:

- Surface code ownership from commit histories, organizational knowledge, and documents
- Generate architecture diagrams directly from meeting transcripts using Work IQ
- Compare deployed code against design specs, highlighting differences or drifts

### Example: Embedding Work Context in VS Code

By integrating Work IQ (the intelligence layer for Microsoft 365 Copilot) with the Copilot CLI, you can:

- Pull meeting transcripts, design docs, and relevant files from Microsoft 365, SharePoint, and OneDrive
- Surface these resources contextually within the editor, minimizing tool-switching
- Ensure architectural clarity and speed up onboarding for new developers

### How to Get Started

To use these capabilities, you need:

- A [GitHub Copilot subscription](https://github.com/features/copilot)
- A [Microsoft 365 subscription](https://developer.microsoft.com/microsoft-365/dev-program), with access to Microsoft Copilot
- Tenant admin approval (details in the [Work IQ MCP Server Repo](https://aka.ms/WorkIQMCP))

#### Setup Steps

1. Make sure you're using the latest GitHub Copilot CLI
2. Install the Work IQ MCP server using:

   ```shell
   /plugin marketplace add github/copilot-plugins
   /plugin install workiq@copilot-plugins
   ```

3. Restart the CLI to see the MCP server
4. [Visual README walkthrough](https://devblogs.microsoft.com/wp-content/uploads/2026/01/work-iq-setup.webp)

## Real-world Usage Examples

- **Finding code owners**: Copilot surfaces organizational links and contributors, even when project members change
- **Creating architecture diagrams**: Automatically generate diagrams from meetings or repository context
- **Design spec comparison**: Detect and highlight deviations between actual code and documented design

## Resources and Community

- [Scott Hanselman's demo video](https://youtu.be/q8wGUe3L3hs)
- Share your builds with #WorkIQBuilds and [connect with the team on GitHub](https://github.com/microsoft/work-iq-mcp)

The integration between GitHub Copilot, Work IQ, and developer environments promises less context switching and better decision traceability. Developers can now harness the full flow of work-related knowledge directly within their tools, reducing friction and boosting productivity.

This post appeared first on "Microsoft Blog". [Read the entire article here](https://devblogs.microsoft.com/blog/bringing-work-context-to-your-code-in-github-copilot)
