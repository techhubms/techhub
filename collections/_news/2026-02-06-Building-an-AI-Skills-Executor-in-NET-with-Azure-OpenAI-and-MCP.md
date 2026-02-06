---
layout: "post"
title: "Building an AI Skills Executor in .NET with Azure OpenAI and MCP"
description: "This article by Matt Kruczek presents a complete guide for enterprise developers to build a modular AI Skills Executor in .NET. By integrating Azure OpenAI with the Model Context Protocol (MCP) C# SDK, developers can standardize agentic behaviors and orchestrate intelligent agent loops. The guide details reusable skills, tool orchestration using MCP servers, and how to separate business logic from platform orchestration. The implementation aims to create composable, maintainable, and testable AI-powered workflows within the Microsoft ecosystem."
author: "Matt Kruczek"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/foundry/dotnet-ai-skills-executor-azure-openai-mcp/"
viewing_mode: "external"
feed_name: "Microsoft AI Foundry Blog"
feed_url: "https://devblogs.microsoft.com/foundry/feed/"
date: 2026-02-06 19:11:04 +00:00
permalink: "/2026-02-06-Building-an-AI-Skills-Executor-in-NET-with-Azure-OpenAI-and-MCP.html"
categories: ["AI", "Azure", "Coding"]
tags: [".NET", "Agentic Architecture", "AI", "AI Agent", "AI Agents", "AI Development", "Azure", "Azure AI", "Azure OpenAI", "C#", "Coding", "Enterprise AI", "LLM Orchestration", "MCP", "MCP Server", "Microsoft Foundry", "News", "NuGet", "Prompt Engineering", "SDK", "Skill Chaining", "Skill Versioning", "Skills Executor", "Tool Integration", "YAML"]
tags_normalized: ["dotnet", "agentic architecture", "ai", "ai agent", "ai agents", "ai development", "azure", "azure ai", "azure openai", "csharp", "coding", "enterprise ai", "llm orchestration", "mcp", "mcp server", "microsoft foundry", "news", "nuget", "prompt engineering", "sdk", "skill chaining", "skill versioning", "skills executor", "tool integration", "yaml"]
---

Matt Kruczek demonstrates how to architect a .NET-based AI Skills Executor, leveraging Azure OpenAI Service and the Model Context Protocol C# SDK to create agentic workflows for the Microsoft ecosystem.<!--excerpt_end-->

# Building an AI Skills Executor in .NET: Azure OpenAI and MCP Integration

By Matt Kruczek

This article explores how enterprise developers can construct an AI Skills Executor in .NET, fusing Azure OpenAI's large language models with the Model Context Protocol (MCP) C# SDK. The result is a modular, agentic architecture that supports reusable, maintainable AI skills and tool integrations in enterprise applications.

## Overview

- **Agentic Patterns**: Inspired by Anthropic's Skills framework, which treats AI skills as composable software artifacts.
- **Microsoft Ecosystem**: Implementation is tailored for .NET developers using Azure OpenAI and Microsoft-provided SDKs.
- **No Hardcoded Business Logic**: The Skills Executor orchestrates agent loops, standardizing AI interactions without embedding business rules into application code.

---

## Architecture & Components

1. **Skill Loader**: Discovers SKILL.md files, extracts YAML-based metadata and system prompts.
2. **Azure OpenAI Service**: Handles all language model interactions (completions, chat, function calls).
3. **MCP Client Service**: Connects to one or more MCP servers, exposes and calls custom and third-party tools.
4. **Skill Executor**: Orchestrates the user input–LLM–tool loop, enabling multi-step reasoning and tool use by the model.

**Diagram:** Shows how skills, Azure OpenAI, and MCP servers interact to enable agentic loops.

---

## Defining Skills

Each skill is a folder with a `SKILL.md` file (YAML frontmatter + markdown body). Example:

```markdown
---
name: Code Reviewer
description: Reviews code for bugs, security issues, and best practices.
version: 1.0.0
author: Platform Team
category: development
tags:
- code
- review
- quality
---

# Code Reviewer

You are an expert code reviewer. ...
```

### Skill Loader Service

Parses each skill file and loads it into memory, handling frontmatter and body separately.

---

## Tool Integration with MCP

- **MCP C# SDK**: Official SDK maintained by Microsoft and Anthropic.
- **Client/Server Roles**: MCP clients connect to MCP servers to discover tools and route requests.
- **Sample Tools**: Directory analysis, code line counts, finding TODO comments, etc.

Tools are exposed to the LLM for dynamic, agent-driven workflows.

---

## The Agentic Loop Pattern

SkillExecutor manages the loop:

- Loads chosen skill as system prompt
- User input is sent to Azure OpenAI along with available tool definitions
- LLM either responds or requests tool calls via function calls
- Tool responses are added to chat context, process repeats until final output

Limits are set to avoid infinite loops (e.g., max 10 iterations).

---

## Examples: Skills in Action

- **Code Explainer**: Pure LLM, no tool calls
- **Project Analyzer**: Uses custom tools via MCP server
- **GitHub Assistant**: Uses external GitHub MCP server tools

Skills can guide or require tool usage, allowing both reasoning and data-driven workflows.

---

## Enterprise Benefits

- **Standardization**: Move AI business logic into editable, versioned skills.
- **Tool Reusability**: Skills can share tools exposed via MCP servers.
- **Portability**: Following open standards enables skills to work in VS Code, GitHub Copilot, and future tools.
- **Separation of Concerns**: Skill authors, platform teams, and tool developers work independently.
- **Testing**: Each layer can be unit/integration/end-to-end tested with standard .NET practices.

---

## Extensibility & Next Steps

- **Skill Chaining**: Enable complex workflows
- **Skill Versioning**: Manage breaking changes
- **Skill Registry**: Centralized skill discovery
- **Observability**: Track usage and metrics for AI interactions

---

## Resources & Further Reading

- [DotNetSkills on GitHub](https://github.com/MCKRUZ/DotNetSkills)
- [Model Context Protocol Specification](https://modelcontextprotocol.io/)
- [MCP C# SDK](https://github.com/modelcontextprotocol/csharp-sdk)
- [Azure OpenAI Service Docs](https://learn.microsoft.com/en-us/azure/ai-services/openai/)

The full source and implementation details are available on the linked repositories, enabling teams to adapt and extend this reference architecture for real-world enterprise AI adoption.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/dotnet-ai-skills-executor-azure-openai-mcp/)
