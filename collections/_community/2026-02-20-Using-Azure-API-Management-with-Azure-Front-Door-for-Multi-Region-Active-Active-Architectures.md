---
layout: "post"
title: "Using Azure API Management with Azure Front Door for Multi-Region, Active-Active Architectures"
description: "This guide explains how to architect and configure a globally available, multi-region Azure API Management (APIM) instance behind Azure Front Door. It walks through the technical steps to leverage custom origins, details the traffic routing logic, discusses APIM deployment modes (internal vs. external), and explores why combining AFD and APIM adds value for global API delivery, performance, and security."
author: "juneesingh"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/microsoft-developer-community/using-azure-api-management-with-azure-front-door-for-global/ba-p/4492384"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-20 08:00:00 +00:00
permalink: "/2026-02-20-Using-Azure-API-Management-with-Azure-Front-Door-for-Multi-Region-Active-Active-Architectures.html"
categories: ["Azure"]
tags: ["Active Active", "API Gateway", "Application Gateway", "Azure", "Azure API Management", "Azure Front Door", "Cloud Architecture", "Community", "Custom Origin", "Developer Portal", "Global Endpoint", "High Availability", "Load Balancing", "Multi Region", "Network Security", "Traffic Routing"]
tags_normalized: ["active active", "api gateway", "application gateway", "azure", "azure api management", "azure front door", "cloud architecture", "community", "custom origin", "developer portal", "global endpoint", "high availability", "load balancing", "multi region", "network security", "traffic routing"]
---

Junee Singh provides a detailed walkthrough of how to deploy and configure Azure API Management in a multi-region, active-active setup backed by Azure Front Door. This article covers architecture, traffic management, deployment modes, and security considerations for building globally available, high-performance APIs.<!--excerpt_end-->

# Using Azure API Management with Azure Front Door for Multi-Region, Active-Active Architectures

**Author:** Junee Singh (Senior Solution Engineer at Microsoft)

## Introduction

Modern API‑driven applications need global reach, low latency, and high availability. Microsoft Azure offers two complementary services to achieve this:

- **Azure API Management (APIM):** The API gateway platform for centralizing API access, security, and developer onboarding.
- **Azure Front Door (AFD):** A global entry point and load balancer that offers performance acceleration and security at the edge.

This guide shows how to pair a multi-region, active-active APIM deployment with Azure Front Door, ensuring global availability while maintaining excellent performance and strong security controls.

## Solution Architecture Overview

- APIM Premium tier supports deploying gateways in multiple regions (active-active).
- Each region exposes its own regional endpoint, e.g.:
    - `https://mydemo-apim-westeurope-01.regional.azure-api.net`
    - `https://mydemo-apim-eastus-01.regional.azure-api.net`
- Azure Front Door sits in front of these endpoints and acts as a unified, globally available public API endpoint and intelligent traffic distributor.

## Azure Front Door Configuration

### 1. Create an Origin Group

- In your Front Door profile, navigate to **Origin Groups** > **Add**.
- Define a group that will include each regional APIM gateway endpoint.

### 2. Add Each APIM Region as a Custom Origin

For each APIM region:

- **Origin type:** Custom
- **Host name:** Set to the regional APIM endpoint (e.g., `mydemo-apim-westeurope-01.regional.azure-api.net`)
- **Origin host header:** Same as host name
- **Enable certificate subject name validation** (recommended for private link or TLS)
- **Priority & Weight:** Control failover and distribution logic
- **Status:** Enable

Repeat for all APIM regions, assigning priority and weight according to desired traffic engineering.

## Verifying Regional Traffic Routing

To see which region is responding:

1. Deploy a VM in each APIM region (e.g., West Europe and East US).
2. Use curl to send a request to the AFD global endpoint, which should route the request to the regionally appropriate APIM instance based on health, priority, latency, and weight.

    ```shell
    curl -v "https://afd-blah.b01.azurefd.net/echo/resource?param1=sample"
    ```

## How Azure Front Door Routes Traffic

AFD routes requests using these criteria:

1. **Health:** Only healthy APIM endpoints are eligible.
2. **Priority:** Chooses the highest-priority available origins.
3. **Latency (optional):** May choose lowest-latency pool among candidates.
4. **Weight:** Distributes traffic proportionally across origins of equal priority.

**Example configuration:**

- West Europe (priority 1, weight 1000)
- East US (priority 1, weight 500)
- Central US (priority 2, weight 1000)

AFD sends most traffic to West Europe and East US, only failing over to Central US if both are unavailable.

See [Traffic routing methods to origin - Azure Front Door | Microsoft Learn](https://learn.microsoft.com/en-us/azure/frontdoor/routing-methods#overall-decision-flow) for detailed algorithm.

## Why Use Azure API Management?

- **Centralized API gateway**
    - Policy enforcement (auth, rate limiting, transformation, caching)
    - Modernizes APIs without breaking existing clients
- **Security & Governance**
    - Azure AD, OAuth2, mTLS integrations
    - Threat protection, schema validation
- **Developer ecosystem**
    - Portal, documentation, versioning, releases
- **Multi-region gateways (Premium)**
    - Enables global, low-latency, active-active deployments

## APIM Deployment Modes

- **External Mode:**
    - APIM is publicly accessible; can be fronted by Azure Front Door directly
    - Preferred when exposing APIs to partners, mobile apps, or internet clients
- **Internal Mode:**
    - APIM is deployed inside a VNet and only privately accessible
    - To expose privately, use both Application Gateway (as layer-7 reverse proxy) and Front Door. AFD cannot directly access internal APIM; Application Gateway provides a public endpoint.
    - Ref: [Integrate internal-mode APIM with Application Gateway](https://github.com/MicrosoftDocs/azure-docs/blob/main/articles/api-management/api-management-howto-integrate-internal-vnet-appgateway.md)

## Why Put Azure Front Door in Front of APIM?

- **Global Load Balancing:** Distributes client traffic efficiently across regions
- **Edge Security:** Web Application Firewall (WAF), DDoS protection, TLS offload
- **Performance:** Anycast routing and global points of presence (POPs) reduce latency
- **Unified Global Endpoint:** Single public URL that abstracts multi-region complexity
    - Example: `https://api.contoso.com` routes intelligently to multiple APIM regions
- Detailed info: [Traffic acceleration - Azure Front Door | Microsoft Learn](https://learn.microsoft.com/en-us/azure/frontdoor/front-door-traffic-acceleration?pivots=front-door-standard-premium#select-the-front-door-edge-location-for-the-request-anycast)

## Conclusion

Combining Azure Front Door with multi-region APIM deployment delivers a highly available, secure, and performant global API platform for modern cloud architectures.

---
**Credits:**  
*Junee Singh, Senior Solution Engineer at Microsoft*  
*Isiah Hudson, Senior Solution Engineer at Microsoft*  

*Updated Feb 12, 2026 / Version 1.0*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/using-azure-api-management-with-azure-front-door-for-global/ba-p/4492384)
