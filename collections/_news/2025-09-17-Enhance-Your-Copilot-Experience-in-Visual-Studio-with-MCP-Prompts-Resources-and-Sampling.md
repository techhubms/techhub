---
layout: post
title: Enhance Your Copilot Experience in Visual Studio with MCP Prompts, Resources, and Sampling
author: Allie Barry, Praveen Sethuraman
canonical_url: https://devblogs.microsoft.com/visualstudio/mcp-prompts-resources-sampling/
viewing_mode: external
feed_name: Microsoft VisualStudio Blog
feed_url: https://devblogs.microsoft.com/visualstudio/feed/
date: 2025-09-17 17:30:37 +00:00
permalink: /github-copilot/news/Enhance-Your-Copilot-Experience-in-Visual-Studio-with-MCP-Prompts-Resources-and-Sampling
tags:
- Agentic Behaviors
- AI
- Azure
- Azure DevOps
- Chat
- Coding
- Copilot
- Developer
- Developer Productivity
- DevOps
- DuckDB
- Extensions
- Figma
- GitHub
- GitHub Copilot
- Hugging Face
- IDE Integration
- LLM
- MCP
- MCP Prompts
- MCP Resources
- MCP Sampling
- MongoDB
- News
- Playwright
- Prompt Engineering
- Prompts
- Resource Templates
- Resources
- Sampling
- Server
- VS
section_names:
- ai
- azure
- coding
- devops
- github-copilot
---
Allie Barry and Praveen Sethuraman introduce powerful new MCP features in Visual Studio, including prompts, resources, and sampling, to supercharge Copilot with deeper context and developer integrations.<!--excerpt_end-->

# Enhance Your Copilot Experience in Visual Studio: MCP Prompts, Resources, and Sampling

Authors: Allie Barry, Praveen Sethuraman

Visual Studio now supports Model Context Protocol (MCP) prompts, resources, and sampling to deliver enhanced Copilot experiences for developers. MCP brings context from across the engineering stack directly into the IDE, enabling smarter, context-aware development workflows.

## MCP Resources and Resource Templates: Integrate External Data

MCP establishes standardized access for servers to expose resources (e.g., files, database schemas, app-specific data), all identified by URIs. Developers can reference:

- **Azure DevOps Work Items**: Directly access work items, user stories, and sprint data in Copilot chat for project management and context-driven suggestions.
- **Resource Templates**: Use dynamic templates with arguments to customize resources for specific needs, streamlining setup within Copilot chat.
- **Figma Design Context**: Bridge design and development via Figma MCP servers, referencing design components and style guides within development discussions.

## MCP Prompts and Prompt Templates: Specialized Instructions

MCP prompts provide structured messages or instructions, tailored per server, for more relevant interactions with language models. Developers:

1. Discover available prompts and retrieve contents from MCP servers.
2. Provide arguments for prompt templates to fine-tune instructions.
3. Benefit from specialized prompts for tasks like code review (GitHub MCP server), commit message generation, and design analysis.

## MCP Sampling: Advanced Agentic Behaviors

Sampling allows MCP servers to make LLM calls within other MCP features, enabling:

- Multi-step automation
- Complex behavior like Playwright-based test scenario generation (analyzing DOM, user flows, auto-generating tests)
- Full developer control via approval dialogs for sampling requests

## Getting Started

These MCP features are available now in Visual Studio. Developers can quickly install MCP servers for integrations like:

- MarkItDown (convert files to Markdown)
- DuckDB, MongoDB (local/cloud database operations)
- Hugging Face (models and datasets access)

By integrating external tools and rich context, MCP empowers Copilot to deliver more actionable, relevant assistance for daily development tasks.

## Examples of How MCP Enhances Developer Workflows

- Reference Azure DevOps sprints in chat for planning
- Use prompt templates for PR review via GitHub MCP
- Access Figma design specs for UI work directly in chat
- Automate Playwright test script creation from live DOM samples

## Feedback and Ongoing Improvements

The Visual Studio team encourages user feedback to guide future MCP upgrades. Developers can share input via the [community portal](https://developercommunity.visualstudio.com/home).

---

For more on MCP integrations and to try these features, visit [Visual Studio](https://visualstudio.microsoft.com/).

This post appeared first on "Microsoft VisualStudio Blog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/mcp-prompts-resources-sampling/)
