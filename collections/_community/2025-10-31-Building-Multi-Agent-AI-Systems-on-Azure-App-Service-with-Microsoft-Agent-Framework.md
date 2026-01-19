---
layout: post
title: Building Multi-Agent AI Systems on Azure App Service with Microsoft Agent Framework
author: jordanselig
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/part-2-build-long-running-ai-agents-on-azure-app-service-with/ba-p/4465825
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-10-31 15:44:05 +00:00
permalink: /ai/community/Building-Multi-Agent-AI-Systems-on-Azure-App-Service-with-Microsoft-Agent-Framework
tags:
- .NET 9
- AI Agents
- Async Request Reply
- Azure AI Foundry
- Azure App Service
- Bicep
- Cloud Architecture
- Cosmos DB
- External API Integration
- Microsoft Agent Framework
- Multi Agent Systems
- Production Patterns
- Service Bus
- WebJobs
- Workflow Orchestration
section_names:
- ai
- azure
---
jordanselig demonstrates how to build sophisticated multi-agent AI solutions on Azure App Service using Microsoft Agent Framework, providing real-world workflow orchestration and deployment guidance for developers.<!--excerpt_end-->

# Building Multi-Agent AI Systems on Azure App Service with Microsoft Agent Framework

*By jordanselig*

This guide explores how to construct advanced, long-running AI agent workflows using the Microsoft Agent Framework on Azure App Service. It builds upon the single-agent async request-reply architecture, introducing patterns for orchestrating multiple specialized agents with practical code samples and real-world integration strategies.

## Introduction

After sharing a previous tutorial on single-agent workflows, this post answers a reader's question about leveraging Microsoft Agent Framework (MAF) workflow patterns and classes to connect collaborating AI agents for more robust use cases.

## Why Use Multi-Agent Systems?

Real-world AI applications often demand specialized expertise across multiple domains. Instead of overloading a single agent, multi-agent systems assign focused tasks to distinct agents, improving result quality, modularity, and maintainability.

### Example Scenario: Travel Planning Challenge

- **Currency Converter Agent**: Integrates with Frankfurter API for exchange rates
- **Weather Advisor Agent**: Pulls packing advice from National Weather Service API
- **Local Knowledge Agent**: Provides cultural and etiquette insights
- **Itinerary Planner Agent**: Constructs daily schedules
- **Budget Optimizer Agent**: Allocates trip funds efficiently
- **Coordinator Agent**: Assembles final itinerary

Each agent is specialized, testable, and can be extended or replaced independently.

## Microsoft Agent Framework Overview

Microsoft Agent Framework (MAF) goes beyond simple client-code orchestration (e.g., Semantic Kernel) by creating persistent, managed agent resources in Azure AI Foundry. Key advantages:

- Agents as Azure resources with server-side execution and persistence
- Structured primitives: agents, threads, runs
- Built-in state management and progress tracking
- Robust conversation context and multi-turn interactions
- Extensible external API/tool integration

## Multi-Agent Workflow Architecture

A typical workflow involves four execution phases:

1. **Parallel Information Gathering** (Currency, Weather, Local Knowledge agent execution)
2. **Itinerary Planning** (Synthesizes Phase 1 outputs)
3. **Budget Optimization** (Analyzes itinerary and suggests budgeting)
4. **Final Assembly** (Coordinator compiles outputs)

**Benefits:**

- Parallel execution for speed
- Specialized outputs increase result accuracy
- Debug and unit-test each agent distinctly
- Modular and easily extendable for new capabilities

## Reference Implementation

The accompanying [GitHub repository](https://github.com/Azure-Samples/app-service-maf-workflow-travel-agent-dotnet) provides complete .NET 9 source code, Bicep infrastructure-as-code templates, web UI, external API integrations, and deployment automation.

### Key Technologies Employed

- Azure App Service (P0v4 Premium)
- Azure Service Bus (async orchestration)
- Azure Cosmos DB (distributed state management)
- Azure AI Foundry and Microsoft Agent Framework
- GPT-4o model deployment
- WebJobs for background processing

### Deployment Steps

1. `git clone https://github.com/Azure-Samples/app-service-maf-workflow-travel-agent-dotnet.git`
2. `cd app-service-maf-workflow-travel-agent-dotnet`
3. `azd auth login`
4. `azd up`
5. Deploy WebJob per [README](https://github.com/Azure-Samples/app-service-maf-workflow-travel-agent-dotnet#deploy-the-webjob)

## Extending the Pattern

- Add new specialist agents (flight, hotel, activity planner, transport)
- Enable agent-to-agent communication and negotiation
- Integrate advanced ML/AI (RAG, user memory, vision)
- Enhance for production: Entra AD authentication, Application Insights tracing, VNet Integration, auto-scaling, webhooks

## Key Takeaways

- Multi-agent systems allow granular, focused automation in complex AI workflows
- Azure App Service and Microsoft Agent Framework make managed, scalable deployments approachable
- Async patterns with Service Bus and Cosmos DB boost reliability and scale
- Open-ended extensibility supports future-proof architectures for intelligent apps

## Resources

- [Microsoft Agent Framework Documentation](https://learn.microsoft.com/en-us/agent-framework/overview/agent-framework-overview)
- [Azure App Service Best Practices](https://learn.microsoft.com/azure/app-service/app-service-best-practices)
- [Async Request-Reply Pattern](https://learn.microsoft.com/azure/architecture/patterns/async-request-reply)
- [Azure App Service WebJobs](https://learn.microsoft.com/azure/app-service/overview-webjobs)

**Got multi-agent solutions to share or questions about Microsoft Agent Framework and App Service? Drop a comment in the linked post!**

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/part-2-build-long-running-ai-agents-on-azure-app-service-with/ba-p/4465825)
