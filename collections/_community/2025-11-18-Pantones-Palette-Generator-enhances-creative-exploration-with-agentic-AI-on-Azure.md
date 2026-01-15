---
layout: post
title: Pantone’s Palette Generator enhances creative exploration with agentic AI on Azure
author: mtoiba
canonical_url: https://techcommunity.microsoft.com/t5/azure-customer-innovation-blog/pantone-s-palette-generator-enhances-creative-exploration-with/ba-p/4469830
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-18 16:54:22 +00:00
permalink: /github-copilot/community/Pantones-Palette-Generator-enhances-creative-exploration-with-agentic-AI-on-Azure
tags:
- Agentic RAG
- AI
- AI Orchestration
- Azure
- Azure AI Search
- Azure Cosmos DB
- Azure Functions
- Azure OpenAI
- Coding
- Color Science
- Community
- Data Persistence
- Design Automation
- Developer Tools
- GitHub Copilot
- LLM Integration
- Microsoft Agent Framework
- Microsoft Foundry
- Multi Agent Architecture
- Pantone Palette Generator
- Prompt Engineering
- Serverless Deployment
- Trend Forecasting
- Vector Search
section_names:
- ai
- azure
- coding
- github-copilot
---
mtoiba details how Pantone's Palette Generator leverages Microsoft Azure AI, GitHub Copilot, and multi-agent systems to enable designers to generate color palettes in seconds, transforming creative workflows.<!--excerpt_end-->

# Pantone’s Palette Generator enhances creative exploration with agentic AI on Azure

Pantone, a globally recognized color authority, has developed the Palette Generator—an AI-powered design agent designed to advance creative workflows for designers and creators. Built on Microsoft Foundry, the solution integrates Azure AI Search, Azure Cosmos DB, Azure Functions, and GitHub Copilot, offering a rapid, reliable way to generate precise, trend-backed color palettes.

## Turning Pantone’s color legacy into an AI offering

For over 60 years, Pantone has enabled creatives to translate inspiration into reproducible color choices. The Palette Generator builds on this legacy by tapping thousands of articles and guides from the *Color Insider* library, alongside Pantone’s trend forecasting expertise. Leveraging Microsoft Foundry and Azure AI services, the platform delivers instant, accurate color palettes tailored to user prompts—for example, "soft pastels for an eco-friendly line of baby clothes" or "vibrant metallics for next spring."

## Technical Architecture and Multi-Agent System

The solution uses a multi-agent architecture developed with Microsoft Agent Framework. Three core agents handle orchestration:

- **Orchestrator Agent:** Receives user prompts and coordinates the workflow.
- **Context Retrieval Agent:** Uses Azure AI Search with vector data indexing to pull relevant insights from Pantone’s datasets, understanding semantics and intent.
- **Palette Assembly Agent:** Applies color science rules to generate palette combinations meeting harmony, contrast, and accessibility standards.

Agents are connected and spun up in just a few lines of code using Microsoft Agent Framework, streamlining development and allowing rapid iteration.

## Data Management and Persistence

Azure Cosmos DB manages user sessions, prompts, and palette results, enabling long-term data persistence. This supports enhanced user experience—designers can revisit past palette explorations—and gives Pantone usage analytics to refine prompt engineering and system improvements.

## AI Search and Retrieval-Augmented Generation (RAG)

Azure AI Search enables vectorized query handling, making palette recommendations that understand descriptive language (like "serene and oceanic"), not just keywords. This enriches search accuracy and reduces reliance on LLM inference calls, resulting in lower latency and better relevancy.

## Developer Productivity with GitHub Copilot

GitHub Copilot supported Pantone’s development teams—saving over 200 person-hours during the proof of concept phase, streamlining code generation for agent orchestration and prompt handling logic.

## Performance Optimization and Scalability

The team improved latency by switching from GPT-4.1 to GPT-5 and employing Azure AI Search for ranking and filtering. Moving orchestration to Azure Functions will fully enable serverless scalability and integration with other Azure services.

## Next Steps and Expansion

Looking ahead, Pantone’s engineering team plans to add specialized agents for palette harmony and trend prediction, deepening the usefulness of the Palette Generator.

---

**Key Takeaways:**

- End-to-end AI integration using Azure services and Microsoft Foundry
- Multi-agent orchestration for context retrieval, semantic search, and palette generation
- GitHub Copilot boosted developer productivity
- Scalable, serverless deployment with Azure Functions on the roadmap
- Persistent data and user analytics through Azure Cosmos DB

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-customer-innovation-blog/pantone-s-palette-generator-enhances-creative-exploration-with/ba-p/4469830)
