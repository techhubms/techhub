---
external_url: https://github.blog/changelog/2026-02-03-dependabot-now-supports-oidc-authentication
title: Dependabot Adds OIDC Authentication for Private Registries
author: Allison
primary_section: azure
feed_name: The GitHub Blog
date: 2026-02-03 17:09:59 +00:00
tags:
- Azure
- Azure DevOps Artifacts
- Continuous Integration
- Credential Management
- Dependabot
- DevOps
- GitHub Actions
- Improvement
- News
- OIDC Authentication
- OpenID Connect
- Private Registries
- Security
- Supply Chain Security
section_names:
- azure
- devops
- security
---
Allison outlines how Dependabot now supports OIDC authentication for private registries, including Azure DevOps Artifacts. This enhancement improves repository security and simplifies credential management for developers and DevOps teams.<!--excerpt_end-->

# Dependabot Now Supports OIDC Authentication

Dependabot can now use OpenID Connect (OIDC) to authenticate with private registries, removing the need for static, long-lived secrets in repositories. This mirrors how GitHub Actions already leverages OIDC federation for secure cloud integrations.

## What’s New

- **OIDC-based authentication**: Dependabot update jobs can dynamically obtain short-lived credentials from your cloud identity provider.
- **Supported registries**:
  - AWS CodeArtifact
  - Azure DevOps Artifacts
  - JFrog Artifactory

## Key Benefits

- **Enhanced security**: Removes static credentials from repositories, reducing risk and operational overhead.
- **Simplified management**: Enables policy-compliant, dynamic access control for private registries.
- **Prevents rate limiting**: Short-lived tokens avoid issues tied to reused static credentials.

## How to Enable

Update your `dependabot.yml` to configure OIDC authentication for supported registries. Detailed instructions and sample configurations are available in the [GitHub documentation](https://docs.github.com/code-security/dependabot/working-with-dependabot/configuring-access-to-private-registries-for-dependabot).

## Learn More

- [Configuring access to private registries for Dependabot](https://docs.github.com/code-security/dependabot/working-with-dependabot/configuring-access-to-private-registries-for-dependabot)
- [About OpenID Connect security hardening](https://docs.github.com/actions/security-for-github-actions/security-hardening-your-deployments/about-security-hardening-with-openid-connect)

Be sure to update your configurations to take advantage of OIDC federation and improve the security of your software supply chain.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-03-dependabot-now-supports-oidc-authentication)
