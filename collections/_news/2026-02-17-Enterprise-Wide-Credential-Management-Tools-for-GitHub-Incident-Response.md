---
external_url: https://github.blog/changelog/2026-02-17-enterprise-wide-credential-management-tools-for-incident-response
title: Enterprise-Wide Credential Management Tools for GitHub Incident Response
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-02-17 18:55:38 +00:00
tags:
- API Security
- Application Security
- Audit Logs
- Credential Management
- DevOps
- Enterprise Management
- Enterprise Management Tools
- Enterprise Security
- Fine Grained Permissions
- GitHub Enterprise Cloud
- Improvement
- Incident Response
- News
- OAuth Tokens
- Personal Access Tokens
- Security
- Security Incident
- Single Sign On
- SSH Keys
- SSO
- Token Revocation
section_names:
- devops
- security
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
