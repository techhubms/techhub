---
section_names:
- ai
- azure
- devops
- github-copilot
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/from-prototype-to-production-building-a-hosted-agent-with-ai/ba-p/4501969
author: carlottacaste
date: 2026-03-16 07:00:00 +00:00
primary_section: github-copilot
feed_name: Microsoft Tech Community
title: 'From Prototype to Production: Building a Hosted Agent with AI Toolkit & Microsoft Foundry'
tags:
- Agent Builder
- Agent Evaluation
- Agentic AI
- AI
- AI Toolkit
- Azure
- Azure AI Foundry
- Cloud Deployment
- Code Interpreter
- Community
- Container Configuration
- DevOps
- File Search
- GitHub Copilot
- Hosted Agent
- LLM Judges
- Local Debugging
- Microsoft Agent Framework
- Microsoft Foundry
- Model Catalog
- Model Selection
- Monitoring Dashboards
- Observability
- Red Teaming
- Tool Calling
- VS Code
- YAML Agent Definition
---

carlottacaste walks through an end-to-end workflow for taking an agent from prototype to production using the AI Toolkit in VS Code and Microsoft Foundry, covering model selection, agent setup, migration to hosted code, deployment, and ongoing evaluation/monitoring.<!--excerpt_end-->

## Overview

This post (with an accompanying video) walks through building, testing, and operationalizing a **hosted AI agent** using:

- [AI Toolkit in Visual Studio Code](https://code.visualstudio.com/docs/intelligentapps/overview)
- [Microsoft Foundry](https://learn.microsoft.com/azure/foundry/)

The focus is on moving beyond a prompt-only prototype to something **scalable, testable, and production-ready**.

## Scenario: retail agent for sales and inventory insights

The demo uses a fictional DIY/home-improvement retailer called **Zava**. The agent should help an internal team:

- Analyze **sales data** (reasoning over a **product catalog**, identifying **top-selling categories**, etc.)
- Manage **inventory** (detecting **low-stock products**, triggering **restock actions**, etc.)

## Chapter 1 (00:00–01:20): Model selection with GitHub Copilot and AI Toolkit

Work starts in [Visual Studio Code](https://code.visualstudio.com/) using [GitHub Copilot](https://github.com/features/copilot) together with the AI Toolkit.

Instead of choosing a model ad hoc, the workflow is:

- Describe the business scenario in natural language
- Ask Copilot to run a **comparative analysis** between two candidate models
- Define explicit evaluation criteria:
  - reasoning quality
  - tool support
  - suitability for analytics

Copilot uses AI Toolkit capabilities to explain why one model is a better fit, making model selection more transparent and repeatable.

Then the post highlights the **AI Toolkit Model Catalog**, including:

- Browsing hundreds of models
- Filtering by hosting platform (GitHub, Microsoft Foundry, local)
- Filtering by publisher (open source and proprietary)

After selecting a model, it’s deployed to **Microsoft Foundry** “with a single click” and validated with test prompts.

## Chapter 2 (01:20–02:48): Rapid prototyping with Agent Builder UI

With a model deployed, the agent is configured in an **Agent Builder UI**:

- Agent identity (name, role, responsibilities)
- Instructions defining tone, behavior, and scope
- The model the agent runs on
- Tools and data sources

Tools/data used for the Zava scenario:

- **File search** grounded on uploaded sales logs and a product catalog
- **Code interpreter** to compute metrics, generate charts, and write reports

Example prompt used in the playground:

> “What were the top three selling categories in 2025?”

The post emphasizes that answers are grounded in the provided retailer data and that you can inspect which tools/data were used.

Agent Builder also includes local **evaluation** and **tracing**.

## Chapter 3 (02:48–04:04): From UI prototype to hosted agent code

To move toward real production needs (custom logic), the workflow migrates from UI configuration to a **hosted agent template**.

The scaffold includes:

- Agent code built with [Microsoft Agent Framework](https://github.com/microsoft/agent-framework) (Python or C#)
- A YAML-based agent definition
- Container configuration files

From there, the agent is extended with **custom functions**, such as creating and managing restock orders. GitHub Copilot is used to adapt the template to the Zava scenario.

## Chapter 4 (04:04–05:12): Local debugging and cloud deployment

Before deployment, the agent is tested locally:

- Identify products running out of stock
- Trigger a restock action via the custom function
- Debug the full end-to-end tool-calling flow

Once validated, the agent is deployed to **Microsoft Foundry**.

## Chapter 5 (05:12–08:04): Evaluation, safety, and monitoring in Foundry

In the Foundry portal, the post calls out production operational features:

- **Evaluation runs** using real and synthetic datasets
- **LLM-based judges** scoring responses across multiple metrics with explanations
- **Red teaming** via an adversarial agent probing for unsafe/undesired behavior
- **Monitoring dashboards** across the agent fleet:
  - usage
  - latency
  - regressions
  - cost

The intent is shifting from ad-hoc testing to continuous quality and safety assessment.

## Why this workflow matters

Key point:

- Agentic AI in production is as much about **operating agents responsibly at scale** as it is about building them.

Combining AI Toolkit in VS Code with Microsoft Foundry is presented as enabling:

- A smoother developer experience
- Separation between experimentation and production
- Built-in evaluation, safety, and observability

## Resources

- Demo sample repository: https://github.com/carlotta94c/zava-sales-manager-hosted-agent
- Foundry tutorials playlist: https://www.youtube.com/playlist?list=PLlrxD0HtieHj61bBwrAqd5yHvwjB8s_oz

[Read the entire article](https://techcommunity.microsoft.com/t5/microsoft-developer-community/from-prototype-to-production-building-a-hosted-agent-with-ai/ba-p/4501969)

