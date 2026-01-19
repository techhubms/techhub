---
layout: post
title: 'GitHub Enterprise Server 3.19: New Release Candidate Features and Enhancements'
author: Allison
canonical_url: https://github.blog/changelog/2025-12-02-github-enterprise-server-3-19-release-candidate-is-now-available
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-12-02 20:55:34 +00:00
permalink: /devops/news/GitHub-Enterprise-Server-319-New-Release-Candidate-Features-and-Enhancements
tags:
- App Manager
- Collaboration Tools
- Enterprise Administration
- GHES 3.19
- GitHub Enterprise Server
- Monitoring
- OpenTelemetry
- Policy Enforcement
- Release Candidate
- Repository Management
- Ruleset Management
- SHA Pinning
- TLS Ciphers
- Version Control
section_names:
- devops
- security
---
Allison outlines the technical highlights in GitHub Enterprise Server 3.19, focusing on improved repository workflows, security policies, monitoring updates, and administration enhancements for enterprise DevOps teams.<!--excerpt_end-->

# GitHub Enterprise Server 3.19 Release Candidate: What's New

GitHub Enterprise Server (GHES) version 3.19 introduces several enhancements designed for enterprise administrators, developers, and DevOps teams seeking better deployment efficiency, stronger security, and improved policy and monitoring controls.

## Key Features and Updates

### 1. Repository Creation Improvements

- A new, modern interface for repository creation, allowing metadata enforcement, consistent policies, and improved workflow.
- Enhanced custom property collection and automatic application of repository rules at creation.
- [Learn more](https://github.blog/changelog/2025-08-26-improved-repository-creation-generally-available-plus-ruleset-insights-improvements/).

### 2. Policy and Ruleset Management

- **Ruleset history, import, and export:** Administrators can now efficiently track, roll back, and share rulesets, improving overall repository governance. Access pre-built [ruleset recipes](https://github.com/github/ruleset-recipes).
- [Managing rulesets documentation](https://docs.github.com/enterprise-server@3.19/repositories/configuring-branches-and-merges-in-your-repository/managing-rulesets/managing-rulesets-for-a-repository).

### 3. Enhanced Actions Security

- Administrators can block actions and require SHA pinning for workflows using public actions, mitigating risks by ensuring only vetted actions (by immutable SHAs) are used.
- [More details](https://github.blog/changelog/2025-08-15-github-actions-policy-now-supports-blocking-and-sha-pinning-actions/).

### 4. GitHub App Manager Roles

- Ability to designate users and teams as application managers of GitHub Apps at the organization level.
- Improved role-based access via the roles platform streamlines permission assignments.
- [About GitHub App Managers](https://docs.github.com/en/enterprise-server@3.19/apps/maintaining-github-apps/about-github-app-managers).

### 5. Metrics and Monitoring

- **OpenTelemetry metrics** are now enabled by default for new installations; Collectd is disabled by default but can be switched if needed.
- In upcoming releases, OpenTelemetry will be the only supported monitoring option.
- [OpenTelemetry metrics docs](https://docs.github.com/en/enterprise-server@3.19/admin/monitoring-and-managing-your-instance/monitoring-your-instance/opentelemetry-metrics).

### 6. Security: TLS and SSH Cipher Configuration

- Administrators can now customize which SSH and TLS ciphers are supported, improving security flexibility and helping avoid weak cryptographic suites.
- [Configuring cipher suites](https://docs.github.com/en/enterprise-server@3.19/admin/configuring-settings/hardening-security-for-your-enterprise/configuring-tls-and-ssh-ciphers).

### 7. Release Candidate Feedback

- Enterprises are encouraged to test drive the release candidate to explore features early and provide feedback for final stability.
- [Release notes](https://docs.github.com/enterprise-server@3.19/admin/release-notes)
- [Download 3.19 release candidate](https://enterprise.github.com/releases/3.19.0/download)
- [Contact support](https://support.github.com/features/enterprise-administrators-server)

## Summary

GHES 3.19 continues to streamline administration for enterprise environments, focusing on repository consistency, policy enforcement, security, and modern monitoring. These tools and roles help admins and development teams improve control over their software supply chain and infrastructure.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-02-github-enterprise-server-3-19-release-candidate-is-now-available)
