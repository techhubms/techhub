---
layout: "post"
title: "Auto Model Selection in GitHub Copilot Now Available in Visual Studio Code"
description: "This update announces the general availability of auto model selection in GitHub Copilot for all plans using Visual Studio Code. The feature dynamically chooses the best model based on availability, mitigates rate limits, and respects user/admin settings. Premium request billing is explained, along with transparency features and upcoming enhancements for intelligent model selection."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-12-10-auto-model-selection-is-generally-available-in-github-copilot-in-visual-studio-code"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-12-10 19:09:43 +00:00
permalink: "/news/2025-12-10-Auto-Model-Selection-in-GitHub-Copilot-Now-Available-in-Visual-Studio-Code.html"
categories: ["AI", "GitHub Copilot"]
tags: ["AI", "AI Models", "Auto Model Selection", "Cloud IDE", "Copilot", "Copilot Plans", "Developer Tools", "GitHub Copilot", "GPT 4.1", "GPT 5.1 Codex Max", "Haiku 4.5", "Model Multiplier", "News", "Premium Requests", "Sonnet 4.5", "VS Code"]
tags_normalized: ["ai", "ai models", "auto model selection", "cloud ide", "copilot", "copilot plans", "developer tools", "github copilot", "gpt 4dot1", "gpt 5dot1 codex max", "haiku 4dot5", "model multiplier", "news", "premium requests", "sonnet 4dot5", "vs code"]
---

Allison details the release of auto model selection for GitHub Copilot in Visual Studio Code, providing developers dynamic access to high-quality AI models with transparent billing and customization options.<!--excerpt_end-->

# Auto Model Selection in GitHub Copilot Now Available in Visual Studio Code

GitHub Copilot's auto model selection feature is now generally available in Visual Studio Code for all Copilot plans. With the "auto" setting, Copilot dynamically selects a model for code completions based on real-time availability, helping developers stay productive even during periods of high demand or rate limiting.

## How Auto Model Selection Works

- **Dynamic Routing:** Auto chooses the most available model at the moment, automatically switching between options such as GPT-5.1-Codex-Max, GPT-5 mini, GPT-4.1, Sonnet 4.5, and Haiku 4.5, depending on your plan and administrative policies.
- **Transparency:** Developers can see which model was used by hovering over the model response within Visual Studio Code.
- **Flexible Control:** You can toggle between auto and explicit model selections at any time.
- **Policy Compliance:** Auto respects all user and admin model settings.

## Premium Request Billing

When using auto model selection, premium requests are billed according to the selected model's multiplier value (0x to 1x). All paid subscribers receive a 10% discount when auto is active. For instance, if auto selects a model with a 1x multiplier, only 0.9 premium requests are consumed instead of 1. See [model multipliers](https://docs.github.com/copilot/concepts/billing/copilot-requests#model-multipliers) for more details.

## Roadmap and Future Enhancements

Auto currently routes requests to the best available models, but upcoming updates will make it smarter, allowing Copilot to select models matching the complexity of your coding task.

## Community Engagement

Questions or feedback? Developers are encouraged to join the [Copilot Conversations Community discussion](https://github.com/orgs/community/discussions/categories/copilot-conversations).

---

For more details, visit the [official announcement](https://github.blog/changelog/2025-12-10-auto-model-selection-is-generally-available-in-github-copilot-in-visual-studio-code).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-10-auto-model-selection-is-generally-available-in-github-copilot-in-visual-studio-code)
