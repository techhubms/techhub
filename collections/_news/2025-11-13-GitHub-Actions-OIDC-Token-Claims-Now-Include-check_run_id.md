---
external_url: https://github.blog/changelog/2025-11-13-github-actions-oidc-token-claims-now-include-check_run_id
title: GitHub Actions OIDC Token Claims Now Include check_run_id
author: Allison
feed_name: The GitHub Blog
date: 2025-11-13 15:16:52 +00:00
tags:
- Actions
- Attribute Based Access Control
- Auditing
- Azure Integration
- Check Run Id
- CI/CD
- Compliance
- GitHub Actions
- Improvement
- Least Privilege
- OIDC
- OpenID Connect
- Token Claims
- Workflow Security
- DevOps
- News
section_names:
- devops
primary_section: devops
---
Allison highlights how GitHub Actions now includes the check_run_id in OIDC token claims, improving traceability and access control for secure workflow automation.<!--excerpt_end-->

# GitHub Actions OIDC Token Claims Now Include check_run_id

GitHub Actions has enhanced its security and compliance capabilities by adding the `check_run_id` to the claims in OpenID Connect (OIDC) tokens. This update enables:

- **Granular Attribute-Based Access Control:** Platform teams can now enforce least-privilege policies, as workflows can be linked to specific job runs via the `check_run_id`. This reduces the need to enumerate every repository for access control.
- **Improved Auditability:** The ability to correlate an OIDC token with the exact workflow job and compute resource that generated it allows for better auditing and compliance tracking. This is particularly important for organizations with strict regulatory requirements or those integrating with cloud platforms like Azure.
- **Enhanced Security:** By mapping access requests to governed repository states, organizations can reduce secret exposure risk and more rapidly revoke access if needed.

## Practical Benefits

- Trace tokens directly to the workflow job that requested them, improving transparency in CI/CD automation.
- Implement precise repository access controls using the new token claim.
- Accelerate response to potential security incidents by mapping access to specific runs.

For configuration tips and further details, refer to the [GitHub Actions OIDC documentation](https://docs.github.com/actions/concepts/security/openid-connect).

---

Allison explains how these improvements streamline secure workflow automation for development and platform teams.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-13-github-actions-oidc-token-claims-now-include-check_run_id)
