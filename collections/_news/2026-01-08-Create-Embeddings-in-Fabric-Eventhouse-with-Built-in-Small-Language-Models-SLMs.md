---
external_url: https://blog.fabric.microsoft.com/en-US/blog/create-embeddings-in-fabric-eventhouse-with-built-in-small-language-models-slms/
title: Create Embeddings in Fabric Eventhouse with Built-in Small Language Models (SLMs)
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2026-01-08 11:00:00 +00:00
tags:
- Azure Data Explorer
- E5 Small V2
- Embedding Generation
- Fabric Eventhouse
- Jina V2 Small
- KQL
- Kusto
- NLP
- Python Plugin
- RAG Pipeline
- Semantic Search
- Slm Embeddings Fl
- Small Language Models
- Update Policy
- Vector Database
section_names:
- ai
- azure
- ml
---
Microsoft Fabric Blog explains how slm_embeddings_fl lets users generate embeddings directly inside Fabric Eventhouse using local SLMs, with code samples and scenarios for RAG, semantic search, and large-scale analytics workflows.<!--excerpt_end-->

# Create Embeddings in Fabric Eventhouse with Built-in Small Language Models (SLMs)

The Microsoft Fabric Blog introduces `slm_embeddings_fl()`, a new user-defined function (UDF) that enables generating text embeddings directly inside Fabric Eventhouse using local Small Language Models (SLMs) running in the Kusto Python sandbox. This approach eliminates the need for external endpoints, callout policies, and reduces operational overhead and cost traditionally associated with cloud API-based embedding generation.

## What is `slm_embeddings_fl()`?

- **A tabular UDF callable in any KQL query**, allowing you to generate embeddings for text columns directly in Eventhouse or Azure Data Explorer
- **Uses local SLMs** rather than requiring Azure OpenAI Service endpoints
- Supports models: [`jina-v2-small`](https://huggingface.co/jinaai/jina-embeddings-v2-small-en) (long-context) and [`e5-small-v2`](https://huggingface.co/intfloat/e5-small-v2) (retrieval-optimized)
- Syntax example:

  ```
  T | invoke slm_embeddings_fl(text_col, embeddings_col [, batch_size ] [, model_name ] [, prefix ])
  ```

- Configurable batch size, model, and (optional) text prefix parameter

## Benefits Over Azure OpenAI for Embeddings

- **Operational Simplicity**: No need to manage Azure OpenAI provision, throttling, timeouts, or request costs
- **Privacy**: Embedding models run locally inside the Kusto Python plugin on your cluster
- **High Throughput**: Embedding generation is only limited by your cluster resources, not remote rate limits
- **Cost Savings**: No per-request cost for embeddings

## Models and Scenarios

- `e5-small-v2`: Best for retrieval-style semantic search (supports input prefixes for queries and passages)
- `jina-v2-small`: Designed for long context inputs (up to 8192 tokens), ideal for large documents

### Example: End-to-End Semantic Search in KQL

1. **Embed documents**:

   ```kql
   .set stored_query_result slm_e5_test_tbl <| datatable(text:string) [
     "Machine learning models can process natural language efficiently.",
     "Python is a versatile programming language for data science.",
     "Azure Data Explorer provides fast analytics on large datasets.",
     "Embeddings convert text into numerical vector representations.",
     "Neural networks learn patterns from training data."
   ]
   | extend text_embeddings=dynamic(null)
   | invoke slm_embeddings_fl('text', 'text_embeddings', model_name='e5-small-v2', prefix='passage:')
   ```

2. **Embed a query and compute similarity**:

   ```kql
   let item = "Embeddings vectors are used for semantic search.";
   let embedding = toscalar(print query=item |
     extend embedding=dynamic(null) |
     invoke slm_embeddings_fl(text_col='query', embeddings_col='embedding', model_name='e5-small-v2', prefix='query:') |
     project embedding);
   stored_query_result('slm_e5_test_tbl') |
     extend item, embedding |
     extend similarity=series_cosine_similarity(embedding, text_embeddings, 1.0, 1.0) |
     project item, text, similarity |
     top 2 by similarity
   ```

## Unlocked Scenarios in Fabric Eventhouse

1. **Instant semantic search for logs, tickets, traces, etc.** — Embed and search any textual column natively in KQL
2. **RAG pipelines and agent memory** — High-performance vector store for retrieval-augmented generation workflows
3. **High-volume embedding generation** — No external rate limits
4. **Efficient long document support** — Jina’s 8K+ token limit reduces chunking overhead
5. **Real-time ingestion with Update Policy** — Auto-embed data on ingest for immediate semantic search

## When to Use slm_embeddings_fl() vs ai_embeddings()

- Use **ai_embeddings()** if you need highest-quality, centrally managed Azure OpenAI models
- Use **slm_embeddings_fl()** for privacy, local processing, cost reduction, and high-throughput scenarios

#### Summary

Local SLM-based embedding support directly in Fabric Eventhouse marks a significant advance for scalable, real-time, and cost-effective AI-powered analytics on Microsoft’s data platform. Whether you're powering semantic search or RAG architectures, these new capabilities minimize operational friction and let you focus on results.

For more implementation details, model links, and Microsoft documentation, see:

- [slm_embeddings_fl() Docs](https://learn.microsoft.com/en-us/kusto/functions-library/slm-embeddings-fl)
- [Optimizing Vector Similarity Search on Azure Data Explorer](https://techcommunity.microsoft.com/blog/azuredataexplorer/optimizing-vector-similarity-search-on-azure-data-explorer-%E2%80%93-performance-update/4033082)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/create-embeddings-in-fabric-eventhouse-with-built-in-small-language-models-slms/)
