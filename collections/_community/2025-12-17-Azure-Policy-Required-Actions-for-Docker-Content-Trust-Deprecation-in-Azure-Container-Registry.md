---
layout: post
title: 'Azure Policy: Required Actions for Docker Content Trust Deprecation in Azure Container Registry'
author: ShannonHicks
canonical_url: https://techcommunity.microsoft.com/t5/azure-governance-and-management/azure-policy-required-actions-for-docker-content-trust/ba-p/4478951
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-12-17 22:36:46 +00:00
permalink: /azure/community/Azure-Policy-Required-Actions-for-Docker-Content-Trust-Deprecation-in-Azure-Container-Registry
tags:
- ACR
- API Deprecation
- ARM API
- Azure Container Registry
- Azure Policy
- Cloud Governance
- Container Security
- Docker Content Trust
- Notary Project
- Policy Compliance
- Policy Definition
- Security Compliance
- Trustpolicy
section_names:
- azure
- devops
- security
---
ShannonHicks outlines the ramifications of Azure deprecating Docker Content Trust in Azure Container Registry, detailing steps Azure Policy administrators must take to ensure continued compliance and secure container governance.<!--excerpt_end-->

# Azure Policy: Required Actions for Docker Content Trust Deprecation in Azure Container Registry

As Azure evolves, certain features are deprecated to streamline services and improve security and performance. One such upcoming change is the [deprecation of the Docker Content Trust (DCT) feature in Azure Container Registry (ACR)](https://azure.microsoft.com/en-us/updates?searchterms=content+trust), which will take place over a three-year period. As part of this, the *trustPolicy* property will eventually be removed from underlying APIs.

This article explains what changes are coming, how they impact your Azure Policy environment, and practical steps for policy administrators to avoid disruption.

## What is Changing?

- **Docker Content Trust (DCT)** is being deprecated for ACR.
- The *trustPolicy* property will be removed from Azure Resource Manager (ARM) APIs in the future.
- Any Azure Policy [aliases](https://learn.microsoft.com/en-us/azure/governance/policy/concepts/definition-structure-alias) referencing this property—such as:
  - `Microsoft.ContainerRegistry/registries/trustPolicy`
  - `Microsoft.ContainerRegistry/registries/trustPolicy.type`
  - `Microsoft.ContainerRegistry/registries/trustPolicy.status`
  — will be affected.
- No built-in policy definitions currently use these aliases, so no built-in policies will be deprecated because of this change.
- The alias `trustPolicy.status` is modifiable, so any custom modify policies using it will break when the property is removed.

## Impacts on Azure Policy

- If you have custom policy assignments referencing these aliases, update or remove them during the deprecation window to avoid compliance issues.
- Policies requiring `trustPolicy` to be enabled (e.g. `Microsoft.ContainerRegistry/registries/trustPolicy.status == "enabled"`) will become non-compliant for any new ACRs when the property is removed.
- Modify policies targeting `trustPolicy.status` will fail once the alias is deleted or changed to non-modifiable.

## Steps to Mitigate Impact

1. **Identify Affected Policies and Assignments:** Review all custom policies in your environment that reference these TrustPolicy-related aliases.
2. **Update Policy Definitions:** Remove or update references to `trustPolicy` properties. If a policy’s only function is to evaluate ACR trustPolicy, consider retiring the policy.
3. **Test and Validate:** After updating, ensure that your policies still enforce compliance as intended, without relying on deprecated properties.
4. **Monitor for Updates:** Follow Azure Container Registry [retirement documentation](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-content-trust-deprecation) for future updates and to learn about transitioning to **Notary Project** as a replacement solution.

Staying proactive will help maintain security and compliance as Azure phases out Docker Content Trust for container registries.

---
**Author:** ShannonHicks

*Published: Dec 17, 2025*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-governance-and-management/azure-policy-required-actions-for-docker-content-trust/ba-p/4478951)
