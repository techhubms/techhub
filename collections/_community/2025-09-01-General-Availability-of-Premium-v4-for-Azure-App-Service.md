---
layout: post
title: General Availability of Premium v4 for Azure App Service
author: Stefan_Schackow
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-general-availability-of-premium-v4-for-azure-app/ba-p/4446204
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-09-01 15:44:34 +00:00
permalink: /azure/community/General-Availability-of-Premium-v4-for-Azure-App-Service
tags:
- App Modernization
- ASP.NET
- Availability Zones
- Azure
- Azure App Service
- Cloud Scalability
- Community
- Cost Optimization
- Linux
- Load Testing
- PaaS
- Performance Improvement
- Premium V4
- VM Sizing
- Web Hosting
- Windows
section_names:
- azure
---
Stefan Schackow shares an overview of Azure App Service Premium v4 GA, outlining its improved performance, lower costs, hardware enhancements, and updated service tiers for both Windows and Linux users.<!--excerpt_end-->

# General Availability of Premium v4 for Azure App Service

Azure App Service Premium v4 is now generally available for both Windows and Linux customers. Premium v4 brings substantial improvements in platform performance, scalability, and cost effectiveness for running web applications at scale.

## Key Benefits

- Fully managed platform-as-a-service (PaaS) for web stacks on Windows and Linux
- Built with the latest AMD Dadsv6/Eadsv6 series hardware featuring NVMe temporary storage
- Enhanced performance: Up to 58% higher throughput and lower response times compared to earlier plans
- New pricing optimized for large-scale app modernization
- Additional reliability with features like integrated monitoring, deployment slots, and zone resilience
- Multiple SKU options from 1 vCPU/4GB (P0v4) up to 32 vCPUs/256GB (P5mv4)

## Performance and Load Testing Results

Recent comparative load tests between Premium v4 and Premium v3 revealed:

| SKU     | CPU Utilization | Throughput (RPS) | Pv4 vs Pv3 Throughput | Response Time (p90) |
|---------|----------------|------------------|----------------------|---------------------|
| P1v3    | 77.3%          | 817              |                      | 20 ms               |
| **P1v4**    | 78.8%          | **1295**         | **+58.5% higher**    | **17 ms**           |
| P1mv3   | 79.6%          | 1048             |                      | 17 ms               |
| **P1mv4**   | 79.5%          | **1329**         | **+26.8% higher**    | **16 ms**           |

- Test site: Classic ASP.NET app used in lab settings, with simple .aspx pages and no external dependencies.
- Load was generated using Azure App Testing in the same Azure region for reliable measurements.
- All load scenarios were targeted at ~80% CPU utilization on a single instance to estimate real-world performance.

*Note: Hardware used for these comparisons was Standard_D2d_v4 for P1v3 and Standard_D2ads_v6 for P1v4; Standard_E2ads_v5 for P1mv3 and Standard_E2ads_v6 for P1mv4.*

## Getting Started

Premium v4 is available in [twenty Azure regions](https://aka.ms/AppService/Pv4regions). Guidance for configuration and a live list of supported regions is available in the [product documentation](https://aka.ms/AppService/Premiumv4).

- Leverage Azure partners and tools to analyze and modernize your apps.
- Utilize new pricing and scaling options for higher throughput and cost savings.
- Enhance reliability with improved hardware and availability zones.

## Resources

- [Azure App Service documentation](https://aka.ms/AppService/Pv4docs)
- [Azure App Service web page](https://www.azure.com/appservice)
- [Available regions](https://aka.ms/AppService/Pv4regions)
- [@AzAppService on X](https://x.com/AzAppService)

## Author

Stefan Schackow

*For further information, follow the links above to start exploring Premium v4 and take advantage of enhanced web app hosting on Azure.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-general-availability-of-premium-v4-for-azure-app/ba-p/4446204)
