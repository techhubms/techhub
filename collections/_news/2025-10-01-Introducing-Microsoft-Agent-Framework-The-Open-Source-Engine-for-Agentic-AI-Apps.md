---
external_url: https://devblogs.microsoft.com/foundry/introducing-microsoft-agent-framework-the-open-source-engine-for-agentic-ai-apps/
title: 'Introducing Microsoft Agent Framework: The Open-Source Engine for Agentic AI Apps'
author: Takuto Higuchi, Shawn Henry, Elijah Straight
feed_name: Microsoft AI Foundry Blog
date: 2025-10-01 13:57:12 +00:00
tags:
- .NET
- A2A
- Agent Orchestration
- Agents
- AI Agent
- AI Agents
- AI Development
- AI Tools
- AutoGen
- Azure AI Foundry
- Azure OpenAI
- Compliance
- Declarative Agents
- Elasticsearch
- Enterprise AI
- MCP
- Microsoft Agent Framework
- Multi Agent Systems
- Observability
- OpenAPI
- OpenTelemetry
- Python SDK
- Redis
- Semantic Kernel
- VS Code AI Toolkit
- Workflow Orchestration
- AI
- Azure
- News
section_names:
- ai
- azure
- dotnet
primary_section: ai
---
Takuto Higuchi, Shawn Henry, and Elijah Straight provide a deep dive into the Microsoft Agent Framework, showing how it helps developers build, manage, and scale AI agent systems with robust tools for enterprise, research, and production.<!--excerpt_end-->

# Introducing Microsoft Agent Framework: The Open-Source Engine for Agentic AI Apps

By Takuto Higuchi, Shawn Henry, Elijah Straight

## Why Agents Need a New Foundation

AI agents are emerging as autonomous software components that go beyond chatbots or copilots—they can reason about objectives, call external APIs, collaborate, and adapt dynamically. However, taking them from prototype to production has exposed major gaps: fragmented frameworks, poor cloud/deployment mapping, and lack of enterprise-readiness (compliance, observability, security).

Microsoft’s experience with projects like Semantic Kernel (stable SDK, enterprise connectors) and AutoGen (experimental multi-agent orchestration) revealed developer demand for a unified solution combining innovation with production stability.

The result: the [Microsoft Agent Framework](https://aka.ms/AgentFramework) — an open-source SDK and runtime that brings together the strengths of both prior projects for streamlined development, testing, deployment, and management of agentic systems.

## Key Features of Microsoft Agent Framework

**1. Open Standards & Interoperability**

- Fully supports MCP (Model Context Protocol), A2A (Agent-to-Agent), and OpenAPI for portable, vendor-neutral systems.
- Cloud-agnostic: works in containers, on-prem, or multi-cloud environments.
- Pluggable memory and connectors make it easy to integrate enterprise data from sources such as Azure AI Foundry, Microsoft Fabric, SharePoint, Oracle, MongoDB, and more.
- Enhanced developer experience in VS Code via the latest [AI Toolkit](https://aka.ms/aitoolkit).

**2. Pipeline for Research-to-Production**

- Incorporates proven orchestration patterns from Microsoft Research, such as sequential, concurrent, group chat, and handoff orchestration.
- Experimental features channel for rapid prototyping and innovation.

**3. Extensible & Community-Driven**

- Open source and modular, supporting YAML/JSON declarative definitions, pluggable memory backends (Redis, Pinecone, Qdrant, Elasticsearch, Postgres), and community-contributed orchestration strategies.
- Community participation shapes the framework.

**4. Enterprise-Ready Architecture**

- End-to-end observability via [OpenTelemetry](https://aka.ms/MultiAgentTracingBlog), with integration into Azure AI Foundry dashboards.
- Security/compliance via Entra ID, Azure AI Content Safety, role-based access, private data handling.
- Supports long-running, durable workflows, CI/CD pipelines (GitHub Actions, Azure DevOps), human-in-the-loop operations, and error resilience.

## Migration and Integration

- **Semantic Kernel users:** Migrate by replacing Kernel/plugs with new Agent and Tool abstractions. .NET developers use Microsoft.Extensions.AI.* namespaces; Python users install via pip. Vector data and tools migrate directly.
- **AutoGen users:** Orchestration, agent roles, and messaging migrate smoothly to new Workflow APIs and ChatAgent abstractions. Observability and composability improved.
- Existing Microsoft 365, Teams, and Azure AI integrations are supported and converging in the shared agent runtime.

## Real-World Adoption

Enterprises such as KPMG, Commerzbank, BMW, Fujitsu, Citrix, Fractal, TCS, Sitecore, NTT Data, MTech Systems, TeamViewer, Weights & Biases, and Elastic are piloting and deploying solutions with Microsoft Agent Framework. Use cases span audit automation, avatar-driven support, vehicle telemetry analysis, human-in-the-loop workflows, and more—showing both innovation and enterprise stability.

## Get Started

- [Download the SDK](https://aka.ms/AgentFramework)
- [Read the documentation](https://aka.ms/AgentFramework/Docs)
- [View step-by-step learning paths](https://learn.microsoft.com/en-us/training/paths/develop-ai-agents-on-azure/)
- [Join Azure AI Foundry Discord](https://aka.ms/foundry/discord)

Agentic AI is rapidly becoming the next foundational layer for application logic, and Microsoft Agent Framework positions developers to leverage the full power of open, extensible, and production-ready multi-agent systems.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/introducing-microsoft-agent-framework-the-open-source-engine-for-agentic-ai-apps/)
