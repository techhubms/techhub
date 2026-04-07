---
section_names:
- ai
- devops
- github-copilot
- security
date: 2026-03-31 12:07:49 +00:00
feed_name: The GitHub Blog
title: CodeQL pull requests insights on security overview now cover all protected branches
external_url: https://github.blog/changelog/2026-03-31-codeql-pull-requests-insights-on-security-overview-now-cover-all-protected-branches
primary_section: github-copilot
author: Allison
tags:
- AI
- Alert Statistics
- Application Security
- Autofix Suggestions
- Branch Protection Rules
- Code Scanning
- CodeQL
- Copilot Autofix
- CSV Export
- Default Branch
- DevOps
- GitHub Advanced Security
- GitHub Copilot
- GitHub Security Overview
- Improvement
- News
- Protected Branches
- Pull Request Insights
- Secure Development
- Security
---

Allison explains an update to GitHub Security Overview: the CodeQL pull request insights tab now aggregates Copilot Autofix and alert stats across all protected branches, giving teams more representative security and remediation metrics than default-branch-only reporting.<!--excerpt_end-->

# CodeQL pull requests insights on security overview now cover all protected branches

The CodeQL pull request insights tab in GitHub Security Overview now reports Copilot Autofix and alert statistics from **all protected branches**, not just the default branch. This provides a more complete view of the impact Autofix has across the codebase.

## What’s new

- The **nine insight tiles** on the CodeQL pull request insights tab now aggregate data from **all protected branches** (previously: default branch only).
- The **CSV export** from the same tab also uses the all-protected-branches aggregation.
- Metrics for **alerts fixed with Autofix suggestions** may appear **higher and more representative**, reflecting fixes across protected branches.
- Data is expected to change **retrospectively**, so historical numbers may increase.

## Why this matters

Previously, Security Overview only showed CodeQL alert and Copilot Autofix statistics for the **default branch**, which could understate Autofix’s value.

With aggregation across **all protected branches**, teams can get a more comprehensive view of how Copilot Autofix helps developers resolve **GitHub code scanning alerts** across the whole codebase.

## References

- GitHub documentation: GitHub Copilot Autofix for CodeQL code scanning — https://docs.github.com/code-security/code-scanning/managing-code-scanning-alerts/about-autofix-for-codeql-code-scanning
- GitHub documentation: Security overview — https://docs.github.com/code-security/security-overview/about-security-overview


[Read the entire article](https://github.blog/changelog/2026-03-31-codeql-pull-requests-insights-on-security-overview-now-cover-all-protected-branches)

