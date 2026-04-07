---
primary_section: ai
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/why-data-platforms-must-become-intelligence-platforms-for-ai/ba-p/4505653
section_names:
- ai
- azure
- ml
date: 2026-04-07 07:00:00 +00:00
author: AnjaliSadhukhan
tags:
- Agentic AI
- AI
- Azure
- Azure AI Foundry
- Azure Data Lake Storage Gen2
- Community
- Data Agents
- Data Governance
- Data Integration
- Data Lineage
- Data Platform Architecture
- DAX
- Enterprise Data
- Grounding
- Managed Identity
- Microsoft Entra ID
- Microsoft Fabric
- Microsoft Graph
- ML
- OneLake
- Row Level Security
- Semantic Link
- Semantic Models
- Tool Calling
feed_name: Microsoft Tech Community
title: Why data platforms must become intelligence platforms for AI agents (with Microsoft Fabric + Azure AI Foundry)
---

AnjaliSadhukhan argues that AI agents fail on enterprise questions mainly due to fragmented data and missing semantics, and outlines how Microsoft Fabric (OneLake, semantic models, Data Agents) and Azure AI Foundry can work together to provide governed, agent-ready access to business data.<!--excerpt_end-->

## The promise and the gap

You invest in an AI agent and ask:

> “Prepare a summary of Q3 revenue by region, including year-over-year trends and top product lines.”

The agent has to pull from multiple systems—each with different schemas and definitions:

- Revenue in a SQL warehouse
- Product metadata in Dataverse
- Regional mappings in SharePoint
- Historical data in Azure Blob Storage
- Organizational context in Microsoft Graph

With five sources and no shared definitions, the agent may hallucinate, return incomplete data, or ask many clarifying questions.

The post’s core claim: **the bottleneck isn’t model capability; it’s that enterprise data isn’t structured for reasoning.** Traditional platforms were built for humans to query; “intelligence platforms” must be built for agents to reason over.

## What you’ll understand

- Why fragmented enterprise data blocks effective AI agents
- What distinguishes a storage platform from an intelligence platform
- How **Microsoft Fabric** and **Azure AI Foundry** can combine to enable agent-ready, trustworthy access to data

## The enterprise pain: fragmented data breaks AI agents

Enterprise data is spread across databases, data lakes, business apps, collaboration platforms, third-party APIs, and Microsoft Graph—each with its own schema and security model.

Humans compensate with institutional knowledge (e.g., “revenue” in the warehouse is net after returns, but in CRM it’s gross bookings). An agent doesn’t have that implicit meaning.

A common consequence is repeated, bespoke prep work for each agent deployment:

- Custom integrations
- Transformation pipelines
- “Make it usable” steps that don’t scale

## Why agents struggle without a semantic layer

To be trustworthy, an AI agent needs:

1. **Data access** — reach the relevant sources
2. **Semantic context** — definitions, relationships, hierarchies (what the data *means*)
3. **Trust signals** — lineage, permissions, freshness

Traditional platforms often provide (1), but not (2) or (3), so agents infer meaning from names/structures—fragile and error-prone.

## From storage to intelligence: what must change

The post frames this as a shift in platform expectations:

- **Storage platform:** “Where is the data, and how do I access it?”
- **Intelligence platform:** “What does the data mean, who can use it, and how can an agent reason over it?”

It proposes four pillars.

## Pillar 1: Unified data access (OneLake)

**OneLake** (in Microsoft Fabric) provides a single logical namespace across the organization.

- Works across lakehouses, warehouses, and external storage
- Uses **shortcuts** and **mirroring** to reduce fragmentation without forcing migration

More: https://learn.microsoft.com/fabric/onelake/onelake-overview

## Pillar 2: Shared semantic layer (Fabric semantic models)

**Semantic models** in Fabric encode:

- Business measures
- Table relationships
- Human-readable field descriptions
- Row-level security

Key idea: an agent querying a semantic model gets **business-level answers** (e.g., `[Total Revenue]`) instead of raw tables.

### Before vs after (agent perspective)

**Without semantic layer:**

- Queries raw tables
- Infers business meaning
- Risks incorrect aggregation

**With semantic layer:**

- Queries `[Total Revenue]`
- Uses business-defined logic
- Gets consistent, governed results

### Example mapping

| Raw table column | Semantic model measure |
| --- | --- |
| `fact_sales.amount` | `[Total Revenue]` — Sum of net sales after returns |
| `fact_sales.amount / dim_product.cost` | `[Gross Margin %]` — Revenue minus COGS as a percentage |
| `fact_sales.qty` YoY comparison | `[YoY Growth %]` — Year-over-year quantity growth |

### Code Snippet 1 — Querying a Fabric semantic model with Semantic Link (Python)

```python
import sempy.fabric as fabric

# Query business-defined measures — no need to know underlying table schemas
dax_query = """
EVALUATE
SUMMARIZECOLUMNS(
  'Geography'[Region],
  'Calendar'[FiscalQuarter],
  "Total Revenue", [Total Revenue],
  "YoY Growth %", [YoY Growth %]
)
"""

result_df = fabric.evaluate_dax(
  dataset="Contoso Sales Analytics",
  workspace="Contoso Analytics Workspace",
  dax_string=dax_query
)

print(result_df.head())

# NOTE: Output shown is illustrative and based on the semantic model definition
```

Key takeaway from the post: the agent doesn’t need to know where revenue lives physically or how fiscal/calendar quarters align; the semantic model encodes it.

### Code Snippet 2 — Discovering available models and measures (Python)

```python
import sempy.fabric as fabric

# Discover available semantic models in the workspace
datasets = fabric.list_datasets(workspace="Contoso Analytics Workspace")
print(datasets[["Dataset Name", "Description"]])

# Inspect available measures — business-defined metrics an agent can query
measures = fabric.list_measures(
  dataset="Contoso Sales Analytics",
  workspace="Contoso Analytics Workspace"
)
print(measures[["Table Name", "Measure Name", "Description"]])

# NOTE: Output shown is illustrative and based on the semantic model definition
```

Semantic Link docs (as referenced): https://learn.microsoft.com/fabric/data-science/semantic-%20link-overview

## Pillar 3: Context enrichment (Microsoft Graph)

**Microsoft Graph** adds organizational signals:

- Who is asking (role-appropriate detail)
- What’s relevant (e.g., trending datasets)
- Who should review (data stewards)

The post positions this as a way for agents to be not only correct, but appropriately scoped.

## Pillar 4: Agent-ready APIs (Fabric Data Agents, preview)

**Data Agents** in Microsoft Fabric (preview) provide a natural-language interface to semantic models and lakehouses.

- Instead of forcing the agent to generate SQL/DAX, the platform provides structured context and business logic.

Preview terms: https://learn.microsoft.com/legal/microsoft-fabric-preview

Data Agents overview: https://learn.microsoft.com/fabric/data-science/concept-data-agent

## Microsoft Fabric as the intelligence layer

The post reframes Fabric’s role: not just unified analytics, but an **intelligence layer** that makes enterprise data understandable to autonomous systems.

### OneLake: one namespace, many sources

OneLake is described as being backed by **Azure Data Lake Storage Gen2** and supporting:

- Shortcuts (reference external data without copying)
- Mirroring (replicate from Azure SQL, Cosmos DB, or Snowflake)
- A unified security model

## Tying it together: Azure AI Foundry + Microsoft Fabric

The integration pattern described:

1. **User prompt** — asked via an AI Foundry-powered app
2. **Tool call** — agent selects a Fabric Data Agent tool and sends a natural-language query
3. **Semantic resolution** — Data Agent translates to DAX and executes against the semantic model via OneLake
4. **Structured response** — results return with added context (definitions, permission checks, lineage)
5. **User response** — AI Foundry agent returns a grounded, sourced answer

Why it matters (as listed):

- No custom ETL for agents
- No prompt-stuffing (semantic model provides business context at query time)
- No trust gap (governed semantic models enforce RLS and lineage)
- No one-off integrations (multiple agents can reuse Data Agents)

### Code Snippet 3 — Azure AI Foundry agent using a Fabric Data Agent tool (Python)

```python
from azure.ai.projects import AIProjectClient
from azure.ai.projects.models import FabricTool
from azure.identity import DefaultAzureCredential

# Connect to Azure AI Foundry project
project_client = AIProjectClient.from_connection_string(
  credential=DefaultAzureCredential(),
  conn_str="<your-ai-foundry-connection-string>"
)

# Register a Fabric Data Agent as a grounding tool
fabric_tool = FabricTool(connection_id="<fabric-connection-id>")

# Create an agent that uses the Fabric Data Agent for data queries
agent = project_client.agents.create_agent(
  model="gpt-4o",
  name="Contoso Revenue Analyst",
  instructions=(
    "You are a business analytics assistant for Contoso. "
    "Use the Fabric Data Agent tool to answer questions about revenue, margins, and growth. "
    "Always cite the source semantic model."
  ),
  tools=fabric_tool.definitions
)

# Start a conversation
thread = project_client.agents.create_thread()
project_client.agents.create_message(
  thread_id=thread.id,
  role="user",
  content="What was Q3 revenue by region, and which region grew fastest?"
)

# The agent automatically calls the Fabric Data Agent tool
run = project_client.agents.create_and_process_run(
  thread_id=thread.id,
  agent_id=agent.id
)

# Retrieve the agent's response
messages = project_client.agents.list_messages(thread_id=thread.id)
print(messages.data[0].content[0].text.value)

# NOTE: Output shown is illustrative and based on the semantic model definition
```

## Security/auth notes called out

For production scenarios, the post recommends:

- Use **managed identities** or **Microsoft Entra ID** authentication
- Follow **least privilege**

Least privilege guidance (as linked):

- https://learn.microsoft.com/entra/identity-platform/secure-least-privileged-access

## Getting started: practical next steps

1. **Consolidate access through OneLake**
   - Create shortcuts to critical sources
   - No migration required
   - https://learn.microsoft.com/fabric/onelake/create-onelake-shortcut

2. **Build semantic models with business definitions**
   - Measures, relationships, descriptions, row-level security
   - https://learn.microsoft.com/fabric/data-warehouse/semantic-models

3. **Enable Data Agents (preview)**
   - Expose semantic models as natural-language endpoints
   - Review preview terms; plan for API changes

4. **Connect Azure AI Foundry agents**
   - Register Data Agents as tools
   - Azure AI Foundry docs: https://learn.microsoft.com/azure/ai-studio/

## Conclusion

The post concludes that effective enterprise agents depend more on platform readiness than model capability. To support trustworthy reasoning, a platform needs unified access, semantics, org context, and agent-ready APIs—positioning **Microsoft Fabric** plus **Azure AI Foundry** as an implementation pattern.

> Disclaimer (from the post): Fabric Data Agents and other features described may be in preview and can change before GA; availability, functionality, and pricing may differ. See https://learn.microsoft.com/legal/microsoft-fabric-preview.

[Read the entire article](https://techcommunity.microsoft.com/t5/microsoft-developer-community/why-data-platforms-must-become-intelligence-platforms-for-ai/ba-p/4505653)

