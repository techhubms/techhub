---
layout: "post"
title: "Higher Log Rate Announced for Azure SQL Managed Instance Business Critical Tier"
description: "This article details a major performance improvement for Azure SQL Managed Instance users running the business critical service tier on premium series hardware. The maximum log rate per vCore has been significantly increased, boosting transactional throughput, data recovery speeds, and scalability for mission-critical cloud workloads. The update benefits all global instances and is delivered at no additional cost."
author: "UrosMilanovic"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-sql-blog/higher-log-rate-for-business-critical-service-tier-in-azure-sql/ba-p/4444127"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-15 10:39:42 +00:00
permalink: "/2025-08-15-Higher-Log-Rate-Announced-for-Azure-SQL-Managed-Instance-Business-Critical-Tier.html"
categories: ["Azure"]
tags: ["Azure", "Azure SQL Managed Instance", "Business Critical Tier", "Cloud Databases", "Community", "Database Optimization", "Database Scalability", "Log Write Throughput", "Microsoft Azure", "Performance Tuning", "Premium Series Hardware", "SQL Managed Instance Service Tiers", "Transaction Log Rate", "Vcore Scaling"]
tags_normalized: ["azure", "azure sql managed instance", "business critical tier", "cloud databases", "community", "database optimization", "database scalability", "log write throughput", "microsoft azure", "performance tuning", "premium series hardware", "sql managed instance service tiers", "transaction log rate", "vcore scaling"]
---

UrosMilanovic explains how Azure SQL Managed Instance's business critical tier on premium series hardware now offers a much higher log rate, enhancing transaction processing, recovery, and scalability for demanding workloads.<!--excerpt_end-->

# Higher Log Rate Announced for Azure SQL Managed Instance Business Critical Tier

Azure SQL Managed Instance customers using the **business critical service tier** on **premium series hardware** will now benefit from a much higher log rate. This enhancement means improved transaction processing, faster data recovery, and expanded scalability—directly supporting mission-critical operations in the cloud. The change is available worldwide at no additional cost.

## Enhanced Log Rate Performance

- Maximum log rate per instance has doubled in previous releases, and now per-vCore log rate has increased from **4.5 MiB/s to 12 MiB/s** for premium series hardware (including memory-optimized variants), applicable up to 16–20 vCores (the per-instance cap remains 192 MiB/s).
- For standard series, the log rate remains unchanged.

| Log Write Throughput Limit           | Before                                    | Now                                    |
|-------------------------------------|-------------------------------------------|----------------------------------------|
| Standard series                     | 4.5 MiB/s per vCore, max 96 MiB/s         | Unchanged                              |
| Premium series                      | 4.5 MiB/s per vCore, max 192 MiB/s        | 12 MiB/s per vCore, max 192 MiB/s      |
| Premium series memory-optimized     | 4.5 MiB/s per vCore, max 192 MiB/s        | 12 MiB/s per vCore, max 192 MiB/s      |

This upgrade empowers high-throughput and mission-critical applications by supporting larger transactional volumes and more complex workloads within Azure SQL Managed Instance.

## What Is Log Rate and Why Is It Important?

The log rate, or transaction log throughput, controls how quickly your database handles inserts, updates, and deletes. A higher log rate results in:

- Faster, more reliable transactional processing
- Quicker database restore operations
- Better support for high-throughput, business-critical scenarios

## Azure SQL Managed Instance Service Tiers Overview

- **General Purpose**: Suits most workloads, offering balanced compute/storage. A Next-gen GP version (preview) promises even better performance at the same price point.
  - More: [Next-gen GP Overview](https://techcommunity.microsoft.com/blog/azuresqlblog/introducing-azure-sql-managed-instance-next-gen-gp/4092647)
- **Business Critical**: Designed for the highest resilience, rapid failover, and enhanced performance—now with much greater log throughput on premium series hardware.

## Learn More and Next Steps

- [Free Azure SQL Managed Instance offer](https://techcommunity.microsoft.com/blog/azuresqlblog/free-sql-managed-instance-offer-is-now-generally-available/4415664)
- [Performance tuning guidance for Azure SQL MI](https://learn.microsoft.com/azure/azure-sql/managed-instance/performance-guidance?view=azuresql)
- [What is Azure SQL Managed Instance?](https://learn.microsoft.com/en-us/azure/azure-sql/managed-instance/sql-managed-instance-paas-overview?view=azuresql)

Stay tuned for future updates to Azure SQL Managed Instance, and share your feedback with Microsoft to help shape the service to your needs.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-sql-blog/higher-log-rate-for-business-critical-service-tier-in-azure-sql/ba-p/4444127)
