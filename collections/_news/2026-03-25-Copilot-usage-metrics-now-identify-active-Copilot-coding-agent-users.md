---
author: Allison
date: 2026-03-25 18:49:10 +00:00
external_url: https://github.blog/changelog/2026-03-25-copilot-usage-metrics-now-identify-active-copilot-coding-agent-users
title: Copilot usage metrics now identify active Copilot coding agent users
primary_section: github-copilot
tags:
- '@copilot'
- 28 Day Report
- Adoption Tracking
- Agent Mode
- AI
- API Response
- Copilot
- Copilot Coding Agent
- Copilot Usage Metrics
- Daily Report
- Enterprise Admins
- GitHub Copilot
- GitHub Enterprise Cloud
- IDE Metrics
- Improvement
- Issues
- News
- Organization Admins
- Pull Requests
- REST API
- Used Copilot Coding Agent
feed_name: The GitHub Blog
section_names:
- ai
- github-copilot
---

Allison announces an update to GitHub Copilot usage metrics that lets enterprise and organization admins see which users are actively using Copilot coding agent, via a new user-level API field and reporting support.<!--excerpt_end-->

## Summary

Copilot usage metrics now indicate which users have **Copilot coding agent (CCA)** activity. This helps **enterprise and organization admins** track Copilot adoption beyond IDE usage.

## What changed

In the Copilot usage metrics REST API response, CCA usage is now exposed at the **user level**:

- Field: `used_copilot_coding_agent`
- API docs: https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2026-03-10
- Concepts/docs: https://docs.github.com/copilot/concepts/copilot-usage-metrics/copilot-metrics

## What counts as Copilot coding agent activity

A user is considered to have triggered a Copilot coding agent session when they:

- Assign Copilot to an issue
- Tag `@copilot` in a pull request comment

## Why it matters

With this update, you can:

- Track Copilot coding agent adoption across your enterprise/org alongside existing IDE metrics.
- Distinguish between IDE agent mode usage (`used_agent`) and Copilot coding agent usage (`used_copilot_coding_agent`) for clearer reporting.
- Better understand how teams are using Copilot beyond the IDE to plan and code more autonomously.

## Next steps

- Review the API documentation: https://docs.github.com/enterprise-cloud@latest/rest/copilot/copilot-usage-metrics?apiVersion=2026-03-10
- Follow community discussion/announcements: https://github.com/orgs/community/discussions/categories/announcements


[Read the entire article](https://github.blog/changelog/2026-03-25-copilot-usage-metrics-now-identify-active-copilot-coding-agent-users)

