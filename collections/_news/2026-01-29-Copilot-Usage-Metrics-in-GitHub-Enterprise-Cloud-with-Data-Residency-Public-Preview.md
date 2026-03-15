---
external_url: https://github.blog/changelog/2026-01-29-copilot-metrics-in-github-enterprise-cloud-with-data-residency-in-public-preview
title: 'Copilot Usage Metrics in GitHub Enterprise Cloud with Data Residency: Public Preview'
author: Allison
primary_section: github-copilot
feed_name: The GitHub Blog
date: 2026-01-29 17:57:56 +00:00
tags:
- Advanced Analytics
- AI
- AI Controls
- API Access
- Code Generation Analytics
- Compliance
- Copilot
- Copilot Metrics
- Copilot Usage
- Data Residency
- DevOps
- Enterprise Management
- Enterprise Management Tools
- Feature Adoption
- GitHub Copilot
- GitHub Enterprise Cloud
- Monitoring
- News
- Permissions
- Reporting
- Usage Dashboards
section_names:
- ai
- devops
- github-copilot
---
Allison shares news about GitHub Copilot usage and code generation dashboards now available for Enterprise Cloud customers with data residency, along with new APIs and fine-grained access permissions.<!--excerpt_end-->

# Copilot Usage Metrics in GitHub Enterprise Cloud with Data Residency: Public Preview

GitHub has announced that comprehensive Copilot metrics, usage dashboards, and a corresponding API are now available for Enterprise Cloud customers taking advantage of data residency controls. This release helps organizations meet compliance and regional requirements while gaining critical insights into how Copilot is being used across teams.

## Key Features

- **Copilot Usage Dashboard:**
  - Visualize how teams interact with GitHub Copilot.
  - Metrics include code completion activity, IDE usage, and lines of code generated.
- **Code Generation Dashboard:**
  - Quantifies Copilot's suggestions, additions, and deletions for completions, chat, and agent features.
- **Fine-Grained Permissions:**
  - Anyone with the `View enterprise Copilot metrics` role (not just admins or billing managers) can access these insights.
- **Organization-Wide Metrics:**
  - APIs allow programmatic access to Copilot adoption, feature-specific usage, user engagement, and more for detailed reporting and analytics.
- **API Access:**
  - Integrate Copilot usage data into custom dashboards for compliance, monitoring, and advanced analysis.
  - [API documentation](https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2022-11-28) is available for implementation details.

## Data Residency & Availability

- Usage data is attributed to the new enterprise after migration to the data residency-enabled environment; historical metrics remain with the original instance.
- If users operate multiple GitHub accounts in their IDE, all Copilot activity is attributed to the account associated with Enterprise Cloud with data residency.

## Access Instructions

- To view dashboards: Navigate to **enterprise account → AI Controls → Copilot → Metrics**. The dashboards will be available under **Insights → Copilot usage** or **Code generation** sections.
- For API-based access and automation, consult the [Copilot usage metrics API docs](https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2022-11-28).

## Additional Resources

- [GitHub Community discussion](https://github.com/orgs/community/discussions/185365)
- [Copilot usage metrics API documentation](https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2022-11-28)

---

GitHub's new usage metrics support enterprise reporting, compliance, and advanced analytics, empowering organizations to make better-informed decisions about Copilot adoption and utilization.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-29-copilot-metrics-in-github-enterprise-cloud-with-data-residency-in-public-preview)
