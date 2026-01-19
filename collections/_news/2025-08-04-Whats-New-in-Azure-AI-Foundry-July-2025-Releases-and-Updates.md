---
external_url: https://devblogs.microsoft.com/foundry/whats-new-in-azure-ai-foundry-july-2025/
title: 'What’s New in Azure AI Foundry: July 2025 Releases and Updates'
author: Nick Brady
viewing_mode: external
feed_name: Microsoft DevBlog
date: 2025-08-04 15:00:27 +00:00
tags:
- .NET
- Agent Monitoring
- AgentOps
- AI Development
- AI Red Teaming
- Azure AI Foundry
- Azure OpenAI
- Azure SDK
- Deep Research
- Deep Research Agent
- Entra Agent ID
- Fine Tuning
- Generative AI
- GPT Image 1
- Java
- JavaScript
- MCP
- Model Evaluation
- OpenAI
- Prompt Shields
- Purview
- Python
- Reinforcement Learning
- TypeScript
section_names:
- ai
- azure
- coding
- security
---
Authored by Nick Brady, this post highlights the July 2025 updates to Azure AI Foundry, including Deep Research Agent, GPT-image-1 model improvements, agent tools, and platform and security features for advanced AI development.<!--excerpt_end-->

## What’s New in Azure AI Foundry — July 2025

**Author:** Nick Brady

Azure AI Foundry's July 2025 release introduces a range of significant improvements for AI developers on Microsoft Azure. This roundup outlines new AI agent capabilities, developer tools, SDK enhancements, platform features, and updated security measures.

---

### TL;DR

- **Deep Research Agent (Public Preview):** Automate complex, multi-step web research using OpenAI’s o3-deep-research model, now available through Azure AI Foundry Agent Service.
- **GPT-image-1 Model:** Adds `input_fidelity` parameter for controlling style preservation during image edits and partial image streaming for real-time visual previews.
- **AgentOps, Red Teaming, Tracing:** Improved agent monitoring, evaluation, and compliance tools help ensure reliability and security in production.
- **Platform & Security:** New pay-as-you-go compute, Prompt Shields (GA), Entra Agent ID, and Purview integration deliver secure, scalable AI deployments.
- **SDK & Documentation:** Major updates for Python, .NET, Java, and JavaScript SDKs; new guides for Deep Research and agent development.

---

## Developer Community

Join 25,000+ developers via Discord, GitHub Discussions, and live events for open-source courses, code samples, and regular office hours covering Agents, MCP, and Generative AI topics.

[Join Community](https://aka.ms/foundrydevs?utm_source=devblog&amp;utm_medium=blog&amp;utm_campaign=whats-new-july-2025&amp;utm_content=developer-community)

---

## Models

### GPT-image-1 Model Enhancements (Preview)

- **Input Fidelity Parameter:** The new `input_fidelity` in the image edits API controls how much the original style and features are preserved during edits. This benefits photo editing (e.g., altering facial features, preserving avatars), brand identity, and product imagery.
- **Partial Image Streaming:** Users now receive progressive visual feedback as images are generated or edited, improving the editing workflow experience.

[Learn more](https://learn.microsoft.com/en-us/azure/ai-foundry/openai/whats-new?utm_source=devblog&amp;utm_medium=blog&amp;utm_campaign=whats-new-july-2025&amp;utm_content=model-leaderboard#july-2025)

---

## Agents

### Deep Research Agent (Public Preview)

- New in Azure AI Foundry Agent Service, this agent automates sophisticated web research using OpenAI’s latest agentic model (`o3-deep-research`), integrated with Bing Search for up-to-date and authoritative information.
- Developers can [sign up for preview access](https://aka.ms/oai/deepresearchaccess).

[Read announcement](https://azure.microsoft.com/en-us/blog/introducing-deep-research-in-azure-ai-foundry-agent-service/?msockid=24a603ad21096ed4294316bd20246ff8?utm_source=devblog&amp;utm_medium=blog&amp;utm_campaign=whats-new-july-2025&amp;utm_content=deep-research)
[Learn more](https://aka.ms/agents-deep-research?utm_source=devblog&amp;utm_medium=blog&amp;utm_campaign=whats-new-july-2025&amp;utm_content=deep-research)

---

## Tools

### AgentOps: Tracing, Evaluation, and Monitoring

- Delivers enhanced tracing and validation for agent performance, with improved tools for robust monitoring and optimization in deployment scenarios.

[More info](https://learn.microsoft.com/en-us/azure/ai-foundry/how-to/develop/trace-agents-sdk?utm_source=devblog&amp;utm_medium=blog&amp;utm_campaign=whats-new-july-2025&amp;utm_content=agentops)

### AI Red Teaming Agent (Preview)

- Enables AI red teaming in the cloud through a new UI, with direct access to evaluation results within Azure AI Foundry projects.

[Get started](https://learn.microsoft.com/en-us/azure/ai-foundry/how-to/develop/run-ai-red-teaming-cloud?utm_source=devblog&amp;utm_medium=blog&amp;utm_campaign=whats-new-july-2025&amp;utm_content=red-teaming)

---

## Platform (API, SDK, UI, and more)

### Python SDK Updates

- **AI Agents 1.0.2/1.1.0b4:** Bug fixes, new tool support (Deep Research, MCP), and `tool_resources` overrides.
- **AI Evaluation 1.9.0:** Improved evaluators, Azure OpenAI model grader, and expanded risk categories for red teaming.
- **AI Projects 1.0.0b12:** Refactored methods, argument renames, and telemetry improvements.
- **Azure AI Search 11.5.3:** Operation handling bug fix.

[Python SDK Release Notes](https://azure.github.io/azure-sdk/releases/2025-07/python.html)

### .NET SDK Updates

- New features and breaking changes for Deep Research integration, method renames, agent management APIs, and project endpoint requirements.
- AI Foundry project client improvements, OpenAI chat client enhancements, bug fixes for Azure AI Search.

[.NET SDK Release Notes](https://azure.github.io/azure-sdk/releases/2025-07/dotnet.html)

### Java SDK Updates

- Major refactor, client merges, new authentication options, and improved code samples for AI Agents and Projects (beta).

[Java SDK Release Notes](https://azure.github.io/azure-sdk/releases/2025-07/java.html)

### JavaScript/TypeScript SDK Updates

- Bug fixes, breaking constructor changes, and new agent orchestration and evaluation features.

[JS/TS SDK Release Notes](https://azure.github.io/azure-sdk/releases/2025-07/js.html)

---

### Fine-tuning & Evaluation

- **RFT Observability (Preview):** Real-time checkpoint evaluations for Reinforcement Fine-tuning jobs.
- **Quick Evals (Preview):** One-click rapid assessment of model outputs.
- **Python Grader (Preview):** Custom logic for domain-specific fits.
- **Development Tier (GA):** 24-hour, zero-cost fine-tuning tier for rapid experimentation.

[Learn more](https://techcommunity.microsoft.com/blog/azure-ai-services-blog/what%E2%80%99s-new-in-azure-ai-foundry-finetuning-july-2025/4438850)

---

### Platform & Service API Improvements

- Model Context Protocol (MCP) connections enter public preview.
- Model Router and Leaderboard available through API and SDK.
- Reinforcement fine-tuning supported in all SDKs.
- New Developer Tier for free experimentation.

[More platform updates](https://learn.microsoft.com/en-us/azure/ai-foundry/whats-new-azure-ai-foundry)

---

## Documentation Updates

- [Provision with Terraform](https://learn.microsoft.com/en-us/azure/ai-foundry/how-to/create-resource-terraform)
- [Agent subnet requirements](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/how-to/virtual-networks)
- [Deep Research code samples](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/how-to/tools/deep-research-samples)
- [GPT-4.1 token limits](https://learn.microsoft.com/en-us/azure/ai-foundry/openai/concepts/models)
- [Connection string deprecation](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/includes/connection-string-deprecation)
- [Azure AI Administrator role GA](https://learn.microsoft.com/en-us/azure/ai-foundry/concepts/rbac-azure-ai-foundry)
- [File upload limits & quotas](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/quotas-limits)
- [Region availability updates](https://learn.microsoft.com/en-us/azure/ai-foundry/includes/region-availability-maas)
- [CLI instructions](https://learn.microsoft.com/en-us/azure/ai-foundry/openai/how-to/fine-tuning-deploy)
- [Project & hub deletion](https://learn.microsoft.com/en-us/azure/ai-foundry/how-to/create-projects)
- [Capability hosts](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/concepts/capability-hosts)
- [Python MCP samples](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/how-to/tools/model-context-protocol-samples)
- [Model docs, new region support](https://learn.microsoft.com/en-us/azure/ai-foundry/foundry-models/concepts/models)
- [Security PII filter](https://learn.microsoft.com/en-us/azure/ai-foundry/openai/concepts/content-filter-personal-information)
- [Fine-tuning cost management](https://learn.microsoft.com/en-us/azure/ai-foundry/openai/how-to/fine-tuning-cost-management)
- [Fine-tuning overview guide](https://learn.microsoft.com/en-us/azure/ai-foundry/concepts/fine-tuning-overview)

---

Happy building—and let us know what you ship with #AzureAIFoundry!

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/foundry/whats-new-in-azure-ai-foundry-july-2025/)
