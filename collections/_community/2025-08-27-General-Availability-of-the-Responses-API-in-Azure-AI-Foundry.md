---
layout: post
title: General Availability of the Responses API in Azure AI Foundry
author: BalaPV
canonical_url: https://techcommunity.microsoft.com/t5/ai-azure-ai-services-blog/the-responses-api-in-azure-ai-foundry-is-now-generally-available/ba-p/4446567
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=AI
date: 2025-08-27 19:00:00 +00:00
permalink: /ai/community/General-Availability-of-the-Responses-API-in-Azure-AI-Foundry
tags:
- Agent Development
- AgentOps
- AI
- API Orchestration
- AutoGen
- Azure
- Azure AI Foundry
- Azure OpenAI Service
- Code Interpreter
- Community
- Enterprise Automation
- Function Calling
- GPT 5
- Microsoft Azure
- Multi Turn Conversations
- OpenAI
- Production Ready API
- Responses API
- Semantic Kernel
- Stateful Agents
- Tool Integration
section_names:
- ai
- azure
---
BalaPV announces the general availability of the Responses API in Azure AI Foundry, empowering developers to easily build intelligent, multi-turn agents with built-in tool support and robust integration features.<!--excerpt_end-->

# General Availability of the Responses API in Azure AI Foundry

**Author:** BalaPV

## Overview

Microsoft has announced the general availability (GA) of the Responses API in Azure AI Foundry. This API simplifies the process for developers to create intelligent, tool-using AI agents capable of stateful, multi-turn conversations—all through a single API call.

Since its initial public preview, customers have leveraged this API to build agents that perform advanced reasoning, call various tools, and generate structured outputs without the need for elaborate orchestration code. With GA, the API is production-ready and supports all mainstream OpenAI models, including the advanced GPT-5 series and fine-tuned variants.

---

## Key Features and Benefits

- **Single-Call, Stateful Conversations:** Developers can design agents that automatically maintain conversation state and context across multiple turns, eliminating manual context management.
- **Tool-Oriented Workflows:** The API allows agents to call multiple tools seamlessly within a flow, integrating tool usage, model reasoning, and structured outputs.
- **Broad Model Support:** Full compatibility with Azure OpenAI models, covering rapid prototyping and domain-specific deployments with fine-tuned models.
- **Enterprise-Readiness:** Built on Azure's security, identity, and compliance foundations for scalable, production-grade deployments.
- **Integrated Tools Library:** Use foundation tools such as File Search, Function Calling (with JSON schema), Code Interpreter, Computer Use, Image Generation, and Remote MCP Server—all tightly coupled for natural decision-making by the agent.

---

## Adoption and Real-World Examples

- **UiPath:** Uses the Responses API to power enterprise automation agents capable of interpreting instructions and automating actions across SaaS, documents, and legacy software.
- **Other Industries:** Financial, healthcare, and retail organizations employ the API for copilots in research, compliance, document analysis, and task automation.
- **Common Scenarios:** Automating customer service workflows, enabling complex document analysis, and accelerating application development by reducing orchestration overhead.

---

## How It Works

The Responses API is built for agentic workflows, abstracting away infrastructure so developers can focus on agent design:

- Retains turn-by-turn context automatically
- Enables tool calls and model reasoning within a unified API
- Supports Python code execution and business logic integration
- Allows for rapid prototyping with standard or fine-tuned large language models

**Sample Built-in Tools**

- **File Search:** Ground agent responses in specific content
- **Function Calling:** Invoke custom APIs directly
- **Code Interpreter:** Execute Python securely for analytics
- **Computer Use:** Automate UI and browser actions
- **Image Generation:** Create or edit images inline
- **Remote MCP Server:** Integrate with external or community tools

---

## Integration with Azure AI Foundry Agent Stack

- **Single Agent Use:** Responses API is the quickest way to get started with robust, tool-using agents.
- **Multiple Agents and Enterprise Integration:** Use Azure AI Foundry Agent Service for management, observability, debugging, and secure deployment. Integrate with Microsoft Fabric, SharePoint, Bing, and more.
- **Prototyping:** Leverage Semantic Kernel and AutoGen for open-source, local development, with straightforward deployment paths into Foundry.

---

## Getting Started

The Responses API is available in all supported Azure regions and ready for production use. Explore more at:

- [Azure AI Foundry overview](https://techcommunity.microsoft.com/t5/ai.azure.com)
- [Official documentation](https://learn.microsoft.com/en-us/azure/ai-foundry/openai/how-to/responses?tabs=python-secure)

_Updated Aug 26, 2025_

---

## About the Author

BalaPV is a contributor at Microsoft, sharing updates and best practices for Azure AI services.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/ai-azure-ai-services-blog/the-responses-api-in-azure-ai-foundry-is-now-generally-available/ba-p/4446567)
