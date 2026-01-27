---
external_url: https://techcommunity.microsoft.com/t5/azure-sql-blog/higher-log-rate-for-business-critical-service-tier-in-azure-sql/ba-p/4444127
title: Higher Log Rate Enhancement in Azure SQL Managed Instance's Business Critical Tier
author: UrosMilanovic
feed_name: Microsoft Tech Community
date: 2025-08-15 12:49:40 +00:00
tags:
- Azure SQL
- Azure SQL Managed Instance
- Business Critical Tier
- Cloud Database
- Data Recovery
- Database Throughput
- Log Rate
- Managed Database
- Microsoft Azure
- Performance Optimization
- Premium Series Hardware
- Scalability
- Service Tier Improvement
- Transaction Processing
- Vcore Scaling
section_names:
- azure
primary_section: azure
---
UrosMilanovic provides an in-depth overview of a significant enhancement to Azure SQL Managed Instance: the Business Critical service tier on premium series hardware now features a higher transaction log rate, enabling improved performance for demanding workloads.<!--excerpt_end-->

# Higher Log Rate Enhancement in Azure SQL Managed Instance's Business Critical Tier

Azure SQL Managed Instance has introduced a critical update for its Business Critical service tier on premium series hardware. Customers using this configuration now benefit from significantly increased transaction log rates, leading to faster data processing, enhanced data recovery, and expanded scalability for mission-critical workloads—all at no extra cost.

## Enhanced Log Rate Performance

- **August 2023 Update:** The maximum log rate for Business Critical instances on premium series hardware was previously increased from 96 MiB/s to 192 MiB/s.
- **June 2025 Enhancement:** The per-vCore log rate has now been further increased from 4.5 MiB/s to 12 MiB/s, applying this benefit to instances running up to 20 vCores. The total per-instance limit remains 192 MiB/s.
- **Scaling Details:** Log rate is allocated per vCore, so transaction throughput scales up as you provision more vCores, up to the capped maximum.
- **Efficiency for Demanding Workloads:** These improvements allow high-throughput, mission-critical apps to achieve greater efficiency in cloud-based operations.

### Log Rate Comparison Table

| Log write throughput limit                | Before                                 | Now                                   |
|-------------------------------------------|----------------------------------------|----------------------------------------|
| Standard series                           | 4.5 MiB/s per vCore, up to 96 MiB/s    | Unchanged                             |
| Premium series                            | 4.5 MiB/s per vCore, up to 192 MiB/s   | 12 MiB/s per vCore, up to 192 MiB/s   |
| Premium series memory-optimized           | 4.5 MiB/s per vCore, up to 192 MiB/s   | 12 MiB/s per vCore, up to 192 MiB/s   |

## Why Does Log Rate Matter?

Transaction log throughput is a key determinant of a database's ability to handle concurrent operations and large workloads. Improved log rate means:

- Faster, more reliable transaction processing
- Quicker restore and recovery operations
- Stronger support for high-throughput, business-critical applications

## Azure SQL Managed Instance Service Tiers Overview

- **General Purpose:** Suited to most business workloads, balancing compute and storage needs. A Next-gen General Purpose version (currently in preview) offers even more performance at the same cost.
- **Business Critical:** Tailored for highest resilience, speed, and failover—now with substantially improved log throughput on premium series hardware.

## Take Action

- [Try out the Free SQL Managed Instance offer](https://techcommunity.microsoft.com/blog/azuresqlblog/free-sql-managed-instance-offer-is-now-generally-available/4415664)
- [Performance tuning guidance for Azure SQL Managed Instance](https://learn.microsoft.com/azure/azure-sql/managed-instance/performance-guidance?view=azuresql)
- [Azure SQL Managed Instance overview](https://learn.microsoft.com/en-us/azure/azure-sql/managed-instance/sql-managed-instance-paas-overview?view=azuresql)

## References & Further Reading

- [Original announcement: Doubling the max log rate](https://techcommunity.microsoft.com/blog/azuresqlblog/your-max-log-rate-on-sql-managed-instance-business-critical-is-now-doubled/3899817)
- [Introducing Azure SQL Managed Instance Next-gen GP](https://techcommunity.microsoft.com/blog/azuresqlblog/introducing-azure-sql-managed-instance-next-gen-gp/4092647)

---

*Article by UrosMilanovic, updated August 15, 2025.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-sql-blog/higher-log-rate-for-business-critical-service-tier-in-azure-sql/ba-p/4444127)
