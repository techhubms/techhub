---
layout: "post"
title: "What's New in Microsoft Foundry: February 2026 Update"
description: "This post details the major February 2026 advancements in Microsoft Foundry, including new AI models (Claude Opus 4.6, Sonnet 4.6), audio APIs, Grok 4.0 general availability, the Microsoft Agent Framework (RC), durable agent orchestration with Azure Durable Functions, Foundry Local for sovereign deployments, REST API v1 (GA), and SDK updates across Python, .NET, JavaScript/TypeScript, and Java, along with significant documentation expansions."
author: "Nick Brady"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/foundry/whats-new-in-microsoft-foundry-feb-2026/"
viewing_mode: "external"
feed_name: "Microsoft AI Foundry Blog"
feed_url: "https://devblogs.microsoft.com/foundry/feed/"
date: 2026-03-06 23:08:31 +00:00
permalink: "/2026-03-06-Whats-New-in-Microsoft-Foundry-February-2026-Update.html"
categories: ["AI", "Azure", "Coding", "ML"]
tags: [".NET", "Agent Framework", "Agent Framework Azure AI", "Agent Framework Core", "Agents", "AI", "AI Toolkit For VS Code", "Anthropic", "Audio", "Azure", "Azure Identity", "Claude Opus 4.6", "Claude Sonnet 4.6", "Coding", "Durable Functions", "FLUX.2 Flex", "Foundry Local", "GPT Audio 1.5", "GPT Realtime 1.5", "Grok 4.0", "Grok 4.1 Fast", "Human in The Loop", "Java", "JavaScript", "Machine Learning", "MCP", "Microsoft Agent Framework", "Microsoft Foundry", "ML", "Model Router", "Models", "News", "Python", "REST API", "REST API V1", "SDK", "SignalR", "TypeScript"]
tags_normalized: ["dotnet", "agent framework", "agent framework azure ai", "agent framework core", "agents", "ai", "ai toolkit for vs code", "anthropic", "audio", "azure", "azure identity", "claude opus 4dot6", "claude sonnet 4dot6", "coding", "durable functions", "fluxdot2 flex", "foundry local", "gpt audio 1dot5", "gpt realtime 1dot5", "grok 4dot0", "grok 4dot1 fast", "human in the loop", "java", "javascript", "machine learning", "mcp", "microsoft agent framework", "microsoft foundry", "ml", "model router", "models", "news", "python", "rest api", "rest api v1", "sdk", "signalr", "typescript"]
---

Nick Brady rounds up February 2026's major Microsoft Foundry updates—including the arrival of frontier AI models, Agent Framework RC, REST API GA, SDK previews, and architectural innovations for developers and machine learning practitioners.<!--excerpt_end-->

# What's New in Microsoft Foundry: February 2026 Update

*By Nick Brady*

February 2026 introduces landmark features, new models, and developer tooling for the Microsoft Foundry platform. This digest highlights the most impactful updates and guides you to related resources for deep diving and migration.

## Major Announcements (TL;DR)

- **Claude Opus 4.6 & Sonnet 4.6 (Anthropic):** Arrive in Foundry with 1M-token context support (beta), adaptive reasoning, and context compaction. Opus targets deep reasoning, Sonnet optimizes for scale and cost.
- **GPT-Realtime-1.5 & GPT-Audio-1.5:** Next-generation audio models offer improved instruction following, multilingual capabilities, and higher transcription accuracy.
- **Grok 4.0 (GA) & Grok 4.1 Fast (Preview):** Reasoning and high-throughput models for diverse workloads.
- **FLUX.2 Flex:** Specialized text-heavy image generation for UI and typography.
- **Microsoft Agent Framework (RC):** v1.0.0rc1 for Python—API surface is locked, featuring breaking changes and a published migration guide.
- **Durable Agent Orchestration:** Combines Azure Durable Functions with the Agent Framework and SignalR for robust, human-in-the-loop AI agents.
- **Foundry Local—Sovereign Cloud:** Enables large, multimodal models to run on disconnected local hardware, mirroring the Foundry cloud API.
- **AI Toolkit for VS Code v0.30.0:** Streamlined agent development, insights, and debugging.
- **REST API v1 (GA):** Foundation REST endpoints are production-ready, underpinning all SDKs and new developments across languages.
- **SDK releases across Python, .NET, JS/TS, and Java:** Beta releases with consolidated class naming, credential management, and preview feature gates; all aligning on the GA REST API surface.

## Key Model Releases

### Anthropic Models: Claude Opus 4.6 & Sonnet 4.6

- Available in Microsoft Foundry as first-party deployments.
- **Opus 4.6:** Suited for end-to-end codebase reasoning, 1M-token context window, 128K output tokens, adaptive effort levels, and automatic context summarization.
- **Sonnet 4.6:** Offers similar intelligence with cost and throughput optimizations, perfect for scaling professional agentic workflows.
- Both deployable serverlessly or on managed Azure compute.

Resources: [Opus 4.6 announcement](https://azure.microsoft.com/en-us/blog/claude-opus-4-6-anthropics-powerful-model-for-coding-agents-and-enterprise-workflows-is-now-available-in-microsoft-foundry-on-azure/), [Sonnet 4.6 details](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/claude-sonnet-4-6-in-microsoft-foundry-frontier-performance-for-scale/4494873).

### GPT-Realtime-1.5 & GPT-Audio-1.5

- Upgrades for real-time audio and voice agent developers: improved reasoning, tool calling, and multilingual accuracy.
- Seamless API compatibility with legacy models.

Read more: [Model release blog](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/new-azure-open-ai-models-bring-fast-expressive-and-real%E2%80%91time-ai-experiences-in-m/4496184)

### xAI's Grok 4.0 & 4.1 Fast

- **Grok 4.0** is GA and fit for complex multi-step workflows.
- **Grok 4.1 Fast** is previewed for high-throughput, non-reasoning scenarios.

See: [Grok release and performance](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/grok-4-0-goes-ga-in-microsoft-foundry-and-grok-4-1-fast-arrives-with-major-enhan/4497964)

### FLUX.2 Flex

- Focused on accurate text generation in images—targeting UI prototyping and infographics.

See: [FLUX.2 Flex overview](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/meet-flux-2-flex-for-text%E2%80%91heavy-design-and-ui-prototyping-now-available-on-micro/4496041)

### Model Router and GPT-5 Series Support

- The Foundry Model Router now supports GPT-5 family models, handling chat endpoint routing automatically.

Read: [Model Router Documentation](https://learn.microsoft.com/en-us/azure/foundry/openai/concepts/model-router?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-feb-2026&utm_content=model-router)

## Agents and Orchestration

### Microsoft Agent Framework 1.0.0rc1

- RC milestone introduces a locked API for Python with significant changes: unified credential patterns (Azure Identity), session-based interactions (replacing threads), and endpoint renames for workflows and responses.
- Supports hosting on Azure Functions, multi-agent orchestration, and now includes first-class Claude/GitHub Copilot providers.

Technical action: Pin both `agent-framework-core==1.0.0rc1` and `agent-framework-azure-ai==1.0.0rc1`, then review the [migration guide](https://learn.microsoft.com/en-us/agent-framework/support/upgrade/python-2026-significant-changes?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-feb-2026&utm_content=agent-framework-migration).

### Durable Agent Orchestration

- Pattern leverages Azure Durable Functions and SignalR so AI agents can pause for human input and resume after days or restarts. Useful for incident response and document reviews.

Read more: [Durable Agent Orchestration guide](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/building-ai-agents-that-wait-for-humans/4496310)

## Platform Evolution

### Foundry Local

- Adds support for disconnected, sovereign deployments—large multimodal models are now possible entirely on-premises, ideal for sectors with strict data governance policies.

Announcement: [Sovereign Cloud expansion](https://blogs.microsoft.com/blog/2026/02/24/microsoft-sovereign-cloud-adds-governance-productivity-and-support-for-large-ai-models-securely-running-even-when-completely-disconnected/)

### AI Toolkit for VS Code v0.30.0

- Major upgrades improve agent developer workflows: tool and model catalogs, agent inspector with step debugging, "Build Agent with Copilot" integration for rapid prototyping.

Update post: [VS Code Toolkit](https://techcommunity.microsoft.com/blog/azuredevcommunityblog/%F0%9F%9A%80-ai-toolkit-for-vs-code-%E2%80%94-february-2026-update/4493673)

### REST API v1 General Availability

- Core Foundry REST endpoints are now GA with production-ready stability. SDKs are previewing but all build on this locked API surface—making this a critical foundation for developers.

Docs: [API lifecycle](https://learn.microsoft.com/en-us/azure/foundry/openai/api-version-lifecycle?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-feb-2026&utm_content=rest-api-v1-ga), [API reference](https://learn.microsoft.com/en-us/azure/foundry/openai/reference?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-feb-2026&utm_content=rest-api-reference)

## Language SDK Updates

- **Python, .NET, JavaScript/TypeScript, and Java SDKs** shipped new beta releases with major breaking changes, all converging on REST API v1 as the foundation.
- Naming conventions, preview APIs, credential formats, and tracing patterns have shifted (see content for full before/after code samples and upgrade actions).
- Expected SDK GAs in March 2026.
- See changelogs: [Python](https://pypi.org/project/azure-ai-projects/2.0.0b4/), [C#](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/ai/Azure.AI.Projects/CHANGELOG.md), [JS/TS](https://github.com/Azure/azure-sdk-for-js/tree/@azure/ai-projects_2.0.0-beta.4/sdk/ai/ai-projects/CHANGELOG.md), [Java](https://github.com/Azure/azure-sdk-for-java/blob/azure-ai-projects_2.0.0-beta.1/sdk/ai/azure-ai-projects/CHANGELOG.md)

## Documentation and Migration

- Over 100 new and updated articles released on agents, model context, architecture, agent-to-agent workflows, fine-tuning vision models, and platform guardrails. Highlights include:
  - [Model Context Protocol (MCP)](https://learn.microsoft.com/azure/ai-foundry/agents/how-to/tools/model-context-protocol)
  - [Agent development lifecycle](https://learn.microsoft.com/azure/ai-foundry/agents/concepts/development-lifecycle)
  - [Publishing agents to Copilot](https://learn.microsoft.com/azure/ai-foundry/agents/how-to/publish-copilot)
  - [Realtime Audio API](https://learn.microsoft.com/azure/ai-foundry/ai-services/openai/how-to/realtime-audio)
  - [Content filter prompt shields](https://learn.microsoft.com/azure/ai-foundry/ai-services/openai/how-to/content-filter-prompt-shields)
- Full list of articles and updates provided in main content.

## Next Steps

- Upgrade SDKs to the latest beta releases and start targeting REST API v1 for long-term platform stability.
- Review migration guides due to major breaking changes in APIs, class names, and credential handling.
- Engage with the Foundry community on [Discord](https://aka.ms/foundry/discord) and [GitHub Discussions](https://aka.ms/foundry/forum).
- Follow for upcoming SDK GAs and further platform news.

---

Stay at the front of Microsoft’s AI, agent, and orchestration innovations with these February 2026 updates. For details or troubleshooting, see the included technical docs and migration guides.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/whats-new-in-microsoft-foundry-feb-2026/)
