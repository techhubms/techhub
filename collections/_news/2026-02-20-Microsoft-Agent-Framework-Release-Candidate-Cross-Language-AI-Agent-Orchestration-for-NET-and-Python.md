---
layout: "post"
title: "Microsoft Agent Framework Release Candidate: Cross-Language AI Agent Orchestration for .NET and Python"
description: "This update announces the Release Candidate of Microsoft Agent Framework for both .NET and Python, signaling a stable API and feature completeness ahead of version 1.0. The framework unifies agent-based AI app development, offering multi-agent workflows, migration from Semantic Kernel/AutoGen, and broad provider compatibility—including Azure OpenAI, GitHub Copilot, and third-party AI services."
author: "Shawn Henry"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/foundry/microsoft-agent-framework-reaches-release-candidate/"
viewing_mode: "external"
feed_name: "Microsoft AI Foundry Blog"
feed_url: "https://devblogs.microsoft.com/foundry/feed/"
date: 2026-02-20 05:51:29 +00:00
permalink: "/2026-02-20-Microsoft-Agent-Framework-Release-Candidate-Cross-Language-AI-Agent-Orchestration-for-NET-and-Python.html"
categories: ["AI", "Azure", "Coding"]
tags: [".NET", "A2A", "AG UI", "Agent Framework", "Agent Orchestration", "AI", "AI Agents", "AutoGen Migration", "Azure", "Azure OpenAI", "Coding", "GitHub Copilot", "GitHub Copilot Integration", "Graph Based Workflows", "MCP", "Microsoft Agent Framework", "Microsoft Foundry", "News", "NuGet", "PyPI", "Python", "Semantic Kernel", "Workflow Engine"]
tags_normalized: ["dotnet", "a2a", "ag ui", "agent framework", "agent orchestration", "ai", "ai agents", "autogen migration", "azure", "azure openai", "coding", "github copilot", "github copilot integration", "graph based workflows", "mcp", "microsoft agent framework", "microsoft foundry", "news", "nuget", "pypi", "python", "semantic kernel", "workflow engine"]
---

Shawn Henry details the Release Candidate of the Microsoft Agent Framework, sharing technical insights for developers orchestrating AI agents in both .NET and Python environments, with extensive code samples and migration guidance.<!--excerpt_end-->

# Microsoft Agent Framework Reaches Release Candidate

**Author: Shawn Henry**

Microsoft has announced the Release Candidate (RC) status for the Microsoft Agent Framework, now available for both .NET and Python. This milestone indicates API surface stability and a complete feature set targeting the general availability (GA) release of version 1.0. The Agent Framework is open source and aims to be a unified solution for building, orchestrating, and deploying AI agents across languages and providers.

## What is Microsoft Agent Framework?

Microsoft Agent Framework is the successor to Semantic Kernel and AutoGen, providing a modern, open-source way to build single- and multi-agent systems. Key features include:

- **Simple agent creation** for rapid prototyping and deployment
- **Function tools** enabling type-safe extension of agents
- **Graph-based workflows**: sequential, concurrent, handoff, and group chat coordination with streaming and human-in-the-loop support
- **Multi-provider support**: Compatible with Microsoft Foundry, Azure OpenAI, OpenAI, GitHub Copilot, Anthropic Claude, AWS Bedrock, Ollama, and more
- **Standards support**: Interoperability via A2A, AG-UI, MCP protocols

## Getting Started: Example Agent Creation

Install for either platform:

- Python: `pip install agent-framework --pre`
- .NET: `dotnet add package Microsoft.Agents.AI.OpenAI --prerelease`

**Python sample:**

```python
import asyncio
from agent_framework.azure import AzureOpenAIResponsesClient
from azure.identity import AzureCliCredential

async def main():
    agent = AzureOpenAIResponsesClient(credential=AzureCliCredential()).as_agent(
        name="HaikuBot",
        instructions="You are an upbeat assistant that writes beautifully."
    )
    print(await agent.run("Write a haiku about Microsoft Agent Framework."))

if __name__ == "__main__":
    asyncio.run(main())
```

**.NET sample:**

```csharp
using System.ClientModel.Primitives;
using Azure.Identity;
using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Responses;

var agent = new OpenAIClient(
    new BearerTokenPolicy(new AzureCliCredential(), "https://ai.azure.com/.default"),
    new OpenAIClientOptions() { Endpoint = new Uri("https://<resource>.openai.azure.com/openai/v1") }
)
.GetResponsesClient("gpt-4.1")
.AsAIAgent(name: "HaikuBot", instructions: "You are an upbeat assistant that writes beautifully.");

Console.WriteLine(await agent.RunAsync("Write a haiku about Microsoft Agent Framework."));
```

## Multi-Agent Workflows

The framework facilitates scalable agent orchestration, letting you compose agents into complex workflows (sequential, concurrent, handoff, or group chat patterns) with full streaming support.

**Python workflow example:**

```python
pip install agent-framework-orchestrations --pre

import asyncio
from agent_framework.azure import AzureOpenAIChatClient
from agent_framework.orchestrations import SequentialBuilder
from azure.identity import AzureCliCredential

async def main() -> None:
    client = AzureOpenAIChatClient(credential=AzureCliCredential())
    writer = client.as_agent(
        instructions="You are a concise copywriter.", name="writer"
    )
    reviewer = client.as_agent(
        instructions="You are a thoughtful reviewer.", name="reviewer"
    )
    workflow = SequentialBuilder(participants=[writer, reviewer]).build()
    async for event in workflow.run("Write a tagline for a budget-friendly eBike.", stream=True):
        # handle output events

if __name__ == "__main__":
    asyncio.run(main())
```

**.NET workflow example:**

```csharp
using Microsoft.Agents.AI.Workflows;
// ...
Workflow workflow = AgentWorkflowBuilder.BuildSequential(writer, reviewer);
// Run and process streaming outputs as shown in original sample code
```

## Migration from Semantic Kernel and AutoGen

Agent Framework is designed as the next step for those coming from Semantic Kernel and AutoGen. Microsoft provides specific [migration guides for Semantic Kernel](https://learn.microsoft.com/en-us/agent-framework/migration-guide/from-semantic-kernel) and [AutoGen](https://learn.microsoft.com/en-us/agent-framework/migration-guide/from-autogen) users.

## Additional Information

- [Official documentation](https://learn.microsoft.com/en-us/agent-framework/)
- [Microsoft Agent Framework on GitHub](https://github.com/microsoft/agent-framework)
- [NuGet package (.NET)](https://www.nuget.org/packages/Microsoft.Agents.AI/)
- [PyPI package (Python)](https://pypi.org/project/agent-framework/)
- [GitHub Discussions](https://github.com/microsoft/agent-framework/discussions)
- [Discord Channel](https://discordapp.com/channels/1113626258182504448/1422947050441543861)

> **Feedback welcome:** Microsoft encourages the community to try out the release candidate and provide feedback as the team approaches GA.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/microsoft-agent-framework-reaches-release-candidate/)
