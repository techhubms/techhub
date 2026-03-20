---
date: 2026-03-19 07:00:00 +00:00
primary_section: ai
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-knowledge-grounded-ai-agents-with-foundry-iq/ba-p/4499683
tags:
- Agentic Retrieval
- AI
- Azure
- Azure AI Foundry
- Azure AI Projects
- Azure AI Projects SDK
- Azure AI Search
- Azure Blob Storage
- Azure Role Assignment
- Citations
- Community
- DefaultAzureCredential
- Foundry Agent Service
- Knowledge Base Retrieve
- Knowledge Bases
- Managed Identity
- MCP
- Microsoft OneLake
- Microsoft Purview Sensitivity Labels
- Permission Aware Retrieval
- Python
- Query Decomposition
- RAG
- RBAC
- Retrieval Augmented Generation
- Security
- Semantic Reranking
- SharePoint
- Vector Embeddings
author: NelsonKumari
section_names:
- ai
- azure
- security
title: Building Knowledge-Grounded AI Agents with Foundry IQ
feed_name: Microsoft Tech Community
---

NelsonKumari explains how to connect Foundry IQ (built on Azure AI Search) to Foundry Agent Service using MCP, so agents can retrieve enterprise data with permissions enforced and return citation-backed answers.<!--excerpt_end-->

## Overview

Foundry IQ integrates with Foundry Agent Service via **MCP (Model Context Protocol)** to help you build **knowledge-grounded AI agents** that retrieve and reason over enterprise data.

Key capabilities:

- Auto-chunking of documents
- Vector embedding generation
- Permission-aware retrieval
- Semantic reranking
- Citation-backed responses

The goal is to deliver responses that are **accurate, traceable (citations), and aligned with user permissions**.

## Why use Foundry IQ with Foundry Agent Service?

### Intelligent retrieval

Foundry IQ extends beyond basic vector search with:

- LLM-powered query decomposition
- Parallel retrieval across multiple sources
- Semantic reranking of results

This is aimed at improving retrieval quality for complex questions.

### Permission-aware retrieval

Agents only access content a user is authorized to see. ACLs from sources such as:

- SharePoint
- OneLake
- Azure Blob Storage

…are synchronized and enforced **at query time**.

### Auto-managed indexing

Foundry IQ automatically manages:

- Document chunking
- Vector embedding generation
- Indexing

This avoids manually building ingestion pipelines.

## The three pillars of Foundry IQ

### 1) Knowledge sources

Connect to enterprise data locations (SharePoint, Azure Blob Storage, OneLake, and more). When you add a source:

- **Auto-chunking** — documents are split into optimal segments
- **Auto-embedding** — embeddings are generated automatically
- **Auto-ACL sync** — permissions are synchronized (e.g., SharePoint, OneLake)
- **Auto-Purview integration** — sensitivity labels are respected (where supported)

### 2) Knowledge bases

A Knowledge Base unifies multiple sources into a single queryable index. Multiple agents can share the same KB for consistent answers.

### 3) Agentic retrieval

An LLM-assisted retrieval pipeline that:

- Decomposes complex questions into subqueries
- Executes searches in parallel across sources
- Applies semantic reranking
- Returns a unified response with citations

The **retrievalReasoningEffort** parameter controls reasoning depth:

- `minimal` — fast queries
- `low` — balanced reasoning
- `medium` — complex multi-part questions

## Project architecture (high level)

- Agent (model: `gpt-4.1`) in **Foundry Agent Service**
- Agent calls an **MCP tool** (`knowledge_base_retrieve`)
- Tool uses a **project connection** (RemoteTool + Managed Identity auth)
- MCP endpoint is exposed by Foundry IQ on **Azure AI Search**:
  - `/knowledgebases/{kb-name}/mcp?api-version=2025-11-01-preview`

## Prerequisites

### Enable RBAC on Azure AI Search

```bash
az search service update --name your-search --resource-group your-rg \
  --auth-options aadOrApiKey
```

### Assign a role to the project's managed identity

```bash
az role assignment create --assignee $PROJECT_MI \
  --role "Search Index Data Reader" \
  --scope "/subscriptions/.../Microsoft.Search/searchServices/{search}"
```

### Install Python dependencies

```bash
pip install azure-ai-projects>=2.0.0b4 azure-identity python-dotenv requests
```

## Connect a knowledge base to an agent via MCP

The integration is described as three steps:

1. Create a project connection to the knowledge base (using **ProjectManagedIdentity** auth)
2. Create an agent with an **MCPTool** configured for `knowledge_base_retrieve`
3. Chat with the agent using the OpenAI client

### Step 1: Create project connection

```python
import requests
from azure.identity import DefaultAzureCredential, get_bearer_token_provider

credential = DefaultAzureCredential()

PROJECT_RESOURCE_ID = "/subscriptions/.../providers/Microsoft.CognitiveServices/accounts/.../projects/..."
MCP_ENDPOINT = "https://{search}.search.windows.net/knowledgebases/{kb}/mcp?api-version=2025-11-01-preview"

def create_project_connection():
    """Create MCP connection to knowledge base."""
    bearer = get_bearer_token_provider(credential, "https://management.azure.com/.default")

    response = requests.put(
        f"https://management.azure.com{PROJECT_RESOURCE_ID}/connections/kb-connection?api-version=2025-10-01-preview",
        headers={"Authorization": f"Bearer {bearer()}"},
        json={
            "name": "kb-connection",
            "properties": {
                "authType": "ProjectManagedIdentity",
                "category": "RemoteTool",
                "target": MCP_ENDPOINT,
                "isSharedToAll": True,
                "audience": "https://search.azure.com/",
                "metadata": {"ApiType": "Azure"}
            }
        }
    )
    response.raise_for_status()
```

### Step 2: Create an agent with MCP tool

```python
from azure.ai.projects import AIProjectClient
from azure.ai.projects.models import PromptAgentDefinition, MCPTool

def create_agent():
    client = AIProjectClient(endpoint=PROJECT_ENDPOINT, credential=credential)

    # MCP tool connects agent to knowledge base
    mcp_kb_tool = MCPTool(
        server_label="knowledge-base",
        server_url=MCP_ENDPOINT,
        require_approval="never",
        allowed_tools=["knowledge_base_retrieve"],
        project_connection_id="kb-connection"
    )

    # Create agent with knowledge base tool
    agent = client.agents.create_version(
        agent_name="enterprise-assistant",
        definition=PromptAgentDefinition(
            model="gpt-4.1",
            instructions="""You MUST use the knowledge_base_retrieve tool for every question. Include citations from sources.""",
            tools=[mcp_kb_tool]
        )
    )

    return agent, client
```

### Step 3: Chat with the agent

```python
def chat(agent, client):
    openai_client = client.get_openai_client()
    conversation = openai_client.conversations.create()

    while True:
        question = input("You: ").strip()
        if question.lower() == "quit":
            break

        response = openai_client.responses.create(
            conversation=conversation.id,
            input=question,
            extra_body={
                "agent_reference": {
                    "name": agent.name,
                    "type": "agent_reference"
                }
            }
        )

        print(f"Assistant: {response.output_text}")
```

## References

- Azure AI Search Knowledge Stores: https://learn.microsoft.com/azure/search/search-knowledge-stores
- Foundry Agent Service: https://learn.microsoft.com/azure/ai-services/agents/
- Model Context Protocol (MCP): https://modelcontextprotocol.io/
- Azure AI Projects SDK: https://pypi.org/project/azure-ai-projects/

## Summary

Foundry IQ + Foundry Agent Service provides a pattern for building enterprise agents that:

- Use **MCP-based tool calling** to query a knowledge base
- Enforce **permission-aware retrieval** (ACLs, RBAC/managed identity)
- Automate **chunking/embedding/indexing**
- Return **citation-backed** answers with semantic reranking

[Read the entire article](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-knowledge-grounded-ai-agents-with-foundry-iq/ba-p/4499683)

