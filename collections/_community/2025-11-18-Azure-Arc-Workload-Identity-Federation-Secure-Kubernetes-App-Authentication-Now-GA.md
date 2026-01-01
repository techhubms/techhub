---
layout: "post"
title: "Azure Arc Workload Identity Federation: Secure Kubernetes App Authentication Now GA"
description: "This announcement details the general availability of Workload Identity support for Azure Arc-enabled Kubernetes clusters, enabling secure identity federation for workloads running outside Azure. Applications can now use OpenID Connect (OIDC) with Microsoft Entra ID to access Azure services without managing secrets, reducing operational overhead and enhancing security. The guide covers setup steps, supported Kubernetes platforms, and compliance benefits."
author: "Poornima99"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-arc-blog/workload-identity-support-for-azure-arc-enabled-kubernetes/ba-p/4467851"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-18 16:00:00 +00:00
permalink: "/2025-11-18-Azure-Arc-Workload-Identity-Federation-Secure-Kubernetes-App-Authentication-Now-GA.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Azure Arc", "Azure CLI", "Azure Key Vault", "Azure Storage", "Community", "Edge Computing", "Hybrid Cloud", "Identity Federation", "Kubernetes", "Managed Identity", "Microsoft Entra ID", "OIDC", "Rancher K3s", "Red Hat OpenShift", "Security", "Service Account", "Token Federation", "VMware Tanzu", "Workload Identity"]
tags_normalized: ["azure", "azure arc", "azure cli", "azure key vault", "azure storage", "community", "edge computing", "hybrid cloud", "identity federation", "kubernetes", "managed identity", "microsoft entra id", "oidc", "rancher k3s", "red hat openshift", "security", "service account", "token federation", "vmware tanzu", "workload identity"]
---

Poornima99 announces that Workload Identity support for Azure Arc-enabled Kubernetes clusters is now generally available, providing organizations with secure, secretless authentication to Azure resources.<!--excerpt_end-->

# Azure Arc Workload Identity Federation: Secure Kubernetes App Authentication Now Generally Available

Workload Identity support for Azure Arc-enabled Kubernetes is now generally available (GA). This milestone brings a more secure, secretless way for workloads running on non-Azure clusters (connected by Azure Arc) to authenticate to Azure services, including Event Hubs, Azure Key Vault, and Azure Storage, all while eliminating manual credential management.

## Key Benefits

- **Secure authentication via OpenID Connect (OIDC):** Workloads can obtain Azure tokens using federated identity, removing the need for static credentials.
- **Reduced operational overhead:** No more secret rotation or manual certificate management.
- **Compliance and governance:** Trusted integration with Microsoft Entra ID and service account mapping ensures better oversight.

## How It Works

Azure Arc workload identity federation utilizes Kubernetes best practices:

1. **Enable OIDC Issuer & Workload Identity on Arc-connected cluster:**

   ```sh
   az connectedk8s connect --name "${CLUSTER_NAME}" --resource-group "${RESOURCE_GROUP}" --enable-oidc-issuer –-enable-workload-identity
   ```

2. **Configure user-assigned managed identity in Azure:** Link the Azure identity with your cluster's OIDC issuer by creating federated identity credentials.
3. **Pod-level identity integration:** Annotated Kubernetes service accounts allow apps running in pods to fetch Azure tokens via Microsoft Entra ID, enabling access to Azure services based on configured permissions.

## Supported Kubernetes Platforms

- AKS-Arc
- Red Hat OpenShift
- Rancher K3s
- VMware Tanzu Kubernetes Grid (TKGm)

This lets you federate identity for workloads in hybrid, on-prem, or edge locations including retail stores, manufacturing facilities, or remote sites.

## Getting Started

A step-by-step guide is available to help you deploy and configure workload identity federation on Azure Arc-enabled Kubernetes:

- [Deploying and Configuring Workload Identity Federation in Azure Arc-enabled Kubernetes](https://learn.microsoft.com/en-us/azure/azure-arc/kubernetes/workload-identity)

## Additional Resources

- Azure Arc documentation
- Security best practices for Kubernetes and Azure Resource authentication
- Microsoft Entra Workload ID federation overview

---
Author: Poornima99  
Microsoft Tech Community Contributor

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/workload-identity-support-for-azure-arc-enabled-kubernetes/ba-p/4467851)
