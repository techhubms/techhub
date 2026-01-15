---
layout: post
title: Enforcing and Auditing Policy Inheritance in Azure API Management
author: budzynski
canonical_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/enforce-or-audit-policy-inheritance-in-api-management/ba-p/4452204
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-09-09 14:47:14 +00:00
permalink: /azure/community/Enforcing-and-Auditing-Policy-Inheritance-in-Azure-API-Management
tags:
- API Management
- API Security
- Audit
- Azure
- Azure Policy
- Azure Portal
- Community
- Compliance
- Governance
- Operational Requirements
- Policy Enforcement
- Policy Inheritance
- Security
- Security Controls
section_names:
- azure
- security
---
budzynski explains how Azure Policy now enables enforcement and auditing of policy inheritance within Azure API Management, offering practical steps to enhance consistency, compliance, and security across APIs.<!--excerpt_end-->

# Enforcing and Auditing Policy Inheritance in Azure API Management

A new Azure Policy definition strengthens governance in Azure API Management by ensuring that policy inheritance is consistently applied across policy scopes (operations, APIs, products, and workspaces). This empowers platform and governance teams to enforce or audit the use of the `<base />` policy element, which is essential for:

- Inheriting security controls (e.g., authentication, IP restrictions)
- Enforcing operational requirements (e.g., logging, tracing, rate-limiting)
- Applying business policies (e.g., quota enforcement)

Without proper policy inheritance, critical platform rules can be bypassed, resulting in inconsistency, compliance drift, and governance gaps.

## How the New Policy Definition Works

- **Automatic Enforcement/Audit**: The Azure Policy checks that `<base />` is present at the start of each API Management policy section (`inbound`, `outbound`, `backend`, and `on-error`) for all operations, APIs, products, and workspaces.
- **Effect Parameter**:
  - `Audit`: Flags policies missing `<base />` for review
  - `Deny`: Blocks deployment of policies that do not include `<base />`

## Getting Started

1. In the [Azure Portal](https://portal.azure.com/), navigate to **Azure Policy**.
2. Go to **Definitions** and search for "API Management policies should inherit parent scope policies using <base />".
3. In the policy view, select **Assign**.
4. Configure the policy assignment scope, set parameters (choose audit or deny), and save your assignment.

## Benefits

- Prevents circumvention of crucial security, operational, or business rules
- Increases consistency and compliance across API estates
- Reduces risk of configuration mistakes and governance drift

## Additional Resources

- [Built-in Azure Policy definitions for API Management](https://learn.microsoft.com/azure/api-management/policy-reference)

_Last updated: Sep 09, 2025 – Version 1.0_

---

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/enforce-or-audit-policy-inheritance-in-api-management/ba-p/4452204)
