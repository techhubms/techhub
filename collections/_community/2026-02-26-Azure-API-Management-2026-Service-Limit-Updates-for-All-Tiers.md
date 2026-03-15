---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/new-azure-api-management-service-limits/ba-p/4497574
title: 'Azure API Management: 2026 Service Limit Updates for All Tiers'
author: Sreekanth_Thirthala
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-26 16:52:30 +00:00
tags:
- API Limits
- API Management
- API Operations
- Azure
- Azure API Management
- Azure Support
- Basic Tier
- Cloud Operations
- Community
- Consumption Tier
- Developer Tier
- Enterprise Integration
- Grandfathering Policy
- Premium Tier
- Resource Limits
- Self Hosted Gateway
- Service Tiers
- Standard Tier
section_names:
- azure
---
Sreekanth Thirthala outlines the 2026 changes to Azure API Management service limits, detailing resource caps per tier and strategies for managing or increasing those limits.<!--excerpt_end-->

# Azure API Management: 2026 Service Limit Updates for All Tiers

**Author:** Sreekanth Thirthala  
**Updated:** February 26, 2026  
**Version:** 2.0

## Overview

Azure API Management (APIM) will update service limits across all tiers beginning March 2026. These changes affect Consumption, Developer, Basic (and Basic v2), Standard (and Standard v2), and Premium (and Premium v2) tiers. The updates provide consistent, capacity-driven resource caps and clarify the process for managing or increasing limits.

## Why Service Limits Matter

API Management runs on physical Azure infrastructure with finite resources. To guarantee reliability for all customers:

- Limits are based on Azure platform capacity, tier capability, and real-world usage trends.
- Resource caps are interrelated and designed to prevent a single usage pattern from degrading shared service performance.

## 2026 Change Details

**Rollout dates:**

- Consumption, Developer, Basic, Basic v2: _March 15, 2026_
- Standard, Standard v2: _April 15, 2026_
- Premium, Premium v2: _May 15, 2026_

### Updated Resource Limits (per instance)

| Resource/Entity             | Consumption | Developer | Basic/Basic v2 | Standard/Standard v2 | Premium/Premium v2 |
|----------------------------|-------------|-----------|----------------|----------------------|-------------------|
| API operations             | 3,000       | 3,000     | 10,000         | 50,000               | 75,000            |
| API tags                   | 1,500       | 1,500     | 1,500          | 2,500                | 15,000            |
| Named values               | 5,000       | 5,000     | 5,000          | 10,000               | 18,000            |
| Loggers                    | 100         | 100       | 100            | 200                  | 400               |
| Products                   | 100         | 100       | 200            | 500                  | 2,000             |
| Subscriptions              | N/A         | 10,000    | 15,000         | 25,000               | 75,000            |
| Users                      | N/A         | 20,000    | 20,000         | 50,000               | 75,000            |
| Workspaces per gateway     | N/A         | N/A       | N/A            | N/A                   | 30                |
| Self-hosted gateways       | N/A         | 5         | N/A            | N/A                   | 1001              |

*Premium values include special cases (see full documentation for details).

### Key Changes

- Classic tier limits are now consistent with v2 tier limits.
- Enforced for major resource types (API operations, tags, products, subscriptions) tied directly to service capacity.

## Existing Customers: Grandfathering Policy

- Customers whose current usage exceeds new limits will be _grandfathered_: those resources will have new caps set 10% above observed usage at enforcement time.
- Grandfathering is per service and per service tier.
- All new services and non-grandfathered resources will comply with the new limits directly.

## Guidelines for Limit Increases

If you need to exceed these limits:

- First, review and optimize existing usage ([Microsoft doc: Manage resources within limits](https://learn.microsoft.com/en-us/azure/api-management/service-limits#strategies-to-manage-resources)).
- Limit increases can only be requested for _Standard/Standard v2_ and _Premium/Premium v2_ tiers.
- Requests are evaluated case-by-case, with priority for Premium customers.
- Submit requests through the Azure portal ([See Azure support plans](https://azure.microsoft.com/support/)).

> _Raising limits may negatively affect instance stability or performance. Consider alternatives before submitting requests._

## Conclusion

These updates standardize resource limits across all tiers and clarify the process for managing large, production-scale workloads. Customers are encouraged to review their API Management instances ahead of rollout and to proactively optimize resource consumption where possible.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/new-azure-api-management-service-limits/ba-p/4497574)
