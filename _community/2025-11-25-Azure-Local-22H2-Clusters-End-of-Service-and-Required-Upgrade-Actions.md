---
layout: "post"
title: "Azure Local 22H2 Clusters: End of Service and Required Upgrade Actions"
description: "This post from Arpita Duppala details the end of service (EOS) for Azure Local (formerly Azure Stack HCI) 22H2 clusters, the impact on security and compliance, and the crucial need for Microsoft customers to upgrade to version 24H2. It covers what will change, risks of remaining unsupported, disabling of ESU/WSS, and provides actionable recommendations for IT administrators managing Azure hybrid or adaptive cloud environments."
author: "Arpita Duppala"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-arc-blog/azure-local-22h2-clusters-end-of-service-and-feature-degradation/ba-p/4470129"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-25 15:17:52 +00:00
permalink: "/2025-11-25-Azure-Local-22H2-Clusters-End-of-Service-and-Required-Upgrade-Actions.html"
categories: ["Azure", "Security"]
tags: ["22H2", "24H2", "Azure", "Azure Local", "Azure Stack HCI", "Cluster Management", "Community", "Compliance", "End Of Service", "ESU", "Feature Degradation", "Hybrid Cloud", "Lifecycle Policy", "Microsoft Azure", "Security", "Security Updates", "Upgrade Guidance", "Windows Server Subscription"]
tags_normalized: ["22h2", "24h2", "azure", "azure local", "azure stack hci", "cluster management", "community", "compliance", "end of service", "esu", "feature degradation", "hybrid cloud", "lifecycle policy", "microsoft azure", "security", "security updates", "upgrade guidance", "windows server subscription"]
---

Arpita Duppala explains the end of service for Azure Local 22H2 clusters, highlighting critical security and compliance risks and outlining the steps administrators must take to upgrade and avoid disruption.<!--excerpt_end-->

# Azure Local 22H2 Clusters: End of Service and Required Upgrade Actions

Azure Local (formerly Azure Stack HCI) version 22H2 reached **End of Service (EOS)** on May 31, 2025. After this date, Microsoft no longer provides security updates or bug fixes for 22H2 clusters. Only limited support—for upgrade assistance—is available.

## What's Changing?

Starting around **February 23, 2026**, Microsoft will begin to degrade features on 22H2 clusters to enforce its [Modern Lifecycle Policy](https://learn.microsoft.com/en-us/lifecycle/policies/modern). This means:

- **Extended Security Updates (ESU) will be disabled**: No more ESU available for 22H2 clusters.
- **Windows Server Subscription (WSS) will be removed**: Subscription benefits will not be available or renewable.
- **ESU updates will no longer be offered**, exposing guest OSes to security vulnerabilities.
- **Guest operating systems will no longer be licensed**, risking compliance violations and potential disruptions.
- **Degraded features are not recoverable**, even with best-effort support.

## Customer Responsibility

If you choose to stay on 22H2, you:

- **Fully assume all security and compliance risks** for unsupported clusters.
- May face government regulatory issues and service disruptions.
- Will not receive guarantees or risk remediation from Microsoft for continued use of 22H2.

## Next Steps

To maintain a secure and supported operational environment:

- **Plan and execute an upgrade to 24H2 as soon as possible.**
- Reference Microsoft’s official guidance to ensure a smooth transition: [Upgrade instructions](https://learn.microsoft.com/en-us/azure/azure-local/upgrade/upgrade-22h2-to-23h2-powershell?view=azloc-2510&pivots=os-24h2)
- Proactively schedule your upgrade to avoid loss of features, compliance risks, and security vulnerabilities.

## Additional Information

For more context on Microsoft's adaptive cloud approach, cluster management, and hybrid cloud lifecycle practices, follow updates on the [Azure Arc Blog](/category/azure/blog/azurearcblog).

**Updated:** Nov 17, 2025  
**Author:** Arpita Duppala  

---

*Stay current to stay protected and compliant when running Azure hybrid environments. Upgrade paths are available and customers are strongly encouraged to act before critical features are disabled.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/azure-local-22h2-clusters-end-of-service-and-feature-degradation/ba-p/4470129)
