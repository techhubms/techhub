---
layout: "post"
title: "Replicating VMware Workloads to Azure with Azure Site Recovery for Disaster Recovery"
description: "This community post by ADEDAYO1880 outlines requirements and asks for recommendations on replicating over 80 VMware-hosted workloads, including databases and application servers, to Azure using Azure Site Recovery (ASR). The post covers seamless failover, backup frequency, Entra ID integration, database replication, application server strategies, network security integration, cost optimization, and best practices for disaster recovery drills."
author: "ADEDAYO1880"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure/replicate-workload-from-vmware-to-azure-using-azure-site/m-p/4460851#M22268"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-11 18:46:30 +00:00
permalink: "/2025-10-11-Replicating-VMware-Workloads-to-Azure-with-Azure-Site-Recovery-for-Disaster-Recovery.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Azure Site Recovery", "Backup", "Community", "Cost Optimization", "Database Replication", "Disaster Recovery", "Failover", "Linux", "Microsoft Entra ID", "MySQL", "Network Security", "Non Destructive DR Drill", "Oracle", "Security", "SQL Server", "VMware", "Windows Server"]
tags_normalized: ["azure", "azure site recovery", "backup", "community", "cost optimization", "database replication", "disaster recovery", "failover", "linux", "microsoft entra id", "mysql", "network security", "non destructive dr drill", "oracle", "security", "sql server", "vmware", "windows server"]
---

ADEDAYO1880 seeks guidance on replicating VMware workloads to Azure using Azure Site Recovery, covering database replication, application server strategies, security integration, backup practices, and DR tests.<!--excerpt_end-->

# Replicating VMware Workloads to Azure Using Azure Site Recovery (ASR)

**Author:** ADEDAYO1880

## Overview

This discussion centers on replicating workloads hosted on VMware infrastructure to Azure using Azure Site Recovery (ASR) for disaster recovery purposes. It involves more than 80 VMs (both Linux and Windows) and several database platforms.

---

## Current Environment

- 80+ VMs hosted on VMware vSphere
- Operating Systems: Linux, Windows Server
- Databases: Oracle DB, Microsoft SQL Server, MySQL

---

## Requirements

- **Seamless failover and disaster recovery**
- **Scalable setup** to handle large workloads
- **Zero downtime** during replication and failover
- **Integration with Microsoft Entra ID** for identity and access management
- **Recovery Time Objective (RTO):** less than 2 hours
- **Recovery Point Objective (RPO):** greater than 15 minutes
- **Backup strategies**:
  - Critical databases: every 3 hours
  - Application servers: daily (incremental), weekly (full)
  - Transaction logs: every 10 minutes
  - Configuration backups: daily

---

## Key Questions & Discussion Points

1. **Database Replication (Oracle, SQL Server, MySQL):**
   - Seeks best method for seamless, low-downtime replication of heterogeneous databases to Azure.
   - [Official Azure Site Recovery Guide](https://learn.microsoft.com/en-us/azure/site-recovery/vmware-azure-failback)
2. **Application Server Replication Approach:**
   - Discussion of techniques/tools for replicating application servers.
3. **Security Tool Integration:**
   - Wants advice on integrating an existing on-premise third-party firewall, rather than relying solely on Azure-native security tools.
4. **Cost Optimization:**
   - Looking for techniques to minimize the cost of DR in Azure.
5. **Disaster Recovery Drills:**
   - Requests best practices for conducting non-disruptive DR tests.

---

## Additional Notes

- Identity and access management is to be tightly integrated with Microsoft Entra ID (formerly Azure AD).
- The environment emphasizes both operational resilience and technical efficiency.
- Cost and operational best practices are key considerations.

---

## Potential Implementation Insights (Community Input Needed)

- Evaluate Azure Site Recovery capabilities and compatibility with the described VM and database types.
- Consider native Azure backup solutions for databases alongside ASR.
- Explore options for firewall integration—possible use of third-party network virtual appliances in Azure.
- Assess automation options for DR drills (using ASR's test failover features).
- Use Azure Cost Management and reserved instances where feasible for optimization.

---

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure/replicate-workload-from-vmware-to-azure-using-azure-site/m-p/4460851#M22268)
