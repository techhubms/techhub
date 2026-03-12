---
layout: "post"
title: "Build AI Agents with Claude Agent SDK and Microsoft Agent Framework"
description: "This article provides a comprehensive overview of integrating the Claude Agent SDK with the Microsoft Agent Framework. It outlines the benefits of consistent agent abstraction, access to multi-agent workflows, built-in and custom tooling, and orchestrated agent collaboration. Detailed Python code examples show how to install, configure, and compose Claude and Azure OpenAI agents, build multi-agent workflows, set permission modes, and connect to Model Context Protocol (MCP) servers, enabling developers to build powerful, agentic AI applications on Microsoft's platform."
author: "Dmytro Struk"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/agent-framework/build-ai-agents-with-claude-agent-sdk-and-microsoft-agent-framework/"
viewing_mode: "external"
feed_name: "Microsoft Semantic Kernel Blog"
feed_url: "https://devblogs.microsoft.com/semantic-kernel/feed/"
date: 2026-01-30 19:21:49 +00:00
permalink: "/2026-01-30-Build-AI-Agents-with-Claude-Agent-SDK-and-Microsoft-Agent-Framework.html"
categories: ["AI"]
tags: ["Agent Framework", "Agent Orchestration", "AI", "AI Agents", "Azure OpenAI", "Claude Agent SDK", "File Operations", "Function Calling", "MCP", "Microsoft Agent Framework", "Multi Agent Systems", "Multi Turn Conversation", "News", "Python", "Shell Commands", "Streaming Responses", "Workflow Automation"]
tags_normalized: ["agent framework", "agent orchestration", "ai", "ai agents", "azure openai", "claude agent sdk", "file operations", "function calling", "mcp", "microsoft agent framework", "multi agent systems", "multi turn conversation", "news", "python", "shell commands", "streaming responses", "workflow automation"]
---

Dmytro Struk explains how to build agentic AI applications by integrating the Claude Agent SDK with Microsoft Agent Framework, covering installation, Python examples, and orchestration features.<!--excerpt_end-->

# Build AI Agents with Claude Agent SDK and Microsoft Agent Framework

Microsoft Agent Framework now integrates with the [Claude Agent SDK](https://github.com/anthropics/claude-agent-sdk-python), letting you build AI agents in Python that leverage Claude’s agentic features inside a consistent Microsoft abstraction. This integration lets you combine Claude with other agent providers (such as Azure OpenAI and GitHub Copilot) in multi-agent workflows—using a unified ecosystem with orchestration, built-in tools, and consistent patterns for function tools, session management, streaming, and more.

## Why Use Agent Framework with Claude Agent SDK?

- **Consistent agent abstraction:** Claude agents implement the same `BaseAgent` interface as all other agents in Microsoft Agent Framework, letting you swap or combine different providers with minimal code changes.
- **Multi-agent workflows:** Compose Claude agents with other providers (Azure OpenAI, GitHub Copilot, etc.) using built-in workflow orchestrators for sequential, concurrent, handoff, and group chat scenarios.
- **Ecosystem integration:** Access declarative agent definitions, A2A protocol support, and standard patterns for tools, session management, and streaming across all providers.
- **Scalable systems:** Treat Claude as a component in larger, scalable agentic applications instead of a single-use tool.

## Installation

To install the integration for Python:

```bash
pip install agent-framework-claude --pre
```

## Create a Claude Agent

Instantiate a ClaudeAgent and interact with it using the async context manager pattern:

```python
from agent_framework_claude import ClaudeAgent

async def main():
    async with ClaudeAgent(
        instructions="You are a helpful assistant.",
    ) as agent:
        response = await agent.run("What is Microsoft Agent Framework?")
        print(response.text)
```

## Using Built-in Tools

Claude Agent SDK enables file operations, shell commands, and more by specifying tool names:

```python
async with ClaudeAgent(
    instructions="You are a helpful coding assistant.",
    tools=["Read", "Write", "Bash", "Glob"],
) as agent:
    response = await agent.run("List all Python files in the current directory")
    print(response.text)
```

## Add Function Tools

Add custom Python functions as tools to provide new capabilities:

```python
from typing import Annotated
from pydantic import Field

# Custom tool

def get_weather(location: Annotated[str, Field(description="The location to get the weather for.")]) -> str:
    return f"The weather in {location} is sunny with a high of 25C."

async with ClaudeAgent(
    instructions="You are a helpful weather agent.",
    tools=[get_weather],
) as agent:
    response = await agent.run("What's the weather like in Seattle?")
    print(response.text)
```

## Stream Responses

Enable real-time feedback with streaming outputs:

```python
async with ClaudeAgent(
    instructions="You are a helpful assistant.",
) as agent:
    print("Agent: ", end="", flush=True)
    async for chunk in agent.run_stream("Tell me a short story."):
        if chunk.text:
            print(chunk.text, end="", flush=True)
    print()
```

## Multi-Turn Conversations

Maintain conversation context with session-managed threads:

```python
async with ClaudeAgent(
    instructions="You are a helpful assistant. Keep your answers short.",
) as agent:
    thread = agent.get_new_thread()
    # First turn
    await agent.run("My name is Alice.", thread=thread)
    # Second turn
    response = await agent.run("What is my name?", thread=thread)
    print(response.text) # Should mention "Alice"
```

## Configure Permission Modes

Control permission handling for file and command operations with `permission_mode`:

```python
async with ClaudeAgent(
    instructions="You are a coding assistant that can edit files.",
    tools=["Read", "Write", "Bash"],
    default_options={
        "permission_mode": "acceptEdits",  # Auto-accept file edits
    },
) as agent:
    response = await agent.run("Create a hello.py file that prints 'Hello, World!'")
    print(response.text)
```

## Connect MCP Servers

Enable agents to use external MCP servers for broader tool and data access:

```python
async with ClaudeAgent(
    instructions="You are a helpful assistant with access to the filesystem.",
    default_options={
        "mcp_servers": {
            "filesystem": {
                "command": "npx",
                "args": ["-y", "@modelcontextprotocol/server-filesystem", "."],
            },
        },
    },
) as agent:
    response = await agent.run("List all files in the current directory using MCP")
    print(response.text)
```

## Multi-Agent Workflows with Azure OpenAI and Claude

Compose Claude agents with Azure OpenAI (or other providers) using workflow orchestration:

```python
import asyncio
from typing import cast
from agent_framework import ChatMessage, Role, SequentialBuilder, WorkflowOutputEvent
from agent_framework.azure import AzureOpenAIChatClient
from agent_framework_claude import ClaudeAgent
from azure.identity import AzureCliCredential

async def main():
    # Azure OpenAI agent
    chat_client = AzureOpenAIChatClient(credential=AzureCliCredential())
    writer = chat_client.as_agent(
        instructions="You are a concise copywriter. Provide a single, punchy marketing sentence based on the prompt.",
        name="writer",
    )
    # Claude agent as reviewer
    reviewer = ClaudeAgent(
        instructions="You are a thoughtful reviewer. Give brief feedback on the previous assistant message.",
        name="reviewer",
    )
    # Build sequential workflow: writer → reviewer
    workflow = SequentialBuilder().participants([writer, reviewer]).build()
    async for event in workflow.run_stream("Write a tagline for a budget-friendly electric bike."):
        if isinstance(event, WorkflowOutputEvent):
            messages = cast(list[ChatMessage], event.data)
            for msg in messages:
                name = msg.author_name or ("assistant" if msg.role == Role.ASSISTANT else "user")
                print(f"[{name}]: {msg.text}\n")
asyncio.run(main())
```

This example demonstrates how different AI agents (Azure OpenAI, Claude) can collaborate in a coordinated workflow using Agent Framework.

## More Information

- [Claude Agent SDK on GitHub](https://github.com/anthropics/claude-agent-sdk-python)
- [Microsoft Agent Framework on GitHub](https://github.com/microsoft/agent-framework)
- [Agent Framework Getting Started Tutorials](https://learn.microsoft.com/agent-framework/tutorials/overview)

## Summary

The Claude Agent SDK integration for Microsoft Agent Framework enables Python developers to build AI agents with full agentic and collaborative capabilities. The unified abstraction allows for rapid composition and robust automation in modern AI applications.

This post appeared first on "Microsoft Semantic Kernel Blog". [Read the entire article here](https://devblogs.microsoft.com/agent-framework/build-ai-agents-with-claude-agent-sdk-and-microsoft-agent-framework/)
