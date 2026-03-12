---
layout: "post"
title: "Build AI Agents with GitHub Copilot SDK and Microsoft Agent Framework"
description: "This article introduces the integration between Microsoft Agent Framework and GitHub Copilot SDK, showing how developers can build powerful AI agents using consistent abstractions and Copilot capabilities across .NET and Python. Topics include installation, orchestration, multi-agent workflows, streaming, permissions, function tools, and real-world code examples."
author: "Dmytro Struk"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/agent-framework/build-ai-agents-with-github-copilot-sdk-and-microsoft-agent-framework/"
viewing_mode: "external"
feed_name: "Microsoft Semantic Kernel Blog"
feed_url: "https://devblogs.microsoft.com/semantic-kernel/feed/"
date: 2026-01-27 21:37:26 +00:00
permalink: "/2026-01-27-Build-AI-Agents-with-GitHub-Copilot-SDK-and-Microsoft-Agent-Framework.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: [".NET", "Agent Framework", "AI", "AI Agents", "Coding", "Copilot Integration", "File Operations", "Function Calling", "GitHub Copilot", "GitHub Copilot SDK", "MCP", "Multi Agent Workflow", "News", "Orchestration", "Python", "Session Management", "Shell Command Execution", "Streaming Responses"]
tags_normalized: ["dotnet", "agent framework", "ai", "ai agents", "coding", "copilot integration", "file operations", "function calling", "github copilot", "github copilot sdk", "mcp", "multi agent workflow", "news", "orchestration", "python", "session management", "shell command execution", "streaming responses"]
---

Dmytro Struk explains how to harness the integration between GitHub Copilot SDK and Microsoft Agent Framework, enabling developers to build AI agents with rich capabilities using consistent interfaces in .NET and Python.<!--excerpt_end-->

# Build AI Agents with GitHub Copilot SDK and Microsoft Agent Framework

Microsoft Agent Framework now integrates with the [GitHub Copilot SDK](https://github.com/github/copilot-sdk), allowing developers to build AI agents powered by Copilot's capabilities. This brings consistent agent abstractions to both .NET and Python and unlocks features like function calling, streaming responses, context management, multi-turn conversations, shell command execution, file operations, URL fetching, and Model Context Protocol (MCP) server integration.

## Why Use Agent Framework with Copilot SDK?

- **Consistent agent abstraction:** Use unified interfaces (`AIAgent` in .NET, `BaseAgent` in Python), so you can easily swap or combine providers.
- **Multi-agent workflows:** Compose GitHub Copilot agents alongside Azure OpenAI, OpenAI, Anthropic, and more within advanced orchestrated workflows.
- **Ecosystem integration:** Access declarative agent definitions, A2A protocol support, function tools, session and streaming support across all providers.

## Installation

**.NET:**

```shell
dotnet add package Microsoft.Agents.AI.GitHub.Copilot --prerelease
```

**Python:**

```shell
pip install agent-framework-github-copilot --pre
```

## Creating Copilot Agents

**.NET Example:**

```csharp
using GitHub.Copilot.SDK;
using Microsoft.Agents.AI;

await using CopilotClient copilotClient = new();
await copilotClient.StartAsync();
AIAgent agent = copilotClient.AsAIAgent();
Console.WriteLine(await agent.RunAsync("What is Microsoft Agent Framework?"));
```

**Python Example:**

```python
from agent_framework.github import GitHubCopilotAgent

async def main():
    agent = GitHubCopilotAgent(default_options={"instructions": "You are a helpful assistant."})
    async with agent:
        result = await agent.run("What is Microsoft Agent Framework?")
        print(result)
```

## Adding Function Tools

Custom function tools can be added to give agents additional domain-specific capabilities:

**.NET:**

```csharp
AIFunction weatherTool = AIFunctionFactory.Create((string location) =>
    $"The weather in {location} is sunny with a high of 25C.",
    "GetWeather",
    "Get the weather for a given location.");
// ... create and use agent as shown above
```

**Python:**

```python
def get_weather(location):
    return f"The weather in {location} is sunny with a high of 25C."

# Pass `tools=[get_weather]` to your Copilot agent
```

## Streaming and Multi-Turn Conversations

- **Stream responses** for real-time user experiences.
- Maintain **conversation context** across multiple interactions using sessions (in .NET) or threads (in Python).

## Permissions and Security

Agents can execute shell commands, read/write files, or fetch URLs if explicitly permitted by the developer. Example permission handlers are provided in both .NET and Python.

## MCP Server Integration

Copilot agents can connect to local or remote MCP servers for advanced external tool and data integrations (e.g., accessing Microsoft Learn search).

## Multi-Agent Workflows

Combine GitHub Copilot agents with other providers like Azure OpenAI within complex workflows (sequencing, concurrent, or group chat patterns). Example code is shown for both .NET and Python, including sequential workflows where agents perform different roles (writer, reviewer, etc.).

## More Resources

- [GitHub Copilot SDK](https://github.com/github/copilot-sdk)
- [Microsoft Agent Framework on GitHub](https://github.com/microsoft/agent-framework)
- [Agent Framework Tutorials](https://learn.microsoft.com/agent-framework/tutorials/overview)

## Summary

The integration empowers developers to craft powerful agentic applications capable of interacting with code, files, shell commands, external APIs, and more, using both .NET and Python environments. Multiple orchestration patterns, function tool extensibility, security controls, and ecosystem integrations make this a flexible foundation for advanced AI development.

If you have feedback or want to discuss further, visit the [discussion boards](https://github.com/microsoft/agent-framework/discussions) or give the project a star on [GitHub](https://github.com/microsoft/agent-framework).

This post appeared first on "Microsoft Semantic Kernel Blog". [Read the entire article here](https://devblogs.microsoft.com/agent-framework/build-ai-agents-with-github-copilot-sdk-and-microsoft-agent-framework/)
