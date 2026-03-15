---
external_url: https://r-vm.com/limited-to-300-free-premium-requests-by-your-org-heres-an-expensive-workaround.html
tags:
- AI
- Azure
- Azure Metered Billing
- Azure Subscription Billing
- Blogs
- Budget
- Claude 3.5 Sonnet
- Claude Opus 4
- Copilot Chat
- Copilot Code Review
- Copilot Coding Agent
- Copilot Extensions
- Copilot Premium Requests
- Copilot Spaces
- Cost Analysis
- Developer Tooling Budgets
- Developer Tools
- Development Costs
- DevOps
- Enterprise
- Feature Limits
- Gemini 2.0 Flash
- GitHub Copilot
- GitHub Copilot Business
- GitHub Copilot Enterprise
- GitHub Organizations
- GPT 4.1
- GPT 4o
- GPT 4o Mini
- Model Multipliers
- MPN Subscription
- Organizations
- Premium Features
- Pricing
- Productivity
- Spark
- Usage Billed To
- Workarounds
feed_name: Reinier van Maanen's blog
section_names:
- ai
- azure
- devops
- github-copilot
author: Reinier van Maanen
date: 2025-07-26 00:00:00 +00:00
title: Limited to 300 free premium requests by your org? Here’s an expensive workaround!
primary_section: github-copilot
---
Reinier van Maanen explains what GitHub Copilot “premium requests” are, how model multipliers burn through the 300/month allowance on Business/Enterprise plans, and an expensive workaround (a second, personal org) including how Azure billing integration may apply for overages.<!--excerpt_end-->

## Overview

When a GitHub organization hits (or disables) GitHub Copilot premium request usage, you can technically work around it by creating a second organization that you control and buying GitHub Copilot Business for it. This can “stack” your included premium request allowances—but it often ends up costing more than simply letting the company pay overages.

## What are premium requests?

Premium requests are Copilot interactions that use advanced AI models beyond the included **GPT-4o** and **GPT-4.1**.

What counts as premium requests:

- **Copilot Chat**: 1 premium request per user prompt, multiplied by the model’s rate
- **Copilot Coding Agent**: 1 premium request per session (when creating/modifying pull requests)
- **Agent Mode in Copilot Chat**: 1 premium request per user prompt, multiplied by the model’s rate
- **Copilot Code Review**: 1 premium request each time Copilot posts comments to a pull request
- **Copilot Extensions**: 1 premium request per user prompt, multiplied by the model’s rate
- **Copilot Spaces**: 1 premium request per user prompt, multiplied by the model’s rate
- **Spark**: 4 premium requests per prompt (fixed rate)

## Understanding model multipliers

On **GitHub Copilot Business** and **Enterprise**, you get **300 free premium requests per user per month**.

Different models consume premium requests at different multipliers:

- **GPT-4o and GPT-4.1**: 0× (free on paid plans)
- **Gemini 2.0 Flash**: 0.25×
- **Claude 3.5 Sonnet**: 1×
- **GPT-4o mini**: 1×
- **Claude Opus 4**: 10×

Implication examples:

- 300 premium requests could yield about **1,200 interactions** with Gemini 2.0 Flash (0.25×)
- 300 premium requests could yield about **30 interactions** with Claude Opus 4 (10×)

## What happens when an organization hits the limit?

After the 300-request allowance is used up, options include:

1. Wait until the next month (resets on the 1st at 00:00:00 UTC)
2. Set a budget for additional premium requests at **$0.04 USD per request**
3. Disable premium requests (forcing users to stick with GPT-4o/GPT-4.1)
4. Upgrade to higher allowances (Enterprise plans)

A key point: an organization can disable premium requests entirely, even if developers want to pay for overages.

## Workaround: create a personal organization and “stack” allowances

If your corporate org has premium requests disabled, the workaround is to create your own paid org and bill premium usage there.

![Creating a personal GitHub organization to bypass premium request](/assets/github-copilot-personal-org/image.png)

### Step 1: create your own Business organization

1. Create a new GitHub organization using the same GitHub account you use for corporate work
2. Purchase **GitHub Copilot Business** for that org (**$19/user/month minimum**)
3. Add yourself as the only member

You’ll also need a GitHub Team plan for the org (**$4/user/month**), bringing the minimum to **$23/month**.

### Step 2: stack allowances via “Usage billed to”

With two orgs providing Copilot licensing, you can effectively switch who is billed:

- Corporate organization: **300** free premium requests/month
- Personal organization: **300** free premium requests/month
- Total: **600** free premium requests/month

You can switch between organizations using the **“Usage billed to”** dropdown in GitHub Copilot.

## Azure billing integration (metered billing)

The post describes connecting the personal GitHub organization to **Azure metered billing**:

1. Set up Azure Subscription billing for the personal organization
2. Configure a budget for usage beyond the included free premium requests
3. Optionally use a **Microsoft Partner Network (MPN)** subscription for potential benefits/credits

After exhausting the second set of 300 requests, the author notes they were still blocked until changing a setting to bill usage to their name. The relevant setting is in Copilot Features:

- Copilot Features settings: https://github.com/settings/copilot/features

![Choose who pays for your Copilot premium requests usage. Included usage from their license applies.](/assets/github-copilot-personal-org/usage-billed-to.png)

> DISCLAIMER: The author is not fully convinced yet that premium requests beyond 600 are actually being treated as metered usage in Azure, noting their billing view still showed 0 and may be slow to update.

Billing overview referenced:

- https://github.com/settings/billing

## Cost analysis

### Monthly costs for the workaround

| Component | Cost | Notes |
| --- | --- | --- |
| GitHub Team Plan | $4/month | Required for organization |
| GitHub Copilot Business | $19/month | Minimum cost for Copilot |
| 600 Free Premium Requests | $0 | 300 from each organization |
| Additional Requests (via Azure) | $0.04/request | Only above 600 |
| Total minimum cost | $23/month | Just to get started |

### Example scenarios

**Light user (400 premium requests/month)**

- Corporate: 300 free requests
- Personal: 100 requests used
- Total cost: **$23/month**

**Heavy user (1,000 premium requests/month)**

- Corporate: 300 free requests
- Personal: 300 free requests
- Azure billing: 400 × $0.04 = **$16**
- Total cost: **$39/month**

Comparison: if the organization simply enabled overages instead of forcing the workaround:

- 1,000 requests → 700 overages × $0.04 = **$28/month** (cheaper than $39/month)

## Copilot Coding Agent tip (premium-request efficiency)

A practical usage tip: **GitHub Copilot Coding Agent uses only 1 premium request per session**, regardless of how much work it does.

A session begins when you assign Copilot to create or modify a pull request. A workflow to leverage this:

- Create detailed GitHub issues
- Assign them to Copilot Coding Agent
- Let it produce comprehensive pull requests
- Get broad changes (code, docs, tests) for a single premium request

## Better alternatives than the workaround

1. Advocate internally
   - Show productivity gains
   - Calculate ROI vs developer time
   - Propose a small-budget pilot
2. Optimize usage
   - Use GPT-4o / GPT-4.1 for most interactions (free on paid plans)
   - Reserve premium models for harder problems
   - Use Coding Agent strategically
3. Consider alternative AI tools
   - Tools without request limits
   - Multiple tools for different tasks
   - Local models for some work

## References

- Copilot Requests Documentation: https://docs.github.com/en/copilot/concepts/billing/copilot-requests
- Creating a New Organization from Scratch: https://docs.github.com/en/organizations/collaborating-with-groups-in-organizations/creating-a-new-organization-from-scratch
- Managing Payment and Billing Information: https://docs.github.com/en/billing/managing-your-billing/managing-your-payment-and-billing-information
- Connecting an Azure Subscription: https://docs.github.com/en/billing/managing-the-plan-for-your-github-account/connecting-an-azure-subscription
- GitHub Copilot Pricing: https://github.com/features/copilot#pricing
- Monitoring Your Copilot Usage and Entitlements: https://docs.github.com/en/copilot/managing-copilot/understanding-and-managing-copilot-usage/monitoring-your-copilot-usage-and-entitlements

*This post notes it was written with assistance from GitHub Copilot Chat.*

[Read the entire article](https://r-vm.com/limited-to-300-free-premium-requests-by-your-org-heres-an-expensive-workaround.html)

