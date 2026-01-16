---
layout: post
title: GitHub Introduces Fine-Grained artifact_metadata Permission for Enhanced API Access Control
author: Allison
canonical_url: https://github.blog/changelog/2026-01-13-new-fine-grained-permission-for-artifact-metadata-is-now-generally-available
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2026-01-13 13:30:02 +00:00
permalink: /devops/news/GitHub-Introduces-Fine-Grained-artifact_metadata-Permission-for-Enhanced-API-Access-Control
tags:
- Access Control
- Actions
- API Permissions
- Application Security
- Artifact Metadata
- CI/CD
- DevOps
- Fine Grained Permissions
- GitHub
- GitHub Actions
- News
- Repository Permissions
- Security
- Supply Chain Security
- Workflow Automation
section_names:
- devops
- security
---
Allison presents GitHub’s new artifact_metadata permission, which enhances security and refines API access control for artifact metadata. Developers and DevOps teams must transition workflows by February 2026.<!--excerpt_end-->

# GitHub Introduces Fine-Grained artifact_metadata Permission for Enhanced API Access Control

GitHub has launched a new permission named `artifact_metadata` to give users more precise management over API access to artifact-related metadata. This upgrade replaces the previously broader `contents:read` and `contents:write` permissions used for interacting with artifact metadata APIs, offering a more secure and targeted approach to access control.

## Impact and Migration Timeline

- **Transition Requirement:** All workflows that currently use the `contents` permission solely for artifact metadata APIs must be updated to use `artifact_metadata`.
- **Deadline:** The old `contents` permission for this purpose will be deprecated on **February 3, 2026**.

## Security Improvements

More granular permissions mean:

- Reduced risk of over-permissioned API tokens
- Improved least-privilege access
- Enhanced supply chain security for organizations and open-source projects
- Greater transparency and control over artifact API access

## Key References

- [Artifact metadata APIs documentation](https://docs.github.com/rest/orgs/artifact-metadata)
- [Repository permissions for artifact metadata](https://docs.github.com/rest/authentication/permissions-required-for-fine-grained-personal-access-tokens#repository-permissions-for-artifact-metadata)
- [Actions permissions reference](https://docs.github.com/actions/reference/workflows-and-actions/workflow-syntax#jobsjob_idpermissions)
- [How artifact metadata improves security alerts](https://docs.github.com/code-security/securing-your-organization/understanding-your-organizations-exposure-to-vulnerabilities/alerts-in-production-code)

## Action Items for Practitioners

- **Audit existing workflows** for use of `contents` permission with artifact metadata APIs.
- **Update GitHub Actions and PAT scopes** to use the new `artifact_metadata` permission.
- Ensure documentation and security policies are revised for this migration.

The shift to more granular permissions supports a secure software supply chain and aligns with modern DevOps best practices.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-13-new-fine-grained-permission-for-artifact-metadata-is-now-generally-available)
