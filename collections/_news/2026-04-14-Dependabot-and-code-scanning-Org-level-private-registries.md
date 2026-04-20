---
feed_name: The GitHub Blog
external_url: https://github.blog/changelog/2026-04-14-dependabot-and-code-scanning-org-level-private-registries
title: 'Dependabot and code scanning: Org-level private registries'
primary_section: devops
tags:
- AWS CodeArtifact
- Azure DevOps Artifacts
- Code Scanning
- Dependabot
- DevOps
- Docker Registries
- GHES 3.24
- GitHub Advanced Security
- GitHub Enterprise Cloud
- GitHub Enterprise Server
- Improvement
- JFrog Artifactory
- Maven
- Multiple Registries Per Ecosystem
- News
- npm
- NuGet
- OIDC
- OpenID Connect
- Organization Security Settings
- pip
- Private Registries
- REST API
- RubyGems
- Security
- Supply Chain Security
author: Allison
date: 2026-04-14 18:16:23 +00:00
section_names:
- devops
- security
---

Allison announces an update that lets GitHub organizations configure multiple private registries per package ecosystem for Dependabot and code scanning, including org-level OIDC authentication support via the UI and REST API.<!--excerpt_end-->

# Dependabot and code scanning: Org-level private registries

It’s now easier to configure Dependabot and code scanning for organizations that rely on multiple internal package feeds.

Previously, organization-level settings only allowed a single private registry configuration per ecosystem type (for example, one Maven registry or one npm registry). Now you can register all of your private feeds for the same ecosystem at the organization level.

## What changed

### Multiple registries per ecosystem

You can add as many private registries as needed for supported ecosystems, including:

- npm
- Maven
- NuGet
- Docker
- pip
- RubyGems
- Other supported ecosystems

This is configured directly from your organization’s security settings.

### OIDC authentication support

You can configure OIDC-based authentication for org-level private registries via:

- The GitHub UI
- The REST API

OIDC support includes integrations with:

- Azure DevOps Artifacts
- AWS CodeArtifact
- JFrog Artifactory

## Availability

- Available now on github.com and GitHub Enterprise Cloud
- Will be available on GitHub Enterprise Server starting with **GHES 3.24**

## References

- Configure Dependabot access to private registries: https://docs.github.com/code-security/dependabot/working-with-dependabot/configuring-access-to-private-registries-for-dependabot
- Discussion: https://github.com/orgs/community/discussions/categories/announcements


[Read the entire article](https://github.blog/changelog/2026-04-14-dependabot-and-code-scanning-org-level-private-registries)

