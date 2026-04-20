---
section_names:
- ai
- azure
- dotnet
primary_section: ai
title: Monitor AI Agents on App Service with OpenTelemetry and the New Application Insights Agents View
feed_name: Microsoft Tech Community
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/monitor-ai-agents-on-app-service-with-opentelemetry-and-the-new/ba-p/4510023
author: jordanselig
date: 2026-04-09 16:43:43 +00:00
tags:
- .NET
- AI
- Application Insights
- ASP.NET Core
- Azure
- Azure App Service
- Azure Cosmos DB
- Azure Managed Grafana
- Azure Monitor
- Azure OpenAI
- Azure Service Bus
- Bicep
- Community
- DefaultAzureCredential
- Distributed Tracing
- GenAI Semantic Conventions
- Log Analytics Workspace
- Microsoft Agent Framework
- Microsoft.Extensions.AI
- Observability
- OpenTelemetry
- Token Usage
- WebJobs
---

jordanselig shows how to instrument Microsoft Agent Framework agents with OpenTelemetry GenAI semantic conventions and send that telemetry to Azure Application Insights, enabling the Agents (Preview) view for per-agent token usage, latency, errors, and end-to-end agent runs across an ASP.NET Core API and a WebJob.<!--excerpt_end-->

## Overview

This post (Part 2 of 2) explains how to monitor a multi-agent travel planner running on **Azure App Service** by instrumenting both the LLM calls and the agent runtime with **OpenTelemetry**, then exporting that telemetry to **Azure Application Insights** so you can use the new **Agents (Preview)** view.

### Prerequisite

You need the sample from Part 1 already deployed: an App Service hosting:

- ASP.NET Core API
- WebJob for async processing
- Azure Service Bus
- Azure Cosmos DB
- Azure OpenAI

Blog 1: [Deploy Multi-Agent AI Apps on Azure App Service with MAF 1.0 GA](https://techcommunity.microsoft.com/blog/appsonazureblog/build-multi-agent-ai-apps-on-azure-app-service-with-microsoft-agent-framework-1-/4510017)

## Why agent observability is different

Beyond normal APM (HTTP latency, exceptions), AI agent workloads usually need visibility into:

- Token usage per agent
- Agent-to-agent latency differences
- Number of LLM calls per workflow
- Which agent (or tool call) failed in a workflow

Application Insights’ **Agents (Preview)** view aims to provide this “inside the agent” visibility.

## Application Insights: Agents (Preview) view

Application Insights includes an **Agents (Preview)** blade that understands agent concepts if your telemetry follows the OpenTelemetry **GenAI semantic conventions**:

- OpenTelemetry GenAI semantic conventions: https://opentelemetry.io/docs/specs/semconv/registry/attributes/gen-ai/
- Docs: [Application Insights Agents (Preview) view](https://learn.microsoft.com/en-us/azure/azure-monitor/app/agents-view)

What the view provides:

- **Agent dropdown filter** based on `gen_ai.agent.name`
- **Token usage** (input/output) by agent
- **Operational metrics** (latency, error rate, throughput) by agent
- **End-to-end trace drill-down** of a workflow, including agent steps and tool calls
- **Grafana integration** for dashboards/alerting

## GenAI semantic conventions used

The Agents view is powered by span attributes defined by the GenAI semantic conventions.

Key attributes described in the post:

- `gen_ai.agent.name`: human-readable agent name (drives the dashboard dropdown)
- `gen_ai.agent.description`: describes the agent’s role
- `gen_ai.agent.id`: stable identifier for the agent instance
- `gen_ai.operation.name`: operation type like `"chat"` or `"execute_tool"`
- `gen_ai.request.model` / `gen_ai.response.model`: request/response model identity (e.g., `"gpt-4o"`)
- `gen_ai.usage.input_tokens` / `gen_ai.usage.output_tokens`: token consumption per call
- `gen_ai.system`: AI provider (example: `"openai"`)

## Two layers of OpenTelemetry instrumentation

The sample instruments both:

1. **LLM call layer** (model-centric) via `Microsoft.Extensions.AI` and `.UseOpenTelemetry()`
2. **Agent layer** (agent identity + lifecycle) via Microsoft Agent Framework (MAF) and `.UseOpenTelemetry(sourceName: AgentName)`

### Layer 1: `IChatClient`-level instrumentation

Wrap the Azure OpenAI chat client so each LLM call emits GenAI spans:

```csharp
var client = new AzureOpenAIClient(azureOpenAIEndpoint, new DefaultAzureCredential());

// Wrap with OpenTelemetry to emit GenAI semantic convention spans
return client.GetChatClient(modelDeploymentName)
    .AsIChatClient()
    .AsBuilder()
    .UseOpenTelemetry()
    .Build();
```

Emits attributes including provider, model, token counts, and operation name.

### Layer 2: Agent-level instrumentation (MAF)

Instrument each agent instance to emit agent identity and invocation spans:

```csharp
Agent = new ChatClientAgent(
        chatClient,
        instructions: Instructions,
        name: AgentName,
        description: Description,
        tools: chatOptions.Tools?.ToList())
    .AsBuilder()
    .UseOpenTelemetry(sourceName: AgentName)
    .Build();
```

### Trade-off: span duplication

With both layers enabled, you might see some overlap because both wrappers can emit spans for the underlying LLM call. The post suggests:

- **Agent layer only**: remove `.UseOpenTelemetry()` from `IChatClient` (lose per-call token breakdowns)
- **IChatClient layer only**: remove `.UseOpenTelemetry(...)` from the agent builder (lose agent identity in Agents view)

MAF Observability docs: https://learn.microsoft.com/en-us/agent-framework/agents/observability

## Export telemetry to Application Insights

You need the OpenTelemetry spans to reach App Insights so the Agents view can render them.

The sample has two processes on the same App Service:

- ASP.NET Core API
- WebJob

### ASP.NET Core API: Azure Monitor OpenTelemetry Distro

Use the distro for a one-line setup:

```csharp
// Configure OpenTelemetry with Azure Monitor for traces, metrics, and logs.
// The APPLICATIONINSIGHTS_CONNECTION_STRING env var is auto-discovered.
builder.Services.AddOpenTelemetry().UseAzureMonitor();
```

Docs: https://learn.microsoft.com/en-us/azure/azure-monitor/app/opentelemetry-enable?tabs=aspnetcore

### WebJob: manual exporter configuration

Because the WebJob host is not ASP.NET Core, configure exporters explicitly:

```csharp
// Configure OpenTelemetry with Azure Monitor for the WebJob (non-ASP.NET Core host).
// The APPLICATIONINSIGHTS_CONNECTION_STRING env var is auto-discovered.
builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService("TravelPlanner.WebJob"))
    .WithTracing(t => t
        .AddSource("*")
        .AddAzureMonitorTraceExporter())
    .WithMetrics(m => m
        .AddMeter("*")
        .AddAzureMonitorMetricExporter());

builder.Logging.AddOpenTelemetry(o => o.AddAzureMonitorLogExporter());
```

Notes called out in the post:

- `.AddSource("*")` captures sources emitted by MAF (`sourceName: AgentName`) but may be narrowed for performance.
- `.ConfigureResource(...AddService(...))` helps distinguish API vs WebJob telemetry.
- Both setups auto-discover `APPLICATIONINSIGHTS_CONNECTION_STRING`.

## Infrastructure as Code (Bicep): provisioning monitoring

The monitoring stack is provisioned alongside the app:

### Log Analytics workspace

`infra/core/monitor/loganalytics.bicep`:

```bicep
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: name
  location: location
  tags: tags
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
  }
}
```

### Application Insights (workspace-based)

`infra/core/monitor/appinsights.bicep`:

```bicep
resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: name
  location: location
  tags: tags
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspaceId
  }
}

output connectionString string = appInsights.properties.ConnectionString
```

### Wiring the connection string to App Service

`infra/main.bicep` passes it via app settings:

```bicep
appSettings: {
  APPLICATIONINSIGHTS_CONNECTION_STRING: appInsights.outputs.connectionString
  // ... other app settings
}
```

## Using the Agents (Preview) view

In the Azure portal:

1. Open your **Application Insights** resource.
2. Navigate to **Agents (Preview)**.
3. Use the agent dropdown (`gen_ai.agent.name`) to filter metrics to one agent.
4. Review token usage across agents.
5. Use **View Traces with Agent Runs** and drill into traces for the “simple view” workflow breakdown.

## Grafana dashboards for ongoing monitoring

From the Agents view, you can jump to Azure Managed Grafana dashboards:

- Agent Framework Dashboard: https://aka.ms/amg/dash/af-agent
- Agent Framework Workflow Dashboard: https://aka.ms/amg/dash/af-workflow

## Package versions referenced

| Package | Version | Purpose |
| --- | --- | --- |
| Azure.Monitor.OpenTelemetry.AspNetCore | 1.3.0 | Azure Monitor OpenTelemetry distro for ASP.NET Core |
| Azure.Monitor.OpenTelemetry.Exporter | 1.3.0 | Azure Monitor exporters for non-ASP.NET Core hosts |
| Microsoft.Agents.AI | 1.0.0 | Microsoft Agent Framework 1.0 GA + agent-level OTEL |
| Microsoft.Extensions.AI | 10.4.1 | `IChatClient` abstraction + OTEL wrapper |
| OpenTelemetry.Extensions.Hosting | 1.11.2 | OTEL DI integration for generic hosts |
| Microsoft.Extensions.AI.OpenAI | 10.4.1 | OpenAI/Azure OpenAI adapter for `IChatClient` |

## Resources

- Sample repo: https://github.com/seligj95/app-service-multi-agent-maf-otel
- App Insights Agents view docs: https://learn.microsoft.com/en-us/azure/azure-monitor/app/agents-view
- GenAI semantic conventions: https://opentelemetry.io/docs/specs/semconv/registry/attributes/gen-ai/
- MAF observability guide: https://learn.microsoft.com/en-us/agent-framework/agents/observability
- Enable OpenTelemetry for .NET (Azure Monitor): https://learn.microsoft.com/en-us/azure/azure-monitor/app/opentelemetry-enable?tabs=aspnetcore
- Monitor Azure App Service: https://learn.microsoft.com/en-us/azure/app-service/monitor-app-service?tabs=aspnetcore
- Part 1: https://techcommunity.microsoft.com/blog/appsonazureblog/build-multi-agent-ai-apps-on-azure-app-service-with-microsoft-agent-framework-1-/4510017


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/monitor-ai-agents-on-app-service-with-opentelemetry-and-the-new/ba-p/4510023)

