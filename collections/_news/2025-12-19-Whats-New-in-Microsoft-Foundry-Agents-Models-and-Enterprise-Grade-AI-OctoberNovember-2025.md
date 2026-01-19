---
layout: post
title: "What's New in Microsoft Foundry: Agents, Models, and Enterprise-Grade AI (October–November 2025)"
author: Jenn Cockrell
canonical_url: https://devblogs.microsoft.com/foundry/whats-new-in-microsoft-foundry-oct-nov-2025/
viewing_mode: external
feed_name: Microsoft AI Foundry Blog
feed_url: https://devblogs.microsoft.com/foundry/feed/
date: 2025-12-19 04:55:08 +00:00
permalink: /ai/news/Whats-New-in-Microsoft-Foundry-Agents-Models-and-Enterprise-Grade-AI-OctoberNovember-2025
tags:
- AI Agents
- AI Tools
- Anthropic
- Anthropic Claude
- Azure AI Search
- BYO Models
- Enterprise AI
- Foundry Agent Framework
- Foundry Tools
- Guardrails
- Hosted Agents
- Identity And Governance
- MCP Integration
- Microsoft Foundry
- Model Fine Tuning
- Multi Agent System
- Observability
- OpenAI Sora
- Persistent Memory
- Reinforcement Learning
- Sora
- VNET
- Zero Trust
section_names:
- ai
- azure
- coding
- ml
- security
---
Jenn Cockrell outlines the latest Microsoft Foundry enhancements, introducing powerful agent frameworks, model support, developer tools, and enterprise security features—all designed to accelerate AI innovation in the enterprise.<!--excerpt_end-->

# What's New in Microsoft Foundry: October–November 2025

_Author: Jenn Cockrell_

Microsoft Foundry (formerly Azure AI Foundry) is rolling out sweeping updates that boost agentic AI capabilities, speed up innovation, and integrate enterprise security. This news roundup includes major platform evolutions, new models, advanced agent frameworks, granular controls, and more.

## Highlights

- **Move prototypes to production quickly:** Leverage the new Microsoft Agent Framework and Hosted Agents to rapidly build, test, and deploy multi-agent AI systems—no containers, no Kubernetes required.
- **Model and deployment flexibility:** Model Router and BYO Model Gateway unify thousands of models (Anthropic Claude, OpenAI GPT, Mistral, and custom) under centralized, compliant governance with zero code changes.
- **Enterprise-grade management:** Control Plane, guardrails, and security features offer deep observability, compliance, and lifecycle controls for agent fleets.
- **Direct integrations:** One-click deployment to Teams and Microsoft 365 lowers the barrier for AI agent adoption.
- **Development and data science acceleration:** Redesigned UI, reinforcement fine-tuning for GPT-5, and parity-building for third-party models like Mistral foster custom model innovation.
- **Edge support:** Foundry Local now brings advanced, on-device AI to Windows, Mac, and Android.

## 1. Agent Framework & Hosted Agents

### Microsoft Agent Framework

- Open-source SDK and runtime for building scalable multi-agent AI systems.
- Combines reliability from Semantic Kernel with orchestration from AutoGen.
- Features: Cross-cloud connectors (Azure, AWS, GCP), observability, identity, governance, autoscaling, and no complex infrastructure.

[Download SDK](https://aka.ms/AgentFramework) | [Documentation](https://aka.ms/AgentFramework/Docs)

### Hosted Agents and Foundry Agent Service

- Deploy custom agents (built on Microsoft Agent Framework, CrewAI, LangGraph, etc.) to a managed runtime—no container images or Kubernetes needed.
- Provides role-based access, telemetry, audit trails, and lifecycle management.
- Interoperates with multi-agent orchestration, persistent memory, and Microsoft 365 integration.

[Foundry Agent Service @ Ignite](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/foundry-agent-service-at-ignite-2025-simple-to-build-powerful-to-deploy-trusted-/4469788)

### Bring Your Own Models (BYO Model Gateway)

- Connects enterprise-managed AI models via Azure API Management, Mulesoft, Kong, etc.
- Supports pre/post-LLM hooks, automated failover, multi-region strategies, and enforced governance/compliance.

[Learn More](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/how-to/ai-gateway?view=foundry)

### Built-In Memory

- Long-term, persistent memory lets AI agents remember context, preferences, and task results reliably across sessions.

## 2. Anthropic Claude, OpenAI Sora 2, and Model Router

- **Anthropic Claude (Haiku 4.5, Sonnet 4.5, Opus 4.1):** Now accessible alongside GPT models, expanding model flexibility and scenario coverage.
- **Sora 2 API:** Enables generative video and audio creation, with robust performance and compliance.
- **Model Router (GA):** Automatically directs prompts to the most efficient and context-appropriate LLM; reduces operational cost and complexity. Supports integrations and future-scale upgrades.

[Anthropic's Claude Announcement](https://azure.microsoft.com/en-us/blog/introducing-anthropics-claude-models-in-microsoft-foundry-bringing-frontier-intelligence-to-azure/) | [Sora 2 API](https://azure.microsoft.com/en-us/blog/sora-2-now-available-in-azure-ai-foundry/) | [Model Router Documentation](https://learn.microsoft.com/en-us/azure/ai-foundry/foundry-models/whats-new-model-router?view=foundry-classic)

## 3. Fine-Tuning, RFT, and Unified Workflows

- Redesigned user experience focuses on agents, integrates with VS Code, and supports model creation/evaluation/deployment end-to-end.
- **Reinforcement Fine-Tuning (RFT) for GPT-5** and fine-tuning for non-OpenAI models like Mistral expand customization and business use-case coverage.
- Unified APIs and tiered pricing (developer, production) improve cost efficiency and iteration speed for data scientists and engineers.

## 4. Foundry Tools and Foundry IQ

- **Foundry Tools:** Suite of rebranded, prebuilt AI services (vision, speech, text, audio, documents) now offers bring-your-own-model, live interpreter (as in Teams), LLM Speech, and Photo Avatar (VASA-1 tech).
- **Foundry IQ:** Knowledge system, powered by Azure AI Search, for unifying enterprise knowledge access—connects SharePoint, Fabric OneLake, and web; governed by Microsoft Purview and Entra ID.

[Foundry Tools @ Ignite](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/build-smarter-connected-agentic-apps-with-foundry-tools/4470763) | [Foundry IQ Announcement](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/foundry-iq-unlocking-ubiquitous-knowledge-for-agents/4470812)

## 5. Platform Security, Control, and Observability

- **Foundry Control Plane:** Centralized dashboard for AI security, cost, governance, and agent lifecycle. Anchored by Entra Agent ID for adherence to enterprise and Zero Trust policies.
- **Observability:** Cloud-based monitoring, OpenTelemetry tracing, visual cluster analysis, synthetic datasets, AI Red Teaming for safety and adversarial testing.
- **Network and Integration Security:** Managed VNET support, granular RBAC, secure model/tool access, BYO Key Vault, and private endpoints for regulated industries.
- **Cost Management:** Azure Cost Management integration enables tagging, resource tracking, budgeting, and automated cost controls.

[Control Plane Docs](https://learn.microsoft.com/en-us/azure/ai-foundry/control-plane/overview?view=foundry)

## 6. Developer and Edge Experience

- **One-click publish:** Instantly deploy agents to Microsoft Teams, Microsoft 365, and even non-Microsoft channels.
- **AI Templates:** Ready-to-use blueprints for voice bots, SharePoint integrations, and more.
- **Developer Training Tier:** Low-cost model training with spot capacity, ideal for experimentation and MVPs.
- **Foundry Local on Edge:** Whisper model, Windows, Mac, Android support; runs AI locally (privacy-first) with redesigned SDK for chat, audio, and hardware acceleration.

[Foundry Local Docs](http://aka.ms/foundry-local-docs) | [Foundry Local @ Ignite](https://devblogs.microsoft.com/foundry/foundry-local-comes-to-android/)

## 7. Enhanced Governance and Guardrails

- **Customizable guardrails:** Intelligent filtering and mitigation for prompt injection, adherence, harms, and output control at the agent level.
- **Risk management:** Flexible configuration, intervention points (prompts, outputs, tool calls/responses), and robust incident response.

## 8. Community and Future

- New Discord, GitHub forums, and regular community sessions (Model Mondays, Agent AMAs) to keep practitioners engaged.
- Rapid update cadence and clear developer focus underscore Microsoft’s commitment to modular, enterprise-ready AI.

[Ignite 2025 Recap](https://azure.microsoft.com/en-us/blog/actioning-agentic-ai-5-ways-to-build-with-news-from-microsoft-ignite-2025/)

---

For the full breakdown and links to each feature, review the official Microsoft Foundry docs and blogs provided above. Happy building!

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/whats-new-in-microsoft-foundry-oct-nov-2025/)
