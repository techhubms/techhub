---
layout: "post"
title: "Enterprise-Wide Credential Management Tools for GitHub Incident Response"
description: "This update introduces new credential management actions for GitHub Enterprise Cloud enterprise owners. These actions enhance security incident response by enabling owners to review SSO credentials, block access, revoke authorizations, and delete compromised tokens across the enterprise. A new fine-grained permission streamlines delegation of these sensitive operations."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-17-enterprise-wide-credential-management-tools-for-incident-response"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-17 18:55:38 +00:00
permalink: "/2026-02-17-Enterprise-Wide-Credential-Management-Tools-for-GitHub-Incident-Response.html"
categories: ["DevOps", "Security"]
tags: ["API Security", "Application Security", "Audit Logs", "Credential Management", "DevOps", "Enterprise Management", "Enterprise Management Tools", "Enterprise Security", "Fine Grained Permissions", "GitHub Enterprise Cloud", "Improvement", "Incident Response", "News", "OAuth Tokens", "Personal Access Tokens", "Security", "Security Incident", "Single Sign On", "SSH Keys", "SSO", "Token Revocation"]
tags_normalized: ["api security", "application security", "audit logs", "credential management", "devops", "enterprise management", "enterprise management tools", "enterprise security", "fine grained permissions", "github enterprise cloud", "improvement", "incident response", "news", "oauth tokens", "personal access tokens", "security", "security incident", "single sign on", "ssh keys", "sso", "token revocation"]
---

Allison reports on new enterprise-wide credential management features in GitHub Enterprise Cloud, allowing owners to respond swiftly to major security incidents by revoking or blocking credentials and delegating these tasks to trusted admins.<!--excerpt_end-->

# Enterprise-Wide Credential Management Tools for GitHub Incident Response

Enterprise owners now have enhanced tools for managing credentials and responding effectively to major security incidents within their GitHub Enterprise Cloud environments.

## Key Capabilities

- **Review SSO Credential Counts**: See counts of credentials authorized via single sign-on across organizations.
- **Temporarily Block SSO Access**: Restrict SSO to only enterprise owners during incident investigation to limit exposure.
- **Revoke SSO Authorizations**: Remove user credential authorizations (personal access tokens, SSH keys, OAuth tokens) across the enterprise.
- **Delete Tokens and Keys**: For EMU accounts, delete tokens and SSH keys not tied to SSO to cover non-standard authorizations.

> **Note:** These actions can disrupt developer automations and workflows and should be reserved for significant security incidents.

## Best Practices

- **Audit Logs**: Enterprise owners can use audit logs to review the scope and details of revoked or deleted credentials.
- **Token Lifetimes**: For regular token management, configure [maximum token lifetimes](https://docs.github.com/enterprise-cloud@latest/admin/enforcing-policies/enforcing-policies-for-your-enterprise/enforcing-policies-for-personal-access-tokens-in-your-enterprise) for improved compliance and automation safety.

## Delegation and Permissions

A new fine-grained permission, `Manage enterprise credentials`, allows owners to delegate credential management responsibilities to trusted administrators, enabling scalable and secure incident response.

## Documentation and Community

- [Incident Response Documentation](https://docs.github.com/enterprise-cloud@latest/admin/managing-iam/respond-to-incidents)
- [GitHub Community Announcement](https://github.com/orgs/community/discussions/categories/announcements)

---

These features help organizations reinforce enterprise security and streamline their response to compromised credentials, supporting ongoing DevOps practices and secure CI/CD pipelines.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-17-enterprise-wide-credential-management-tools-for-incident-response)
