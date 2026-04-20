---
feed_name: The GitHub Blog
primary_section: github-copilot
external_url: https://github.blog/news-insights/company-news/changes-to-github-copilot-individual-plans/
section_names:
- ai
- github-copilot
date: 2026-04-20 18:15:28 +00:00
tags:
- Agentic Workflows
- AI
- Auto Model Selection
- Claude Opus 4.7
- Company News
- Copilot CLI
- Copilot Pro
- Copilot Pro+
- Copilot Student
- GitHub Copilot
- Model Availability
- Model Multipliers
- News
- News & Insights
- Parallel Workflows
- Plan Mode
- Refund Policy
- Service Reliability
- Session Limits
- Subscription Changes
- Token Consumption
- Usage Limits
- VS Code
- Weekly Limits
title: Changes to GitHub Copilot Individual plans
author: Joe Binder
---

Joe Binder explains upcoming changes to GitHub Copilot Individual plans, including paused new sign-ups, tighter token-based usage limits, and adjustments to model availability, with guidance on how to avoid hitting limits in VS Code and Copilot CLI.<!--excerpt_end-->

# Changes to GitHub Copilot Individual plans

Today GitHub is making changes to Copilot’s Individual plans to protect reliability and provide a more predictable experience for existing customers. The changes cover new sign-ups, usage limits, and model availability.

## What’s changing

1. **New sign-ups are paused** for:
   - GitHub Copilot Pro
   - GitHub Copilot Pro+
   - GitHub Copilot Student

2. **Usage limits are being tightened** for individual plans.
   - Pro+ offers **more than 5×** the limits of Pro.
   - Usage limits are now displayed in:
     - **VS Code**
     - **Copilot CLI**

3. **Opus models availability is changing**.
   - **Opus models are no longer available in Pro plans**.
   - **Opus 4.7** remains available in **Pro+**.
   - As previously announced in the changelog, **Opus 4.5 and Opus 4.6 will be removed from Pro+**.

## Refund and cancellation note

If these changes don’t work for you, you can cancel your Pro or Pro+ subscription and you will not be charged for April usage. For refunds, contact GitHub Support **between April 20 and May 20**.

## How usage limits work in GitHub Copilot

GitHub Copilot currently has **two** usage limits:

- **Session limits**
- **Weekly (7 day) limits**

Both are affected by:

- **Token consumption**
- **The model’s multiplier**

### Session limits

- Designed to prevent overload during peak usage.
- Intended so most users aren’t impacted.
- If you hit a session limit, you must wait until the usage window resets to resume using Copilot.
- GitHub expects these limits to be adjusted over time to balance reliability and demand.

### Weekly limits

- A cap on the total number of tokens you can consume in a week.
- Introduced to control costs from **parallelized, long-trajectory requests** that can run for extended periods.

If you hit a weekly limit:

- If you still have premium requests remaining, you can continue using Copilot with **Auto** model selection.
- **Model choice is reenabled** when the weekly period resets.
- Pro users can **upgrade to Pro+** to increase weekly limits.

### Premium requests vs usage limits

- **Premium requests** determine:
  - Which models you can access
  - How many requests you can make
- **Usage limits** are token-based guardrails that cap how many tokens you can consume in a time window.

It’s possible to have premium requests remaining and still hit a usage limit.

## Avoiding surprise limits and improving transparency

Starting today, **VS Code** and **Copilot CLI** display available usage as you approach a limit.

![Screenshot of a usage limit being hit in VS Code. A message appears that says 'You've used over 75% of your weekly usage limit. Your limit resets on Apr 27 at 8:00 PM.'](https://github.blog/wp-content/uploads/2026/04/Screenshot-2026-04-20-at-2.05.12-PM.png?resize=1024%2C898)

Usage limits in VS Code

![A screenshot of a usage limit being hit in GitHub Copilot CLI. A message appears that says '! You've used over 75% of your weekly usage limit. Your limit resets on Apr 24 at 3 PM.'](https://github.blog/wp-content/uploads/2026/04/image-20.png?resize=1024%2C662)

Usage limits in Copilot CLI

### Practical ways to reduce the chance of hitting limits

- Use a model with **a smaller multiplier** for simpler tasks.
- Consider **upgrading to Pro+** if you’re on Pro (over **5×** higher limits).
- Use **plan mode** to improve task efficiency:
  - VS Code: https://code.visualstudio.com/docs/copilot/concepts/agents#_planning
  - Copilot CLI: https://docs.github.com/en/copilot/how-tos/copilot-cli/cli-best-practices#2-plan-before-you-code
- **Reduce parallel workflows**. Tools such as `/fleet` increase token consumption and should be used sparingly when nearing limits.

## Why GitHub is doing this

GitHub attributes the changes to increased demand driven by **agentic workflows**:

- Long-running, parallelized sessions consume significantly more compute than the original plan structure expected.
- More customers are hitting limits designed to maintain reliability.
- In some cases, a handful of requests can incur costs that exceed the plan price.

These measures are presented as a way to maintain service quality for existing users while GitHub works on a more sustainable long-term solution.


[Read the entire article](https://github.blog/news-insights/company-news/changes-to-github-copilot-individual-plans/)

