---
layout: "post"
title: "What’s New in Microsoft Foundry: Models, SDKs, and Platform Updates (Dec 2025 – Jan 2026)"
description: "This comprehensive update covers all major changes in Microsoft Foundry for December 2025 and January 2026, including new AI models (GPT-5.2, Codex Max), audio and image generation improvements, expanded fine-tuning options, SDK unification, platform enhancements, and deprecation notices. The post is designed for developers building advanced AI and agentic solutions with Foundry on Azure."
author: "Nick Brady"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/foundry/whats-new-in-microsoft-foundry-dec-2025-jan-2026/"
viewing_mode: "external"
feed_name: "Microsoft AI Foundry Blog"
feed_url: "https://devblogs.microsoft.com/foundry/feed/"
date: 2026-02-19 01:01:09 +00:00
permalink: "/2026-02-19-Whats-New-in-Microsoft-Foundry-Models-SDKs-and-Platform-Updates-Dec-2025-Jan-2026.html"
categories: ["AI", "Azure", "Coding", "ML"]
tags: [".NET SDK", "A2A Tool", "Agents", "AI", "ASR", "Audio", "Azure", "Azure AI Projects", "Azure Machine Learning", "CI/CD", "Codex Max", "Coding", "DeepSeek V3.2", "Fine Tuning", "FLUX.2", "Foundry Agent Service", "GPT 5.2", "Image Generation", "JavaScript SDK", "Mcp", "MCP Server", "Memory Store", "Microsoft Foundry", "Mistral Large 3", "ML", "Models", "News", "OpenAI Responses", "Python SDK", "RAG", "SDK", "TTS", "VS Code Extension"]
tags_normalized: ["dotnet sdk", "a2a tool", "agents", "ai", "asr", "audio", "azure", "azure ai projects", "azure machine learning", "cislashcd", "codex max", "coding", "deepseek v3dot2", "fine tuning", "fluxdot2", "foundry agent service", "gpt 5dot2", "image generation", "javascript sdk", "mcp", "mcp server", "memory store", "microsoft foundry", "mistral large 3", "ml", "models", "news", "openai responses", "python sdk", "rag", "sdk", "tts", "vs code extension"]
---

Nick Brady offers an in-depth roundup of Microsoft Foundry updates for Dec 2025 and Jan 2026. Developers gain insights into newly released AI models, agentic tools, SDK consolidation, and workflow enhancements across Azure Foundry.<!--excerpt_end-->

# What’s New in Microsoft Foundry: Dec 2025 & Jan 2026

**Author:** Nick Brady

Stay up-to-date with the latest releases and improvements in Microsoft Foundry, Microsoft's advanced platform for AI, agents, and multi-modal development on Azure. This edition highlights major model launches, SDK upgrades, and foundational platform changes for both Python and .NET/JavaScript developers working with large language models, agentic patterns, and enterprise AI.

## Major Model Releases

- **GPT-5.2 (GA):** Enhanced enterprise reasoning for math, science, coding, and multimodal tasks. Available as `gpt-5.2` and `gpt-5.2-chat-latest`.
- **GPT-5.1 Codex Max (GA):** 77.9% on SWE-Bench, supports 400K token context and 50+ languages. Targeted at autonomous coding pipelines and CI/CD automation.
- **Mistral Large 3 (Public Preview):** Open-weight, Apache 2.0 license, strong instruction following, multimodal support, and cost-effective token pricing.
- **DeepSeek V3.2 + V3.2‑Speciale (Public Preview):** 128K context window, up to 3× faster reasoning, Speciale variant is dedicated to pure reasoning.
- **Kimi-K2 Thinking:** Deep reasoning model by Moonshot AI with a 256K context, integrated directly from Azure.
- **Cohere Rerank 4:** Advanced cross-encoding reranker for RAG pipelines, supporting 100+ languages for improved semantic retrieval.
- **GPT-image-1.5 (GA):** Up to 4× faster image generation and 20% cost reduction, with inpainting and face preservation features.
- **FLUX.2 [pro] (Public Preview):** Next-gen image model with multi-reference image support and improved text rendering, hosted with enterprise SLAs on Azure.

## Audio Models

- **Realtime Mini, ASR, and TTS (GA):** New audio models with better accuracy and latency. All operate via API on Azure with improved multilingual and noise resilience features.

## Fine-Tuning Expansions

- New open-source models (Ministral 3B, Qwen3 32B, OSS-20B, Llama 3.3 70B) are available for serverless fine-tuning, all with Microsoft’s Responsible AI guardrails and security compliance.

## Agents and Tools

- **Memory in Foundry Agent Service:** Managed long-term memory store for agent sessions, automating extraction and consolidation of user preferences.
- **Agent-to-Agent (A2A) Tool:** Supports inter-agent communication with explicit authentication and standardized thread control. Quickly connect agents to endpoints using the A2A protocol.
- **Computer Use Tool (Preview):** Allows agentic UI testing and automation of desktop/browser environments using visual input, with .NET SDK support available now.

## Platform Enhancements

- **Foundry MCP Server:** Fully managed, cloud-hosted MCP endpoint for model ops, evaluations, and agent management, secured via Azure Entra ID.
- **VS Code Extension Update:** Multi-workflow visualization, agent playground, improved resource navigation, and autogenerated code samples for various agent types.
- **Unified Foundry Portal at ai.azure.com:** Consolidated tools and workflows across both classic and new generation agents, simplifying resource management and discovery.

## SDK and Language Updates

- **Python:** Release of `azure-ai-projects` 2.0.0b3 consolidates agents, inference, and memory management. Class and API renames for alignment with OpenAI and OpenTelemetry standards.
- **.NET:** `Azure.AI.Agents.Persistent` brings Computer Use support; `Azure.AI.Projects` adds OpenAI 2.8.0 compatibility.
- **JavaScript/TypeScript:** `@azure/ai-projects` v2 beta series introduces major class renames reflecting OpenAI standards and usability improvements.

## Migration & Deprecation Notices

- **AzureML SDK v1** reaches end of life on June 30, 2026. Migrate current pipelines to SDK v2 and corresponding CLI upgrades as soon as possible.

## Get Started

- [Microsoft Foundry Blog](https://devblogs.microsoft.com/foundry/whats-new-in-microsoft-foundry-dec-2025-jan-2026/)
- [Model Catalog](https://ai.azure.com/catalog/models)
- [AzureML SDK Migration Guide](https://learn.microsoft.com/en-us/azure/machine-learning/how-to-migrate-from-v1)

For more details, code samples, and changelogs, see the linked resources throughout this summary.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/whats-new-in-microsoft-foundry-dec-2025-jan-2026/)
