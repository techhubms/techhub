---
section_names:
- ai
- devops
- github-copilot
feed_name: The GitHub Blog
external_url: https://github.blog/news-insights/company-news/github-availability-report-march-2026/
tags:
- AI
- Authentication
- Caching
- Company News
- Copilot Coding Agent
- Credential Rotation
- Degraded Performance
- DevOps
- GitHub Actions
- GitHub API
- GitHub Availability Report
- GitHub Copilot
- GitHub Status
- Incident Postmortem
- Load Balancer Configuration
- Microsoft Teams Integration
- Monitoring And Alerting
- News
- News & Insights
- Observability
- Operational Runbooks
- Redis
- Replication Delays
- Rollback
- Service Availability
- Webhooks And Notifications
primary_section: github-copilot
title: 'GitHub availability report: March 2026'
date: 2026-04-09 02:21:17 +00:00
author: Jakub Oleksy
---

Jakub Oleksy summarizes four March 2026 GitHub incidents, including outages affecting github.com/API, GitHub Actions, and GitHub Copilot/Copilot Coding Agent, with root causes (caching, Redis config, auth/credentials) and concrete mitigations like rollbacks, improved monitoring, and configuration safeguards.<!--excerpt_end-->

# GitHub availability report: March 2026

In March 2026, GitHub experienced four incidents that caused degraded performance across multiple GitHub services.

## March 03 18:59 UTC (lasting 1 hour and 10 minutes)

**Time window:** March 3, 2026, 18:46–20:09 UTC

**Customer impact (peak/observed error rates):**

- github.com request failures peaked at **~40%**
- GitHub API request failures were **~43%**
- Git operations over **HTTP** had an error rate of **~6%**
  - **SSH was not impacted**
- GitHub Copilot requests had an error rate of **~21%**
- GitHub Actions experienced **<1%** impact

**What happened (root cause):**

- The incident had the same underlying cause as an early February incident: a large volume of writes to the **user settings caching mechanism**.
- While deploying a change intended to reduce the burden of these writes, a **bug** caused **every user’s cache** to:
  - expire
  - get recalculated
  - get rewritten
- The resulting load led to **replication delays**, which then cascaded to affected services.

**Mitigation:**

- Immediate **rollback** of the faulty deployment.

**Immediate follow-up actions:**

- Added a **killswitch** and improved **monitoring** for the caching mechanism to detect issues before user impact.
- Moving the cache mechanism to a **dedicated host** so future issues are isolated to dependent services.

## March 05 16:35 UTC (lasting 2 hours and 55 minutes)

**Time window:** March 5, 2026, 16:24–19:30 UTC

**Customer impact:**

- **95%** of workflow runs failed to start within **5 minutes**
  - average delay: **30 minutes**
- **10%** of workflow runs failed with an **infrastructure error**

**What happened (root cause):**

- GitHub was rolling out **Redis infrastructure updates** to improve resiliency.
- Those updates introduced incorrect configuration changes into the **Redis load balancer**.
- Internal traffic was routed to an **incorrect host**, leading to two incidents during the window.

**Mitigation and recovery:**

- Corrected the misconfigured load balancer.
- Actions jobs were running successfully starting at **17:24 UTC**.
- Remaining time was spent burning through the queued jobs.

**Follow-up actions:**

- Rolled back contributing updates and **froze changes** in this area until follow-up work completes.
- Improving automation so incorrect configuration changes cannot propagate.
- Improving alerting to catch misconfigured load balancers earlier.
- Updating the Redis client configuration in Actions to improve resiliency to brief cache interruptions.

## March 19 13:44 UTC (lasting 48 minutes)

This section covers two related incidents affecting Copilot Coding Agent.

**Time windows:**

- March 19, 2026, **01:05–02:52 UTC**
- March 20, 2026, **00:42–01:58 UTC**

**Customer impact:**

- Users could not start new **Copilot Agent sessions** or view existing ones.
- First incident:
  - average error rate **~53%**
  - peak error rate **~93%**
- Second incident:
  - average error rate **~99%**
  - peak error rate **~100%**
  - included significant **retry amplification**

**What happened (root cause):**

- Both incidents were caused by the same underlying **system authentication issue**.
- The issue prevented the service from connecting to its backing datastore.

**Mitigation:**

- Rotated affected credentials to restore connectivity and normalize error rates.
- Mitigation time: **01:24**.
- The second occurrence was due to **incomplete remediation** of the first.

**Follow-up actions:**

- Implemented automated monitoring for credential lifecycle events.
- Improving operational processes to reduce time to detection/mitigation for similar issues.

## March 24 16:59 UTC (lasting 2 hours and 52 minutes)

**Time window:** March 24, 2026, 15:57–19:51 UTC

**Customer impact:**

- **Microsoft Teams Integration** and **Teams Copilot Integration** services were degraded.
- GitHub event notifications failed to deliver to Microsoft Teams.
- Average error rate: **37.4%**
- Peak error rate: **90.1%**
- Approximately **19%** of all integration installs failed to receive GitHub-to-Teams notifications during the incident.

**What happened (root cause):**

- An outage at an **upstream dependency** caused HTTP **500** errors and connection resets for the Teams integration.

**Mitigation and recovery:**

- Coordinated with relevant service teams.
- Resolved at **19:51 UTC** when the upstream incident was mitigated.

**Follow-up actions:**

- Updating observability and runbooks to reduce time to mitigation.

## Links

- Status page for real-time updates: https://www.githubstatus.com/
- GitHub Blog engineering section: https://github.blog/category/engineering/
- Tag: https://github.blog/tag/github-availability-report/

## Written by

Jakub Oleksy

![GitHub avatar of Jakub Oleksy](https://avatars.githubusercontent.com/u/6147691?v=4&s=200)

[Read the entire article](https://github.blog/news-insights/company-news/github-availability-report-march-2026/)

