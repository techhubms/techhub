---
layout: "post"
title: "Azure Arc-enabled SQL Server Now Generally Available in US Government Virginia Region"
description: "This announcement covers the general availability of Azure Arc-enabled SQL Server on Windows in the US Government Virginia region. It explains new management capabilities for SQL Server instances outside Azure, accessible through the Azure Government portal. The article breaks down onboarding steps, available features, and current limitations, as well as future improvement plans for the service."
author: "AbdullahMSFT"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-arc-blog/arc-enabled-sql-server-is-now-generally-available-in-the-us/ba-p/4443077"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-14 19:50:41 +00:00
permalink: "/2025-08-14-Azure-Arc-enabled-SQL-Server-Now-Generally-Available-in-US-Government-Virginia-Region.html"
categories: ["Azure"]
tags: ["Azure", "Azure Arc", "Azure Government", "Cloud Management", "Community", "Compliance", "Data Estate", "Feature Limitations", "Hybrid Cloud", "Hybrid Data Management", "Licensing", "Security Updates", "SQL Server", "SQL Server Inventory", "US Government Cloud", "Windows Server"]
tags_normalized: ["azure", "azure arc", "azure government", "cloud management", "community", "compliance", "data estate", "feature limitations", "hybrid cloud", "hybrid data management", "licensing", "security updates", "sql server", "sql server inventory", "us government cloud", "windows server"]
---

AbdullahMSFT announces the general availability of Azure Arc-enabled SQL Server for the US Government Virginia region, explaining how government users can securely manage their SQL Server instances through the Azure Government portal.<!--excerpt_end-->

# Azure Arc-enabled SQL Server Now Generally Available in US Government Virginia Region

**Author:** AbdullahMSFT

Azure Arc-enabled SQL Server on Windows is now generally available in the US Government Virginia region. With this expansion, U.S. government agencies and organizations can centrally manage their SQL Server instances outside Azure directly from the Azure Government portal, supporting security and compliance needs unique to government workloads.

## Key Features Available

- **Onboard SQL Server Instances:** Easily connect your SQL Server to Azure Arc ([onboarding steps](https://review.learn.microsoft.com/en-us/sql/sql-server/azure-arc/connect?view=sql-server-ver17)), making your SQL Servers manageable as Azure resources.
- **SQL Server Inventory:** View SQL Server and database resources, access instance properties like version and edition, and monitor your entire SQL Server estate from a single interface.
- **Extended Security Updates:** Subscribe to Extended Security Updates for production environments to maintain security compliance.
- **License and Billing Management:** Manage licensing (virtual cores) and review billing directly via the Azure Government portal. ([See licensing limitations](https://review.learn.microsoft.com/en-us/sql/sql-server/azure-arc/us-government-region?view=sql-server-ver17&branch=main#limitations)).

*Note: [Other features](https://review.learn.microsoft.com/en-us/sql/sql-server/azure-arc/overview?view=sql-server-ver17#feature-differentiation) may not yet be available in this region.*

## Onboarding Process

To onboard your SQL Server to Azure Arc in US Gov Virginia:

1. [Connect your hybrid machines with Azure Arc-enabled servers](https://learn.microsoft.com/en-us/azure/azure-arc/servers/learn/quick-enable-hybrid-vm).
2. [Connect your SQL Server to Azure Arc on a server already enabled by Azure Arc](https://learn.microsoft.com/sql/sql-server/azure-arc/connect-already-enabled).

## Limitations

Some features are not currently available in US Government regions:

- Failover cluster instance (FCI)
- Availability groups (AG)
- Licensing for physical cores (p-cores) / unlimited virtualization
- License for p-cores without VMs
- Associated services:
  - [SQL Server Analysis Services](https://review.learn.microsoft.com/en-us/azure/analysis-services/)
  - [SQL Server Integration Services](https://review.learn.microsoft.com/en-us/sql/integration-services/sql-server-integration-services?view=sql-server-ver17)
  - [SQL Server Reporting Services](https://review.learn.microsoft.com/en-us/sql/reporting-services/create-deploy-and-manage-mobile-and-paginated-reports?view=sql-server-ver17)
  - [Power BI Report Server](https://review.learn.microsoft.com/en-us/power-bi/report-server/get-started)

Review the [feature differentiation documentation](https://review.learn.microsoft.com/en-us/sql/sql-server/azure-arc/overview?view=sql-server-ver17#feature-differentiation) for the most current availability.

## Looking Ahead

This release is a foundational step for hybrid data management on Azure Government. Microsoft plans to roll out further enhancements to bring the service to parity with other regions.

## Learn More

- [SQL Server enabled by Azure Arc in US Government](https://learn.microsoft.com/sql/sql-server/azure-arc/us-government-region?)
- [SQL Server enabled by Azure Arc - Overview](https://learn.microsoft.com/sql/sql-server/azure-arc/overview?view=sql-server-ver17)

For feedback and discussion, visit the [community forum](https://feedback.azure.com/d365community/forum/04fe6ee0-3b25-ec11-b6e6-000d3a4f0da0) or connect with your Microsoft representative.

_Last updated: Aug 14, 2025. Version 1.0_

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/arc-enabled-sql-server-is-now-generally-available-in-the-us/ba-p/4443077)
