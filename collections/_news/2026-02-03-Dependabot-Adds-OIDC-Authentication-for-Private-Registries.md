---
layout: "post"
title: "Dependabot Adds OIDC Authentication for Private Registries"
description: "This update covers the introduction of OpenID Connect (OIDC) authentication support in Dependabot, allowing update jobs to securely access private registries (including Azure DevOps Artifacts) without storing long-lived credentials. It discusses setup steps, supported registries, and benefits to supply chain security and operational overhead."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-03-dependabot-now-supports-oidc-authentication"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-03 17:09:59 +00:00
permalink: "/2026-02-03-Dependabot-Adds-OIDC-Authentication-for-Private-Registries.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Azure", "Azure DevOps Artifacts", "Continuous Integration", "Credential Management", "Dependabot", "DevOps", "GitHub Actions", "Improvement", "News", "OIDC Authentication", "OpenID Connect", "Private Registries", "Security", "Supply Chain Security"]
tags_normalized: ["azure", "azure devops artifacts", "continuous integration", "credential management", "dependabot", "devops", "github actions", "improvement", "news", "oidc authentication", "openid connect", "private registries", "security", "supply chain security"]
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
