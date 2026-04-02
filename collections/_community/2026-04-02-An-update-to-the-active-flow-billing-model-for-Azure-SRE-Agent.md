---
date: 2026-04-02 22:53:51 +00:00
section_names:
- ai
- azure
- devops
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/an-update-to-the-active-flow-billing-model-for-azure-sre-agent/ba-p/4507866
title: An update to the active flow billing model for Azure SRE Agent
primary_section: ai
author: Mayunk_Jain
feed_name: Microsoft Tech Community
tags:
- AAU
- Active Flow Billing
- Agent Consumption Settings
- AI
- AI Model Providers
- Alert Response
- Always On Flow Pricing
- Anthropic
- Autonomous Actions
- Azure
- Azure Agent Unit
- Azure SRE Agent
- Chat
- Community
- DevOps
- Incident Investigation
- Reliability Operations
- Spending Controls
- SRE
- Token Based Billing
- Usage Limits
---

Mayunk_Jain explains how Azure SRE Agent’s “active flow” charges are changing from time-based billing to token-based billing on April 15, 2026, including what stays the same (AAUs and always-on pricing) and where to find current per-model rates and usage controls.<!--excerpt_end-->

# An update to the active flow billing model for Azure SRE Agent

Active flow billing is changing from **time-based** to **token-based** usage. You’ll be billed based on the **tokens consumed** when Azure SRE Agent is actively doing work. Each model provider has its own published rate (**AAUs per million tokens**).

Earlier today, Microsoft announced that **Azure SRE Agent now supports multiple AI model providers**, starting with **Anthropic**.

This change is **effective April 15, 2026**.

## At a glance

### What’s changing

- **Active flow billing moves from time-based to token-based usage.** You’ll be billed based on the tokens consumed when SRE Agent is actively doing work (for example, investigating an incident, responding to an alert, or helping in chat).
- **Each model provider has its own published rate** (AAUs per million tokens), so you can choose the provider that fits your scenario and budget.

### What stays the same

- **Azure Agent Unit (AAU)** remains the billing unit.
- **Always-on flow pricing is unchanged:** 4 AAUs per agent-hour.
- Your bill continues to have **two components**:
  - a fixed **always-on** component
  - a variable **active flow** component

### What you need to do

- For most customers, **no action is required**. Existing agents continue running.
- For current AAU rates by model provider and example consumption scenarios, see the pricing documentation:
  - [https://aka.ms/sreagent/pricing](https://aka.ms/sreagent/pricing)

## Why Microsoft is making this change

Reliability operations tasks vary widely (for example, a quick health check vs. a multi-step investigation across logs, deployments, and metrics). With support for multiple model providers, **token consumption varies by provider and by task complexity**.

Moving to token-based active flow billing is intended to make costs align more directly with the work performed and make usage easier to understand as model options expand.

## How token-based active flow helps

### More predictable costs for common tasks

- Simple interactions typically use fewer tokens.
- More complex investigations use more tokens.
- Token-based billing is positioned as making the relationship between task complexity and active usage clearer.

### More flexibility as Azure SRE Agent adds models

- You choose the provider; the service selects the best model for the job.
- As model providers release newer models and Azure SRE Agent adopts them, Microsoft will publish updated AAU-per-token rates.
- Current rates are published in the product docs:
  - [https://sre.azure.com/docs/reference/pricing-billing#aau-rates-by-model](https://sre.azure.com/docs/reference/pricing-billing#aau-rates-by-model)

### Spending controls stay in place

You can still set a monthly AAU allocation limit in the portal:

- **Settings → Agent consumption**

When you reach your **active flow** limit:

- your agent continues to run, but
- it **pauses chat and autonomous actions** until the next month.

You can adjust the limit at any time.

## Next steps

- For most customers, this requires **no action**.
- **Always-on billing is unchanged**, existing agents continue running, and the AAU meter remains the same.
- The change affects only how **active flow usage** is measured and calculated.

For details including AAU rates per model, example consumption scenarios (light/medium/heavy workloads), and spending limit guidance, see:

- [https://aka.ms/sreagent/pricing](https://aka.ms/sreagent/pricing)

> **NOTE:** The pricing section in product documentation is the authoritative source for current rates until the pricing page is updated.

Questions or feedback: use the **Feedback & issues** link in the SRE Agent portal or reach out through the Azure SRE Agent community.

## Additional resources

- Product documentation: [https://aka.ms/sreagent/docs](https://aka.ms/sreagent/docs)
- Self-paced hands-on labs: [https://aka.ms/sreagent/lab](https://aka.ms/sreagent/lab)
- Technical videos and demos: [https://aka.ms/sreagent/youtube](https://aka.ms/sreagent/youtube)
- Azure SRE Agent home page: [https://www.azure.com/sreagent](https://www.azure.com/sreagent)
- Azure SRE Agent on X: [https://x.com/azuresreagent](https://x.com/azuresreagent)

Updated Apr 02, 2026

Version 2.0


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/an-update-to-the-active-flow-billing-model-for-azure-sre-agent/ba-p/4507866)

