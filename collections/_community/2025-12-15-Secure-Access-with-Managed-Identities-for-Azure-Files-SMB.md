---
external_url: https://techcommunity.microsoft.com/t5/azure-storage-blog/secure-seamless-access-using-managed-identities-with-azure-files/ba-p/4477565
title: Secure Access with Managed Identities for Azure Files SMB
author: Priyanka-Gangal
feed_name: Microsoft Tech Community
date: 2025-12-15 17:46:43 +00:00
tags:
- AKS
- Azure DevOps
- Azure Files
- CI/CD
- Cloud Native
- FIPS Compliance
- Kerberos
- Linux Clients
- Managed Identities
- Microsoft Entra ID
- OAuth Tokens
- Persistent Storage
- RBAC
- Role Based Access Control
- SMB Shares
- Windows Clients
- Zero Trust
- Azure
- DevOps
- Security
- Community
section_names:
- azure
- devops
- security
primary_section: azure
---
Priyanka-Gangal explains how Managed Identities empower secure, credential-free access to Azure Files SMB for applications and DevOps scenarios, detailing benefits and practical implementation advice.<!--excerpt_end-->

# Secure, Seamless Access using Managed Identities with Azure Files SMB

Organizations operating in on-premises, hybrid, and cloud environments commonly face challenges around securely granting access to application storage. Traditional credential-based models using storage account keys are insufficient for Zero Trust architectures, exposing organizations to risks associated with static secrets.

**Managed Identities with Azure Files SMB (Public Preview)** introduces a secure, identity-driven model where applications and services authenticate with Azure Files using their own managed identities. This eliminates the need for passwords or keys and integrates directly with Microsoft Entra ID, leveraging built-in Azure RBAC for fine-grained control.

## Key Features & Benefits

- **Zero Trust Alignment:** Resource-specific identity, hourly OAuth token refresh, and end-to-end Azure identity management.
- **Role Based Access Control (RBAC):** Granular least-privilege enforcement using Azure RBAC.
- **Compliance:** FIPS-compliant, removing legacy NTLMv2 dependency.
- **Multi-Client Support:** SMB shares accessible from Windows and Linux clients.

## Real-World Application Scenarios

### Eliminating Secret Sprawl in CI/CD Workloads

- Azure Files SMB serves as a central repository for CI/CD artifacts including binaries and configuration files.
- Build agents in Azure DevOps or other systems authenticate using managed identities, governed by Microsoft Entra ID and Azure RBAC.
- Static credentials are eliminated, resulting in stronger security and streamlined compliance.

> “Managed Identities support with SMB shares will enable us to remove dependencies on storage account keys to run our CI/CD pipelines, enabling stronger security and alignment with Zero-Trust principles.” — Alex Garcia, Staff Dev Ops Engineer, Unity Technologies.

### Securing Persistent Storage in Azure Kubernetes Service (AKS)

- Stateful workloads in AKS depend on persistent volumes for application data.
- Managed Identities enable direct AKS cluster authentication to Azure Files SMB, eliminating stored keys or secrets within Kubernetes.
- Improves security posture and simplifies compliance, aligning with zero exception policies for credentials.

[Read more in Azure Files AKS CSI documentation](https://learn.microsoft.com/en-us/azure/aks/azure-files-csi#use-managed-identity-to-access-azure-files-storage--preview).

## Getting Started

- Managed Identities for Azure Files are available at no extra cost and supported across HDD and SSD SMB shares.
- Setup guidance and further documentation: [Managed Identities with Azure Files](https://go.microsoft.com/fwlink/?linkid=2338790).
- Applies to both new storage deployments or upgrades to existing environments.

For inquiries, contact: [azurefiles@microsoft.com](mailto:azurefiles@microsoft.com)

---

**Version:** 1.0
**Published:** Dec 15, 2025
**Author:** Priyanka-Gangal

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-storage-blog/secure-seamless-access-using-managed-identities-with-azure-files/ba-p/4477565)
