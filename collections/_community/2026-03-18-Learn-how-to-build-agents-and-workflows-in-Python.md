---
title: Learn how to build agents and workflows in Python
author: Pamela_Fox
feed_name: Microsoft Tech Community
date: 2026-03-18 07:00:00 +00:00
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/learn-how-to-build-agents-and-workflows-in-python/ba-p/4502144
section_names:
- ai
- azure
tags:
- Agentic Workflows
- AI
- AI Agents
- Aspire Dashboard
- Azure
- Azure AI Evaluation SDK
- Checkpoints
- Community
- Conditional Branching
- DevUI
- GitHub Models
- Human in The Loop
- Long Term Memory
- MCP Servers
- Mem0
- Microsoft Agent Framework
- Microsoft Foundry Models
- Multi Agent Orchestration
- Observability
- OpenTelemetry
- PostgreSQL
- Python
- RAG
- Redis
- Retrieval Augmented Generation
- SQLite
- Structured Outputs
- Subagents
- Supervisor Agent Pattern
- Tool Approval
- Tool Calling
primary_section: ai
---

Pamela_Fox shares a six-part “Python + Agents” livestream series recap with links to recordings, slides, and runnable code that teach how to build AI agents and agentic workflows in Python using the Microsoft Agent Framework, including context/memory, observability, evaluation, orchestration patterns, and human-in-the-loop checkpoints.<!--excerpt_end-->

# Learn how to build agents and workflows in Python

Pamela_Fox recaps **Python + Agents**, a six-part livestream series on building **AI agents in Python** with the **Microsoft Agent Framework**, and links all learning materials (videos, slides, and code).

## What the series covers

- Building agents with:
  - **Tools** (tool calling)
  - **MCP servers**
  - **Subagents**
- Adding **context** and **long-term memory**:
  - Database calls (examples mentioned: **SQLite**, **PostgreSQL**)
  - Persistent memory using **Redis** or **Mem0**
- **Monitoring** and **evaluation**:
  - Observability with **OpenTelemetry**
  - Evaluation with the **Azure AI Evaluation SDK**
  - Using a local **Aspire dashboard** to inspect traces/metrics/logs
- **AI-driven workflows**:
  - Conditional branching
  - **Structured outputs** for more reliable branching
  - **Multi-agent orchestration**
- **Human-in-the-loop (HITL)**:
  - Tool approval
  - Checkpoints and resuming long-running workflows

## Materials (recordings, slides, code)

The post links:

- **Video recordings** for each session
- **PowerPoint slides** you can reuse to review/teach
- **Open-source code samples** runnable with frontier LLMs from **GitHub Models** or **Microsoft Foundry Models**

Spanish version is also available: https://aka.ms/pythonagentes/recursos

Follow-up Q&A options:

- Weekly Python+AI office hours: http://aka.ms/aipython/oh
- Weekly Agent Framework office hours: https://github.com/microsoft/agent-framework/blob/main/COMMUNITY.md#public-community-office-hours

## Session 1: Building your first agent in Python

Recording: https://www.youtube.com/watch?v=I4vCp9cpsiI

Covers:

- Agent fundamentals: what agents are and how they work
- Tool calling progression:
  - Single tool → multiple tools → tools exposed via **local MCP servers**
- **Supervisor agent** pattern:
  - A supervisor coordinates subtasks across multiple subagents by treating each agent as a tool
- Debugging/inspection tips:
  - Using the Agent Framework **DevUI** interface for interacting with prototypes

Resources:

- Slides: https://aka.ms/pythonagents/slides/building
- Code repo: https://github.com/Azure-Samples/python-agentframework-demos
- Write-up: https://github.com/Azure-Samples/python-agentframework-demos/blob/main/presentations/english/session-1/README.md

## Session 2: Adding context and memory to agents

Recording: https://www.youtube.com/watch?v=BMzI9cEaGBM

Covers:

- **Context (RAG)**:
  - Grounding agent responses using knowledge retrieved from local data sources (examples: **SQLite**, **PostgreSQL**)
  - Goal: reduce hallucinations and improve domain accuracy
- **Memory**:
  - Short-term (thread-level) context
  - Long-term persistent memory using **Redis** or **Mem0**
  - Remembering prior interactions, preferences, and evolving tasks across sessions

Resources:

- Slides: https://aka.ms/pythonagents/slides/contextmemory
- Code repo: https://github.com/Azure-Samples/python-agentframework-demos
- Write-up: https://github.com/Azure-Samples/python-agentframework-demos/blob/main/presentations/english/session-2/README.md

## Session 3: Monitoring and evaluating agents

Recording: https://www.youtube.com/watch?v=3yS-G-NEBu8

Covers:

- Observability with **OpenTelemetry**:
  - Capturing traces, metrics, and logs from agent actions
  - Using a local **Aspire dashboard** to identify slowdowns and failures
- Evaluation with **Azure AI Evaluation SDK**:
  - Defining evaluation criteria
  - Running automated assessments over a task set
  - Analyzing results for accuracy/helpfulness/task success

Resources:

- Slides: https://aka.ms/pythonagents/slides/monitoreval
- Code repo: https://github.com/Azure-Samples/python-agentframework-demos
- Write-up: https://github.com/Azure-Samples/python-agentframework-demos/blob/main/presentations/english/session-3/README.md

## Session 4: Building your first AI-driven workflows

Recording: https://www.youtube.com/watch?v=FQtZCKWjARI

Covers:

- Workflow fundamentals in Microsoft Agent Framework:
  - Defining workflow steps, connecting them, and passing data
  - Core components: **executors**, **edges**, **events**
- Building steps as:
  - Plain Python functions
  - Full AI agents when model-driven behavior is needed
- Branching:
  - **Conditional branching** based on model outputs/intermediate results/decision functions
  - **Structured outputs** to avoid brittle string checks and to branch on typed data
- Dev experience:
  - Using **DevUI** to visualize the workflow graph and streaming events
- A demo application:
  - E2E app using workflows inside a user-facing frontend/backend

Resources:

- Slides: https://aka.ms/pythonagents/slides/workflows
- Code repo: https://github.com/Azure-Samples/python-agentframework-demos
- Write-up: https://github.com/Azure-Samples/python-agentframework-demos/blob/main/presentations/english/session-4/README.md

## Session 5: Orchestrating advanced multi-agent workflows

Recording: https://www.youtube.com/watch?v=WtZbDrd-RJg

Covers:

- Advanced orchestration patterns:
  - Sequential vs concurrent execution
  - Parallel workflow steps
  - **Fan-out/fan-in** edges to run branches concurrently and aggregate results
- Built-in multi-agent orchestration approaches:
  - **Handoff**: control transfers from one agent to another based on workflow logic
  - **Magentic**: a planning-oriented supervisor that creates a high-level plan and delegates parts to other agents
- Demo:
  - E2E concurrent multi-agent workflow example

Resources:

- Slides: https://aka.ms/pythonagents/slides/advancedworkflows
- Code repo: https://github.com/Azure-Samples/python-agentframework-demos
- Write-up: https://github.com/Azure-Samples/python-agentframework-demos/blob/main/presentations/english/session-5/README.md

## Session 6: Adding a human in the loop to agentic workflows

Recording: https://www.youtube.com/watch?v=7pGqASn-LEY

Covers:

- Human-in-the-loop (HITL) for reliability when LLM outputs are uncertain/inconsistent
- Framework model for HITL:
  - **Requests-and-responses** for pausing workflows, collecting human input, then continuing
- Common HITL pattern:
  - **Tool approval** (approve/reject pending tool calls)
- Long-running scenarios:
  - **Checkpoints** and **resuming** execution later
  - How progress is stored and resumed for multi-step review cycles

Resources:

- Slides: https://aka.ms/pythonagents/slides/hitl
- Code repo: https://github.com/Azure-Samples/python-agentframework-demos
- Write-up: https://github.com/Azure-Samples/python-agentframework-demos/blob/main/presentations/english/session-6/README.md


[Read the entire article](https://techcommunity.microsoft.com/t5/microsoft-developer-community/learn-how-to-build-agents-and-workflows-in-python/ba-p/4502144)

