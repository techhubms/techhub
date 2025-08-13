---
layout: "post"
title: "Azure DevOps MCP Server Now in Public Preview: Integrating GitHub Copilot with DevOps Context"
description: "Microsoft has released the Azure DevOps MCP Server in public preview, allowing GitHub Copilot in Visual Studio and VS Code to access and interact with Azure DevOps resources locally. This Model Context Provider enhances AI interactions by injecting project context and securely accessing private data within your environment."
author: "Dan Hellem"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/devops/azure-devops-mcp-server-public-preview/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/devops/feed/"
date: 2025-06-17 18:40:23 +00:00
permalink: "/2025-06-17-Azure-DevOps-MCP-Server-Now-in-Public-Preview-Integrating-GitHub-Copilot-with-DevOps-Context.html"
categories: ["AI", "Azure", "DevOps", "GitHub Copilot"]
tags: ["Agentic AI", "Agile", "AI", "AI Integration", "Azure", "Azure & Cloud", "Azure DevOps", "Build Automation", "Cloud Development", "Developer Tools", "DevOps", "GitHub Copilot", "LLM", "MCP Server", "News", "Public Preview", "Pull Requests", "Test Plans", "Visual Studio", "Visual Studio Code", "Work Items"]
tags_normalized: ["agentic ai", "agile", "ai", "ai integration", "azure", "azure cloud", "azure devops", "build automation", "cloud development", "developer tools", "devops", "github copilot", "llm", "mcp server", "news", "public preview", "pull requests", "test plans", "visual studio", "visual studio code", "work items"]
---

Dan Hellem introduces the public preview of the Azure DevOps MCP Server. This new local service lets GitHub Copilot interact with Azure DevOps, providing AI-driven support tailored to your projects while keeping your data secure within your environment.<!--excerpt_end-->

# Azure DevOps MCP Server, Public Preview

*Author: Dan Hellem*

A few weeks ago at Microsoft BUILD, the upcoming Azure DevOps MCP Server was announced:

👉 [Azure DevOps with GitHub Repositories – Your path to Agentic AI](https://devblogs.microsoft.com/devops/azure-devops-with-github-repositories-your-path-to-agentic-ai/)

Today, the local Azure DevOps MCP Server is now available in **public preview**.

## What Does This Mean?

The Azure DevOps MCP Server enables GitHub Copilot (in both Visual Studio and Visual Studio Code) to access and interact with your Azure DevOps environment, including:

- Work items
- Pull requests
- Test plans
- Builds
- Releases
- Wiki pages

## What is an MCP Server?

A local **MCP Server** (Model Context Provider) acts as a bridge between your AI assistant (e.g., GitHub Copilot) and your Azure DevOps organization. Its primary role is to inject rich, real-time project context—like work items, PRs, test plans—into LLM prompts. This allows the AI to provide better, more targeted, and context-sensitive support tailored to your Azure DevOps project.

Unlike cloud-only solutions, the MCP Server runs within your local network or development environment. It was designed to securely access your private project data without that data leaving your control.

## Example Usage

The [how-to guide](https://github.com/microsoft/azure-devops-mcp/blob/main/docs/HOWTO.md#%EF%B8%8F-examples) offers multiple examples and prompts. One example:

- Take a user story
- Generate detailed test cases from the work item description
- Link those test cases directly back to the original story

If you want examples for other scenarios, suggestions and requests are welcome via [the repository’s issues page](https://github.com/microsoft/azure-devops-mcp/issues).

## Getting Started

Initial setup is straightforward, though a few steps are required. It is advised to follow the [full setup instructions](https://github.com/microsoft/azure-devops-mcp/blob/main/README.md) for best results.

Quick overview:

1. Visit the [Azure DevOps MCP Server repository](https://github.com/microsoft/azure-devops-mcp)
2. Sign in using the Azure CLI: `az login`
3. Copy and paste the provided configuration into your local `.vscode\mcp.json` file
4. Start the MCP Server and begin integrating with your Azure DevOps tools

## Feedback

The Azure DevOps team is actively collecting feedback during this public preview stage. Any issues, ideas, or suggestions can be submitted or discussed via [Issues in the repo](https://github.com/microsoft/azure-devops-mcp/issues).

---
For further resources, guides, and updates, refer to the [Azure DevOps MCP Server repository](https://github.com/microsoft/azure-devops-mcp).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/devops/azure-devops-mcp-server-public-preview/)
