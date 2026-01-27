---
external_url: https://github.blog/changelog/2025-12-16-enterprise-governance-and-policy-improvements-for-secret-scanning-now-generally-available
title: Enterprise Governance and Policy Improvements for GitHub Secret Scanning
author: Allison
feed_name: The GitHub Blog
date: 2025-12-16 20:46:09 +00:00
tags:
- Alert Management
- Application Security
- Custom Patterns
- DevOps Security
- Enterprise Security
- GitHub
- GitHub Apps
- Improvement
- Permissions
- Policy Management
- Push Protection
- Secret Scanning
- Security Governance
- Security Manager Role
section_names:
- devops
- security
primary_section: devops
---
Allison summarizes significant improvements to GitHub's secret scanning for enterprises, including expanded alert permissions and more flexible policy management across teams and roles.<!--excerpt_end-->

# Enterprise Governance and Policy Improvements for GitHub Secret Scanning

**Author:** Allison

GitHub is rolling out several new features aimed at strengthening enterprise security and scaling governance for secret scanning. This update discusses recent improvements to permissions, alert management, and policy flexibility to help organizations better manage their code security.

## Key Improvements

### 1. Expanded Permissions for Secret Scanning Alerts

- **Wider Assignment Capabilities:** Anyone with alert `write` permissions can now assign, modify, or remove secret scanning alert assignments.
- **Alert Modification by Assignees:** Alert assignees now have the ability to resolve alerts or remove themselves as assignees, making alert handling more actionable and efficient.

### 2. Enhanced Custom Pattern Controls for Enterprises

- **Enterprise-Level Pattern Editing:** Enterprise owners and enterprise security managers can now edit any custom secret scanning patterns, regardless of who originally created them. This resolves prior issues with orphaned patterns that couldn't be updated.

### 3. Improved Push Protection Bypass Management

- **Delegated Push Protection:** Enterprises can now delegate push protection bypass permissions to specific Enterprise Teams, organization roles, and GitHub Apps. This helps scale policy management for large organizations.
- **More Flexible Bypass Lists:** The requirement to have at least one actor in the push protection bypass list is being removed. Now, custom roles with the required permissions can manage bypasses with finer granularity, without needing to assign a default role or team.

## Learn More

- [Delegated bypasses for push protection](https://docs.github.com/en/enterprise-cloud@latest/code-security/secret-scanning/using-advanced-secret-scanning-and-push-protection-features/delegated-bypass-for-push-protection/about-delegated-bypass-for-push-protection)
- [Custom pattern management](https://docs.github.com/en/enterprise-cloud@latest/code-security/secret-scanning/using-advanced-secret-scanning-and-push-protection-features/custom-patterns/defining-custom-patterns-for-secret-scanning)
- [Secret scanning documentation](https://docs.github.com/en/enterprise-cloud@latest/code-security/secret-scanning/introduction/about-secret-scanning)

These changes make it easier for enterprises to govern, scale, and secure sensitive code across organizations using GitHub, with fine-grained control over security policies.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-16-enterprise-governance-and-policy-improvements-for-secret-scanning-now-generally-available)
