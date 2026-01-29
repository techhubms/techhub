---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/part-3-client-side-multi-agent-orchestration-on-azure-app/ba-p/4466728
title: Client-Side Multi-Agent Orchestration with ChatClientAgent on Azure App Service
author: jordanselig
feed_name: Microsoft Tech Community
date: 2025-11-04 15:35:47 +00:00
tags:
- .NET 9
- AIFunctionFactory
- Azure AI Foundry
- Azure App Service
- Azure OpenAI
- C#
- ChatClientAgent
- Cloud Architecture
- Cosmos DB
- Function Calling
- Microsoft Agent Framework
- Multi Agent Systems
- Service Bus
- WebJob
- Workflow Orchestration
- AI
- Azure
- Coding
- Community
section_names:
- ai
- azure
- coding
primary_section: ai
---
jordanselig explains how developers can build client-side multi-agent systems on Azure App Service using ChatClientAgent and the Microsoft Agent Framework, sharing architecture insights, code samples, and decision factors.<!--excerpt_end-->

# Client-Side Multi-Agent Orchestration with ChatClientAgent on Azure App Service

Author: **jordanselig**

In this comprehensive article, jordanselig demonstrates how to implement client-side multi-agent orchestration using the Microsoft Agent Framework's ChatClientAgent in .NET 9, deployed on Azure App Service. The article compares server-side (Azure AI Foundry) and client-side (ChatClientAgent) agent management, guides you through the architectural decisions, and provides actionable insights into workflow, code patterns, storage, and deployment.

## Introduction

The Microsoft Agent Framework supports both server-side (Foundry Agents) and client-side (ChatClientAgent) multi-agent architectures. Choosing between them depends on your priorities: control, cost, DevOps integration, or leveraging managed infrastructure. This post focuses on building client-side multi-agent systems, where developers have full orchestration and chat history control.

## Server-Side vs. Client-Side Agents: Key Differences

| Feature                      | Server-Side (Foundry)        | Client-Side (ChatClientAgent)          |
|------------------------------|------------------------------|----------------------------------------|
| Lifecycle Management         | Azure-managed                | Developer-controlled                   |
| Orchestration Patterns       | Portal-driven, built-in      | Any custom logic/code                  |
| Chat History Storage         | Foundry threads              | Cosmos DB, Redis, SQL, or custom       |
| DevOps Integration           | Portal + code                | All code, Git/versioned                |
| Provider Flexibility         | Azure AI only                | Any IChatClient (Azure, OpenAI, local) |
| Cost Structure               | API + infra/storage fees     | API usage costs only                   |

## ChatClientAgent Architecture

You orchestrate multiple specialized agents (e.g., currency conversion, weather advice, itinerary planning) using .NET and client-managed chat history. ChatClientAgent runs as a C# object within your application, giving full control over agent execution, orchestration patterns, and conversation storage.

**Key architectural components:**

- **TravelPlanningWorkflow**: Central orchestration logic for running agents in parallel/sequential workflows
- **ChatClientAgent**: Encapsulates instructions, tools, and AI completion logic
- **IChatClient**: Abstraction for AI provider endpoints (supports Azure OpenAI)
- **Custom Storage**: Use Cosmos DB, Redis, or SQL for chat history

## Example: Implementing a Client-Side BaseAgent

```csharp
public abstract class BaseAgent : IAgent {
    protected readonly ChatClientAgent Agent;
    protected abstract string AgentName { get; }
    protected abstract string Instructions { get; }

    // Constructor without tools
    protected BaseAgent(ILogger logger, IOptions<AgentOptions> options, IChatClient chatClient) {
        Agent = new ChatClientAgent(chatClient, new ChatClientAgentOptions {
            Name = AgentName,
            Instructions = Instructions
        });
    }
    // Constructor with tools (e.g., weather, currency)
    protected BaseAgent(ILogger logger, IOptions<AgentOptions> options, IChatClient chatClient, ChatOptions chatOptions) {
        Agent = new ChatClientAgent(chatClient, new ChatClientAgentOptions {
            Name = AgentName,
            Instructions = Instructions,
            ChatOptions = chatOptions // Tools via AIFunctionFactory
        });
    }
    public async Task<ChatMessage> InvokeAsync(IList<ChatMessage> chatHistory, CancellationToken cancellationToken = default) {
        var response = await Agent.RunAsync(chatHistory, thread: null, options: null, cancellationToken);
        return response.Messages.LastOrDefault() ?? new ChatMessage(ChatRole.Assistant, "No response generated.");
    }
}
```

## Managing Chat History

With ChatClientAgent, you store and manage chat histories for each agent:

```csharp
public class WorkflowState {
    public Dictionary<string, List<ChatMessage>> AgentChatHistories { get; set; } = new();
    public List<ChatMessage> GetChatHistory(string agentType) {
        if (!AgentChatHistories.ContainsKey(agentType)) {
            AgentChatHistories[agentType] = new List<ChatMessage>();
        }
        return AgentChatHistories[agentType];
    }
}
```

You can persist these conversations in Cosmos DB for analytics, compliance, or custom retention.

## Orchestration Patterns and Tooling

- **Parallel agent execution:** Run multiple agents simultaneously for efficiency.
- **Sequential dependencies:** Later phases depend on earlier agent outputs.
- **Tool integration:** Register C# APIs (e.g., weather lookup) as callable tools via `AIFunctionFactory` and `ChatOptions`.

## Decision Guide: When to Use Each Approach

**Prefer ChatClientAgent when:**

- You require complex, custom orchestration
- Cost optimization (no extra infra fees) is required
- DevOps/code-first workflows are a priority
- You need total control over chat and data storage
- The solution may target non-Azure AI services in the future

**Prefer Foundry Agents when:**

- You need managed infrastructure with minimal code
- Built-in capabilities (e.g., RAG, code interpreter) are essential
- Portal UI, low-code config, or quick prototyping are most important

## Azure App Service: The Hosting Platform

Both approaches run reliably on Azure App Service, leveraging:

- Async request-reply patterns
- Service Bus for messaging
- Cosmos DB for state management
- Always-On premium hosting
- Managed Identities for authentication
- WebJobs for background processing

## End-to-End Deployment

The provided GitHub repo enables you to:

- Deploy the full architecture (App Service, Service Bus, Cosmos DB, Azure AI Services)
- Use Infrastructure as Code (Bicep) for reproducible environments
- Run, compare, and customize both client-side and server-side agent implementations

[GitHub: app-service-maf-openai-travel-agent-dotnet](https://github.com/Azure-Samples/app-service-maf-openai-travel-agent-dotnet)

## Conclusion

Microsoft Agent Framework empowers developers with both managed and code-first agent orchestration models. Client-side ChatClientAgent is ideal for scenarios requiring maximum flexibility, DevOps best practices, or specific data compliance. Azure App Service cleanly supports either approach, enabling robust, scalable AI workloads in the Microsoft cloud.

Try out both patterns, compare results, and choose what best supports your team's project and operational goals.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/part-3-client-side-multi-agent-orchestration-on-azure-app/ba-p/4466728)
