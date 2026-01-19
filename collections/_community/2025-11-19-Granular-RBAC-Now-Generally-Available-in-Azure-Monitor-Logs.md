---
layout: post
title: Granular RBAC Now Generally Available in Azure Monitor Logs
author: Ron Frenkel
canonical_url: https://techcommunity.microsoft.com/t5/azure-observability-blog/general-availability-granular-rbac-in-azure-monitor-logs/ba-p/4471299
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-19 09:55:04 +00:00
permalink: /azure/community/Granular-RBAC-Now-Generally-Available-in-Azure-Monitor-Logs
tags:
- ABAC
- Access Management
- Attribute Based Access Control
- Azure China
- Azure Government
- Azure Monitor Logs
- Azure Observability
- Azure Public Cloud
- Azure RBAC
- Compliance
- Data Security
- Data Segregation
- Granular RBAC
- Least Privilege
- Log Analytics
- Log Analytics Data Reader
- Role Based Access Control
- Row Level Security
- Table Level Security
- Workspace Security
section_names:
- azure
- security
---
Ron Frenkel introduces the general availability of Granular RBAC in Azure Monitor Logs, describing fine-grained access controls and enhanced security options for centralized log management.<!--excerpt_end-->

# Granular RBAC Now Generally Available in Azure Monitor Logs

**Author:** Ron Frenkel

## Overview

Granular Role-Based Access Control (RBAC) is now generally available in Azure Monitor Logs, delivering advanced data access control at the row level. This update follows the feature's public preview in May 2025 and brings production-grade security and flexibility to organizations needing precise data segregation.

## Key Capabilities

- **Row-Level, Table-Level, and Workspace-Level Security**: Implement the principle of least privilege with access controls that can be set on any data tier.
- **Centralized Log Analytics Workspace**: Maintain all log data in a single platform while customizing user access as needed.
- **Data Plane and Control Plane Separation**: Utilize Azure Attribute-Based Access Control (ABAC) with RBAC role assignments for rigorous control.
- **Dynamic Data Filtering**: Target access according to roles, regions, and data sensitivity, ensuring users see only what they're authorized to.

## What's New

- **Worldwide Availability**: Supported now in Azure Public Cloud, Azure Government (GCC), and Azure China.
- **Updated Built-in Role**: The Log Analytics Data Reader role is enhanced for full Granular RBAC support for seamless integration.

## Getting Started

To configure Granular RBAC for Azure Monitor Logs:

1. Review [official documentation](https://learn.microsoft.com/en-us/azure/azure-monitor/logs/manage-access?tabs=portal) for managing access.
2. Assign roles using Azure ABAC for specific control at row, table, or workspace levels.
3. Set up conditional filters based on organizational structure or compliance needs.
4. Validate access by testing with representative users and roles.

[Further setup information and resources](https://aka.ms/LogsGranularRBAC)

## Example Usage Scenarios

- Segregating log data so only finance team members see relevant events.
- Restricting access to security logs based on role or region.
- Managing multi-tenant workspaces without compromising confidentiality.

## Additional Details

- **Version:** 1.0
- **Publication Date:** November 19, 2025
- **Applies To:** Azure Monitor, Log Analytics, ABAC-enabled access management

## Author Profile

Ron Frenkel is a member of the Microsoft Azure Observability team, contributing to enhancements in monitoring, logging, and cloud access security.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/general-availability-granular-rbac-in-azure-monitor-logs/ba-p/4471299)
