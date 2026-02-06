---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-functions-ignite-2025-update/ba-p/4469815
title: 'Azure Functions Ignite 2025: Flex Consumption, MCP Servers, Agents, OpenTelemetry, and More'
author: paulyuk
feed_name: Microsoft Tech Community
date: 2025-11-18 17:01:52 +00:00
tags:
- .NET 10
- Agentic Workloads
- Aspire
- Azure Functions
- Azure Monitor
- C#
- Cloud Security
- Custom Handlers
- Durable Functions
- Elastic Premium
- Flex Consumption
- Identity Management
- Java 25
- MCP
- MCP Server
- Microsoft Agent Framework
- Node.js 24
- OpenTelemetry
- Python
- Rolling Updates
- Serverless
- TypeScript
- VNET Networking
- AI
- Azure
- DevOps
- ML
- Security
- Community
- .NET
- Machine Learning
section_names:
- ai
- azure
- dotnet
- devops
- ml
- security
primary_section: ai
---
paulyuk presents a rich technical overview of Azure Functions advancements for 2025, focusing on scalable serverless architecture, AI agent enablement, and deep integration of security, monitoring, and developer experience.<!--excerpt_end-->

# Azure Functions Ignite 2025 Update

Azure Functions continues to redefine event-driven applications and APIs for the modern cloud, accelerating innovation for developers building intelligent and resilient workloads. The 2025 update introduces significant advancements across key domains:

## 1. Flex Consumption Plan & Platform Enhancements

- **Flex Consumption GA & Improvements:** Elastic-scale, pay-for-usage, longer executions, per-instance concurrency, VNET/private networking, Always Ready instances, and new 512MB instance size to match lighter workloads.
- **Rolling Updates (Preview):** Enables zero-downtime deployments by draining instances in batches and scaling out the newest code, ensuring uninterrupted execution.
- **Migration Tools:** Step-by-step guides for moving from Azure Functions Consumption plan or AWS Lambda help modernize your serverless workloads.

## 2. AI & Agentic Scenarios with MCP Servers

- **Remote MCP Server Hosting:** Azure Functions now fully supports scalable, secure MCP (Model Context Protocol) servers. Use triggers and bindings to expose tools for AI agents, including robust authentication via Microsoft Entra ID and OAuth.
- **Self-hosted MCP:** Run official MCP SDKs (Python, TypeScript, .NET, Java) with zero code rewrite, benefit from serverless authentication, scaling, and enterprise-grade security.
- **Integration with Foundry Agents:** Easily connect MCP-powered Functions to Microsoft Foundry agents for advanced intelligent agent architectures.
- **Sample MCP Trigger Syntax:**

  ```csharp
  [Function(nameof(GetSnippet))]
  public object GetSnippet([
    McpToolTrigger(GetSnippetToolName, GetSnippetToolDescription)] ToolInvocationContext context,
    [BlobInput(BlobPath)] string snippetContent)
  {
    return snippetContent;
  }
  ```

## 3. Microsoft Agent Framework & Durable Extensions

- **Agent Framework 2.0 Refresh:** Host multi-agent orchestrations on Azure Functions using capabilities from Semantic Kernel and AutoGen. Extreme scalability, enterprise networking, built-in authentication, and VNET support.
- **Durable Task Extension:**
  - **Resilient Execution:** Survives crashes, supports distributed execution across thousands of agents.
  - **Session Management:** Persistent context, automatic recovery, deterministic multi-agent orchestration.
  - **Observability:** Durable Task Scheduler UI provides deep debugging and monitoring.
  - **Human-in-the-Loop:** Pause for user input without compute cost.
  - **Code Example:**

    ```python
    # Document Publishing Agent example
    agent = AzureOpenAIChatClient(...).create_agent(
      instructions="...",
      name="DocumentPublisher",
      tools=[AIFunctionFactory.Create(search_web), AIFunctionFactory.Create(generate_outline)]
    )
    ```

## 4. Advanced Monitoring & Observability

- **OpenTelemetry GA:** Unified logs, traces, and metrics export using open standards, broad language support (.NET, Java, JS, Python, PowerShell, TypeScript).
- **Diagnostic Settings:** Route logs/metrics to Azure Monitor or OpenTelemetry-compliant backends.

## 5. Security, Identity & Networking

- **Built-in Authentication & Authorization:** Leverage Microsoft Entra ID/OpenID for secure MCP servers.
- **Managed Identity:** Eliminate secrets, simplify developer experience, secure inbound/outbound traffic.
- **Enterprise Networking:** Full support for VNETs, private endpoints, NAT gateways, plus secure portal workflows.

## 6. Developer Productivity & Language Updates

- **.NET 10 Support:** Available for isolated worker model across plans (except Linux Consumption). Encouraged migration from legacy in-process model.
- **Aspire Integration:** Simplified distributed app orchestration—develop, debug, and deploy Azure Functions inside an Aspire solution, including direct publishing to Azure Container Apps.
- **Java 25 & Node.js 24:** New language versions supported in preview for Functions development and deployment.
- **Quickstart & Samples:** Guided resources for scaffolding, deployment, and architecture best practices.

## 7. Durable Functions & Orchestration Improvements

- **Distributed Tracing:** Deep visibility across orchestrations, activities, App Insights, and OpenTelemetry.
- **Orchestration Versioning:** Enable zero-downtime deployments, backward compatibility.
- **Extended Sessions:** Faster performance for sequential activities and fan-out/fan-in architectures.
- **Dedicated & Consumption SKUs:** Predictable pricing for steady and dynamic workloads, automatic checkpointing, advanced state protection.

---

## Get Started

- [Azure Functions Core Tools](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=macos%2Cisolated-process%2Cnode-v4%2Cpython-v2%2Chttp-trigger%2Ccontainer-apps&pivots=programming-language-csharp)
- [Flex Consumption Samples](http://aka.ms/flexconsumption/samples)
- [Functions Scenarios & Secure Samples Gallery](https://azure.github.io/awesome-azd/?tags=functions&tags=msft)
- [Foundry Functions MCP Tutorial](https://aka.ms/foundry-functions-mcp-tutorial)
- [OpenTelemetry Documentation](https://aka.ms/functions-otel)

Azure Functions brings together rapid development, robust event processing, advanced AI agent hosting, scalable serverless architecture, and security by design—giving developers the tools to build the next generation of intelligent, resilient, and secure cloud applications.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-functions-ignite-2025-update/ba-p/4469815)
