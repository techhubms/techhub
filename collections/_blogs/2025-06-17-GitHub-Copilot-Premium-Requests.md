---
external_url: https://devopsjournal.io/blog/2025/06/17/Copilot-premium-requests
tags:
- Agent Mode
- AI
- Billing
- Blogs
- Budgets
- Coding Agent
- Copilot Business
- Copilot Chat
- Copilot Enterprise
- Copilot Extensions
- Copilot Spaces
- CSV Export
- Gemini 2.0 Flash
- Generative AI
- GitHub Copilot
- GitHub Pages
- GitHub Spark
- GPT 4.1
- GPT 4.5
- GPT 4o
- JetBrains IDEs
- LLM Cost
- Model Multipliers
- Premium Requests
- Pull Request Code Review
- Usage Reporting
- VS
- VS Code
feed_name: Rob Bos' Blog
section_names:
- ai
- github-copilot
author: Rob Bos
date: 2025-06-17 00:00:00 +00:00
title: GitHub Copilot Premium Requests
primary_section: github-copilot
---
Rob Bos explains how GitHub Copilot “Premium Requests” work, what features and model choices consume premium requests, what each plan includes, and how to use budgets and usage reports to avoid surprise charges.<!--excerpt_end-->

## GitHub Copilot Premium Requests

Date posted: **17 Jun 2025**

Some important changes are happening: you may need to start paying based on how much generative AI you use with GitHub Copilot. The goal is to make the cost of LLM requests visible to end users and help avoid overspending.

## What are Premium Requests?

GitHub Copilot Premium Requests will be enforced starting **June 18**.

Documentation:
- Premium Requests overview: https://docs.github.com/en/copilot/managing-copilot/monitoring-usage-and-entitlements/about-premium-requests
- Understanding and managing requests: https://docs.github.com/en/copilot/managing-copilot/understanding-and-managing-copilot-usage/understanding-and-managing-requests-in-copilot

Copilot Premium Requests are requests made to **any model that is not the default** (at the time of writing: **GPT-4o** and **GPT-4.1**).

Different models and features have different **multipliers**:
- Some actions cost **1 premium request**.
- Some models cost more (example given: **GPT-4.5 is x50**).
- Some models cost less (example given: **Gemini 2.0 Flash is 0.25x**).

![Screenshot of the different multipliers per model](/images/2025/20250617/20250617_multipliers.png)

Related video:
- https://github-copilot.xebia.ms/detail?videoId=43

[![Screenshot of the GitHub Copilot Premium Requests documentation](/images/2025/20250617/20250617_video.png)](https://github-copilot.xebia.ms/detail?videoId=43)

## What consumes a premium request?

Main features that consume premium requests:

- **Chatting with a non-default model**: 1 premium request per chat “turn” (every question and answer is a turn)
- **Every step in the Coding Agent**: 1 premium request per step
  - The agent may take multiple steps, consuming multiple requests
  - The author notes a hoped-for future control like a “max requests” setting to reduce overspending risk
- **Requesting a Code Review in a Pull Request**: 1 premium request
- **Agent Mode in an editor**: 1 premium request per user-initiated request
- **Copilot Coding Agent**: can use multiple premium requests as it sees fit
- **Copilot Spaces and Extensions** also consume premium requests per GitHub docs, but:
  - It is unclear how many requests they consume or for what
  - The author speculates a chat turn against a Copilot Space likely consumes a premium request each time
  - It’s unclear whether extension usage applies to:
    - local extensions (example: the **@Azure** extension that runs locally in VS Code)
    - remote extensions installed as GitHub Apps in an organization

## Free plan

If you are on the **Copilot Free** plan, even the **base model** will consume a premium request when using the **Chat** feature.

## Paid plans: included requests and overages

On paid plans, you get a certain number of premium requests per month.

If you go over, you are charged **$0.04 per premium request**.

Example cost impact noted by the author:
- A single request against a **50x model** could cost **$2.00** (50 × $0.04).

See the full multiplier table here:
- https://docs.github.com/en/copilot/managing-copilot/monitoring-usage-and-entitlements/about-premium-requests

### Included premium requests per plan

| Plan | Premium requests | Copilot Chat in IDEs | Code completion |
| --- | --- | --- | --- |
| Copilot Free | 50 per month | 50 messages per month | 2000 completions per month |
| Copilot Pro | 300 per month | Unlimited with base model | Unlimited |
| Copilot Pro+ | 1500 per month | Unlimited with base model | Unlimited |
| Copilot Business | 300 per user per month | Unlimited with base model | Unlimited |
| Copilot Enterprise | 1000 per user per month | Unlimited with base model | Unlimited |

## Preventing overspending with budgets

You can configure a spending budget for premium requests:
- Documentation: https://docs.github.com/en/billing/managing-your-billing/preventing-overspending

Notes from the post:
- Default budget is **$0.00**.
- You can set it to any amount.
- If the budget is reached, premium requests are **blocked until the next month**.

![Screenshot of Copilot premium request budget setting](/images/2025/20250617/20250618_copilotbudget.png)

Budget visibility is also available in the new Coding Agent session panel:

![Screenshot showing that coding agent ran for 14 minutes and consumed 54 premium request](/images/2025/20250617/20250618_codingagent.png)

## Finding your usage information as a user

The author notes UI changes that expose model multipliers so users can decide which model to use.

![Screenshot of the different multipliers per model](/images/2025/20250617/20250618_copilotmodelselection.png)

This is visible in:
- Visual Studio Code
- JetBrains IDEs
- Visual Studio

The IDEs also show account setup and current usage:

![Screenshot of the setup for your account in Visual Studio Code](/images/2025/20250617/20250618_copilotoverview.png)

Visual Studio shows usage via the Copilot icon in the top right corner:

![Screenshot of the amount of premium requests used in Visual Studio](/images/2025/20250617/20250618_visualstudioused.png)

You can also view usage by going to:
- User → Settings → Billing: https://github.com/settings/billing

![Screenshot of user usage information showing the cost of your Copilot and Premium Requests usage in the current period](/images/2025/20250617/20250618_userusage.png)

## Analyzing Premium Requests usage for an organization/enterprise

The post describes a small tool the author built to analyze the Copilot Premium Requests CSV report (since there is **no API yet**).

Approach:
- A single page application (SPA) built with **GitHub Spark** and **GitHub Copilot Coding Agent**
- Displays an overview of the Premium Requests CSV you can download

Notes:
- Can be hosted on **GitHub Pages**
- Upload the CSV from enterprise export:
  - Billing and Licenses → Usage → Export dropdown (top right)

Example screenshots:

![Screenshot of top bar information](/images/2025/20250617/20250617_01.png)

![Screenshot of usage statistics over time](/images/2025/20250617/20250617_02.png)

![Screenshot of model usage in bars](/images/2025/20250617/20250617_03.png)

Repo:
- https://github.com/devops-actions/github-copilot-premium-reqs-usage

It’s open source; the author invites contributions and feature requests.

[Read the entire article](https://devopsjournal.io/blog/2025/06/17/Copilot-premium-requests)

