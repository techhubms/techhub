---
feed_name: Microsoft All Things Azure Blog
external_url: https://devblogs.microsoft.com/all-things-azure/hostedagent/
primary_section: ai
date: 2026-04-15 05:00:56 +00:00
title: Choosing the Right Azure Hosting Option for Your AI Agents
section_names:
- ai
- azure
- devops
author: Reenu Saluja
tags:
- Agentic
- Agentic Workloads
- Agents
- Agents API
- AI
- AI Agent
- AI Agents
- AI Apps
- AI Infrastructure
- AI Platform
- AKS
- All Things Azure
- App Development
- Application Insights
- Autoscaling
- Azd
- Azure
- Azure AI Foundry
- Azure App Service
- Azure CLI
- Azure Container Apps
- Azure Container Registry
- Azure Developer CLI
- Azure DevOps
- Azure Functions
- Azure Kubernetes Service (aks)
- Containers
- Conversation State
- Dapr
- DevOps
- GitHub Actions
- Hosted Agents
- KEDA
- LangGraph
- Managed Identity
- Microsoft Agent Framework
- Microsoft Foundry Agent Service
- News
- OpenTelemetry
- Private Endpoints
- RBAC
- Responses API
- Scale To Zero
- Thought Leadership
- VNet Integration
---

Reenu Saluja breaks down the main Azure hosting options for production AI agents and explains when to use each, with a deeper walkthrough of Microsoft Foundry Hosted Agents (deployment, lifecycle management, observability, scaling, and invocation patterns).<!--excerpt_end-->

# Choosing the Right Azure Hosting Option for Your AI Agents

*A comprehensive guide to hosting AI agents on Azure, with a deep dive into Microsoft Foundry Hosted Agents*

As AI agents become a core building block for intelligent applications, a practical question comes up quickly: where should you host them, and what tradeoffs are you taking on around control, complexity, and managed capabilities?

This guide covers the main Azure hosting options for agentic workloads and then goes deeper on **Hosted Agents in Microsoft Foundry** (a managed platform designed specifically for running AI agents).

## The Azure agent hosting landscape

Common Azure options for deploying AI agents to production include:

- Azure Container Apps
- Azure Kubernetes Service (AKS)
- Azure Functions
- Azure App Service
- Microsoft Foundry Agents
- Microsoft Foundry Hosted Agents

## Option 1: Azure Container Apps

**When to choose:** You want full control over your container runtime while avoiding Kubernetes complexity.

Azure Container Apps (ACA) is a serverless container platform with built-in autoscaling and ingress management. It’s suited to running LangGraph, Semantic Kernel, or custom agent frameworks in containers you control.

```json
# Example: Deploying a LangGraph agent to Container Apps
apiVersion: apps.containerapps.io/v1
kind: ContainerApp
metadata:
  name: langgraph-agent
spec:
  template:
    containers:
      - image: myacr.azurecr.io/langgraph-agent:latest
        resources:
          cpu: 1.0
          memory: 2Gi
```

**Pros**

- Full container control
- KEDA-based autoscaling
- VNet integration and private endpoints
- Dapr integration for microservices

**Cons**

- Manual observability setup
- No built-in agent lifecycle management
- You manage conversation state yourself

## Option 2: Azure Kubernetes Service (AKS)

**When to choose:** Enterprise scenarios requiring strict compliance, multi-cluster deployment, or complex networking.

AKS provides maximum control for organizations running large-scale agent deployments across multiple regions with custom networking, security policies, and compliance requirements.

**Pros**

- Maximum flexibility and control
- Multi-cluster federation
- Advanced networking (CNI, service mesh)
- GitOps and policy-as-code

**Cons**

- Significant operational overhead
- Requires Kubernetes expertise
- Higher cost for small workloads

## Option 3: Azure Functions

**When to choose:** Event-driven agents triggered by queues, HTTP requests, or timers.

For stateless or short-running agent tasks (for example, processing a document or responding to a webhook), Azure Functions provides a serverless model with consumption-based pricing.

**Pros**

- True serverless, pay-per-execution
- Built-in triggers (HTTP, Queue, Timer, Event Grid)
- Easy integration with Azure services

**Cons**

- Execution time limits (up to 10 minutes on Consumption)
- Cold starts can impact latency
- Not ideal for long-running agent workflows

## Option 4: Azure App Service

**When to choose:** You have a simple HTTP-facing agent with predictable traffic and want a familiar PaaS deployment model.

**Pros**

- Fully managed PaaS (no OS patching or runtime management)
- Built-in CI/CD (GitHub Actions, Azure DevOps) and deployment slots
- Easy Auth with Entra ID out of the box

**Cons**

- No agent-native abstractions; conversation state is your responsibility
- Instance-based scaling is less granular than KEDA-driven autoscaling
- Not cost-efficient for spiky or highly variable traffic

## Option 5: Microsoft Foundry Agents

**When to choose:** You want a managed, code-optional agent configured through the portal or SDK—no containers and no infrastructure.

Foundry Agents are defined with a system prompt and built-in tools (file search, code interpreter, Bing grounding, Azure AI Search). You invoke them via the Agents API or the Foundry playground.

**Pros**

- Zero infrastructure (no containers, Docker builds, or clusters)
- Built-in tools: file search, code interpreter, Bing, AI Search, OpenAPI actions
- Create and iterate in the Foundry portal or via the `azure-ai-projects` SDK
- Platform-managed thread and message history

**Cons**

- No custom framework support (LangGraph, Semantic Kernel)—use Hosted Agents for that
- Agent logic is limited to model instructions and built-in tools
- Complex multi-step state machines are better served by Hosted Agents

## Deep dive: Microsoft Foundry Hosted Agents

If you want managed infrastructure *and* custom agent code, **Hosted Agents** in Microsoft Foundry are positioned as the middle ground.

### What are Hosted Agents?

Hosted Agents are containerized agentic AI applications that run on **Foundry Agent Service**. Compared to “plain” container hosting, they add agent-specific platform capabilities:

- **Agent-native abstractions** (conversations, responses, tool calls)
- **Managed lifecycle** (create/start/update/stop/delete via API)
- **Built-in observability** (OpenTelemetry traces, metrics, and logs)
- **Framework support** (LangGraph, Microsoft Agent Framework, or custom code)

![Hosted Agent architecture diagram](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2026/05/Architecture-1.webp)

### The Hosting Adapter (bridging frameworks to Foundry)

Hosted Agents use a **Hosting Adapter** to expose your agent as an HTTP service with Foundry integration.

**LangGraph adapter example**

```py
from azure.ai.agentserver.langgraph import from_langgraph

# Your LangGraph agent
# graph = StateGraph(MessagesState)
# graph.add_node("agent", call_model)
# graph.add_node("tools", tool_node)
# ... build your graph

# app = graph.compile()

# Wrap with the hosting adapter
if __name__ == "__main__":
    from_langgraph(app).run()
```

**Microsoft Agent Framework adapter example**

```py
from azure.ai.agentserver.agentframework import from_agent_framework

agent = ChatAgent(
    chat_client=AzureAIAgentClient(...),
    instructions="You are a helpful assistant.",
    tools=[get_local_time],
)

if __name__ == "__main__":
    from_agent_framework(agent).run()
```

The adapter handles:

| Capability | What it handles |
| --- | --- |
| Protocol translation | Converts Foundry Responses API ↔ your framework’s format |
| Conversation management | Message serialization, history management |
| Streaming | Server-sent events for real-time responses |
| Observability | TracerProvider, MeterProvider, LoggerProvider via OpenTelemetry |
| Local testing | Runs on `localhost:8088` |

## Building and deploying a Hosted Agent (example)

This walkthrough deploys a LangGraph calculator agent to Microsoft Foundry.

### Step 1: Define your agent code (`main.py`)

```py
from langchain_core.tools import tool
from langgraph.graph import MessagesState, StateGraph, START, END
from azure.ai.agentserver.langgraph import from_langgraph

@tool
def multiply(a: int, b: int) -> int:
    """Multiply two numbers."""
    return a * b

@tool
def add(a: int, b: int) -> int:
    """Add two numbers."""
    return a + b

# Build the graph
tools = [multiply, add]
# ... graph construction ...

app = graph.compile()

if __name__ == "__main__":
    from_langgraph(app).run()
```

### Step 2: Create the agent manifest (`agent.yaml`)

```yaml
name: CalculatorAgent
description: A LangGraph agent that performs arithmetic calculations.
metadata:
  tags:
    - calculator
    - math

template:
  name: CalculatorAgentLG
  kind: hosted # This makes it a Hosted Agent

protocols:
  - protocol: responses

version: v1

environment_variables:
  - name: AZURE_OPENAI_ENDPOINT
    value: ${AZURE_OPENAI_ENDPOINT}
  - name: AZURE_AI_MODEL_DEPLOYMENT_NAME
    value: "{{chat}}" # Resolved at runtime

resources:
  - kind: model
    id: gpt-4o
    name: chat
```

### Step 3: Containerize with Docker

```dockerfile
FROM python:3.11-slim

WORKDIR /app

COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY . .

# The hosting adapter listens on port 8088
EXPOSE 8088

CMD ["python", "main.py"]
```

### Step 4: Deploy with Azure Developer CLI (`azd`)

The `azd` AI agent extension bundles build + push + provision + deploy.

```text
# Install the extension
azd ext install azure.ai.agents

# Initialize your project
azd ai agent init

# Build, push, and deploy in one command
azd up
```

The post describes `azd up` as doing the following:

1. Builds your container image
2. Pushes to Azure Container Registry
3. Creates the Foundry project (if needed)
4. Deploys model endpoints
5. Creates and starts your Hosted Agent

## Managing Hosted Agent lifecycle

You can manage the agent with Azure CLI commands.

```text
# Start an agent (with scale-to-zero support)
az cognitiveservices agent start \
  --account-name myFoundry \
  --project-name myProject \
  --name CalculatorAgent \
  --agent-version 1 \
  --min-replicas 0 \
  --max-replicas 3

# Update replicas without creating a new version
az cognitiveservices agent update \
  --min-replicas 1 \
  --max-replicas 5

# Stop the agent
az cognitiveservices agent stop \
  --account-name myFoundry \
  --project-name myProject \
  --name CalculatorAgent \
  --agent-version 1
```

## Invoking a Hosted Agent (Azure AI Projects SDK)

```py
from azure.ai.projects import AIProjectClient
from azure.identity import DefaultAzureCredential

client = AIProjectClient(
    endpoint="https://your-project.services.ai.azure.com/api/projects/your-project",
    credential=DefaultAzureCredential(),
    allow_preview=True,
)

# Get the OpenAI-compatible client
openai_client = client.get_openai_client()

# Invoke the agent
response = openai_client.responses.create(
    input=[{"role": "user", "content": "What is 25 * 17 + 42?"}],
    extra_body={
        "agent_reference": {
            "name": "CalculatorAgent",
            "type": "agent_reference"
        }
    }
)

print(response.output_text)
# Output: 25 * 17 = 425, then 425 + 42 = 467
```

## Built-in observability

Hosted Agents automatically export telemetry to Application Insights (or another OpenTelemetry collector).

```text
# Traces are exported automatically - no code changes needed!
# View in Azure Portal → Application Insights → Transaction Search
```

You can also stream container logs:

```bash
curl -N "https://{endpoint}/api/projects/{project}/agents/{agent}/versions/1/containers/default:logstream?kind=console&tail=100" \
  -H "Authorization: Bearer $(az account get-access-token --resource https://ai.azure.com --query accessToken -o tsv)"
```

## Conversation management

Hosted Agents integrate with Foundry’s conversation system so you don’t have to manage conversation state like you would in “raw” containers.

```py
# Create a persistent conversation
conversation = openai_client.conversations.create()

# First turn
response1 = openai_client.responses.create(
    conversation=conversation.id,
    extra_body={"agent_reference": {"name": "CalculatorAgent", "type": "agent_reference"}},
    input="Remember: my favorite number is 42.",
)

# Later turn - agent has context
response2 = openai_client.responses.create(
    conversation=conversation.id,
    extra_body={"agent_reference": {"name": "CalculatorAgent", "type": "agent_reference"}},
    input="Multiply my favorite number by 10.",
)
```

## Resource scaling options

Supported CPU/memory combinations:

| CPU | Memory |
| --- | --- |
| 0.25 | 0.5 Gi |
| 0.5 | 1.0 Gi |
| 1.0 | 2.0 Gi |
| 2.0 | 4.0 Gi |
| 4.0 | 8.0 Gi |

Horizontal scaling settings:

| Setting | Description |
| --- | --- |
| `min-replicas: 0` | Scale to zero when idle (cost savings, cold start on first request) |
| `min-replicas: 1` | Always warm (no cold starts, steady cost) |
| `max-replicas: 5` | Maximum horizontal scale (preview limit) |

## Publishing to channels

The post lists multiple ways to publish a production-ready agent:

- Web Application Preview (shareable demo interface)
- Microsoft 365 Copilot & Teams (appear in the agent store)
- Stable API Endpoint (consistent REST API for custom apps)

It also notes a permissions gotcha:

- Publishing creates a **dedicated agent identity** separate from the project managed identity.
- After publishing, you must reconfigure RBAC permissions because project managed identity permissions don’t automatically transfer.

## Decision framework: choosing your hosting option

A simplified rule of thumb from the post:

- Use **Azure Functions** for event-driven, short-running tasks.
- Use **Container Apps** if you want container control without Kubernetes.
- Use **AKS** for strict enterprise requirements and maximum control.
- Use **App Service** for straightforward HTTP agents with predictable traffic.
- Use **Foundry Agents** for code-optional, fully managed agents.
- Use **Foundry Hosted Agents** for custom code + managed agent lifecycle/observability.

![Decision tree diagram for hosting options](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2026/04/DecisionTree.webp)

## Getting started

1. Install prerequisites:

```text
pip install azure-ai-projects azure-identity
az extension add –name cognitiveservices
azd ext install azure.ai.agents
```

2. Start from a sample:

```text
azd ai agent init
# Select “Start new from a template”
# Choose LangGraph or Agent Framework
```

3. Deploy:

```text
azd up
```

4. Test in the playground:

- Open the Foundry portal and use the agent playground UI.

## What’s next

Expected improvements as the platform matures past preview:

- Private networking support for network-isolated workloads
- Expanded regional availability (noted as 25+ regions currently)
- Deeper tool integration with MCP servers and Foundry Tools

## References

- Azure Container Apps Overview: https://learn.microsoft.com/azure/container-apps/overview
- What is Azure Kubernetes Service (AKS)?: https://learn.microsoft.com/azure/aks/what-is-aks
- Azure App Service Overview: https://learn.microsoft.com/azure/app-service/overview
- What is Azure Functions?: https://learn.microsoft.com/azure/azure-functions/functions-overview
- What is Microsoft Foundry Agent Service?: https://learn.microsoft.com/azure/foundry/agents/overview
- Hosted Agents Documentation: https://learn.microsoft.com/azure/ai-foundry/agents/concepts/hosted-agents
- Agent Runtime Components: https://learn.microsoft.com/azure/ai-foundry/agents/concepts/runtime-components
- Agent Development Lifecycle: https://learn.microsoft.com/azure/ai-foundry/agents/concepts/development-lifecycle
- Azure Developer CLI – AI Agent Extension: https://learn.microsoft.com/azure/developer/azure-developer-cli/extensions/azure-ai-foundry-extension
- Foundry Samples repository: https://github.com/microsoft-foundry/foundry-samples/tree/main/samples/python/hosted-agents


[Read the entire article](https://devblogs.microsoft.com/all-things-azure/hostedagent/)

