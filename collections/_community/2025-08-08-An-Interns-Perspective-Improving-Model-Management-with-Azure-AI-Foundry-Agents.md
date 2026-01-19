---
layout: post
title: "An Intern's Perspective: Improving Model Management with Azure AI Foundry Agents"
author: Harry-Cheng
canonical_url: https://techcommunity.microsoft.com/t5/ai-ai-platform-blog/the-future-of-ai-an-intern-s-adventure-improving-usability-with/ba-p/4440857
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=AI
date: 2025-08-08 13:31:56 +00:00
permalink: /ai/community/An-Interns-Perspective-Improving-Model-Management-with-Azure-AI-Foundry-Agents
tags:
- AI Agents
- Azure AI
- Azure AI Foundry
- Azure OpenAI Service
- Bicep
- Cloud Automation
- Conversational AI
- Deployment Automation
- Enterprise AI
- IaC
- Infrastructure as Agents
- Microsoft
- Model Inventory
- Model Lifecycle
- Model Management
- Model Operation Agent
- Model Upgrades
- Quota Validation
- SDK
- Terraform
- VS Code
section_names:
- ai
- azure
---
Harry-Cheng shares insights from his internship on the Azure AI Foundry team, presenting a hands-on look at building the 'model operation agent.' This post highlights how conversational AI is used to tackle enterprise model management challenges.<!--excerpt_end-->

# An Intern's Perspective: Improving Model Management with Azure AI Foundry Agents

*By Harry-Cheng*

## Introduction

As part of the AI Futures team at Microsoft, this post dives into the complexities of managing AI models across enterprise Azure environments. With organizations deploying multiple models in various regions and under different configurations, operations can quickly become overwhelming. The 'model operation agent' project addresses these pain points by offering a chat-driven assistant for model management on Azure AI Foundry.

## Why Is Model Management Challenging?

- **Geographically distributed deployments:** Models may be hosted in various Azure regions to meet compliance and performance needs.
- **Diverse infrastructure SKUs:** Teams might use Standard, Global, DataZone, or other SKUs.
- **Model/version sprawl:** Different model families and versions can proliferate throughout an organization.
- **Quota and capacity constraints:** Each deployment is subject to quota and resource limitations in Azure.

Manual management is complex, error-prone, and inefficient at scale.

## The Model Operation Agent

### Four Core Capabilities

1. **Discovery:**
   - Automatically scans subscriptions for all model deployments
   - Builds a centralized, searchable inventory
   - Example output:
     
     | Account Name     | Resource Group | Location | Deployment   | Model        | Version   | SKU       | Capacity |
     |-----------------|---------------|----------|--------------|--------------|-----------|-----------|----------|
     | foundry-eastus  | rg-prod       | eastus   | gpt4-prod    | gpt-4o       | 2024-05-13| Standard  | 10       |
     | foundry-westus  | rg-prod       | westus   | gpt4-west    | gpt-4        | 0613      | GlobalStd | 20       |
     | foundry-europe  | rg-eu         | westeu   | chat-eu      | gpt-35-turbo | 1106      | DataZone  | 30       |

2. **Analysis:**
   - Looks up Microsoft's model retirement schedules
   - Identifies upgrade needs and recommends replacement models
   - Example output:
     - Model: gpt-4 (version 0613)
     - Retirement Date: June 6, 2025
     - Recommended Replacement: gpt-4o version 2024-11-20
     - Upgrade Priority: High (retirement in 4 months)

3. **Validation:**
   - Checks current quota and usage per region/SKU
   - Warns if upgrades would breach capacity limits
   - Suggests alternative regions/SKUs for smoother upgrades

4. **Execution:**
   - Supports bulk updates across multiple deployments
   - Provides detailed logs, error reports, and rollback suggestions

## From IaC to IaA: The Future of Cloud Operations

- **Vision:** Move beyond traditional Infrastructure as Code tools like Terraform and Bicep.
- **Goal:** Enable Infrastructure as Agents (IaA), where you interact with cloud resources through natural language and intelligent agents.
- **Potential Benefits:**
  - Simplifies cloud orchestration
  - Reduces the learning curve for infrastructure provisioning and management
  - Allows product teams to focus on innovation over syntax

## Getting Started with Azure AI Foundry

- [Azure AI Foundry portal](https://ai.azure.com/)
- [Visual Studio Code extension](https://marketplace.visualstudio.com/items?itemName=TeamsDevApp.vscode-ai-foundry)
- [Azure AI Foundry SDK](https://aka.ms/aifoundrysdk)
- [Learning modules](https://aka.ms/CreateAgenticAISolutions)
- [Documentation](https://learn.microsoft.com/azure/ai-foundry/)
- Community links: [GitHub](https://aka.ms/azureaifoundry/forum), [Discord](https://aka.ms/azureaifoundry/discord)

---

*Published: Aug 08, 2025 | Author: Harry-Cheng*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/ai-ai-platform-blog/the-future-of-ai-an-intern-s-adventure-improving-usability-with/ba-p/4440857)
