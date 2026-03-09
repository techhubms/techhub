---
external_url: https://github.blog/changelog/2026-03-03-github-code-quality-enterprise-policy
title: GitHub Code Quality Enterprise Policy
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-03-03 19:07:26 +00:00
tags:
- Application Security
- Code Quality
- Code Security
- DevOps
- Enterprise Management Tools
- Enterprise Policy
- Enterprise Security
- GitHub
- GitHub Advanced Security
- News
- Platform Governance
- Policy Management
- Repository Management
- Security
- Security Policy
section_names:
- devops
- security
---
Allison outlines how enterprises can now independently manage GitHub Code Quality through dedicated policies, enhancing flexibility and governance for development teams.<!--excerpt_end-->

# GitHub Code Quality Enterprise Policy

GitHub has introduced new functionality in GitHub Advanced Security, allowing organizations to manage GitHub Code Quality availability separately from Code Security. This change provides greater flexibility for enterprise administrators to tailor code analysis and policy enforcement according to organizational needs. Previously, enabling Code Quality could unintentionally enable Code Security; now, these features are decoupled.

## Key Updates

- **Advanced Security enterprise policy settings** no longer reference the Code Quality feature, creating a clearer separation between security and code quality management.
- **Dedicated Code Quality policy page**: Administrators can now configure Code Quality availability at the repository level, similar to existing workflows for Advanced Security policies.
- **Improved governance**: These changes allow enterprises to grant access to Code Quality tools without altering their security policy posture.

## How to Use the New Policy Controls

Enterprises can now:

- Access a dedicated interface for configuring Code Quality at scale
- Assign code quality settings independently from code security settings
- Apply granular controls across repositories, aligning tools with team or project requirements

For additional technical details and setup instructions, see the [Code Quality documentation](https://docs.github.com/enterprise-cloud@latest/code-security/how-tos/secure-at-scale/configure-enterprise-security/configure-specific-tools/allow-github-code-quality-in-enterprise).

## Summary

These updates help organizations streamline repository management, maintain compliance, and maximize the benefits of GitHub's analysis tooling without compromising on security policy controls.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-03-03-github-code-quality-enterprise-policy)
