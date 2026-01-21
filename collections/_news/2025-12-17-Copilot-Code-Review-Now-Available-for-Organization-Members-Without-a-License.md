---
external_url: https://github.blog/changelog/2025-12-17-copilot-code-review-now-available-for-organization-members-without-a-license
title: Copilot Code Review Now Available for Organization Members Without a License
author: Allison
feed_name: The GitHub Blog
date: 2025-12-17 15:30:47 +00:00
tags:
- Admin Policy
- Analytics Dashboard
- Billing
- Business Plans
- Code Review Automation
- Copilot
- Copilot Code Review
- Enterprise Plans
- License Management
- Open Source
- Organization Repository
- Premium Requests
- Pull Requests
section_names:
- ai
- github-copilot
---
Allison details how Copilot code review can now be applied to pull requests from organization members without a Copilot license, streamlining AI-powered reviews and billing for enterprises.<!--excerpt_end-->

# Copilot Code Review for Unlicensed Organization Members

**Author: Allison**

Organizations using Copilot Enterprise or Copilot Business plans now have the option to enable Copilot code review for all pull requests, including those from contributors without a Copilot license. This update is rolling out over the next few days and provides:

- **Comprehensive AI coverage** across all organization repositories, regardless of contributors' license status.
- **Seamless billing**, with unlicensed users' Copilot code review billed as premium requests to the organization, removing the need for individual license management.
- **Greater administrative control**, requiring an explicit opt-in to enable this capability.

## How It Works

- Previously, only licensed users could receive Copilot code review suggestions. Now, once enabled, all pull requests are eligible for Copilot-powered analysis.
- Reviews on pull requests from unlicensed contributors consume premium requests, billed to the org; licensed users' usage continues as before.

## Enabling the Feature

Admins must:

1. Enable **Premium requests paid usage** so anyone can use premium requests for Copilot code review ([see how](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/manage-and-track-spending/manage-request-allowances#setting-a-policy-for-paid-usage)).
2. Enable the **Copilot code review** policy and toggle the option allowing members without a license to use Copilot code review ([read more](https://docs.github.com/enterprise-cloud@latest/copilot/concepts/agents/code-review)).

Screenshots in the original announcement show how to set these options in the admin dashboard.

## Monitoring and Budgeting

- Track premium request usage and costs using the [analytics dashboard](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/manage-and-track-spending/monitor-premium-requests#viewing-detailed-analytics-of-your-usage) on your organization's Billing & Licensing page.
- Usage can be filtered and grouped to distinguish between licensed and unlicensed contributors.
- It is recommended to [set up a premium request budget](https://docs.github.com/enterprise-cloud@latest/billing/tutorials/set-up-budgets) to control and predict charges.

## Summary

- Organizations now have more flexibility and coverage when using Copilot code review, benefiting enterprise teams and open source projects.
- For further information, refer to the [Copilot code review documentation](https://docs.github.com/enterprise-cloud@latest/copilot/concepts/agents/code-review) and [community discussion](https://github.com/orgs/community/discussions/178962).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-17-copilot-code-review-now-available-for-organization-members-without-a-license)
