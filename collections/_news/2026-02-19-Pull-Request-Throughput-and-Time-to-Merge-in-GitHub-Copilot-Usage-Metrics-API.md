---
layout: "post"
title: "Pull Request Throughput and Time to Merge in GitHub Copilot Usage Metrics API"
description: "This update introduces new enterprise-level APIs that provide GitHub Copilot usage metrics, empowering organizations to analyze Copilot's impact on pull request workflows. Enterprise admins can track review suggestions, pull request merges, and overall cycle time, gaining actionable insight into Copilot-assisted development."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-19-pull-request-throughput-and-time-to-merge-available-in-copilot-usage-metrics-api"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-19 16:08:02 +00:00
permalink: "/2026-02-19-Pull-Request-Throughput-and-Time-to-Merge-in-GitHub-Copilot-Usage-Metrics-API.html"
categories: ["AI", "DevOps", "GitHub Copilot"]
tags: ["Account Management", "AI", "API Integration", "Copilot", "Copilot Metrics API", "Development Metrics", "DevOps", "DevOps Insights", "Enterprise Admin", "Enterprise Management Tools", "GitHub Copilot", "GitHub Enterprise", "Merge Cycle Time", "News", "Pull Request Analytics", "Review Suggestions"]
tags_normalized: ["account management", "ai", "api integration", "copilot", "copilot metrics api", "development metrics", "devops", "devops insights", "enterprise admin", "enterprise management tools", "github copilot", "github enterprise", "merge cycle time", "news", "pull request analytics", "review suggestions"]
---

Allison details how the Copilot usage metrics API helps enterprise admins gain visibility into the impact of GitHub Copilot on pull request workflows and team productivity.<!--excerpt_end-->

# Pull Request Throughput and Time to Merge in GitHub Copilot Usage Metrics API

GitHub has released new enterprise-level Copilot usage metrics APIs that let organizations analyze how GitHub Copilot affects pull request outcomes. These metrics expose detailed data about how Copilot assists in authoring and merging pull requests, helping teams better understand its influence on development velocity and collaboration.

## How It Works

- **API Metrics Provided:**
  - Pull request review suggestions and acceptance rates
  - Number of pull requests created with Copilot that were merged
  - Pull request cycle time (idea to merge)

- **Eligibility:**
  - Available to enterprise admins or users with the `View Enterprise Copilot Metrics` role
  - Copilot usage metrics must be enabled for the organization

## Key Benefits

- Track how Copilot helps author and review pull requests
- Analyze whether Copilot usage reduces time to merge
- Measure the real-world impact of Copilot review suggestions

## Getting Started

- Review the [Copilot usage metrics API documentation](https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2022-11-280) for details on endpoints and required permissions.
- Join the ongoing discussion in the [GitHub Community announcements](https://github.com/orgs/community/discussions/categories/announcements).

These metrics enable data-driven decisions for organizations seeking to optimize code reviews, branch management, and Copilot adoption. For development teams, this results in improved collaboration and clearer insight into which Copilot features deliver the most value.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-19-pull-request-throughput-and-time-to-merge-available-in-copilot-usage-metrics-api)
