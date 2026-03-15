---
external_url: https://github.blog/news-insights/company-news/github-availability-report-february-2026/
title: 'GitHub Availability Report: Service Outages and Performance Incidents in February 2026'
author: Jakub Oleksy
primary_section: github-copilot
feed_name: The GitHub Blog
date: 2026-03-12 03:23:54 +00:00
tags:
- AI
- Automation
- CI/CD
- CodeQL
- Company
- Company News
- Dependabot
- Developer Workflows
- DevOps
- GitHub
- GitHub Actions
- GitHub Availability Report
- GitHub Codespaces
- GitHub Copilot
- Incident Report
- Infrastructure
- Monitoring
- News
- News & Insights
- Platform Reliability
- Root Cause Analysis
- Service Outage
section_names:
- ai
- devops
- github-copilot
---
Jakub Oleksy presents a thorough incident report detailing GitHub's six major outages in February 2026, with insights on DevOps impact, Copilot disruptions, and ongoing platform reliability improvements.<!--excerpt_end-->

# GitHub Availability Report: Service Outages and Performance Incidents in February 2026

**Author:** Jakub Oleksy

In February 2026, GitHub experienced six significant incidents that degraded performance and impacted various developer services. This availability report provides a transparent summary of each incident, their causes, how GitHub responded, and what steps are being taken to enhance reliability going forward.

## Overview

The outages affected core developer tools and workflows, disrupting:

- GitHub Actions (CI/CD automation)
- GitHub Codespaces (cloud development environments)
- GitHub Copilot (AI-powered code assistant)
- Pull request automation via Dependabot
- Git operations and repository access

## Incident Summaries

### 1. February 2 (17:41 UTC, 1h 5m)

- **Service Impacted:** Dependabot failed to create 10% of automated pull requests due to a cluster failover tied to a read-only database.
- **Mitigation:** Paused queues and rerouted traffic; failed jobs were restarted. Monitoring and alerting improvements were implemented.

### 2. February 2 (19:03 UTC, 5h 53m)

- **Services Impacted:** Hosted runners for GitHub Actions and Codespaces were unavailable. Copilot, CodeQL, Dependabot, and GitHub Pages also saw outages. All regions and runner types were impacted, but self-hosted runners were unaffected.
- **Root Cause:** Telemetry loss led to incorrect security policies on backend storage, blocking critical VM operations.
- **Mitigation:** Policy rollback and VM recovery. Improved incident response processes set in motion.

### 3. & 4. February 9 (Two Related Incidents, Total 2h 43m)

- **Impact:** Degraded availability across github.com, the API, Actions, GitHub Copilot, Git, and related services. Users saw errors with web access, Git operations, and workflow runs.
- **Root Cause:** Misconfigured caching mechanism led to infrastructure overload and connection exhaustion. Resolved by disabling cache rewrites and restarting affected components.
- **Improvements:** Optimization of the caching process, safeguards, and proxy layer resilience.

### 5. February 12 (07:53 UTC, 2h 3m)

- **Impact:** Codespaces provisioning failures (up to 90%) across EU, Asia, Australia, due to an authorization claim change.
- **Mitigation:** Enhanced validation, improved monitoring thresholds, better failover automation.

### 6. February 12 (10:38 UTC, 34m)

- **Service Impacted:** Repository archive downloads (with LFS objects) failed for some requests due to network misconfiguration.
- **Mitigation:** Applied fixes, strengthened validation and auto-rollback detection.

## Lessons Learned and Improvements

- Proactive monitoring and alerting investments to reduce detection latency
- Improved incident response plans and process engagement with compute providers
- Hardening of backend components, especially around automation and scaling
- Enhanced safeguards around configuration and change management

## Further Information

- [Addressing GitHub’s Recent Availability Issues](https://github.blog/news-insights/company-news/addressing-githubs-recent-availability-issues-2/)
- [GitHub Status Page](https://www.githubstatus.com/)
- [Engineering Updates on the GitHub Blog](https://github.blog/category/engineering/)

*Jakub Oleksy and the engineering team thank users for their patience as they continue working to improve GitHub’s resilience and developer experience.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/news-insights/company-news/github-availability-report-february-2026/)
