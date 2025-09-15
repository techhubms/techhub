---
layout: "post"
title: "Auto Model Selection Preview for GitHub Copilot Chat in VS Code"
description: "This article introduces the new auto model selection feature—now in preview—within GitHub Copilot Chat for VS Code. It outlines how this capability automatically picks the optimal AI model for each chat request, dynamically balancing performance and capacity, and details upcoming improvements. The post explains pricing impacts, such as discounts for paid users, and how the feature manages model choices like Claude Sonnet 4 or GPT-5-mini. The article is written by Isidor Nikolic."
author: "Isidor Nikolic"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://code.visualstudio.com/blogs/2025/09/15/autoModelSelection"
viewing_mode: "external"
feed_name: "Visual Studio Code Releases"
feed_url: "https://code.visualstudio.com/feed.xml"
date: 2025-09-15 00:00:00 +00:00
permalink: "/2025-09-15-Auto-Model-Selection-Preview-for-GitHub-Copilot-Chat-in-VS-Code.html"
categories: ["AI", "GitHub Copilot"]
tags: ["AI", "AI Integration", "AI Model Selection", "Auto Selection", "Claude Sonnet 4", "Copilot Chat", "Developer Tools", "GitHub Copilot", "GPT 5", "GPT 5 Mini", "Model Discount", "Model Multiplier", "News", "OpenAI", "Premium Requests", "VS Code", "VS Code Dev Days"]
tags_normalized: ["ai", "ai integration", "ai model selection", "auto selection", "claude sonnet 4", "copilot chat", "developer tools", "github copilot", "gpt 5", "gpt 5 mini", "model discount", "model multiplier", "news", "openai", "premium requests", "vs code", "vs code dev days"]
---

Isidor Nikolic details the auto model selection feature preview for GitHub Copilot in VS Code, explaining how it optimizes model choices for better performance and savings for developers.<!--excerpt_end-->

# Introducing Auto Model Selection (Preview) for GitHub Copilot in VS Code

*By Isidor Nikolic*

The new auto model selection feature, now available in preview for GitHub Copilot users in VS Code, brings faster responses, reduced rate limiting, and cost savings through automatic discounting on select premium requests. With auto model selection, Copilot Chat automatically chooses the best available AI model for each request—such as Claude Sonnet 4, GPT-5, or GPT-5 mini—based on current capacity and performance, instead of requiring users to pick models manually.

> **Key benefits:**
> - Optimizes performance by dynamically choosing between large and small models
> - Minimizes your risk of hitting rate limits
> - Applies a 10% discount on premium requests for paid users
> - Keeps chat sessions on the same model but will allow switching based on task complexity in future updates

## How Auto Model Selection Works

- **Automatic Model Choice:** Auto decides between available models for each request, prioritizing speed and resource availability.
- **Chat Session Consistency:** Once a model is selected for your chat session, all further messages use that model; this may change as task-based switching is introduced.
- **Pricing and Discounts:** For paid users, auto tends to favor Claude Sonnet 4 and provides a 10% discount on premium requests. The model multiplier varies depending on which model is selected (see [documentation](https://docs.github.com/en/copilot/concepts/billing/copilot-requests#model-multipliers)).
- **Premium Request Management:** If you exhaust premium requests, auto will default to models that do not count against your balance—such as GPT-5 mini—so you can keep coding without interruption.

You can view which model is active along with the applicable multiplier by hovering over a chat response in VS Code.

## Upcoming Enhancements

The feature roadmap includes:

- Dynamic switching between models according to complexity of your task
- Broadening available language models in auto
- Allowing free plan users to leverage newer models through auto
- UI improvements to clarify model and discount information at a glance

## Get Involved and Learn More

Join a [VS Code Dev Days event](/dev-days) for hands-on learning, or test new features early using [VS Code Insiders](https://code.visualstudio.com/insiders/). To provide feedback and contribute ideas, visit the [VS Code repository](https://github.com/microsoft/vscode/issues). For technical reference and the latest updates, check out the [auto model selection documentation](https://code.visualstudio.com/docs/copilot/customization/language-models#_auto-model-selection).

---

Auto model selection is designed to help VS Code scale with increasing demand and ensure that every developer gets a smooth and efficient AI-assisted coding experience.

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2025/09/15/autoModelSelection)
