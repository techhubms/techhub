---
layout: "post"
title: "Auto Model Selection for GitHub Copilot in VS Code Public Preview"
description: "This announcement details the public preview release of the new Auto model selection feature in GitHub Copilot for Visual Studio Code. The Auto option automatically selects the most suitable model for each request, optimizing for availability while maintaining transparency and allowing user control. It is currently available for Copilot Free, Pro, and Pro+ users, with planned support for Enterprise and Business licenses. Key features include discounted premium requests for Pro tiers, policy respect, and the ability for users to see or override model choices. The roadmap includes further intelligence for task-specific optimization."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-09-14-auto-model-selection-for-copilot-in-vs-code-in-public-preview"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-09-15 12:53:46 +00:00
permalink: "/2025-09-15-Auto-Model-Selection-for-GitHub-Copilot-in-VS-Code-Public-Preview.html"
categories: ["AI", "GitHub Copilot"]
tags: ["AI", "AI Models", "Auto Model Selection", "Billing", "Copilot", "Copilot Business", "Copilot Enterprise", "Copilot Free", "Copilot Pro", "Copilot Pro+", "Developer Tools", "GitHub Copilot", "GPT 4.1", "GPT 5", "Model Picker", "News", "Premium Request", "Public Preview", "Sonnet 4", "VS Code", "VS Code Extension"]
tags_normalized: ["ai", "ai models", "auto model selection", "billing", "copilot", "copilot business", "copilot enterprise", "copilot free", "copilot pro", "copilot proplus", "developer tools", "github copilot", "gpt 4dot1", "gpt 5", "model picker", "news", "premium request", "public preview", "sonnet 4", "vs code", "vs code extension"]
---

Allison outlines the new Auto model selection capability in GitHub Copilot for Visual Studio Code, detailing how it streamlines model choice, offers premium request discounts to Pro users, and gives developers more flexibility and control.<!--excerpt_end-->

# Auto Model Selection for GitHub Copilot in VS Code Public Preview

GitHub Copilot has introduced an **Auto** model selection feature in Visual Studio Code, aiming to simplify the experience of choosing an AI model for code completion and chat. This feature is now available in public preview for Copilot Free, Pro, and Pro+ subscriptions. Support for Copilot Enterprise and Copilot Business is expected in the near future.

## How Auto Model Selection Works

- **Model Optimization**: The Auto option dynamically picks from several available models, currently including GPT-5, GPT-5 mini, GPT-4.1, Sonnet 4, and Sonnet 3.5. Future updates will introduce additional models.
- **Transparency**: Users can see which model completed a particular request by hovering over the model response in the UI.
- **User Control**: At any time, developers can switch between Auto and specific model choices as needed.
- **Policy Compliance**: The Auto selection feature respects all user-defined model usage policies.

## Premium Request and Billing Details

- **Model-Based Premium Requests**: The cost (in premium requests) for auto-selected models corresponds to the selected model's multiplier. For more details, refer to the [GitHub Copilot request billing documentation](https://docs.github.com/copilot/concepts/billing/copilot-requests#model-multipliers).
- **Discounts for Pro Tiers**: Copilot Pro and Pro+ subscribers receive a 10% discount on the premium request multiplier when Auto chooses a premium model. For example, if a model normally uses a 1x multiplier, Auto mode will draw down only 0.9 premium requests for Pro and Pro+ accounts.

## Getting Started

- Access Copilot Chat within Visual Studio Code.
- In the model picker, select **Auto**.
- You can read more in the [official documentation](https://docs.github.com/copilot/concepts/auto-model-selection).

## Looking Forward

This initial preview focuses on optimizing for model availability. GitHub's team is actively developing more intelligent selection methods that will account for specific tasks in the future. Your feedback is encouraged—contribute in the ongoing [Community discussion](https://github.com/orgs/community/discussions/173188) to help shape the direction of this feature.

*Disclaimer: The Auto model selection UI is in public preview and may change before general availability.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-14-auto-model-selection-for-copilot-in-vs-code-in-public-preview)
