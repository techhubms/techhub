---
external_url: https://github.blog/changelog/2025-08-26-the-secret-risk-assessment-is-generally-available
title: GitHub Introduces Free Secret Risk Assessment Tool for Organizations
author: Allison
feed_name: The GitHub Blog
date: 2025-08-26 21:21:48 +00:00
tags:
- Continuous Monitoring
- GitHub
- GitHub Enterprise
- Incident Management
- Organization Security
- Risk Assessment
- Secret Scanning
- Security Tools
- Token Exposure
section_names:
- devops
- security
primary_section: devops
---
Allison presents GitHub's new secret risk assessment tool, enabling organizational admins to detect and analyze secret leaks across their repositories. This feature enhances security practices and supports actionable improvements.<!--excerpt_end-->

# GitHub Introduces Free Secret Risk Assessment Tool for Organizations

GitHub has launched a new, free secret risk assessment feature aimed at helping organizations strengthen their security by recognizing and mitigating the risks of secret leaks within their repositories.

## Key Features

- **Aggregate Insights**: Scan your organization for information on public leaks, private exposures, and the types of tokens affected.
- **Dashboards**: The risk assessment dashboard, accessible via the Security tab, provides:
  - The number of secrets leaked by type
  - Counts of publicly visible secrets in public repositories
  - Repositories affected by secret type
- **Static Reports**: Scans are one-time, static snapshots that can be re-run every 90 days. Results can be downloaded as a CSV file for further analysis.
- **Privacy**: No specific secrets are stored or shared after the scan—only aggregate insights are returned.
- **Scope**: The scan covers all public, private, internal, and archived repositories in your organization.

## Adoption

- The feature is free for organizations using GitHub Team or Enterprise plans.
- Organization and security administrators can run the report and review results.
- Secret risk assessment is coming to Enterprise Server starting with GHES 3.18.

## Recommended Practices

- For continuous monitoring, GitHub recommends enabling secret scanning for proactive detection and incident management.
- For more on protecting secrets, see [GitHub Secret Protection](https://github.com/security/advanced-security/secret-protection).

## Summary

This new feature underscores GitHub's commitment to supporting the developer community by offering security insights and practical tools to prevent secret leaks. By empowering organizations with clear exposure reports and actionable recommendations, the secret risk assessment tool helps reinforce robust DevOps and security practices.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-26-the-secret-risk-assessment-is-generally-available)
