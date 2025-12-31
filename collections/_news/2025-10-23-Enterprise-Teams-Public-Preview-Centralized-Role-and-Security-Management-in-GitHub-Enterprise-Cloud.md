---
layout: "post"
title: "Enterprise Teams Public Preview: Centralized Role and Security Management in GitHub Enterprise Cloud"
description: "This update details the public preview of GitHub Enterprise teams for managing roles, permissions, and security at scale. New features empower enterprise owners with improved governance, streamlined user and team management, and introduce the Enterprise Security Manager role for centralized security administration using GitHub’s APIs and UI."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-10-23-managing-roles-and-governance-via-enterprise-teams-is-in-public-preview"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-10-23 17:00:00 +00:00
permalink: "/news/2025-10-23-Enterprise-Teams-Public-Preview-Centralized-Role-and-Security-Management-in-GitHub-Enterprise-Cloud.html"
categories: ["DevOps", "Security"]
tags: ["Advanced Security", "Application Security", "Code Security", "DevOps", "Enterprise Management Tools", "Enterprise Teams", "GitHub API", "GitHub Enterprise Cloud", "Governance", "Granular Access Control", "News", "Organization Management", "Permissions", "Platform Governance", "Policy Management", "Push Protection", "Repository Rulesets", "Role Management", "Secret Scanning", "Security", "Security Manager"]
tags_normalized: ["advanced security", "application security", "code security", "devops", "enterprise management tools", "enterprise teams", "github api", "github enterprise cloud", "governance", "granular access control", "news", "organization management", "permissions", "platform governance", "policy management", "push protection", "repository rulesets", "role management", "secret scanning", "security", "security manager"]
---

Allison explains new enterprise team features in GitHub Enterprise Cloud, highlighting improved role management, centralized enterprise security, and advanced governance for organizations and repositories.<!--excerpt_end-->

# Managing Roles and Governance via Enterprise Teams in GitHub Enterprise Cloud

*Author: Allison*

GitHub Enterprise Cloud continues to expand its enterprise management capabilities with the public preview of enterprise teams for managing Copilot Business licenses and broader platform governance. These updates provide enterprise owners with powerful tools for scaling policies, managing permissions, and improving security across multiple organizations.

## Key Enhancements in Public Preview

- **Assign Enterprise Teams to Organizations:** Easily designate groups of users to multiple organizations without repeated configuration.
- **Custom and Predefined Enterprise Roles:** Assign custom or out-of-the-box roles (e.g., Enterprise Security Manager) to teams and individuals across the enterprise.
- **Granular Permissions and Roles:** Organization and repository owners can assign roles within their scope, although only enterprise owners can grant or revoke certain permissions.
- **Ruleset Bypass Lists:** Assign enterprise teams and roles to bypass lists, giving fine-grained control over compliance and exceptions for repository rulesets.

## Enterprise Security Manager Role

Security teams now benefit from the new predefined Enterprise Security Manager (ESM) role, available for GitHub Code Security, Secret Protection, and Advanced Security customers. The ESM role enables:

- Centralized management of alerts (code scanning, secret scanning, Dependabot)
- Enterprise-wide configuration of security settings and custom scanning patterns
- Approval and review of delegated alert dismissals and push protection bypass requests
- Enterprise-level use of security-related APIs
- Streamlined compliance and governance administration

## Improved Policy Management

- **Granular Repository Ruleset Permissions:** Delegate ruleset bypass rights to teams, roles, and apps at various levels (enterprise, organization, repository).
- **Delegated Push Ruleset Bypasses:** Empower teams to manage push ruleset bypass requests efficiently at scale.

## Limitations and Resources

Some limitations exist in public preview. For specifics, refer to [enterprise teams product limits](https://docs.github.com/enterprise-cloud@latest/admin/concepts/enterprise-fundamentals/teams-in-an-enterprise#what-kind-of-team-should-i-use).

Explore further documentation:

- [Enterprise Teams](https://docs.github.com/enterprise-cloud@latest/admin/concepts/enterprise-fundamentals/teams-in-an-enterprise)
- [Enterprise Roles](https://docs.github.com/enterprise-cloud@latest/admin/concepts/enterprise-fundamentals/roles-in-an-enterprise)
- [Security Managers](https://docs.github.com/enterprise-cloud@latest/admin/managing-accounts-and-repositories/managing-roles-in-your-enterprise/abilities-of-roles#security-managers)
- [Rulesets](https://docs.github.com/enterprise-cloud@latest/admin/enforcing-policies/enforcing-policies-for-your-enterprise/enforcing-policies-for-code-governance)

## Community Feedback

Share your feedback or questions on [this GitHub Community discussion](https://github.com/orgs/community/discussions/177040).

*Disclaimer: Public preview UI may change in future releases.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-23-managing-roles-and-governance-via-enterprise-teams-is-in-public-preview)
