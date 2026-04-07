April 3rd, 2026

![celebrate](https://devblogs.microsoft.com/agent-framework/wp-content/themes/devblogs-evo/images/emojis/celebrate.svg)![like](https://devblogs.microsoft.com/agent-framework/wp-content/themes/devblogs-evo/images/emojis/like.svg)![heart](https://devblogs.microsoft.com/agent-framework/wp-content/themes/devblogs-evo/images/emojis/heart.svg)6 reactions

 

![Shawn Henry](https://devblogs.microsoft.com/agent-framework/wp-content/uploads/sites/78/2024/10/ShawnHenry-96x96.jpg)

Principal Group Product Manager

 

Today, we’re thrilled to announce that **Microsoft Agent Framework has reached version 1.0** for both [.NET](https://www.nuget.org/packages/Microsoft.Agents.AI/) and [Python](https://pypi.org/project/agent-framework/). This is the production-ready release: stable APIs, and a commitment to long-term support. Whether you’re building a single assistant or orchestrating a fleet of specialized agents, Agent Framework 1.0 gives you enterprise-grade multi-agent orchestration, multi-provider model support, and cross-runtime interoperability via A2A and MCP.

When we [introduced Microsoft Agent Framework](https://devblogs.microsoft.com/foundry/introducing-microsoft-agent-framework-the-open-source-engine-for-agentic-ai-apps/) last October, we set out to unify the enterprise-ready foundations of Semantic Kernel with the innovative orchestrations of AutoGen into a single, open-source SDK. When we hit [Release Candidate](https://devblogs.microsoft.com/foundry/microsoft-agent-framework-reaches-release-candidate/) in February, we locked the feature surface and invited the community to put it through its paces. Today, after months of feedback, hardening, and real-world validation with customers and partners, Agent Framework 1.0 is ready for production.

[https://devblogs.microsoft.com/agent-framework/wp-content/uploads/sites/78/2026/04/devui6_720p.mp4](https://devblogs.microsoft.com/agent-framework/wp-content/uploads/sites/78/2026/04/devui6_720p.mp4)

**Create Your First Agent**

Getting started takes just a few lines of code. Here’s how to create a simple agent in both languages.

**Python**

```py
# pip install agent-framework
# Use `az login` to authenticate with Azure CLI

import asyncio

from agent_framework import Agent
from agent_framework.foundry import FoundryChatClient
from azure.identity import AzureCliCredential

agent = Agent(
    client= FoundryChatClient(
      project_endpoint="https://your-project.services.ai.azure.com",
      model="gpt-5.3",
      credential=AzureCliCredential(),
    ),
    name="HelloAgent",
    instructions="You are a friendly assistant."
)

print(asyncio.run(agent.run("Write a haiku about shipping 1.0.")))
```

**.NET**

```csharp
// dotnet add package Microsoft.Agents.AI.OpenAI --prerelease
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Foundry
using Azure.Identity;

// Replace the with your OpenAI API key.
var agent = new AIProjectClient(endpoint:"https://your-project.services.ai.azure.com")
    .GetResponsesClient("gpt-5.3")
    .AsAIAgent(
 name: "HaikuBot", 
 instructions: "You are an upbeat assistant that writes beautifully."
 );

Console.WriteLine(await agent.RunAsync("Write a haiku about shipping 1.0."));
```

That’s it – a working AI agent in a handful of lines. From here you can add function tools, sessions for multi-turn conversations, streaming responses, and more.

**Multi-Agent Workflows**

Single agents are powerful, but real-world applications often need multiple agents working together. Here’s a sequential workflow where a copywriter drafts a tagline and a reviewer provides feedback:

**Python**

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
    async for event in workflow.run("Write a tagline for Microsoft Agent Framework 1.0.", stream=True):
        if event.type == "output":
            outputs.append(cast(list[Message], event.data))
    if outputs:
        for msg in outputs[-1]:
            print(f"[{msg.author_name or 'user'}]: {msg.text}")

if __name__ == "__main__":
    asyncio.run(main())
```

and check out the .NET version here: [Getting Started with Workflows](https://github.com/microsoft/agent-framework/tree/main/dotnet/samples/03-workflows/_StartHere/02_AgentsInWorkflows)

**What’s in version 1.0**

Version 1.0 represents the features we’ve battle-tested, stabilized, and committed to supporting with full backward compatibility going forward.

- **Single Agent and Service Connectors – **The core agent abstraction is stable and production-ready across both .NET and Python. Agent Framework ships with first-party service connectors for Microsoft Foundry, Azure OpenAI, OpenAI, Anthropic Claude, Amazon Bedrock, Google Gemini and Ollama. [[Learn more… ]](https://learn.microsoft.com/en-us/agent-framework/agents/)

- **Middleware Hooks – **The middleware pipeline lets you intercept, transform, and extend agent behavior at every stage of execution: content safety filters, logging, compliance policies, custom logic – all without modifying agent prompts. [[Learn more… ]](https://learn.microsoft.com/en-us/agent-framework/agents/middleware/)

- **Agent Memory and Context Providers – **Pluggable memory architecture supporting conversational history, persistent key-value state, and vector-based retrieval. Choose your backend: Memory in Foundry Agent Service, Mem0, Redis, Neo4j or use a custom store. [[Learn more…]](https://learn.microsoft.com/en-us/agent-framework/agents/conversations/)

- **Agent Workflows – **The graph-based workflow engine for composing agents and functions into deterministic, repeatable processes is now stable. Build workflows that combine agent reasoning with business logic, branch on conditions, fan out to parallel steps, and converge results. Checkpointing and hydration ensure long-running processes survive interruptions. [[Learn more… ]](https://learn.microsoft.com/en-us/agent-framework/workflows/)

- **Multi-Agent Orchestration – **Stable support for the orchestration patterns that emerged from Microsoft Research and AutoGen: sequential, concurrent, handoff, group chat, and Magentic-One. All patterns support streaming, checkpointing, human-in-the-loop approvals, and pause/resume for long-running workflows. [[Learn more…]](https://learn.microsoft.com/en-us/agent-framework/workflows/orchestrations/)

- **Declarative Agents and Workflows (YAML) – **Define agents’ instructions, tools, memory configuration, and orchestration topology in version-controlled YAML files, then load and run them with a single API call.[ [Learn more… ]](https://learn.microsoft.com/en-us/agent-framework/agents/declarative)

- **A2A, and MCP – MCP (Model Context Protocol)** support lets agents dynamically discover and invoke external tools exposed over MCP-compliant servers. **A2A (Agent-to-Agent) protocol **(A2A 1.0 support coming soon) support enables cross-runtime agent collaboration – your agents can coordinate with agents running in other frameworks using structured, protocol-driven messaging. [[Learn more…]](https://learn.microsoft.com/en-us/agent-framework/integrations/)

- **Migration Assistants (Semantic Kernel and AutoGen) – **For teams coming from Semantic Kernel or AutoGen, migration assistants analyze your existing code and generate step-by-step migration plans. The [Semantic Kernel migration guide](https://learn.microsoft.com/en-us/agent-framework/migration-guide/from-semantic-kernel) and [AutoGen migration guide](https://learn.microsoft.com/en-us/agent-framework/migration-guide/from-autogen) provide detailed walkthroughs.

**What’s new since the October release**

We’re also shipping with preview features that are functional and available for early adoption. These APIs may evolve based on community feedback before reaching ‘stable’.

- **DevUI** – browser-based local debugger for visualizing agent execution, message flows, tool calls, and orchestration decisions in real time. [[Learn more… ]](https://learn.microsoft.com/en-us/agent-framework/devui/)

- **Foundry Hosted Agent Integration** – run Agent Framework agents as managed services on Microsoft Foundry or as Azure Durable Functions. [[Learn more… ]](https://learn.microsoft.com/en-us/agent-framework/integrations/azure-functions)

- **Foundry Tools, Memory, Observability and Evaluations** – deep integration with Foundry’s managed tool ecosystem, memory, and OpenTelemetry-powered observability and evaluations dashboards. [[Learn more… ]](https://learn.microsoft.com/en-us/agent-framework/agents/tools/)

- **AG-UI / CopilotKit / ChatKit** – stream agent output to frontend surfaces with adapters for CopilotKit and ChatKit, including tool execution status and human-in-the-loop flows. [[Learn more… ]](https://learn.microsoft.com/en-us/agent-framework/integrations/ag-ui/)

- **Skills** – reusable domain capability packages (instructions + scripts + resources)) that give agents structured capabilities out of the box. [[Learn more… ]](https://learn.microsoft.com/en-us/agent-framework/agents/skills)

- **GitHub Copilot SDK and Claude Code SDK** — use GitHub Copilot SDK or Claude Code as an agent harness directly from your Agent Framework orchestration code. These SDKs handle the autonomous agent loop – planning, tool execution, file edits, and session management – and Agent Framework wraps them, letting you compose a coding-capable agent alongside other agents (Azure OpenAI, Anthropic, custom) in the same multi-agent workflow. [[Learn more… ]](https://learn.microsoft.com/en-us/agent-framework/agents/providers/github-copilot)

- **Agent Harness** — customizable Microsoft Agent Framework harness and local runtime giving agents access to shell, file system, and messaging loop for coding agents, automation, and personal assistant patterns. [[Learn more… ]](https://devblogs.microsoft.com/agent-framework/agent-harness-in-agent-framework/)

[![Microsoft Agent Framework DevUI](https://devblogs.microsoft.com/agent-framework/wp-content/uploads/sites/78/2026/04/Screenshot-2026-04-02-090737-300x170.webp)](https://devblogs.microsoft.com/agent-framework/wp-content/uploads/sites/78/2026/04/Screenshot-2026-04-02-090737.webp)

**Getting Started**

If you’ve been running on the RC packages, upgrading to 1.0 is a package version bump. If you’re new to Agent Framework, install and go:

**Python**

pip install agent-framework

**.NET**

dotnet add package Microsoft.Agents.AI

Check out the [quickstart guide](https://learn.microsoft.com/en-us/agent-framework/get-started/) for walkthroughs in both languages, or jump straight to the [samples on GitHub](https://github.com/microsoft/agent-framework).

Coming from AutoGen or Semantic Kernel? Now is the time to migrate to Microsoft Agent Framework. The [Semantic Kernel migration guide](https://learn.microsoft.com/en-us/agent-framework/migration-guide/from-semantic-kernel) and [AutoGen migration guide](https://learn.microsoft.com/en-us/agent-framework/migration-guide/from-autogen) provide detailed walkthroughs.

**What’s Next**

Version 1.0 is a beginning, not a destination. We’re continuing to invest in graduating preview features, expanding the connector and skills ecosystem, deepening Foundry integration, and incorporating the latest orchestration research from Microsoft Research. The framework is 100% open source – we build in the open, and your feedback and contributions shape what comes next.

Thank you to everyone who filed issues, submitted PRs, tested release candidates, and pushed us to make Agent Framework better. This milestone belongs to the community as much as it does to the team.

**Get started today:**

- **Download the SDK:** [aka.ms/AgentFramework](https://aka.ms/AgentFramework)

- **Documentation:** [aka.ms/AgentFramework/Docs](https://aka.ms/AgentFramework/Docs)

- **GitHub:** [github.com/microsoft/agent-framework](https://github.com/microsoft/agent-framework)

- **Discord:** [aka.ms/foundry/discord](https://aka.ms/foundry/discord)

- **NuGet (.NET):** [Microsoft.Agents.AI](https://www.nuget.org/packages/Microsoft.Agents.AI/)

- **PyPI (Python):** [agent-framework](https://pypi.org/project/agent-framework/)

 
 

Category

Topics

## Author

![Shawn Henry](https://devblogs.microsoft.com/agent-framework/wp-content/uploads/sites/78/2024/10/ShawnHenry-96x96.jpg)

Principal Group Product Manager

Principal Group Product Manager