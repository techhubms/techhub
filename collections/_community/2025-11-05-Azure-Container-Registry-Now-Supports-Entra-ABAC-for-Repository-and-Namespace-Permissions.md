---
layout: "post"
title: "Azure Container Registry Now Supports Entra ABAC for Repository and Namespace Permissions"
description: "This announcement details the general availability of Microsoft Entra attribute-based access control (ABAC) for Azure Container Registry (ACR). It explains how ABAC integrates with Azure RBAC to enable least-privilege access at the repository and namespace level, supporting multi-tenant engineering patterns and enhancing security for CI/CD and runtime scenarios. The post describes permission management, role configuration changes, supported identities, and how enterprises can adopt ABAC for granular access control."
author: "johshmsft"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-container-registry-repository-permissions-with-attribute/ba-p/4467182"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-05 00:31:02 +00:00
permalink: "/community/2025-11-05-Azure-Container-Registry-Now-Supports-Entra-ABAC-for-Repository-and-Namespace-Permissions.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["ABAC", "AKS", "Azure", "Azure Container Apps", "Azure Container Instances", "Azure Container Registry", "Azure RBAC", "CI/CD", "Community", "DevOps", "Kubernetes", "Least Privilege", "Microsoft Entra ID", "Multi Tenant", "Permission Management", "Registry Administration", "Role Assignments", "Security", "Zero Trust"]
tags_normalized: ["abac", "aks", "azure", "azure container apps", "azure container instances", "azure container registry", "azure rbac", "cislashcd", "community", "devops", "kubernetes", "least privilege", "microsoft entra id", "multi tenant", "permission management", "registry administration", "role assignments", "security", "zero trust"]
---

johshmsft outlines the new support for Microsoft Entra ABAC in Azure Container Registry, showing how teams can enforce least-privilege boundaries for repositories and namespaces with fine-grained security controls.<!--excerpt_end-->

# Azure Container Registry Repository Permissions with Attribute-based Access Control (ABAC)

## Overview

Microsoft has announced general availability for Attribute-Based Access Control (ABAC) support in Azure Container Registry (ACR), powered by Microsoft Entra ID. This addition allows organizations to enforce least-privilege permissions with fine granularity at the repository and namespace level. ABAC integrates with the existing Azure RBAC model, letting teams scope role assignments to specific repositories or entire logical namespaces—ideal for modern multi-tenant platforms where central registries serve many domains.

## Why ABAC Matters

Centralized container registries increasingly host artifacts for multiple business units. With ABAC:

- CI/CD pipelines can push images only to authorized namespaces and repositories.
- Consumers like Azure Kubernetes Service (AKS) clusters, Azure Container Apps (ACA), and Azure Container Instances (ACI) can pull images only from permitted locations.
- Explicit permission boundaries align with zero trust and supply chain hardening strategies using standard Microsoft Entra identities.

## Configuring ABAC in ACR

ACR registries now offer a "RBAC Registry + ABAC Repository Permissions" mode:

- Registry admins can add optional ABAC conditions during Azure RBAC assignments.
- Conditions scope permissions to specific repositories or namespace prefixes.
- ABAC is available for new and existing registries across all SKUs.

### How to Enable ABAC

- Set "RBAC Registry + ABAC Repository Permissions" during registry creation, or change in Portal Properties for existing registries.

## Built-in ABAC-Enabled Roles

Three new ABAC-scoped roles are available:

- **Container Registry Repository Reader**: Pull images, read metadata, resolve tags.
- **Container Registry Repository Writer**: Includes reader permissions plus image/tag push rights.
- **Container Registry Repository Contributor**: Reader and writer rights plus delete permissions.

> Note: Listing repositories requires "Container Registry Repository Catalog Lister", which does *not* support ABAC scoping—assigning it grants list permissions across the whole registry.

## Important Role Changes in ABAC Mode

In ABAC-enabled registries:

- **Legacy roles (AcrPull/AcrPush/AcrDelete)** are no longer honored. Use new ABAC-enabled roles instead.
- **Broad roles (Owner, Contributor, Reader)** now only grant control plane permissions; they do *not* allow image push/pull/delete or repository listing.
- Tasks and quick builds no longer inherit default data-plane permissions—explicit ABAC-based role assignments are mandatory.

## Role Assignment Scenarios

ABAC uses Microsoft Entra assignments for:

- Individual users, groups, service principals
- Managed identities for workloads (AKS, ACA, ACI kubelets)

## Next Steps

- Begin enforcing least-privilege boundaries in ACR registries using ABAC for CI/CD and runtime scenarios.
- ABAC is recommended for multi-tenant platforms and central registry deployments.
- For setup instructions, see [the official ACR ABAC documentation](https://aka.ms/acr/auth/abac)

## References

- [General ACR Permissions Docs](https://aka.ms/acr/auth/abac)

## Version

Published Nov 05, 2025
Version 1.0

---
*Author: johshmsft*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-container-registry-repository-permissions-with-attribute/ba-p/4467182)
