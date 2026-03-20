---
external_url: https://github.blog/changelog/2026-03-20-copilot-usage-metrics-now-resolve-auto-model-selection-to-actual-models
primary_section: github-copilot
feed_name: The GitHub Blog
date: 2026-03-20 18:37:18 +00:00
section_names:
- ai
- github-copilot
tags:
- Admin Dashboards
- AI
- Audit Workflows
- Auto Model Selection
- Chat Mode Metrics
- Compliance Reporting
- Copilot
- Copilot Usage Metrics
- Copilot Usage Metrics REST API
- Enterprise Reporting
- GitHub Copilot
- Improvement
- Model Breakdowns
- Model Usage Reporting
- News
- Organization Reporting
- REST API
- Totals By Model Feature
- User Level Reports
title: Copilot usage metrics now resolve auto model selection to actual models
author: Allison
---

Allison announces an update to GitHub Copilot usage metrics that resolves “Auto” model selection to the actual model names in both the dashboard and REST API, improving accuracy for admin reporting and compliance/audit use cases.<!--excerpt_end-->

# Copilot usage metrics now resolve auto model selection to actual models

Copilot usage metrics now provides full clarity into model usage when auto model selection is enabled. Activity that previously appeared under a generic “Auto” label now resolves to the actual model name, giving admins a complete and accurate view of which models their teams are using.

This applies to model breakdowns across the Copilot usage metrics REST API and dashboard, including `totals_by_model_feature` in the `enterprise`, `org`, and `user` level reports, as well as the “Model usage per chat mode” charts.

## What changed

- Usage that used to be grouped under **“Auto”** is now attributed to the **actual model name**.
- The change applies across:
  - The **Copilot usage metrics REST API**: https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2026-03-10
  - The **Copilot usage metrics dashboard**
  - `totals_by_model_feature` in **enterprise**, **org**, and **user** reports
  - The **“Model usage per chat mode”** charts

## Why this matters

- See exactly which models are being used across your organization, even when auto mode is the default.
- Gain more accurate model-level reporting to support compliance and audit workflows.
- As auto mode adoption grows, model-level metrics stay clear and actionable.

## Current limitation

Auto usage is rolled into the resolved model totals. A separate breakdown of auto-selected vs. manually selected model usage is not yet available, but is being considered for a future update.

## Discussion

Join the discussion in GitHub Community announcements:

- https://github.com/orgs/community/discussions/categories/announcements


[Read the entire article](https://github.blog/changelog/2026-03-20-copilot-usage-metrics-now-resolve-auto-model-selection-to-actual-models)

