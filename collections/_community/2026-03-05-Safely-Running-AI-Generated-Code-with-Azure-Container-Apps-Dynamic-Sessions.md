---
layout: "post"
title: "Safely Running AI-Generated Code with Azure Container Apps Dynamic Sessions"
description: "This in-depth community post by Jan-Kalis explains how Azure Container Apps Dynamic Sessions enable secure execution of AI-generated and agent-provided code. Covering both architecture patterns and real-world implementation, it showcases how Dynamic Sessions integrate with Model Context Protocol (MCP), the Microsoft Agent Framework, and Azure OpenAI. Key topics include setting up isolated code interpreter and custom container sessions, managing authentication, observability with Application Insights, and end-to-end infrastructure as code using Bicep and AZD."
author: "Jan-Kalis"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/even-simpler-to-safely-execute-ai-generated-code-with-azure/ba-p/4499795"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-03-05 21:04:59 +00:00
permalink: "/2026-03-05-Safely-Running-AI-Generated-Code-with-Azure-Container-Apps-Dynamic-Sessions.html"
categories: ["AI", "Azure", "Coding", "DevOps"]
tags: ["AI", "AI Agents", "Application Insights", "Azure", "Azure Container Apps", "Azure OpenAI", "Bicep", "Code Interpreter", "Coding", "Community", "Custom Containers", "DevOps", "DevOps Automation", "Dynamic Sessions", "IaC", "MCP", "Microsoft Agent Framework", "Node.js", "Observability", "Python", "Shell", "Telemetry"]
tags_normalized: ["ai", "ai agents", "application insights", "azure", "azure container apps", "azure openai", "bicep", "code interpreter", "coding", "community", "custom containers", "devops", "devops automation", "dynamic sessions", "iac", "mcp", "microsoft agent framework", "nodedotjs", "observability", "python", "shell", "telemetry"]
---

Jan-Kalis details how Azure Container Apps Dynamic Sessions can securely execute AI-generated and agent-run code using isolated sandboxes, with MCP integration, code interpreters, and custom containers. This practical guide illustrates setup, security, and deployment best practices.<!--excerpt_end-->

# Safely Running AI-Generated Code with Azure Container Apps Dynamic Sessions

Jan-Kalis

AI can now generate and run code on your behalf—but **where** should that code safely execute? This post explains how Azure Container Apps Dynamic Sessions provide on-demand, sandboxed environments to securely run untrusted or LLM-generated code, integrated with Microsoft agent tools and cloud infrastructure.

## Overview: Why Use Dynamic Sessions?

- **Isolation**: Each session runs in a Hyper-V isolated sandbox, so bad code cannot affect your application or data.
- **Performance**: Pre-warmed session pools support near-instant startup (millisecond ready) and scale to thousands of concurrent runs.
- **Simplicity**: REST APIs, automatic lifecycle, and session cleanup—no infrastructure to manage.
- **Security**: Prevent dangerous operations (e.g., `os.remove('/')`) by avoiding direct execution in the app context.

## Dynamic Sessions for Agents and AI

Dynamic Sessions are now even easier to drive from AI tools using the Model Context Protocol (MCP). This enables:

- Quick spin-up of interpreter sessions for AI agents
- Out-of-process execution under tight security controls
- Starter samples using Microsoft Agent Framework integrations (code interpreter and custom containers)

## Session Types

### 1. Code Interpreter Sessions

- **Inline Code** run in a Hyper-V sandbox via REST API
- **Runtimes:**
  - **Python** (NumPy, pandas, matplotlib, etc.) – ideal for AI-generated analysis and charts
  - **Node.js** – for JavaScript scripting and transformation
  - **Shell** – full Linux command environment for DevOps, CI/CD, and CLI tools
- **Usage Example:**
  - Provide code input, receive real-time output, manage files/persistence (within session lifespan)

### 2. Custom Container Sessions

- Bring your own container images for more advanced scenarios
- Pooling, scaling, and cleanup handled automatically
- Useful for custom code interpreters or specialized runtimes

## MCP Support

Enabling MCP turns dynamic sessions into remote tool servers for AI agents:

- Expose file-system, tool execution, and shell commands in a secure ephemeral container
- Set `isMCPServerEnabled: true` in the session pool config
- Plug endpoint and API key directly into Azure Foundry or other agents
- Example walkthrough: [How to add an MCP tool to your Azure Foundry agent](https://techcommunity.microsoft.com/blog/appsonazureblog/how-to-add-an-mcp-tool-to-your-azure-foundry-agent-using-dynamic-sessions-on-azu/4468844)

## End-to-End Example: AI Travel Agent with Sandboxed Compute

### Architecture

- **Microsoft Agent Framework** – agent runtime and middleware
- **Azure OpenAI (GPT-4o)** – code and chat generation
- **ACA Session Pools** – secure Python/Node/Shell code interpreters
- **Azure Container Apps** – host for agent container
- **Application Insights** – full-stack observability

### Scenario A: Static Code in a Sandbox (Weather Research)

- Agent sends pre-written Python to a sandbox to fetch live weather data (Open-Meteo API).
- Uses Azure identity for secure authentication.
- Ensures code runs only inside the isolated session.

```python
import requests
from azure.identity import DefaultAzureCredential
credential = DefaultAzureCredential()
token = credential.get_token("https://dynamicsessions.io/.default")
response = requests.post(
    f"{pool_endpoint}/code/execute?api-version=2024-02-02-preview&identifier=weather-session-1",
    headers={"Authorization": f"Bearer {token.token}"},
    json={"properties": {
      "codeInputType": "inline",
      "executionType": "synchronous",
      "code": weather_code  # Python fetching weather
    }},
)
result = response.json()["properties"]["stdout"]
```

### Scenario B: LLM-Generated Code in a Sandbox (Dynamic Charting)

- User asks for weather chart (Miami vs Tokyo)
- Steps:
  1. Fetch weather data
  2. Azure OpenAI (GPT-4o) generates matplotlib code via system-prompt
  3. Safety-checks for forbidden imports
  4. Code is sent to sandbox
  5. Agent downloads PNG chart from output directory

```python
from openai import AzureOpenAI
client = AzureOpenAI(azure_endpoint=endpoint, api_key=key, api_version="2024-12-01-preview")
generated_code = client.chat.completions.create(
  model="gpt-4o",
  messages=[{"role": "system", "content": CODE_GEN_PROMPT}, {"role": "user", "content": f"Weather data: {weather_json}"}],
  temperature=0.2
).choices[0].message.content

# Execute in sandbox

requests.post(
    f"{pool_endpoint}/code/execute?api-version=2024-02-02-preview&identifier=chart-session-1",
    headers={"Authorization": f"Bearer {token.token}"},
    json={"properties": {
      "codeInputType": "inline",
      "executionType": "synchronous",
      "code": f"import json, matplotlib\nmatplotlib.use('Agg')\nimport matplotlib.pyplot as plt\nweather_data = json.loads('{weather_json}')\n{generated_code}"
    }},
)

# Download chart

img = requests.get(
    f"{pool_endpoint}/files/content/chart.png?api-version=2024-02-02-preview&identifier=chart-session-1",
    headers={"Authorization": f"Bearer {token.token}"}
).content
```

### Authentication

- Use `DefaultAzureCredential` (local)
- Use `ManagedIdentityCredential` (in Container Apps)
- Tokens auto-cached/refreshed

### Observability (Application Insights)

- Full tracing for agent invocation, tool/LLM calls, sandbox timing
- OpenTelemetry spans for easy tracking
- Session pools emit utilization metrics and logs
- Full pipeline visibility from user prompt → agent → LLM → sandbox → response

```python
from azure.monitor.opentelemetry import configure_azure_monitor
from agent_framework.observability import create_resource, enable_instrumentation
configure_azure_monitor(
  connection_string="InstrumentationKey=...",
  resource=create_resource(),
  enable_live_metrics=True,
)
enable_instrumentation(enable_sensitive_data=False)
```

### Deployment

- Full infra-as-code via Bicep
- One-command provisioning using `azd`

```bash
azd auth login
azd up
```

## Resources & Further Reading

- [Dynamic Sessions documentation – Microsoft Learn](https://learn.microsoft.com/en-us/azure/container-apps/sessions)
- [How to add an MCP tool to your Foundry agent](https://techcommunity.microsoft.com/blog/appsonazureblog/how-to-add-an-mcp-tool-to-your-azure-foundry-agent-using-dynamic-sessions-on-azu/4468844)
- [Custom container sessions sample](https://github.com/Azure-Samples/dynamic-sessions-custom-container)
- [AI Agent + Dynamic Sessions code sample](https://github.com/jkalis-MS/AIAgent-ACA-DynamicSession)

---
*Author: Jan-Kalis (Microsoft, Apps on Azure Blog)*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/even-simpler-to-safely-execute-ai-generated-code-with-azure/ba-p/4499795)
