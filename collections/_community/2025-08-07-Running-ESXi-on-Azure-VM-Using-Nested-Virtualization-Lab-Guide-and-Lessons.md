---
external_url: https://www.reddit.com/r/AZURE/comments/1mkatqa/esxi_on_azure_vm/
title: 'Running ESXi on Azure VM Using Nested Virtualization: Lab Guide and Lessons'
author: delecoute
viewing_mode: external
feed_name: Reddit Azure
date: 2025-08-07 20:18:01 +00:00
tags:
- AVS
- Azure Migrate
- Azure VM
- Cloud Labs
- CSP
- EA
- ESXi
- High Availability
- Lab Environment
- Microsoft Azure
- Nested Virtualization
- Pay as You Go
- Student Subscription
- Subscription Models
- VMware
section_names:
- azure
---
delecoute documents their experience attempting to run ESXi on an Azure VM with nested virtualization, offering a transparent technical guide and thoughtful analysis for fellow cloud lab enthusiasts.<!--excerpt_end-->

# Running ESXi on Azure VM Using Nested Virtualization: Lab Guide and Lessons

**Author:** delecoute  

## Overview

This post shares a detailed account of running ESXi on a Microsoft Azure VM using nested virtualization. The author provides a lab guide ([lab guide link](https://rsemane.github.io/ESXI-on-Azure-VM/)) and openly discusses the challenges faced, including incomplete success but valuable roadblocks and learning outcomes for others attempting similar setups.

## Key Learnings and Considerations

- **Lab/Test Focus:** The guide is primarily for lab and test purposes, where users may not have advanced physical resources.
- **Azure Subscription Flexibility:** You can perform this setup using a pay-as-you-go, student, or Visual Studio subscription.
- **Production vs Lab:** For production workloads requiring high availability and resiliency, Microsoft offers Azure VMware Solution (AVS), which mandates EA or CSP subscriptions.
- **Benefits and Limitations:**
    - Running ESXi on an Azure VM may help those limited by local hardware resources.
    - This approach is not recommended for production due to feature limitations and the experimental nature of nested virtualization in Azure.
- **Unresolved Challenges:** The process did not reach full success, reflecting platform constraints or missing features, but documents all encountered blocks so others can learn or help resolve them.
- **Community Mindset:** The author encourages collaborative problem-solving and welcomes feedback or suggestions for overcoming remaining hurdles.
- **Practical Use Case:** One potential use is to utilize Azure Migrate for VMware, allowing users to test migration scenarios in a lab environment.

## Comparison: ESXi on Azure VM vs AVS

| Factor           | ESXi on Azure VM (Lab)       | Azure VMware Solution (AVS, Production) |
|------------------|-----------------------------|-----------------------------------------|
| Subscription     | Pay-as-you-go, Student, VS  | EA or CSP only                          |
| Reliability      | For testing only            | High availability & resiliency           |
| Target Use       | Labs, tests, learning       | Production workloads                     |

## Conclusion

While deploying ESXi on Azure VMs remains a challenging and partially successful experiment, the documented steps and problems provide valuable knowledge for the community. The effort bridges cloud and virtualization enthusiasts looking to expand their hybrid lab experiences.

Read the full guide to explore detailed instructions and troubleshooting, and contribute suggestions or solutions if you have them.

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mkatqa/esxi_on_azure_vm/)
