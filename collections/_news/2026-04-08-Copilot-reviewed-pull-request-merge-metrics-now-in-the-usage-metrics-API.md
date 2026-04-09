---
external_url: https://github.blog/changelog/2026-04-08-copilot-reviewed-pull-request-merge-metrics-now-in-the-usage-metrics-api
author: Allison
feed_name: The GitHub Blog
primary_section: github-copilot
section_names:
- ai
- devops
- github-copilot
date: 2026-04-08 23:21:50 +00:00
tags:
- 28 Day Rolling Window
- Account Management
- AI
- Code Review Automation
- Copilot
- Copilot Code Review
- Copilot Coding Agent
- Copilot Usage Metrics API
- Cycle Time
- DevOps
- Enterprise Administrators
- Enterprise Management Tools
- Enterprise Reporting
- GitHub Copilot
- GitHub REST API
- Merge Rate
- News
- Organization Owners
- Organization Reporting
- Pull Request Metrics
- Throughput
- Time To Merge
- Usage Metrics
title: Copilot-reviewed pull request merge metrics now in the usage metrics API
---

Allison announces new GitHub Copilot usage metrics API fields that measure how Copilot code review affects pull request merges and time-to-merge, enabling enterprise and org owners to track adoption and review impact across reporting windows.<!--excerpt_end-->

# Copilot-reviewed pull request merge metrics now in the usage metrics API

Building on the earlier release of pull request throughput and cycle-time metrics (for Copilot-created pull requests), the **Copilot usage metrics API** now adds two new metrics focused on **Copilot code review** activity.

Related links:

- Pull request throughput and cycle-time metrics (Feb release): https://github.blog/changelog/2026-02-19-pull-request-throughput-and-time-to-merge-available-in-copilot-usage-metrics-api
- Copilot usage metrics API docs: https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2026-03-10

## New metrics

- **`total_merged_reviewed_by_copilot`**
  - Total number of pull requests that were **both merged** and **reviewed by Copilot code review** during the reporting period.
- **`median_minutes_to_merge_copilot_reviewed`**
  - Median minutes from **pull request creation → merge**, calculated only for pull requests that received a **Copilot code review**.

## What’s new

The earlier release captured how Copilot helps **author** pull requests (specifically, coding agent–created PRs that were merged, and their time to merge).

These new metrics capture how Copilot helps **review** pull requests. The idea is to compare merge rates and cycle times for PRs that received a Copilot code review against your baseline.

## Reporting availability

Both metrics are available:

- As **single-day** reports
- As **28-day rolling window** reports
- At the **enterprise** and **organization** levels

## Why this matters

- **Measure review impact**: Compare time-to-merge for Copilot-reviewed PRs against your overall median to see whether automated reviews help PRs land faster.
- **Track code review adoption**: Track how many merged PRs included a Copilot review across your enterprise or organization.
- **Complete PR lifecycle visibility**: Combined with the existing Copilot-created PR metrics, you can track Copilot’s participation from **authoring → review → merge**.

## Important notes

- These metrics are available to:
  - **Enterprise administrators**
  - **Organization owners**
  - (who have access to Copilot usage metrics) https://docs.github.com/copilot/concepts/copilot-usage-metrics
- Only PRs that received a **Copilot code review** are included in these two new metrics.
- PRs created by the **Copilot coding agent** are tracked separately via existing fields:
  - `copilot_created_pull_requests_merged`
  - `median_minutes_to_merge_copilot_created`

Discussion link:

- GitHub Community announcements: https://github.com/orgs/community/discussions/categories/announcements


[Read the entire article](https://github.blog/changelog/2026-04-08-copilot-reviewed-pull-request-merge-metrics-now-in-the-usage-metrics-api)

