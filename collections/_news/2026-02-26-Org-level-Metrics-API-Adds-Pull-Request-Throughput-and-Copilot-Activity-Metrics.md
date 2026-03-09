---
layout: "post"
title: "Org-level Metrics API Adds Pull Request Throughput and Copilot Activity Metrics"
description: "This update to the GitHub Copilot Usage Metrics API expands organization-level endpoints to include detailed pull request activity, such as creation volume, median time to merge, and Copilot participation in both coding and code reviews. It brings org-level metrics into parity with existing enterprise-level analytics, providing greater visibility into non-IDE Copilot impact on development workflows. The change empowers teams to standardize reporting and better understand Copilot's influence on pull request lifecycle efficiency and review quality."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-25-org-level-metrics-api-now-includes-pull-request-throughput-metric-parity"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-26 00:11:37 +00:00
permalink: "/2026-02-26-Org-level-Metrics-API-Adds-Pull-Request-Throughput-and-Copilot-Activity-Metrics.html"
categories: ["AI", "DevOps", "GitHub Copilot"]
tags: ["Account Management", "AI", "API Update", "Applied Suggestions", "Code Review", "Copilot", "Copilot Participation", "Cycle Time", "Development Analytics", "DevOps", "Enterprise Management", "Enterprise Management Tools", "GitHub Copilot", "Metrics API", "News", "Org Level Reporting", "Pull Requests", "Release Notes", "Review Activity", "Team Productivity", "Throughput"]
tags_normalized: ["account management", "ai", "api update", "applied suggestions", "code review", "copilot", "copilot participation", "cycle time", "development analytics", "devops", "enterprise management", "enterprise management tools", "github copilot", "metrics api", "news", "org level reporting", "pull requests", "release notes", "review activity", "team productivity", "throughput"]
---

Allison details important enhancements to the Copilot Usage Metrics API, now offering organization-level pull request and Copilot participation analytics for development teams.<!--excerpt_end-->

# Org-level Metrics API Adds Pull Request Throughput and Copilot Activity Metrics

**Author: Allison**

The GitHub Copilot Usage Metrics API has been updated to expand its coverage at the organization level. This update introduces:

- Daily aggregated metrics for pull request (PR) activity
- Copilot participation analytics in both pull request creation and review
- Endpoint feature parity with the previous enterprise-level API

## What's New

Organizations can now access:

- Baseline pull request activity with metrics including:
  - Total pull requests created, reviewed, and merged
  - Median time to merge (in minutes)
- Copilot-specific participation data:
  - PRs created and reviewed by Copilot (as a coding agent or reviewer)
  - Number of Copilot suggestions and those that were applied
  - Outcomes for Copilot-created pull requests, including merged status and median merge time

## Why This Matters

These detailed metrics enable teams to:

- Standardize reporting across organizations
- Quantify Copilot's impact at various pull request workflow stages
- Distinguish between suggestion volume and applied feedback for actionable insights
- Gain new visibility into non-IDE Copilot activity, such as agent-authored PRs and Copilot-driven review suggestions

## Key Notes and Considerations

- **Report Consistency:** Organization and enterprise totals may differ. Some users contribute across multiple orgs, and enterprise-level reporting deduplicates user data.
- **Empty API Results:** If an org displays API data only for pull requests and none for IDE usage, it's likely due to lack of IDE activity.
- **Attribution Splits:** Repository and organization transfers may split event attribution (creation, review, and merge) between entities, affecting report data at both org and enterprise levels.

## Further Reading

For more information on field definitions and metric computation, consult the [Copilot usage metrics documentation](https://docs.github.com/copilot/reference/copilot-usage-metrics/copilot-usage-metrics). Join the ongoing discussion within the [GitHub Community](https://github.com/orgs/community/discussions/categories/announcements).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-25-org-level-metrics-api-now-includes-pull-request-throughput-metric-parity)
