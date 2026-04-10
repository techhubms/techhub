---
title: Copilot CLI activity now included in usage metrics totals and feature breakdowns
tags:
- Account Management
- AI
- Copilot
- Copilot CLI
- Copilot Usage Metrics API
- Dashboards
- Dimensional Breakdowns
- Enterprise Cloud
- Enterprise Management Tools
- Enterprise Reports
- GitHub Copilot
- Improvement
- News
- Organization Reports
- Per User Reports
- Reporting Thresholds
- Request Counts
- REST API
- Session Counts
- Token Usage
- Totals By CLI
- Totals By Feature
- Totals By IDE
- Usage Metrics
feed_name: The GitHub Blog
author: Allison
primary_section: github-copilot
section_names:
- ai
- github-copilot
date: 2026-04-10 20:03:28 +00:00
external_url: https://github.blog/changelog/2026-04-10-copilot-cli-activity-now-included-in-usage-metrics-totals-and-feature-breakdowns
---

Allison explains an update to the GitHub Copilot usage metrics API where Copilot CLI activity is now included in top-level totals and feature breakdowns, changing how enterprise and organization dashboards should interpret usage fields.<!--excerpt_end-->

# Copilot CLI activity now included in usage metrics totals and feature breakdowns

Previous releases added a standalone `totals_by_cli` section to the Copilot usage metrics API to report Copilot CLI activity (session counts, request counts, and token usage). That CLI section was **separate** from the rest of the report—meaning top-level totals and dimensional breakdowns reflected **IDE activity only**.

With this update, **CLI activity is integrated into the metrics you already use**.

## What’s new

### Top-level totals now include CLI contributions
The following fields now represent **IDE + CLI combined**:

- `code_generation_activity_count`
- `code_acceptance_activity_count`
- `user_initiated_interaction_count`
- `loc_added_sum`
- `loc_deleted_sum`

### CLI appears in dimensional breakdowns
CLI activity now appears as `feature=copilot_cli` in:

- `totals_by_feature`
- `totals_by_model_feature`
- `totals_by_language_feature`
- `totals_by_language_model`

### What did not change

- CLI is **excluded** from `totals_by_ide`.
- The existing `totals_by_cli` section remains unchanged.
- The per-user CLI fields (added in recent releases) remain unchanged.

## Scope of impact

This affects **single-day and 28-day** reports for:

- Enterprise
- Organization
- Per-user

## Why this matters

- **Unified view of Copilot usage**: Top-level totals now reflect all Copilot activity across surfaces (IDE + CLI), avoiding undercounting for organizations using Copilot CLI.
- **Compare CLI alongside other features**: Since CLI is included in `totals_by_feature`, admins can compare CLI usage distribution against other Copilot capabilities (like code completions).
- **No more manual stitching**: You no longer need to combine `totals_by_cli` with IDE-only totals to get a complete picture—the API handles it.

## Important notes

- **Top-level totals have changed meaning**: If your dashboards assumed those fields were **IDE-only**, your numbers will increase due to CLI contributions. Revisit thresholds, alerts, and comparisons that depend on these values.

## References

- Copilot usage metrics API documentation: https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2026-03-10
- GitHub Community announcements: https://github.com/orgs/community/discussions/categories/announcements


[Read the entire article](https://github.blog/changelog/2026-04-10-copilot-cli-activity-now-included-in-usage-metrics-totals-and-feature-breakdowns)

