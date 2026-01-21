---
external_url: https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/secure-medallion-architecture-pattern-on-azure-databricks-part-i/ba-p/4459268
title: Secure Medallion Architecture Pattern on Azure Databricks (Part I)
author: mscagliola
feed_name: Microsoft Tech Community
date: 2025-10-07 06:25:50 +00:00
tags:
- ADLS Gen2
- Azure Databricks
- Azure Key Vault
- Cluster Policies
- Data Governance
- Delta Lake
- Lakeflow
- Lakehouse
- Least Privilege
- Managed Identities
- Medallion Architecture
- Microsoft Entra ID
- Operational Security
- System Tables
- Unity Catalog
section_names:
- azure
- ml
- security
---
mscagliola presents a practical guide for deploying a secure Medallion Architecture on Azure Databricks, detailing how to implement least-privilege access, strong data governance, and robust pipeline isolation using Microsoft technologies.<!--excerpt_end-->

# Secure Medallion Architecture Pattern on Azure Databricks (Part I)

*Author: mscagliola*

> Disclaimer: The views in this article are my own and do not represent Microsoft or Databricks.

## Overview

This article walks through a secure deployment pattern for data platforms on Azure Databricks, leveraging the Medallion Architecture and Lakehouse principles. The focus is on achieving robust data governance, pipeline isolation, and operational control using Azure-native identity and security services.

## Key Components

- **Azure Databricks**: Core compute and orchestration engine for analytics workloads.
- **Medallion Architecture**: Data flows through Bronze (raw), Silver (refined), and Gold (curated) layers, each with clear responsibilities and security boundaries.
- **Microsoft Entra ID (formerly Azure AD)**: Manages service principals and managed identities for authentication and authorisation.
- **Azure Data Lake Storage Gen2 (ADLS Gen2)**: Stores data securely at each Medallion layer.
- **Unity Catalog**: Centralised governance for data discovery, access control, and cataloging in the Lakehouse.
- **Azure Key Vault**: Securely manages and rotates secrets, credentials, and sensitive parameters.
- **Lakeflow Jobs**: Each Medallion layer gets its own Lakeflow job, running under a distinct identity and compute cluster.

## Why Medallion Layer Isolation Matters

Layering data (Bronze, Silver, Gold) helps maintain quality and compliance. By isolating identities, storage, and compute resources for each layer, you minimize the blast radius of compromise and ensure only authorised jobs can manipulate targeted data assets. Managed identities handled by Microsoft Entra ID enable automation without embedding credentials.

## Implementation Highlights

### 1. Storage and Identity Segregation

- **Each data layer** (Bronze, Silver, Gold) has its own storage account and managed identity/service principal.
- These service principals are tightly scoped to only the permissions necessary for each layer (least privilege).
- Unity Catalog external locations are configured per layer for isolation.

### 2. Compute Isolation

- Each layer uses a dedicated Databricks cluster tailored for its workload (raw ingest, transformation, or analytics).
- Cluster policies are built for cost control, security, and workload-specific tuning.

### 3. Secrets Management

- Secrets (API keys, passwords) are stored in Azure Key Vault.
- Databricks secret scopes integrate with Key Vault for secure, runtime-only access.
- Enforce rotation and restrict scope to only required principals.

### 4. Orchestrated Lakeflow Jobs

- Three Lakeflow jobs (one per layer) are orchestrated by a parent Lakeflow.
- Each job runs under its own managed identity, ensuring operational continuity and auditability.
- Access to notebooks and data is scoped using Unity Catalog and Databricks workspace permissions.

### 5. Governance and Observability

- Enable system tables for detailed monitoring of job runs, failures, duration, and cost by Medallion layer.
- Use Jobs Monitoring UI for operational triage and alerting.
- Track reliability and enforce usage policies per environment.

### 6. Best Practices (From Article)

- Never use broad/human credentials for production jobs.
- Separate catalogs and external locations for each data layer in Unity Catalog.
- Use managed tables unless direct external file control is required.
- Keep secrets out of code and parameters.
- Regularly audit and rotate secrets using AKV logging.
- Apply auto-scaling and auto-termination policies on clusters.
- Validate secret and identity access in non-production first.

## Example Provisioning Steps

1. **Create storage accounts** for Bronze, Silver, Gold layers in ADLS Gen2.
2. **Register managed identities/service principals** for each layer in Microsoft Entra ID.
3. In Unity Catalog, map each external location to its respective storage and identity.
4. Provision dedicated Databricks clusters for each stage and create cluster policies.
5. Configure Azure Key Vault and establish Databricks secret scopes.
6. Build Lakeflow jobs per layer and an orchestrator job; assign the correct run/manage permissions.

## Table Management

- Prefer managed tables under Unity Catalog for operational simplicity and built-in optimizations.
- Use external tables only when data lifecycle and ownership must remain outside Databricks management.
- Obfuscated storage paths enhance security by reducing the risk of direct identification.

## Monitoring and Cost Control

- Enable Jobs system tables and billing tables for visibility.
- Set up notification and alerting to respond to pipeline issues or cost overruns promptly.

## Conclusion and Next Steps

This secure architecture pattern ensures each part of your analytics pipeline operates under minimum necessary permissions, with clear boundaries and full auditability. In Part II, practical CI/CD code examples for automating this setup and handling further operational challenges will be provided.

---

*For more details or questions, contact the author via Microsoft Tech Community profile.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/secure-medallion-architecture-pattern-on-azure-databricks-part-i/ba-p/4459268)
