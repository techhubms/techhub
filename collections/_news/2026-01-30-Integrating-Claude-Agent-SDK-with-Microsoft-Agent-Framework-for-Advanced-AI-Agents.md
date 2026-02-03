---
layout: "post"
title: "Integrating Claude Agent SDK with Microsoft Agent Framework for Advanced AI Agents"
description: "This article demonstrates how developers can leverage the integration between Microsoft Agent Framework and the Claude Agent SDK to build powerful, agentic AI applications in Python. It covers consistent agent abstractions, multi-agent workflow orchestration, and hands-on code examples—including Azure OpenAI and GitHub Copilot interoperability."
author: "Dmytro Struk"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/semantic-kernel/build-ai-agents-with-claude-agent-sdk-and-microsoft-agent-framework/"
viewing_mode: "external"
feed_name: "Microsoft Semantic Kernel Blog"
feed_url: "https://devblogs.microsoft.com/semantic-kernel/feed/"
date: 2026-01-30 19:21:49 +00:00
permalink: "/2026-01-30-Integrating-Claude-Agent-SDK-with-Microsoft-Agent-Framework-for-Advanced-AI-Agents.html"
categories: ["AI", "Azure", "Coding"]
tags: ["Agent Abstraction", "Agent Framework", "AI", "AI Agents", "Async Programming", "Azure", "Azure OpenAI", "Claude Agent SDK", "Coding", "Function Calling", "MCP Integration", "Microsoft Agent Framework", "Multi Agent Orchestration", "News", "Python", "SDK Integration", "Streaming Responses", "Workflow Automation"]
tags_normalized: ["agent abstraction", "agent framework", "ai", "ai agents", "async programming", "azure", "azure openai", "claude agent sdk", "coding", "function calling", "mcp integration", "microsoft agent framework", "multi agent orchestration", "news", "python", "sdk integration", "streaming responses", "workflow automation"]
---

Dmytro Struk explains how to utilize the Claude Agent SDK and Microsoft Agent Framework together to build advanced AI agents. The article features practical examples in Python and explores multi-agent workflows, including integration with Azure OpenAI.<!--excerpt_end-->

# Integrating Claude Agent SDK with Microsoft Agent Framework for Advanced AI Agents

*Author: Dmytro Struk*

The Microsoft Agent Framework now supports direct integration with the Claude Agent SDK. This enables developers to harness Claude’s full suite of agentic features—such as code execution, file editing, function calling, streaming responses, multi-turn conversations, and Model Context Protocol (MCP) server integration—directly within a consistent agent abstraction provided by Agent Framework, all within Python.

## Why Use the Agent Framework with Claude Agent SDK?

Using the Microsoft Agent Framework's BaseAgent interface, you can:

- Swap or combine Claude, Azure OpenAI, GitHub Copilot, and other agents in unified workflows
- Create sequential, concurrent, handoff, and group chat interaction patterns
- Leverage ecosystem components like declarative agent definitions, A2A protocol, and consistent session/tooling patterns

## Getting Started

Install the integration for Python:

```sh
pip install agent-framework-claude --pre
```

## Basic Usage Example: Creating a Claude Agent

```python
from agent_framework_claude import ClaudeAgent

async def main():
    async with ClaudeAgent(
        instructions="You are a helpful assistant.",
    ) as agent:
        response = await agent.run("What is Microsoft Agent Framework?")
        print(response.text)
```

## Enabling Built-in Tools

Activate capabilities like file operations and shell commands:

```python
from agent_framework_claude import ClaudeAgent

async def main():
    async with ClaudeAgent(
        instructions="You are a helpful coding assistant.",
        tools=["Read", "Write", "Bash", "Glob"],
    ) as agent:
        response = await agent.run("List all Python files in the current directory")
        print(response.text)
```

## Extending With Custom Function Tools

Add your own logic:

```python
from typing import Annotated
from pydantic import Field
from agent_framework_claude import ClaudeAgent

def get_weather(location: Annotated[str, Field(description="The location to get the weather for.")]) -> str:
    return f"The weather in {location} is sunny with a high of 25C."

async def main():
    async with ClaudeAgent(
        instructions="You are a helpful weather agent.",
        tools=[get_weather],
    ) as agent:
        response = await agent.run("What's the weather like in Seattle?")
        print(response.text)
```

## Streaming Responses

Stream responses for enhanced interactivity:

```python
from agent_framework_claude import ClaudeAgent

async def main():
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

Maintain conversational context over sessions:

```python
from agent_framework_claude import ClaudeAgent

async def main():
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

## Permission Modes and MCP Server Integration

Control file/command permissions and connect to MCP servers for extra power:

```python
from agent_framework_claude import ClaudeAgent

async def main():
    async with ClaudeAgent(
        instructions="You are a coding assistant that can edit files.",
        tools=["Read", "Write", "Bash"],
        default_options={"permission_mode": "acceptEdits"},
    ) as agent:
        response = await agent.run("Create a hello.py file that prints 'Hello, World!'")
        print(response.text)

    # Integrating an external MCP server
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

## Multi-Agent Workflows: Claude, Azure OpenAI, and More

The framework allows orchestration across agents from different providers—for instance, using Azure OpenAI for drafting and Claude for review:

```python
import asyncio
from typing import cast
from agent_framework import ChatMessage, Role, SequentialBuilder, WorkflowOutputEvent
from agent_framework.azure import AzureOpenAIChatClient
from agent_framework_claude import ClaudeAgent
from azure.identity import AzureCliCredential

async def main():
    chat_client = AzureOpenAIChatClient(credential=AzureCliCredential())
    writer = chat_client.as_agent(
        instructions="You are a concise copywriter. Provide a single, punchy marketing sentence based on the prompt.",
        name="writer",
    )
    reviewer = ClaudeAgent(
        instructions="You are a thoughtful reviewer. Give brief feedback on the previous assistant message.",
        name="reviewer",
    )
    workflow = SequentialBuilder().participants([writer, reviewer]).build()
    async for event in workflow.run_stream("Write a tagline for a budget-friendly electric bike."):
        if isinstance(event, WorkflowOutputEvent):
            messages = cast(list[ChatMessage], event.data)
            for msg in messages:
                name = msg.author_name or ("assistant" if msg.role == Role.ASSISTANT else "user")
                print(f"[{name}]: {msg.text}\n")

asyncio.run(main())
```

You can expand this to parallel (concurrent), handoff, or group chat workflows, seamlessly integrating agents like Claude, Azure OpenAI, and GitHub Copilot.

## Further Resources

- [Claude Agent SDK](https://github.com/anthropics/claude-agent-sdk-python)
- [Microsoft Agent Framework on GitHub](https://github.com/microsoft/agent-framework)
- [Agent Framework Tutorials](https://learn.microsoft.com/agent-framework/tutorials/overview)
- [Semantic Kernel Blog](https://devblogs.microsoft.com/semantic-kernel)

## Conclusion

The integration of Claude Agent SDK with Microsoft Agent Framework opens up highly flexible, advanced Python-based AI agentic systems, allowing developers to combine tools, orchestrate workflows, and build practical, powerful applications.

This post appeared first on "Microsoft Semantic Kernel Blog". [Read the entire article here](https://devblogs.microsoft.com/semantic-kernel/build-ai-agents-with-claude-agent-sdk-and-microsoft-agent-framework/)
