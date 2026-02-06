---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-long-running-ai-agents-on-azure-app-service-with-microsoft/ba-p/4463159
title: Build Long-Running AI Agents on Azure App Service with Microsoft Agent Framework
author: jordanselig
feed_name: Microsoft Tech Community
date: 2025-10-21 17:04:22 +00:00
tags:
- .NET 9
- Agent Framework Samples
- AI Agents
- Async Pattern
- Azure AI Foundry
- Azure App Service
- Azure Durable Functions
- Background Workers
- Cloud Architecture
- Cosmos DB
- GPT 4o
- Long Running Workflows
- Microsoft Agent Framework
- Production AI
- Request Reply Pattern
- Service Bus
- AI
- Azure
- Community
- .NET
section_names:
- ai
- azure
- dotnet
primary_section: ai
---
jordanselig delivers a comprehensive guide on developing robust, intelligent AI agents using Microsoft Agent Framework and Azure App Service, addressing long-running workflow scenarios with modern async architectures.<!--excerpt_end-->

# Build Long-Running AI Agents on Azure App Service with Microsoft Agent Framework

The AI landscape is evolving rapidly. With the introduction of [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/overview/agent-framework-overview), developers now have a platform for building sophisticated AI agents capable of handling complex, multi-step workflows and persisting conversation context—far beyond simple LLM chat completions.

## The Challenge: Long-Running Agent Flows

Production AI applications often require workflows such as:

- Multi-turn reasoning and iterative LLM calls
- Tool/function integration and external real-time data queries
- Multi-phase business logic (e.g., itinerary building, budgeting)
- Conversation and state persistence

These often exceed the time limits of synchronous HTTP requests, leading to problems like timeouts, client disconnects, and scalability bottlenecks.

## The Solution: Async Pattern with Azure App Service

The recommended implementation leverages Azure App Service for both the API and background workers using an async request-reply approach:

1. API immediately responds (202 Accepted) with a task ID
2. Background worker processes the Agent Framework task
3. Client polls status endpoints, receiving progress updates
4. Cosmos DB stores durable state and results

Key Azure Services:

- **App Service (P0v4 Premium)** for hosting REST API & workers in one deployment
- **Service Bus** for decoupling API from long-running agent orchestration
- **Cosmos DB** for status, task results, and cleanup
- **AI Foundry** for agent execution, thread management, and LLM integration

## Sample Reference: AI Travel Planner App

The article features a practical travel planner
app where an AI agent:

- Researches attractions & activities
- Optimizes schedules
- Calculates budgets
- Delivers personalized travel tips

App Service runs both the API and background worker for simplicity. Service Bus and Cosmos DB manage reliability and persistence. Foundation models like GPT-4o provide intelligence.

### Key Advantages of Azure App Service

- Simple deployments, no containers or orchestrations required
- Combined API and worker reduce complexity and operational overhead
- Enterprise readiness: managed identity, private endpoints, built-in monitoring
- Cost-effectiveness: single instance covers both workloads
- Familiar tools: CI/CD, Application Insights, deployment slots
- Flexible scaling and rapid updates as Agent Framework evolves

### Comparison with Other Azure Architectures

Summarizes trade-offs of Durable Functions, background jobs, and Container Apps, advocating App Service for rapid, manageable, and cost-efficient deployment.

## Architecture Visibility: Inspecting Agents and Conversation Threads

Azure AI Foundry lets you inspect and debug agent runs and conversation histories directly within the Azure portal, offering transparency for advanced workflows.

## Get Started

Deploy the reference app via GitHub:
[https://github.com/Azure-Samples/app-service-agent-framework-travel-agent-dotnet](https://github.com/Azure-Samples/app-service-agent-framework-travel-agent-dotnet)

- .NET 9 source code
- Bicep IaC templates
- Web UI
- Full setup/deployment documentation

## Extend and Innovate

The guide highlights how to build upon this foundation:

- Add tool-calling for live APIs (weather, pricing, availability)
- Multi-agent systems for collaboration
- Retrieval-augmented generation (RAG)
- Real-time interactivity and multi-language expansion

## Learn More

- [Agent Framework Docs](https://learn.microsoft.com/en-us/agent-framework/overview/agent-framework-overview)
- [Azure App Service Docs](https://learn.microsoft.com/azure/app-service/)
- [Async Request-Reply Pattern](https://learn.microsoft.com/azure/architecture/patterns/async-request-reply)

**Have you built similar AI agents? Share your experience or ask questions below!**

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-long-running-ai-agents-on-azure-app-service-with-microsoft/ba-p/4463159)
