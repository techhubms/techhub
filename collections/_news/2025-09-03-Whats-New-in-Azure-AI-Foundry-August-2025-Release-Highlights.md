---
external_url: https://devblogs.microsoft.com/foundry/whats-new-in-azure-ai-foundry-august-2025/
title: 'What’s New in Azure AI Foundry: August 2025 Release Highlights'
author: Nick Brady
feed_name: Microsoft AI Foundry Blog
date: 2025-09-03 15:00:50 +00:00
tags:
- .NET SDK
- Agents
- Azure AI Foundry
- Azure OpenAI
- Azure SDKs
- Browser Automation
- CMK
- FLUX
- FLUX Models
- Foundry Local
- GPT 5
- GPT OSS
- Java SDK
- JavaScript SDK
- Mistral Document AI
- Model Deployment
- Model Router
- Observability
- OpenAI Gpt Oss
- Playwright Testing
- Provisioned Throughput
- Python SDK
- Region Availability
- Responses API
- REST API
- Serverless Inference
- Sora
- Sora API
- Tracing
- VibeVoice
section_names:
- ai
- azure
- coding
primary_section: ai
---
Nick Brady summarizes the August 2025 updates to Azure AI Foundry, highlighting the launch of the GPT-5 family, new agent tools like Browser Automation, updated AI/ML models, and significant enhancements to APIs, SDKs, and documentation for Microsoft-centric developers.<!--excerpt_end-->

# What’s New in Azure AI Foundry: August 2025 Release Highlights

## Overview

August 2025 features significant updates to Azure AI Foundry:

- **GPT-5 family** (including mini, nano, and chat variants) now available, supporting advanced reasoning and multimodal conversations.
- **Sora API** adds image-to-video and inpainting with new regional support.
- **Mistral Document AI**: serverless OCR model for structured document understanding.
- **FLUX Image Models**: Fast, robust multimodal and text-to-image models now deployable from Azure.
- **Browser Automation (Public Preview)**: Automate real browser tasks via agents, tightly integrated with Microsoft Playwright Testing Workspaces.
- **Model Router**: Intelligent dynamic switching among GPT-5 models for cost/performance optimization.
- **Responses API**: General Availability for unified, multi-turn, tool-integrated conversations.
- **SDKs**: New releases and previews for Python, .NET, Java, and JavaScript/TypeScript extend development flexibility.
- **Documentation**: Upgrades include guides on migration, observability, compliance (CMK), and disaster recovery.

---

## GPT-5 Family Now in Foundry

Azure AI Foundry now supports:

- `gpt-5`: Advanced, long-context (up to ~272K tokens), next-gen reasoning model; registration required.
- `gpt-5-mini`, `gpt-5-nano`, `gpt-5-chat`: No registration (mini and nano are optimized for speed and low latency; chat is multimodal, large-context).
- Available in East US 2 and Sweden Central at launch, with detailed region/model availability matrices.
- Tool-calling improvements: Freeform execution of Python, SQL, and config scripts from prompts, enabling new integration scenarios.
- **Model Router** enables dynamic model selection for pricing and quality balance.

## Model Updates: Sora, Mistral Document AI, FLUX, OpenAI gpt-oss, VibeVoice

- **Sora API**: Adds image-to-video support (including inpainting) and expands geographic availability.
- **Mistral Document AI**: High-fidelity, layout-aware document OCR with structured exports for downstream applications (e.g., RAG workflows).
- **FLUX.1 Kontext [pro]**: Robust text-to-image, multi-edit, faster inference at large resolutions; **FLUX1.1 [pro]** optimized for high-speed, large images.
- **OpenAI gpt-oss**: Open-weight models (20B/120B) run on Foundry Local and Windows AI Foundry, designed for local/offline or cloud inference.
- **VibeVoice**: Forthcoming open-source, long-form, multi-speaker conversational TTS models (1.5B, Large, and upcoming 0.5B Streaming).

## Agents and Automation

- **Browser Automation Tool**: In public preview, enables natural language-driven browser workflows—automates navigation, form filling, bookings, and profile updates.
- Deep Playwright Testing Workspace integration simplifies setup and increases automation resilience.
- Expanded Agent Service regional availability in Brazil South, Germany West Central, Italy North, South Central US (17 regions total).
- Sample code provided for integrating with both Python and Java SDKs.

## APIs and SDKs

- **Responses API** is now GA – supports stateful, multi-turn conversations and integrates with the latest OpenAI models, including GPT-5 series.
- Updates to Python, .NET, Java, and JavaScript/TypeScript SDKs:
  - Agent orchestration, code/evaluation improvements, new sample workflows.
  - Beta/stable release cadence improves adoption flexibility across different project maturity levels.
- Tools like File Search, Code Interpreter, and new support for Azure AI Search and Azure Functions in samples.

## Platform Features

- Infrastructure-as-Code (IaC) provisioning with Terraform.
- Advanced observability: distributed tracing/monitoring via OpenTelemetry.
- RBAC refinements, enforced compliance with customer-managed keys.
- Comprehensive region/model matrices for planning and compliance.

## Documentation & Guidance

Numerous new and updated resources for advanced users and teams:

- Cost management for fine-tuning jobs.
- Migrating from legacy hubs to Foundry Projects.
- Enhanced disaster recovery and managed network setup.
- Evaluations, simulators, and advanced guides on deploying and securing resources.

## Resources & Getting Started

- [Quickstart guides for each API/SDK](https://learn.microsoft.com/en-us/azure/ai-foundry/openai/how-to/reasoning)
- [Demo: Browser Automation](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/how-to/tools/browser-automation)
- [Python SDK Release Notes](https://azure.github.io/azure-sdk/releases/2025-08/python.html)
- [Foundry Status Dashboard](https://learn.microsoft.com/en-us/azure/ai-foundry/azure-ai-foundry-status-dashboard-documentation)

Stay up-to-date by subscribing to the ["What’s New in Foundry" RSS feed](https://devblogs.microsoft.com/foundry/category/whats-new/feed/).

---

**Author:** Nick Brady  
For comprehensive release details, reference the [official blog post](https://devblogs.microsoft.com/foundry/whats-new-in-azure-ai-foundry-august-2025/).

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/whats-new-in-azure-ai-foundry-august-2025/)
