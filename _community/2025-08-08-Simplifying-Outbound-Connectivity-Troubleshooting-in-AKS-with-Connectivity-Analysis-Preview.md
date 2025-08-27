---
layout: "post"
title: "Simplifying Outbound Connectivity Troubleshooting in AKS with Connectivity Analysis (Preview)"
description: "This article introduces the Connectivity Analysis (Preview) feature for Azure Kubernetes Service (AKS), a new tool integrated into the Azure Portal to streamline troubleshooting of outbound connectivity issues. It explains how this feature analyzes Azure networking components, its current limitations, step-by-step usage, and real-world troubleshooting scenarios such as egress failures, image pulling issues, and external API connectivity challenges."
author: "juliayin"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/simplifying-outbound-connectivity-troubleshooting-in-aks-with/ba-p/4441200"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-08 18:14:42 +00:00
permalink: "/2025-08-08-Simplifying-Outbound-Connectivity-Troubleshooting-in-AKS-with-Connectivity-Analysis-Preview.html"
categories: ["Azure"]
tags: ["ACR", "AKS", "Azure", "Azure Networking", "Azure Portal", "Azure Virtual Network Verifier", "Cloud Native", "Cluster Operations", "CNI Overlay", "Community", "Connectivity Analysis", "Container Registry", "Containers", "EgressBlocked", "Firewall", "Kubernetes Networking", "MCR", "Network Security Group", "Node Pool", "NSG", "Outbound Connectivity", "Public Preview", "Troubleshooting", "UDR", "User Defined Routes", "VMSS"]
tags_normalized: ["acr", "aks", "azure", "azure networking", "azure portal", "azure virtual network verifier", "cloud native", "cluster operations", "cni overlay", "community", "connectivity analysis", "container registry", "containers", "egressblocked", "firewall", "kubernetes networking", "mcr", "network security group", "node pool", "nsg", "outbound connectivity", "public preview", "troubleshooting", "udr", "user defined routes", "vmss"]
---

Author juliayin presents an overview of the new Connectivity Analysis (Preview) tool for Azure Kubernetes Service, explaining how it simplifies network troubleshooting for AKS outbound connections.<!--excerpt_end-->

# Simplifying Outbound Connectivity Troubleshooting in AKS with Connectivity Analysis (Preview)

## Background

Troubleshooting outbound connectivity in Azure Kubernetes Service (AKS) is often complex due to Azure networking layers like Network Security Groups (NSGs), user defined routes (UDRs), firewalls, and private endpoints. If a pod cannot reach external services, identifying the root cause traditionally involves in-depth Azure networking knowledge and manual inspection of multiple resources, increasing time-to-resolution for platform teams.

## Introducing Connectivity Analysis (Preview) for AKS

Azure has announced the **Connectivity Analysis (Preview)** feature for AKS to address these challenges. Powered by the Azure Virtual Network Verifier (VNV) engine, this tool is purpose-built for AKS and integrated into the Azure Portal, giving users an efficient way to verify whether outbound traffic from AKS node pools is blocked by Azure resources such as:

- Azure Load Balancer
- Azure Firewall
- NAT Gateway
- Network Security Group (NSG)
- Network policies
- User defined routes (UDR)
- Virtual network peering

Unlike traditional diagnostics, Connectivity Analysis (Preview) doesn't generate test traffic; it analyzes your network resource configurations and rule sets to determine if anything would block traffic to the intended endpoint. **Note:** The tool analyzes only Azure internal resources. If blocked traffic isn't detected, you should additionally check external network elements outside Azure's control.

Currently, the feature supports only outbound connectivity from AKS node pools to public Internet endpoints. Expanded support for node pool → API server and in-cluster node-to-node connectivity is planned for future releases.

## How To Use the Connectivity Analysis (Preview) Feature

1. Navigate to your AKS cluster in the **Azure Portal**.
2. Under **Settings**, select the **Node pools** tab.
3. Pick your node pool and click **Connectivity Analysis (Preview)** in the toolbar (expand with "..." if necessary).
4. Select a Virtual Machine Scale Set (VMSS) instance as the source (IP pre-populated).
5. Choose or enter the destination endpoint (e.g., `mcr.microsoft.com`; IP pre-populated).
6. Run the analysis and wait (up to 2 minutes) for results. Visualize outcomes with the network flow diagram, and click "More details" to see comprehensive JSON diagnostics.

## Known Limitations

- CNI Overlay clusters are not yet supported in the following regions: West US, West US 2, South Central US. Full rollout is expected by early September.
- For unsupported regions, the tool will display an error if connectivity analysis is run from a CNI Overlay cluster.
- User interface improvements to display detailed results and guidance are in progress.

## Example Scenarios Where Connectivity Analysis (Preview) Helps

- **EgressBlocked or Node Provisioning Failures:** If your AKS nodes can't reach required Azure endpoints (i.e., for scaling or maintenance), this may result in EgressBlocked conditions and cluster failures. Use the tool to confirm node pool access to all essential endpoints.
- **Pod Image Pull Failures:** Deployments may fail if NSG or Firewall rules block outbound HTTPS to container registries (e.g., Azure Container Registry, Docker Hub, Microsoft Container Registry). Test connectivity from node subnet to these registries and pinpoint network components causing issues.
- **Unreachable Webhooks or External APIs:** When admission controllers or monitoring tools cannot reach external services (causing timeouts or errors), analyze which rules or routes are responsible for blocking traffic and validate access to FQDNs or IP addresses.

---

*Author: juliayin | Updated: Aug 08, 2025 | Version 1.0*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/simplifying-outbound-connectivity-troubleshooting-in-aks-with/ba-p/4441200)
