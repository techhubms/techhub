---
layout: "post"
title: "Azure Arc-Enabled SQL Server Now Generally Available in US Government Virginia Region"
description: "This announcement details the general availability of Azure Arc-enabled SQL Server in the US Government Virginia region. It outlines core features such as onboarding, inventory management, compliance, and licensing for SQL Server instances managed outside of Azure, as well as current limitations and links to technical resources. The post targets agencies and organizations leveraging hybrid data management capabilities via Azure Arc."
author: "AbdullahMSFT"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-arc-blog/sql-server-enabled-by-azure-arc-is-now-generally-available-in/ba-p/4443077"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-14 21:43:34 +00:00
permalink: "/2025-08-14-Azure-Arc-Enabled-SQL-Server-Now-Generally-Available-in-US-Government-Virginia-Region.html"
categories: ["Azure"]
tags: ["Azure", "Azure Arc", "Azure Arc Onboarding", "Azure Government", "Azure Portal", "Cloud Compliance", "Cloud Security", "Community", "Data Estate Management", "Extended Security Updates", "Government Cloud", "Hybrid Data Management", "Licensing", "Public Sector", "Resource Monitoring", "SQL Server", "SQL Server Inventory"]
tags_normalized: ["azure", "azure arc", "azure arc onboarding", "azure government", "azure portal", "cloud compliance", "cloud security", "community", "data estate management", "extended security updates", "government cloud", "hybrid data management", "licensing", "public sector", "resource monitoring", "sql server", "sql server inventory"]
---

AbdullahMSFT introduces the general availability of Azure Arc-enabled SQL Server in the US Government Virginia region, highlighting streamlined hybrid management and new capabilities for government organizations.<!--excerpt_end-->

# Azure Arc-Enabled SQL Server Now Generally Available in US Government Virginia Region

**Author:** AbdullahMSFT

Azure Arc-enabled SQL Server on Windows is now generally available for the US Government Virginia region, providing government agencies and organizations with a secure and compliant way to manage SQL Server instances running outside of Azure directly from the Azure Government portal.

## Key Features

- **Onboarding:** Seamlessly connect your SQL Server instances to Azure Arc ([documentation](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/connect?view=sql-server-ver17)), making them visible as resources within the Azure Government portal.
- **Inventory Management:** Leverage the Azure portal to view all SQL Server instances and their properties (version, edition, databases) centrally, aiding large-scale cloud estate management.
- **Security and Compliance:** Subscribe to [Extended Security Updates](https://review.learn.microsoft.com/en-us/sql/sql-server/azure-arc/extended-security-updates?view=sql-server-ver17#subscribe-to-extended-security-updates-in-a-production-environment) for production workloads and manage licensing/billing ([details](https://review.learn.microsoft.com/en-us/sql/sql-server/azure-arc/manage-license-billing?view=sql-server-ver17)).
- **Licensing Options:** Manage virtual core licenses (review [limitations](https://review.learn.microsoft.com/en-us/sql/sql-server/azure-arc/us-government-region?view=sql-server-ver17&branch=main#limitations)), monitor consumption, and track compliance for SQL Server deployments in the Government cloud.

## How to Get Started

1. [Connect hybrid machines](https://learn.microsoft.com/en-us/azure/azure-arc/servers/learn/quick-enable-hybrid-vm) to Azure Arc-enabled servers.
2. [Onboard SQL Server instances](https://learn.microsoft.com/sql/sql-server/azure-arc/connect-already-enabled) to Azure Arc on the hybrid-enabled server.

## Limitations

Certain SQL Server features are not yet available in US Government regions, including:

- Failover cluster instances (FCI) and Availability Groups (AG)
- Licensing physical cores (p-cores) with unlimited virtualization or without VMs
- Associated services: [SQL Server Analysis Services](https://review.learn.microsoft.com/en-us/azure/analysis-services/), [Integration Services](https://review.learn.microsoft.com/en-us/sql/integration-services/sql-server-integration-services?view=sql-server-ver17), [Reporting Services](https://review.learn.microsoft.com/en-us/sql/reporting-services/create-deploy-and-manage-mobile-and-paginated-reports?view=sql-server-ver17), [Power BI Report Server](https://review.learn.microsoft.com/en-us/power-bi/report-server/get-started)

For all feature differentiation and up-to-date limitations, consult the [official documentation](https://review.learn.microsoft.com/en-us/sql/sql-server/azure-arc/overview?view=sql-server-ver17#feature-differentiation).

## Roadmap

This milestone introduces hybrid data management within Azure Government; future iterations will continue to close feature gaps and achieve service parity with commercial regions.

## Get Involved

- Try Arc-enabled SQL Server through the [Azure Government portal](https://learn.microsoft.com/sql/sql-server/azure-arc/us-government-region?).
- Share feedback through the [community forum](https://feedback.azure.com/d365community/forum/04fe6ee0-3b25-ec11-b6e6-000d3a4f0da0) or your Microsoft representatives.

## Learn More

- [Azure Arc-enabled SQL Server overview](https://learn.microsoft.com/sql/sql-server/azure-arc/overview?view=sql-server-ver17)
- [How to onboard and manage SQL Server resources in Azure Arc](https://learn.microsoft.com/en-us/sql/sql-server/azure-arc/connect?view=sql-server-ver17)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/sql-server-enabled-by-azure-arc-is-now-generally-available-in/ba-p/4443077)
