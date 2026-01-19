---
external_url: https://github.blog/news-insights/company-news/github-availability-report-december-2025/
title: 'GitHub Availability Report: December 2025'
author: Jakub Oleksy
viewing_mode: external
feed_name: The GitHub Blog
date: 2026-01-14 22:06:49 +00:00
tags:
- API Reliability
- Availability Engineering
- Cloud Infrastructure
- Company News
- Copilot Code Review
- Copilot Policies
- GitHub Actions
- GitHub Availability Report
- Incident Analysis
- Incident Mitigation
- Kafka
- Monitoring
- News & Insights
- Operational Resilience
- Platform Engineering
- Queue Management
- Service Degradation
section_names:
- ai
- devops
- github-copilot
---
Jakub Oleksy provides a comprehensive account of GitHub's service stability in December 2025, covering incidents affecting GitHub Actions, Copilot, and core platform availability, along with mitigations and improvements.<!--excerpt_end-->

# GitHub Availability Report: December 2025

*Author: Jakub Oleksy*

In December 2025, GitHub experienced five separate incidents leading to degraded performance in various services. This report outlines the issues, root causes, mitigation steps, and future improvements for each event.

## December 08: Enterprise AI Controls Agent Session Disruption

- **Timeline:** Nov 26 – Dec 8, 2025
- **Impact:** Enterprise admins unable to view agent session activities in the Enterprise AI Controls page. Other audit mechanisms unaffected.
- **Root Cause:** Misconfiguration in a deployment prevented data flow to a Kafka topic feeding the AI Controls page.
- **Mitigation:** Configuration was corrected on Dec 8. Monitoring for pipeline dependencies and pre-deployment checks are being improved.

## December 15: Copilot Code Review Service Degradation

- **Timeline:** Dec 15, 2025
- **Impact:** Nearly 47% of Copilot Code Review requests failed and needed manual retriggering. Error message: “Copilot encountered an error... You can try again by re-requesting a review.”
- **Root Cause:** Latency in a model-backed dependency led to worker queue backlog and timeouts.
- **Mitigation:** Temporarily bypassed fix suggestions, increased worker capacity, and tuned model configuration for lower latency. Further capacity, instrumentation, and alerting improvements were initiated.

## December 18: GitHub Actions Runner API Failures

- **Timeline:** Dec 18, 2025
- **Impact:** Runners in the West US region saw setup and workflow failures due to API call timeouts (impacted ~0.28% of Actions jobs).
- **Root Cause:** Network packet loss between runners and an edge site.
- **Mitigation:** Rerouted traffic away from the affected edge site. Plans underway to improve cross-cloud connectivity monitoring and mitigation.

## December 18: Copilot Policy Update Failures

- **Timeline:** Dec 18, 2025
- **Impact:** Users unable to update Copilot-related policies; other GitHub and Copilot services were unaffected.
- **Root Cause:** Schema drift following a database migration.
- **Mitigation:** Schema resynchronized and service hardening completed. Further improvements to deployment pipelines planned.

## December 22: Degraded Unauthenticated Requests

- **Timeline:** Dec 22, 2025
- **Impact:** Unauthenticated github.com and API requests were slow or timed out, including Actions release downloads. Authenticated usage not impacted.
- **Root Cause:** Sudden spike in traffic, especially to search endpoints.
- **Mitigation:** Identified and mitigated traffic spike, improved endpoint traffic limiters, and started work on faster detection of large traffic changes.

---

**Further Information**

- [GitHub Status Page](https://www.githubstatus.com/)
- [GitHub Blog Engineering Section](https://github.blog/category/engineering/)

---

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/news-insights/company-news/github-availability-report-december-2025/)
