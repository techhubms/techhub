---
layout: post
title: 'GitHub Availability Report: June 2025'
author: Jakub Oleksy
canonical_url: https://github.blog/news-insights/company-news/github-availability-report-june-2025/
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/feed/
date: 2025-07-16 21:06:17 +00:00
permalink: /github-copilot/news/GitHub-Availability-Report-June-2025
tags:
- Claude Models
- Company News
- Copilot Chat
- Copilot Coding Agent
- Error Rates
- Gemini Models
- GitHub Actions
- GitHub Availability Report
- Incident Report
- JetBrains IDEs
- Mitigation
- Network Routing
- News & Insights
- Service Outage
- Site Reliability
- VS Code
section_names:
- ai
- devops
- github-copilot
---
Jakub Oleksy reviews service incidents that impacted GitHub, including Copilot and Actions, in the June 2025 Availability Report.<!--excerpt_end-->

## Summary of the June 2025 GitHub Availability Report

Authored by Jakub Oleksy, this report provides a month-overview of three significant incidents affecting GitHub’s platform and its users in June 2025. The focus is on how these disruptions impacted platform reliability, including detailed breakdowns of root causes, mitigation steps, and plans for future prevention.

### Incident 1: June 5, 2025 – Actions Service Degradation

- **Timeframe:** 17:47 to 19:20 UTC (1 hour, 33 minutes)
- **Impact:**
  - 47.2% of Actions runs experienced start delays averaging 14 minutes
  - 21% of Actions runs failed
  - 60% of Copilot Coding Agent sessions were cancelled
  - Branch-based GitHub Pages builds failed to deploy
- **Root Cause:** A spike in internal load exposed a configuration error, causing request throttling on the critical path for Actions run starts.
- **Mitigation:** Service configuration was corrected and deployment processes were updated to retain proper settings, preventing future recurrences.

### Incident 2: June 12, 2025 – Copilot Model Provider Outage

- **Timeframe:** 17:55 to 21:07 UTC (3 hours, 12 minutes)
- **Impact:**
  - Copilot was degraded, with Gemini models unavailable and Claude models experiencing reduced availability
  - Elevated error rates, slow responses, and timeouts for chat completions
  - Service disruptions across multiple IDEs (VS Code, JetBrains, Copilot Chat)
- **Root Cause:** Outage at a Copilot model provider
- **Mitigation:** Impacted endpoints were temporarily disabled; plans set to enhance incident response playbooks and monitoring for third-party dependencies.

### Incident 3: June 17, 2025 – Network Routing Policy Deployment Failure

- **Timeframe:** 19:32 to 20:03 UTC (31 minutes)
- **Impact:**
  - Authenticated users saw 3-4% error rates on the UI
  - API calls for authenticated users failed at a 40% rate
  - Unauthenticated UI and API requests nearly all failed
  - Delays and failures in Actions runs, and minor errors for LFS requests
- **Root Cause:** A flawed internal deployment of a routing policy led to network segmentation and inaccessible services.
- **Mitigation:** The deployment was rolled back, systems were restored, and validation steps for network policy changes were expanded.

### Insights and Continuous Improvement

- Each incident is followed up with practical, technical mitigations and an emphasis on improved configuration, response playbooks, and monitoring.
- GitHub encourages users to monitor the [GitHub Status page](https://www.githubstatus.com/) for real-time updates.

### Conclusion

This availability report demonstrates GitHub’s commitment to transparency and continuous reliability improvements. The detailed incident analyses inform users about operational challenges while outlining clear steps taken to reduce repetition and impact in the future.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/news-insights/company-news/github-availability-report-june-2025/)
