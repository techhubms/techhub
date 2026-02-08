---
external_url: https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/serverless-workspaces-are-generally-available-in-azure/ba-p/4491314
title: Serverless Workspaces Now Generally Available in Azure Databricks
author: AnaviNahar
primary_section: ml
feed_name: Microsoft Tech Community
date: 2026-02-02 17:15:00 +00:00
tags:
- Azure
- Azure Databricks
- Classic Workspaces
- Cluster Management
- Community
- Data Analytics
- Data Engineering
- Identity Federation
- Managed Storage
- ML
- Network Configuration
- Object Storage
- Resource Optimization
- Serverless Compute
- Serverless Workspaces
- Unity Catalog
- Workspace Governance
section_names:
- azure
- ml
---
AnaviNahar introduces the general availability of Serverless Workspaces in Azure Databricks, detailing their architecture and guidance for when to choose Serverless or Classic models.<!--excerpt_end-->

# Serverless Workspaces Now Generally Available in Azure Databricks

Azure Databricks has announced the general availability of Serverless Workspaces, expanding from the previous public preview. Organizations can now choose between **Serverless** and **Classic** workspace models:

## Serverless Workspace Highlights

- **Managed Infrastructure**: Azure Databricks handles all environment setup and management. There's no need to configure networking, compute clusters, or storage manually.
- **Governance**: While Databricks manages the environment, you still define governance with Unity Catalog, identity federation, and workspace-level policies.
- **Instant Readiness**: Serverless workspaces are ready to use immediately upon creation, helping teams start data and analytics projects without traditional setup delays.

### Key Capabilities

- **Storage**: Each workspace offers fully managed object storage ("default storage"). Users can build managed catalogs, volumes, and tables without provisioning storage accounts or credentials. Security features restrict access to authorized users, and Classic compute cannot access default storage assets.
- **External Storage Support**: Organizations can connect existing Azure Blob Storage accounts to Serverless Workspaces, supporting scenarios with security or compliance needs.
- **Serverless Compute**: Workloads run on auto-provisioned compute; scaling and resource optimization are automatic.
- **Simplified Networking**: Serverless removes requirements for NAT gateways, firewalls, or Private Link endpoints. Serverless egress and Private Link controls are managed at the workspace level.
- **Unified Governance**: Unity Catalog is provisioned automatically, preserving consistent access control and data security.

## Classic Workspace Model

- **Custom Control**: Classic workspaces require manual setup of networking, compute, and storage, suitable for highly regulated or specialized environments.
- **Fine-Grained Security**: Ideal when precise network topologies or regulatory requirements demand granular configuration.

## Choosing the Right Workspace Type

- **Serverless**: Use for rapid creation, minimal configuration, and reduced operational overhead—a practical default for most scenarios.
- **Classic**: Use when custom network/security configurations or features not yet available in Serverless are required.

> _Serverless workspaces are available only in regions supporting serverless compute. See the [supported regions documentation](https://learn.microsoft.com/en-us/azure/databricks/resources/supported-regions) for details._

### Further Resources

- [Create a Serverless Workspace in Azure Databricks](https://learn.microsoft.com/en-us/azure/databricks/admin/workspace/serverless-workspaces)
- [Connecting Azure Storage to Serverless Workspace](https://learn.microsoft.com/en-us/azure/databricks/connect/unity-catalog/cloud-storage/)

---

**Author:** AnaviNahar  
Published: Feb 01, 2026

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/serverless-workspaces-are-generally-available-in-azure/ba-p/4491314)
