---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/bulletproof-agents-with-the-durable-task-extension-for-microsoft/ba-p/4467122
title: Building Resilient AI Agents with the Durable Task Extension for Microsoft Agent Framework
author: greenie-msft
feed_name: Microsoft Tech Community
date: 2025-11-13 20:33:42 +00:00
tags:
- AI Agents
- Azure Durable Functions
- Azure Functions
- C#
- Distributed Systems
- Durable Task Extension
- Human in The Loop
- Microsoft Agent Framework
- Multi Agent Systems
- Orchestration
- Production Grade Reliability
- Python
- Serverless Architecture
- Session Management
- State Management
section_names:
- ai
- azure
- coding
---
greenie-msft explains how the durable task extension for the Microsoft Agent Framework simplifies building highly resilient and scalable AI agents on Azure, with practical code samples and best practices for serverless durability.<!--excerpt_end-->

# Building Resilient AI Agents with the Durable Task Extension for Microsoft Agent Framework

*Author: greenie-msft*

## Overview

The durable task extension for the Microsoft Agent Framework allows developers to create production-ready AI agents that are resilient, scalable, and cost-effective. By integrating features from Azure Durable Functions, the extension brings automatic session management, distributed execution, and deterministic orchestration to AI agents deployed on Azure.

## Key Features

- **Automatic Session Management**: Agents preserve context and state across crashes, restarts, and distributed environments.
- **Deterministic Multi-Agent Orchestrations**: Predictable, code-driven workflows for coordinating specialized agents.
- **Serverless Cost Savings**: Pause agent execution for human input without consuming compute resources.
- **Built-in Observability**: Visualize agent operations and orchestrations with the Durable Task Scheduler's UI dashboard.

## Why Use the Durable Task Extension?

AI workloads increasingly require features such as:

- Long-running conversations and workflows
- Resilience to failures
- Elastic scaling across thousands of parallel agent instances
- Human-in-the-loop approval flows without unnecessary compute costs

The durable task extension enables these patterns using technology proven in Azure Durable Functions and entities, abstracted into the Microsoft Agent Framework for ease of use.

## The 4 Pillars ("4D's")

1. **Durability**: Agents automatically checkpoint state for reliable, persistent operation.
2. **Distributed**: Scale execution across many Azure instances with seamless failover.
3. **Deterministic**: Write orchestrations as ordinary code for testability and predictability.
4. **Debuggability**: Use familiar programming tools and techniques for agent development and troubleshooting.

## Example: Building an AI Agent with Durable Execution

### Python Example

```python
import os
from azure.agentframework import AzureOpenAIChatClient, AgentFunctionApp
from azure.identity import AzureCliCredential

endpoint = os.getenv("AZURE_OPENAI_ENDPOINT")
deployment_name = os.getenv("AZURE_OPENAI_DEPLOYMENT_NAME", "gpt-4o-mini")

agent = AzureOpenAIChatClient(
    endpoint=endpoint,
    deployment_name=deployment_name,
    credential=AzureCliCredential()
).create_agent(
    instructions="You are good at telling jokes.",
    name="Joker"
)

app = AgentFunctionApp(agents=[agent])
app.run()
```

### C# Example

```csharp
var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
var deploymentName = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT") ?? "gpt-4o-mini";

AIAgent agent = new AzureOpenAIClient(new Uri(endpoint), new AzureCliCredential())
    .GetChatClient(deploymentName)
    .CreateAIAgent(instructions: "You are good at telling jokes.", name: "Joker");

var app = FunctionsApplication
    .CreateBuilder(args)
    .ConfigureFunctionsWebApplication()
    .ConfigureDurableAgents(options => options.AddAIAgent(agent))
    .Build();
app.Run();
```

## Advanced Scenarios

### Human-in-the-Loop Workflow

Agents can pause indefinitely for human approval, incurring no compute costs while waiting. Once input arrives, execution resumes automatically with all relevant state.

#### Python Sample

```python
app.orchestration_trigger(context_name="context")
def content_approval_workflow(context: DurableOrchestrationContext):
    topic = context.get_input()
    content_agent = context.get_agent("ContentGenerationAgent")
    draft_content = yield content_agent.run(f"Write an article about {topic}")

    yield context.call_activity("notify_reviewer", draft_content)
    approval_event = context.wait_for_external_event("ApprovalDecision")
    timeout_task = context.create_timer(context.current_utc_datetime + timedelta(hours=24))
    winner = yield context.task_any([approval_event, timeout_task])
    if winner == approval_event:
        timeout_task.cancel()
        approved = approval_event.result
        if approved:
            result = yield context.call_activity("publish_content", draft_content)
            return result
        else:
            return "Content rejected"
    else:
        result = yield context.call_activity("escalate_for_review", draft_content)
        return result
```

## Operational Visibility

Integrate your Function App backend with the [Durable Task Scheduler](https://learn.microsoft.com/azure/container-apps/durable-task-scheduler) for built-in observability:

- **Conversation history**
- **Multi-agent visualization**
- **Performance metrics and execution logs**

## Supported Languages and Environments

- C# (.NET 8.0+) on Azure Functions
- Python (3.10+) on Azure Functions
- Additional compute options coming soon

## Get Started

- [Create and run a durable agent](https://aka.ms/create-and-run-durable-agent)
- [Durable Task Extension (Documentation)](https://aka.ms/durable-extension-for-af)
- [C# Samples](https://aka.ms/durable-extension-csharp-samples)
- [Python Samples](https://aka.ms/durable-extension-python-samples)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/bulletproof-agents-with-the-durable-task-extension-for-microsoft/ba-p/4467122)
