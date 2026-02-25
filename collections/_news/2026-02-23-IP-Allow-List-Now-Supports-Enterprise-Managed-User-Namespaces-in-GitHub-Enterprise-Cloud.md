---
external_url: https://github.blog/changelog/2026-02-23-ip-allow-list-coverage-extended-to-emu-namespaces-in-public-preview
title: IP Allow List Now Supports Enterprise Managed User Namespaces in GitHub Enterprise Cloud
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-02-23 17:44:32 +00:00
tags:
- Access Control
- API Security
- DevOps
- Enterprise Administration
- Enterprise Managed Users
- Enterprise Management Tools
- Git Protocol
- GitHub Enterprise Cloud
- Improvement
- IP Allow List
- Network Access
- News
- Security
- Security Policies
- User Namespace Management
section_names:
- devops
- security
---
Allison highlights how GitHub Enterprise Cloud's IP allow list now extends to Enterprise Managed User namespaces, providing unified network access control for organizations.<!--excerpt_end-->

# IP Allow List Coverage Extended to EMU Namespaces in GitHub Enterprise Cloud

GitHub Enterprise Cloud with Enterprise Managed Users (EMUs) now supports applying GitHub’s native IP allow list settings to user namespaces. EMUs give enterprises ownership of user accounts within their domain, improving administrative control and compliance.

## Key Features

- **Scope Extension**: The IP allow list now covers not only organization and enterprise resources but also user namespaces tied to EMUs.
- **Unified Network Control**: All repositories associated with EMUs must adhere to enterprise-defined network policies.
- **Access Filtering**: Access to resources via the web UI, git operations, and APIs is restricted based on the enterprise’s IP allow list.
- **Credential Enforcement**: All forms of credentials (personal access tokens, app tokens, SSH keys) are included in the policy’s scope.

## Benefits

- Strengthens network security and compliance for large organizations.
- Ensures only approved network locations can access critical repositories, regardless of whether they are owned by the organization or associated user accounts.

## How To Learn More

Read more in the official [GitHub documentation on configuring IP allow lists](https://docs.github.com/enterprise-cloud@latest/admin/configuring-settings/hardening-security-for-your-enterprise/restricting-network-traffic-to-your-enterprise-with-an-ip-allow-list).

For additional background or updates, refer to the original [GitHub blog changelog announcement](https://github.blog/changelog/2026-02-23-ip-allow-list-coverage-extended-to-emu-namespaces-in-public-preview).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-23-ip-allow-list-coverage-extended-to-emu-namespaces-in-public-preview)
