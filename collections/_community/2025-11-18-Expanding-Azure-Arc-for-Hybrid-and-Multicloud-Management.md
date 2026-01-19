---
external_url: https://techcommunity.microsoft.com/t5/azure-arc-blog/expanding-azure-arc-for-hybrid-and-multicloud-management/ba-p/4470656
title: Expanding Azure Arc for Hybrid and Multicloud Management
author: SatyaVel
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-11-18 16:26:46 +00:00
tags:
- Agentless Inventory
- AWS
- Azure Arc
- Azure Key Vault
- Azure Migrate
- Azure Policy
- Azure Virtual Desktop
- Compliance
- Connected Machine Agent
- Disaster Recovery
- Google Cloud Platform
- Hybrid Cloud
- Kubernetes
- Microsoft Entra
- Multicloud
- OIDC Federation
- Operational Consistency
- OS Configuration Editor
- Site Management
- Windows Recovery Environment
- Workload Identity
section_names:
- azure
- devops
- security
---
SatyaVel showcases Ignite 2025’s Azure Arc enhancements, covering new multicloud management, hybrid VDI, compliance, and security features, offering practical insights for professionals managing diverse IT estates.<!--excerpt_end-->

# Expanding Azure Arc for Hybrid and Multicloud Management

Azure Arc has evolved to become an essential platform for managing hybrid and multicloud environments. At Ignite 2025, several new features and improvements were announced to empower IT teams and cloud architects:

## What’s New in Azure Arc

### 1. Multicloud Connector for GCP (Public Preview)

- **Agentless Inventory:** Automatically discovers GCP resources and presents them within the Azure Resource Graph for unified visibility.
- **Arc Onboarding for GCP VMs:** Allows monitoring and policy management of GCP virtual machines from Azure.
- **Secure Authentication (OIDC Federation):** Enables access management without persistent credentials, enhancing security.

[Learn more](https://aka.ms/multicloud-connector-gcp-blog)

### 2. Azure Virtual Desktop for Hybrid Environments

- Supports local, on-premises VDI via Azure Arc without new hardware or hypervisors.
- Addresses latency, data residency, and application constraints while utilizing Azure management for operations.

[Learn more](https://aka.ms/AVDHybridIgnite2025Blog)

### 3. Azure Arc Auto-Agent Upgrades (Public Preview)

- **Automatic Upgrades:** Keeps agents updated automatically.
- **Flexible Controls:** Configurable via Portal, CLI, or PowerShell.
- **Built-In Resilience:** Supports rollback and retry for failed upgrades.

[Details](https://techcommunity.microsoft.com/blog/azurearcblog/public-preview-auto-agent-upgrade-for-azure-arc-enabled-servers/4442556)

### 4. OS Configuration Editor & Policy

- **Visual Authoring:** No-code interface for OS guest configuration policies.
- **Fleet Auditing:** Apply and audit settings across Azure and Arc-enabled servers.
- **Integrated Governance:** Direct tie-in with Azure Policy for consistent enforcement.

[Learn more](https://aka.ms/MCBaselinesPreviewBlog)

### 5. Windows Server Recovery Configuration Audit (Resiliency Initiative)

- **WinRE Audits:** Validate readiness for disaster recovery.
- **Compliance Reporting:** Dashboard views for recovery status and gaps.
- **Roadmap:** Planned auto-updates and remote remediation.

[Info](https://techcommunity.microsoft.com/blog/azurearcblog/public-preview-audit-and-enable-windows-recovery-environment-winre-for-azure-arc/4462939)

### 6. Workload Identity for Arc-enabled Kubernetes (General Availability)

- **Federated Identity with Microsoft Entra:** Removes secret sprawl and manual credential management.
- **Token Authentication:** Secure access to Azure resources from pods.
- **Supports Multiple Kubernetes Distros:** Centralizes identity management for hybrid/edge applications.

[Learn more](https://aka.ms/workload-identity-arc-for-kubernetes-ga)

### 7. Azure Arc Site Manager (Public Preview Refresh)

- **Hierarchical Organization:** Structure sites to mirror enterprise organization.
- **Aggregated Monitoring:** Consolidated dashboard for connectivity, updates, alerts, and security.
- **Reusable Configurations:** Blend policy and partner solutions efficiently.

[More info](https://aka.ms/SiteManagerGA)

### 8. Azure Migrate Integration for Arc Customers

- **Automatic Migration Assessments:** Use Arc data to build a migration business case and readiness assessment.
- **Comprehensive Analytics:** Evaluate TCO, sustainability, and workload fit for Azure resources.

[Details](https://aka.ms/arc2azure-preview-blog)

### 9. Azure Key Vault Secret Store Extension (General Availability)

- **Offline Secret Access for Kubernetes:** On-prem clusters maintain access to secrets even during connectivity interruptions.
- **High-Scale Support:** SSE aids distributed deployments with thousands of clusters.

[Learn more](https://learn.microsoft.com/azure/azure-arc/kubernetes/secret-store-extension)

---

These innovations further empower organizations to manage distributed environments securely, efficiently, and at scale—whether in cloud, on-premises, or at the edge. For detailed guides on each feature, check out the provided Microsoft links.

*Authored by SatyaVel for the Tech Hub community.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/expanding-azure-arc-for-hybrid-and-multicloud-management/ba-p/4470656)
