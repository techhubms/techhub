---
layout: "post"
title: "Anthropic Models Integrated with OpenAI in Microsoft Copilot Studio"
description: "This announcement introduces Anthropic's Claude Sonnet 4 and Claude Opus 4.1 models alongside OpenAI models in Microsoft Copilot Studio. The update details how admins can manage model access, how makers select models for different agent scenarios, and provides a practical walkthrough for building an onboarding agent with model choice capabilities."
author: "stclarke"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/anthropic-joins-the-multi-model-lineup-in-microsoft-copilot-studio/"
viewing_mode: "external"
feed_name: "Microsoft News"
feed_url: "https://news.microsoft.com/source/feed/"
date: 2025-09-24 16:48:15 +00:00
permalink: "/2025-09-24-Anthropic-Models-Integrated-with-OpenAI-in-Microsoft-Copilot-Studio.html"
categories: ["AI"]
tags: ["Admin Controls", "Agent Deployment", "AI", "AI Tools", "Anthropic", "Claude Opus 4.1", "Claude Sonnet 4", "Company News", "GPT 4o", "HR Onboarding Agent", "Microsoft Copilot Studio", "Model Orchestration", "Multi Agent Orchestration", "News", "OpenAI", "Power Platform Admin Center", "Prompt Engineering", "Workflow Automation"]
tags_normalized: ["admin controls", "agent deployment", "ai", "ai tools", "anthropic", "claude opus 4dot1", "claude sonnet 4", "company news", "gpt 4o", "hr onboarding agent", "microsoft copilot studio", "model orchestration", "multi agent orchestration", "news", "openai", "power platform admin center", "prompt engineering", "workflow automation"]
---

stclarke shares how Microsoft Copilot Studio now supports Anthropic models alongside OpenAI models, allowing users and admins to choose and orchestrate AI models for agent development and automation.<!--excerpt_end-->

# Anthropic Models Integrated with OpenAI in Microsoft Copilot Studio

Microsoft Copilot Studio now supports Anthropic models—Claude Sonnet 4 and Claude Opus 4.1—alongside OpenAI models, giving makers and admins more flexibility in designing agents for business process automation and reasoning tasks.

## Multi-Model Choices for Agents

- **Anthropic and OpenAI Models:** Users can now select either OpenAI or Anthropic models (Claude Sonnet 4, Claude Opus 4.1) for building, orchestrating, and optimizing agents and workflows in Copilot Studio.
- **Model Selection:** The prompt builder menu in Copilot Studio lets you choose a specific model for each part of an agent's logic, making it easier to optimize for unique business use cases.

## How to Enable and Manage Anthropic Models

1. **Admin Enablement:**
   - Anthropic models must be enabled via the Microsoft 365 Admin Center (MAC).
   - Once enabled, they're on by default in the Power Platform Admin Center (PPAC), where additional environment controls are available.
2. **Fallback Handling:**
   - If Anthropic models are disabled, agents default back to OpenAI GPT-4o automatically without further user configuration.

## Example: Building an HR Onboarding Agent

Follow these steps to leverage the new model flexibility:

1. **Create Agent:** Start a new agent in Copilot Studio, targeting HR onboarding.
2. **Connect Knowledge:** Attach onboarding docs, FAQs, and policy materials.
3. **Design Prompts and Workflows:** Define the conversational flows and logic for onboarding scenarios.
4. **Model Selection:** In agent settings, choose between OpenAI GPT-5, Claude Sonnet 4, or Claude Opus 4.1 as the primary reasoning model.
5. **Configure AI Tools:** Assign different models for specific prompts or tasks, such as using one for communications and another for compliance tasks.
6. **Deploy and Test:** Publish the agent, test with real onboarding scenarios, and iterate based on user feedback.

## Multi-Agent Orchestration

- Copilot Studio supports coordinating multiple agents, each with its own primary model, enabling complex automation and collaboration across workflows.

## Getting Started and Information

- [Try Copilot Studio](https://www.microsoft.com/en-us/microsoft-copilot/microsoft-copilot-studio/)
- [Model choice documentation](https://go.microsoft.com/fwlink/?linkid=2334705)
- [Admin early release cycle info](https://learn.microsoft.com/en-us/power-platform/admin/early-release)
- [Power Platform Admin Center (PPAC)](https://go.microsoft.com/fwlink/?linkid=2334706)

For community support and feedback, visit the [Power Platform Community Forums](https://powerusers.microsoft.com/t5/Forums/ct-p/pva_forums).

> **Note:** Model choice is available in early release environments and will be available in preview and production soon. Admins control model availability tenant-wide.

This post appeared first on "Microsoft News". [Read the entire article here](https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/anthropic-joins-the-multi-model-lineup-in-microsoft-copilot-studio/)
