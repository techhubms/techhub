---
layout: "post"
title: "What’s New in Azure AI Foundry: June 2025 Major Model Releases, Agent Service GA, and Developer Tools"
description: "This update from Nick Brady covers the June 2025 enhancements in Azure AI Foundry, including new AI models, Agent Service GA with MCP support, improved developer tools, unified SDK, and platform features like Model Safety Leaderboards, VS Code extension upgrades, GitHub and Logic Apps integrations, and expanded agent workflows."
author: "Nick Brady"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/foundry/whats-new-in-azure-ai-foundry-june-2025/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/foundry/feed/"
date: 2025-06-30 15:00:36 +00:00
permalink: "/2025-06-30-Whats-New-in-Azure-AI-Foundry-June-2025-Major-Model-Releases-Agent-Service-GA-and-Developer-Tools.html"
categories: ["AI", "Azure", "Coding", "DevOps"]
tags: ["Agent Service", "AI", "AI Development", "AutoGen", "Azure", "Azure AI Foundry", "Azure AI Search", "Azure OpenAI", "Coding", "DeepSeek R1", "DevOps", "Fine Tuning", "Generative AI", "GitHub Integration", "Healthcare AI", "Logic Apps", "MCP", "Model Context Protocol", "Model Safety", "News", "O3 Pro", "SDK", "Semantic Kernel", "SharePoint Integration", "Sora", "Voice Live API", "VS Code Extension"]
tags_normalized: ["agent service", "ai", "ai development", "autogen", "azure", "azure ai foundry", "azure ai search", "azure openai", "coding", "deepseek r1", "devops", "fine tuning", "generative ai", "github integration", "healthcare ai", "logic apps", "mcp", "model context protocol", "model safety", "news", "o3 pro", "sdk", "semantic kernel", "sharepoint integration", "sora", "voice live api", "vs code extension"]
---

Authored by Nick Brady, this comprehensive news post details the latest advancements in Azure AI Foundry as of June 2025, highlighting new models, developer tooling, and platform updates designed to streamline enterprise AI development.<!--excerpt_end-->

# What’s New in Azure AI Foundry | June 2025

*By Nick Brady*

## Excerpt

Authored by Nick Brady, this comprehensive news post details the latest advancements in Azure AI Foundry as of June 2025, highlighting new models, developer tooling, and platform updates designed to streamline enterprise AI development.

---

## TL;DR

- **Agent Service GA**: Now supports Model Context Protocol (MCP) for integrating any backend with standardized JSON-RPC calls, removing the need for custom OpenAPI specs.
- **New Models**: o3-pro (enterprise reasoning), Sora (video generation), DeepSeek-R1 (open-source reasoning), codex-mini (coding assistant), Grok 3 and Grok 3 Mini.
- **Platform Changes**: Unified Azure AI Foundry resource type, projects for self-service, simplified API versioning, unified SDK, enhanced VS Code extension.
- **API Updates**: Voice Live API (real-time speech), enhanced Responses API with tool-calling and image generation, SDK add-ins (Bing Custom Search, SharePoint, Microsoft Fabric tools).
- **Model Safety**: New leaderboards, promoting safety as first-class criterion.
- **Community**: Access to Discord, GitHub, live events, open-source courses, and code samples.

---

## Developer Community

Join over 25,000 developers in the Azure AI Foundry Developer Community:

- Connect via Discord and GitHub Discussions.
- Attend live events (e.g., AMAs on Model Context Protocol, .NET + AI office hours).
- Access free open-source AI learning resources and code samples.

[Join the Community](https://aka.ms/foundrydevs?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=developer-community)

---

## Models

### o3-pro Enterprise Reasoning

- Advanced reasoning, multi-modal input, structured outputs, tool integration.
- Designed for precision and depth in complex organizational tasks.

[Read More](https://devblogs.microsoft.com/foundry/azure-openai-o3-pro-ai-foundry/?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=o3-pro)

### Sora Video Generation

- Public preview: Text-to-video generation (20s, 1080p, various aspect ratios).
- Image-to-video support coming soon.
- Use cases: marketing, education, creative workflows.

[Quickstart](https://learn.microsoft.com/azure/ai-services/openai/video-generation-quickstart?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=sora-video)

### codex-mini Lightweight Coding Assistant

- Lightweight, efficient code generation/completion.
- Suited for embedded/devices where performance is key.

[Quickstart](https://learn.microsoft.com/azure/ai-services/openai/whats-new?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=codex-mini)

### Grok 3 & Grok 3 Mini

- xAI models for real-time conversational AI and general reasoning.

[More Info](https://devblogs.microsoft.com/foundry/announcing-grok-3-and-grok-3-mini-on-azure-ai-foundry/?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=grok-3)

### DeepSeek-R1-0528

- Open-source model for deeper reasoning and inference, supports chain-of-thought prompting, reinforcement learning fine-tuning, and multi-language.

[Model Details](https://ai.azure.com/explore/models/DeepSeek-R1/version/1/registry/azureml-deepseek?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=deepseek-r1)

---

## Agent Capabilities

### Model Context Protocol (MCP) Support (Preview)

- Import capabilities automatically from any compliant MCP server.
- Reduce need for custom specs/integrations, with enterprise security built-in.

[Learn More](https://devblogs.microsoft.com/foundry/announcing-model-context-protocol-support-preview-in-azure-ai-foundry-agent-service/?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=mcp-support)

### SharePoint Integration

- Secure agent access to private documents; uses Microsoft 365 Copilot API.
- Identity passthrough for accurate access control.

[How-To](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/how-to/tools/sharepoint?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=sharepoint-integration)

### Multi-Agent Workflows

- Orchestrate multiple agents for complex business processes.

[Getting Started](https://aka.ms/Build25/Multi-Agent_Workflows?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=multi-agent-workflows)

### Semantic Kernel & AutoGen Unification

- Semantic Kernel and AutoGen merged into a single, composable agentic AI toolkit.
- Enterprise-ready, with advanced protocols (MCP, agent-to-agent, etc.).

[Details](https://devblogs.microsoft.com/foundry/semantic-kernel-commitment-ai-innovation/?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=semantic-kernel-autogen)

### Agent Catalog & Healthcare Orchestration

- Centralized code samples for common use-cases (translation, sales, customer service).
- Healthcare agents streamline clinician workflows.

[More Info](https://learn.microsoft.com/azure/ai-foundry/agents/whats-new?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=agent-catalog)

---

## Tools

### Enhanced VS Code Extension

- Full agent CRUD, YAML IntelliSense, deployment directly from VS Code.

[Extension](https://marketplace.visualstudio.com/items?itemName=TeamsDevApp.vscode-ai-foundry&utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=vscode-extension)

### Agent Debugging (Trace Agents)

- Visualize and monitor agent threads and behaviors for every run.

[Start Debugging](https://learn.microsoft.com/azure/ai-foundry/agents/whats-new?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=trace-debugging)

### GitHub Integration

- Streamlined development/deployment workflows inside GitHub repositories.

[Integration Details](https://cloudwars.com/ai/microsoft-accelerates-ai-development-with-new-azure-ai-foundry-github-services-at-build-2025/?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=github-integration)

### Logic Apps Triggers

- Invoke agents with Logic Apps for event-driven automation (e.g., new email/customer ticket).

[How-To](https://learn.microsoft.com/azure/ai-foundry/agents/whats-new?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=logic-apps-triggers)

### Expanded Agent Tools

- Bing Custom Search for agents, Morningstar for investment research, multi-industry expansions.

### Azure AI Search Enhancements

- Multi-vector, semantic ranking, improved multimodal and long-document support.

[Details](https://learn.microsoft.com/azure/search/vector-search-multi-vector-fields?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=ai-search-enhancements)

### Fine-Tuning for o4-mini

- Reinforcement learning fine-tuning for model customization and efficiency.

---

## Platform Updates

### Azure AI Foundry Resource & Projects

- Unified management of models, agents, tools with enterprise controls.
- Self-service project environments, tracing/monitoring, onboarding wizards.

[More Info](https://techcommunity.microsoft.com/blog/aiplatformblog/build-recap-new-azure-ai-foundry-resource-developer-apis-and-tools/4427241?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=foundry-resource-projects)

### API Versioning Simplified

- Use `latest` or `preview` for API endpoints—removes constant version updates and simplifies switching between OpenAI and Azure OpenAI.

[Docs](https://learn.microsoft.com/en-us/azure/ai-services/openai/api-version-lifecycle?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=api-version-lifecycle)

### Unified SDK

- Single SDK covers models, agent service, evaluations, tracing, and external connections.
- New tools: Bing Custom Search, SharePoint, Microsoft Fabric, and more.

[SDK Overview](https://learn.microsoft.com/en-us/azure/ai-foundry/how-to/develop/sdk-overview?pivots=programming-language-python#unified-projects-client-library?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=unified-sdk)

### Global Fine-Tuning Training

- Distributed, multi-region data support improves fine-tuning performance and lowers cost across global organizations.

### Voice Live API (Public Beta)

- Low-latency voice/speech-to-speech (WebSocket), noise suppression, echo cancellation, and function calling.

[Quickstart](https://learn.microsoft.com/azure/ai-services/speech-service/voice-live?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=voice-live-api)

### Model Safety Leaderboards

- Dedicated leaderboards using HarmBench to assess safety across multiple categories.
- Quality/safety trade-offs visualized—for use in regulated industries.

[Safety Leaderboards](https://learn.microsoft.com/en-us/azure/ai-foundry/concepts/model-benchmarks#safety-benchmarks-of-language-models?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=safety-leaderboards)

### Enhanced Responses API

- New MCP tool support, image generation, background task execution, enhanced reasoning controls, and role hierarchy for better agent deployments.

[Details](https://devblogs.microsoft.com/foundry/introducing-new-tools-and-features-in-the-responses-api-in-azure-ai-foundry/?utm_source=devblog&utm_medium=blog&utm_campaign=whats-new-june-2025&utm_content=responses-api)

---

## Summary

These Azure AI Foundry June 2025 updates mark significant advancements in enterprise AI development, model safety, multi-agent orchestration, developer tooling, and platform unification—enabling organizations to move from prototype to production with greater speed, safety, and efficiency.

Happy building, and share your projects with #AzureAIFoundry!

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/foundry/whats-new-in-azure-ai-foundry-june-2025/)
