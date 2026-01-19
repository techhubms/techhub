---
layout: post
title: Building Reliable AI Travel Agents with the Durable Task Extension for Microsoft Agent Framework
author: greenie-msft
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-reliable-ai-travel-agents-with-the-durable-task/ba-p/4478913
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-12-18 18:35:34 +00:00
permalink: /ai/community/Building-Reliable-AI-Travel-Agents-with-the-Durable-Task-Extension-for-Microsoft-Agent-Framework
tags:
- .NET 9
- Agent Patterns
- Automated Workflows
- Azure Functions
- Azure OpenAI Service
- Azure Static Web Apps
- Blob Storage
- C#
- Durable Task Extension
- Durable Task Scheduler
- GPT 4o Mini
- Human in The Loop
- Microsoft Agent Framework
- Multi Agent Systems
- Orchestration
- Python
- Reactive Programming
- Serverless
- State Persistence
section_names:
- ai
- azure
- coding
---
In this detailed guide, greenie-msft explores building the AI Travel Planner using the durable task extension for Microsoft Agent Framework with Azure technologies, highlighting orchestration patterns, reliable multi-agent workflows, and hands-on implementation.<!--excerpt_end-->

# Building Reliable AI Travel Agents with the Durable Task Extension for Microsoft Agent Framework

This guide provides a comprehensive walkthrough of developing reliable, scalable multi-agent AI applications using the durable task extension for Microsoft Agent Framework. The AI Travel Planner sample, built in C# with a Python counterpart in progress, demonstrates practical architecture and implementation for building end-to-end AI workflows with robust state management in Azure.

## Overview

- **Application Focus:** Multi-agent orchestration using persistent, serverless AI agents for a travel planning scenario
- **Core Technologies:**
  - Durable Task Extension for Microsoft Agent Framework
  - Azure Functions (.NET 9)
  - Azure Static Web Apps (React frontend)
  - Azure OpenAI (GPT-4o-mini powering agents)
  - Azure Blob Storage
  - Durable Task Scheduler (monitoring)

## Key Challenges Solved

- **Resilience Across Failures:** Agents maintain state, so conversation history and progress are never lost during crashes or restarts.
- **Workflow Orchestration:** Deterministic control over multi-agent conversations and business workflows.
- **Human-in-the-Loop:** Workflows can pause for user input without consuming resources or incurring costs.
- **Observability:** Built-in dashboard and monitoring via Durable Task Scheduler.
- **Scalability:** Serverless hosting allows thousands of concurrent workflows with zero resource cost when paused.

## Travel Planner Architecture

The workflow coordinates three durable AI agents:

1. **Destination Recommender** – Suggests travel destinations based on user input.
2. **Itinerary Planner** – Generates day-by-day trip plans for the top recommended destination.
3. **Local Recommendations** – Enriches the plan with insider tips and local attractions.

### Workflow Steps

1. **User Request** – User submits trip preferences from a React frontend.
2. **Orchestration** – Azure Functions backend schedules an agentic orchestration using the durable task extension.
3. **Recommendations** – Destination Recommender suggests destinations.
4. **Parallel Planning** – Itinerary and Local Recommendations agents execute in parallel.
5. **Storage** – Travel plan saved to Azure Blob Storage.
6. **Human Approval** – User can review and approve plan (with workflow paused and no resource cost, supporting up to 7-day wait).
7. **Booking** – Upon approval, trip booking process completes.

## Implementation Patterns

### Durable Agents Setup

- Agents are defined and registered with the durable task extension for both C# and Python.
- Conversation sessions and orchestration are persisted automatically.

#### Example (C#)

```csharp
FunctionsApplication.CreateBuilder(args)
    .ConfigureDurableAgents(configure => {
        configure.AddAIAgentFactory("DestinationRecommenderAgent", ...);
        configure.AddAIAgentFactory("ItineraryPlannerAgent", ...);
        configure.AddAIAgentFactory("LocalRecommendationsAgent", ...);
    });
```

### Orchestration Model

- Orchestrations leverage async/await for robust, imperative logic.
- Steps include agent calls, parallel tasks, external event waits (human approval), and activity functions (e.g., storage, booking).

#### Example Orchestration (C#)

```csharp
public async Task RunTravelPlannerOrchestration(TaskOrchestrationContext context) {
    // Agent initialization
    ...
    // Get recommendations
    var destinations = await destinationAgent.RunAsync(...);
    var topDestination = destinations.Result.Recommendations.First();
    // Parallel execution
    var itineraryTask = itineraryAgent.RunAsync(...);
    var localTask = localAgent.RunAsync(...);
    await Task.WhenAll(itineraryTask, localTask);
    // Storage and human-in-the-loop
    await context.CallActivityAsync("SaveTravelPlanToBlob", travelPlan);
    var approval = await context.WaitForExternalEvent("ApprovalEvent", TimeSpan.FromDays(7));
    if (approval.Approved) await context.CallActivityAsync("BookTrip", travelPlan);
}
```

### Patterns Demonstrated

- **Agent Chaining:** Output from one agent directly informs the input to the next (e.g., Destination → Itinerary).
- **Parallel Execution:** Itinerary and Local Recommendations agents run concurrently to minimize latency.
- **Human-in-the-Loop:** Workflow can pause (“wait for approval”) with zero compute cost until user action.

### Monitoring and Observability

- The Durable Task Scheduler provides live dashboards—showing conversation threads, execution flow, timing, and resource metrics—for clear visibility and debugging.

## Getting Started

- **Sample Code Repository:** [AI Travel Planner GitHub Repository](https://github.com/Azure-Samples/durable-agents-travel-planner)
- **Quick Deployment:** Use Azure Developer CLI and Bicep scripts for rapid Azure deployment.
- **Learn More:**
  - [Durable Task Extension Overview](https://learn.microsoft.com/agent-framework/user-guide/agents/agent-types/durable-agent/create-durable-agent)
  - [Durable Task Scheduler Documentation](https://learn.microsoft.com/azure/azure-functions/durable/durable-task-scheduler/durable-task-scheduler)

## Conclusion

This solution exemplifies best practices for orchestrating robust, real-world AI systems with Azure’s latest tools. The durable task extension for Microsoft Agent Framework, paired with Azure Functions and OpenAI, makes it possible to build scalable, reliable, and cost-effective AI agent workflows for any business scenario.

---

*Author: greenie-msft | Updated December 18, 2025*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-reliable-ai-travel-agents-with-the-durable-task/ba-p/4478913)
