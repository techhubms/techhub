---
layout: post
title: 'Introducing Microsoft Agent Framework: Simplifying AI Agent Development for .NET Developers'
author: Luis Quintanilla
canonical_url: https://devblogs.microsoft.com/dotnet/introducing-microsoft-agent-framework-preview/
viewing_mode: external
feed_name: Microsoft .NET Blog
feed_url: https://devblogs.microsoft.com/dotnet/feed/
date: 2025-10-01 14:05:00 +00:00
permalink: /ai/news/Introducing-Microsoft-Agent-Framework-Simplifying-AI-Agent-Development-for-NET-Developers
tags:
- .NET
- Agent Orchestration
- Agents
- AI
- AI Agents
- Aspire
- AutoGen
- Azure Monitor
- C#
- ChatClientAgent
- Coding
- Evaluation
- GitHub Models
- Hosting
- MCP
- Microsoft Agent Framework
- Microsoft.Extensions.AI
- Microsoftagentframework
- News
- NuGet Packages
- Observability
- OpenTelemetry
- Semantic Kernel
- Testing
- Workflow Automation
section_names:
- ai
- coding
---
Luis Quintanilla introduces the Microsoft Agent Framework, guiding .NET developers in building, orchestrating, and deploying AI agents using familiar tools and robust abstractions.<!--excerpt_end-->

# Introducing Microsoft Agent Framework: Making AI Agents Simple for Every Developer

*By Luis Quintanilla*

Microsoft Agent Framework (Preview) is a comprehensive set of .NET libraries designed to make AI agent development accessible and efficient for all .NET developers. The framework simplifies the agent creation process by unifying development, orchestration, hosting, monitoring, and observability in one package.

## Agents and Workflows: The Building Blocks

**Agents** are autonomous systems designed to accomplish objectives using reasoning (LLMs or search algorithms), tool usage (like MCP, code execution, APIs), and context awareness (leveraging chat history, knowledge graphs, etc.).

**Workflows** break complex goals into manageable steps, allowing agents to execute multi-step processes, often with feedback and iterative loops.

Combining these, the Agent Framework enables multi-agent systems, where specialized agents operate within workflows to tackle complex problems collaboratively.

## Key Framework Features

- **Unified Library**: Simplifies code and orchestration for agent development in .NET.
- **Seamless Orchestration**: Build, connect, and manage single or multiple agents using structured workflows (sequential, concurrent, handoff, group chat).
- **Production-Ready Hosting**: Integrate directly with ASP.NET and .NET hosting patterns—normal deployment skills apply.
- **Observability**: Out-of-the-box OpenTelemetry support, integrating with platforms like Aspire, Azure Monitor, and Grafana.
- **Evaluation and Testing**: Supports automated agent evaluation and regression testing within your CI/CD pipeline.

## Technology Foundations

- **Semantic Kernel**: Robust orchestration engine.
- **AutoGen**: Advanced tools for multi-agent collaboration.
- **Microsoft.Extensions.AI**: Standardized AI interfaces and building blocks for .NET.

## Quick Start: Hello World Agent

1. **Prerequisites**:
   - .NET 9 SDK or later
   - GitHub Personal Access Token (PAT) with `models` scope
2. **Setup**:
   - Initialize a C# console project
   - Install packages: `Microsoft.Agents.AI`, `OpenAI`, `Microsoft.Extensions.AI.OpenAI`, `Microsoft.Extensions.AI`
3. **Sample Code** (Program.cs):

   ```csharp
   IChatClient chatClient = new ChatClient(
       "gpt-4o-mini",
       new ApiKeyCredential(Environment.GetEnvironmentVariable("GITHUB_TOKEN")!),
       new OpenAIClientOptions { Endpoint = new Uri("https://models.github.ai/inference") }
   ).AsIChatClient();

   AIAgent writer = new ChatClientAgent(
       chatClient,
       new ChatClientAgentOptions { Name = "Writer", Instructions = "Write stories that are engaging and creative." }
   );

   AgentRunResponse response = await writer.RunAsync("Write a short story about a haunted house.");
   Console.WriteLine(response.Text);
   ```

## Advanced Use: Multi-Agent Workflows

Multi-agent setups are enabled through simple building blocks. Example (adding an editor):

- Install `Microsoft.Agents.AI.Workflows` NuGet package
- Define Editor agent and sequential workflow:

   ```csharp
   AIAgent editor = new ChatClientAgent(chatClient, new ChatClientAgentOptions {
       Name = "Editor",
       Instructions = "Make the story more engaging, fix grammar, and enhance the plot."
   });

   Workflow workflow = AgentWorkflowBuilder.BuildSequential(writer, editor);
   AIAgent workflowAgent = await workflow.AsAgentAsync();
   AgentRunResponse workflowResponse = await workflowAgent.RunAsync("Write a short story about a haunted house.");
   Console.WriteLine(workflowResponse.Text);
   ```

## Tool Integration

- Easily add custom functions and tool integrations to empower agents (e.g., story formatting, database/API access via MCP).

## Hosting and Monitoring

- Integrate with .NET hosting and dependency injection
- Support for REST APIs and web application endpoints
- Enable OpenTelemetry for comprehensive monitoring and insights

## Evaluation and Testing

- Automated CI/CD integrations for testing agent quality and regression detection
- Microsoft.Extensions.AI.Evaluations library support

## Documentation and Resources

- [Agent Framework documentation](https://aka.ms/dotnet/agent-framework/docs)
- [Hello World agent sample](https://aka.ms/dotnet/agent-framework/helloworld)

## Conclusion

Microsoft Agent Framework democratizes AI agent development for .NET developers, abstracting complex orchestration and enabling production-ready solutions with minimal setup. Whether building a simple chatbot or orchestrating complex multi-agent workflows, this framework offers extensibility, observability, and integration with Microsoft's robust developer ecosystem.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/introducing-microsoft-agent-framework-preview/)
