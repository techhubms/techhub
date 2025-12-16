---
layout: "post"
title: "Enterprise Governance and Policy Improvements for GitHub Secret Scanning"
description: "This news update details recent enhancements to GitHub's secret scanning features that improve enterprise-level governance and security. The article covers expanded permissions for alert management, updates for custom pattern controls, and new options for managing push protection bypasses across enterprise teams, roles, and GitHub Apps."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-12-16-enterprise-governance-and-policy-improvements-for-secret-scanning-now-generally-available"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-12-16 20:46:09 +00:00
permalink: "/2025-12-16-Enterprise-Governance-and-Policy-Improvements-for-GitHub-Secret-Scanning.html"
categories: ["DevOps", "Security"]
tags: ["Alert Management", "Application Security", "Custom Patterns", "DevOps", "DevOps Security", "Enterprise Security", "GitHub", "GitHub Apps", "Improvement", "News", "Permissions", "Policy Management", "Push Protection", "Secret Scanning", "Security", "Security Governance", "Security Manager Role"]
tags_normalized: ["alert management", "application security", "custom patterns", "devops", "devops security", "enterprise security", "github", "github apps", "improvement", "news", "permissions", "policy management", "push protection", "secret scanning", "security", "security governance", "security manager role"]
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
