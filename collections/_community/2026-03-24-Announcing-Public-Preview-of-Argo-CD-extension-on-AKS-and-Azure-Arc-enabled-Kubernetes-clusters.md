---
primary_section: azure
date: 2026-03-24 08:00:00 +00:00
feed_name: Microsoft Tech Community
title: Announcing Public Preview of Argo CD extension on AKS and Azure Arc enabled Kubernetes clusters
tags:
- ACR
- AKS
- ApplicationSet
- Argo CD
- Automatic Patch Releases
- Azure
- Azure Arc
- Azure Arc Enabled Kubernetes
- Azure CLI
- Azure Container Registry
- Azure DevOps
- Azure Linux
- Azure Portal
- Community
- CVE Reduction
- DevOps
- GitOps
- HA
- High Availability
- Hub And Spoke Architecture
- Hybrid
- Kubernetes
- Microsoft Entra ID
- Multi Cluster Management
- Security
- Security Hardening
- Single Sign On
- SSO
- Workload Identity Federation
- Zero Trust
author: Poornima99
external_url: https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-public-preview-of-argo-cd-extension-on-aks-and-azure/ba-p/4504497
section_names:
- azure
- devops
- security
---

Poornima99 announces a public preview Argo CD extension for AKS and Azure Arc-enabled Kubernetes, highlighting GitOps enablement with Entra ID-based authentication, workload identity federation to ACR/Azure DevOps, security hardening via Azure Linux images and patching, and upstream Argo CD feature parity for HA and multi-cluster setups.<!--excerpt_end-->

# Announcing Public Preview of Argo CD extension on AKS and Azure Arc enabled Kubernetes clusters

Poornima99 announces a public preview Argo CD extension for AKS and Azure Arc-enabled Kubernetes, highlighting GitOps enablement with Entra ID-based authentication, workload identity federation to ACR/Azure DevOps, security hardening via Azure Linux images and patching, and upstream Argo CD feature parity for HA and multi-cluster setups.

## Overview

Microsoft has announced a **public preview** of the **Argo CD extension** for:

- **Azure Kubernetes Service (AKS)**
- **Azure Arc-enabled Kubernetes clusters**

The goal is to help enterprises adopt **GitOps** while staying aligned with security and identity best practices.

## Trusted identity and secure access

The extension integrates with **Microsoft Entra ID** to provide an enterprise identity experience, including:

- **Secure authentication using Workload Identity federation** to:
  - **Azure Container Registry (ACR)**
  - **Azure DevOps**
  
  This is positioned as a way to avoid **long-lived credentials** and **hard-coded secrets** in Git repositories, moving CD pipelines closer to a **zero-trust** approach.

- **Single Sign-On (SSO)** using existing Azure identities

## Enterprise-grade hardening and security

Security-related enhancements called out in the preview:

- Extension images are built on **Azure Linux**, intended to reduce the attack surface via:
  - **Reduced CVEs**
  - **Improved baseline security**

- An **opt-in** path to **automatic patch releases** to stay current on fixes, while keeping change management control

## Parity with upstream Argo CD

The extension aims to stay aligned with upstream Argo CD, with support for:

- **High availability (HA)** configuration for production workloads
- **Hub-and-spoke** architecture for multi-cluster GitOps
- Argo CD **Application** and **ApplicationSet** for scalable delivery across large cluster fleets

## Getting started

- Follow the Microsoft Learn tutorial to enable the extension using the **Azure CLI**:
  - https://learn.microsoft.com/en-us/azure/azure-arc/kubernetes/tutorial-use-gitops-argocd
- **Azure Portal** management is stated as “available in a few weeks.”

## Metadata from the source

- Updated: **Mar 22, 2026**
- Version: **1.0**


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-public-preview-of-argo-cd-extension-on-aks-and-azure/ba-p/4504497)

