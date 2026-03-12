---
layout: "post"
title: "The “Golden Triangle” of Agentic Development with Microsoft Agent Framework: AG-UI, DevUI & OpenTelemetry Deep Dive"
description: "This article provides a detailed exploration of end-to-end development with the Microsoft Agent Framework, showcasing a modern workflow that leverages AG-UI, DevUI, and OpenTelemetry. Through the GHModel.AI case, it demonstrates how GitHub Models are used for rapid prototyping, DevUI for debugging, AG-UI for presentation, and OpenTelemetry for observability, offering practical code samples in Python and .NET."
author: "Kinfey Lo"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/agent-framework/the-golden-triangle-of-agentic-development-with-microsoft-agent-framework-ag-ui-devui-opentelemetry-deep-dive/"
viewing_mode: "external"
feed_name: "Microsoft Semantic Kernel Blog"
feed_url: "https://devblogs.microsoft.com/semantic-kernel/feed/"
date: 2025-12-01 17:08:48 +00:00
permalink: "/2025-12-01-The-Golden-Triangle-of-Agentic-Development-with-Microsoft-Agent-Framework-AG-UI-DevUI-and-OpenTelemetry-Deep-Dive.html"
categories: ["AI", "Coding"]
tags: [".NET", "AG UI", "AG UI Protocol", "Agent Development", "Agent Framework", "Agentic AI", "Agents", "AI", "Coding", "CopilotKit", "Debugging", "DevUI", "Distributed Tracing", "GitHub Models", "Microsoft Agent Framework", "News", "Observability", "OpenTelemetry", "Python", "Telemetry"]
tags_normalized: ["dotnet", "ag ui", "ag ui protocol", "agent development", "agent framework", "agentic ai", "agents", "ai", "coding", "copilotkit", "debugging", "devui", "distributed tracing", "github models", "microsoft agent framework", "news", "observability", "opentelemetry", "python", "telemetry"]
---

Kinfey Lo walks through modern agentic development practices with the Microsoft Agent Framework, using AG-UI, DevUI, and OpenTelemetry to accelerate and improve agent development workflows.<!--excerpt_end-->

# The “Golden Triangle” of Agentic Development with Microsoft Agent Framework: AG-UI, DevUI & OpenTelemetry Deep Dive

Author: Kinfey Lo

Building agentic AI solutions can be a challenge, especially when facing issues like insight into agent reasoning, quick stakeholder demoing, and tracking performance. This guide uses the [GHModel.AI sample](https://github.com/microsoft/Agent-Framework-Samples/tree/main/09.Cases/GHModel.AI) to show a practical, developer-focused way forward by combining three key tools: DevUI, AG-UI, and OpenTelemetry.

## Agentic Development Challenges

- **Opaque Execution**: Difficulty debugging agent logic (what is my agent thinking?).
- **Demo Barriers**: Building a UI for demonstrations can be time-consuming.
- **Performance/Cost Blind Spots**: Tracking latency and token usage is historically cumbersome.

## Solution: The Golden Triangle Stack

### **1. Model Layer — GitHub Models**

- Rapid access to models like GPT-4o and Llama 3 with a GitHub account.
- Agent Framework’s SDK allows back-end switching with minimal code change.
- Clearly declared agent definitions for both Python and .NET, emphasizing maintainability.

#### **Sample Python Agent Declaration**

```python
from agent_framework.openai import OpenAIChatClient
chat_client = OpenAIChatClient(base_url=os.environ.get("GITHUB_ENDPOINT"),
                               api_key=os.environ.get("GITHUB_TOKEN"),
                               model_id=os.environ.get("GITHUB_MODEL_ID"))

# Create agents and workflow as in the article
```

#### **Sample .NET Agent Declaration**

```csharp
var openAIOptions = new OpenAIClientOptions() { Endpoint = new Uri(github_endpoint) };
var openAIClient = new OpenAIClient(new ApiKeyCredential(github_token), openAIOptions);
var chatClient = openAIClient.GetChatClient(github_model_id).AsIChatClient();
// Create agents and workflow as in the article
```

### **2. DevUI — Debug Layer**

- Visualizes agent reasoning, chain of thought, and state in real time.
- Fast feedback loop for logic iteration and troubleshooting (see screenshot in article).
- Code samples provided for easy integration in both Python and .NET projects.

### **3. AG-UI — Presentation Layer**

- Provides out-of-the-box Agent-User interface using a standardized protocol.
- Supports direct connection with tools like CopilotKit for rich frontend capabilities.
- Enables streaming responses and dynamic UI component rendering from the backend.

#### **Sample Integration (Python & .NET)**

```python
from agent_framework_ag_ui import add_agent_framework_fastapi_endpoint

# register workflow and endpoint as in the article
```

```csharp
builder.Services.AddAGUI();
// Map AGUI endpoint in ASP.NET Core as in the article
```

### **4. OpenTelemetry — Observability Layer**

- Baked-in distributed tracing to analyze full agentic execution paths.
- Monitors token consumption, latency, and execution costs.
- Easily integrated in code and supports dashboards via Application Insights and Grafana.

#### **Sample Setup**

```python
from agent_framework.observability import setup_observability
setup_observability()
```

```csharp
Sdk.CreateTracerProviderBuilder()
   .AddSource("*Microsoft.Agents.AI")
   .AddOtlpExporter(options => options.Endpoint = new Uri("http://localhost:4317"))
   .Build();
```

## Conclusion and Best Practices

- **Model Layer**: Validate ideas with public/free models via GitHub.
- **DevUI**: Gain real-time perspective for fast debugging.
- **AG-UI**: Standardize and accelerate user interface generation.
- **OpenTelemetry**: Get data-driven insight into performance and cost.

See the [Microsoft Agent Framework GitHub Repo](https://github.com/microsoft/agent-framework) for source code, samples, and more resources to start your agentic development journey.

## Resources

1. [Microsoft Agent Framework GitHub Repo](https://github.com/microsoft/agent-framework)
2. [Microsoft Agent Framework Samples](https://github.com/microsoft/Agent-Framework-Samples)
3. [DevUI Getting Started](https://github.com/microsoft/agent-framework/tree/main/python/samples/getting_started/devui)
4. [Observability Samples](https://github.com/microsoft/agent-framework/tree/main/python/samples/getting_started/observability)

This post appeared first on "Microsoft Semantic Kernel Blog". [Read the entire article here](https://devblogs.microsoft.com/agent-framework/the-golden-triangle-of-agentic-development-with-microsoft-agent-framework-ag-ui-devui-opentelemetry-deep-dive/)
