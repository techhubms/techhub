---
primary_section: devops
author: Allison
external_url: https://github.blog/changelog/2026-04-16-rule-insights-dashboard-and-unified-filter-bar
feed_name: The GitHub Blog
title: Rule insights dashboard and unified filter bar
tags:
- Alert Management
- Auditing
- Branch Protection
- Bypass Activity
- Code Scanning
- Custom Properties
- Dependabot
- DevOps
- Filter Bar
- GitHub
- Improvement
- Incident Response
- News
- Platform Governance
- Public Preview
- Push Protection
- Repository Rulesets
- Rule Evaluation
- Rule Insights Dashboard
- Secret Scanning
- Security
date: 2026-04-16 15:30:10 +00:00
section_names:
- devops
- security
---

Allison announces new GitHub improvements: a rule insights dashboard to visualize repository ruleset evaluations (successes, failures, bypasses) and a unified filter bar across alert dismissal and bypass request pages for code scanning, Dependabot, and secret scanning.<!--excerpt_end-->

## Rule insights dashboard

GitHub repository rulesets are powerful, but spotting trends (for example, spikes in blocked pushes during an incident, or patterns in bypass activity) previously required digging into the rule insights page.

A new **rule insights dashboard** is now available in **Repository Settings > Rules**. It provides a visual, high-level view of rule evaluation activity, including:

- Successes, failures, and bypasses over time
- The most active bypassers for your rulesets

![GitHub Rule Insights Dashboard screenshot showing rule suites performance with charts and counts for allowed, failed, and bypassed evaluations.](https://github.com/user-attachments/assets/9befa788-af48-4a9c-965f-96264697f4de)

Each chart links back to the rule insights page with filters prefilled, letting you drill into:

- Specific statuses (success/failure/bypass)
- Bypassers
- Time ranges

The goal is to make it easier to:

- Respond to incidents by quickly identifying trends (like blocked push spikes)
- Audit bypass activity without manually filtering and correlating data

## Unified filter bar for alert dismissal and bypass request pages

Building on earlier filter bar improvements (February), GitHub replaced custom dropdowns on several alert management pages with a **unified filter bar component**. This change affects:

- **GitHub code scanning** alert dismissal requests (enterprise and organization)
- **GitHub Dependabot** alert dismissal requests (enterprise and organization)
- **GitHub secret scanning** alert dismissals (enterprise and organization)
- **GitHub secret scanning push protection** bypass requests (enterprise, organization, and repository)

This provides a consistent filtering experience across these pages, including support for **custom properties**.

## Links and availability

- Learn more: GitHub Docs — Managing rulesets (About rulesets): https://docs.github.com/repositories/configuring-branches-and-merges-in-your-repository/managing-rulesets/about-rulesets
- Availability: **Public preview**

[Read the entire article](https://github.blog/changelog/2026-04-16-rule-insights-dashboard-and-unified-filter-bar)

