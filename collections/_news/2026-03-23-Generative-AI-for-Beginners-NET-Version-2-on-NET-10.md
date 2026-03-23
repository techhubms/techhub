---
tags:
- .NET
- .NET 10
- .NET Aspire
- .NET Fundamentals
- Agent Framework
- AI
- AI Course
- Azure CLI Authentication
- AzureCliCredential
- C#
- Content Safety
- Dependency Injection
- Evaluation
- Generative AI
- GitHub Codespaces
- GitHub Repository
- IChatClient
- MCP
- Microsoft Agent Framework
- Microsoft.Extensions.AI
- Middleware Pipeline
- Multi Agent Systems
- News
- Open Source Course
- Orchestration
- RAG
- Responsible AI
- Retrieval Augmented Generation
- Semantic Kernel
- Tool Use
external_url: https://devblogs.microsoft.com/dotnet/generative-ai-for-beginners-dotnet-version-2-on-dotnet-10/
author: Pablo Lopes, Bruno Capuano
date: 2026-03-23 17:05:00 +00:00
title: 'Generative AI for Beginners .NET: Version 2 on .NET 10'
feed_name: Microsoft .NET Blog
primary_section: ai
section_names:
- ai
- dotnet
---

Pablo Lopes and Bruno Capuano announce Version 2 of the free, open-source “Generative AI for Beginners .NET” course, rebuilt on .NET 10 with Microsoft.Extensions.AI, refreshed RAG samples, and a new Microsoft Agent Framework lesson aimed at helping developers build more production-ready AI apps.<!--excerpt_end-->

# Generative AI for Beginners .NET: Version 2 on .NET 10

Today, **Version 2** of [Generative AI for Beginners .NET](https://aka.ms/genainet) is released: a free, open-source course for building **AI-powered .NET applications**.

![Graphic for the announcement of Generative AI for Beginners .NET - Version 2](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2026/03/GenAiV2.webp)

## TL;DR

- Completely rewritten curriculum with **five structured lessons**
- All samples updated to **.NET 10**
- New AI abstraction layer using **Microsoft.Extensions.AI**
- Updated **RAG** implementations using **native SDKs**
- New **Microsoft Agent Framework** lesson

## Five lessons, fully rewritten

Version 2 restructures and rewrites the course into five lessons, each with explanations, working samples, and a learning progression:

1. **Introduction to Generative AI**
   - What generative AI is and how it differs from traditional programming
   - Why .NET is a first-class citizen for AI development
   - The Microsoft AI stack and where each piece fits
   - How to run samples in GitHub Codespaces or configure local development

2. **Generative AI Techniques**
   - Chat conversations with context and memory
   - Text embeddings: what they are and why they matter
   - Processing multiple content types (images and documents)
   - Calling AI models using **Microsoft.Extensions.AI** abstractions

3. **AI Patterns and Applications**
   - Building semantic search that understands meaning
   - Implementing **retrieval augmented generation (RAG)**
   - Building applications that process and understand documents
   - When to use each pattern and how to combine them

4. **Agents with MAF**
   - Multi-agent systems using the **Microsoft Agent Framework**
   - Tool use, orchestration, and collaboration between agents

5. **Responsible AI**
   - Safety, content filtering, evaluation
   - Practices for shipping AI features responsibly

Lesson links:

- [Introduction to Generative AI](https://github.com/microsoft/Generative-AI-for-beginners-dotnet/blob/main/01-IntroductionToGenerativeAI/readme.md)
- [Generative AI Techniques](https://github.com/microsoft/Generative-AI-for-beginners-dotnet/blob/main/02-GenerativeAITechniques/readme.md)
- [AI Patterns and Applications](https://github.com/microsoft/Generative-AI-for-beginners-dotnet/blob/main/03-AIPatternsAndApplications/readme.md)
- [AI Agents with Microsoft Agent Framework](https://github.com/microsoft/Generative-AI-for-beginners-dotnet/blob/main/04-AgentsWithMAF/readme.md)
- [Responsible AI](https://github.com/microsoft/Generative-AI-for-beginners-dotnet/blob/main/05-ResponsibleAI/readme.md)

## .NET 10 across the board

The samples were updated to modern .NET patterns including:

- **Dependency injection**
- **Middleware pipelines**
- **File-based apps** introduced in .NET 10
- Standardized authentication

### Authentication standardization

All file-based samples now use `AzureCliCredential`, so you authenticate once with the **Azure CLI** and the samples pick it up without requiring you to manage connection strings or API keys across many projects.

### Model references

Documentation and samples reference `gpt-5-mini` across the course.

## Microsoft.Extensions.AI as the foundation

Version 1 used **Semantic Kernel** as the primary approach to talk to AI models. Version 2 uses **Microsoft.Extensions.AI (MEAI)**.

The rationale given:

- MEAI ships as part of the **.NET 10 ecosystem**
- It follows familiar .NET patterns like `ILogger` and `IConfiguration`
- It works across providers without locking you into a specific orchestration framework

As part of the rewrite, core samples were moved to `IChatClient` and the MEAI middleware pipeline, aiming to simplify common scenarios (like basic chat completions) to look like normal .NET service registration.

A demo video is referenced here:

https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2026/03/DemoGenAi.webm

## RAG samples rewritten with native SDKs

Changes called out:

- **11 pure Semantic Kernel samples** were moved to `samples/deprecated/`
  - They still build and work, but are no longer on the main learning path.
- Mixed SK + MEAI samples (examples given: `BasicChat-05AIFoundryModels`, `BasicChat-11FoundryClaude`) were updated to remove Semantic Kernel dependencies and run on MEAI.

The stated intent is that a beginner course should focus on the “foundational layer” first (MEAI in .NET 10), while **Microsoft Agent Framework** is positioned as the main toolkit for agentic scenarios.

## Microsoft Agent Framework RC

Lesson 4 covers **Microsoft Agent Framework**, and the course documents it.

It also mentions five MAF web app samples covering:

- Multi-agent orchestration
- PDF ingestion
- Chat middleware patterns

## Updated translations

Eight language translations were updated to match the new structure and changes:

- Chinese
- French
- Portuguese
- Spanish
- German
- Japanese
- Korean
- Traditional Chinese

## Getting started

- Start here: [Generative AI for Beginners – .NET](https://aka.ms/genainet)
- Pick a provider (**Microsoft Foundry** or **Ollama** for local development)
- Work through the lessons in order

Community links:

- Issues: [https://github.com/microsoft/Generative-AI-for-beginners-dotnet/issues](https://github.com/microsoft/Generative-AI-for-beginners-dotnet/issues)
- Pull requests: [https://github.com/microsoft/Generative-AI-for-beginners-dotnet/pulls](https://github.com/microsoft/Generative-AI-for-beginners-dotnet/pulls)


[Read the entire article](https://devblogs.microsoft.com/dotnet/generative-ai-for-beginners-dotnet-version-2-on-dotnet-10/)

