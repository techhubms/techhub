---
layout: post
title: 'Azure API Management Workspaces Breaking Changes Update: Built-in Gateway & Tier Support'
author: budzynski
canonical_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/update-to-api-management-workspaces-breaking-changes-built-in/ba-p/4443668
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community
date: 2025-08-14 20:52:17 +00:00
permalink: /azure/community/Azure-API-Management-Workspaces-Breaking-Changes-Update-Built-in-Gateway-and-Tier-Support
tags:
- API Limits
- API Management
- Azure API Management
- Breaking Changes
- Built in Gateway
- Cloud Integration
- Developer Tier
- Federated API
- Managed Identities
- Microsoft Azure
- Premium V2
- Service Tiers
- Standard V2
- Workspace Gateways
- Workspaces
section_names:
- azure
---
budzynski outlines the latest adjustments to Azure API Management workspace support, specifically regarding built-in gateway functionality and tier-based limitations. This update is crucial for developers and architects managing APIs across various Azure service tiers.<!--excerpt_end-->

# Update To API Management Workspaces Breaking Changes: Built-in Gateway & Tiers Support

## Overview

Microsoft has announced an update to previously communicated breaking changes around Azure API Management (APIM) workspaces. This specifically affects the use of preview workspaces on the built-in gateway.

## What’s Changing?

- **Continued support**: If your APIM instance uses preview workspaces on the built-in gateway and aligns with the new tier-based workspace limits, these workspaces will continue to operate unchanged. Additionally, such workspaces will automatically transition to general availability upon the formal release of built-in gateway support.
- **Workspace limits** are now tier-dependent:

  | API Management Tier     | Workspace Limit on Built-in Gateway |
  |------------------------|--------------------------------------|
  | Premium & Premium v2   | Up to 30 workspaces                   |
  | Standard & Standard v2 | Up to 5 workspaces                    |
  | Basic & Basic v2       | Up to 1 workspace                     |
  | Developer              | Up to 1 workspace                     |

## Why Is This Changing?

Microsoft initially required workspace gateways to improve reliability and scalability, especially in large or federated API environments. However, feedback from customers utilizing preview workspaces or operating in non-Premium tiers highlighted a continued need for built-in gateway support with workspaces. This rollback ensures existing workflows are maintained while still encouraging the use of workspace gateways where advanced scalability and isolation are needed.

## What’s Not Changing?

- Other aspects of the workspace-related breaking changes remain in effect.
- Service-level managed identities remain unavailable within workspaces.
- Premium and Premium v2 services continue to support both workspace models: built-in gateway and dedicated workspace gateways.

## Resources

- [Workspaces in Azure API Management](https://aka.ms/apimdocs/workspaces)
- [Breaking Change Announcements](https://learn.microsoft.com/azure/api-management/breaking-changes/workspaces-breaking-changes-june-2024)
- [Requirement for workspace gateways (March 2025)](https://learn.microsoft.com/azure/api-management/breaking-changes/workspaces-breaking-changes-march-2025)

_Last updated: August 14, 2025_

For additional technical discussion or support, visit the [Azure Integration Services Blog](/category/azure/blog/integrationsonazureblog).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/update-to-api-management-workspaces-breaking-changes-built-in/ba-p/4443668)
