---
external_url: https://github.blog/changelog/2025-10-28-copilot-usage-metrics-dashboard-and-api-in-public-preview
title: Copilot Usage Metrics Dashboard and API in Public Preview for GitHub Enterprise
author: Allison
viewing_mode: external
feed_name: The GitHub Blog
date: 2025-10-28 15:23:43 +00:00
tags:
- AI Adoption
- API
- Copilot
- Copilot Agent
- Copilot Analytics
- Dashboard
- Enterprise Metrics
- IDE Usage
- Model Usage
- Programming Languages
- Public Preview
- Software Development Metrics
- Universe25
section_names:
- ai
- github-copilot
---
Allison introduces the public preview of GitHub Copilot usage metrics dashboard and API, offering GitHub Enterprise admins deeper insight into adoption and usage patterns of Copilot within their organizations.<!--excerpt_end-->

# Copilot Usage Metrics Dashboard and API in Public Preview

**Author: Allison**

The GitHub Copilot usage metrics dashboard and its corresponding API are now available in public preview for GitHub Enterprise customers. This release provides enterprise administrators and billing managers with detailed visibility into how Copilot is used across organizations, moving beyond simply tracking AI adoption to understanding engagement quality and usage depth.

## What's Included in the Public Preview

- **Dashboard Access**: The Copilot usage metrics dashboard is accessible under the *Insights* tab for enterprise-level management.
- **Granular Metrics**: Offers both enterprise-level aggregation and user-level details via an NDJSON download or API.
- **Key Metrics Provided**:
  - Daily and weekly active users across IDE modes (including agent mode)
  - Agent adoption rates across user base
  - Lines of code added and deleted in all IDE modes
  - Programming language and model usage patterns

These metrics help answer fundamental questions about Copilot's role in enterprise development, such as:

- Overall usage and adoption at the organizational level
- Which AI models and languages are most frequently used
- Agent adoption for advanced tasks like refactoring or debugging

## Enabling Copilot Usage Metrics

By default, the metrics feature is disabled unless activated during the private preview. To enable:

1. Go to your **AI Controls** tab on [github.com](https://github.com).
2. Select **Copilot** in the left sidebar.
3. Find the “Metrics” section and set **Copilot usage metrics** to **Enabled**.
4. Once enabled, access the dashboard in the *Insights* tab or use the [Enterprise and user-level API](https://docs.github.com/enterprise-cloud@latest/early-access/copilot-metrics/apis/about-the-copilot-metrics-apis?utm_source=changelog-docs-copilot-usage-metrics-dashboard&utm_medium=changelog&utm_campaign=universe25).

## Learn More

- [Usage Metrics Dashboard Documentation](https://docs.github.com/enterprise-cloud@latest/early-access/copilot-metrics/dashboards/about-the-copilot-metrics-dashboard?utm_source=changelog-docs-copilot-usage-metrics-dashboard&utm_medium=changelog&utm_campaign=universe25)
- [Copilot Usage Metrics API Guide](https://docs.github.com/enterprise-cloud@latest/early-access/copilot-metrics/apis/about-the-copilot-metrics-apis?utm_source=changelog-docs-copilot-usage-metrics-dashboard&utm_medium=changelog&utm_campaign=universe25)
- [Community Discussion & Feedback](https://github.com/orgs/community/discussions/177155?utm_source=changelog-community-copilot-usage-metrics-dashboard&utm_medium=changelog&utm_campaign=universe25)

*Note: UI and features may change before general availability.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-28-copilot-usage-metrics-dashboard-and-api-in-public-preview)
