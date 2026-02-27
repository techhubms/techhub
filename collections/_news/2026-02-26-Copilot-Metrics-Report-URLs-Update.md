---
external_url: https://github.blog/changelog/2026-02-26-copilot-metrics-report-urls-update
title: Copilot Metrics Report URLs Update
author: Allison
primary_section: github-copilot
feed_name: The GitHub Blog
date: 2026-02-26 20:11:34 +00:00
tags:
- Account Management
- AI
- Allowlist
- API Contract
- API Update
- Azure CDN
- CDN Domains
- Copilot
- DevOps
- DevOps Tools
- Endpoint Change
- Enterprise Management
- Enterprise Management Tools
- Firewall Configuration
- GitHub Copilot
- Improvement
- News
- Usage Metrics
section_names:
- ai
- devops
- github-copilot
---
Allison explains the required action on firewall allowlist configurations due to a new endpoint for GitHub Copilot usage metrics API report downloads. Report data and schemas are unaffected.<!--excerpt_end-->

# Copilot Metrics Report URLs Update

The download URLs returned by the [GitHub Copilot usage metrics API](https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2022-11-28) now point to a new CDN endpoint. All report data, the API contract, and the response schema remain unchanged.

## Action Required

- If you maintain firewall rules that allowlist specific CDN domains, add:
  - `copilot-reports-production-*.b01.azurefd.net`
  - Retain the existing pattern: `copilot-reports-*.b01.azurefd.net`
- **No action is required** if you download reports using links from the dashboard UI.

To review the current set of allowed domains, refer to the [GitHub Copilot allowlist reference documentation](https://docs.github.com/copilot/reference/copilot-allowlist-reference).

For more information or to discuss this change, join the conversation in [GitHub Community announcements](https://github.com/orgs/community/discussions/categories/announcements).

## Summary

- API endpoints now use an updated CDN domain for report downloads.
- Network administrators must update allowlists for continued uninterrupted access.
- End users downloading via UI require no changes.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-26-copilot-metrics-report-urls-update)
