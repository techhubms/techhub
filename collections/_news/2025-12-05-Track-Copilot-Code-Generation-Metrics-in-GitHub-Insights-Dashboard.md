---
layout: post
title: Track Copilot Code Generation Metrics in GitHub Insights Dashboard
author: Allison
canonical_url: https://github.blog/changelog/2025-12-05-track-copilot-code-generation-metrics-in-a-dashboard
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-12-05 18:40:33 +00:00
permalink: /github-copilot/news/Track-Copilot-Code-Generation-Metrics-in-GitHub-Insights-Dashboard
tags:
- Agent Actions
- AI Insights
- Code Generation
- Completions
- Copilot
- Dashboard
- Data Analysis
- Enterprise
- GitHub
- Lines Of Code
- Metrics
- NDJSON
- Permissions
- Programming Languages
- Usage Metrics
- User Roles
section_names:
- ai
- github-copilot
---
Allison introduces an update for GitHub Copilot enterprise users: the ability to track lines of code metrics from Copilot directly in the code generation insights dashboard using detailed AI controls.<!--excerpt_end-->

# Track Copilot Code Generation Metrics in GitHub Insights Dashboard

GitHub enterprise users now have access to an enhanced dashboard for monitoring Copilot's impact on code generation. The code generation insights dashboard provides:

- **Lines of Code (LoC) Metrics:** Total lines added or deleted with AI assistance across all interaction modes.
- **User-Initiated Code Changes:** Tracks code suggested or manually added through completions and chat actions in Copilot.
- **Agent-Initiated Code Changes:** Includes lines automatically managed by agents in edit, agent, and custom modes.
- **Activity Breakdown:** Metrics grouped by Copilot model, programming language, and initiation type (user or agent).

## How to Access Metrics

1. Go to the [Enterprises](https://github.com/settings/enterprises) page.
2. Select your enterprise, then click on the **Insights** tab.
3. In the sidebar, choose **Code generation** to view activity metrics.
4. NDJSON files with more detailed data can be downloaded from the dashboard.

## Enabling Metrics Collection

- Metrics are visible only if the **Copilot usage metrics** policy is enabled.
- Enable it via [AI Controls](https://github.com/settings/enterprises), select your enterprise, then **Copilot > Metrics**.

## Permissions and Access

- The dashboard is accessible to enterprise owners, billing managers, and custom role users with **View Enterprise Copilot Metrics** permission.
- For full details, consult [GitHub's official documentation](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/administer-copilot/manage-for-enterprise/view-code-generation).

*Features in public preview may change as GitHub updates the UI and capabilities.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-05-track-copilot-code-generation-metrics-in-a-dashboard)
