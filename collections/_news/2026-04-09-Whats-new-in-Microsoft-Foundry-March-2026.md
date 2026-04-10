---
external_url: https://devblogs.microsoft.com/foundry/whats-new-in-microsoft-foundry-mar-2026/
author: Nick Brady
title: What’s new in Microsoft Foundry | March 2026
feed_name: Microsoft AI Foundry Blog
tags:
- .NET
- Agent Framework
- Agents
- AI
- AIProjectClient
- Azure
- Azure AI Foundry
- Azure AI Projects SDK
- Azure Monitor
- Azure.AI.Projects (.net SDK)
- Continuous Monitoring
- Data Leakage Prevention
- Evaluations
- Fine Tuning
- Fireworks
- Fireworks AI
- Foundry Agent Service
- Gpt 5 4
- Gpt 5 4 Mini
- GPT 5.4
- Guardrails
- Java SDK
- JavaScript/TypeScript SDK
- Managed Identity
- Microsoft Entra ID
- Microsoft Foundry
- Model Context Protocol (mcp)
- Models
- News
- NVIDIA Nemotron
- OAuth Identity Passthrough
- OpenAI Responses API
- OpenTelemetry
- Phi 4
- Phi 4 Reasoning Vision
- Priority Processing
- Private Networking
- Prompt Injection
- SDK
- Security
- Speech
- Speech To Speech
- Speech To Text
- Text To Speech
- Tracing
- Virtual Networks (vnet)
primary_section: ai
section_names:
- ai
- azure
- dotnet
- security
date: 2026-04-09 23:33:53 +00:00
---

Nick Brady’s March 2026 digest for Microsoft Foundry (Azure AI Foundry) covers major GA releases like Foundry Agent Service, GPT-5.4 family models, evaluations with continuous monitoring into Azure Monitor, private networking, and SDK 2.0 updates across Python, JS/TS, Java, and .NET—plus guardrails and third-party runtime security integrations.<!--excerpt_end-->

# What’s new in Microsoft Foundry | March 2026

## TL;DR

- **Foundry Agent Service (GA):** Production-ready agent runtime built on the OpenAI Responses API, with end-to-end private networking, expanded MCP auth (including OAuth passthrough), Voice Live preview, and hosted agents in 6 new regions.
- **GPT-5.4 + GPT-5.4 Pro (GA):** Reliability-focused reasoning models with integrated computer use and more dependable multi-step execution.
- **GPT-5.4 Mini (GA):** Cost-efficient small model for classification/extraction and lightweight tool calls.
- **Phi-4 Reasoning Vision 15B:** Multimodal reasoning (charts, diagrams, document layouts) within the Phi family.
- **Evaluations (GA) + Continuous Monitoring:** Built-in + custom evaluators, and continuous production monitoring surfaced in **Azure Monitor**.
- **`azure-ai-projects` SDK (GA):** Stable 2.0 releases for Python/JS/TS/Java targeting the GA REST v1 surface; **.NET 2.0.0** shipped April 1. `azure-ai-agents` is removed; everything is under `AIProjectClient`.
- **Fireworks AI on Foundry (Preview):** Open model inference (DeepSeek V3.2, gpt-oss-120b, Kimi K2.5, MiniMax M2.5) and bring-your-own-weights (BYOW).
- **NVIDIA Nemotron models:** Added to the Foundry catalog.
- **Priority Processing (Preview):** Reserved capacity lane for latency-sensitive inference.
- **Palo Alto Prisma AIRS + Zenity (GA):** Runtime security integrations for prompt injection, data leakage, and tool misuse.
- **Tracing (GA):** Improved end-to-end trace inspection, with new OTel semantics for AI workloads.
- **PromptFlow deprecation:** Migration to **Microsoft Framework Workflows** required by January 2027.

Quickstart: [Build Your First Agent with Foundry Agent Service](https://learn.microsoft.com/azure/foundry/quickstarts/get-started-code)

## Join the community

- Discord: https://aka.ms/foundry/discord
- GitHub Discussions: https://aka.ms/foundry/forum
- RSS: https://devblogs.microsoft.com/foundry/category/whats-new/feed/

## Models

### GPT-5.4 + GPT-5.4 Pro (GA)

**GPT-5.4** (GA March 5) is positioned as a reliability upgrade for production agents: less task drift, fewer mid-workflow failures, better tool calling, and stronger long-interaction reasoning. It also includes **integrated computer use**.

**GPT-5.4 Pro** targets deeper analytical workloads: multi-path reasoning evaluation, improved stability across long reasoning chains, and decision support for scientific research and complex trade-offs.

| Model | Context | Pricing (per M tokens) | Best for |
| --- | --- | --- | --- |
| **GPT-5.4** (≤272K input) | 272K | $2.50 input / $0.25 cached / $15 output | Production agents, coding, document workflows |
| **GPT-5.4** (>272K input) | Extended | $5.00 input / $0.50 cached / $22.50 output | Large-context reasoning |
| **GPT-5.4 Pro** | Full | $30 input / $180 output | Deep analysis, scientific reasoning |

Deployment: Standard Global and Standard Data Zone (US) at launch.

- Model catalog: https://ai.azure.com/catalog
- Announcement: https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/introducing-gpt-5-4-in-microsoft-foundry/4499785

### GPT-5.4 Mini (GA)

GA March 17. A smaller model optimized for fast, cost-efficient tasks (classification, extraction, lightweight tool calls). The post frames it as the “high-volume tier” in a routing strategy where GPT-5.4 handles harder reasoning.

Announcement: https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/introducing-open-ai%E2%80%99s-gpt%E2%80%915-4-mini-in-microsoft-foundry/4500569

### Phi-4 Reasoning Vision 15B

A 15B multimodal model in the Phi family that combines visual understanding with chain-of-thought reasoning for charts, diagrams, document layouts, and visual Q&A.

More info: https://aka.ms/Phi-4-reasoning-vision-15B

### Grok 4.2 (GA)

xAI’s Grok 4.2 went GA March 30; available via serverless or provisioned throughput.

Catalog: https://ai.azure.com/catalog

### Fireworks AI on Foundry (Public Preview)

Fireworks AI adds high-performance open model inference. Launch models listed:

| Model | Notes |
| --- | --- |
| **DeepSeek V3.2** | Sparse attention, 128K context |
| **gpt-oss-120b** | OpenAI’s open-source model |
| **Kimi K2.5** | Moonshot AI |
| **MiniMax M2.5** | Serverless support |

Key capability highlighted: **bring-your-own-weights (BYOW)** for quantized or fine-tuned weights, deployed via serverless (pay-per-token) or provisioned throughput.

Get started: https://azure.microsoft.com/en-us/blog/introducing-fireworks-ai-on-microsoft-foundry-bringing-high-performance-low-latency-open-model-inference-to-azure/

### NVIDIA Nemotron models

Nemotron models are available through the Foundry catalog, announced at NVIDIA GTC.

Catalog: https://ai.azure.com/catalog

### OSS Models in NextGen (GA)

Open-source models are fully integrated into the NextGen Foundry experience at GA, with unified deployment/management across OSS and OpenAI models.

Catalog: https://ai.azure.com/catalog

## Agents

### Foundry Agent Service (GA)

Foundry Agent Service GA is described as the major release of the month. It’s built on the **OpenAI Responses API**, is **wire-compatible with OpenAI agents**, and is open to models across providers.

Key gains called out for enterprises:

- **Private networking**
- **Entra RBAC**
- **Full tracing**
- **Evaluation tooling**

Sample (Python) creating an agent and running a Responses API call through the agent reference:

```python
import os
from azure.identity import DefaultAzureCredential
from azure.ai.projects import AIProjectClient
from azure.ai.projects.models import PromptAgentDefinition

with (
    DefaultAzureCredential() as credential,
    AIProjectClient(
        endpoint=os.environ["AZURE_AI_PROJECT_ENDPOINT"],
        credential=credential
    ) as project_client,
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

Announcement: https://devblogs.microsoft.com/foundry/foundry-agent-service-ga/

### Voice Live + Foundry Agents (Preview)

**Voice Live** is a managed, real-time speech-to-speech runtime that collapses STT → LLM → TTS into a single API. Features called out include voice activity detection, end-of-turn detection, server-side noise suppression, echo cancellation, and barge-in.

Sample (Python async) connecting an agent session:

```python
import asyncio
import os
from azure.ai.voicelive.aio import connect
from azure.identity.aio import DefaultAzureCredential

async def run():
    agent_config = {
        "agent_name": "my-enterprise-agent",
        "project_name": "my-foundry-project",
    }

    async with DefaultAzureCredential() as credential:
        async with connect(
            endpoint=os.environ["AZURE_VOICELIVE_ENDPOINT"],
            credential=credential,
            agent_config=agent_config,
        ) as connection:
            # session update and event loop (as shown in the source)
            async for event in connection:
                ...

asyncio.run(run())
```

Sample link: https://github.com/microsoft-foundry/voicelive-samples/blob/main/python/voice-live-quickstarts/AgentsNewQuickstart/voice-live-with-agent-v2.py

### MCP authentication expansion

MCP tool connections now support:

| Auth method | Use case |
| --- | --- |
| Key-based | Static API tokens via Custom Keys connection |
| Entra Agent Identity | Service-to-service with managed identity |
| Managed Identity | Azure resource access |
| OAuth Identity Passthrough | User-delegated access (OneDrive, Salesforce, SaaS APIs) |

The post emphasizes **OAuth Identity Passthrough** for “act on behalf of a specific user” scenarios.

Example MCP tool configuration:

```python
import os
from azure.ai.projects.models import MCPTool, PromptAgentDefinition

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

Docs: https://learn.microsoft.com/en-us/azure/ai-foundry/agents/how-to/tools/model-context-protocol

### Evaluations (GA) + continuous monitoring

Foundry Evaluations GA includes:

- Built-in evaluators (coherence, relevance, groundedness, retrieval quality, safety)
- Custom evaluators (your own criteria)
- Continuous evaluation: sampling live traffic, running evaluators, and surfacing results in **Azure Monitor** dashboards with alerting for drift

The post includes an example pattern using the OpenAI client’s eval APIs to create an evaluation and run it against an agent target.

Evaluation samples: https://github.com/Azure/azure-sdk-for-python/tree/main/sdk/ai/azure-ai-projects/samples/evaluations

### Prompt Optimizer in Agent Playground

Prompt Optimizer is integrated into the Agent playground, tying iterative prompt improvement to evaluation results.

Try it: https://ai.azure.com

### Hosted Agents in 6 new regions

Hosted agent deployments expanded to regions including **East US, North Central US, Sweden Central, Southeast Asia, Japan East** (and more).

Quickstart: https://learn.microsoft.com/azure/foundry/agents/quickstarts/quickstart-hosted-agent

## Safety & guardrails

### Palo Alto Prisma AIRS + Zenity (GA)

Third-party runtime security integrations GA for detecting:

- prompt injection
- toxic content
- malicious URLs
- sensitive data leakage
- tool misuse / data exfiltration

Docs: https://learn.microsoft.com/azure/foundry/guardrails/guardrails-overview

### Native and tool-call guardrails

- **Task Adherence** is now a native guardrail risk type.
- New guardrail intervention points for **tool invocations and tool responses**.
- GA support for “agent mitigations & guardrail customization.”

Docs: https://learn.microsoft.com/azure/foundry/guardrails/guardrails-overview

## Speech, audio & avatars

### Neural HD TTS updates + MAI-voice-1

Updates to Neural HD TTS including MAI-voice-1 integration.

Docs: https://learn.microsoft.com/en-us/azure/ai-services/speech-service/text-to-speech

### Fast Transcription — ~5-hour support

Fast Transcription supports up to ~5-hour audio inputs.

Docs: https://learn.microsoft.com/en-us/azure/ai-services/speech-service/batch-transcription

### Dynamic vocabulary (GA)

Dynamic vocabulary for English is GA; custom dictionary support (Tier 2) is in preview.

Docs: https://learn.microsoft.com/en-us/azure/ai-services/speech-service/speech-to-text

### Custom photo & video avatars in NextGen

Custom photo/video avatars supported in Foundry NextGen.

Docs: https://learn.microsoft.com/en-us/azure/ai-services/speech-service/text-to-speech-avatar/what-is-text-to-speech-avatar

### Playgrounds GA

- TTS Playground (GA)
- Avatar Playground (GA)
- STT & Speech Translation Playground (GA)

Try: https://ai.azure.com

## Platform

### Priority Processing (Preview)

Priority Processing provides reserved capacity for latency-sensitive inference (real-time agents, customer-facing chat, time-sensitive pipelines). It’s available for OpenAI models in Foundry with configurable priority tiers.

Announcement: https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/announcing-priority-processing-in-microsoft-foundry-for-performance-sensitive-ai/4504788

### Tracing (GA)

Tracing reached GA with better sorting/filtering, data model refinements, and new OpenTelemetry semantics for AI workloads (memory, state, planning).

Docs: https://learn.microsoft.com/en-us/azure/ai-foundry/how-to/develop/visualize-traces

### End-to-end private networking

Foundry Agent Service supports Standard Setup with private networking:

- BYO VNet
- no public egress
- container/subnet injection
- private paths extended to tool connectivity (MCP servers, Azure AI Search indexes, Fabric data agents)

Docs: https://learn.microsoft.com/en-us/azure/ai-foundry/agents/how-to/virtual-networks

### Foundry Control Plane ARM API

A consolidated ARM API for managing agents/models/tools. Public preview mentions governance support for **Azure Functions** and **App Service**.

Docs: https://learn.microsoft.com/azure/foundry/control-plane/how-to-manage-agents

### Fine-tuning CLI

A CLI for configuring/submitting/monitoring fine-tuning jobs, including cost estimation based on token projections.

Docs: https://learn.microsoft.com/en-us/azure/ai-foundry/concepts/fine-tuning-overview

### Platform updates rollup (selected)

- Eval results linked to agent traces
- Local evals aligned with Evaluators catalog
- PII NextGen playground configuration panels
- Tenant-level notification center + email notifications
- Free Trial and PAYG subscription creation improvements
- CMK for Azure AI Search (service-level configuration)

## SDK & language changelog (March 2026)

March highlights:

- Foundry REST API GA (Feb), followed by SDK GA in March.
- **Python, JS/TS, Java**: stable **2.0.0** releases targeting the v1 REST surface.
- **.NET**: 2.0.0 shipped April 1.
- `azure-ai-agents` removed across languages; agents/evals/memory/inference live under unified **`AIProjectClient`**.

### Python: `azure-ai-projects` 2.0.0 + 2.0.1

Notable changes:

- `azure-ai-projects` bundles `openai` and `azure-identity` as direct dependencies.
- `allow_preview` on `AIProjectClient` replaces per-method `foundry_features`.

Changelog: https://pypi.org/project/azure-ai-projects/2.0.1/

### .NET: `Azure.AI.Projects` 2.0.0 (GA April 1)

Restructure and renames mentioned:

- Agents administration moved to `Azure.AI.Projects.Agents`
- `Azure.AI.Projects.OpenAI` renamed to `Azure.AI.Extensions.OpenAI`
- `AIProjectClient.OpenAI` → `AIProjectClient.ProjectOpenAIClient`
- `AIProjectClient.Agents` → `AIProjectClient.AgentAdministrationClient`

Breaking changes link: https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/ai/Azure.AI.Projects/CHANGELOG.md

### JavaScript/TypeScript: `@azure/ai-projects` 2.0.0 + 2.0.1

Noted breaking changes include options bag changes and renames (for example, evaluators listing method rename).

Changelog: https://www.npmjs.com/package/@azure/ai-projects

### Java: `azure-ai-projects` to 2.0.0 GA

Noted breaking changes:

- Method renames for disambiguation (e.g., `list()` → `listDeployments()`)
- Credential method singularization
- Enum changes
- Some exception type changes

Changelog: https://github.com/Azure/azure-sdk-for-java/blob/main/sdk/ai/azure-ai-projects/CHANGELOG.md

## Deprecations

| Deprecation | Migration target | Deadline |
| --- | --- | --- |
| PromptFlow (Azure AI Foundry + Azure ML) | Microsoft Framework Workflows | January 2027 |
| Import Data / Data Connections (Azure ML) | Fabric OneLake patterns | Effective now |
| Low-priority VMs (Azure ML) | Spot VMs | Effective now |
| Default internet access for new managed VNets | Explicit outbound configuration | Effective March 31, 2026 |

Microsoft Framework Workflows: https://learn.microsoft.com/en-us/agent-framework

## Resources & community

- Forrester TEI study: https://azure.microsoft.com/en-us/blog/the-economics-of-enterprise-ai-what-the-forrester-tei-study-reveals-about-microsoft-foundry/
- Model Mondays (YouTube): https://aka.ms/model-mondays
- The Shift podcast playlist: https://www.youtube.com/playlist?list=PLLasX02E8BPBCP7KdYsjKKFFQUmNEUmE9


[Read the entire article](https://devblogs.microsoft.com/foundry/whats-new-in-microsoft-foundry-mar-2026/)

