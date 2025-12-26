---
layout: "post"
title: "Serverless Workspaces Are Now Available in Azure Databricks"
description: "This announcement from AnaviNahar introduces the public preview of Serverless workspaces in Azure Databricks. The article explains how serverless workspaces simplify the deployment and management of infrastructure for large-scale data, analytics, and AI projects. It details the new features available—including automatic storage management, managed compute, network security configurations, consistent data governance through Unity Catalog, bring-your-own-data options, and cost management features. Guidance is provided on choosing between Serverless and Classic workspaces, along with considerations around region availability and compute/service limitations."
author: "AnaviNahar"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/serverless-workspaces-are-live-in-azure-databricks/ba-p/4474712"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-12-02 23:08:45 +00:00
permalink: "/community/2025-12-02-Serverless-Workspaces-Are-Now-Available-in-Azure-Databricks.html"
categories: ["AI", "Azure", "ML", "Security"]
tags: ["AI", "AI Projects", "Azure", "Azure Databricks", "Cloud Security", "Community", "Cost Management", "Data Analytics", "Data Governance", "Managed Compute", "ML", "Network Security", "Object Storage", "Preview Features", "Python", "Region Availability", "Security", "Serverless Workspaces", "SQL", "Unity Catalog", "Workspace Configuration"]
tags_normalized: ["ai", "ai projects", "azure", "azure databricks", "cloud security", "community", "cost management", "data analytics", "data governance", "managed compute", "ml", "network security", "object storage", "preview features", "python", "region availability", "security", "serverless workspaces", "sql", "unity catalog", "workspace configuration"]
---

AnaviNahar announces the public preview of Serverless workspaces in Azure Databricks, describing how this feature streamlines setup for analytics and AI projects by automating infrastructure management and governance.<!--excerpt_end-->

# Serverless Workspaces Are Now Available in Azure Databricks

Azure Databricks has launched public preview support for Serverless workspaces, making it easier for teams to deploy and manage large-scale data and AI projects on Azure. Serverless workspaces remove the complexity of manual infrastructure setup by providing:

- **Instant Workspace Creation:** Fast creation with serverless compute and built-in default storage.
- **Automated Infrastructure:** No need to configure VNets, clusters, or storage credentials.
- **Unity Catalog Integration:** Automated catalog and governance setup for seamless user access control.
- **Managed Storage:** Create managed catalogs, tables, and volumes without bringing external Azure Blob storage.
- **Serverless Compute:** Workloads run without cluster provisioning, allowing data scientists and analysts to focus on their tasks.
- **Network Security:** Define serverless egress policies and Private Link rules, eliminating the need for manual network components. [Serverless Private Link documentation](https://learn.microsoft.com/en-us/azure/databricks/security/network/serverless-network-security/pl-to-internal-network)
- **Cost Management:** Attribution tags and budget policies can be configured for spend analysis.

## Feature Comparison

- **Serverless Workspaces:** Best for rapid setup and projects where minimizing infrastructure management is key. Built-in governance, automated storage and compute, and easy scaling.
- **Classic Workspaces:** Use when custom network design or direct resource management is required.

## Security and Governance Highlights

- **Unity Catalog:** Ensures existing governance and permissions are maintained across serverless workspaces.
- **Network Security:** Serverless egress policies simplify outbound connectivity management.
- **Managed Access:** Data access is limited and protected with multi-key projection; direct access to object storage is prevented.

## Technical Constraints and Region Support

- Serverless workspaces are available only in regions with serverless compute support. (See [supported regions](https://learn.microsoft.com/en-us/azure/databricks/resources/supported-regions).)
- Not all legacy APIs or workloads are supported; focus is on Python and SQL. [Serverless limitations](https://learn.microsoft.com/en-us/azure/databricks/compute/serverless/limitations)
- Default storage billing is currently inactive; Azure will notify on future changes.

## Getting Started

Learn how to deploy a serverless workspace and explore the simplified setup process [here](https://learn.microsoft.com/en-us/azure/databricks/admin/workspace/serverless-workspaces).

## Summary Checklist

- Create workspaces quickly for analytics/AI workloads
- Governance and security inherited from Unity Catalog
- Network and compute managed automatically
- Bring your own data when needed
- Built-in cost management with budget tags

## References

- [Azure Databricks Serverless Workspaces Documentation](https://learn.microsoft.com/en-us/azure/databricks/compute/serverless/limitations)
- [Network Security for Serverless Workloads](https://www.databricks.com/trust/security-features/serverless-egress-controls)

If your team is looking to accelerate analytics, AI, or data science projects on Azure, Serverless workspaces provide simplified setup, secure data management, and clear budget controls.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/serverless-workspaces-are-live-in-azure-databricks/ba-p/4474712)
