---
layout: post
title: Evaluating GPT-5 Models for RAG on Azure AI Foundry
author: Pamela_Fox
canonical_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/gpt-5-will-it-rag/ba-p/4442676
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community
date: 2025-08-13 07:54:51 +00:00
permalink: /ai/community/Evaluating-GPT-5-Models-for-RAG-on-Azure-AI-Foundry
tags:
- AI
- Azure
- Azure AI Evaluation
- Azure AI Foundry
- Clarifying Questions
- Community
- Evaluation SDK
- GPT 5
- Groundedness
- Hybrid Search
- Latency Metrics
- LLM Evaluation
- OpenAI
- Prompt Engineering
- RAG
- Relevance Metrics
- Retrieval Augmented Generation
- Synonyms Search
section_names:
- ai
- azure
---
Pamela Fox evaluates the performance of GPT-5 models in Retrieval-Augmented Generation use cases on Azure AI Foundry, providing practical insights on accuracy, relevance, and developer experience for AI practitioners.<!--excerpt_end-->

# Evaluating GPT-5 Models for Retrieval-Augmented Generation (RAG) on Azure AI Foundry

**Author:** Pamela Fox  
**Published:** August 13, 2025

## Overview

OpenAI's GPT-5 model family—including variants like gpt-5-mini, gpt-5-chat, and gpt-5-pro—is now available in Azure AI Foundry, bringing enhanced capabilities for Retrieval-Augmented Generation (RAG) applications. This article documents a comprehensive evaluation of GPT-5's performance versus earlier models, focusing on groundedness, relevance, and pragmatism in responding to user queries.

## GPT-5 Variants Evaluated

- **gpt-5:** Core reasoning model
- **gpt-5-mini:** Smaller footprint for efficiency
- **gpt-5-nano:** Not evaluated in depth for RAG due to lackluster manual testing
- **gpt-5-chat:** Optimized for conversational use, less for nuanced reasoning
- **gpt-5-pro:** Currently ChatGPT-only

## Why RAG and Hallucination Reduction Matter

RAG systems combine retrieval from authoritative data sources with generative models. GPT-5 emphasizes disciplined tool use and reduced hallucinations. During testing, GPT-5 notably refused to answer when underlying documents lacked the relevant information, favoring "I don’t know" over plausible-sounding but unsupported responses.

## Evaluation Setup and Metrics

- **Tools:** [azure-ai-evaluation SDK](https://github.com/Azure/azure-sdk-for-python/tree/main/sdk/evaluation/azure-ai-evaluation), open source RAG template on Azure
- **Metrics:**
  - **Groundedness:** Is the answer supported by the retrieved context (LLM-judged)?
  - **Relevance:** Does the response fully answer the question (LLM-judged)?
  - **Citations Matched:** Alignment of citations with ground truth (regex-based)
- **Test Set:** 50 Q/A pairs on HR document search

## Results and Insights

- **Groundedness:**
  - GPT-5 achieved a 100% pass rate (scores 4-5/5)
  - Most models in the family performed at or above baseline (gpt-4.1-mini)
- **Relevance:**
  - GPT-4.1-mini led with 94% pass; GPT-5 was close behind, but sometimes withheld answers, preferring partial or withheld responses to avoid inaccuracy
- **Latency:**
  - GPT-5 models showed higher average latency; suitable for developer-controlled settings but consider response time for user-facing apps
- **Citations Matched:**
  - Generally strong, indicating effectiveness in relevant document retrieval
- **Model Behavior Differences:**
  - **Honesty:** GPT-5 is more likely to admit information gaps upfront
  - **Clarifying Questions:** GPT-5-chat uniquely asks users to clarify ambiguous requests
  - **Answer Formatting:** GPT-5 reasoning models prefer lists; gpt-5-chat gives shorter, less structured answers
  - **Query Rewriting:** GPT-5 variants expand queries with synonyms, potentially benefiting hybrid/vector search
  - **Unicode Handling:** Use of smart quotes, which may affect downstream text processing

## Practical Considerations for Developers

- **Prompt Engineering:** You may want to modify system prompts for answer length, structure, or style (e.g., discourage lists)
- **Latency Management:** Adjust "reasoning_effort" and verbosity parameters as appropriate for your application
- **Evaluation Tools:** Leverage SDKs like azure-ai-evaluation, [DeepEval](https://deepeval.com/), or [promptfoo](https://github.com/promptfoo/promptfoo) for model benchmarking
- **Production Readiness:** Test on domain-relevant datasets and with subject experts for real-world validity, as high grounding scores don't guarantee factual accuracy

## Next Steps

If you use the [Azure open source RAG template](https://github.com/Azure-Samples/azure-search-openai-demo/), deploy GPT-5 and try the evaluation methodology shared above. Iterating on evaluation and refining prompts can yield more honest, relevant, and user-friendly QA experiences powered by Azure.

**References:**

- [azure-ai-evaluation SDK](https://github.com/Azure/azure-sdk-for-python/tree/main/sdk/evaluation/azure-ai-evaluation)
- [Azure RAG Demo](https://github.com/Azure-Samples/azure-search-openai-demo/)
- [Model Announcement](https://azure.microsoft.com/en-us/blog/gpt-5-in-azure-ai-foundry-the-future-of-ai-apps-and-agents-starts-here/)

---
*This practical exploration is based on the work and analysis of Pamela Fox. For further details and the latest insights, follow her publications and the Microsoft Developer Community Blog.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/gpt-5-will-it-rag/ba-p/4442676)
