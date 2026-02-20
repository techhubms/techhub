---
layout: "post"
title: "Migrating to Microsoft Agent Framework Release Candidate: A Guide for Semantic Kernel and AutoGen Users"
description: "This announcement introduces the Microsoft Agent Framework Release Candidate for both .NET and Python, offering guidance for developers migrating from Semantic Kernel and AutoGen. It explores framework features, agent creation, multi-agent workflows, and hands-on code examples, targeting AI and orchestration developers working within Microsoft’s ecosystem."
author: "Dmytro Struk, Shawn Henry"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/semantic-kernel/migrate-your-semantic-kernel-and-autogen-projects-to-microsoft-agent-framework-release-candidate/"
viewing_mode: "external"
feed_name: "Microsoft Semantic Kernel Blog"
feed_url: "https://devblogs.microsoft.com/semantic-kernel/feed/"
date: 2026-02-20 05:52:57 +00:00
permalink: "/2026-02-20-Migrating-to-Microsoft-Agent-Framework-Release-Candidate-A-Guide-for-Semantic-Kernel-and-AutoGen-Users.html"
categories: ["AI", "Azure", "Coding"]
tags: [".NET", "Agent Framework", "Agent Migration", "Agent Orchestration", "AI", "AI Agents", "AutoGen", "Azure", "Azure OpenAI", "Code Example", "Coding", "Graph Based Workflows", "MCP", "Microsoft Agent Framework", "Microsoft Foundry", "Multi Agent Systems", "News", "NuGet", "OpenAI", "PyPI", "Python", "Semantic Kernel"]
tags_normalized: ["dotnet", "agent framework", "agent migration", "agent orchestration", "ai", "ai agents", "autogen", "azure", "azure openai", "code example", "coding", "graph based workflows", "mcp", "microsoft agent framework", "microsoft foundry", "multi agent systems", "news", "nuget", "openai", "pypi", "python", "semantic kernel"]
---

Dmytro Struk and Shawn Henry introduce the Microsoft Agent Framework Release Candidate for .NET and Python, explaining how developers can migrate from Semantic Kernel and AutoGen to this unified agent orchestration platform.<!--excerpt_end-->

# Migrating to Microsoft Agent Framework Release Candidate: A Guide for Semantic Kernel and AutoGen Users

**Authors**: Dmytro Struk, Shawn Henry

---

Microsoft has announced the Release Candidate (RC) status for the Microsoft Agent Framework, available for both .NET and Python. This milestone signals a stable API and feature-complete offering leading up to the 1.0 General Availability release, and encourages developers to migrate from previous agent orchestration libraries like Semantic Kernel and AutoGen.

## What is Microsoft Agent Framework?

Microsoft Agent Framework is an open-source platform for building, orchestrating, and deploying AI agents across .NET and Python. It is the logical successor to Semantic Kernel and AutoGen, offering a unified programming model and several major capabilities:

- **Simple agent creation**: Quickly build agents in minimal code.
- **Function tools**: Allow agents type-safe access to your custom logic.
- **Graph-based workflows**: Compose agents and functions into orchestration patterns (sequential, concurrent, handoff, group chat), including streaming, checkpointing, and human-in-the-loop.
- **Multi-provider support**: Support for Microsoft Foundry, Azure OpenAI, OpenAI, GitHub Copilot, Anthropic Claude, AWS Bedrock, Ollama, and more.
- **Interoperability**: Conformance with A2A (Agent-to-Agent), AG-UI, and MCP (Model Context Protocol) standards.

## Migrating from Semantic Kernel and AutoGen

If you've built solutions with Semantic Kernel or AutoGen, Microsoft Agent Framework is recommended as the next-generation migration target. Microsoft provides comprehensive guidance:

- [Migration Guide: Semantic Kernel](https://learn.microsoft.com/en-us/agent-framework/migration-guide/from-semantic-kernel)
- [Migration Guide: AutoGen](https://learn.microsoft.com/en-us/agent-framework/migration-guide/from-autogen)

## Creating Your First Agent

You can install the agent framework for your language of choice:

**Python**

```bash
pip install agent-framework --pre
```

Example agent definition in Python:

```python
import asyncio
from agent_framework.azure import AzureOpenAIResponsesClient
from azure.identity import AzureCliCredential

async def main():
    agent = AzureOpenAIResponsesClient(
        credential=AzureCliCredential(),
    ).as_agent(
        name="HaikuBot",
        instructions="You are an upbeat assistant that writes beautifully.",
    )
    print(await agent.run("Write a haiku about Microsoft Agent Framework."))

if __name__ == "__main__":
    asyncio.run(main())
```

**.NET**

```shell
dotnet add package Microsoft.Agents.AI.OpenAI --prerelease
dotnet add package Azure.Identity
```

Example in .NET:

```csharp
using System.ClientModel.Primitives;
using Azure.Identity;
using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Responses;

// Replace <resource> and gpt-4.1 with your Azure OpenAI resource and deployment name.
var agent = new OpenAIClient(
    new BearerTokenPolicy(new AzureCliCredential(), "https://ai.azure.com/.default"),
    new OpenAIClientOptions { Endpoint = new Uri("https://<resource>.openai.azure.com/openai/v1") })
    .GetResponsesClient("gpt-4.1")
    .AsAIAgent(name: "HaikuBot", instructions: "You are an upbeat assistant that writes beautifully.");

Console.WriteLine(await agent.RunAsync("Write a haiku about Microsoft Agent Framework."));
```

These samples demonstrate agent setup, credential handling, and prompt interaction for both platforms.

## Multi-Agent Workflows

The framework supports orchestrating multiple agents for complex scenarios, such as writing and reviewing text in sequence.

**Python Orchestration Example:**

```python
pip install agent-framework-orchestrations --pre
```

```python
import asyncio
from typing import cast
from agent_framework import Message
from agent_framework.azure import AzureOpenAIChatClient
from agent_framework.orchestrations import SequentialBuilder
from azure.identity import AzureCliCredential

async def main() -> None:
    client = AzureOpenAIChatClient(credential=AzureCliCredential())
    writer = client.as_agent(
        instructions="You are a concise copywriter. Provide a single, punchy marketing sentence based on the prompt.",
        name="writer",
    )
    reviewer = client.as_agent(
        instructions="You are a thoughtful reviewer. Give brief feedback on the previous assistant message.",
        name="reviewer",
    )
    workflow = SequentialBuilder(participants=[writer, reviewer]).build()
    outputs = []
    async for event in workflow.run("Write a tagline for a budget-friendly eBike.", stream=True):
        if event.type == "output":
            outputs.append(cast(list[Message], event.data))
    if outputs:
        for msg in outputs[-1]:
            name = msg.author_name or "user"
            print(f"[{name}]: {msg.text}")
if __name__ == "__main__":
    asyncio.run(main())
```

**.NET Orchestration Example:**

```csharp
dotnet add package Microsoft.Agents.AI.Workflows --prerelease
```

```csharp
using System.ClientModel.Primitives;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using OpenAI;

// Replace <resource> and gpt-4.1 with your Azure OpenAI resource and deployment name.
var chatClient = new OpenAIClient(
    new BearerTokenPolicy(new AzureCliCredential(), "https://ai.azure.com/.default"),
    new OpenAIClientOptions { Endpoint = new Uri("https://<resource>.openai.azure.com/openai/v1") })
    .GetChatClient("gpt-4.1")
    .AsIChatClient();

ChatClientAgent writer = new(chatClient, "You are a concise copywriter. Provide a single, punchy marketing sentence based on the prompt.", "writer");
ChatClientAgent reviewer = new(chatClient, "You are a thoughtful reviewer. Give brief feedback on the previous assistant message.", "reviewer");

Workflow workflow = AgentWorkflowBuilder.BuildSequential(writer, reviewer);
List<ChatMessage> messages = [new(ChatRole.User, "Write a tagline for a budget-friendly eBike.")];

await using StreamingRun run = await InProcessExecution.RunStreamingAsync(workflow, messages);
await run.TrySendMessageAsync(new TurnToken(emitEvents: true));
await foreach (WorkflowEvent evt in run.WatchStreamAsync()) {
    if (evt is AgentResponseUpdateEvent e) {
        Console.Write(e.Update.Text);
    }
}
```

## Next Steps & Resources

The Agent Framework is nearing release, so developer feedback is actively encouraged. For documentation, further examples, and installation instructions, see:

- [Microsoft Agent Framework Docs](https://learn.microsoft.com/en-us/agent-framework/)
- [Microsoft Agent Framework on GitHub](https://github.com/microsoft/agent-framework)
- [NuGet (for .NET)](https://www.nuget.org/packages/Microsoft.Agents.AI/)
- [PyPI (for Python)](https://pypi.org/project/agent-framework/)

This post appeared first on "Microsoft Semantic Kernel Blog". [Read the entire article here](https://devblogs.microsoft.com/semantic-kernel/migrate-your-semantic-kernel-and-autogen-projects-to-microsoft-agent-framework-release-candidate/)
