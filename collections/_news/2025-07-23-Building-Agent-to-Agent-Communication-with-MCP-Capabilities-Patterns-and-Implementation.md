---
layout: "post"
title: "Building Agent-to-Agent Communication with MCP: Capabilities, Patterns, and Implementation"
description: "This article examines recent enhancements to Microsoft's Model Context Protocol (MCP), highlighting its advanced support for building agent-to-agent communication systems. It covers key features like streaming, resumability, durability, and multi-turn interactions with practical, code-driven examples for enabling complex, long-running, and autonomous agent workflows."
author: "Victor Dibia, Mike Kistler, Maria Naggaga"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/blog/can-you-build-agent2agent-communication-on-mcp-yes"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/feed"
date: 2025-07-23 17:00:19 +00:00
permalink: "/news/2025-07-23-Building-Agent-to-Agent-Communication-with-MCP-Capabilities-Patterns-and-Implementation.html"
categories: ["AI"]
tags: ["Agent Communication", "AI", "AI Agent", "Elicitation", "MCP", "Microsoft AI", "Multi Turn Interaction", "News", "Progress Updates", "Resumable Streams", "Sampling", "Session Resumption"]
tags_normalized: ["agent communication", "ai", "ai agent", "elicitation", "mcp", "microsoft ai", "multi turn interaction", "news", "progress updates", "resumable streams", "sampling", "session resumption"]
---

Authored by Victor Dibia, Mike Kistler, and Maria Naggaga, this article explores advanced features in Microsoft's Model Context Protocol (MCP), focusing on building robust agent-to-agent communication systems and showcasing practical implementation strategies.<!--excerpt_end-->

# Building Agent-to-Agent Communication with MCP

*By Victor Dibia, Mike Kistler, and Maria Naggaga*

## Overview

MCP (Model Context Protocol) has evolved significantly beyond its initial purpose of providing context to large language models (LLMs). Recent enhancements—including resumable streams, elicitation, sampling, and advanced notifications—have transformed MCP into a robust protocol suitable for building sophisticated agent-to-agent communication systems.

This article explores:

- How to use MCP's capabilities for agent-to-agent communication where both hosts and tools act as intelligent agents.
- Four key capabilities that make MCP tools “agentic”: streaming, resumability, durability, and multi-turn interactions.
- Implementing long-running agents with a complete Python example featuring an MCP server with travel and research agents.

## What’s New in MCP?

The latest upgrades to MCP include:

- **Resumable Streams:** Allows session continuity even after network interruptions.
- **Elicitation:** Enables agents to request human input mid-execution.
- **Sampling:** Allows agents to request AI-generated completions during tasks.
- **Notifications for Progress and Resources:** Supports real-time status updates and persistent resource tracking.

Together, these features empower developers to create complex multi-agent systems on top of MCP.

> **TL;DR:** MCP now supports composing advanced features (like resumable streams, elicitation, sampling, and progress notifications) to facilitate agent-centric, multi-agent applications. [Full codebase](http://aka.ms/mcp-agent-tutorial).

## Misconceptions and Enhancements

There's a common misconception that MCP tools are only fit for simple request-response operations. This is outdated: with the enhancements above, MCP tools can now function as true autonomous agents suitable for long-running and interactive scenarios.

### Capabilities Overview

| Feature                   | Use Case                                                     | MCP Support                                                   |
|---------------------------|--------------------------------------------------------------|---------------------------------------------------------------|
| **Streaming**             | Real-time progress updates and partial result streaming      | Progress notifications; message payload extensions            |
| **Resumability**          | Session continuity after disconnection                       | StreamableHTTP transport, event replay, EventStore            |
| **Durability**            | Persistent state across restarts; polling and notification   | Resource links, persistent storage, status notifications      |
| **Multi-turn Interaction**| User/AI input mid-execution for complex subtasks            | Elicitation (human input), sampling (AI input)                |

## What Makes an MCP Tool “Agentic”?

An agent in the MCP context is any entity capable of autonomously executing extended tasks requiring multiple, dynamic interactions. Ordinary MCP tools become “agentic” by leveraging:

1. **Streaming & Partial Results:**
    - *Use cases:* Codebase migrations, book generation, real-time progress feedback.
    - *MCP support:* Progress notifications; partial results possible through message payloads.

2. **Resumability:**
    - Ensures no loss of task progress after client interruptions via session IDs and event replay.
    - *Implementation requires an EventStore for event replays.*

3. **Durability:**
    - Persistent results through resource links allow clients to poll or subscribe to ongoing task states.
    - *Scaling considerations*: community is exploring webhooks to reduce overhead from polling.

4. **Multi-turn Interaction:**
    - Elicitation requests user input (confirmation/clarification) during task execution.
    - Sampling lets agents request AI completions for sub-tasks.

## Agent Architecture in MCP

Agent-to-agent communication is realized by viewing both host applications and tools as agents:

- Orchestrator Agents (hosts): coordinate tasks, manage state, route intelligently.
- Specialist Agents (tools): act as service endpoints responding to orchestrator directions.

MCP’s enhanced primitives make it possible to build these patterns with off-the-shelf protocol features.

## Practical Implementation: Python Code Examples

### Agent Server Setup

A [complete code repository](http://aka.ms/mcp-agent-tutorial) accompanies this article, demonstrating:

- **Travel Agent:** Simulates bookings with mid-execution price confirmations (elicitation).
- **Research Agent:** Performs research and requests AI-generated summaries (sampling).

**Project Components:**

- `server/server.py`: Resumable MCP server and tools (agents).
- `client/client.py`: Interactive host client with event handlers and token management.
- `server/event_store.py`: In-memory event store supporting event replay and session resumption.

### Key Concepts & Example Code

#### Streaming & Progress Updates

Agents can stream progress updates to provide feedback:

```python
# Server-side (travel agent streaming updates)

for i, step in enumerate(steps):
    await ctx.session.send_progress_notification(
        progress_token=ctx.request_id,
        progress=i * 25,
        total=100,
        message=step,
        related_request_id=str(ctx.request_id)
    )
    await anyio.sleep(2)
```

Client listens for these updates in real-time.

#### Elicitation

Requesting input from the user mid-execution:

```python
# Server-side (travel agent requesting price confirmation)

elicit_result = await ctx.session.elicit(
    message=f"Please confirm the estimated price of $1200 for your trip to {destination}",
    requestedSchema=PriceConfirmationSchema.model_json_schema(),
    related_request_id=ctx.request_id,
)
if elicit_result and elicit_result.action == "accept":
    # Continue with booking
    ...
elif elicit_result and elicit_result.action == "decline":
    # Cancel the booking
    ...
```

#### Sampling

Requesting AI/LMM content for a sub-task:

```python
# Server-side (research agent requesting AI summary)

sampling_result = await ctx.session.create_message(
    messages=[SamplingMessage(role="user", content=TextContent(type="text", text=f"Please summarize the key findings for research on: {topic}"))],
    max_tokens=100,
    related_request_id=ctx.request_id,
)
if sampling_result and sampling_result.content:
    if sampling_result.content.type == "text":
        sampling_summary = sampling_result.content.text
```

#### Resumability & Event Store

Enables seamless continuation after interruptions:

```python
# Simple EventStore for session resumption

class SimpleEventStore(EventStore):
    def __init__(self):
        self._events: list[tuple[StreamId, EventId, JSONRPCMessage]] = []
        self._event_id_counter = 0
    ...
```

Client maintains resumption tokens and can reconnect using stored state for task progression without loss.

## Beyond a Single Server: Multi-Agent, Multi-Server Communication

The architecture supports scaling to multiple MCP servers with an orchestrator agent distributing and correlating tasks. Extensions (not demoed but suggested) include:

- Intelligent task decomposition
- Task state management across concurrent agents
- User context preservation
- Resilience and result synthesis

## Conclusion

Recent enhancements make MCP a powerful framework for building complex, durable, and interactive multi-agent communication systems rooted in Microsoft AI technologies. MCP’s continuous evolution paves the way for more advanced and flexible agent designs.

- Review the [official MCP documentation](https://modelcontextprotocol.io/introduction) for updates
- Explore the [code repository](http://aka.ms/mcp-agent-tutorial) for hands-on examples

**Acknowledgments:** Thank you to Caitie McCaffrey, Marius de Vogel, Donald Thompson, Adam Kaplan, Toby Padilla, Marc Baiza, Harald Kirschner, and others for insights and feedback on this topic.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/blog/can-you-build-agent2agent-communication-on-mcp-yes)
