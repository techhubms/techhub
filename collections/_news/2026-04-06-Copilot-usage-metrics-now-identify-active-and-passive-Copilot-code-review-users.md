---
primary_section: github-copilot
feed_name: The GitHub Blog
section_names:
- ai
- github-copilot
title: Copilot usage metrics now identify active and passive Copilot code review users
tags:
- 28 Day Reports
- Account Management
- Adoption Metrics
- AI
- CCR
- Copilot
- Copilot Code Review
- Copilot Metrics API
- Copilot Usage Metrics
- Daily Reports
- Engagement Tracking
- Enterprise Cloud
- Enterprise Management Tools
- GitHub Copilot
- News
- Organization Administration
- Pull Requests
- Repository Policy
- REST API
- Reviewer Assignment
- Suggestion Application
- Used Agent
- Used Copilot Code Review Active
- Used Copilot Code Review Passive
- Used Copilot Coding Agent
- User Level Reporting
external_url: https://github.blog/changelog/2026-04-06-copilot-usage-metrics-now-identify-active-and-passive-copilot-code-review-users
date: 2026-04-06 23:20:14 +00:00
author: Allison
---

Allison explains a GitHub Copilot usage-metrics update that distinguishes active vs passive Copilot code review activity in user-level reports, helping enterprise and org admins measure real engagement and adoption over daily and 28-day windows.<!--excerpt_end-->

# Copilot usage metrics now identify active and passive Copilot code review users

Copilot usage metrics now indicate which users have **Copilot code review (CCR)** activity, and whether that activity was **active** or **passive**. This is available to **enterprise and organization admins** in **daily** and **28-day** user-level reports.

## What changed

In the Copilot usage metrics API response, CCR usage is represented by two **user-level** boolean fields:

- `used_copilot_code_review_active`
  - Set to `true` when a user intentionally engaged with Copilot code review by:
    - Assigning Copilot as a reviewer on a pull request
    - Requesting Copilot reviews again
    - Applying a CCR suggestion
- `used_copilot_code_review_passive`
  - Set to `true` when Copilot code review automatically ran on a user’s pull request (via a **repo-level policy**), but the user did not interact with the review

If a user has both active and passive CCR events on the same day, the **active** signal takes precedence.

## Links

- API reference: https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2026-03-10
- Copilot usage metrics docs: https://docs.github.com/copilot/concepts/copilot-usage-metrics/copilot-metrics

## Why this matters

With this update, you can:

- Measure real engagement (active interaction) vs coverage (automatic reviews that weren’t interacted with)
- Track adoption maturity with more nuance (for example, repo coverage vs percentage of users actively engaging)
- Compare CCR engagement alongside other Copilot “surfaces”, including:
  - `used_agent` (IDE agent mode)
  - `used_copilot_coding_agent` (CCA)

## Discussion

- GitHub Community announcements: https://github.com/orgs/community/discussions/categories/announcements


[Read the entire article](https://github.blog/changelog/2026-04-06-copilot-usage-metrics-now-identify-active-and-passive-copilot-code-review-users)

