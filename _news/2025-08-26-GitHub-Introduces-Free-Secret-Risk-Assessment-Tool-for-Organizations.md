---
layout: "post"
title: "GitHub Introduces Free Secret Risk Assessment Tool for Organizations"
description: "GitHub has launched a free secret risk assessment feature for organizational security. This tool allows security admins to scan for secret leaks across all repositories, provides aggregate insights into exposures, and offers actionable steps to strengthen code security. Designed for GitHub Team and Enterprise plan users, the tool helps organizations address secret leaks and improve security posture."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-08-26-the-secret-risk-assessment-is-generally-available"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-08-26 21:21:48 +00:00
permalink: "/2025-08-26-GitHub-Introduces-Free-Secret-Risk-Assessment-Tool-for-Organizations.html"
categories: ["DevOps", "Security"]
tags: ["Continuous Monitoring", "DevOps", "GitHub", "GitHub Enterprise", "Incident Management", "News", "Organization Security", "Risk Assessment", "Secret Scanning", "Security", "Security Tools", "Token Exposure"]
tags_normalized: ["continuous monitoring", "devops", "github", "github enterprise", "incident management", "news", "organization security", "risk assessment", "secret scanning", "security", "security tools", "token exposure"]
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
