---
layout: "post"
title: "What’s New in Azure AI Foundry: September 2025 Feature Roundup"
description: "This in-depth news post by Nick Brady covers the September 2025 updates to Azure AI Foundry, including the general availability of GPT‑5‑Codex, new Sora video-to-video capabilities, Grok 4 Fast models, enhanced browser automation tools, major updates to Azure AI Search, secure Key Vault integrations, Voice Live improvements, SDK changelogs across Python, .NET, Java, and JavaScript/TypeScript, and extensive documentation changes for agents, evaluation, and security features."
author: "Nick Brady"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/foundry/whats-new-in-azure-ai-foundry-september-2025/"
viewing_mode: "external"
feed_name: "Microsoft AI Foundry Blog"
feed_url: "https://devblogs.microsoft.com/foundry/feed/"
date: 2025-10-08 00:58:44 +00:00
permalink: "/2025-10-08-Whats-New-in-Azure-AI-Foundry-September-2025-Feature-Roundup.html"
categories: ["AI", "Azure", "Coding", "DevOps", "ML", "Security"]
tags: [".NET SDK", "AI", "AI Agents", "Avatar 4K", "Azure", "Azure AI Evaluation", "Azure AI Foundry", "Azure AI Search", "BCDR", "Browser Automation", "Coding", "Data Services", "DevOps", "Evaluations", "Foundry Local", "Function Calling", "Gpt 5 Codex", "Grok 4 Fast", "Identity SDK", "Java SDK", "JavaScript SDK", "Key Vault", "Knowledge Sources", "Microsoft Agent Framework", "ML", "Model ORchestration", "News", "O4 Mini", "OpenAI", "Python SDK", "Red Teaming", "Reinforcement Fine Tuning", "SDK Changelog", "Security", "Sora", "Speech To Speech", "Structured Output", "Telemetry", "Translator API", "Voice Live", "Voicelive"]
tags_normalized: ["dotnet sdk", "ai", "ai agents", "avatar 4k", "azure", "azure ai evaluation", "azure ai foundry", "azure ai search", "bcdr", "browser automation", "coding", "data services", "devops", "evaluations", "foundry local", "function calling", "gpt 5 codex", "grok 4 fast", "identity sdk", "java sdk", "javascript sdk", "key vault", "knowledge sources", "microsoft agent framework", "ml", "model orchestration", "news", "o4 mini", "openai", "python sdk", "red teaming", "reinforcement fine tuning", "sdk changelog", "security", "sora", "speech to speech", "structured output", "telemetry", "translator api", "voice live", "voicelive"]
---

Nick Brady summarizes the latest Azure AI Foundry updates for September 2025, highlighting sweeping improvements in models, agents, security, search, developer tools, and SDKs.<!--excerpt_end-->

# What’s New in Azure AI Foundry | September 2025

**Author:** Nick Brady

The September 2025 Azure AI Foundry update delivers significant model, tooling, and developer experience enhancements. Here’s a technical walkthrough of key changes, including major GA announcements, preview features, SDK updates, and new documentation resources.

## 🧠 Models

- **GPT-5-Codex (GA):** Multimodal code reasoning and advanced repo intelligence directly in Azure AI Foundry. Enables deep architecture refactor suggestions, staged migration flows, and performance/security code review. [Learn more](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/announcing-gpt%E2%80%915%E2%80%91codex-redefining-developer-experience-in-azure-ai-foundry/4455524)
- **Sora (Preview):** Video-to-video model accepts input clips and natural language instructions, returning temporally coherent, styled output. Abrupt content, high-res and sensitive scenarios capped during preview. [Quickstart](https://learn.microsoft.com/en-us/azure/ai-foundry/openai/video-generation-quickstart)
- **gpt-realtime (GA):** Unified speech-to-speech model collapses prior audio reasoning pipelines for ultra-low-latency, multimodal dialog and code collaboration.
- **Grok 4 Fast models (Preview):** Parallel function calling, long context support (131K tokens), and structured output schema for agentic orchestration. Designed for speed and deterministic routing.
- **o4-mini RFT (GA):** Reward-driven optimization using explicit grading logic, now in production (no longer only SFT required). [Announcement](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/o4-mini-reinforcement-fine-tuning-rft-now-generally-available-on-azure-ai-foundr/4452597)
- **Foundry Local v0.7:** Expanded NPU support on Windows and dynamic execution provider detection, enabling seamless hybrid local-cloud apps.
- **Avatar 4K + Emotion:** High-fidelity avatar rendering and synchronized emotion control for accessible, expressive agent applications.

## 🤖 Agents & Tools

- **Browser Automation / Computer Use (Preview):** Resilient DOM automation and new pixel-level UI workflows. Critical for legacy app automation where semantic access is limited.
- **Microsoft Agent Framework (OSS):** Open-source orchestration for agents, offering Semantic Kernel durability, A2A messaging, YAML/JSON agent specs, OpenTelemetry, and enterprise-ready hooks. [Explore the framework](https://aka.ms/AgentFramework)
- **Evaluation & Safety:** `azure-ai-evaluation` 1.11.0 adds structured experiment tags, groundedness checks, enhanced red teaming, and multilingual categories for better model safety testing.
- **Azure AI Search—Knowledge Sources:** Unified abstraction over content indexes and blob sources with integrated vectorization and answer synthesis, improving multi-source retrieval and latency transparency.

## 🔐 Platform & Security

- **Key Vault Connections:** Direct native secret management (no more inline secrets) for improved data security and rotational best practices.
- **Identity SDKs:** Across major languages, now enforce environmental variables and clarify claims challenges, reducing misconfiguration and onboarding friction.

## 🌐 Language & Data Services

- **Translator API (Preview):** Seamlessly brings neural translation to agent and multimodal workflows, supporting real-time chat, batch translation, terminology compliance, and scalable localization.
- **Text Analytics 6.0.0b1:** Next-gen analytics surface with expanded data flows.

## 💻 SDK and Language Updates

Includes extensive highlights for:

- **Python:** New agent tools, evaluation and search enhancements, identity improvements, Cosmos DB optimizations.
- **.NET:** AOT compatibility, agent tool streaming support, knowledge source refactor in Search, improved identity options, storage and Synapse overhauls.
- **Java:** Search knowledge sources, Cosmos throughput and encryption, call automation features, and unified core dependency upgrades.
- **JavaScript/TypeScript:** OpenTelemetry updates, improved communication automation, Cosmos reliability, Playwright package migration, and TS HTTP runtime enhancements.

## 📝 Documentation Updates

- **New Guides:** Announcements, how-tos, security, deployment, orchestration, region expansion, cost management, retirement timelines, and privacy notes.
- **Updated Docs:** Major rewrite for models, Sora, Grok, RFT, function calling, RBAC/Entra, network security, and more.
- **Platform Changes:** Unified cost and usage dashboards, region and quota expansions, new playgrounds, SDK migration notes, and BCDR best practices for agents.

## 📣 Join the Community

Engage with Azure AI Foundry on [Discord](https://aka.ms/azureaifoundry/discord), contribute on [GitHub Discussions](https://aka.ms/azureaifoundry/forum), or join the next Model Mondays or Agent AMA event!

---
This comprehensive update ensures developers and platform architects working with Azure AI Foundry are equipped with the latest capabilities, best practices, and code-to-production insights.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/whats-new-in-azure-ai-foundry-september-2025/)
