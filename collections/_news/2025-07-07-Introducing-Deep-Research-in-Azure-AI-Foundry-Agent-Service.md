---
layout: "post"
title: "Introducing Deep Research in Azure AI Foundry Agent Service"
description: "Microsoft announces the public preview of Deep Research in Azure AI Foundry, an API and SDK-based agentic research capability built on OpenAI models. This tool automates enterprise-scale web research, offering programmable, composable, and auditable agents for advanced information synthesis and integration with Azure workflows."
author: "Yina Arenas"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://azure.microsoft.com/en-us/blog/introducing-deep-research-in-azure-ai-foundry-agent-service/"
viewing_mode: "external"
feed_name: "The Azure Blog"
feed_url: "https://azure.microsoft.com/en-us/blog/feed/"
date: 2025-07-07 17:00:00 +00:00
permalink: "/news/2025-07-07-Introducing-Deep-Research-in-Azure-AI-Foundry-Agent-Service.html"
categories: ["AI", "Azure"]
tags: ["Agents", "AI", "AI + Machine Learning", "API", "Azure", "Azure AI Foundry", "Azure Functions", "Azure Logic Apps", "Bing Search", "Compliance", "Compute", "Containers", "Deep Research", "Enterprise Automation", "GPT 4", "Integration", "Internet Of Things", "News", "OpenAI", "Pricing", "SDK", "Web Research"]
tags_normalized: ["agents", "ai", "ai plus machine learning", "api", "azure", "azure ai foundry", "azure functions", "azure logic apps", "bing search", "compliance", "compute", "containers", "deep research", "enterprise automation", "gpt 4", "integration", "internet of things", "news", "openai", "pricing", "sdk", "web research"]
---

Authored by Yina Arenas, this article introduces Deep Research in Azure AI Foundry Agent Service, detailing its features and benefits for enterprise AI-driven research automation using API and SDK integrations.<!--excerpt_end-->

# Introducing Deep Research in Azure AI Foundry Agent Service

**Author:** Yina Arenas

Microsoft has announced the public preview of **Deep Research in Azure AI Foundry**—an API and SDK-based offering leveraging OpenAI’s advanced agentic research capabilities. This new tool is aimed at organizations that require enterprise-scale web research automation and programmable agent-based workflows, fully integrated with the Azure ecosystem.

---

## Unlocking Enterprise-Scale Web Research Automation

Deep Research enables developers to build agents capable of deep planning, analysis, and synthesis of information from across the web. The service allows complex research tasks to be automated, generating transparent and auditable outputs, and composing multi-step workflows through seamless integration with [Azure AI Foundry](https://azure.microsoft.com/en-us/products/ai-foundry/?msockid=36d156f74ff96d723016422b4e966cdb).

Developers and enterprises can:

- Automate in-depth research tasks.
- Ensure every insight is traceable and auditable.
- Embed research capabilities into business apps and automated workflows.
- Govern knowledge and research processes at enterprise scale.

[Create with Azure AI Foundry](https://azure.microsoft.com/en-us/products/ai-foundry/?msockid=36d156f74ff96d723016422b4e966cdb)

---

## Meeting the Next Frontier of Research Automation

Generative AI and large language models are transforming research and analysis, powering solutions like ChatGPT Deep Research and Researcher in Microsoft 365 Copilot. While these solutions are designed for individual and team productivity, organizations are seeking to:

- Integrate deep research directly into business applications.
- Automate multi-step processes.
- Enable programmable, composable, and auditable research automation.

Azure AI Foundry’s Deep Research delivers this flexibility, empowering businesses to orchestrate research-as-a-service within their entire ecosystem, using their own data and systems.

---

## Capabilities in Azure AI Foundry Agent Service

Deep Research in Foundry Agent Service is designed for developers who need to move beyond basic chat assistants. Key features include:

- **Automated, web-scale research** grounded with Bing Search, providing traceable insights.
- **Programmatic agent creation**, allowing agents to be called by workflows, apps, or other agents.
- **Workflow orchestration,** integrating with Azure Logic Apps, [Azure Functions](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/how-to/tools/logic-apps?pivots=portal), and other connectors for tasks like reporting and notifications.
- **Enterprise governance** with security, compliance, and observability controls built-in.

Unlike static assistants, Deep Research agents are extensible and integrate smoothly with future internal data sources.

---

## Architecture and Agent Flow

The Deep Research service is architected for flexibility, transparency, and composability, automating robust research processes. The core research model (**o3-deep-research**) orchestrates a multi-step pipeline:

1. **Clarifying Intent & Scoping:**
   - Utilizes GPT-series models (GPT-4o, GPT-4.1) to understand the query, gather context, and precisely define the research scope for actionable results.
2. **Web Grounding with Bing Search:**
   - Invokes Bing Search to retrieve recent, high-quality web data, ensuring up-to-date and authoritative information.
3. **Deep Research Execution:**
   - Engages in iterative analysis and synthesis of information across sources, reasoning step-by-step to uncover nuanced insights.
4. **Transparency, Safety, and Compliance:**
   - Produces a structured report including answers, reasoning, citations, and clarification steps for full auditability.
5. **Programmatic Integration and Composition:**
   - Offers API access, enabling invocation from custom business apps, internal portals, workflow tools, or broader agent ecosystems.
   - Supports scenarios like multi-agent chains where, for example, one agent handles research, another generates reports (e.g., slide decks via Azure Functions), and another manages communications (e.g., emailing results through Azure Logic Apps).

This flexible architecture facilitates embedding Deep Research into various enterprise workflows. Organizations are already exploring use cases including market analysis, competitive intelligence, analytics, and regulatory reporting.

---

## Pricing

- **Input Tokens:** $10.00 per 1 million tokens
- **Cached Input:** $2.50 per 1 million tokens
- **Output:** $40.00 per 1 million tokens

Search context tokens are charged at the input token price of the selected model. Additional charges apply for [Grounding with Bing Search](https://www.microsoft.com/en-us/bing/apis/grounding-pricing) and any base GPT models used for clarifying queries.

---

## Getting Started with Deep Research

Deep Research is available now as a limited public preview for Azure AI Foundry Agent Service customers. To begin:

- [Sign up for early access.](https://aka.ms/oai/deepresearchaccess)
- Explore the [Deep Research documentation](https://aka.ms/agents-deep-research).
- Complete [learning modules](https://learn.microsoft.com/en-us/training/paths/develop-ai-agents-on-azure/?source=docs) to build your first Azure AI Foundry agent.
- Start creating with [Azure AI Foundry](https://ai.azure.com/).

Microsoft encourages developers to experiment and look forward to future enhancements and customer stories as the Deep Research platform evolves.

---

*For more information, refer to the official Microsoft Azure Blog announcement.*

This post appeared first on "The Azure Blog". [Read the entire article here](https://azure.microsoft.com/en-us/blog/introducing-deep-research-in-azure-ai-foundry-agent-service/)
