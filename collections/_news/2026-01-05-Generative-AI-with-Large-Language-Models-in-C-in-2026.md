---
external_url: https://devblogs.microsoft.com/dotnet/generative-ai-with-large-language-models-in-dotnet-and-csharp/
title: Generative AI with Large Language Models in C# in 2026
author: Jeremy Likness
viewing_mode: external
feed_name: Microsoft .NET Blog
date: 2026-01-05 18:05:00 +00:00
tags:
- .NET
- Agentic Workflows
- AI Engineering
- Azure OpenAI
- Azure OpenAI Service
- C#
- ChatGPT
- Embeddings
- Generative AI
- GitHub Models
- Large Language Models
- MCP
- MEAI
- Microsoft Extensions For AI
- Microsoft Foundry
- Ollama
- Prompt Engineering
- Rag
- Semantic Kernel
- Tokenization
- Vector Databases
section_names:
- ai
- azure
- coding
---
Jeremy Likness introduces .NET developers to the essentials of generative AI with large language models, focusing on C# and the evolving Microsoft platform ecosystem.<!--excerpt_end-->

# Generative AI with Large Language Models in C# in 2026

Jeremy Likness presents a comprehensive overview for .NET developers wanting to understand or build generative AI–powered solutions with C# using Microsoft's modern set of services and libraries.

## Introduction

Generative AI is now a mainstream technology. With the launch of OpenAI's GPT-3.5 and the Azure OpenAI Service, Microsoft has positioned itself as a leading provider of secure, scalable access to powerful language models. This article revisits AI fundamentals and recent advancements targeting .NET and C# developers.

## Key AI Concepts and Terms

- **Artificial Intelligence (AI)**: Software that mimics tasks typically needing human intelligence, including reasoning, language, and perception. Modern talks of 'AI' usually mean generative AI.
- **Generative AI**: Systems capable of creating new text, images, code, etc. GPT (Generative Pre-trained Transformer) models are at the center of this trend.
- **Large Language Models (LLMs)**: Models like GPT-3.5/4, trained on massive datasets, excel in multilingual content generation and contextual understanding. Their magic lies in semantic understanding—going beyond string matching to true meaning.
- **Tokens and Embeddings**: LLMs process data as tokens—word fragments, full words, or characters—then convert to embeddings (vectors representing meaning). Semantic search and content relevance operate on these embeddings.
- **Model Size (Parameters)**: Modern models vary by parameter count, from millions (GPT-1) to hundreds of billions. More parameters generally mean richer and more nuanced models.
- **Prompts and System Instructions**: The user gives a prompt; the system provides hidden instructions to guide behavior. LLMs can also use function calling to leverage outside tools and APIs (e.g., weather, databases).
- **Retrieval-Augmented Generation (RAG)**: LLMs combine generation with retrieval from external knowledge sources, greatly improving factual reliability and specialization.
- **Model Context Protocol (MCP)**: A standard for agent and tool interoperability in AI workflows.
- **Agents**: Combinations of models, tools, and context tailored for specialized solutions.

## Timeline: The Rise of AI in .NET

From GPT-1's release to 2026, AI capabilities have grown rapidly—now including enterprise-scale, local, and open-source options accessible from .NET.

## Microsoft Ecosystem for AI in C#

### 1. Azure OpenAI Service

Microsoft's managed API for OpenAI models provides secure endpoints for enterprise use, allowing C# apps to use state-of-the-art LLMs in production.

### 2. Semantic Kernel (SK)

A library for managing prompts, memories, and plugins—enabling orchestration of LLM workflows using C# or Python.

### 3. Microsoft Extensions for AI (MEAI)

Provides unified abstractions with interfaces like `IChatClient`, making it easier to work across model providers.

### 4. GitHub Models

A hosted catalog providing API-compatible access to a range of open and frontier LLMs. Great for prototyping and automation—no infrastructure overhead.

### 5. Microsoft Foundry

The enterprise rebrand of Azure AI Studio, offering model catalogs, agentic workflows, security, governance, and fine-tuning, both in cloud and on-premises via Foundry Local.

### 6. Ollama

A local runtime for running open LLMs (like Mistral, Llama 3, Phi-3) on developer workstations, integrated via community-maintained libraries like OllamaSharp.

## Unified Abstractions and Integration

The .NET team is advancing a unified API approach by standardizing on contracts like `IChatClient` for model conversations—making it easier to switch providers and adopt new tools with minimal code changes. These extensions enable flexible middleware for logging, tracing, behavioral customization, and more.

## Conclusion

C# developers now have access to a mature, extensive toolkit for generative AI—from hosted APIs (Azure OpenAI, GitHub Models) to on-premises inference (Foundry Local, Ollama)—all orchestrated with unified extensions. For more deep dives, subscribe to the .NET blog and community events.

---

*References:*

- [Azure OpenAI Service](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/)
- [Semantic Kernel](https://github.com/microsoft/semantic-kernel)
- [Microsoft Foundry](https://ai.azure.com/)
- [GitHub Models](https://github.com/features/models)
- [OllamaSharp](https://github.com/awaescher/OllamaSharp)

For further learning, explore upcoming community standups on the [.NET YouTube Channel](https://www.youtube.com/@dotnet) and join the Microsoft Source newsletter.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/generative-ai-with-large-language-models-in-dotnet-and-csharp/)
