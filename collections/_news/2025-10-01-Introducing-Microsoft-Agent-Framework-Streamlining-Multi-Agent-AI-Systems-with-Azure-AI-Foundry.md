---
layout: post
title: 'Introducing Microsoft Agent Framework: Streamlining Multi-Agent AI Systems with Azure AI Foundry'
author: stclarke
canonical_url: https://azure.microsoft.com/en-us/blog/introducing-microsoft-agent-framework/
viewing_mode: external
feed_name: Microsoft News
feed_url: https://news.microsoft.com/source/feed/
date: 2025-10-01 19:03:22 +00:00
permalink: /ai/news/Introducing-Microsoft-Agent-Framework-Streamlining-Multi-Agent-AI-Systems-with-Azure-AI-Foundry
tags:
- Agent2Agent
- AI
- AI Agents
- AutoGen
- Azure
- Azure AI Foundry
- Coding
- Company News
- Developer Tools
- Enterprise AI
- KPMG Clara
- MCP
- Microsoft Agent Framework
- Multi Agent Systems
- News
- Observability
- OpenAPI
- OpenTelemetry
- Responsible AI
- SDK
- Semantic Kernel
- Voice Live API
- VS Code Extension
- Workflow Automation
section_names:
- ai
- azure
- coding
---
stclarke introduces Microsoft Agent Framework and Azure AI Foundry enhancements, offering developers unified tools to build, orchestrate, and govern enterprise-grade multi-agent AI systems with integrated observability and responsible AI features.<!--excerpt_end-->

# Introducing Microsoft Agent Framework: Streamlining Multi-Agent AI Systems with Azure AI Foundry

**Author:** stclarke  
**Source:** [Microsoft Blog](https://azure.microsoft.com/en-us/blog/introducing-microsoft-agent-framework/)

## Overview

Microsoft has announced the public preview of the Microsoft Agent Framework, an open-source SDK and runtime that simplifies the orchestration of multi-agent AI systems. Alongside this release, Azure AI Foundry provides new capabilities that support developers with building, monitoring, and governing agentic solutions at enterprise scale.

## Key Features

### Unified Multi-Agent Platform

- **Microsoft Agent Framework** brings together research and ready-for-enterprise foundations (merging [AutoGen](https://github.com/microsoft/autogen) and [Semantic Kernel](https://github.com/microsoft/semantic-kernel)).
- Developers can experiment locally and deploy to Azure AI Foundry, benefiting from built-in observability, durability, and compliance.
- OpenAPI-based API integration, Agent2Agent (A2A) runtime collaboration, and Model Context Protocol (MCP) support for dynamic tool connections.
- Support for the latest multi-agent patterns, including Magentic One and orchestrated workflows.

### Developer Experience & Ecosystem

- Reduces context-switching and platform fragmentation.
- Open-source connectors facilitate interoperability between Azure AI Foundry, Microsoft 365 Copilot, and external systems.
- Strong focus on developer feedback and community contributions.

### Enterprise Applications & Adoption

- **KPMG Clara AI** uses the framework for audit automation, benefiting from enterprise-scale security and compliance.
- **Commerzbank** pilots avatar-driven AI support, emphasizing productivity and operational reliability.
- **Citrix, TCS, Sitecore, and Elastic** develop domain-specific solutions, from VDI productivity to content automation and enhanced search integrations.

### Multi-Agent Workflows (Private Preview)

- Foundry Agent Service introduces workflow orchestration, leveraging persistent state and context sharing for long-running tasks.
- Visual authoring and debugging via VS Code Extension or Azure AI Foundry.
- Supports complex enterprise scenarios such as customer onboarding and financial operations.

### Observability & Standards

- Contributions to [OpenTelemetry](https://aka.ms/MultiAgentTracingBlog) standardize tracing and monitoring for agentic systems across frameworks.
- Unified observability now available for agents built with Microsoft Agent Framework, LangChain, LangGraph, and OpenAI Agents SDK.

### Voice Live API Availability

- [Voice Live API](https://aka.ms/VoiceLiveGA) enables real-time, speech-to-speech agent solutions—integrating STT, TTS, avatars, and conversational features in a single API pipeline.
- Customers (Capgemini, healow, Astra Tech, Agora) leverage this for voice-enabled customer service, HR, and education applications.

### Responsible AI Features

- Public preview launches for:
  - **Task adherence** ([details](https://aka.ms/TaskAdherence)): Guiding agents to stay aligned with instructions.
  - **Prompt shields with spotlighting** ([details](https://aka.ms/spotlighting)): Detect and mitigate prompt injection risks.
  - **PII detection**: Identifying and managing sensitive data.
- Built-in to Azure AI Foundry to foster trustworthy and compliant systems.

## Customer Momentum & Use Cases

- Over 70,000 organizations (from startups to large enterprises) are realizing ROI from Azure AI Foundry and Agent Framework-powered agentic AI solutions.
- Testimonials and case studies from KPMG, Commerzbank, Citrix, TCS, Sitecore, and Elastic reflect the diversity and scalability of enterprise adoption.

## Conclusion

Microsoft Agent Framework and Azure AI Foundry represent a unified, open, and responsible approach to agentic AI development. Developers can leverage cutting-edge tools, enterprise-grade governance, and broad ecosystem integration to accelerate the creation of scalable, trustworthy multi-agent systems.

**Learn More:**  

- [Microsoft Agent Framework introduction](https://azure.microsoft.com/en-us/blog/introducing-microsoft-agent-framework/)  
- [Azure AI Foundry](https://azure.microsoft.com/en-us/products/ai-foundry)  
- [Get started with Microsoft Agent Framework](https://aka.ms/AgentFramework/PuPr)

**References:**

1. [PwC’s AI Agent Survey](https://www.pwc.com/us/en/tech-effect/ai-analytics/ai-agent-survey.html)  
2. [AI adoption is rising, but friction persists.](https://www.atlassian.com/blog/developer/developer-experience-report-2025)  
3. [Global AI Trust Maturity Survey](https://www.mckinsey.com/capabilities/mckinsey-digital/our-insights/tech-forward/insights-on-responsible-ai-from-the-global-ai-trust-maturity-survey)

This post appeared first on "Microsoft News". [Read the entire article here](https://azure.microsoft.com/en-us/blog/introducing-microsoft-agent-framework/)
