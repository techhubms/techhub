---
external_url: https://devblogs.microsoft.com/foundry/ground-your-agents-faster-native-azure-ai-search-indexing-foundry/
title: Ground Your Agents Faster with Native Azure AI Search Indexing in Foundry
author: Farzad Sunavala
viewing_mode: external
feed_name: Microsoft AI Foundry Blog
date: 2025-09-17 08:00:51 +00:00
tags:
- ADLS Gen2
- Agent Development
- Agents
- AI Development
- AI Indexing
- Azure AI Foundry
- Azure AI Search
- Azure Blob Storage
- Azure RBAC
- Data Ingestion
- Embedding Models
- Enterprise Search
- Hybrid Retrieval
- Knowledge Grounding
- Microsoft OneLake
- Retrieval Augmented Generation
- Vector Search
section_names:
- ai
- azure
---
Farzad Sunavala explains how Azure AI Foundry now lets developers instantly create Azure AI Search vector indexes from major cloud data sources, simplifying agent grounding and accelerating AI projects.<!--excerpt_end-->

# Ground Your Agents Faster with Native Azure AI Search Indexing in Foundry

**Author:** Farzad Sunavala

## Overview

Azure AI Foundry introduces a streamlined process that allows developers to natively create an Azure AI Search vector index during the agent "Add knowledge" workflow. With this new capability, ingestion from Azure Blob Storage, ADLS Gen2, or Microsoft OneLake is seamless, requiring no pre-existing index or manual pipeline setup. The indexing flow tightly integrates embedding model selection and hybrid vector-plus-keyword retrieval, all protected by Azure RBAC and network isolation features.

## Why This Matters

Traditional grounding of AI agents previously demanded manual setup of Azure AI Search indexes, custom schema definition, and pipeline work—all acting as barriers to quick experimentation and integration. With Foundry’s enhancement, developers can:

- Choose a data source from supported Azure storage options
- Select an Azure OpenAI embedding model (e.g., `text-embedding-*`)
- Initiate automatic ingestion, document chunking, embedding, and index creation—all with a single click
- Enable their agents to answer grounded, enterprise-specific questions instantly

## Key Capabilities

- **Inline index creation:** No need to bring or configure a pre-existing Azure AI Search index.
- **Automatic ingestion:** Content is pulled and prepared for embeddings automatically.
- **Embedding model selection:** Choose models during creation for tailored results.
- **Hybrid-ready:** Index supports combined vector and keyword search.
- **Security:** Built-in respect for Azure RBAC and network isolation of data.

## Supported Data Sources (Initial Wave)

- **Azure Blob Storage**
- **Azure Data Lake Storage (ADLS) Gen2**
- **Microsoft OneLake (Fabric)**

## How To Use It

1. Open or create an agent in Azure AI Foundry.
2. Click **Add knowledge**.
3. Choose your preferred data source (Blob / ADLS Gen2 / OneLake).
4. Authorize the data source if required and select relevant containers or file paths.
5. Select an Azure OpenAI embedding model.
6. Hit **Create index & ingest**.
7. Foundry ingests, chunks, embeds, and provisions an Azure AI Search index for your agent.
8. Your agent is now immediately ready to answer grounded questions using your data.

No extra pipelines, no schema hand-editing—just connect, select, and deploy.

## Additional Resources

- [How to create an Azure AI Search index in Foundry (Tutorial)](https://review.learn.microsoft.com/en-us/azure/ai-foundry/agents/how-to/tools/azure-ai-search?branch=main&branchFallbackFrom=pr-en-us-6213&tabs=azurecli)
- [Azure AI Search Concepts](https://learn.microsoft.com/azure/search/search-what-is-azure-search)
- [Hybrid Retrieval Overview](https://learn.microsoft.com/azure/search/hybrid-search-overview)
- [Embeddings Models in Foundry](https://learn.microsoft.com/azure/ai-foundry/openai/concepts/models)
- [Agentic Retrieval Updates in Azure AI Search](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/agentic-retrieval-updates-in-azure-ai-search/4450621)

Developers are encouraged to try out the new workflow and share their experiences and launches using #AzureAIFoundry.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/ground-your-agents-faster-native-azure-ai-search-indexing-foundry/)
