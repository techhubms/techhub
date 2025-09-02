---
layout: "post"
title: "Broadcom VMware Licensing Changes Impacting Azure VMware Solution Customers"
description: "This article provides an in-depth overview of recent changes in Broadcom's VMware licensing model and their implications for Azure VMware Solution (AVS) customers. It addresses the new bring-your-own-license (BYOL) requirements, key transition dates, compatibility with AVS, and offers practical guidance for navigating the upcoming changes."
author: "christopheherrbach"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-migration-and/broadcom-vmware-licensing-changes-what-azure-vmware-solution/ba-p/4448784"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-09-02 17:41:37 +00:00
permalink: "/2025-09-02-Broadcom-VMware-Licensing-Changes-Impacting-Azure-VMware-Solution-Customers.html"
categories: ["Azure"]
tags: ["AVS", "Azure", "Azure VMware Solution", "Broadcom", "BYOL", "Cloud Deployment", "Cloud Licensing", "Community", "Hyperscaler Platforms", "Infrastructure Management", "Licensing", "Microsoft", "Microsoft Azure", "Reserved Instance", "VCF", "VMware Cloud Foundation"]
tags_normalized: ["avs", "azure", "azure vmware solution", "broadcom", "byol", "cloud deployment", "cloud licensing", "community", "hyperscaler platforms", "infrastructure management", "licensing", "microsoft", "microsoft azure", "reserved instance", "vcf", "vmware cloud foundation"]
---

Christophe Herrbach explains the upcoming changes to Broadcom's VMware licensing and how Azure VMware Solution customers can adapt to the new bring-your-own license model.<!--excerpt_end-->

# Broadcom VMware Licensing Changes: What Azure VMware Solution Customers Need to Know

**Author:** Christophe Herrbach, Head of AVS Product Management at Microsoft

## Overview

Broadcom has announced significant changes to the VMware licensing model for hyperscaler platforms, including Azure VMware Solution (AVS), effective from November 1, 2025. This update impacts how customers will license VMware Cloud Foundation (VCF) when deploying workloads on cloud platforms.

## What’s Changing?

- **Portable Subscription Required:** Broadcom will require customers to purchase "bring your own" portable VCF subscriptions directly from Broadcom in order to use VMware services on hyperscaler clouds in the future including AVS.
- **Licensing, Not Product, Change:** The structure of AVS as a managed VMware VCF service on Azure remains unchanged. Microsoft will continue to manage all infrastructure and VMware host software, including maintenance, patching, and upgrades.

## AVS Compatibility and Options

- **AVS BYOL Model Ready:** AVS already supports the BYOL model with the AVS VCF BYO L option available across all 35 AVS regions. This option allows customers to use their own VCF license, typically at a lower cost compared to bundled licensing.
- **No Interruptions to Service:** No technical or feature changes affect how AVS operates. All service management, patching, and monitoring remain handled by Microsoft.

## Transition Dates & Migration Details

- **October 15, 2025:** Microsoft stops selling AVS with bundled VCF subscriptions. After this, new AVS nodes require a customer-provided VCF subscription.
- **Existing Reserved Instances:** Customers who purchase AVS Reserved Instances (RIs) on or before this date can continue using their nodes until the end of their RI term with no licensing changes.
- **PayGo Nodes:** AVS with license-included PayGo nodes are supported with current licensing through October 31, 2026. Customers may also switch to AVS RIs with included licenses before the cutoff.

## Resources and Next Steps

- [Using portable VCF subscriptions with Azure VMware Solution](https://learn.microsoft.com/en-us/azure/azure-vmware/vmware-cloud-foundations-license-portability)
- [Purchasing AVS Reserved Instances](https://learn.microsoft.com/en-us/azure/azure-vmware/reserved-instance)
- [Broadcom Blog: VMware Licensing Change Announcement](https://blogs.vmware.com/cloud-foundation/2025/08/29/vmware-cloud-foundation-cloud-on-your-terms/)
- [Broadcom Channel Partners](https://www.broadcom.com/how-to-buy/partner-distributor-lookup)

## Support

Microsoft will engage directly with current AVS customers to provide more details and support during the transition. For questions, contact your Microsoft account representative.

---

_Last updated: September 2, 2025_

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-migration-and/broadcom-vmware-licensing-changes-what-azure-vmware-solution/ba-p/4448784)
