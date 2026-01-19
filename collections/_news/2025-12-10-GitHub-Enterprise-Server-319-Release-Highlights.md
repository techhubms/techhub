---
layout: post
title: GitHub Enterprise Server 3.19 Release Highlights
author: Allison
canonical_url: https://github.blog/changelog/2025-12-10-github-enterprise-server-3-19-is-now-generally-available
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-12-10 16:20:41 +00:00
permalink: /devops/news/GitHub-Enterprise-Server-319-Release-Highlights
tags:
- App Managers
- Collaboration Tools
- Enterprise Collaboration
- GHES 3.19
- GitHub Actions
- GitHub Enterprise Server
- Monitoring
- OpenTelemetry
- Policy Enforcement
- Repository Policies
- Ruleset Management
- SHA Pinning
- SSH Ciphers
- TLS Ciphers
- Workflow Automation
section_names:
- devops
- security
---
Allison presents a summary of GitHub Enterprise Server 3.19's new features, covering updates to repository creation, security, policy management, and administrative roles for enterprise developers and administrators.<!--excerpt_end-->

# GitHub Enterprise Server 3.19 Release Highlights

GitHub Enterprise Server (GHES) 3.19 introduces several updates designed to streamline enterprise development workflows, bolster security, and enhance monitoring and policy enforcement.

## Key New Features

- **Repository Creation Flow**: An improved, user-friendly interface enables administrators to collect and enforce repository metadata and custom properties at creation. This supports consistent configuration across teams. [More details](https://github.blog/changelog/2025-08-26-improved-repository-creation-generally-available-plus-ruleset-insights-improvements/).

- **Advanced Ruleset Management**:
  - Ruleset history provides versioning and rollback capability.
  - Admins can import/export rulesets for easier sharing and application, including templates from [GitHub’s ruleset-recipes](https://github.com/github/ruleset-recipes).
  - Read the [documentation](https://docs.github.com/enterprise-server@3.19/repositories/configuring-branches-and-merges-in-your-repository/managing-rulesets/managing-rulesets-for-a-repository) for more.

- **Workflow Security - SHA Pinning & Blocking**:
  - Administrators can require workflows to only use actions referenced by SHA, blocking unapproved or mutable actions.
  - These policies help maintain workflow security and supply chain integrity. [Learn more](https://github.blog/changelog/2025-08-15-github-actions-policy-now-supports-blocking-and-sha-pinning-actions/).

- **Expanded App Manager Roles**:
  - Organizations and teams can now be assigned as GitHub App managers, with granular role support. App managers may update app registrations but cannot manage installations.
  - Refer to [GitHub App Managers](https://docs.github.com/enterprise-server@3.19/apps/maintaining-github-apps/about-github-app-managers) for details.

- **Metrics Transition**:
  - OpenTelemetry metrics are now enabled by default for new installations, phasing out Collectd over coming releases.
  - Metrics setup can be changed per organizational needs. Documentation is available [here](https://docs.github.com/enterprise-server@3.19/admin/monitoring-and-managing-your-instance/monitoring-your-instance/opentelemetry-metrics).

- **SSH and TLS Cipher Suite Configuration**:
  - Administrators can now customize cipher suites for SSH and TLS, enabling the exclusion of weak ciphers and the enforcement of enterprise security standards. Details can be found [here](https://docs.github.com/enterprise-server@3.19/admin/configuring-settings/hardening-security-for-your-enterprise/configuring-tls-and-ssh-ciphers).

## Additional Resources

- [Release Notes](https://docs.github.com/enterprise-server@3.19/admin/release-notes)
- [GHES 3.19 Download](https://enterprise.github.com/releases/3.19.0/download)
- [Support Team](https://support.github.com/features/enterprise-administrators-server)
- [Community Discussion](https://github.com/orgs/community/discussions/181173)

These improvements in repository management, workflow security, monitoring, and app administration provide technical teams with more control, better traceability, and stronger security compliance.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-10-github-enterprise-server-3-19-is-now-generally-available)
