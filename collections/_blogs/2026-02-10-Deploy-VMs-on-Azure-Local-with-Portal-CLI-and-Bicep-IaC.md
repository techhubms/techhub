---
layout: "post"
title: "Deploy VMs on Azure Local with Portal, CLI & Bicep (IaC)"
description: "This guide by Thomas Maurer provides a practical walkthrough for deploying and managing virtual machines (VMs) on Azure Local. It covers three approaches: Azure Portal, Azure CLI, and Infrastructure-as-Code (IaC) with Bicep, highlighting how to enable consistent management and automation in hybrid and sovereign cloud scenarios using Azure Arc integration."
author: "Thomas Maurer"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.thomasmaurer.ch/2026/02/deploy-vms-on-azure-local-with-portal-cli-bicep-iac/"
viewing_mode: "external"
feed_name: "Thomas Maurer's Blog"
feed_url: "https://www.thomasmaurer.ch/feed/"
date: 2026-02-10 16:12:10 +00:00
permalink: "/2026-02-10-Deploy-VMs-on-Azure-Local-with-Portal-CLI-and-Bicep-IaC.html"
categories: ["Azure", "Coding", "DevOps"]
tags: ["ARM Templates", "Automation", "Azure", "Azure Arc", "Azure CLI", "Azure Local", "Azure Portal", "Bicep", "Blogs", "Cloud", "Cloud Native", "Coding", "DevOps", "Hybrid Cloud", "Hyper V", "IaC", "Microsoft", "Microsoft Azure", "RBAC", "Sovereign Clod", "Sovereign Cloud", "Virtual Machines", "Virtualization", "VM Deployment", "VM Management", "Windows Server"]
tags_normalized: ["arm templates", "automation", "azure", "azure arc", "azure cli", "azure local", "azure portal", "bicep", "blogs", "cloud", "cloud native", "coding", "devops", "hybrid cloud", "hyper v", "iac", "microsoft", "microsoft azure", "rbac", "sovereign clod", "sovereign cloud", "virtual machines", "virtualization", "vm deployment", "vm management", "windows server"]
---

Thomas Maurer guides readers through deploying VMs on Azure Local using the Azure Portal, CLI, and Bicep, showing practical workflows for hybrid and sovereign cloud environments.<!--excerpt_end-->

# Deploy VMs on Azure Local with Portal, CLI & Bicep (IaC)

*Author: Thomas Maurer*

If you're getting started with **Azure Local**, this guide will help you deploy and manage virtual machines (VMs) on-premises, bringing a cloud-native experience directly into your datacenter using the Azure control plane and Azure Arc integration.

## Why Use Azure Local for VMs?

Azure Local brings Azure’s unified management plane to customer-owned environments. It enables:

- **Centralized, RBAC-driven self-service management** for VMs, disks, logical networks, NICs, and images.
- Use of familiar Azure tools such as **Portal**, **CLI**, and **Infrastructure-as-Code (IaC) with Bicep/ARM**.
- Unified operations, policy, and monitoring across both Azure and on-premises environments using Azure Arc.
- Support for **sovereign/private cloud** needs and low-latency/regulatory workloads.
- Integration with Azure services like **Policy**, **Defender for Cloud**, and **Monitor**.

## What This Walkthrough Covers

1. **Creating a VM with the Azure Portal:**
   - Step-by-step GUI process for provisioning VMs in Azure Local.
2. **Deploying a VM with Azure CLI:**
   - Practical commands and options for automating VM lifecycle operations.
3. **Automating VM Creation with Bicep (IaC):**
   - Using Bicep templates to define and deploy infrastructure in a repeatable way.
4. **Basic VM Operations and Guest Management:**
   - Managing running VMs, modifying resources, and day-to-day operations.

## Key Concepts

- **Azure Arc:** Extends Azure control and management to on-premises infrastructure.
- **Bicep:** A domain-specific language for declaratively deploying Azure resources.
- **RBAC (Role-Based Access Control):** Centralizes and standardizes permissions for hybrid cloud VM management.

## Demonstration Chapters (Video Reference)

- **00:00** — Introduction
- **01:14** — Creating VMs via Azure Portal
- **08:42** — Managing VMs in Azure Local
- **19:56** — Deploying VMs with Azure CLI
- **23:30** — Using Bicep/ARM for IaC

## Additional Resources

- [Azure Local solution overview](https://learn.microsoft.com/en-us/azure/azure-local/?view=azloc-2601)
- [Azure Local Overview Video](https://www.youtube.com/watch?v=Iv6NxRv8TW4)
- [Create Azure Local VMs with Azure Arc](https://learn.microsoft.com/en-us/azure/azure-local/manage/create-arc-virtual-machines?view=azloc-2601&tabs=azurecli)

## About the Author

Thomas Maurer is the EMEA Global Black Belt (GBB) for Sovereign Cloud at Microsoft, specializing in hybrid and multicloud strategies, scalable architectures, and cloud innovation.

---

*For more tutorials and insights, visit [Thomas Maurer’s Blog](https://www.thomasmaurer.ch/).*

This post appeared first on "Thomas Maurer's Blog". [Read the entire article here](https://www.thomasmaurer.ch/2026/02/deploy-vms-on-azure-local-with-portal-cli-bicep-iac/)
