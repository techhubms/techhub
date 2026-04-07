The hardest part of shipping production AI agents isn’t the prototype — it’s everything after. Network isolation requirements. Compliance audits. Voice channels your operations team actually wants to use. Evaluations that aren’t just a pre-ship checkbox.

Today’s GA release of the next-gen Foundry Agent Service addresses all of these directly. Here’s what shipped and what it means for your builds.

## What’s new

- **Foundry Agent Service (GA):** Responses API-based runtime, wire-compatible with OpenAI agents, open model support across DeepSeek, xAI, Meta, LangChain, LangGraph, and more

- **End-to-end private networking:** BYO VNet with no public egress, extended to cover tool connectivity — MCP servers, Azure AI Search, and Fabric data agents

- **MCP authentication expansion:** Key-based, Entra Agent Identity, Managed Identity, and OAuth Identity Passthrough in a single service

- **Voice Live (preview) + Foundry Agents:** Real-time speech-to-speech, fully managed, wired natively to your agent’s prompt, tools, and tracing

- **Evaluations (GA):** Out-of-the-box evaluators, custom evaluators, and continuous production monitoring piped into Azure Monitor

- **Hosted agents (preview) in six new Azure regions:** East US, North Central US, Sweden Central, Southeast Asia, Japan East, and more

## Foundry Agent Service GA: built on the Responses API

The next-gen Foundry Agent Service is built on the OpenAI Responses API — the same agentic wire protocol developers are already building on. If you’re building with the Responses API today, migrating to Foundry is minimal code changes. What you gain immediately: Foundry’s enterprise security layer, private networking, Entra RBAC, full tracing, and evaluation — on top of your existing agent logic.

[https://devblogs.microsoft.com/foundry/wp-content/uploads/sites/89/2026/03/agents-demo.mp4](https://devblogs.microsoft.com/foundry/wp-content/uploads/sites/89/2026/03/agents-demo.mp4)

The architecture is intentionally open. You’re not locked to a single model provider or orchestration framework. Use a DeepSeek model for planning, an OpenAI model for generation, LangGraph for orchestration — the runtime handles the consistency layer. Agents, tools, and the surrounding infrastructure all speak the same protocol.

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
 extra_body={"agent_reference": {"name": agent.name, "type": "agent_reference"}},
 )
 print(response.output_text)
```

> 

**Note:** If you’re coming from the `azure-ai-agents` package, agents are now first-class operations on `AIProjectClient` in `azure-ai-projects`. Remove your standalone `azure-ai-agents` pin and use `get_openai_client()` to drive responses.

## End-to-end private networking

Unmanaged network paths are a showstopper for enterprises operating under data classification policies that prohibit external routing of query content or retrieved documents. Every retrieval call, every tool invocation, every model round-trip is a potential exposure vector if it crosses the public internet.

Foundry Agent Service now supports **Standard Setup with private networking**, where you bring your own virtual network (BYO VNet):

- No public egress — agent traffic never traverses the public internet

- Container/subnet injection into your network for local communication to Azure resources

- Access to private resources via the platform network with appropriate authorization

More importantly, private networking is extended to **tool connectivity**. MCP servers, Azure AI Search indexes, and Fabric data agents can all operate over private network paths — so retrieval and action surfaces sit inside your network boundary, not just inference calls.

## MCP authentication: the full spectrum

MCP as a connection primitive is only as secure as its auth model. Enterprise MCP deployments span org-wide shared services, user-delegated access, and service-to-service connections — and they need different auth patterns for each.

Foundry now supports the full spectrum for MCP server connections:

Auth method

When to use

**Key-based**

Simple shared access for org-wide internal tools

**Entra Agent Identity**

Service-to-service; the agent authenticates as itself

**Entra Foundry Project Managed Identity**

Per-project permission isolation; no credential management overhead

**OAuth Identity Passthrough**

User-delegated access; user authenticates to the MCP server and grants the agent their credentials

OAuth Identity Passthrough is the one worth calling out. When users need to grant an agent access to their personal data or permissions — their OneDrive, their Salesforce org, a SaaS API that scopes by user — the agent should act on their behalf, not as a shared system identity. Passthrough enables exactly that with standard OAuth flows.

For key-based auth, add a Custom Keys connection in your Foundry project with an `Authorization: Bearer ` header, then reference it via `project_connection_id`:

```python
from azure.ai.projects.models import MCPTool, PromptAgentDefinition

# project_connection_id: resource ID of a Custom Keys connection
# storing Authorization: Bearer 
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

## Voice Live (preview): a managed speech channel for your agents

Adding voice to an agent used to mean stitching together three separate services (STT, LLM, TTS) — three latency hops, three billing surfaces, three failure modes, all synchronized by hand. **Voice Live** is a fully managed, real-time speech-to-speech runtime that collapses that into a single managed API.

What Voice Live handles:

- **Semantic voice activity detection** — knows when you’ve stopped speaking based on meaning, not just silence or audio level

- **Semantic end-of-turn detection** — understands conversational context to determine when the agent should respond

- **Server-side noise suppression and echo cancellation** — no post-processing pipeline required

- **Barge-in support** — users can interrupt mid-response

With this integration, you connect Voice Live directly to an existing Foundry agent. The agent’s prompt, tool definitions, and configuration are managed in Foundry; Voice Live handles the audio pipeline. Voice interactions go through the same agent runtime as text — which means the same evaluators, the same traces, the same cost visibility. Voice doesn’t get a second-class observability story.

For customer support, field service, accessibility, and any hands-free workflow where spoken dialogue is the primary interface, this replaces what previously required a custom audio pipeline.

Connecting Voice Live to a Foundry agent uses `AgentSessionConfig` at connection time — point it at an agent name and project, and the session is immediately voice-enabled:

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

The agent’s prompt, tool definitions, and safety configuration stay in Foundry. Voice Live owns the audio I/O. The full working sample — including audio capture/playback via PyAudio and interrupt handling — is in the SDK repo.

## Evaluations: GA with continuous production monitoring

Running a test suite before shipping is not a production quality strategy — it’s a snapshot. Quality degrades in production as traffic patterns shift, retrieved documents go stale, and new edge cases emerge that never appeared in your eval dataset.

Foundry Evaluations are now generally available with three layers that together enable a proper quality lifecycle:

**Out-of-the-box evaluators** cover the standard RAG and generation scenarios: coherence, relevance, groundedness, retrieval quality, and safety. No custom configuration required — connect them to a dataset or live traffic and get quantitative scores back.

**Custom evaluators** let you encode your own criteria: business logic, internal tone standards, domain-specific compliance rules, or any quality signal that doesn’t map cleanly to a general evaluator.

**Continuous evaluation** closes the production loop. Foundry samples live traffic automatically, runs your evaluator suite against it, and surfaces results through integrated dashboards. Configure Azure Monitor alerts to fire when groundedness drops, safety thresholds breach, or performance degrades — before users notice.

All evaluation results, traces, and red-teaming runs publish to Azure Monitor Application Insights. You get full-stack observability that spans agent quality, infrastructure health, cost, and traditional app telemetry in one place.

Evaluations in `azure-ai-projects` run through the OpenAI-compatible `evals` API on `AIProjectClient`. The pattern: define the schema and evaluators in `openai_client.evals.create()`, then run against an agent target with `openai_client.evals.runs.create()`.

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
 item_schema={"type": "object", "properties": {"query": {"type": "string"}}, "required": ["query"]},
 include_sample_schema=True,
 ),
 testing_criteria=[
 {
 "type": "azure_ai_evaluator",
 "name": "fluency",
 "evaluator_name": "builtin.fluency",
 "initialization_parameters": {"deployment_name": os.environ["AZURE_AI_MODEL_DEPLOYMENT_NAME"]},
 "data_mapping": {"query": "{{item.query}}", "response": "{{sample.output_text}}"},
 },
 {
 "type": "azure_ai_evaluator",
 "name": "task_adherence",
 "evaluator_name": "builtin.task_adherence",
 "initialization_parameters": {"deployment_name": os.environ["AZURE_AI_MODEL_DEPLOYMENT_NAME"]},
 "data_mapping": {"query": "{{item.query}}", "response": "{{sample.output_items}}"},
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
 "content": [{"item": {"query": "What is the capital of France?"}},
 {"item": {"query": "How do I reverse a string in Python?"}}],
 },
 "input_messages": {
 "type": "template",
 "template": [{"type": "message", "role": "user",
 "content": {"type": "input_text", "text": "{{item.query}}"}}],
 },
 "target": {"type": "azure_ai_agent", "name": agent.name, "version": agent.version},
 },
 )

 while run.status not in ["completed", "failed"]:
 run = openai_client.evals.runs.retrieve(run_id=run.id, eval_id=eval_object.id)
 time.sleep(5)

 print(f"Status: {run.status}, Results: {run.result_counts}")
```

## Hosted agents (preview) in six new regions

Hosted agents — containerized agent code running as managed services on Foundry Agent Service — are now available in six additional Azure regions: **East US**, **North Central US**, **Sweden Central**, **Southeast Asia**, **Japan East**, and more.

This is relevant for two concrete scenarios: data residency requirements that mandate processing stays within a geographic boundary, and latency that compresses when your agent runs close to its data sources and users. Foundry handles container orchestration, scaling, networking, and endpoint management — you own the agent behavior and business logic.

## Learn more

For a hands-on walkthrough of the Foundry Agent Service capabilities, watch the session below — covering building a basic conversational agent, adding custom skills, grounding with documents, code execution, real-time internet access, connecting to external servers via MCP, and combining multiple tools:

## Get started

The next-gen Foundry Agent Service is available now. Install the SDK, open the portal, and go:

```bash
pip install azure-ai-projects azure-identity
```

The [Foundry portal](https://ai.azure.com?utm_source=devblog&utm_medium=blog&utm_campaign=foundry-agent-service-ga&utm_content=portal) has an updated agents experience with visual workflow building, a unified Tools tab for MCP, A2A, and Azure AI Search connections, and the separated v1/v2 resource view. If you’re coming from Foundry Classic, the new experience is the default.

For a hands-on introduction, the [agents quickstart](https://learn.microsoft.com/azure/foundry/quickstarts/get-started-code?utm_source=devblog&utm_medium=blog&utm_campaign=foundry-agent-service-ga&utm_content=quickstart-footer) takes you from zero to a running, tool-using agent in a few minutes.

 
 

Category

Topics

## Author

![Bala Venkataraman](https://devblogs.microsoft.com/foundry/wp-content/uploads/sites/89/2025/07/Bala-Venkataraman-96x96.jpg)

Principal Program Manager

![jeffhollan](https://devblogs.microsoft.com/foundry/wp-content/uploads/sites/89/2026/03/Jeff_Hollan-21-small-96x96.webp)

Partner Director of Product

Jeff Hollan is the Partner Director of Product for Microsoft Foundry agent service. His team is responsible for the platform to simplify how enterprises can go from experimentation to production for agents in Foundry.

![Nick Brady](https://devblogs.microsoft.com/foundry/wp-content/uploads/sites/89/2025/03/nick-brady-96x96.png)

Senior Program Manager, Developer Experience

Since 2018, Nick has been the unofficial spokesperson for Microsoft's AI Platform having delivered hundreds of engagements to customers and partners in his time at Microsoft. Nick leads developer experience for Microsoft Foundry, focusing on content, code, and community to empower every developer to build, customize, and scale AI applications.