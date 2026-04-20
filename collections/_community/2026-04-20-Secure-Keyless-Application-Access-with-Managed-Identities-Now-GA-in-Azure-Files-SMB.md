---
feed_name: Microsoft Tech Community
primary_section: azure
external_url: https://techcommunity.microsoft.com/t5/azure-storage-blog/secure-keyless-application-access-with-managed-identities-now-ga/ba-p/4513053
section_names:
- azure
- security
date: 2026-04-20 17:57:56 +00:00
tags:
- AKS Workload Identity
- Azure
- Azure Files
- Azure Kubernetes Service (aks)
- Azure Portal
- Azure Storage Accounts
- CI/CD Pipelines
- Community
- Federated Identity Credentials
- HDD Shares
- Identity Based Access
- Keyless Authentication
- Kubernetes Pods
- Least Privilege
- Managed Identity
- Microsoft Entra ID
- RBAC
- Security
- SMB
- SSD Shares
- Storage Account Keys
- Zero Trust
title: Secure, Keyless Application Access with Managed Identities - Now GA in Azure Files SMB
author: Priyanka-Gangal
---

Priyanka-Gangal announces GA support for Managed Identity authentication to Azure Files over SMB, explaining how to replace storage account keys with Entra ID-based access and highlighting AKS Workload Identity, mixed app + end-user access, and simplified Azure portal enablement.<!--excerpt_end-->

## Summary

Managed Identity support for **Azure Files over SMB** is now **Generally Available (GA)**. The goal is to let applications and VMs access Azure Files **without secrets** (no storage account keys, passwords, or key distribution), improving auditability and reducing the operational overhead of rotating and protecting shared credentials.

## Why this matters

Traditional approaches (account keys, stored credentials, or domain-joined infrastructure) can introduce:

- Credential leakage risk
- Weak identity attribution (harder to know “who did what”)
- Excess privilege blast radius (shared keys)

The GA feature aligns better with **Zero Trust** and **least privilege** practices.

## What’s new in GA

### AKS Workload Identity support

The post calls out **AKS Workload Identity** (preview) as a way to move identity from the **node** to the **pod**:

- Each pod can use its own **federated identity**, mapped to a **Microsoft Entra ID** principal
- Enables:
  - Pod-level identity isolation (not just cluster-level)
  - Least-privilege access with RBAC
  - Scaling/redeployment without identity reconfiguration
  - No secrets, no key rotation, no credential injection

Combined with Azure Files over SMB, this supports per-pod, identity-driven access to shared file storage. The post mentions availability with **AKS 1.35** and highlights regulated scenarios (example: financial services) where isolation and auditability matter.

### Co-existence of application identities and end-user access

Azure Files can now support both:

- **Managed Identity** for applications
- **End-user access**

…on the same storage account, with authentication through **Entra ID** and authorization via a shared permissions model.

Example scenarios:

- Developers debugging using the same share as the application
- Admins managing content used by automated workflows
- Hybrid setups mixing user-driven and app-driven access

### Simplified enablement in the Azure portal

A dedicated **Managed Identity** property in the Azure portal is available for both new and existing storage accounts, intended to make it easier to enable identity-based SMB access and adopt incrementally while keeping existing governance/user access patterns.

## Get started

- The feature is stated to be available **at no additional cost**.
- Supported on **HDD and SSD SMB shares** across billing models.
- Setup guidance: https://go.microsoft.com/fwlink/?linkid=2338790

## Contact

Questions: [azurefiles@microsoft.com](mailto:azurefiles@microsoft.com)


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-storage-blog/secure-keyless-application-access-with-managed-identities-now-ga/ba-p/4513053)

