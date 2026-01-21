---
external_url: https://devblogs.microsoft.com/visualstudio/introducing-copilot-auto-model-selection-preview/
title: Introducing Copilot Auto Model Selection (Preview)
author: Rhea Patel, Nhu Do
feed_name: Microsoft VisualStudio Blog
date: 2025-11-12 17:38:43 +00:00
tags:
- AI Development
- AI Integration
- AI Models
- Auto
- Auto Model Selection
- Claude Sonnet 4.5
- Copilot
- Copilot Chat
- GPT 4.1
- GPT 5
- Model Multiplier
- Models
- Premium Requests
- Software Engineering
- VS
section_names:
- ai
- github-copilot
---
Rhea Patel and Nhu Do showcase GitHub Copilot's new auto model selection feature, detailing how it improves Copilot Chat performance and flexibility for developers.<!--excerpt_end-->

# Introducing Copilot Auto Model Selection (Preview)

**Authors:** Rhea Patel, Nhu Do

GitHub Copilot Chat introduces a new preview feature called “auto” model selection. With this change, users no longer need to pick a specific language model for their Copilot Chat sessions. Instead, Copilot automatically selects the most suitable model based on current capacity and performance, providing faster responses, reducing the likelihood of rate limiting, and offering a 10% discount on premium requests for paid users.

## How Auto Model Selection Works

- **Automatic Model Choice:** Copilot's auto mode evaluates available models for each request and selects the optimal one, factoring in current capacity and performance statistics.
- **Supported Models:** Auto can choose from various advanced models, including GPT-5, GPT-5 mini, GPT-4.1, Claude Sonnet 4.5, Haiku 4.5, and possibly others, unless an organization specifically disables access.
- **Consistency Per Session:** Once a model is chosen for a chat session, that model remains in use throughout the session. Future updates aim to further refine this and allow switching based on task complexity.
- **Paid User Benefits:** Paid Copilot users primarily benefit from auto using Claude Sonnet 4.5 and receive a 10% discount on premium requests, as auto applies a model multiplier (e.g., 0.9x for Sonnet 4.5). If a paid user runs out of premium requests, auto falls back to a 0x model, such as GPT-4.1, so chat remains uninterrupted.
- **Model Transparency:** Users can check which model and multiplier Copilot used by hovering over a chat response.

## What’s Next for Copilot Auto Selection

- **Dynamic Task-Based Switching:** Plans include automatically toggling between large and small models as needed for specific tasks, optimizing performance and request usage.
- **Expanded Model Support:** More language models will be integrated into the auto selection pool.
- **Free Plan Access:** Users on a free plan will have the chance to access newer models via auto.
- **UI Improvements:** Enhancements to the model dropdown will make it clearer which models and discounts apply.

For additional documentation, see:

- [GitHub: Auto Model Selection](https://docs.github.com/en/copilot/concepts/auto-model-selection)
- [GitHub: Configure Model Access](https://docs.github.com/en/copilot/how-tos/use-ai-models/configure-access-to-ai-models)
- [GitHub: Model Multiplier](https://docs.github.com/en/copilot/concepts/billing/copilot-requests#model-multipliers)

Thanks for exploring this preview feature!

This post appeared first on "Microsoft VisualStudio Blog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/introducing-copilot-auto-model-selection-preview/)
