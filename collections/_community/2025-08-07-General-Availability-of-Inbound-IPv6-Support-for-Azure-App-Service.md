---
layout: "post"
title: "General Availability of Inbound IPv6 Support for Azure App Service"
description: "This announcement details the general availability of inbound IPv6 support for public multi-tenant Azure App Service, including across all regions and Microsoft government clouds. It covers configuration steps (such as IPMode property), client requirements, DNS management, compatibility details, current limitations, and roadmap for future IPv6 features on Azure App Service for both Windows and Linux."
author: "jordanselig"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-general-availability-of-app-service-inbound-ipv6/ba-p/4423358"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-07 15:04:17 +00:00
permalink: "/community/2025-08-07-General-Availability-of-Inbound-IPv6-Support-for-Azure-App-Service.html"
categories: ["Azure"]
tags: ["Azure", "Azure App Service", "Azure Government", "Cloud Deployment", "Community", "DNS", "Functions Consumption", "Functions Elastic Premium", "Inbound Traffic", "IPMode", "IPv6", "Linux App Service", "Logic Apps Standard", "Multi Tenant", "Network Configuration", "Public Cloud", "Windows App Service"]
tags_normalized: ["azure", "azure app service", "azure government", "cloud deployment", "community", "dns", "functions consumption", "functions elastic premium", "inbound traffic", "ipmode", "ipv6", "linux app service", "logic apps standard", "multi tenant", "network configuration", "public cloud", "windows app service"]
---

jordanselig from Microsoft announces the general availability of inbound IPv6 support for Azure App Service, outlining its implementation, configuration, and future plans.<!--excerpt_end-->

# General Availability of Inbound IPv6 Support for Azure App Service

**Author:** jordanselig

Inbound IPv6 support is now generally available on public multi-tenant Azure App Service, after a period in public preview. This release covers all public Azure regions, Azure Government, and Microsoft Azure operated by 21Vianet, and is available for multi-tenant apps on all Basic, Standard, and Premium SKUs, Functions Consumption, Functions Elastic Premium, and Logic Apps Standard.

## Key Details

- **IP-SSL IPv6 bindings** are still not supported (other limitations from preview have been removed).
- IPv6 inbound requires:
  - An IPv6 address accepting incoming traffic
  - DNS record returning an IPv6 (AAAA) record
  - Clients capable of sending/receiving IPv6 traffic (many networks only support IPv4)
- All App Service deployment units now have IPv6 addresses assigned, enabling inbound traffic via both IPv4 and IPv6.
- By default, DNS for `*.azurewebsites.net` only returns the IPv4 address for backward compatibility.
- **IPMode property:** Site property can be set to `IPv6` or `IPv4AndIPv6`:
  - `IPv6`: DNS returns only IPv6; clients must support IPv6
  - `IPv4AndIPv6`: DNS returns both; enables dual-stack connectivity
- For custom domains, manage DNS records similarly (AAAA for IPv6, CNAME to default hostname as needed).
- **Important:** IPMode affects only DNS resolution, not endpoint accessibility (all App Service sites accept traffic on both protocols regardless of IPMode setting).

## How to Test

Use a client/system that supports IPv6 networking. Example command:

```sh
curl -6 https://<app-name>.azurewebsites.net
```

## Documentation

For configuration and implementation details, see the [App Service inbound IPv6 documentation](https://aka.ms/app-service-inbound-ipv6).

## Future Roadmap

1. *Coming soon*: Public preview of IPv6 non-vnet outbound support for Linux (multi-tenant); Windows support is already in preview ([official blog post](https://techcommunity.microsoft.com/blog/appsonazureblog/announcing-app-service-outbound-ipv6-support-in-public-preview/4423368))
2. *Backlog*: IPv6 vnet outbound support (multi-tenant & ASE v3)
3. *Backlog*: IPv6 vnet inbound support (ASE v3, internal & external)

Version 2.0 (Updated Aug 07, 2025)

For more news and updates, follow the [Apps on Azure Blog](https://azure.github.io/AppService/2024/11/08/Announcing-Inbound-IPv6-support).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-general-availability-of-app-service-inbound-ipv6/ba-p/4423358)
