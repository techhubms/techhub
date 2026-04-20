---
section_names:
- ai
- devops
- github-copilot
author: Jakub Oleksy
external_url: https://github.blog/news-insights/company-news/bringing-more-transparency-to-githubs-status-page/
date: 2026-04-17 16:00:00 +00:00
feed_name: The GitHub Blog
primary_section: github-copilot
title: Bringing more transparency to GitHub’s status page
tags:
- AI
- Auto Model Selection
- Availability
- Company News
- Copilot Chat
- Copilot Cloud Agent
- Degraded Performance
- DevOps
- Downtime Weighting
- GitHub Copilot
- GitHub Status Page
- Incident Communication
- Incident Severity
- Major Outage
- Model Providers
- News
- News & Insights
- Observability
- Operations
- Partial Outage
- Reliability
- Service Health
- Statuspage Metrics
- Uptime Metrics
---

Jakub Oleksy explains changes to GitHub’s status page, including a new “Degraded Performance” incident state, per-service uptime metrics, and more granular reporting for Copilot AI model provider disruptions.<!--excerpt_end-->

# Bringing more transparency to GitHub’s status page

GitHub is rolling out changes to its status page to improve how it reports service health during and after incidents, with an emphasis on transparency, accuracy, and timeliness.

## What’s changing

GitHub is introducing three updates:

- A new **Degraded Performance** state for incident classification
- **Per-service uptime metrics** published for each service (last 90 days)
- More granular disruption reporting for Copilot, starting with a dedicated **Copilot AI Model Providers** component

![Image of the updated GitHub status page.](https://github.blog/wp-content/uploads/2026/04/image-6.png?resize=2084%2C1756)

## Improving accuracy with a new incident state

GitHub is adding a new incident severity level: **Degraded Performance**, creating a three-tier system:

| State | What It Means |
| --- | --- |
| **Degraded Performance** | The service is operational but impaired. You may experience elevated latency, reduced functionality, or intermittent errors affecting a small percentage of requests. |
| **Partial Outage** | A significant portion of the service is unavailable or severely impacted for a meaningful number of users. |
| **Major Outage** | The service is broadly unavailable, affecting most or all users. |

Previously, incidents with minimal disruption were still classified as at least a partial outage, which could overstate the impact.

## Per-service uptime percentages (last 90 days)

GitHub is now publishing **per-service uptime percentages** for the last 90 days directly on the status page.

How the uptime calculation works:

- It considers **number of incidents**, **severity**, and **duration** per service.
- It uses weighted downtime based on severity:

| Severity | Downtime weight |
| --- | --- |
| **Major Outage** | 100% — the full duration counts as downtime |
| **Partial Outage** | 30% — reflects significant but not total service loss |
| **Degraded Performance** | 0% — does not count as downtime; the service remains functional |

Example given:

- A **1-hour Partial Outage** over a 90-day period counts as **18 minutes** of effective downtime in the uptime calculation (not the full hour).
- A **Degraded Performance** incident does **not** affect the uptime percentage.

## More granular Copilot model provider reporting

GitHub has added a new status page component: **Copilot AI Model Providers**.

Previously:

- If a model provider had an outage, GitHub declared an incident against the broader **Copilot** service, even if the impact was limited to a single model.

Why this is changing:

- Copilot features (including **GitHub Copilot Chat** and **GitHub Copilot cloud agent**, formerly “coding agent”) support multiple models.
- If one model is unavailable, users may be able to:
  - Choose an alternative model, or
  - Use **auto model selection** to pick the best available model: https://docs.github.com/en/copilot/concepts/auto-model-selection

Going forward:

- Model availability incidents will be reported under **Copilot AI Model Providers** rather than the broader Copilot component.
- GitHub says it will continue to publish incident details such as which models are affected.

## Continued focus on transparency

GitHub positions these changes as operationally useful context for teams making decisions during incidents, with clearer classification, service-level reliability history, and more precise reporting for Copilot-related model provider outages.


[Read the entire article](https://github.blog/news-insights/company-news/bringing-more-transparency-to-githubs-status-page/)

