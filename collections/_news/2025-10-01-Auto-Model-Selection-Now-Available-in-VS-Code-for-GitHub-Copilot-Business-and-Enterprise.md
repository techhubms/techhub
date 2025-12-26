---
layout: "post"
title: "Auto Model Selection Now Available in VS Code for GitHub Copilot Business and Enterprise"
description: "This announcement details the public preview of auto model selection in Visual Studio Code for GitHub Copilot Business and Enterprise plans. The feature automatically selects among supported AI models, enhances transparency for developers, and provides administrators with control over policy and access while optimizing for model availability and billing efficiency."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-09-30-auto-model-selection-is-now-in-vs-code-for-copilot-business-and-enterprise"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-10-01 12:59:10 +00:00
permalink: "/news/2025-10-01-Auto-Model-Selection-Now-Available-in-VS-Code-for-GitHub-Copilot-Business-and-Enterprise.html"
categories: ["AI", "GitHub Copilot"]
tags: ["AI", "AI Models", "Auto Model Selection", "Business Plans", "Copilot", "Enterprise Plans", "GitHub Copilot", "GPT 4.1", "GPT 5", "Model Policies", "News", "Premium Requests", "Sonnet 3.5", "Sonnet 4", "VS Code", "VS Code Extension"]
tags_normalized: ["ai", "ai models", "auto model selection", "business plans", "copilot", "enterprise plans", "github copilot", "gpt 4dot1", "gpt 5", "model policies", "news", "premium requests", "sonnet 3dot5", "sonnet 4", "vs code", "vs code extension"]
---

Allison introduces the new auto model selection feature in Visual Studio Code for GitHub Copilot Business and Enterprise, explaining how it improves model selection transparency and policy management for developers and administrators.<!--excerpt_end-->

# Auto Model Selection Now in VS Code for Copilot Business and Enterprise

Auto model selection is now in public preview for GitHub Copilot Business and Enterprise users within Visual Studio Code. With this feature, Copilot automatically selects the AI model to optimize for availability and user needs.

## How Auto Model Selection Works

- **Model Choice**: Copilot automatically routes requests to models such as GPT-5, GPT-5 mini, GPT-4.1, Sonnet 4, and Sonnet 3.5, with new models planned for future support.
- **Transparency**: Users can see which model was used by hovering over each Copilot response in the editor.
- **User Control**: Users may switch between auto selection and specific models at any time.
- **Policy Adherence**: Auto respects and enforces model policies defined by organization administrators.

## Premium Request Billing

- Premium request usage depends on the model automatically selected. Models with multipliers ranging from 0x to 1x are supported.
- Subscribers get a 10% discount on the multiplier when auto is used; for instance, a 1x multiplier costs 0.9 requests.

## Getting Started

- **Administrator Settings**: Admins can enable access to auto model selection by activating the **Editor preview features** policy.
- **User Activation**: Once enabled, users can select **Auto** in Copilot Chat within VS Code.
- More setup and usage details can be found in [the official documentation](https://docs.github.com/copilot/concepts/auto-model-selection).

## Looking Ahead

- The preview currently optimizes for model availability. Future releases will improve intelligent model selection based on user tasks and feedback.
- Developers and administrators are encouraged to participate in the [community discussion](https://github.com/orgs/community/discussions/173188) to share feedback and shape ongoing development.

*Disclaimer: Public preview features and UI are subject to change.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-30-auto-model-selection-is-now-in-vs-code-for-copilot-business-and-enterprise)
