---
layout: "post"
title: "Building an Interview Coach App with Microsoft Agent Framework, Foundry, MCP, and Aspire"
description: "This news post introduces a real-world application that brings together Microsoft Agent Framework, Microsoft Foundry, Model Context Protocol (MCP), and Aspire to build a cloud-native AI-powered interview coach. It explains technical patterns, such as multi-agent handoff, tool integration via MCP, and service orchestration using Aspire, providing actionable insights for .NET developers who want to deploy scalable AI agent applications both locally and on Azure."
author: "Justin Yoo"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/blog/build-a-real-world-example-with-microsoft-agent-framework-microsoft-foundry-mcp-and-aspire"
viewing_mode: "external"
feed_name: "Microsoft Blog"
feed_url: "https://devblogs.microsoft.com/feed"
date: 2026-03-09 16:00:42 +00:00
permalink: "/2026-03-09-Building-an-Interview-Coach-App-with-Microsoft-Agent-Framework-Foundry-MCP-and-Aspire.html"
categories: ["AI", "Azure", "Coding", "ML"]
tags: [".NET", "AI", "AI Agents", "Aspire", "Azure", "Azure Container Apps", "Blazor", "C#", "Cloud Native", "Coding", "DevOps Patterns", "IChatClient", "Interview Coach", "MarkItDown", "MCP", "Microsoft Agent Framework", "Microsoft Foundry", "ML", "Multi Agent Systems", "News", "OpenTelemetry", "Sample Application", "Semantic Kernel", "Service Orchestration", "Uncategorized"]
tags_normalized: ["dotnet", "ai", "ai agents", "aspire", "azure", "azure container apps", "blazor", "csharp", "cloud native", "coding", "devops patterns", "ichatclient", "interview coach", "markitdown", "mcp", "microsoft agent framework", "microsoft foundry", "ml", "multi agent systems", "news", "opentelemetry", "sample application", "semantic kernel", "service orchestration", "uncategorized"]
---

Justin Yoo details how to build a production-ready AI interview coach using Microsoft Agent Framework, Foundry, MCP, and Aspire, sharing multi-agent orchestration, cloud infrastructure, and .NET integration strategies.<!--excerpt_end-->

# Building an Interview Coach App with Microsoft Agent Framework, Foundry, MCP, and Aspire

**Author:** Justin Yoo

This guide demonstrates how developers can create a real-world, cloud-native AI agent application leveraging several Microsoft technologies: Agent Framework, Foundry, Model Context Protocol (MCP), and Aspire. The sample app, 'Interview Coach,' serves as a reference for orchestrating multiple services with persistent state, multi-agent handoff, and production-level infrastructure in .NET.

## Overview

- **Purpose:** Showcase real-world cloud deployment of AI agents using Microsoft architectures.
- **Key Technologies:** Agent Framework (multi-agent orchestration), Foundry (model backend and infra), MCP (integrates independent tool servers), Aspire (service orchestration).
- **Sample:** An interview simulation app that uses agents to perform mock interviews and provide structured performance feedback.

## Why Microsoft Agent Framework?

- **Unified abstraction:** Combines concepts from Semantic Kernel and AutoGen with production capabilities (dependency injection, type safety, OpenTelemetry, and Aspire integration).
- **Agent orchestration:** Enables multiple agents, specializing in different parts of the workflow (triage, receptionist, behavioral/technical interviews, summarizer) using graph-based orchestration.
- **Developers benefit:** Familiar .NET patterns, multi-agent workflows, and scalable enterprise-ready features.

## Microsoft Foundry as Model Backend

- **Single endpoint:** Access to OpenAI, Meta, Mistral, and more via unified API with identity, moderation, and compliance.
- **Content safety:** Built-in moderation, PII detection, and governance.
- **Enterprise integration:** Integrates with Entra ID, Microsoft Defender, and automates cost optimization, evaluation, and fine-tuning.

## MCP: Tool Server Integration

- **Loose coupling:** Tools like resume parsing (MarkItDown) and session management are run as independent MCP servers.
- **Language-agnostic:** Agents can leverage services outside .NET (e.g., MarkItDown uses Python).
- **Least privilege:** Each agent only accesses the tools it needs; improves security and flexibility.

## Aspire: Service Orchestration

- **Service topology:** Aspire configures service relationships, discovery, and health checks.
- **Unified startup:** Launches all components (web UI, agent, tool servers, backend) with a single command.
- **Telemetry:** Distributed tracing, health dashboards, and monitoring out of the box.

## Application Architecture

- **UI:** Blazor frontend for real-time interviews.
- **Backend:** Agent services orchestrated via Aspire.
- **MCP servers:** Document parser (MarkItDown), session storage (InterviewData).
- **Model provider:** Microsoft Foundry as the recommended backend.

### Multi-Agent Handoff Pattern

- The conversation is routed between specialized agents (triage, receptionist, behavioral, technical, summarizer), with clear transitions and fallback.
- Example workflow builder in C# connects agents in sequence using handoff logic.

### Tool Integration via MCP

- MCP clients discover external tool servers at startup.
- The principle of least privilege ensures only relevant tools are given to the corresponding agents.

### Aspire for Service Orchestration

- Simplifies local dev and Azure deployment.
- Includes health checks, telemetry, and service discovery.

## Getting Started

### Prerequisites

- .NET 10 SDK+
- Azure Subscription
- Microsoft Foundry project
- Docker Desktop or another container runtime

### Local Setup

```bash
git clone https://github.com/Azure-Samples/interview-coach-agent-framework.git
cd interview-coach-agent-framework

# Set credentials

dotnet user-secrets --file ./apphost.cs set MicrosoftFoundry:Project:Endpoint "<your-endpoint>"
dotnet user-secrets --file ./apphost.cs set MicrosoftFoundry:Project:ApiKey "<your-key>"

# Start all services

aspire run --file ./apphost.cs
```

Open Aspire Dashboard and start the interview via WebUI.

### Deploy to Azure

```bash
azd auth login
azd up
```

Tear down resources after testing:

```bash
azd down --force --purge
```

## What You'll Learn

- Orchestrating multi-agent systems with Agent Framework
- Using Foundry as a scalable, secure AI backend
- Integrating and managing services via Aspire
- Designing agent workflows and handoff logic
- Running tools (like resume parsing) as standalone services
- Deploying on Azure with modern DevOps tooling

## Resources & Documentation

- [Sample Source & Demo](https://aka.ms/agentframework/interviewcoach)
- [Agent Framework Docs](https://aka.ms/agent-framework)
- [Foundry Docs](https://learn.microsoft.com/azure/foundry/what-is-foundry)
- [Aspire Docs](https://aspire.dev/)
- [MCP Protocol](https://modelcontextprotocol.io/)
- [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)

For questions and contributions, check out the .NET AI Community Standup or open an issue on GitHub!

This post appeared first on "Microsoft Blog". [Read the entire article here](https://devblogs.microsoft.com/blog/build-a-real-world-example-with-microsoft-agent-framework-microsoft-foundry-mcp-and-aspire)
