---
layout: post
title: Debugging and Optimizing RAG Agents in Microsoft Foundry
author: Chang Liu
canonical_url: https://devblogs.microsoft.com/foundry/how-to-debug-and-optimize-rag-agents-in-azure-ai-foundry/
viewing_mode: external
feed_name: Microsoft AI Foundry Blog
feed_url: https://devblogs.microsoft.com/foundry/feed/
date: 2025-11-20 16:00:28 +00:00
permalink: /ai/news/Debugging-and-Optimizing-RAG-Agents-in-Microsoft-Foundry
tags:
- A/B Testing
- Agent Workflow
- Agentic Retrieval
- AI
- Azure
- Azure AI Foundry
- Azure AI Search
- AzureOpenAIModelConfiguration
- Coding
- Document Retrieval Evaluator
- Enterprise Data
- Evaluation Metrics
- Foundry Observability
- Groundedness Evaluator
- Information Retrieval
- Knowledge Agent
- ML
- NDCG
- News
- Parameter Tuning
- Pass/Fail Analysis
- Python
- RAG
- Relevance Evaluator
- Retrieval Augmented Generation
- XDCG
section_names:
- ai
- azure
- coding
- ml
---
Chang Liu details how to debug and optimize Retrieval-Augmented Generation (RAG) agents within Microsoft Foundry, sharing code samples, metrics, and evaluation strategies tailored for developers.<!--excerpt_end-->

# How to Debug and Optimize RAG Agents in Microsoft Foundry

Microsoft Foundry provides robust tools for building, debugging, and deploying Retrieval-Augmented Generation (RAG) agents at enterprise scale. This tutorial outlines two critical best practices for developers aiming to maximize agent relevance and reliability:

## Best Practice 1: Evaluate Your RAG End to End

- **Agentic RAG** uses specialized retrieval pipelines, such as the Knowledge Agent in Azure, to handle complex conversational queries against enterprise knowledge bases.
- **Evaluation Metrics (RAG Triad):**
  - **Retrieval:** Is the search result relevant and useful for the query?
  - **Groundedness:** Is the response supported by the underlying source documents?
  - **Relevance:** Does the response suit the user's needs and context?
- **Code Example:**

```python
from azure.search.documents.agent import KnowledgeAgentRetrievalClient
from azure.search.documents.agent.models import (
    KnowledgeAgentRetrievalRequest, KnowledgeAgentMessage, KnowledgeAgentMessageTextContent, SearchIndexKnowledgeSourceParams
)

agent_client = KnowledgeAgentRetrievalClient(endpoint=endpoint, agent_name=agent_name, credential=credential)

# Compose queries and set knowledge sources...

result = agent_client.retrieve(retrieval_request=retrieval_request, api_version=api_version)
```

- **Synthesized Responses:** The agent generates answers, supports reasoning with references, and exposes activity logs useful for troubleshooting and tracing.

## Evaluating With Foundry

- Use built-in evaluators to measure response quality:

```python
from azure.ai.evaluation import AzureOpenAIModelConfiguration, GroundednessEvaluator, RelevanceEvaluator, evaluate

model_config = AzureOpenAIModelConfiguration(...)
groundedness = GroundednessEvaluator(model_config=model_config)
relevance = RelevanceEvaluator(model_config=model_config)
result = evaluate(
    data=filename,
    evaluators={"groundedness": groundedness, "relevance": relevance},
    azure_ai_project=ai_foundry_project_endpoint,
)
```

- Results are accessible in the Foundry UI for visualization and batch comparison; use parameter sweeps and A/B testing to optimize agent configuration.

## Best Practice 2: Optimize RAG Search Parameters

Often, retrieval quality limits RAG agent effectiveness. For advanced scenarios:

- **Document Retrieval Metrics:** Fidelity, NDCG, XDCG, Max Relevance, Holes (missing judgments); higher scores generally reflect better retrieval performance.
- **Code Example:**

```python
from azure.ai.evaluation import DocumentRetrievalEvaluator

doc_retrieval_eval = DocumentRetrievalEvaluator(...)
results = doc_retrieval_eval(
    retrieval_ground_truth=retrieval_ground_truth,
    retrieved_documents=retrieved_documents
)
print(results)
```

- Metrics report pass/fail against set thresholds, supporting parameter sweeps and batch runs across multiple search strategies (text, semantic, vector, hybrid).

## Workflow for Developers

1. Prepare test queries and ground truths (human or LLM-judged relevance scores).
2. Run RAG agent and evaluate with Foundry tools.
3. Use Groundedness/Relevance evaluators for end-to-end checks.
4. Use Document Retrieval evaluators to validate and compare different search parameter sets.
5. Leverage Foundry’s UI for visualization and analysis; choose best-performing parameters with confidence.
6. Integrate optimal settings back into your deployment pipeline.

## Useful Links and Further Reading

- [How to debug your RAG agent in Microsoft Foundry](https://devblogs.microsoft.com/foundry/how-to-debug-and-optimize-rag-agents-in-azure-ai-foundry/)
- [Agentic RAG](https://techcommunity.microsoft.com/blog/azure-ai-services-blog/bonus-rag-time-journey-agentic-rag/4404652)
- [Unlocking the Power of Agentic Applications in Azure AI Foundry](https://devblogs.microsoft.com/foundry/evaluation-metrics-azure-ai-foundry/)
- [Achieve End-to-End Observability in Azure AI Foundry](https://devblogs.microsoft.com/foundry/achieve-end-to-end-observability-in-azure-ai-foundry/)
- [Evaluation sample notebook](https://aka.ms/knowledge-agent-eval-sample)
- [Document retrieval evaluation notebook](https://aka.ms/doc-retrieval-sample)

---

This workflow addresses real developer needs for reliable, explainable RAG agents on Microsoft platforms and provides actionable steps for improving agent performance using practical evaluation strategies.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/how-to-debug-and-optimize-rag-agents-in-azure-ai-foundry/)
