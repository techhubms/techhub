---
title: Copilot usage metrics now aggregate Copilot cloud agent active user counts
external_url: https://github.blog/changelog/2026-04-10-copilot-usage-metrics-now-aggregate-copilot-cloud-agent-active-user-counts
feed_name: The GitHub Blog
section_names:
- ai
- github-copilot
primary_section: github-copilot
date: 2026-04-10 21:42:47 +00:00
tags:
- Account Management
- Active Users
- Adoption Metrics
- Aggregated Metrics
- AI
- API Versioning
- Copilot
- Copilot Cloud Agent
- Copilot Coding Agent
- Copilot Usage Metrics API
- Daily Active Users
- Enterprise Cloud
- Enterprise Management Tools
- Enterprise Reporting
- GitHub Copilot
- Improvement
- Monthly Active Users
- News
- Organization Reporting
- REST API
- Schema Change
- Usage Reporting
- Weekly Active Users
author: Allison
---

Allison announces updates to the GitHub Copilot usage metrics API, adding aggregated daily, weekly, and monthly active user counts for Copilot cloud agent (formerly Copilot coding agent) in enterprise and organization reports.<!--excerpt_end-->

## Copilot usage metrics now aggregate Copilot cloud agent active user counts

Note: Copilot coding agent has been renamed to **Copilot cloud agent**. GitHub will update its data schema so existing “coding agent” fields reflect the name change, and new fields going forward will use the updated name.

Following the launch of cloud agent active user identification, enterprise and organization usage reports in the **Copilot usage metrics API** now include **aggregated active user counts** for Copilot cloud agent.

- Changelog reference: https://github.blog/changelog/2026-04-01-research-plan-and-code-with-copilot-cloud-agent/
- Prior related update: https://github.blog/changelog/2026-03-25-copilot-usage-metrics-now-identify-active-copilot-coding-agent-users/
- API documentation: https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2026-03-10

## New fields in 1-day and 28-day reports

Three new fields are available in both **1-day** and **28-day** reports at the **enterprise** and **organization** levels:

| Field | Description |
| --- | --- |
| `daily_active_copilot_cloud_agent_users` | Number of unique users who used Copilot cloud agent on that day |
| `weekly_active_copilot_cloud_agent_users` | Number of unique users who used Copilot cloud agent in the trailing 7-day window |
| `monthly_active_copilot_cloud_agent_users` | Number of unique users who used Copilot cloud agent in the trailing 28-day window |

### Nullability behavior

These fields are **nullable**:

- They return a count (including `0`) when data is available.
- They return `null` when no Copilot cloud agent data exists for that period.

## Why this matters

- Track adoption quickly across an enterprise or organization without aggregating user-level data.
- Compare engagement across daily, weekly, and monthly windows to spot trends and gauge rollout impact.
- Use these counts alongside existing metrics such as `monthly_active_agent_users` (IDE agent mode) and the user-level `used_copilot_coding_agent` flag to get a broader view of Copilot adoption across surfaces.

## Discussion

Join the discussion on GitHub Community announcements:

- https://github.com/orgs/community/discussions/categories/announcements


[Read the entire article](https://github.blog/changelog/2026-04-10-copilot-usage-metrics-now-aggregate-copilot-cloud-agent-active-user-counts)

