---
layout: "post"
title: "GitHub Copilot Usage Metrics Now Track Plan Mode Telemetry"
description: "This update from GitHub introduces plan mode telemetry into Copilot usage metrics. Enterprises can now monitor adoption and engagement specifically for plan mode, gaining deeper insights alongside existing Copilot metrics. The new telemetry is available across various IDEs and is reflected in API responses and dashboard visualizations, providing organizations with a more granular understanding of Copilot usage patterns."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-03-02-copilot-metrics-now-includes-plan-mode"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-03-02 16:50:58 +00:00
permalink: "/2026-03-02-GitHub-Copilot-Usage-Metrics-Now-Track-Plan-Mode-Telemetry.html"
categories: ["AI", "GitHub Copilot"]
tags: ["Account Management", "AI", "API Response", "Copilot", "Copilot Metrics", "Dashboard Insights", "Eclipse", "Enterprise Management", "Enterprise Management Tools", "GitHub Copilot", "IDE Integration", "JetBrains", "News", "Plan Mode", "Telemetry", "Usage Analytics", "VS Code", "Xcode"]
tags_normalized: ["account management", "ai", "api response", "copilot", "copilot metrics", "dashboard insights", "eclipse", "enterprise management", "enterprise management tools", "github copilot", "ide integration", "jetbrains", "news", "plan mode", "telemetry", "usage analytics", "vs code", "xcode"]
---

Allison shares news on GitHub Copilot's usage metrics update, now including plan mode telemetry to give enterprises finer-grained visibility into team adoption and engagement.<!--excerpt_end-->

# GitHub Copilot Usage Metrics Now Track Plan Mode Telemetry

GitHub has enhanced Copilot usage metrics by adding plan mode telemetry support. This change allows enterprises and organizations to track how often plan mode is used in their teams, providing a more complete view of Copilot adoption and engagement.

## Key Highlights

- **Plan mode telemetry** is now part of Copilot usage metrics, giving organizations the ability to monitor adoption and engagement trends specifically for plan mode.
- **Supported IDEs** include JetBrains, Eclipse, Xcode, and VS Code Insiders. General support for VS Code is coming soon.
- **Data Access**:
  - In the [API response](https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2022-11-28), plan mode usage is indicated by `chat_panel_plan_mode` within `totals_by_feature`, `totals_by_language_feature`, and `totals_by_model_feature` keys.
  - The dashboard UI now provides plan mode breakdowns under **Insights > Copilot usage**.
- **Important Change**: Previously, plan mode telemetry was grouped under "Custom". With this rollout, plan mode is now separated, which may lead to a visible drop in the "Custom" category in usage breakdown charts.
- **Impact on Reporting**: Charts like "Requests per chat mode" and "Model usage per chat mode" will reflect the new separation for clearer analysis.

## Next Steps

- Make sure that **Copilot usage metrics** are enabled for your enterprise or organization to benefit from these updates.
- Review updated usage dashboards and API responses for plan mode-specific insights.
- Join the ongoing [GitHub Community discussion](https://github.com/orgs/community/discussions/categories/announcements) for questions or feedback.

For more details, visit the [official announcement](https://github.blog/changelog/2026-03-02-copilot-metrics-now-includes-plan-mode).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-03-02-copilot-metrics-now-includes-plan-mode)
