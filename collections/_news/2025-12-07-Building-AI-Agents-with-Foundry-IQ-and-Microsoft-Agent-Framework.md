---
layout: post
title: Building AI Agents with Foundry IQ and Microsoft Agent Framework
author: Farzad Sunavala, Eduard van Valkenburg
canonical_url: https://devblogs.microsoft.com/foundry/foundry-iq-agent-framework-integration/
viewing_mode: external
feed_name: Microsoft AI Foundry Blog
feed_url: https://devblogs.microsoft.com/foundry/feed/
date: 2025-12-07 10:55:00 +00:00
permalink: /ai/news/Building-AI-Agents-with-Foundry-IQ-and-Microsoft-Agent-Framework
tags:
- Agent Development
- Agent Framework
- Agentic AI
- Agentic Mode
- Agentic Retrieval
- Answer Synthesis
- Azure AI Search
- Azure OpenAI
- Context Provider
- Context Providers
- Enterprise AI
- Enterprise Knowledge
- Foundry Iq
- Knowledge Bases
- Managed Identity
- Microsoft Agent Framework
- Microsoft Foundry
- Multi Hop Reasoning
- Open Source AI
- Python
- Query Planning
- Rag
- Semantic Search
section_names:
- ai
- azure
- coding
---
Farzad Sunavala and Eduard van Valkenburg detail how the Azure AI Search Context Provider and Foundry IQ enable developers to build intelligent, context-aware AI agents using minimal Python code in the Microsoft Agent Framework.<!--excerpt_end-->

# Building AI Agents with Foundry IQ and Microsoft Agent Framework

## Executive Summary

Foundry IQ, an intelligent knowledge layer from Microsoft, revolutionizes retrieval-augmented generation (RAG) for AI agents. By treating retrieval as a reasoning task, Foundry IQ enables agents to plan queries, follow chains of information, and synthesize answers drawn from multiple sources.

## Addressing Challenges with Traditional RAG

Traditional retrieval approaches require custom data connectors, chunking logic, and permissions for each project, resulting in duplicated efforts and fragmented pipelines. Foundry IQ mitigates this by centralizing retrieval logic into reusable Knowledge Bases. Developers can plug in a domain Knowledge Base, and Foundry IQ manages indexing, vectorization, query planning, and multi-source routing behind the scenes.

## Quick Start

**Prerequisites:**

- Azure AI Search service
- Azure AI Foundry project
- Python 3.10+
- [Code samples available](https://github.com/microsoft/agent-framework/tree/main/python/samples/getting_started/context_providers/azure_ai_search)

**Installation:**

```bash
pip install agent-framework-azure-ai-search --pre
```

**Minimal Example (Agentic Mode):**

```python
import asyncio
from agent_framework import ChatAgent
from agent_framework.azure import AzureAIAgentClient, AzureAISearchContextProvider
from azure.identity.aio import DefaultAzureCredential

async def main():
    credential = DefaultAzureCredential()
    async with (
        AzureAISearchContextProvider(
            endpoint="YOUR_SEARCH_ENDPOINT",
            knowledge_base_name="YOUR_KNOWLEDGE_BASE",
            credential=credential,
            mode="agentic",
        ) as search,
        AzureAIAgentClient(
            project_endpoint="YOUR_PROJECT_ENDPOINT",
            model_deployment_name="YOUR_MODEL",
            async_credential=credential,
        ) as client,
        ChatAgent(chat_client=client, context_providers=[search]) as agent,
    ):
        print((await agent.run("What's in the knowledge base?")).text)

asyncio.run(main())
```

Your AI agent is now equipped with enterprise-level knowledge retrieval.

## Microsoft Agent Framework Overview

The [Microsoft Agent Framework](https://aka.ms/agent-framework) is an open-source, model-agnostic platform for building agentic AI solutions in Python and .NET. Key features include:

- Unified agent abstractions across leading LLM providers (Azure OpenAI, OpenAI, Anthropic)
- Pluggable context providers for dynamic information injection
- Standardized integration with external tools
- Support for agent communication protocols (A2A, AG-UI)

## Foundry IQ Fundamentals

Foundry IQ leverages Azure AI Search to perform advanced, reasoning-based retrieval. Unlike simple index queries, Foundry IQ supports:

- Query planning by LLMs
- Multi-hop reasoning across documents
- Synthesized answers with citations
- Unified access to multiple data sources

Microsoft’s internal benchmarks show up to 36% increased relevance on complex multi-hop queries compared to classic RAG solutions. [More details](https://aka.ms/AISearch-KB-evals).

## Retrieval Modes: Semantic vs Agentic

| Aspect                    | Semantic Mode            | Agentic Mode (Foundry IQ)    |
|---------------------------|--------------------------|------------------------------|
| Speed                     | Fast                     | Slower (includes planning)   |
| Accuracy                  | Good (direct lookups)    | Excellent (complex queries)  |
| Query Complexity          | Single-hop               | Multi-hop reasoning          |
| Best For                  | Speed-sensitive tasks    | Analytical/research tasks    |

**Semantic Mode Example:**

```python
async with (
    AzureAISearchContextProvider(
        endpoint="https://mysearch.search.windows.net",
        index_name="product-catalog",
        credential=credential,
        mode="semantic",
        top_k=5,
    ) as search,
    AzureAIAgentClient(
        project_endpoint="YOUR_PROJECT_ENDPOINT",
        model_deployment_name="gpt-4o",
        async_credential=credential,
    ) as client,
    ChatAgent(
        chat_client=client,
        instructions="You help customers find products.",
        context_providers=[search],
    ) as agent,
):
    response = await agent.run("What laptops do you have under $1000?")
    print(response.text)
```

**Agentic Mode Example:**

```python
async with (
    AzureAISearchContextProvider(
        endpoint="https://mysearch.search.windows.net",
        knowledge_base_name="enterprise-kb",
        credential=credential,
        mode="agentic",
    ) as search,
    # ...
):
```

_Note: Foundry IQ currently supports Azure OpenAI models only._ [See supported models](https://learn.microsoft.com/azure/search/agentic-retrieval-how-to-create-knowledge-base?tabs=rbac&pivots=python).

## Configuration Options

- **Retrieval Reasoning Effort**: Choose minimal, low, or medium planning for queries.
- **Output Modes**:
  - `extractive_data`: Returns raw data to the agent
  - `answer_synthesis`: Synthesizes answers using the Knowledge Base

## Resources

- **Install**: `pip install agent-framework-azure-ai-search --pre`
- **Documentation**: [Azure AI Search Context Provider](https://github.com/microsoft/agent-framework/tree/main/python/samples/getting_started/context_providers/azure_ai_search)
- **Video Walkthrough**: [Foundry IQ for multi-source AI knowledge bases](https://youtu.be/bHL1jbWjJUc?si=I4FgUT_EHL9CkQfa)
- **Demo Application**: [Knowledge Bases and agentic RAG](https://aka.ms/foundry-iq-demo)

## Summary

With the Azure AI Search Context Provider and Foundry IQ, developers can rapidly build intelligent, context-aware AI agents able to draw upon a unified enterprise knowledge base. The solution supports configurable retrieval strategies and seamless integration with Microsoft Agent Framework, empowering teams to move beyond fragmented pipelines toward scalable, maintainable AI solutions.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/foundry-iq-agent-framework-integration/)
