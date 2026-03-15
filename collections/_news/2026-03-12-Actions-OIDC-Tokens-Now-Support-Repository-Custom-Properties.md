---
external_url: https://github.blog/changelog/2026-03-12-actions-oidc-tokens-now-support-repository-custom-properties
title: Actions OIDC Tokens Now Support Repository Custom Properties
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-03-12 14:53:28 +00:00
tags:
- ABAC
- Actions
- Attribute Based Access Control
- AWS
- Cloud Governance
- DevOps
- DevOps Automation
- GCP
- GitHub Actions
- GitHub Enterprise
- Identity Management
- News
- OIDC
- OpenID Connect
- Repository Custom Properties
- Security
- Security Claims
section_names:
- devops
- security
---
Allison reports on a significant update to GitHub Actions OIDC token functionality: repository custom properties can now be added as claims, supporting stronger and more flexible policy controls across major cloud providers.<!--excerpt_end-->

# Actions OIDC Tokens Now Support Repository Custom Properties

GitHub Actions OpenID Connect (OIDC) tokens can now include repository custom properties as claims, enhancing the way identity and governance are managed across cloud platforms.

## Key Features

- **Repository Custom Properties as Claims:** Admins at the repository, organization, or enterprise level can configure OIDC tokens to include custom properties. These properties are automatically prefixed with `repo_property_` and included in issued OIDC tokens.
- **Centralized Configuration:** A new settings page (currently in public preview) allows easy claim configuration through the UI or API.
- **Automatic Policy Integration:** By embedding repository metadata into OIDC tokens, organizations can implement attribute-based access control (ABAC) without modifying individual workflows.

## Benefits

- **Eliminate Duplication:** Governance metadata is centrally managed and automatically included in tokens used for cloud provider policies.
- **Reduce Configuration Drift:** Policies tied to repository attributes remain accurate as repositories and organizations change over time.
- **Accelerate Onboarding:** New repositories inherit relevant access policies based on assigned properties without additional configuration.
- **Consistent Cross-Cloud Policies:** Enables unified policy management and access control across AWS, Azure, GCP, and other providers.

## How to Use

- **Add repository custom properties** in the OIDC token via the API or the settings UI.
- **Use custom properties in the subject claim** to enable flexible policy targeting.
- **Manage OIDC token claim configuration** at the repository, organization, and enterprise level through the new UI.

Cloud provider trust policies can now reference these enriched claims for secure access management without reliance on static allow lists or manual workflow adjustments.

For more information, see [Customizing the OIDC token](https://docs.github.com/actions/security-for-github-actions/security-hardening-your-deployments/about-security-hardening-with-openid-connect#customizing-the-token-claims).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-03-12-actions-oidc-tokens-now-support-repository-custom-properties)
