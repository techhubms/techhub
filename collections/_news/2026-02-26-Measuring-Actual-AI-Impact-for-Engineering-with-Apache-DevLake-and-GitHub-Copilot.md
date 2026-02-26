---
layout: "post"
title: "Measuring Actual AI Impact for Engineering with Apache DevLake and GitHub Copilot"
description: "This article shows how to use Apache DevLake, enhanced with a GitHub Copilot plugin, to move beyond basic adoption metrics and meaningfully measure the real delivery impact of GitHub Copilot in engineering teams. It covers the observability gap, DevLake’s architecture, DORA metrics, correlation dashboards, and practical setup via the gh-devlake CLI extension."
author: "Eldrick Wega"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/all-things-azure/measuring-actual-ai-impact-for-engineering-with-apache-devlake/"
viewing_mode: "external"
feed_name: "Microsoft All Things Azure Blog"
feed_url: "https://devblogs.microsoft.com/all-things-azure/feed/"
date: 2026-02-26 10:06:19 +00:00
permalink: "/2026-02-26-Measuring-Actual-AI-Impact-for-Engineering-with-Apache-DevLake-and-GitHub-Copilot.html"
categories: ["AI", "DevOps", "GitHub Copilot"]
tags: ["Adoption Metrics", "AI", "All Things Azure", "Apache DevLake", "Azure DevOps", "CI/CD", "Copilot Metrics API", "Developer Analytics", "Developer Productivity", "DevLake", "DevOps", "DevOps Metrics", "DORA", "Engineering Productivity", "Gh Devlake", "GitHub", "GitHub Copilot", "Grafana", "Impact Measurement", "Microsoft", "News", "ROI", "SQL Dashboards", "Thought Leadership"]
tags_normalized: ["adoption metrics", "ai", "all things azure", "apache devlake", "azure devops", "cislashcd", "copilot metrics api", "developer analytics", "developer productivity", "devlake", "devops", "devops metrics", "dora", "engineering productivity", "gh devlake", "github", "github copilot", "grafana", "impact measurement", "microsoft", "news", "roi", "sql dashboards", "thought leadership"]
---

Eldrick Wega details how engineering teams can measure the true delivery impact of GitHub Copilot by integrating Copilot metrics with DevOps data using Apache DevLake and the gh-devlake CLI extension.<!--excerpt_end-->

# Measuring Actual AI Impact for Engineering with Apache DevLake and GitHub Copilot

*By Eldrick Wega*

Engineering teams often roll out GitHub Copilot but struggle to assess whether it impacts actual software delivery and reliability. While GitHub provides built-in adoption metrics (seats assigned, suggestion acceptance rates, feature usage), these metrics are siloed and don’t correlate directly with delivery performance.

## The Observability Gap

Copilot metrics show you adoption and usage, but not improvement in outcomes like shipping speed, reliability, or code quality. Delivery outcomes (like deployment frequency and cycle time) live in CI/CD and issue tools (GitHub, Jira, Jenkins), while Copilot adoption data sits elsewhere.

**To answer leadership's real question—does Copilot really improve engineering performance?—you need to combine these data sources.**

## Apache DevLake: A Data Warehouse for the DevOps Stack

[Apache DevLake](https://devlake.apache.org/) is an open-source platform that connects to 20+ sources (GitHub, GitLab, Jira, Jenkins, Azure DevOps, SonarQube, PagerDuty, Bitbucket, CircleCI, etc.), ingests and normalizes data from all of them, and stores everything in a queryable SQL warehouse.

- Standardizes data models (e.g., Jenkins build and GitHub Actions workflow treated alike)
- Enables cross-tool insights and queries
- Supports team/repo/org-scoped analysis

**Configuration is simple and handled via the [`gh-devlake` GitHub CLI extension](https://github.com/DevExpGBB/gh-devlake).**

### Key Concepts

- **Connections**: Authenticated links to each data source
- **Scopes**: What data (repos, boards, projects) to ingest
- **Projects**: Grouping connections and scopes across teams or initiatives
- **Blueprints**: Scheduling recurring syncs

## DORA and the Framework Approach

DevLake ships with DORA metrics (Deployment Frequency, Lead Time for Changes, Change Failure Rate, Mean Time to Recovery) built-in for benchmarking team performance. You can also align with frameworks like SPACE or GitHub’s own DevEx models.

- **DORA benchmarking:** Elite/High/Medium/Low performance tiers
- **Customizable:** Tweak metrics and queries as needed

## Correlating Copilot with Delivery Outcomes: The Copilot Impact Dashboard

Microsoft’s engineering teams contributed a `gh-copilot` plugin for DevLake that integrates Copilot adoption metrics (DAU/WAU/MAU, acceptance rates, feature usage, seat utilization) into the platform. Data is normalized to DevLake’s schema, enabling:

- **Adoption dashboard:** See rollout health and trend metrics
- **Impact dashboard:** *Correlates Copilot adoption intensity with DORA metrics* (e.g., PR velocity, deployment frequency, recovery time, code review time)

**Example insight:** Weeks with >75% Copilot adoption had 33% shorter PR cycle times and 2x deployments compared to weeks with <25% adoption.

## Extensibility and Data Ownership

- Dashboards are powered by user-accessible SQL queries in Grafana (inspect, customize, and extend)
- Add new panels, change metric definitions, pick custom filters
- Webhook and plugin system for further tool integration

## Getting Started

Use [`gh-devlake`](https://github.com/DevExpGBB/gh-devlake) to deploy a ready-to-use DORA + Copilot dashboard in a few CLI commands. Comprehensive docs and examples are provided for setup and customization:

- [Apache DevLake Documentation](https://devlake.apache.org/)
- [DORA Research](https://dora.dev/research/)
- [GitHub Copilot Metrics API](https://docs.github.com/en/rest/copilot/copilot-metrics)

*For support or ideas, open an issue on the [gh-devlake repo](https://github.com/DevExpGBB/gh-devlake/issues).*

This post appeared first on "Microsoft All Things Azure Blog". [Read the entire article here](https://devblogs.microsoft.com/all-things-azure/measuring-actual-ai-impact-for-engineering-with-apache-devlake/)
