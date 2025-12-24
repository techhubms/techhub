---
layout: "post"
title: "Extended Support for Azure Database for MySQL: What You Need to Know"
description: "Microsoft has announced Extended Support for Azure Database for MySQL, a paid offering that allows customers to continue running workloads on older MySQL versions, such as 5.7 and 8.0, after their community end-of-life dates. This guide details the features, support timelines, pricing structure, and best practices for customers who wish to maintain operational continuity while planning their upgrade journey."
author: "Elendil"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-database-for-mysql-blog/announcing-extended-support-for-azure-database-for-mysql/ba-p/4442924"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-13 15:15:15 +00:00
permalink: "/community/2025-08-13-Extended-Support-for-Azure-Database-for-MySQL-What-You-Need-to-Know.html"
categories: ["Azure"]
tags: ["ARM Templates", "Azure", "Azure CLI", "Azure Database For MySQL", "Azure Flexible Server", "Cloud Database", "Community", "Community End Of Life", "Database Migration", "Enterprise SLA", "Extended Support", "Microsoft Learn", "MySQL 5.7", "MySQL 8.0", "Pay as You Go Pricing", "Security Updates", "Upgrade Best Practices"]
tags_normalized: ["arm templates", "azure", "azure cli", "azure database for mysql", "azure flexible server", "cloud database", "community", "community end of life", "database migration", "enterprise sla", "extended support", "microsoft learn", "mysql 5dot7", "mysql 8dot0", "pay as you go pricing", "security updates", "upgrade best practices"]
---

Elendil explains the new Extended Support program for Azure Database for MySQL, highlighting support timelines, automatic enrollment, and upgrade strategies for customers using older MySQL versions.<!--excerpt_end-->

# Extended Support for Azure Database for MySQL: What You Need to Know

**Author:** Elendil  
**Updated:** Aug 13, 2025  

Microsoft is introducing **Extended Support for Azure Database for MySQL** as a paid offering. This program is designed to help customers continue running workloads on MySQL versions like 5.7 and 8.0 in Azure, even after these versions reach their community end-of-life (EOL).

## Why Extended Support?

- Many customers have dependencies on MySQL 5.7 or 8.0 and cannot immediately upgrade to newer versions.
- Azure aims to provide extra time and flexibility while maintaining security, support, and reliability during the transition period.

## What's Included in Extended Support?

- **SLA-backed availability**: Enterprise-grade service level agreements continue to apply.
- **Security updates**: Ongoing critical security patches help protect workloads.
- **Technical support**: Azure engineers remain available for troubleshooting and operational issues.

### Seamless and Predictable Experience

- **Automatic enrollment**: Once a supported MySQL version hits its community EOL, Azure-managed instances enter Extended Support automatically—no action needed.
- **Grace period**: There's a one-month grace period after EOL before billing starts.
- **Pay-as-you-go pricing**: Billed per vCore per hour, with no upfront cost.
- **Exit on upgrade**: Billing for Extended Support stops as soon as a server is upgraded to a supported major version.

### Pricing & Availability

- Pricing details will be announced by Q3 2025 via the [Azure pricing calculator](https://azure.microsoft.com/en-us/pricing/calculator/).

## Support Timeline

| MySQL Version | Azure Standard Support Ends | Extended Support Starts | Extended Support Ends |
| ------------- | -------------------------- | ---------------------- | -------------------- |
| 5.7           | March 31, 2026             | April 1, 2026          | March 31, 2029       |
| 8.0           | May 31, 2026               | June 1, 2026           | March 31, 2029       |

All future Azure-hosted MySQL versions will follow this Extended Support model after their EOL. More details: [Version Support Policy | Microsoft Learn](https://learn.microsoft.com/en-us/azure/mysql/concepts-version-policy).

## Upgrade Paths for Customers

If you don't want Extended Support, upgrading is recommended. Two main upgrade options are available:

1. **In-place Upgrade on the Same Instance**
   - Supported on Azure Database for MySQL – Flexible Server.
   - Initiate upgrades from the Azure portal, CLI, or ARM templates.
   - Preserves server configurations and connection strings.
2. **Validate the Upgrade Before Production**
   - **Replica-Based Validation**: Create a read replica, upgrade, test, and promote when satisfied.
   - **PITR-Based Validation**: Use point-in-time restore to make a test server and validate before production rollout.

### Best Practices

- Always back up data before upgrades.
- Review MySQL release notes for breaking changes.
- Monitor applications after upgrade to catch issues early.

More info: [Major Version Upgrade - Azure Database for MySQL | Microsoft Learn](https://learn.microsoft.com/en-us/azure/mysql/flexible-server/how-to-upgrade)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-database-for-mysql-blog/announcing-extended-support-for-azure-database-for-mysql/ba-p/4442924)
