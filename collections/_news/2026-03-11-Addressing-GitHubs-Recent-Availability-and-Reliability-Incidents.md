---
external_url: https://github.blog/news-insights/company-news/addressing-githubs-recent-availability-issues-2/
title: Addressing GitHub's Recent Availability and Reliability Incidents
author: Vlad Fedorov
primary_section: azure
feed_name: The GitHub Blog
date: 2026-03-11 21:41:51 +00:00
tags:
- Availability
- Azure
- Azure Migration
- Cloud Migration
- Company News
- Database Scaling
- DevOps
- Failover
- GitHub
- GitHub Actions
- Incident Analysis
- Incident Response
- Infrastructure Resilience
- Load Shedding
- News
- News & Insights
- Platform Engineering
- Platform Outages
- Service Isolation
- Site Reliability Engineering
section_names:
- azure
- devops
---
Vlad Fedorov discusses the recent series of GitHub outages, pinpointing their technical causes and highlighting steps—such as a major migration to Azure—that GitHub's engineering team is taking to improve reliability and incident response for developers.<!--excerpt_end-->

# Addressing GitHub’s Recent Availability Incidents

**Author:** Vlad Fedorov

GitHub recently faced several high-impact outages that affected multiple services, including authentication and GitHub Actions. This post delivers a transparent, technical breakdown of the main incidents, investigates their root causes, and details the ongoing stabilization and architectural improvements being implemented.

## Incident Breakdown

- **Period Affected:** February and March
- **Key Incidents:**
  - **February 2:** Actions hosted runners outage due to security policy propagation to all regions, blocking access to VM metadata and halting runner operations
  - **February 9:** Authentication/user management database cluster overload from a sudden spike in client application API calls and a cache policy change, resulting in widespread disruption
  - **March 5:** Redis cluster failover for Actions job orchestration exposed a configuration issue, leaving no writable primary and interrupting service

## Technical Root Causes

- **Scaling Limitations & Load Growth:** Rapid adoption of client applications increased load on core database clusters beyond original design.
- **Architectural Coupling:** Insufficient isolation between services led to localized failures affecting critical systems.
- **Load Shedding Deficiency:** Inadequate systems to gracefully throttle or block problematic traffic.
- **Failover & Monitoring Challenges:** Gaps in failover procedures and insufficient alarming/validation extended outages.

## Immediate Stabilization and Remediation

1. **Redesigning User Cache:** Migrating policies/user data to segmented clusters for scalability
2. **Capacity Planning:** Full audit of data and compute infrastructure to support growth
3. **Dependency Isolation:** Separating systems so issues remain localized
4. **Load Protection:** Prioritizing and safeguarding critical traffic from cascading effects

## Strategic Platform Improvements

- **Migration to Azure:**
  - Hybrid migration with 12.5% of traffic already served from Azure Central US
  - On track to 50% of traffic by July, improving regional scaling and resilience
  - Long-term goal: More global resiliency and leverage managed cloud services for simplification
- **Decomposition of Monolith:**
  - Breaking up services for independent scaling and management

## Transparency Commitment

GitHub is strengthening transparency by publishing incident reports and monthly availability updates. The unresolved incidents are driving ongoing architectural investments to ensure digital infrastructure supporting millions of developers becomes even more resilient and reliable.

For further updates, visit the [GitHub Status page](https://www.githubstatus.com/) and [monthly availability report](https://github.blog/tag/github-availability-report/).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/news-insights/company-news/addressing-githubs-recent-availability-issues-2/)
