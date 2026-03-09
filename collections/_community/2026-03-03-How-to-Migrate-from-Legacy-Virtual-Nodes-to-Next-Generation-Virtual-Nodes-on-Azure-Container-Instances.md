---
layout: "post"
title: "How to Migrate from Legacy Virtual Nodes to Next-Generation Virtual Nodes on Azure Container Instances"
description: "This comprehensive guide by adamsharif-msft explains the process of migrating an Azure Kubernetes Service (AKS) cluster from the legacy Virtual Nodes managed add-on to the new generation of Virtual Nodes on Azure Container Instances (ACI) managed with Helm. The article discusses new feature additions, limitations, and provides step-by-step migration instructions, including required Azure CLI commands, Helm chart installation, and key troubleshooting steps for successful deployment and validation of the new Virtual Nodes architecture."
author: "adamsharif-msft"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/migrating-to-the-next-generation-of-virtual-nodes-on-azure/ba-p/4496565"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-03-03 09:13:14 +00:00
permalink: "/2026-03-03-How-to-Migrate-from-Legacy-Virtual-Nodes-to-Next-Generation-Virtual-Nodes-on-Azure-Container-Instances.html"
categories: ["Azure", "Coding", "DevOps"]
tags: ["ACI", "AKS", "Application Modernization", "Azure", "Azure CLI", "Azure Container Instances", "Bash", "Cloud Native", "Cluster Migration", "Coding", "Community", "Container Deployment", "Container Networking", "DevOps", "Helm", "Kubernetes", "Managed Identity", "Microservices", "Network Security Groups", "NodeSelector", "Persistent Volumes", "Pod Scheduling", "Troubleshooting", "Virtual Nodes", "VNet Peering"]
tags_normalized: ["aci", "aks", "application modernization", "azure", "azure cli", "azure container instances", "bash", "cloud native", "cluster migration", "coding", "community", "container deployment", "container networking", "devops", "helm", "kubernetes", "managed identity", "microservices", "network security groups", "nodeselector", "persistent volumes", "pod scheduling", "troubleshooting", "virtual nodes", "vnet peering"]
---

adamsharif-msft provides an in-depth walkthrough on migrating from the legacy AKS Virtual Nodes add-on to the next-generation Virtual Nodes on Azure Container Instances, highlighting new features, requirements, and stepwise implementation tips.<!--excerpt_end-->

# Migrating to the Next Generation of Virtual Nodes on Azure Container Instances (ACI)

**Author:** adamsharif-msft  

## Overview

Azure Kubernetes Service (AKS) can burst pods onto Azure Container Instances (ACI) using Virtual Nodes, allowing on-demand scaling without managing extra virtual machine nodes. This guide covers how to migrate an AKS cluster from the legacy Virtual Nodes managed add-on to the next-generation Virtual Nodes on ACI, now deployed and managed via Helm charts.

### Key Topics Covered

- Comparison: legacy Virtual Nodes add-on vs. new Virtual Nodes on ACI (Helm-based)
- Migration plan—step-by-step instructions
- Overview of new features and limitations
- Sample code and CLI commands
- Troubleshooting tips

## 1. Recap: Virtual Nodes/Add-On on AKS

Virtual Nodes enables AKS to scale workloads onto ACI on demand, eliminating the need to scale VM node pools.

## 2. New Generation: Virtual Nodes on ACI

- **Rebuilt platform**: Significant enhancements and fixes
- **Managed with Helm**: Not a native AKS managed add-on anymore

### New Features

- VNet peering with NSG support
- Init containers, host aliases
- Args for exec in ACI; container hooks
- Persistent Volumes & Persistent Volume Claims
- Confidential containers (see supported regions)
- [ACI standby pools](https://github.com/microsoft/virtualnodesOnAzureContainerInstances/blob/main/Docs/NodeCustomizations.md#standby-pools)

### Planned Enhancements

- ACR image pull via SPN
- Kubernetes network policies, IPv6, Windows containers
- Port forwarding

### Limitations

- No DaemonSet support
- Requires AKS with Azure CNI (not Kubenet)
- Incompatible with AKS API server authorized IP range restrictions

### Requirements

- Each deployment needs 3 vCPUs/12 GiB RAM on an AKS VM
- Supports up to 200 pods per deployment

## 3. Deploying the Legacy (Managed Add-on)

1. **Environment setup** (Bash via WSL + Azure CLI)
2. **Resource & network creation:**
    - Setup variables and resource groups
    - Create VNet and subnets
3. **Cluster and node creation:**
    - Deploy AKS cluster
    - Enable virtual-node add-on
4. **Assign permissions: **
    - Set up managed identities for network operations
5. **Validation:**
    - Check cluster with `kubectl get node`

**Note:** Detailed CLI commands are provided for each step.

## 4. Migration: Legacy Add-on to New Virtual Nodes on ACI

Direct upgrade is unsupported; you must uninstall old add-ons/resources and then deploy the new solution.

### Migration Steps

1. **Scale down workloads** (`kubectl delete deploy ...`)
2. **Disable the legacy add-on:** `az aks disable-addons...`
3. **Export subnet config and delete old subnet**
4. **Create new subnet (with required name/config)**
5. **Assign correct roles for cluster’s kubelet identity** (Contributor to resource group, Network Contributor to subnet)
6. **Pull latest kubeconfig**
7. **Clone and install Helm chart**
    - `git clone https://github.com/microsoft/virtualnodesOnAzureContainerInstances.git`
    - `helm install <release> /Helm/virtualnode`
8. **Validate readiness with `kubectl get node`**
9. **Remove old Virtual Node from cluster**
10. **Test pod scheduling**
    - Provide sample manifest/spec for new pod scheduling

## 5. Modifying Deployments

- Update `nodeSelector` and `tolerations` according to new Virtual Nodes requirements
- Example YAML blocks provided for both old and new node definitions in your pod specs

## 6. Troubleshooting

- Check running status of pods in the `vn2` namespace
- Common symptoms: Pending or crashing pods
    - Use `kubectl describe pod` and check logs (especially `proxycri`)
- Validate managed identity role assignments and capacity
- Reference [official troubleshooting docs](https://github.com/microsoft/virtualnodesOnAzureContainerInstances/blob/main/Docs/Troubleshooting.md) for more help

## 7. Support and Documentation

- Raise GitHub issues: [microsoft/virtualnodesOnAzureContainerInstances/issues](https://github.com/microsoft/virtualnodesOnAzureContainerInstances/issues)
- Official Microsoft and community docs linked throughout

---

**References & Further Information:**

- [Virtual Nodes for AKS Overview](https://learn.microsoft.com/en-us/azure/container-instances/container-instances-virtual-nodes)
- [GitHub Repo](https://github.com/microsoft/VirtualNodesOnAzureContainerInstances)
- [Node Customizations Docs](https://github.com/microsoft/virtualnodesOnAzureContainerInstances/blob/main/Docs/NodeCustomizations.md)

---

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/migrating-to-the-next-generation-of-virtual-nodes-on-azure/ba-p/4496565)
