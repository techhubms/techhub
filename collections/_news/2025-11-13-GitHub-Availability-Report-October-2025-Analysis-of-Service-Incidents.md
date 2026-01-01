---
layout: "post"
title: "GitHub Availability Report: October 2025 – Analysis of Service Incidents"
description: "This report outlines four major incidents that occurred on GitHub services in October 2025, examining root causes, impacts on APIs, Codespaces, Actions, and Enterprise features, and the improvement steps GitHub is taking to enhance the reliability of its cloud infrastructure and developer workflows."
author: "Jakub Oleksy"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/news-insights/company-news/github-availability-report-october-2025/"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/feed/"
date: 2025-11-13 17:28:48 +00:00
permalink: "/2025-11-13-GitHub-Availability-Report-October-2025-Analysis-of-Service-Incidents.html"
categories: ["DevOps"]
tags: ["Availability", "CI/CD", "Cloud Infrastructure", "Codespaces", "Company News", "DevOps", "Enterprise Importer", "Error Rates", "GitHub", "GitHub Actions", "GitHub Availability Report", "Incident Report", "Mobile Push Notifications", "Monitoring", "Network Outage", "News", "News & Insights", "Resilience Engineering", "Service Degradation", "Site Reliability", "Third Party Dependencies"]
tags_normalized: ["availability", "cislashcd", "cloud infrastructure", "codespaces", "company news", "devops", "enterprise importer", "error rates", "github", "github actions", "github availability report", "incident report", "mobile push notifications", "monitoring", "network outage", "news", "news and insights", "resilience engineering", "service degradation", "site reliability", "third party dependencies"]
---

Jakub Oleksy presents a clear analysis of four major GitHub service incidents in October 2025, detailing their technical root causes and GitHub's ongoing efforts toward greater reliability.<!--excerpt_end-->

# GitHub Availability Report: October 2025

_Authored by Jakub Oleksy_

In October 2025, GitHub experienced four notable incidents that led to degraded service performance across multiple products. This availability report provides technical details on each event, its impact, and the remediation actions undertaken.

## Incident Summaries

### October 9 – Network Device Repair

- **Summary:** A network device was returned to production before repairs were complete, resulting in packet loss.
- **Impact:**
  - UI latency for authenticated users
  - API error rates peaked at 7.3%
  - GitHub Actions: 24% of runs delayed
  - LFS increased errors (0.038%)
- **Mitigation:** GitHub is improving validation for device repair workflows to avoid premature redeployment.

### October 17 – Mobile Push Notification Disruption

- **Summary:** A configuration error on cloud resources disrupted mobile push notification delivery for 70 minutes on github.com and Enterprise Cloud products.
- **Impact:** Notifications undelivered across all regions.
- **Mitigation:** A review of change management and procedures for cloud resource configuration is underway.

### October 20 – Codespaces Cascading Failure

- **Summary:** An outage in a third-party dependency for building devcontainer images caused major Codespaces creation and resume failures.
- **Impact:**
  - Codespaces creation error rate averaged 39.5%, peaking at 71%
  - Resume operations averaged 23.4%, peaking at 46%
- **Mitigation:** GitHub is planning to eliminate single points of dependency in container builds and enhance monitoring for quicker issue detection.

### October 29 – Widespread Third-Party Outage

- **Summary:** A global third-party provider outage led to severe Codespaces connection failures, impacting other services reliant on external resources.
- **Impact:**
  - Codespaces: 90–100% connection error rate
  - GitHub Actions: 0.5% workflow failures, 10% large runner job delays
  - Enterprise Importer: migration failures, git push delays
  - Trial provisioning delays for Enterprise Cloud Data Residency
  - Copilot Metrics API: ~100 failed requests
- **Mitigation:** GitHub applied mitigations and is devising strategies to reduce reliance on external providers and support graceful service degradation during outages.

## Ongoing Improvements

- Enhanced validation and change management for network devices and cloud resources
- Greater resilience against third-party outages via dependency reduction and improved alerting
- Enhanced detection and incident response capabilities

## Additional Resources

- [GitHub Status Page](https://www.githubstatus.com/) – real-time status updates and post-incident reports
- [GitHub Blog – Engineering Section](https://github.blog/category/engineering/) – insights into ongoing reliability initiatives

GitHub continues to refine its engineering processes to improve platform reliability for developers and enterprise users alike.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/news-insights/company-news/github-availability-report-october-2025/)
