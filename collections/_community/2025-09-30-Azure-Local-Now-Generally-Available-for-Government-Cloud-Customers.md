---
layout: "post"
title: "Azure Local Now Generally Available for Government Cloud Customers"
description: "This announcement details the general availability of Azure Local for Microsoft Azure Government Cloud users. The blog highlights Azure Local's ability to bring the Azure cloud experience to on-premises environments, focusing on secure infrastructure management, compliance, and unified operations for government agencies. Key features include streamlined deployment and management, unified observability with Azure Monitor and Arc, robust built-in security links to Microsoft Defender for Cloud, extended security updates, and support for a wide range of workloads. The article guides agencies in getting started and emphasizes the platform's benefits for modernization and operational control."
author: "meenagowdar"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-general-availability-of-azure-local-on-microsoft/ba-p/4458013"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-09-30 19:12:33 +00:00
permalink: "/2025-09-30-Azure-Local-Now-Generally-Available-for-Government-Cloud-Customers.html"
categories: ["Azure", "Security"]
tags: ["AKS", "ARM Templates", "Azure", "Azure Arc", "Azure Government", "Azure Local", "Azure Monitor", "BitLocker", "Cloud Management", "Community", "Compliance", "Data Residency", "Government IT", "Hybrid Cloud", "IaC", "Kubernetes", "Microsoft Defender For Cloud", "Operational Control", "Security", "Security Updates", "Trusted Launch", "Virtual Machines", "Vtpm"]
tags_normalized: ["aks", "arm templates", "azure", "azure arc", "azure government", "azure local", "azure monitor", "bitlocker", "cloud management", "community", "compliance", "data residency", "government it", "hybrid cloud", "iac", "kubernetes", "microsoft defender for cloud", "operational control", "security", "security updates", "trusted launch", "virtual machines", "vtpm"]
---

meenagowdar outlines the release of Azure Local's general availability for Azure Government, offering government agencies secure, compliant cloud capabilities with on-premises control and advanced infrastructure security.<!--excerpt_end-->

# Azure Local Now Generally Available for Government Cloud Customers

## Overview

Azure Local is now generally available for Microsoft Azure Government Cloud customers. This release marks an important step in bringing secure and compliant cloud-connected infrastructure to government agencies, allowing them to manage workloads on-premises while leveraging the Azure ecosystem.

## What is Azure Local?

Azure Local enables government organizations to run and manage their cloud-connected infrastructure at their own physical locations. This ensures operational control, regulatory compliance, and seamless integration with the broader Azure platform. Key benefits include unified management, robust security, and operational flexibility for various workload types including virtual machines, containers, and mission-critical applications.

## Key Features

- **Streamlined Deployment & Management**: Deploy and manage infrastructure from the Azure portal or automate with tools such as ARM templates. Agencies can provision clusters, define networking and storage, and schedule automated updates, resulting in predictable and efficient operations.
- **Unified Observability**: Through native integration with Azure Monitor and Azure Arc, agencies get visibility into virtual machines, Kubernetes clusters, and physical infrastructure, benefiting from a single dashboard with comprehensive metrics and customizable alerts.
- **Non-Disruptive Updates**: Azure Update Manager simplifies updating workloads and infrastructure, ensuring mission-critical applications remain available even during maintenance.
- **Flexible Workload Support**: Supports a wide range of workloads, including Azure Local Virtual Machines and Arc-enabled Azure Kubernetes Services. Agencies can utilize both custom VM images and those from the Azure Marketplace.
- **Security by Default**: The platform is hardened with best-in-class security practices. Integration with Microsoft Defender for Cloud provides unified security management, threat detection, and automatic remediation. Additional controls include network isolation, identity management, and compliance monitoring.
- **Extended Security Updates (ESU)**: Agencies can maintain security for legacy Microsoft products with ongoing security patches beyond official support dates.
- **Trusted Launch and Data Protection**: Azure Local VMs support Trusted Launch with virtual TPM and Secure Boot. BitLocker encryption and the preservation of vTPM state during migration help protect sensitive workloads.

## Getting Started

- Visit the [Azure portal for US Government](https://portal.azure.us/) to download the latest Azure Local OS image.
- Customize deployment settings to fit cluster configuration, networking, and storage needs.
- For additional documentation, visit the [Azure Local documentation](https://learn.microsoft.com/en-us/azure/azure-local/).

## Why Azure Local for Government?

Azure Local allows agencies to modernize infrastructure, support mission-critical applications, and meet strict regulatory standards, all while maintaining strong operational control and ensuring data residency.

## Conclusion

The availability of Azure Local for Azure Government customers offers agencies secure, resilient, and scalable distributed cloud infrastructure. This solution enables modernization and operational excellence, with ongoing improvements planned for the future.

---

*Updated Sep 30, 2025*

*Author: meenagowdar*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-general-availability-of-azure-local-on-microsoft/ba-p/4458013)
