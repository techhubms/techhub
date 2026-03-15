---
external_url: https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/azure-databricks-fabric-disaster-recovery-the-better-together/ba-p/4481323
title: Disaster Recovery Strategies for Azure Databricks and Microsoft Fabric
author: Rafia_Aqil
feed_name: Microsoft Tech Community
date: 2025-12-31 18:59:48 +00:00
tags:
- Azure Databricks
- Azure Managed Identity
- Azure Monitor
- Azure Storage
- Business Continuity
- CI/CD
- Data Synchronization
- Delta Lake
- Disaster Recovery
- IaC
- Microsoft Fabric
- Power BI
- Python
- Regional Redundancy
- Terraform
- Unity Catalog
- Azure
- DevOps
- ML
- Community
section_names:
- azure
- devops
- ml
primary_section: ml
---
Rafia Aqil and co-authors present a comprehensive technical guide to disaster recovery for Azure Databricks and Microsoft Fabric, focusing on automation, cloud resiliency, and best practices for DR in analytics platforms.<!--excerpt_end-->

# Disaster Recovery Strategies for Azure Databricks and Microsoft Fabric

**Authors:** Amudha Palani, Eric Kwashie, Peter Lo, and Rafia Aqil

Disaster recovery (DR) is essential for cloud-native analytics platforms to ensure business continuity during outages caused by natural disasters, infrastructure failures, or other interruptions. This guide details practical DR strategies for both Azure Databricks and Microsoft Fabric, outlining technical steps for maximizing resilience and minimizing downtime.

## Identify Business Critical Workloads

- **Classify workloads** by operational impact, data freshness, regulatory obligations, SLAs, and system dependencies.
- **Tier workloads** to determine DR investments; only business-critical workloads may require full regional redundancy.

## Azure Databricks Disaster Recovery

Azure Databricks relies on a customer-driven approach for DR. Organizations are responsible for replicating workspaces, data, infrastructure, and security controls across regions.

### Full System Failover (Active-Passive)

- **Replicate all dependent Azure services** (e.g., ADLS, Key Vault, SQL databases) using Terraform.
- **Deploy secondary network infrastructure** in the failover region.
- **Establish data synchronization** using Delta Lake "Deep Clone" and incremental replication jobs.
- **Synchronize workspace assets** (clusters, notebooks, jobs, permissions) via CI/CD and Terraform. Use SCIM for identity and access control.
- **Set job concurrency to zero** for secondary region assets to prevent unintended execution.

### Active-Active Strategy

- **Active-active designs** allow responsibilities and transactions to be processed in parallel, reducing downtime at increased cost and complexity.
- **Partial active-active** can be implemented to balance costs by running a subset of workloads in secondary region.

### Enabling Disaster Recovery

- Set up **secondary workspaces** in paired Azure regions.
- Use **CI/CD pipelines** to keep code (notebooks, libraries) and configurations synchronized.
- Replicate all services and secrets (Key Vault, Storage, SQL DB) via Terraform.
- Update orchestrators (e.g., ADF, Fabric pipelines) for quick region toggling during failover.
- Use **bi-directional sync** for Delta data with parallel workload processing, if required.

#### Three-Pillar DR Design

1. **Infrastructure Provisioning (Terraform):**
    - Deploy resource groups, Databricks workspaces, storage (ADLS Gen2), and monitoring resources to the secondary region with ARM/Terraform.
    - Use protection locks to prevent accidental resource deletion.
2. **Data Synchronization (Delta Notebooks):**
    - Schedule Databricks notebooks to clone and sync critical tables to secondary storage using managed identity access.
    - Synchronize entire table schemas, partitions, and maintain delta logs for point-in-time recovery.
3. **Failover Automation (Python Scripts):**
    - Use Python scripts to automate switching from primary to secondary workspaces during disruptions.

## Microsoft Fabric Disaster Recovery

Microsoft Fabric offers built-in DR tailored for analytics workloads (including Power BI):

- **Power BI** provides out-of-the-box DR:
  - Azure storage geo-redundancy and read-only access to models and dashboards during disasters.
- **Cross-region DR** uses a shared Microsoft-customer model:
  - Microsoft: maintains infra and pairing
  - Customer: enables DR in admin settings, configures secondary capacities/workspaces, and replicates data and configs
- **Enabling DR in Fabric**:
    1. Go to Admin portal → Capacity settings
    2. Select Fabric Capacity and enable Disaster Recovery
    3. Note: A 30-day minimum applies for the setting, with up to 72 hours for activation

## Azure Databricks vs. Microsoft Fabric DR: Procedures & Responsibilities

### Recovery Procedures

| Procedure                 | Databricks                                                | Fabric                                                 |
|---------------------------|-----------------------------------------------------------|--------------------------------------------------------|
| Failover                  | Customer stops workloads, updates routing, resumes in DR. | Microsoft initiates failover; customer restores DR.    |
| Restore to Primary        | Stop secondary, sync data/code, test, resume prod.        | Re-create & restore data in new capacity.              |
| Asset Syncing             | CI/CD & Terraform for clusters, jobs, notebooks, perms.   | Git/pipelines for code, manual for data in Lakehouse.  |

### Business Considerations

| Consideration             | Databricks                            | Fabric                                              |
|--------------------------|---------------------------------------|------------------------------------------------------|
| Control                  | Customers manage DR and failover      | Microsoft manages failover; customers restore        |
| Regional Dependencies    | Secondary region must be ready        | DR only in supported regions with pairings           |
| Power BI Continuity      | Not applicable                        | Built-in DR for semantic models, reports             |
| Activation Timeline      | Immediate after config                | Up to 72h, 30-day change wait                       |

## Key Takeaways

- Azure Databricks DR is flexible but requires technical ownership: automate infra/deployment, data replication (Delta), and failover.
- Microsoft Fabric offloads some DR tasks to Microsoft but still requires customer planning for deeper workloads.
- Use automation (Terraform, CI/CD, Python, notebooks) to orchestrate DR, synchronize assets, and minimize manual steps.

## References

- [Azure Databricks Documentation](https://docs.microsoft.com/azure/databricks/)
- [Microsoft Fabric Documentation](https://learn.microsoft.com/fabric/)
- [Delta Lake Documentation](https://docs.delta.io/)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/azure-databricks-fabric-disaster-recovery-the-better-together/ba-p/4481323)
