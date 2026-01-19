---
layout: post
title: 'Agentic Development Best Practices with Microsoft Agent Framework: AG-UI, DevUI & OpenTelemetry'
author: Kinfey Lo
canonical_url: https://devblogs.microsoft.com/semantic-kernel/the-golden-triangle-of-agentic-development-with-microsoft-agent-framework-ag-ui-devui-opentelemetry-deep-dive/
viewing_mode: external
feed_name: Microsoft Semantic Kernel Blog
feed_url: https://devblogs.microsoft.com/semantic-kernel/feed/
date: 2025-12-01 17:08:48 +00:00
permalink: /ai/news/Agentic-Development-Best-Practices-with-Microsoft-Agent-Framework-AG-UI-DevUI-and-OpenTelemetry
tags:
- .NET
- AG UI
- Agent Framework
- Agent Workflow
- Agentic Development
- Agents
- CopilotKit Integration
- Developer Tools
- DevUI
- Distributed Tracing
- GitHub Models
- LLM Debugging
- Microsoft Agent Framework
- Multi Agent Systems
- Observability
- OpenTelemetry
- Python
section_names:
- ai
- coding
---
Kinfey Lo presents a comprehensive overview of agentic development with Microsoft Agent Framework, detailing how DevUI, AG-UI, and OpenTelemetry accelerate building, debugging, and deploying advanced AI agents in Python and .NET.<!--excerpt_end-->

# Agentic Development with Microsoft Agent Framework: AG-UI, DevUI & OpenTelemetry Deep Dive

Kinfey Lo explores best practices for building intelligent, multi-agent applications in the era of Agentic AI using Microsoft's Agent Framework. The article introduces a progressive stack—the “Golden Triangle”: DevUI for visual debugging, AG-UI for rapid front-end integration, and OpenTelemetry for performance insights—illustrated via the GHModel.AI sample.

## Key Agentic Development Challenges

- **Black-Box Execution**: Difficulty in understanding agent decisions and stuck states.
- **Interaction Silos**: The friction of productizing agents into demos and stakeholder-facing UIs.
- **Performance Blind Spots**: Lacking clarity into performance bottlenecks and token consumption.

## Phase 1: Model Creation and Prototyping

- Utilize **GitHub Models** for zero-barrier access to GPT-4o, Llama 3, and more—minimizing setup complexity.
- Switch model providers easily with Agent Framework abstractions in Python or .NET.
- Example code snippets show agent definitions and workflow setup, emphasizing clean, declarative agent creation.

## Phase 2: Debugging with DevUI

- **DevUI** provides:
  - Chain-of-thought visualization, making each step of agent reasoning and action transparent.
  - Real-time memory and state inspection, diagnosing context overflow and hallucinations.
- Available for Python and .NET agents, instantly accessible at local endpoints (http://localhost:8090 / https://localhost:50516/devui).

## Phase 3: Demoing and User Interaction via AG-UI

- **AG-UI** enables standardized front-end agent protocols—no need for custom React UIs.
- Supports streaming responses, backend tool rendering, human-in-the-loop actions, and CopilotKit integration.
- Simple Python and .NET server examples demonstrate AG-UI endpoint registration.

## Phase 4: Observability with OpenTelemetry

- Integrated **OpenTelemetry** provides distributed tracing and cost monitoring.
- Enables flame graphs, latency breakdowns, and token metrics essential for transitioning from prototyping to production (e.g., moving to Azure OpenAI).
- Quick setup examples for Python and .NET; supports visualization tools Aspire Dashboard, Azure Application Insights, Grafana.

## Best Practice Architecture

| Layer               | Tool               | Purpose                                |
|---------------------|--------------------|----------------------------------------|
| **Model Layer**     | GitHub Models      | Rapid AI prototyping                   |
| **Debug Layer**     | DevUI              | Visual agent reasoning, fast iteration |
| **Presentation**    | AG-UI/CopilotKit   | Out-of-box user interfaces             |
| **Observability**   | OpenTelemetry      | Performance transparency, cost control |

## Resources

- [Microsoft Agent Framework](https://github.com/microsoft/agent-framework)
- [Agent Framework Samples](https://github.com/microsoft/Agent-Framework-Samples)
- [DevUI Getting Started](https://github.com/microsoft/agent-framework/tree/main/python/samples/getting_started/devui)
- [Observability Samples](https://github.com/microsoft/agent-framework/tree/main/python/samples/getting_started/observability)

This stack ultimately empowers developers to build, debug, and deploy multi-agent applications efficiently, reducing friction at every step of the process.

This post appeared first on "Microsoft Semantic Kernel Blog". [Read the entire article here](https://devblogs.microsoft.com/semantic-kernel/the-golden-triangle-of-agentic-development-with-microsoft-agent-framework-ag-ui-devui-opentelemetry-deep-dive/)
