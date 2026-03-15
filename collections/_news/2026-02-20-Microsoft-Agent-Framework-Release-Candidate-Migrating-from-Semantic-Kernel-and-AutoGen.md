---
external_url: https://devblogs.microsoft.com/agent-framework/migrate-your-semantic-kernel-and-autogen-projects-to-microsoft-agent-framework-release-candidate/
title: 'Microsoft Agent Framework Release Candidate: Migrating from Semantic Kernel and AutoGen'
author: Dmytro Struk, Shawn Henry
primary_section: ai
feed_name: Microsoft Semantic Kernel Blog
date: 2026-02-20 05:52:57 +00:00
tags:
- .NET
- A2A
- Agent Framework
- AI
- AI Agents
- AutoGen
- Azure
- Azure OpenAI
- Function Tools
- MCP
- Microsoft Agent Framework
- Migration Guide
- Multi Agent Orchestration
- News
- Open Source AI
- Python
- Semantic Kernel
- Streaming
section_names:
- ai
- azure
- dotnet
---
Dmytro Struk and Shawn Henry introduce the Release Candidate of Microsoft Agent Framework, the open-source successor to Semantic Kernel and AutoGen. This update highlights migration tips, new agent orchestration features, and multi-language support.<!--excerpt_end-->

# Microsoft Agent Framework Release Candidate: Migrating from Semantic Kernel and AutoGen

**Authors:** Dmytro Struk, Shawn Henry

## Overview

Microsoft has announced the Release Candidate (RC) for Microsoft Agent Framework, available for both .NET and Python. This milestone signifies a stable API and feature set ahead of General Availability, encouraging developers to migrate their Semantic Kernel and AutoGen projects to this new open-source framework.

## What is Microsoft Agent Framework?

Microsoft Agent Framework is an open-source, multi-language framework for building, orchestrating, and deploying advanced AI agents. It succeeds both Semantic Kernel and AutoGen, and offers:

- **Simple agent creation**: Get from zero to a working agent quickly in .NET or Python
- **Function tools**: Empower agents to call custom code safely and efficiently
- **Graph-based workflows**: Compose agents and tools into sequential, concurrent, handoff, and group chat orchestration patterns
- **Multi-provider integration**: Works with providers like Microsoft Foundry, Azure OpenAI, OpenAI, Anthropic Claude, AWS Bedrock, Ollama, and GitHub Copilot
- **Interoperability**: Support for Agent-to-Agent (A2A), AG-UI, and Model Context Protocol (MCP) standards

## Migrating from Semantic Kernel and AutoGen

Projects built on Semantic Kernel or AutoGen can now transition to Agent Framework. Microsoft provides comprehensive [migration guides for Semantic Kernel](https://learn.microsoft.com/en-us/agent-framework/migration-guide/from-semantic-kernel) and [for AutoGen](https://learn.microsoft.com/en-us/agent-framework/migration-guide/from-autogen) to assist developers in porting codebases and adopting new capabilities.

## Getting Started: Example Code

### Creating an Agent

**Python:**

```bash
pip install agent-framework --pre
```

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

**.NET:**

```shell
dotnet add package Microsoft.Agents.AI.OpenAI --prerelease
dotnet add package Azure.Identity
```

```csharp
using System.ClientModel.Primitives;
using Azure.Identity;
using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Responses;

// Replace <resource> and gpt-4.1 with your Azure OpenAI resource and deployment name.
var agent = new OpenAIClient(
    new BearerTokenPolicy(new AzureCliCredential(), "https://ai.azure.com/.default"),
    new OpenAIClientOptions() { Endpoint = new Uri("https://<resource>.openai.azure.com/openai/v1") })
    .GetResponsesClient("gpt-4.1")
    .AsAIAgent(name: "HaikuBot", instructions: "You are an upbeat assistant that writes beautifully.");

Console.WriteLine(await agent.RunAsync("Write a haiku about Microsoft Agent Framework."));
```

## Multi-Agent Workflows

Agent Framework introduces workflow engines allowing multiple agents to collaborate in patterns such as sequential, concurrent, handoff, or group chat. Streaming, checkpointing, and human-in-the-loop are supported out-of-the-box.

### Sequential Workflow Example

**Python**

```bash
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

**.NET**

```shell
dotnet add package Microsoft.Agents.AI.Workflows --prerelease
```

```csharp
using System.ClientModel.Primitives;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using OpenAI;

// Replace <resource> and gpt-4.1 accordingly.
var chatClient = new OpenAIClient(
    new BearerTokenPolicy(new AzureCliCredential(), "https://ai.azure.com/.default"),
    new OpenAIClientOptions() { Endpoint = new Uri("https://<resource>.openai.azure.com/openai/v1") })
    .GetChatClient("gpt-4.1")
    .AsIChatClient();

ChatClientAgent writer = new(chatClient, "You are a concise copywriter. Provide a single, punchy marketing sentence based on the prompt.", "writer");
ChatClientAgent reviewer = new(chatClient, "You are a thoughtful reviewer. Give brief feedback on the previous assistant message.", "reviewer");
Workflow workflow = AgentWorkflowBuilder.BuildSequential(writer, reviewer);
List<ChatMessage> messages = [new(ChatRole.User, "Write a tagline for a budget-friendly eBike.")];
await using StreamingRun run = await InProcessExecution.RunStreamingAsync(workflow, messages);
await run.TrySendMessageAsync(new TurnToken(emitEvents: true));
await foreach (WorkflowEvent evt in run.WatchStreamAsync())
{
    if (evt is AgentResponseUpdateEvent e)
    {
        Console.Write(e.Update.Text);
    }
}
```

## Next Steps

The Microsoft Agent Framework RC is available now for open feedback. For documentation, migration guides, and source code, visit:

- [Official Documentation](https://learn.microsoft.com/en-us/agent-framework/)
- [GitHub Repo](https://github.com/microsoft/agent-framework)
- [NuGet (.NET)](https://www.nuget.org/packages/Microsoft.Agents.AI/)
- [PyPI (Python)](https://pypi.org/project/agent-framework/)

The team encourages developers to report issues and share feedback as General Availability approaches.

This post appeared first on "Microsoft Semantic Kernel Blog". [Read the entire article here](https://devblogs.microsoft.com/agent-framework/migrate-your-semantic-kernel-and-autogen-projects-to-microsoft-agent-framework-release-candidate/)
