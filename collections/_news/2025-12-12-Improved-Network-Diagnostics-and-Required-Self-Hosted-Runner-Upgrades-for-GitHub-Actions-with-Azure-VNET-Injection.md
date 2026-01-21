---
external_url: https://github.blog/changelog/2025-12-12-better-diagnostics-for-vnet-injected-runners-and-required-self-hosted-runner-upgrades
title: Improved Network Diagnostics and Required Self-Hosted Runner Upgrades for GitHub Actions with Azure VNET Injection
author: Allison
feed_name: The GitHub Blog
date: 2025-12-12 14:12:05 +00:00
tags:
- Actions
- Azure VNET Injection
- CI/CD
- Connectivity Issues
- Developer Automation
- Docker
- GitHub Actions
- Improvement
- Infrastructure
- Linux
- Network Diagnostics
- Runner Architecture
- Self Hosted Runner
- System Upgrade
- Ubuntu 24.04
- Version Requirement
- Workflow Reliability
section_names:
- azure
- devops
---
Allison informs readers about the latest improvements in diagnosing network issues for GitHub-hosted runners with Azure VNET injection, and outlines required upgrades for self-hosted runners to ensure compatibility and security.<!--excerpt_end-->

# Improved Network Diagnostics and Required Self-Hosted Runner Upgrades for GitHub Actions with Azure VNET Injection

## Enhanced Diagnostics for Azure VNET-Injected Runners

GitHub-hosted larger runners configured for Azure private networking (using Azure VNET-injection) now offer expanded network diagnostics:

- **Per-endpoint visibility:** Easily drill down to specific endpoints to determine where connectivity failures are occurring.
- **Connection metrics:** Detailed data on connection attempts, failure rates, and success percentages for better insight into runner health.
- **Failure classification:** Pinpoint specific failure types such as timeouts, proxy issues, DNS resolution errors, TLS interception difficulties, and blocked domains.

These new diagnostics help developers and DevOps teams quickly identify and resolve Azure network configuration problems without requiring GitHub support intervention.

## Required Upgrade for Self-Hosted Runners

GitHub Actions is being rearchitected to improve reliability, scalability, and deliver new CI/CD features. As part of this change:

- **Minimum Version Requirement:** Starting January 15, 2026, self-hosted runners must use v2.329.0 or later for configuration. Older versions will be blocked and unable to connect to GitHub Actions.
- **Upgrade Process Change:** Previously, legacy runners could self-upgrade after registration. From this date, runners must already meet the minimum version before they can register.
- **Action item:** Upgrade all self-hosted runners to v2.329.0 or later as soon as possible to avoid workflow disruptions. [Runner documentation](https://docs.github.com/actions/reference/runners/self-hosted-runners#runner-software-updates-on-self-hosted-runners) offers upgrade guidance.

## Actions Runner Docker Image Update

- The Docker image for the Actions runner will change from Ubuntu 22.04 to Ubuntu 24.04 in v2.331.0, scheduled for January 2026.
- This update provides newer system libraries, security patches, and better compatibility with modern tools.
- Developers using custom Docker images based on the actions runner should review and update dependencies for Ubuntu 24.04 support; see [pull request #4018](https://github.com/actions/runner/pull/4018) for details.

---

These updates increase the reliability and transparency of CI/CD workflows using GitHub Actions in Azure environments and ensure developers benefit from the latest security patches and features.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-12-better-diagnostics-for-vnet-injected-runners-and-required-self-hosted-runner-upgrades)
