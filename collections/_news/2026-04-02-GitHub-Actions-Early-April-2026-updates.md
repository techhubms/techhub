---
section_names:
- azure
- devops
- security
title: 'GitHub Actions: Early April 2026 updates'
date: 2026-04-02 17:11:45 +00:00
tags:
- Actions
- Audit Log Events
- Azure
- Azure Private Networking
- CI/CD
- Cloud Access Control
- Command Override
- DevOps
- Docker Compose
- Entrypoint Override
- GitHub Actions
- GitHub Hosted Runners
- Improvement
- News
- OIDC
- OIDC Token Claims
- OpenID Connect
- Regional Outage Failover
- Repository Custom Properties
- REST API
- Security
- Service Containers
- Subnet Failover
- Trust Policies
- Virtual Network (vnet)
- Workflow YAML
external_url: https://github.blog/changelog/2026-04-02-github-actions-early-april-2026-updates
primary_section: azure
feed_name: The GitHub Blog
author: Allison
---

Allison summarizes early April 2026 GitHub Actions updates, covering service container entrypoint/command overrides, OIDC token claims for repository custom properties, and Azure private networking VNet failover for hosted runners.<!--excerpt_end-->

# GitHub Actions: Early April 2026 updates

This update covers three changes in GitHub Actions: improved configuration for service containers, more granular OIDC-based access control via repository custom properties, and new failover support for Azure private networking with GitHub-hosted runners.

## Customizing entrypoints for service containers

GitHub Actions now supports overriding the default image configuration for **service containers**.

- You can use new `entrypoint` and `command` keys to override the image defaults from your workflow YAML.
- The naming and behavior match **Docker Compose**, so the syntax should feel familiar.

References:

- Service containers: https://docs.github.com/actions/tutorials/use-containerized-services/use-docker-service-containers
- Workflow syntax for services: https://docs.github.com/actions/reference/workflows-and-actions/workflow-syntax#jobsjob_idservices

## Actions OIDC tokens now support repository custom properties

GitHub Actions **OpenID Connect (OIDC) tokens** now include **repository custom properties** as claims, and this capability is now **generally available**.

Previously available in public preview:

- https://github.blog/changelog/2026-03-12-actions-oidc-tokens-now-support-repository-custom-properties/

You can use repository custom properties in your OIDC claims to create more granular cloud trust policies without enumerating individual repository names or IDs.

With this update, you can:

- Define trust policies based on custom property values, such as environment type, team ownership, or compliance tier.
- Reduce the overhead of maintaining per-repository cloud role configurations.
- Align cloud access controls with your organization’s repository governance model.

Getting started (high level):

- Configure **custom properties** on repositories.
- Reference those properties in your cloud provider’s **OIDC trust policy**.

Reference:

- OIDC token claims documentation: https://docs.github.com/actions/concepts/security/openid-connect

## Azure private networking now supports VNET failover

**Azure private networking for GitHub Actions hosted runners** now supports **failover networks** in **public preview**.

Key points:

- You can configure a **secondary Azure subnet**, optionally in a different region.
- Workflows can keep running if the primary subnet becomes unavailable.
- Failover can be triggered:
  - Manually via the network configuration UI or **REST API**
  - Automatically by GitHub during a regional outage
- When a failover occurs:
  - Enterprise and organization admins are notified via **audit log events** and email
- If you trigger failover manually, you’re responsible for switching back once the primary region is available.

Availability:

- Enterprise and organization accounts using Azure private networking for GitHub-hosted runners.

Reference:

- Azure private networking docs: https://docs.github.com/enterprise-cloud@latest/admin/configuring-settings/configuring-private-networking-for-hosted-compute-products/about-azure-private-networking-for-github-hosted-runners-in-your-enterprise


[Read the entire article](https://github.blog/changelog/2026-04-02-github-actions-early-april-2026-updates)

