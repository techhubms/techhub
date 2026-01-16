---
layout: post
title: 'GitHub Secret Scanning: Automatic Extended Metadata Checks for Security'
author: Allison
canonical_url: https://github.blog/changelog/2026-01-15-secret-scanning-extended-metadata-to-be-automatically-enabled-for-certain-repositories
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2026-01-15 21:59:35 +00:00
permalink: /devops/news/GitHub-Secret-Scanning-Automatic-Extended-Metadata-Checks-for-Security
tags:
- Application Security
- Developer Security
- DevOps
- Enterprise Cloud
- Exposure Assessment
- GitHub
- Improvement
- Incident Response
- Metadata Checks
- News
- Remediation
- Repository Security
- Secret Scanning
- Security
- Security Alerts
- Security Configuration
- Token Security
- Validity Checks
section_names:
- devops
- security
---
Allison reports on GitHub's upcoming automatic enablement of extended metadata checks in secret scanning, bringing improved context to security alerts for more effective remediation.<!--excerpt_end-->

# GitHub Secret Scanning: Automatic Extended Metadata Checks for Security

GitHub is expanding its secret scanning capabilities by supporting extended metadata checks as part of security configurations, with automatic enablement starting February 18, 2026.

## What Are Extended Metadata Checks?

Extended metadata checks provide additional context within secret scanning alerts, including:

- Secret owner details (name, email, identifier)
- Creation and expiry dates
- Project or organization context (when supplied by the secret provider)

These enhancements build upon GitHub's existing validity checks, giving development and security teams more actionable information to triage and prioritize remediation of exposed secrets.

> **Example:** If an OpenAI API key is leaked and the provider supports metadata, alert details will include the secret owner's identity and related organization data.

**Note:** The display of metadata depends on the capabilities of each secret provider and may not always be available for every secret type or instance.

## What's Changing in February 2026?

- Extended metadata checks will be nested features under existing validity checks.
- For repositories with security configurations and validity checks enabled, extended metadata checks will be automatically activated.
- Enterprise Cloud customers with secret scanning will have organization and enterprise-level controls to enable or disable these checks.

## Additional Information

- [Documentation: About secret scanning](https://docs.github.com/code-security/concepts/secret-security/about-secret-scanning)
- [Changelog: Introducing extended metadata checks](https://github.blog/changelog/2025-10-28-introducing-extended-metadata-checks-for-secret-scanning/)
- [Community feedback](https://github.com/orgs/community/discussions/177054)

The expansion helps teams gain faster, more detailed insights when secrets are exposed in repositories, allowing for quicker and more informed security responses.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-15-secret-scanning-extended-metadata-to-be-automatically-enabled-for-certain-repositories)
