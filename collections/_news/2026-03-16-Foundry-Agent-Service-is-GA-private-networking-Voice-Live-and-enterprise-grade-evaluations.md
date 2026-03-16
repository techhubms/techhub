---
author: Bala Venkataraman, jeffhollan, Nick Brady
section_names:
- ai
- azure
- security
primary_section: ai
external_url: https://devblogs.microsoft.com/foundry/foundry-agent-service-ga/
title: 'Foundry Agent Service is GA: private networking, Voice Live, and enterprise-grade evaluations'
feed_name: Microsoft AI Foundry Blog
date: 2026-03-16 20:30:57 +00:00
tags:
- Agent Evaluations
- Agents
- AI
- AI Agents
- Application Insights
- Azure
- Azure AI Foundry
- Azure AI Projects
- Azure AI Search
- Azure Identity
- Azure Monitor
- Bring Your Own VNet (byo VNet)
- Continuous Evaluation
- Evaluations
- Foundry Agent Service
- Hosted Agents
- Managed Identity
- MCP
- Microsoft Entra ID
- Microsoft Fabric Data Agents
- Microsoft Foundry
- Model Context Protocol (mcp)
- News
- No Public Egress
- OAuth Identity Passthrough
- Observability
- OpenAI Responses API
- Private Networking
- Python SDK
- RBAC
- Region Availability
- Security
- Speech To Speech
- Vnet
- Voice Live
---

Bala Venkataraman, jeffhollan, and Nick Brady announce the GA release of Foundry Agent Service, highlighting enterprise features like private networking, expanded MCP authentication options, Voice Live speech-to-speech integration, and built-in evaluations with continuous monitoring via Azure Monitor.<!--excerpt_end-->

## Overview

Shipping production AI agents involves much more than prototyping: network isolation, compliance constraints, voice channels for real operational use, and evaluations that continue after release.

This announcement covers the **GA release of the next-gen Foundry Agent Service** and the related platform capabilities that ship alongside it.

## What’s new

- **Foundry Agent Service (GA)**
  - Responses API-based runtime
  - Wire-compatible with OpenAI agents
  - Open model support across Meta, Mistral, DeepSeek, xAI, plus orchestration frameworks such as LangChain and LangGraph
- **End-to-end private networking**
  - BYO VNet with no public egress
  - Extended to cover tool connectivity (MCP servers, Azure AI Search, and Fabric data agents)
- **MCP authentication expansion**
  - Key-based
  - Entra Agent Identity
  - Managed Identity
  - OAuth Identity Passthrough
- **Voice Live (preview) + Foundry Agents**
  - Fully managed, real-time speech-to-speech
  - Wired to agent prompt, tools, and tracing
- **Evaluations (GA)**
  - Built-in evaluators
  - Custom evaluators
  - Continuous production monitoring piped into Azure Monitor
- **Hosted agents (preview) in additional Azure regions**
  - Includes East US, North Central US, Sweden Central, Southeast Asia, Japan East, and more

## Foundry Agent Service GA: built on the Responses API

The next-gen Foundry Agent Service is built on the **OpenAI Responses API**, intended to reduce migration work if you already build agents with that protocol. The service layers on:

- Enterprise security and **Entra RBAC**
- **Private networking**
- Full tracing
- Evaluation capabilities

Video demo:

https://devblogs.microsoft.com/foundry/wp-content/uploads/sites/89/2026/03/agents-demo.mp4

The runtime is designed to be open across model providers and orchestration frameworks. Example: one model for planning, another for generation, with orchestration via LangGraph—while keeping a consistent runtime protocol.

### Python example: creating and using an agent via `azure-ai-projects`

```python
import os
from azure.identity import DefaultAzureCredential
from azure.ai.projects import AIProjectClient
from azure.ai.projects.models import PromptAgentDefinition

with (
    DefaultAzureCredential() as credential,
    AIProjectClient(endpoint=os.environ["AZURE_AI_PROJECT_ENDPOINT"], credential=credential) as project_client,
    project_client.get_openai_client() as openai_client,
):
    agent = project_client.agents.create_version(
        agent_name="my-enterprise-agent",
        definition=PromptAgentDefinition(
            model=os.environ["AZURE_AI_MODEL_DEPLOYMENT_NAME"],
            instructions="You are a helpful assistant.",
        ),
    )

conversation = openai_client.conversations.create()

response = openai_client.responses.create(
    conversation=conversation.id,
    input="What are best practices for building AI agents?",
    extra_body={
        "agent_reference": {"name": agent.name, "type": "agent_reference"}
    },
)
print(response.output_text)
```

> **Note:** If you’re coming from the `azure-ai-agents` package, agents are now first-class operations on `AIProjectClient` in `azure-ai-projects`. Remove your standalone `azure-ai-agents` pin and use `get_openai_client()` to drive responses.

- Agents quickstart: https://learn.microsoft.com/azure/foundry/quickstarts/get-started-code?utm_source=devblog&utm_medium=blog&utm_campaign=foundry-agent-service-ga&utm_content=agents-quickstart

## End-to-end private networking

For enterprises with strict data classification policies, unmanaged network paths can be a blocker because retrieval calls, tool invocations, and model round-trips may expose data if they traverse the public internet.

**Standard Setup with private networking** supports BYO VNet:

- No public egress (agent traffic does not traverse the public internet)
- Container/subnet injection for local communication to Azure resources
- Access to private resources via the platform network (with appropriate authorization)

Private networking is also extended to **tool connectivity**, including:

- MCP servers
- Azure AI Search indexes
- Fabric data agents

Docs: https://learn.microsoft.com/en-us/azure/ai-foundry/agents/how-to/virtual-networks?utm_source=devblog&utm_medium=blog&utm_campaign=foundry-agent-service-ga&utm_content=private-networking

## MCP authentication: the full spectrum

MCP authentication is presented as a key part of making enterprise MCP deployments workable across different access patterns (shared services, user-delegated, and service-to-service).

Supported MCP server connection auth methods:

| Auth method | When to use |
| --- | --- |
| **Key-based** | Simple shared access for org-wide internal tools |
| **Entra Agent Identity** | Service-to-service; the agent authenticates as itself |
| **Entra Foundry Project Managed Identity** | Per-project permission isolation; no credential management overhead |
| **OAuth Identity Passthrough** | User-delegated access; user authenticates to the MCP server and grants the agent their credentials |

OAuth Identity Passthrough is positioned for scenarios where the agent should act on behalf of the user (e.g., access to personal OneDrive permissions, SaaS APIs that scope per-user).

### Key-based auth example (Custom Keys connection)

Create a Custom Keys connection storing an `Authorization: Bearer <token>` header and reference it using `project_connection_id`:

```python
from azure.ai.projects.models import MCPTool, PromptAgentDefinition

# project_connection_id: resource ID of a Custom Keys connection
# storing Authorization: Bearer <your-pat-token>
tool = MCPTool(
    server_label="github-api",
    server_url="https://api.githubcopilot.com/mcp",
    require_approval="always",
    project_connection_id=os.environ["MCP_PROJECT_CONNECTION_ID"],
)

agent = project_client.agents.create_version(
    agent_name="my-mcp-agent",
    definition=PromptAgentDefinition(
        model=os.environ["AZURE_AI_MODEL_DEPLOYMENT_NAME"],
        instructions="Use MCP tools as needed.",
        tools=[tool],
    ),
)
```

Docs: https://learn.microsoft.com/en-us/azure/ai-foundry/agents/how-to/tools/model-context-protocol?utm_source=devblog&utm_medium=blog&utm_campaign=foundry-agent-service-ga&utm_content=mcp-auth

## Voice Live (preview): a managed speech channel for your agents

Voice integration is described as historically requiring stitching together STT, LLM, and TTS services. **Voice Live** is presented as a managed, real-time speech-to-speech runtime that collapses those components into a single managed API.

Voice Live handles:

- Semantic voice activity detection
- Semantic end-of-turn detection
- Server-side noise suppression and echo cancellation
- Barge-in support

Voice interactions run through the same agent runtime as text, so they share evaluators, traces, and cost visibility.

### Connecting Voice Live to a Foundry agent

Uses `AgentSessionConfig` at connection time:

```python
import asyncio
from azure.ai.voicelive.aio import connect, AgentSessionConfig
from azure.identity.aio import DefaultAzureCredential

async def run():
    agent_config: AgentSessionConfig = {
        "agent_name": "my-enterprise-agent",
        "project_name": "my-foundry-project",
        # "agent_version": "v1", # optional — defaults to latest
    }

    async with DefaultAzureCredential() as credential:
        async with connect(
            endpoint=os.environ["AZURE_VOICELIVE_ENDPOINT"],
            credential=credential,
            agent_config=agent_config,
        ) as connection:
            # Update session: modalities, voice, VAD, echo cancellation
            await connection.session.update(session=session_config)
            # Process audio events
            async for event in connection:
                ...

asyncio.run(run())
```

Sample repo: https://github.com/microsoft-foundry/voicelive-samples/blob/main/python/voice-live-quickstarts/AgentsNewQuickstart/voice-live-with-agent-v2.py

## Evaluations (GA): continuous production monitoring

The post argues that pre-ship test suites are insufficient for production quality because traffic shifts, retrieval sources change, and new edge cases emerge.

Foundry Evaluations (GA) is described in three layers:

- **Out-of-the-box evaluators** for common RAG and generation scenarios (coherence, relevance, groundedness, retrieval quality, safety)
- **Custom evaluators** for business- and domain-specific criteria
- **Continuous evaluation** sampling live traffic and surfacing results in integrated dashboards

Results publish to **Azure Monitor Application Insights**, combining agent quality, infrastructure health, cost, and app telemetry.

### Python example: creating an eval and running it against an agent

```python
import os, time
from azure.identity import DefaultAzureCredential
from azure.ai.projects import AIProjectClient
from azure.ai.projects.models import PromptAgentDefinition
from openai.types.eval_create_params import DataSourceConfigCustom

with (
    DefaultAzureCredential() as credential,
    AIProjectClient(endpoint=os.environ["AZURE_AI_PROJECT_ENDPOINT"], credential=credential) as project_client,
    project_client.get_openai_client() as openai_client,
):
    agent = project_client.agents.create_version(
        agent_name=os.environ["AZURE_AI_AGENT_NAME"],
        definition=PromptAgentDefinition(
            model=os.environ["AZURE_AI_MODEL_DEPLOYMENT_NAME"],
            instructions="You are a helpful assistant.",
        ),
    )

eval_object = openai_client.evals.create(
    name="Agent Quality Evaluation",
    data_source_config=DataSourceConfigCustom(
        type="custom",
        item_schema={
            "type": "object",
            "properties": {"query": {"type": "string"}},
            "required": ["query"],
        },
        include_sample_schema=True,
    ),
    testing_criteria=[
        {
            "type": "azure_ai_evaluator",
            "name": "fluency",
            "evaluator_name": "builtin.fluency",
            "initialization_parameters": {
                "deployment_name": os.environ["AZURE_AI_MODEL_DEPLOYMENT_NAME"]
            },
            "data_mapping": {
                "query": "{{item.query}}",
                "response": "{{sample.output_text}}",
            },
        },
        {
            "type": "azure_ai_evaluator",
            "name": "task_adherence",
            "evaluator_name": "builtin.task_adherence",
            "initialization_parameters": {
                "deployment_name": os.environ["AZURE_AI_MODEL_DEPLOYMENT_NAME"]
            },
            "data_mapping": {
                "query": "{{item.query}}",
                "response": "{{sample.output_items}}",
            },
        },
    ],
)

run = openai_client.evals.runs.create(
    eval_id=eval_object.id,
    name=f"Run for {agent.name}",
    data_source={
        "type": "azure_ai_target_completions",
        "source": {
            "type": "file_content",
            "content": [
                {"item": {"query": "What is the capital of France?"}},
                {"item": {"query": "How do I reverse a string in Python?"}},
            ],
        },
        "input_messages": {
            "type": "template",
            "template": [
                {
                    "type": "message",
                    "role": "user",
                    "content": {"type": "input_text", "text": "{{item.query}}"},
                }
            ],
        },
        "target": {"type": "azure_ai_agent", "name": agent.name, "version": agent.version},
    },
)

while run.status not in ["completed", "failed"]:
    run = openai_client.evals.runs.retrieve(run_id=run.id, eval_id=eval_object.id)
    time.sleep(5)

print(f"Status: {run.status}, Results: {run.result_counts}")
```

Samples: https://github.com/Azure/azure-sdk-for-python/tree/main/sdk/ai/azure-ai-projects/samples/evaluations

## Hosted agents (preview): expanded region availability

Hosted agents (containerized agent code running as managed services on Foundry Agent Service) are available in more Azure regions.

The post calls out two scenarios:

- Data residency requirements (processing stays within a geographic boundary)
- Lower latency by running closer to data sources and users

Docs: https://learn.microsoft.com/azure/foundry/agents/quickstarts/quickstart-hosted-agent?utm_source=devblog&utm_medium=blog&utm_campaign=foundry-agent-service-ga&utm_content=hosted-agents

## Get started

Install the SDK:

```bash
pip install azure-ai-projects azure-identity
```

- Foundry portal: https://ai.azure.com?utm_source=devblog&utm_medium=blog&utm_campaign=foundry-agent-service-ga&utm_content=portal
- Agents quickstart: https://learn.microsoft.com/azure/foundry/quickstarts/get-started-code?utm_source=devblog&utm_medium=blog&utm_campaign=foundry-agent-service-ga&utm_content=quickstart-footer
- Open Foundry: https://ai.azure.com?utm_source=devblog&utm_medium=blog&utm_campaign=foundry-agent-service-ga&utm_content=open-foundry


[Read the entire article](https://devblogs.microsoft.com/foundry/foundry-agent-service-ga/)

