---
layout: post
title: Enterprise-Wide Custom Organization Roles and Increased Role Limits in GitHub
author: Allison
canonical_url: https://github.blog/changelog/2025-08-21-enterprises-can-create-organization-roles-for-use-across-their-enterprise-and-custom-role-limits-have-been-increased
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-08-21 22:36:21 +00:00
permalink: /devops/news/Enterprise-Wide-Custom-Organization-Roles-and-Increased-Role-Limits-in-GitHub
tags:
- Access Control
- Compliance
- Custom Roles
- DevOps
- Enterprise Roles
- GHES 3.19
- GitHub Enterprise
- GitHub Roles API
- News
- Organization Administration
- Organization Settings
- Permissions
- Public Preview
- Role Management
section_names:
- devops
---
Allison details GitHub's introduction of enterprise-wide custom organization roles, standardized management, and higher limits, streamlining role-based access for enterprise users.<!--excerpt_end-->

# Enterprise-Wide Custom Organization Roles and Increased Role Limits in GitHub

GitHub enterprise owners can now create sets of custom organization roles that are accessible across all organizations in their enterprise. These standardized roles aim to support compliance requirements and streamline user movement and permissions management among different organizations within an enterprise.

## Key Features

- **Enterprise-Scoped Roles**: Enterprise owners define custom roles once, usable enterprise-wide.
- **Assignment & Editing**: Only enterprise owners can edit these roles. Organization owners can assign roles via organization settings or using the [organization role assignment API](https://docs.github.com/enterprise-cloud@latest/rest/orgs/organization-roles#assign-an-organization-role-to-a-team).
- **Role Consistency**: The same permissions framework (organization/repository permissions) is used across enterprise and organization level roles.
- **Future Integration**: Enterprise, organization, and repository rulesets will soon support these roles for enhanced rule-bypassing and permissions control.
- **Role Limits Increased**: Each account (enterprise or organization) can now create up to 20 custom organization roles, allowing an organization owner to access up to 40 custom roles in addition to default ones.

## Availability and Compatibility

- This feature is now available as a public preview.
- It will be included with GitHub Enterprise Server (GHES) version 3.19 and forward.
- For details on managing organization roles, see GitHub’s [enterprise documentation](https://docs.github.com/enterprise-cloud@latest/admin/managing-accounts-and-repositories/managing-users-in-your-enterprise/roles-in-an-enterprise).

## Community and Feedback

Users can provide feedback or discuss questions in the [GitHub Community Enterprise forum](https://github.com/orgs/community/discussions/categories/enterprise).

*Disclaimer: The UI for features in public preview may change.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-21-enterprises-can-create-organization-roles-for-use-across-their-enterprise-and-custom-role-limits-have-been-increased)
