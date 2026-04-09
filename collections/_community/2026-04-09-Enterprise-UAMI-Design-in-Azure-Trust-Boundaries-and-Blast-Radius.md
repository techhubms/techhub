---
date: 2026-04-09 08:13:33 +00:00
tags:
- AKS
- Azure
- Azure App Service
- Azure Functions
- Azure Instance Metadata Service
- Azure Key Vault
- Azure RBAC
- Azure SQL Database
- Azure Storage
- Blast Radius
- Community
- Environment Isolation
- IMDS
- Least Privilege
- Managed Identity
- Microsoft Entra ID
- Resource Group Scope
- Secretless Authentication
- Security
- Subscription Scope
- System Assigned Managed Identity
- Trust Boundaries
- User Assigned Managed Identity
- Virtual Machines
feed_name: Microsoft Tech Community
author: AmitManchanda28
title: 'Enterprise UAMI Design in Azure: Trust Boundaries and Blast Radius'
section_names:
- azure
- security
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/enterprise-uami-design-in-azure-trust-boundaries-and-blast/ba-p/4509614
primary_section: azure
---

AmitManchanda28 explains how reusing a User Assigned Managed Identity (UAMI) across Azure environments can unintentionally widen trust boundaries and increase blast radius, and proposes an environment-isolated identity model with tighter RBAC scoping.<!--excerpt_end-->

# Enterprise UAMI Design in Azure: Trust Boundaries and Blast Radius

As organizations move toward **secretless authentication** in Azure, **Managed Identity** is often the preferred way to enable secure service-to-service access without storing credentials.

This post focuses on **User Assigned Managed Identity (UAMI)** and how identity reuse can unintentionally expand trust boundaries in enterprise deployments.

## Why UAMI design matters

UAMI offers flexibility because it can be reused across multiple compute resources such as:

- Azure App Service
- Azure Function Apps
- Virtual Machines
- Azure Kubernetes Service (AKS)

That reuse can simplify operations, but it also introduces architectural risks that are easy to miss during initial rollout—especially in enterprises with shared infrastructure patterns.

## Understanding identity scope in Azure

Compared to **System Assigned Managed Identity**, a **UAMI**:

- Exists independently from the lifecycle of any single compute resource
- Can be attached to multiple services across:
  - Resource Groups
  - Subscriptions
  - Environments

This means a single identity can be reused across DEV/UAT/PROD if desired.

### The trust boundary issue

When an identity is reused across multiple logical environments, **any permission granted to that identity is implicitly available to all services attached to it**.

Architecturally, that creates a shared authentication surface across environments that many teams assume are isolated.

## Shared identity pattern (common enterprise anti-pattern)

Common patterns seen in large Azure deployments include:

- A single UAMI assigned to multiple App Services
- The same identity reused across automation workloads
- Centrally provisioned identities that are attached dynamically

While this reduces identity sprawl, it can introduce **unintended privilege propagation**.

### What happens at runtime

In the shared-identity architecture described:

- Multiple App Services across environments share the same managed identity.
- Each compute instance requests an access token from **Microsoft Entra ID** using **Azure Instance Metadata Service (IMDS)**.
- The issued token is used to authenticate to downstream services, for example:
  - Azure SQL Database
  - Azure Key Vault
  - Azure Storage

Because **RBAC permissions** are assigned to the shared identity (not to a specific environment’s compute instances), the effective boundary becomes **identity-scoped** rather than **environment-scoped**.

### Resulting risk

If a lower-tier environment (like DEV) is compromised, an attacker may be able to obtain an access token via IMDS that still has access to production dependencies—**if the shared identity has PROD-level RBAC**.

That increases the potential blast radius.

## Blast radius considerations

**Blast radius** is the potential scope of impact from a security or configuration compromise.

Using a shared UAMI across multiple services can increase blast radius in several common scenarios:

| Design pattern | Potential risk |
| --- | --- |
| Single UAMI across environments | Cross-environment access |
| Subscription-wide RBAC assignment | Broad privilege scope |
| Identity used for automation pipelines | Lateral movement |
| Shared identity across teams | Ownership ambiguity |

Because Managed Identity relies on **IMDS**, any compromised compute resource with access to IMDS can request a token for the attached identity and then use that token against Azure services where the identity has permissions.

## Enterprise design recommendations: environment-isolated identity model

To reduce identity blast radius, the post recommends aligning identity boundaries with environment boundaries.

### Environment-scoped identity

Provision separate UAMIs per environment, for example:

- UAMI-DEV
- UAMI-UAT
- UAMI-PROD

Avoid reusing the same identity across isolated lifecycle stages.

### Resource-level RBAC assignment

Prefer RBAC assignments scoped at:

- Resource
- Resource Group

Instead of subscription scope wherever feasible.

### Identity ownership model

Ensure clear ownership for identities, aligned to:

- Application ownership
- Service ownership
- Deployment boundary

### Least privilege assignment

Prefer narrow roles such as:

- Key Vault Secrets User
- Storage Blob Data Reader

Instead of broader roles such as:

- Contributor
- Owner

## Recommended high-level architecture

In the recommended approach:

- Each App Service instance is attached to an environment-specific managed identity.
- RBAC assignments are scoped at the resource or resource group level.
- Microsoft Entra ID issues tokens independently for each identity.
- Trust boundaries remain aligned with deployment environments.

A compromised DEV compute instance can only obtain a token for **UAMI-DEV**. If UAMI-DEV does not have RBAC permissions for production resources, lateral access to PROD dependencies is prevented.

## Blast radius containment

This design reduces blast radius by ensuring:

- Identity compromise remains environment-scoped
- Token issuance does not grant unintended cross-environment privileges
- RBAC aligns with application ownership boundaries
- Authentication trust boundaries match deployment lifecycle boundaries

## Conclusion

UAMI is a strong option for secretless authentication in Azure, but identity reuse can unintentionally widen trust boundaries.

By using environment-specific identities and tighter RBAC scoping, enterprises can balance operational efficiency with stronger security governance.


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/enterprise-uami-design-in-azure-trust-boundaries-and-blast/ba-p/4509614)

