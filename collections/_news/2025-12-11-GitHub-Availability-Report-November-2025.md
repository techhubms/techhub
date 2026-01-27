---
external_url: https://github.blog/news-insights/company-news/github-availability-report-november-2025/
title: 'GitHub Availability Report: November 2025'
author: Jakub Oleksy
feed_name: The GitHub Blog
date: 2025-12-11 16:03:53 +00:00
tags:
- Automation
- Availability
- Claude Sonnet 4.5
- Company News
- Copilot
- Dependabot
- GHCR
- Git Operations
- GitHub
- GitHub Availability Report
- Incident Mitigation
- Incident Response
- Monitoring
- News & Insights
- Service Outage
- TLS Certificate
section_names:
- devops
primary_section: devops
---
Jakub Oleksy summarizes November 2025's incidents affecting GitHub's key services, detailing the root causes, mitigations, and ongoing reliability improvements crucial for developers and DevOps professionals.<!--excerpt_end-->

# GitHub Availability Report: November 2025

Author: Jakub Oleksy

This report highlights three incidents that impacted the availability and performance of GitHub services during November 2025. Each incident is detailed with its timeline, cause, resolution, and the steps being taken to improve service reliability.

## November 17: Dependabot Rate Limiting (16:52 – 19:08 UTC)

- **Impact:** About 57% of Dependabot jobs on GitHub Container Registry (GHCR) failed to complete within the service level objective (SLO) due to rate limiting.
- **Mitigation:** GitHub reduced the rate at which Dependabot jobs started and increased GHCR's rate limit. These actions allowed the queue to recover and resolved the incident.
- **Follow-up:** New monitors and alerting have been introduced to catch similar rate issues early.

## November 18: Git Operations Failures (20:30 – 21:34 UTC)

- **Impact:** All Git operations (SSH/HTTP) and raw file access failed, also affecting dependent products.
- **Root Cause:** An internal TLS certificate expired, disrupting service-to-service authentication.
- **Mitigation:** The certificate was replaced and impacted services were restarted, restoring functionality.
- **Follow-up:** Alerting now covers certificate expirations, and GitHub is auditing other certificates and automating service-to-service certificates to avoid manual lapses.

## November 28: Copilot Outage—Claude Sonnet 4.5 Model (05:59 – 08:24 UTC)

- **Impact:** The Copilot Claude Sonnet 4.5 model was unavailable, triggering HTTP 400 errors for users until they switched to another model. All other models remained operational.
- **Root Cause:** A misconfigured internal deployment made Claude Sonnet 4.5 unavailable.
- **Mitigation:** Reverting the configuration change resolved the incident.
- **Follow-up:** Work is underway to strengthen cross-service deployment safeguards to avoid similar issues.

For ongoing updates, visit the [GitHub status page](https://www.githubstatus.com/). For further insights into reliability engineering and incident management, explore the [engineering section of the GitHub Blog](https://github.blog/category/engineering/).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/news-insights/company-news/github-availability-report-november-2025/)
