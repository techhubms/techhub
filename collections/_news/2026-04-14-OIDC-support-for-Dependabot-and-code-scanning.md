---
tags:
- Application Security
- AWS CodeArtifact
- Azure DevOps Artifacts
- Cloud Identity Provider
- Code Scanning
- Dependabot
- DevOps
- GitHub Code Security
- GitHub Enterprise Server 3.22
- Improvement
- JFrog Artifactory
- News
- OIDC
- OIDC Federation
- OpenID Connect
- Organization Security
- Private Registries
- Secrets Management
- Security
- Short Lived Credentials
- Supply Chain Security
external_url: https://github.blog/changelog/2026-04-14-oidc-support-for-dependabot-and-code-scanning
date: 2026-04-14 20:29:03 +00:00
feed_name: The GitHub Blog
author: Allison
primary_section: devops
section_names:
- devops
- security
title: OIDC support for Dependabot and code scanning
---

Allison announces that Dependabot and code scanning can now use OpenID Connect (OIDC) for organization-level access to private registries, reducing reliance on long-lived secrets and enabling short-lived, dynamically issued credentials.<!--excerpt_end-->

# OIDC support for Dependabot and code scanning

Dependabot and code scanning now support **OpenID Connect (OIDC)** authentication for **private registries configured at the organization level**, eliminating the need to store **long-lived credentials** as repository secrets.

## What’s new

- **Organization-level OIDC credentials**: Org administrators can configure OIDC-based credentials for private registries across their organization.
- **Short-lived credentials**: With OIDC-based authentication, you can dynamically obtain short-lived credentials from your cloud identity provider.
- **Same model as GitHub Actions OIDC federation**: This works similarly to GitHub Actions workflows using OIDC federation.
- **Builds on earlier support**: Extends prior support for OIDC in repository-level `dependabot.yml` configuration to the organization level, enabling centralized registry access management for all repositories in the org.

## Supported registries

Currently supported:

- **AWS CodeArtifact**
- **Azure DevOps Artifacts**
- **JFrog Artifactory**

Planned within the next four weeks:

- Cloudsmith
- Google Artifact Registry

## Availability

- Generally available on **github.com**
- Will ship in **GitHub Enterprise Server 3.22**

## Learn more

- Configuring OIDC for Dependabot: https://docs.github.com/code-security/how-tos/secure-your-supply-chain/manage-your-dependency-security/configuring-access-to-private-registries-for-dependabot
- Configuring organization-level access for code scanning: https://docs.github.com/en/code-security/how-tos/secure-at-scale/configure-organization-security/manage-usage-and-access/giving-org-access-private-registries
- Background on GitHub Actions OIDC federation: https://docs.github.com/actions/security-for-github-actions/security-hardening-your-deployments/about-security-hardening-with-openid-connect
- Community discussion: https://github.com/orgs/community/discussions

[Read the entire article](https://github.blog/changelog/2026-04-14-oidc-support-for-dependabot-and-code-scanning)

