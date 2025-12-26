---
layout: "post"
title: "Announcing Public Preview for Azure Service Groups"
description: "This announcement introduces Azure Service Groups—a new Azure resource container that enables flexible membership, hierarchy management, and enhanced observability across your Azure tenant. Service Groups are designed for low-privilege management and allow you to group resources from anywhere in your tenant, supporting advanced monitoring and organizational scenarios without impacting RBAC or policy settings."
author: "kenieva"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-governance-and-management/announcing-public-preview-for-azure-service-groups/ba-p/4446572"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-21 17:58:39 +00:00
permalink: "/community/2025-08-21-Announcing-Public-Preview-for-Azure-Service-Groups.html"
categories: ["Azure"]
tags: ["Azure", "Azure Governance", "Azure Monitor", "Azure Service Groups", "Cloud Management", "Community", "Hierarchy Management", "Management Groups", "Microsoft Entra", "Observability", "RBAC", "Resource Management", "REST API", "Tenant Level Resources"]
tags_normalized: ["azure", "azure governance", "azure monitor", "azure service groups", "cloud management", "community", "hierarchy management", "management groups", "microsoft entra", "observability", "rbac", "resource management", "rest api", "tenant level resources"]
---

kenieva introduces Azure Service Groups, now in public preview. This Azure feature allows flexible grouping and monitoring of resources at the tenant level, offering low-privilege management with support for varied hierarchies.<!--excerpt_end-->

# Announcing Public Preview for Azure Service Groups!

Azure has introduced **Service Groups (SGs)**, a new resource container available for all Azure customers. Service Groups enhance flexibility in grouping, managing, and observing resources spread across the Azure tenant.

## What are Service Groups?

Service Groups are tenant-level resources that enable new management and observability scenarios, especially where resource membership and hierarchy need to be dynamic. Unlike other containers, Service Groups operate independently of tenant-wide RBAC or Policy features, meaning their access model is isolated from broader organizational permissions.

## Key Features

- **Low Privilege Management:** Designed to operate with minimal permissions, Service Groups let users manage resources without elevated access, appealing to a broad set of users and scenarios. Membership in a Service Group does not transfer RBAC or policy assignments.
- **Flexible Membership and Nested Hierarchies:** Resources from anywhere in your Azure tenant can be added to one or multiple service groups. Service Groups can also be nested, enabling multi-dimensional hierarchies based on cost centers, products, organizations, and more.
- **Monitoring and Observability:** Azure Monitor features, such as [Health Models](https://learn.microsoft.com/azure/azure-monitor/health-models/overview), are integrated, allowing you to troubleshoot, investigate, and monitor the health of Service Groups.

## When to Use Service Groups

Service Groups are most useful in environments where resources span across different management containers, leading to complexity in observation and control. Use Service Groups to:

- Model application or service hierarchies that don't fit existing Azure containers
- Enable targeted monitoring and organizational reporting across arbitrary scopes
- Enhance visibility without incurring broad permission changes

Limitations: Service Groups are not deployment scopes and cannot be used to assign RBAC or Azure Policy.

## Getting Started

- **Via REST API:** [Quickstart Guide](https://learn.microsoft.com/azure/governance/service-groups/create-service-group-rest-api)
- **Azure Portal:** [Service Groups Management](https://portal.azure.com/#view/Microsoft_Azure_Resources/ServiceGroupsBrowse.ReactView)
- **Documentation:** [Service Groups Overview](https://learn.microsoft.com/azure/governance/service-groups/overview)

## FAQ

**Do Service Groups replace existing Azure groups?**
> No. They are designed to work in parallel with current containers like Management Groups or Resource Groups. See the [scenario comparison](https://learn.microsoft.com/azure/governance/service-groups/overview#scenario-comparison) for details.

**Who can create Service Groups?**
> Anyone with a valid Azure user account in a Microsoft Entra directory.

**Why tenant-level?**
> To support cross-tenant membership, enabling global resource grouping without granting broad tenant access. Unlike Management Groups, Service Groups do not provide users with wide-ranging rights.

## Feedback and Support

Contact the team at [azureservicegroups@microsoft.com](mailto:azureservicegroups@microsoft.com) for feedback or questions.

*Published: August 21, 2025*

---
*Author: kenieva*

For more information, visit the [Azure Governance and Management Blog](https://techcommunity.microsoft.com/t5/azure-governance-and-management-blog/bg-p/AzureGovernanceandManagementBlog).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-governance-and-management/announcing-public-preview-for-azure-service-groups/ba-p/4446572)
