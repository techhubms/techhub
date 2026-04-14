---
tags:
- Admin Settings
- AI
- Anthropic Claude
- Claude Opus 4.6
- Claude Sonnet 4.6
- Compliance
- Copilot
- Copilot Agent Mode
- Copilot Chat
- Copilot CLI
- Copilot Code Review
- Data Residency
- Enterprise Cloud
- FedRAMP Moderate
- GitHub Copilot
- GPT 5.4
- Model Endpoints
- Model Multiplier
- News
- OpenAI Models
- Organization Policy
- Platform Governance
- Premium Requests
- Pull Request Summaries
- Regional Inference
- Security
date: 2026-04-13 23:03:01 +00:00
author: Allison
section_names:
- ai
- github-copilot
- security
external_url: https://github.blog/changelog/2026-04-13-copilot-data-residency-in-us-eu-and-fedramp-compliance-now-available
feed_name: The GitHub Blog
title: GitHub Copilot data residency in US + EU and FedRAMP compliance now available
primary_section: github-copilot
---

Allison announces GitHub Copilot data residency for US and EU regions plus FedRAMP Moderate support, outlining what features are covered, which models are available, the pricing uplift for compliant endpoints, and how enterprise/org admins can enable the policies.<!--excerpt_end-->

## Overview

GitHub Copilot now supports **data residency** for the **US** and **EU** regions. This keeps **inference processing** and associated data within your selected geography. For **US government customers**, Copilot’s model hosts and infrastructure meet **FedRAMP Moderate** authorization standards.

Source: https://github.blog/changelog/2026-04-13-copilot-data-residency-in-us-eu-and-fedramp-compliance-now-available

## What’s included

All generally available Copilot features are supported under data residency and FedRAMP-enabled policies, including:

- Agent mode
- Inline suggestions
- Chat
- Copilot cloud agent
- Code review
- Pull request summaries
- Copilot CLI

Every feature uses **data-resident, compliance-certified model endpoints** within the designated region.

Related feature list: https://docs.github.com/copilot/get-started/features

## Model availability

At launch, a broad model set is available across both **OpenAI** and **Anthropic**, including:

- GPT-5.4
- Claude Sonnet 4.6
- Claude Opus 4.6

For the full model-by-region matrix and details:

- Data residency docs: https://docs.github.com/enterprise-cloud@latest/admin/data-residency/github-copilot-with-data-residency
- FedRAMP docs: https://docs.github.com/copilot/concepts/fedramp-models

### Not yet supported

- **Gemini models** are not currently supported because **GCP** does not yet offer **data-resident inference endpoints**.
- Newly released models may take additional time to become available in data-resident regions.

## Pricing impact

Data-resident and FedRAMP requests have a **10% increase in the model multiplier**, reflecting provider costs for regional and compliance-certified endpoints.

Example:

- If a model normally costs **1 premium request**, it will cost **1.1 premium requests** under data residency.

## Getting started (admin controls)

**Enterprise and organization admins** can enable data residency and FedRAMP policies from **Copilot settings** to restrict their enterprise/org to models that are:

- Data-resident, or
- FedRAMP compliant

Notes:

- Policies are **off by default**.
- Admins must explicitly opt in and understand the **pricing implications**.

## Regions and roadmap

- Available now: **US** and **EU**
- Planned later in 2026: additional **Proxima regions** (for example **Japan** and **Australia**)


[Read the entire article](https://github.blog/changelog/2026-04-13-copilot-data-residency-in-us-eu-and-fedramp-compliance-now-available)

