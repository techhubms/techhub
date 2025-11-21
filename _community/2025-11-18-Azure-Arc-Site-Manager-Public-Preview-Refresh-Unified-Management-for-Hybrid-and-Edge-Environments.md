---
layout: "post"
title: "Azure Arc Site Manager Public Preview Refresh: Unified Management for Hybrid and Edge Environments"
description: "This announcement introduces the refreshed public preview of Azure Arc Site Manager, a cloud-native solution for unified site configuration, monitoring, and management across industrial, retail, and distributed infrastructure environments. Key features include flexible site scopes, hierarchical organization mirroring business structures, aggregated monitoring of updates and security, reusable configurations, and integration with Azure Resource Manager and standard tooling (CLI, SDKs, Terraform, PowerShell). The post highlights technical details, regional limitations, and provides links to documentation and CLI references for further exploration."
author: "supriyobanerjee"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-arc-blog/public-preview-refresh-announcement-site-manager/ba-p/4468929"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-18 16:00:00 +00:00
permalink: "/2025-11-18-Azure-Arc-Site-Manager-Public-Preview-Refresh-Unified-Management-for-Hybrid-and-Edge-Environments.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Aggregated Alerts", "Azure", "Azure Arc", "Azure CLI", "Azure Resource Manager", "Community", "DevOps", "Edge Computing", "Hierarchical Organization", "Hybrid Cloud", "IoT", "Kubernetes", "Observability", "Operational Efficiency", "PowerShell", "RBAC", "Resource Groups", "Role Based Access Control", "Security", "Security Monitoring", "Service Groups", "Site Manager", "Terraform", "Update Management"]
tags_normalized: ["aggregated alerts", "azure", "azure arc", "azure cli", "azure resource manager", "community", "devops", "edge computing", "hierarchical organization", "hybrid cloud", "iot", "kubernetes", "observability", "operational efficiency", "powershell", "rbac", "resource groups", "role based access control", "security", "security monitoring", "service groups", "site manager", "terraform", "update management"]
---

supriyobanerjee announces the Azure Arc Site Manager Public Preview Refresh, detailing improved management, configuration, and security monitoring for hybrid and edge environments.<!--excerpt_end-->

# Azure Arc Site Manager Public Preview Refresh: Unified Management for Hybrid and Edge Environments

## Introduction

Modern industrial and retail environments increasingly rely on distributed infrastructure such as factories, manufacturing plants, and hybrid cloud deployments. These environments commonly leverage Azure Arc-enabled resources, including servers, VMs, Kubernetes clusters, and IoT assets, alongside on-premises systems. Managing these at scale introduces complexity, operational risk, governance challenges, and the need for unified observability.

## Public Preview Refresh Announcement

Azure Arc Site Manager is now available in refreshed public preview, providing a production-ready platform to streamline site configuration and lifecycle management for hybrid and edge scenarios. Site Manager aggregates security, alerts, updates, and connectivity status, enabling consistent monitoring, policy-driven compliance, and simplified, reliable operations.

## Key Features

### 1. Flexible Site Scope with Service Groups

- Create sites based on Resource Groups, Subscriptions, or Service Groups, allowing logical grouping of resources across boundaries to reflect real-world operations for improved scalability and governance.

### 2. Hierarchical Site Organization

- Support for multi-level site hierarchy, including parent sites and subsites, mirroring organizational structure (e.g., Regions, Business Units, Factories, Stores) for clear visibility and streamlined management.

### 3. Aggregated Monitoring and Insights

- Centralized pane of glass aggregates connectivity, updates, alerts, and security status, simplifying issue identification and operational prioritization across distributed infrastructure.

### 4. Site Configurations

- Define and reuse configuration settings (network, secrets) across deployments and partner solutions, accelerating provisioning and ensuring operational consistency.

## Architecture & Workflow

- Azure Arc Site Manager is built as a cloud-native service on Azure Resource Manager (ARM).
- All monitoring and aggregations are presented through extension resources in the Azure portal or via clients (CLI, SDKs, PowerShell, Terraform), supporting flexible, secure operations with RBAC.
- Management operations such as site creation, updates, and queries can be performed across standard client tools.

## Details & Limitations

- Status aggregation currently applies to supported Azure resource types only. Unsupported resource types are not managed directly but continue normal operation.
- Regional availability is limited; for example, Brazil South, UAE North, and South Africa North currently have restrictions on Arc-enabled machines and Kubernetes clusters for connectivity and update status.

## Feedback and Engagement

- Comments and feedback are invited directly on the blog.
- Feedback forms and contact channels are available, with the team actively monitoring responses and considering user input for future improvements.
- Links provided for deeper technical exploration:
    - [Site Manager Documentation](https://learn.microsoft.com/en-us/azure/azure-arc/site-manager/)
    - [Az.Site CLI Module Reference](https://learn.microsoft.com/en-us/powershell/module/az.site/?view=azps-14.6.0)

## Conclusion

Azure Arc Site Manager advances unified management and observability for hybrid and edge deployments, facilitating aggregated monitoring, simplified configuration, and scalable governance across distributed environments.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/public-preview-refresh-announcement-site-manager/ba-p/4468929)
