---
external_url: https://github.blog/changelog/2025-12-19-you-can-now-require-reviews-before-closing-dependabot-alerts-with-delegated-alert-dismissal
title: Require Reviews for Dependabot Alert Dismissal with Delegated Alert Dismissal in GitHub
author: Allison
feed_name: The GitHub Blog
date: 2025-12-19 20:15:36 +00:00
tags:
- Alert Dismissal
- Audit
- Code Scanning
- Code Security
- Compliance
- Delegated Alert Dismissal
- Dependabot
- GitHub
- GitHub Enterprise Server
- Improvement
- Review Process
- Secret Scanning
- Security Alerts
- Security Governance
- Supply Chain Security
- Vulnerability Management
- DevOps
- Security
- News
section_names:
- devops
- security
primary_section: devops
---
Allison outlines the new delegated alert dismissal feature for Dependabot in GitHub, enabling required reviews before dismissing alerts to improve security governance and compliance.<!--excerpt_end-->

# Require Reviews for Dependabot Alert Dismissal with Delegated Alert Dismissal in GitHub

## Overview

A new feature called **delegated alert dismissal** enables organizations to implement a formal review process before Dependabot alerts are closed. This enhancement is aimed at GitHub Code Security customers and is accessible both via the GitHub UI and API.

## Key Benefits

- **Increases Accountability:** Development teams must have a second set of eyes on dismissal actions, promoting responsible vulnerability management.
- **Prevents Insecure Actions:** Unauthorized or accidental alert dismissals are avoided with enforced reviews.
- **Enhances Scalability:** Provides governing tools to better audit and manage alert activity across numerous repositories.
- **Meets Compliance:** Aligns security processes with audit and regulatory requirements for software development.

## Availability

- **Platform:** Available on github.com and GitHub Enterprise Server version 3.21 for code security customers.
- **Controls:** Brings consistent governance as seen in [code scanning](https://docs.github.com/code-security/code-scanning/managing-your-code-scanning-configuration/enabling-delegated-alert-dismissal-for-code-scanning) and [secret scanning](https://docs.github.com/code-security/secret-scanning/using-advanced-secret-scanning-and-push-protection-features/enabling-delegated-alert-dismissal-for-secret-scanning).

## Usage and Documentation

Teams can set up delegated alert dismissal through both the web interface and the GitHub API. This enables organizations to:

- Standardize their review process for vulnerability alert management
- Maintain robust audit trails for compliance and internal policies

For more setup instructions and details, see the [Dependabot delegated alert dismissal documentation](https://docs.github.com/enterprise-cloud@latest/code-security).

## Conclusion

Delegated alert dismissal for Dependabot provides critical governance controls, helping organizations build secure and compliant development workflows in GitHub.

---
*Author: Allison*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-19-you-can-now-require-reviews-before-closing-dependabot-alerts-with-delegated-alert-dismissal)
