---
date: 2026-04-06 14:44:45 +00:00
section_names:
- azure
- devops
- security
tags:
- ARM Templates
- ARO
- Az Aro
- Azure
- Azure CLI
- Azure Key Vault
- Azure Portal
- Azure RBAC
- Azure Red Hat OpenShift
- Azure SQL Database
- Azure Storage
- Bicep
- Cluster Upgrade
- Community
- DevOps
- Kubernetes Service Account
- Least Privilege
- Managed Identities
- Microsoft Entra Workload ID
- OIDC Federation
- Security
- Service Principal
- Short Lived Credentials
- User Assigned Managed Identity
- Workload Identity
author: MelanieKraintz007
title: 'Azure Red Hat OpenShift: Managed Identity and Workload Identity now generally available'
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-red-hat-openshift-managed-identity-and-workload-identity/ba-p/4504940
primary_section: azure
feed_name: Microsoft Tech Community
---

MelanieKraintz007 announces GA support for managed identities and workload identity in Azure Red Hat OpenShift, explaining how ARO operators and Kubernetes workloads can use short-lived tokens with Azure RBAC to reduce reliance on long-lived service principals.<!--excerpt_end-->

# Azure Red Hat OpenShift: Managed Identity and Workload Identity now generally available

Azure Red Hat OpenShift (ARO) now generally supports **managed identities** and **workload identity**, enabling OpenShift clusters and apps on Azure to avoid **long-lived service principal credentials**.

## ARO, identity, and Azure governance

With GA support for managed identities and workload identities, ARO uses **short-lived credentials** and **least-privilege access** to improve security posture by reducing:

- reliance on long-lived credentials
- overly broad permissions

As an Azure-native service, ARO integrates with:

- **Microsoft Entra workload identities**: https://learn.microsoft.com/en-us/entra/workload-id/workload-identities-overview
- **Azure RBAC** (for scoping permissions)

## Platform identity: managed identities for ARO operators

At the platform layer, ARO now uses **multiple user-assigned managed identities** instead of a single broad-permission service principal.

- Each identity maps to a specific ARO component
- Each identity is associated with a **dedicated built-in ARO role**
- Permissions are scoped to align with **least privilege** and **Azure RBAC best practices**

Deployment options include:

- Pre-create identities and role assignments, then reference them during deployment
- Use the Azure portal “all-in-one” flow to create identities and assignments during cluster creation

Cluster deployment methods:

- Azure portal
- ARM/Bicep templates
- `az aro` CLI (requires Azure CLI **2.84.0+**)

More details:

- **Understand managed identities in Azure Red Hat OpenShift**: https://learn.microsoft.com/en-us/azure/openshift/howto-understand-managed-identities

## Application access: workload identity for Azure services

For applications running on ARO, **workload identity** lets pods get **short-lived tokens** for an Azure managed identity **without storing secrets in the cluster**.

How it works (as described):

- Use **Microsoft Entra workload identities** and **OIDC federation**
- Bind a **user-assigned managed identity** to a **Kubernetes service account**
- Workloads using that service account receive tokens for the associated identity at runtime

This enables granular access patterns, for example:

- a specific app gets **read-only** access to a single:
  - Key Vault
  - Storage account
  - Azure SQL database
- avoid sharing credentials across namespaces
- avoid a cluster-wide service principal

End-to-end guide:

- **Deploy and configure an application using workload identity on an Azure Red Hat OpenShift managed identity cluster**: https://learn.microsoft.com/en-us/azure/openshift/howto-deploy-configure-application

## Existing preview clusters and how to start

- Preview clusters deployed with managed identities automatically transition to GA and are supported for production.
- You can continue upgrades via standard OpenShift mechanisms; guidance:
  - https://learn.microsoft.com/en-us/azure/openshift/howto-upgrade-aro-openshift-cluster

Support status and migration note:

- Clusters using the existing service principal model remain supported.
- There is **not yet a migration path** from service principal to managed identity.
- To adopt managed identities, you deploy a new managed-identity-enabled ARO cluster and migrate workloads.

Recommended next steps:

1. Review concepts and considerations:
   - https://learn.microsoft.com/en-us/azure/openshift/howto-understand-managed-identities
2. Create a cluster using:
   - Azure portal
   - ARM/Bicep template
   - ARO CLI

## Resources

- **Interactive Demo - Create a managed identity Azure Red Hat OpenShift cluster**: https://interact.redhat.com/share/zMI66TmGGHDqWXMdo9Lk
- **Concepts / architecture**: https://learn.microsoft.com/en-us/azure/openshift/howto-understand-managed-identities
- **Cluster creation**: https://learn.microsoft.com/en-us/azure/openshift/howto-create-openshift-cluster
- **Applications / workload identity**: https://learn.microsoft.com/en-us/azure/openshift/howto-deploy-configure-application
- **Automation (ARM/Bicep)**: https://learn.microsoft.com/en-us/azure/openshift/quickstart-openshift-arm-bicep-template
- **Red Hat blog (joint story)**: https://www.redhat.com/en/blog/general-availability-managed-identity-and-workload-identity-microsoft-azure-red-hat-openshift


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-red-hat-openshift-managed-identity-and-workload-identity/ba-p/4504940)

