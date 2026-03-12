---
layout: "post"
title: "Agent Harness Patterns with Microsoft Agent Framework: Shell Execution and Context Compaction in Python and .NET"
description: "This article offers a practical guide to building robust agent systems using Microsoft Agent Framework. It covers local and hosted shell execution harnesses, approval workflows, and advanced context management strategies across Python and .NET, enabling developers to create agents that bridge model reasoning with real-world execution scenarios securely and efficiently."
author: "Dmytro Struk, Chris Rickman, Eduard van Valkenburg"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/agent-framework/agent-harness-in-agent-framework/"
viewing_mode: "external"
feed_name: "Microsoft Semantic Kernel Blog"
feed_url: "https://devblogs.microsoft.com/semantic-kernel/feed/"
date: 2026-03-12 19:30:26 +00:00
permalink: "/2026-03-12-Agent-Harness-Patterns-with-Microsoft-Agent-Framework-Shell-Execution-and-Context-Compaction-in-Python-and-NET.html"
categories: ["AI", "Coding"]
tags: [".NET", "Agent Framework", "AI", "AI Agent", "Approval Flow", "Code Samples", "Coding", "Context Compaction", "Filesystem Access", "in Memory History", "Microsoft Agents", "News", "OpenAI", "Python", "Session Management", "Shell Execution", "Tool Integration", "Workflow Automation"]
tags_normalized: ["dotnet", "agent framework", "ai", "ai agent", "approval flow", "code samples", "coding", "context compaction", "filesystem access", "in memory history", "microsoft agents", "news", "openai", "python", "session management", "shell execution", "tool integration", "workflow automation"]
---

Dmytro Struk, Chris Rickman, and Eduard van Valkenburg present hands-on techniques for building production-ready AI agents using the Microsoft Agent Framework, highlighting secure shell execution and conversational context management.<!--excerpt_end-->

# Agent Harness Patterns with Microsoft Agent Framework: Shell Execution and Context Compaction in Python and .NET

**Authors:** Dmytro Struk, Chris Rickman, Eduard van Valkenburg

Agent harnessing is a critical layer in modern agent systems, connecting advanced language model reasoning to actions in real computing environments. The Microsoft Agent Framework standardizes building these integrations for both Python and .NET developers.

This article introduces three essential patterns for practical agent development:

- **Local shell harness**: Secure execution of host-side shell and filesystem operations with explicit approval mechanisms.
- **Hosted shell harness**: Managed execution inside sandboxed, containerized, or cloud environments.
- **Context compaction**: Techniques for managing session history to keep multi-turn conversations responsive and efficient.

## Shell and Filesystem Harness with Approval Flows

Agents often need capabilities that go beyond simple text generation, such as running shell commands or accessing files on the system. The Agent Framework provides explicit tools and approval workflows to safely enable such actions.

### Python Example: Local Shell Execution with Approval

```python
import asyncio
import subprocess
from agent_framework import Agent, Message, tool
from agent_framework.openai import OpenAIResponsesClient

@tool(approval_mode="always_require")
def run_bash(command: str) -> str:
    result = subprocess.run(command, shell=True, capture_output=True, text=True, timeout=30)
    parts = []
    if result.stdout:
        parts.append(result.stdout)
    if result.stderr:
        parts.append(f"stderr: {result.stderr}")
    parts.append(f"exit_code: {result.returncode}")
    return "\n".join(parts)
```

This function executes shell commands with enforced approval before running, increasing security:

- **Approval required:** Every shell command is checked before execution.
- **Best practice:** Run in an isolated environment and always require user approval.

### Python Example: Hosted Shell Execution

```python
import asyncio
from agent_framework import Agent
from agent_framework.openai import OpenAIResponsesClient

async def main():
    client = OpenAIResponsesClient(model_id="<responses-model-id>", api_key="<your-openai-api-key>")
    shell_tool = client.get_shell_tool()
    agent = Agent(client=client, instructions="You are a helpful assistant that can execute shell commands.", tools=shell_tool)
    result = await agent.run("Use a shell command to show the current date and time")
    print(result)
```

With a hosted setup, commands are executed in a managed, sandboxed environment.

### .NET Example: Local Shell with Approvals

```csharp
using System.Diagnostics;
using Microsoft.Agents.AI;

[Description("Execute a shell command locally and return stdout, stderr and exit code.")]
static string RunBash(string command) { /* ... */ }

IChatClient chatClient = new OpenAIClient(apiKey)
    .GetResponsesClient(model)
    .AsIChatClient();

AIAgent agent = chatClient.AsAIAgent(name: "LocalShellAgent", instructions: "Use tools when needed. Avoid destructive commands.", tools: [new ApprovalRequiredAIFunction(AIFunctionFactory.Create(RunBash, name: "run_bash"))]);
// ... approval flow as in the Python example ...
```

### .NET Example: Hosted Shell with Protocol-Level Config

```csharp
IChatClient chatClient = new OpenAIClient(apiKey)
    .GetResponsesClient(model)
    .AsIChatClient();

// Define a hosted shell execution environment
CreateResponseOptions hostedShellOptions = new();
hostedShellOptions.Patch.Set(
    "$.tools"u8,
    BinaryData.FromObjectAsJson(new object[] {
        new { type = "shell", environment = new { type = "container_auto" } }
    })
);
```

This enables executing shell commands in containerized or managed environments, mitigating host security risks.

---

## Context Compaction: Managing Conversational State

Long-running agent interactions can accumulate excessive context, burdening memory and model token limits. The Agent Framework provides compaction strategies for effective context management.

### Python Example: Sliding Window Context Compaction

```python
from agent_framework import Agent, InMemoryHistoryProvider, SlidingWindowStrategy, tool
from agent_framework.openai import OpenAIChatClient

# Define your compaction strategy

compaction_strategy = SlidingWindowStrategy(keep_last_groups=3)

agent = Agent(client=client, ... context_providers=[InMemoryHistoryProvider()], compaction_strategy=compaction_strategy)
```

Maintains only the most recent set of message groups or turns.

### .NET Example: Multi-Strategy Compaction Pipeline

```csharp
using Microsoft.Agents.AI.Compaction;

// Set up a pipeline of compaction strategies
PipelineCompactionStrategy compactionPipeline = new(
    new ToolResultCompactionStrategy(CompactionTriggers.MessagesExceed(7)),
    new SlidingWindowCompactionStrategy(CompactionTriggers.TurnsExceed(4)),
    new TruncationCompactionStrategy(CompactionTriggers.GroupsExceed(12))
);

AIAgent agent = chatClient
    .AsBuilder()
    .UseAIContextProviders(new CompactionProvider(compactionPipeline))
    .BuildAIAgent(new ChatClientAgentOptions { ... });
```

Combining these strategies allows fine-grained control over historical context management for efficient, cost-conscious agent systems.

---

## Conclusion and Next Steps

The Microsoft Agent Framework now enables:

- Secure, controlled execution of shell and filesystem operations (local or hosted)
- Robust long-term session context management through compaction

These patterns help developers build agents capable of bridging the gap between model reasoning and practical execution, both in Python and in .NET. For more examples and resources, see the [official documentation](https://learn.microsoft.com/en-us/agent-framework/) and sample code on [GitHub](https://github.com/microsoft/agent-framework). You can install the latest libraries via [NuGet (.NET)](https://www.nuget.org/packages/Microsoft.Agents.AI/) or [PyPI (Python)](https://pypi.org/project/agent-framework/).

This post appeared first on "Microsoft Semantic Kernel Blog". [Read the entire article here](https://devblogs.microsoft.com/agent-framework/agent-harness-in-agent-framework/)
