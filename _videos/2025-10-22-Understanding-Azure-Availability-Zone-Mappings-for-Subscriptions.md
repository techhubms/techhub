---
layout: "post"
title: "Understanding Azure Availability Zone Mappings for Subscriptions"
description: "This video by John Savill offers a concise guide to how Azure availability zones (AZs) in a subscription map to the underlying datacenter sets and how those align with AZs in other subscriptions. It includes a walkthrough of a custom PowerShell script, explains the physical vs logical separation of AZs, and demonstrates practical techniques for assessing regional AZ alignment. Resources and further learning links are also provided."
author: "John Savill's Technical Training"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=jBpxG2Fk2jA"
viewing_mode: "internal"
feed_name: "John Savill's Technical Training"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UCpIn7ox7j7bH_OFj7tYouOQ"
date: 2025-10-22 16:32:44 +00:00
permalink: "/2025-10-22-Understanding-Azure-Availability-Zone-Mappings-for-Subscriptions.html"
categories: ["Azure"]
tags: ["Availability Zone", "AZ Alignment", "Azure", "Azure Availability Zones", "Azure Capacity", "Azure Cloud", "Azure Scripting", "Capacity", "Cloud", "Cloud Architecture", "Disaster Recovery", "High Availability", "Logical Zones", "Microsoft", "Microsoft Azure", "Physical Datacenters", "PowerShell", "Read AzureAZs.ps1", "Regional Mapping", "Subscriptions", "Videos"]
tags_normalized: ["availability zone", "az alignment", "azure", "azure availability zones", "azure capacity", "azure cloud", "azure scripting", "capacity", "cloud", "cloud architecture", "disaster recovery", "high availability", "logical zones", "microsoft", "microsoft azure", "physical datacenters", "powershell", "read azureazsdotps1", "regional mapping", "subscriptions", "videos"]
---

John Savill's Technical Training explains how Azure subscription availability zones map to physical datacenters and align across different subscriptions, featuring a PowerShell script walkthrough.<!--excerpt_end-->

{% youtube jBpxG2Fk2jA %}

# Understanding Azure Availability Zone Mappings for Subscriptions

In this video, **John Savill** provides a practical overview of how Azure Availability Zones (AZs) within a given subscription are mapped to the actual physical datacenters in a region and how this mapping may differ or align across different subscriptions. This guidance is essential for architects and engineers planning for redundancy, disaster recovery, and high availability in Azure.

## Topics Covered

- **AZ Refresher**: A brief overview of what Availability Zones are in Azure and why they matter for resiliency and planning (00:13).
- **AZ Alignment Between Subscriptions**: Explanation of how the logical representation of AZs in one subscription may map to different physical datacenters compared to another subscription, and why this is important (01:52).
- **Script Walkthrough**: Step-by-step walkthrough of the `Read-AzureAZs.ps1` PowerShell script, which can be used to analyze AZ mappings within your subscription (04:02).
- **Demo**: Live demonstration of using the script to examine AZ mapping in the `eastus` and `westus2` Azure regions (08:20).

## Script Usage Example

```powershell
. .\Read-AzureAZs.ps1 read-azureazs eastus, westus2
```

This PowerShell script, available on [GitHub](https://github.com/johnthebrit/RandomStuff/blob/master/AzureAZCheck/Read-AzureAZs.ps1), allows users to inspect how their subscription's AZs are mapped and aligned.

## Key Documentation

- [Understanding Azure Availability Zones — Physical and Logical Separation](https://learn.microsoft.com/azure/reliability/availability-zones-overview?tabs=azure-cli#physical-and-logical-availability-zones)

## Additional Learning

- [Azure Learning Path](https://learn.onboardtoazure.com)
- [Certification Content Repository](https://github.com/johnthebrit/CertificationMaterials)
- [Weekly Azure Update Playlist](https://youtube.com/playlist?list=PLlVtbbG169nEv7jSfOVmQGRp9wAoAM0Ks)
- [DevOps Master Class](https://youtube.com/playlist?list=PLlVtbbG169nFr8RzQ4GIxUEznpNR53ERq)
- [PowerShell Master Class](https://youtube.com/playlist?list=PLlVtbbG169nFq_hR7FcMYg32xsSAObuq8)

## Final Notes

- For questions, the author recommends posting on community forums like Reddit or Microsoft Community Hub, as direct responses are not possible due to channel size.
- Subtitles and automatic translations are available for broader accessibility.

---

This content is tailored for Azure practitioners and architects interested in advanced knowledge of cloud infrastructure resilience, AZ alignment, and scripting. Authored by John Savill.
