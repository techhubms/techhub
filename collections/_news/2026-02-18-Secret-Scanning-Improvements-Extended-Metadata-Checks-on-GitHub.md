---
external_url: https://github.blog/changelog/2026-02-18-secret-scanning-improvements-to-extended-metadata-checks
title: 'Secret Scanning Improvements: Extended Metadata Checks on GitHub'
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-02-18 21:31:29 +00:00
tags:
- Access Control
- Application Security
- Audit Log
- Code Security
- Credential Leak Detection
- DevOps
- DevSecOps
- Enterprise Cloud
- Extended Metadata
- GitHub Secret Scanning
- Improvement
- News
- Remediation
- Secret Management
- Security
- Security Configuration
- Software Supply Chain
- Token Validity Checks
section_names:
- devops
- security
---
Allison details improvements to GitHub secret scanning that add extended metadata checks, providing richer context on exposed secrets and supporting more effective remediation for development and security teams.<!--excerpt_end-->

# Secret Scanning Improvements: Extended Metadata Checks on GitHub

GitHub has announced support for extended metadata checks within their secret scanning feature, streamlining the process for organizations to enable and leverage this capability at scale.

## Overview

Extended metadata checks enrich secret scanning alerts by providing extra contextual information such as secret ownership, creation and expiry dates, and organizational context, when supported by the secret provider. This update is particularly useful for development and security teams seeking to quickly assess and remediate exposed secrets within repositories.

## Key Features

- **Automatic Enablement**: For repositories with validity checks enabled, extended metadata checks are automatically activated. Enterprises and organizations can track this feature's enablement via audit logs.
- **Detailed Alerts**: Alerts now include metadata such as:
  - Secret owner name, email, and identifier
  - Relevant organization or project context
  - Creation and expiry dates, depending on data supplied by the secret provider
- **Flexible Configuration**: Extended metadata checks can be managed at both the organization and enterprise level for Enterprise Cloud customers using GitHub security configurations.

## Benefits

- **Faster Triage**: The additional information makes it easier to prioritize and act on potential credential leaks.
- **Scalable Enablement**: Organizations can roll out these checks widely through security configuration settings, improving consistency and coverage.
- **Improved Visibility**: Provides actionable context to development and security teams to speed up remediation.

## Limitations

The availability of extended metadata depends on the token type and provider. Not all metadata fields will be present in every alert; GitHub displays as much detail as is available per secret.

## Further Resources

- [Securing your repositories with secret scanning](https://docs.github.com/enterprise-cloud@latest/code-security/concepts/secret-security/about-secret-scanning)
- [GitHub Changelog Announcement](https://github.blog/changelog/2026-02-18-secret-scanning-improvements-to-extended-metadata-checks)
- [Feedback and Discussion on Secret Scanning](https://github.com/orgs/community/discussions/177054)

---

Authored by Allison, this update underscores GitHub's continuing commitment to developer and organizational security in the software supply chain.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-18-secret-scanning-improvements-to-extended-metadata-checks)
