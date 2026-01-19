---
layout: post
title: Upgrading to Microsoft Agent Framework in Your .NET AI Chat App
author: Bruno Capuano
canonical_url: https://devblogs.microsoft.com/dotnet/upgrading-to-microsoft-agent-framework-in-your-dotnet-ai-chat-app/
viewing_mode: external
feed_name: Microsoft .NET Blog
feed_url: https://devblogs.microsoft.com/dotnet/feed/
date: 2025-10-23 17:00:00 +00:00
permalink: /ai/news/Upgrading-to-Microsoft-Agent-Framework-in-Your-NET-AI-Chat-App
tags:
- .NET
- .NET Aspire
- AgentFx
- AI Agents
- AI Chat Application
- Aspire
- Azure AI Foundry
- Azure OpenAI
- Blazor
- C#
- Dependency Injection
- Microsoft Agent Framework
- Microsoft.Extensions.AI
- Multi Agent Systems
- NuGet Packages
- Program.cs
- Semantic Search
- Telemetry
- Tool Invocation
section_names:
- ai
- azure
- coding
---
Bruno Capuano shows developers how to upgrade a .NET AI chat app to the Microsoft Agent Framework, achieving better architecture, agent orchestration, and deeper Azure OpenAI integration.<!--excerpt_end-->

# Upgrading to Microsoft Agent Framework in Your .NET AI Chat App

### Author: Bruno Capuano

## Overview

This guide explains how to evolve a basic AI chat application built with .NET (using modern AI templates and Blazor) into an advanced AI agent system by adopting the Microsoft Agent Framework. The post covers motivations, technical integration, architectural improvements, and real-world testing.

---

## Why Move Beyond Standard AI Chat Apps?

- AI app templates enable fast chat app bootstrapping with built-in AI, vector search, and Azure OpenAI.
- More advanced use cases require agents that can reason, invoke tools, manage context, and coordinate workflows.
- Microsoft Agent Framework brings structure and testability using familar .NET concepts like DI, middleware, and telemetry.

---

## Step 1: Set Up Your Baseline AI Chat Application

**Prerequisites:**

- .NET 9 SDK
- Visual Studio 2022 or VS Code with C# Dev Kit
- Azure account with OpenAI access (or GitHub Models)
- .NET AI App Templates installed
- Basic knowledge of .NET, Blazor, and AI

**Create Base App:**

- Install templates: `dotnet new install Microsoft.Extensions.AI.Templates`
- Scaffold project via Visual Studio or CLI, select 'AI Chat Web App' template, configure providers/storage, and choose Aspire for orchestration.
- Project structure:
  - `ChatApp20.Web`: Blazor UI
  - `ChatApp20.AppHost`: Aspire orchestration
  - `ChatApp20.ServiceDefaults`: Shared services/configs

**Sample `Program.cs` Setup:**

- Registers Azure OpenAI client & embedding model
- Adds semantic search with vector storage (SQLite)
- Seeds PDFs for sample data ingestion

---

## Step 2: Upgrade to Microsoft Agent Framework

**Install Agent Framework NuGet Packages:**

- `Microsoft.Agents.AI`, `Microsoft.Agents.AI.Abstractions`, `Microsoft.Agents.AI.Hosting`, `Microsoft.Agents.AI.Hosting.OpenAI`, `Microsoft.Agents.AI.OpenAI`

**Refactor Search as a Tool Service:**
Create a dedicated `SearchFunctions` class, wrapping semantic search for dependency injection, testability, and AI tool description metadata.

```csharp
public class SearchFunctions {
  private readonly SemanticSearch _semanticSearch;
  public SearchFunctions(SemanticSearch semanticSearch) {
    _semanticSearch = semanticSearch;
  }
  [Description("Searches for information using a phrase or keyword")]
  public async Task<IEnumerable<string>> SearchAsync(string searchPhrase, string? filenameFilter = null) {
    var results = await _semanticSearch.SearchAsync(searchPhrase, filenameFilter, maxResults: 5);
    return results.Select(r => $"<result filename=\"{r.DocumentId}\" page_number=\"{r.PageNumber}\">{r.Text}</result>");
  }
}
```

**Register the AI Agent:**

In `Program.cs`, configure the agent with dependency injection, agent instructions/description, and tool binding:

```csharp
builder.AddAIAgent("ChatAgent", (sp, key) => {
  var searchFunctions = sp.GetRequiredService<SearchFunctions>();
  var chatClient = sp.GetRequiredService<IChatClient>();
  return chatClient.CreateAIAgent(
    name: key,
    instructions: "You are a useful agent that helps users with short and funny answers.",
    description: "An AI agent that helps users with short and funny answers.",
    tools: [AIFunctionFactory.Create(searchFunctions.SearchAsync)]
  ).AsBuilder()
   .UseOpenTelemetry(configure: c => c.EnableSensitiveData = builder.Environment.IsDevelopment())
   .Build();
});
```

**Update Blazor Chat Component:**

- Inject `IServiceProvider`; resolve agent by key in `OnInitialized()`
- Replace direct `IChatClient` use with `AIAgent.RunStreamingAsync(...)`

---

## Step 3: Run and Test With .NET Aspire & Azure OpenAI

- Aspire dashboard gives unified logging, health checks, and configuration management.
- On first run, configure Azure OpenAI resource, chat & embedding models.
- Test chat interface — agent can reference ingested docs, invoke semantic search, and return structured results with citations.
- Observe agent decision-making and telemetry in Aspire dashboard.

---

## Advanced Scenarios

### Add More Tools

- Register additional services as injectable tools — e.g., `WeatherFunctions` for real-time weather, exposed via descriptions for the agent to call.

### Multi-Agent Patterns

- Register and compose multiple specialized agents for research, writing, or coordination, all orchestrated by the framework.

### Custom Middleware

- Insert logging, caching, or pre/post-processing with middleware builder pattern for agents.

---

## Best Practices

- **Craft tool descriptions carefully** to optimize invocation.
- **Unit and integration test** your services and agent behavior.
- **Monitor** via Application Insights or Aspire dashboard (token use, invoked tools, error rates, performance).

---

## Deployment to Azure

- Use .NET Aspire's Azure provisioning for rapid deployment, including Azure OpenAI, Application Insights, and secrets configuration.

---

## Summary

Upgrading to Microsoft Agent Framework enables .NET developers to build robust, testable, and observable AI agent systems while leveraging existing C# and Azure skills. The new architecture supports multi-agent orchestration, tool integration, and advanced reasoning patterns — all with best practices for monitoring and deployment.

For detailed samples, see the [Generative AI for Beginners – .NET](https://aka.ms/genainet) and official [Agent Framework documentation](https://aka.ms/agent-framework).

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/upgrading-to-microsoft-agent-framework-in-your-dotnet-ai-chat-app/)
