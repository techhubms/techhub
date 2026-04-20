---
date: 2026-04-17 22:32:41 +00:00
feed_name: The GitHub Blog
external_url: https://github.blog/changelog/2026-04-17-github-copilot-cli-now-supports-copilot-auto-model-selection
author: Allison
title: GitHub Copilot CLI now supports Copilot auto model selection
section_names:
- ai
- github-copilot
tags:
- Administrator Controls
- AI
- Auto Model Selection
- Copilot
- Copilot Billing
- Copilot CLI
- GitHub Copilot
- GPT 5.3 Codex
- GPT 5.4
- Haiku 4.5
- Model Multipliers
- Model Policies
- Model Routing
- News
- Premium Requests
- Rate Limits
- Sonnet 4.6
primary_section: github-copilot
---

Allison announces general availability of Copilot auto model selection in GitHub Copilot CLI, explaining how the CLI dynamically routes requests to different models, shows which model was used, respects admin policies, and how premium request billing works when auto is enabled.<!--excerpt_end-->

# GitHub Copilot CLI now supports Copilot auto model selection

[Copilot auto model selection](https://docs.github.com/copilot/concepts/auto-model-selection) is now generally available in GitHub Copilot CLI for all Copilot plans.

With **auto**, Copilot chooses the most efficient model on your behalf.

## How it works

Auto is **dynamic**, aiming to provide reliable access to preferred models while **mitigating rate limits**.

It routes to models such as:

- GPT-5.4
- GPT-5.3-Codex
- Sonnet 4.6
- Haiku 4.5

Which models **auto** can route to will change over time, based on your plan and policies.

Key behaviors:

- **Transparency**: You can see which model was used directly in the Copilot CLI.
- **Stay in control**: You can switch between **auto** and a **specific model** at any time.
- **Respects your policies**: Auto honors all **administrator model settings**.

## Premium request use

Premium request usage for **auto** is billed based on the model it selects.

Currently, auto is limited to [models with 0x to 1x multipliers](https://docs.github.com/copilot/concepts/billing/copilot-requests#model-multipliers) (including the models listed above).

Discount:

- All paid subscribers get a **10% discount** on the model multiplier when using auto.
- Example: if auto uses a model with a **1x** multiplier, it draws down **0.9 premium requests** instead of **1**.

## Discussion

Join the discussion in the GitHub Community announcements category:

- [GitHub Community announcements](https://github.com/orgs/community/discussions/categories/announcements)


[Read the entire article](https://github.blog/changelog/2026-04-17-github-copilot-cli-now-supports-copilot-auto-model-selection)

