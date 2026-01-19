---
layout: post
title: Enhanced Copilot Autofix Metrics for CodeQL Security Overview on GitHub
author: Allison
canonical_url: https://github.blog/changelog/2025-12-16-more-accurate-copilot-autofix-usage-metrics-on-security-overview
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-12-16 10:45:33 +00:00
permalink: /github-copilot/news/Enhanced-Copilot-Autofix-Metrics-for-CodeQL-Security-Overview-on-GitHub
tags:
- Application Security
- Code Scanning
- CodeQL
- Copilot Autofix
- GitHub Enterprise Cloud
- Improvement
- Pull Request Insights
- Remediation
- Security Dashboard
- Security Metrics
- Security Overview
- Vulnerability Management
section_names:
- ai
- github-copilot
- security
---
Allison shares an update about enhanced Copilot Autofix usage metrics on the GitHub security overview dashboard, improving clarity for developers and security teams.<!--excerpt_end-->

# Enhanced Copilot Autofix Metrics for CodeQL Security Overview on GitHub

GitHub has refined the security overview dashboard metrics to provide developers and security teams with more accurate insights into how Copilot Autofix suggestions remediate CodeQL alerts. This update affects both pull request scans and default branch analyses, improving the calculation of how much of a Copilot autofix was incorporated to resolve security issues.

## What Changed?

- **Metric Improvements**: The dashboard now more precisely measures the percentage and number of CodeQL alerts fixed using Copilot Autofix.
- **Affected Areas**: Applies retroactively to alerts detected in pull requests and scans of default branches, recalculating "Alerts fixed with autofix suggestions" and "Percentage of remediated alerts with autofix suggestion".
- **Visibility**: Developers gain better insight into how Copilot Autofix prevents vulnerabilities from being merged and helps reduce long-term security debt.

## Impact

- **Security Overview Dashboard**: The Remediation tab and CodeQL pull request insights will be updated over the next 10 days, with expected changes in related metrics.
- **Actionable Data**: Organizations can assess Copilot's security contributions more reliably at the repository level.

## Availability

These enhanced metrics are available now on **GitHub Enterprise Cloud**.

## Further Reading

- [Security Overview Documentation](https://docs.github.com/enterprise-cloud@latest/code-security/security-overview/about-security-overview)
- [Code Scanning Documentation](https://docs.github.com/enterprise-cloud@latest/code-security/code-scanning/introduction-to-code-scanning/about-code-scanning)

## Screenshot

![Screenshot of table showing CodeQL pull request alerts fixed with Copilot autofixes on the security overview dashboard](https://github.com/user-attachments/assets/8e9d36b1-98dd-4d90-9d09-b2026331dd58)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-16-more-accurate-copilot-autofix-usage-metrics-on-security-overview)
