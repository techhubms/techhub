---
title: Microsoft Agent Framework Version 1.0
primary_section: github-copilot
author: Shawn Henry
external_url: https://devblogs.microsoft.com/agent-framework/microsoft-agent-framework-version-1-0/
section_names:
- ai
- azure
- dotnet
- github-copilot
date: 2026-04-03 17:04:03 +00:00
tags:
- .NET
- .NET SDK
- A2A Protocol
- Agent Framework
- Agent Memory
- Agentic AI
- AI
- AI Agents
- Amazon Bedrock
- Anthropic Claude
- AutoGen Migration
- Azure
- Azure AI Foundry
- Azure CLI
- Azure Durable Functions
- Azure OpenAI
- AzureCliCredential
- Coding Agents
- DevUI
- GitHub Copilot
- GitHub Copilot SDK
- Google Gemini
- MCP
- Microsoft Agent Framework
- Middleware Pipeline
- Multi Agent Orchestration
- Neo4j
- News
- NuGet
- Observability
- Ollama
- OpenAI
- OpenTelemetry
- Orchestration Patterns
- PyPI
- Python SDK
- Redis
- Semantic Kernel Migration
- Vector Retrieval
- Workflows
- YAML Declarative Agents
feed_name: Microsoft Semantic Kernel Blog
---

Shawn Henry announces Microsoft Agent Framework 1.0 for .NET and Python, highlighting production-ready APIs, multi-agent orchestration, Azure Foundry/Azure OpenAI integrations, and new capabilities like workflows, memory providers, MCP/A2A support, and tooling such as DevUI.<!--excerpt_end-->

## Microsoft Agent Framework Version 1.0

Microsoft Agent Framework has reached **version 1.0** for both **.NET** and **Python**. This release is positioned as production-ready, with **stable APIs** and a **long-term support** commitment. The framework targets both single-agent apps and **multi-agent systems**, including orchestration across different runtimes and model providers.

Key themes in this release:

- Production-ready 1.0 surface area with backward compatibility guarantees going forward
- **Multi-agent orchestration** and workflow composition
- **Multi-provider model support** (Microsoft and non-Microsoft)
- Cross-runtime interoperability via **A2A** and **MCP (Model Context Protocol)**

### Background

Microsoft Agent Framework was introduced as an effort to unify:

- The enterprise foundations of **Semantic Kernel**
- The orchestration patterns and research lineage behind **AutoGen**

The post notes a Release Candidate phase where the feature surface was locked, followed by community feedback and hardening leading to this 1.0 release.

### Create your first agent

Getting started is shown with minimal examples in both languages.

#### Python

```py
# pip install agent-framework
# Use `az login` to authenticate with Azure CLI

import asyncio

from agent_framework import Agent
from agent_framework.foundry import FoundryChatClient
from azure.identity import AzureCliCredential

agent = Agent(
  client=FoundryChatClient(
    project_endpoint="https://your-project.services.ai.azure.com",
    model="gpt-5.3",
    credential=AzureCliCredential(),
  ),
  name="HelloAgent",
  instructions="You are a friendly assistant."
)

print(asyncio.run(agent.run("Write a haiku about shipping 1.0.")))
```

#### .NET

```csharp
// dotnet add package Microsoft.Agents.AI.OpenAI --prerelease

using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Foundry;
using Azure.Identity;

// Replace the <apikey> with your OpenAI API key.
var agent = new AIProjectClient(endpoint:"https://your-project.services.ai.azure.com")
  .GetResponsesClient("gpt-5.3")
  .AsAIAgent(
    name: "HaikuBot",
    instructions: "You are an upbeat assistant that writes beautifully."
  );

Console.WriteLine(await agent.RunAsync("Write a haiku about shipping 1.0."));
```

From there, the post calls out typical next steps: adding tools/functions, sessions for multi-turn conversations, and streaming responses.

### Multi-agent workflows

The post argues that real applications often require multiple agents working together. It provides an example of a **sequential workflow** with two agents:

- A **writer** agent that drafts a short tagline
- A **reviewer** agent that provides feedback

#### Python sequential workflow example

```py
import asyncio

from agent_framework import Agent, Message
from agent_framework.foundry import FoundryChatClient
from agent_framework.orchestrations import SequentialBuilder
from azure.identity import AzureCliCredential

async def main() -> None:
  # credentials read from .env
  client = FoundryChatClient(credential=AzureCliCredential())

  writer = Agent(
    client=client,
    instructions="You are a concise copywriter. Provide a single, punchy marketing sentence.",
    name="writer",
  )

  reviewer = Agent(
    client=client,
    instructions="You are a thoughtful reviewer. Give brief feedback on the previous message.",
    name="reviewer",
  )

  workflow = SequentialBuilder(participants=[writer, reviewer]).build()

  outputs: list[list[Message]] = []
  async for event in workflow.run(
    "Write a tagline for Microsoft Agent Framework 1.0.",
    stream=True
  ):
    if event.type == "output":
      outputs.append(cast(list[Message], event.data))

  if outputs:
    for msg in outputs[-1]:
      print(f"[{msg.author_name or 'user'}]: {msg.text}")

if __name__ == "__main__":
  asyncio.run(main())
```

A .NET sample is linked: [Getting Started with Workflows](https://github.com/microsoft/agent-framework/tree/main/dotnet/samples/03-workflows/_StartHere/02_AgentsInWorkflows)

### What’s in version 1.0

Version 1.0 is described as the battle-tested, stabilized feature set with a commitment to support and backward compatibility.

- **Single agent + service connectors**: Stable agent abstraction across .NET and Python, with first-party connectors for:
  - Microsoft Foundry
  - Azure OpenAI
  - OpenAI
  - Anthropic Claude
  - Amazon Bedrock
  - Google Gemini
  - Ollama
  - Docs: https://learn.microsoft.com/en-us/agent-framework/agents/
- **Middleware hooks**: Intercept/transform/extend agent behavior at stages of execution (content safety, logging, compliance policies, custom logic) without changing prompts.
  - Docs: https://learn.microsoft.com/en-us/agent-framework/agents/middleware/
- **Agent memory + context providers**: Pluggable memory for history, persistent key-value state, and vector retrieval, with backends including Foundry Agent Service memory, Mem0, Redis, Neo4j, or custom stores.
  - Docs: https://learn.microsoft.com/en-us/agent-framework/agents/conversations/
- **Agent workflows**: Graph-based workflow engine for deterministic processes; branching, fan-out, convergence; checkpointing/hydration for long-running workflows.
  - Docs: https://learn.microsoft.com/en-us/agent-framework/workflows/
- **Multi-agent orchestration**: Patterns including sequential, concurrent, handoff, group chat, and Magentic-One; with streaming, checkpointing, human-in-the-loop approvals, pause/resume.
  - Docs: https://learn.microsoft.com/en-us/agent-framework/workflows/orchestrations/
- **Declarative agents/workflows (YAML)**: Define instructions, tools, memory config, and orchestration topology in version-controlled YAML, then load/run via API.
  - Docs: https://learn.microsoft.com/en-us/agent-framework/agents/declarative
- **A2A and MCP integrations**:
  - **MCP (Model Context Protocol)** for discovering and invoking external tools exposed via MCP-compliant servers
  - **A2A (Agent-to-Agent)** protocol support, with a note that **A2A 1.0 support is coming soon**
  - Docs: https://learn.microsoft.com/en-us/agent-framework/integrations/
- **Migration assistants** for teams coming from Semantic Kernel and AutoGen:
  - Semantic Kernel migration guide: https://learn.microsoft.com/en-us/agent-framework/migration-guide/from-semantic-kernel
  - AutoGen migration guide: https://learn.microsoft.com/en-us/agent-framework/migration-guide/from-autogen

### What’s new since the October release (preview features)

The post also lists preview features that are usable but may change:

- **DevUI**: Local, browser-based debugger for visualizing execution, message flows, tool calls, and orchestration decisions.
  - Docs: https://learn.microsoft.com/en-us/agent-framework/devui/
- **Foundry Hosted Agent Integration**: Run agents as managed services on Microsoft Foundry or as **Azure Durable Functions**.
  - Docs: https://learn.microsoft.com/en-us/agent-framework/integrations/azure-functions
- **Foundry tools/memory/observability/evaluations**: Integration with managed tools, memory, and OpenTelemetry-powered dashboards.
  - Docs: https://learn.microsoft.com/en-us/agent-framework/agents/tools/
- **AG-UI / CopilotKit / ChatKit** adapters: Stream agent output to frontends, including tool execution status and human-in-the-loop flows.
  - Docs: https://learn.microsoft.com/en-us/agent-framework/integrations/ag-ui/
- **Skills**: Reusable domain capability packages (instructions + scripts + resources).
  - Docs: https://learn.microsoft.com/en-us/agent-framework/agents/skills
- **GitHub Copilot SDK and Claude Code SDK providers**: Use GitHub Copilot SDK or Claude Code as an agent harness directly from Agent Framework orchestration code. The post frames these SDKs as handling the autonomous loop (planning, tool execution, file edits, sessions), while Agent Framework wraps and composes them into broader workflows.
  - Docs: https://learn.microsoft.com/en-us/agent-framework/agents/providers/github-copilot
- **Agent Harness**: A customizable harness and local runtime with shell/file system access for coding agents and automation.
  - Post link: https://devblogs.microsoft.com/agent-framework/agent-harness-in-agent-framework/

![Microsoft Agent Framework DevUI](https://devblogs.microsoft.com/agent-framework/wp-content/uploads/sites/78/2026/04/Screenshot-2026-04-02-090737.webp)

### Getting started

If you were on Release Candidate packages, upgrading is described as a package version bump.

- Python install:

```bash
pip install agent-framework
```

- .NET install:

```bash
dotnet add package Microsoft.Agents.AI
```

Useful links:

- Quickstart: https://learn.microsoft.com/en-us/agent-framework/get-started/
- Samples: https://github.com/microsoft/agent-framework

### What’s next

The post frames 1.0 as a foundation and calls out ongoing investment areas:

- Graduating preview features to stable
- Expanding the connector and skills ecosystem
- Deeper Foundry integration
- Incorporating orchestration research

It also emphasizes that the framework is open source and invites feedback and contributions.

### Links

- Download the SDK: https://aka.ms/AgentFramework
- Documentation: https://aka.ms/AgentFramework/Docs
- GitHub repo: https://github.com/microsoft/agent-framework
- Discord: https://aka.ms/foundry/discord
- NuGet (.NET): https://www.nuget.org/packages/Microsoft.Agents.AI/
- PyPI (Python): https://pypi.org/project/agent-framework/


[Read the entire article](https://devblogs.microsoft.com/agent-framework/microsoft-agent-framework-version-1-0/)

