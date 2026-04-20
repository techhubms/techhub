---
author: Allison
tags:
- AI
- Auto Mode
- Auto Model Selection
- Concurrency
- Copilot
- Copilot Pro+
- GitHub Copilot
- Infrastructure Capacity
- Model Capacity
- Model Family
- Model Retirement
- News
- Opus 4.6
- Opus 4.6 Fast
- Plan Upgrade
- Rate Limiting
- Retired
- Service Reliability
- Usage Limits
primary_section: github-copilot
feed_name: The GitHub Blog
section_names:
- ai
- github-copilot
date: 2026-04-10 22:56:46 +00:00
external_url: https://github.blog/changelog/2026-04-10-enforcing-new-limits-and-retiring-opus-4-6-fast-from-copilot-pro
title: Enforcing new limits and retiring Opus 4.6 Fast from Copilot Pro+
---

Allison explains upcoming GitHub Copilot limit changes aimed at improving service reliability, including rate limiting behavior, guidance to switch models or use Auto mode, and the retirement of Opus 4.6 Fast for Copilot Pro+ users.<!--excerpt_end-->

# Enforcing new limits and retiring Opus 4.6 Fast from Copilot Pro+

As GitHub Copilot continues to rapidly grow, GitHub is seeing increased patterns of high concurrency and intense usage. While some of this can be driven by legitimate workflows, it can place significant strain on shared infrastructure and operating resources.

To keep Copilot fast and reliable for everyone, GitHub is updating limits to better balance capacity. These changes will roll out over the next few weeks.

## What’s changing

Users may see two types of limits:

- **Limits for overall service reliability**
- **Limits for specific models or model family capacity**

## What this means for you

- **If you hit a service reliability limit**: you’ll need to wait until your current session resets. This will be shown in the error experience when you’re rate limited.
- **If you hit a usage limit for a specific model or model family**: you can switch to an alternative model or use Auto mode.

Reference: Auto mode documentation: https://docs.github.com/copilot/concepts/auto-model-selection

## Recommendations

GitHub recommends:

- Distributing requests more evenly over time when possible, instead of sending large, concentrated waves.
- Upgrading your plan for higher limits if you need more capacity: https://github.com/features/copilot/plans

GitHub acknowledges that limits can be frustrating and says it’s exploring ways to increase capacity. More details are available in the rate limiting documentation: https://docs.github.com/copilot/concepts/rate-limits

## Model offering change: Opus 4.6 Fast retirement

To further improve service reliability, GitHub is streamlining model offerings and focusing resources on the models most used by customers.

As a first step:

- **Opus 4.6 Fast is being retired for Copilot Pro+ users, beginning today.**
- GitHub recommends using **Opus 4.6** as an alternative model with similar capabilities.


[Read the entire article](https://github.blog/changelog/2026-04-10-enforcing-new-limits-and-retiring-opus-4-6-fast-from-copilot-pro)

