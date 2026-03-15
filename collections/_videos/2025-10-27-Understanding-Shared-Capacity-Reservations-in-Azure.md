---
external_url: https://www.youtube.com/watch?v=vxBpbNCLANM
title: Understanding Shared Capacity Reservations in Azure
author: John Savill's Technical Training
feed_name: John Savill's Technical Training
date: 2025-10-27 14:02:29 +00:00
tags:
- Availability Zone
- Availability Zones
- Azure Cloud
- Azure SLA
- Azure Virtual Machines
- Capacity
- Capacity Reservation
- Cloud Architecture
- Microsoft
- Microsoft Azure
- Reservation Sharing
- Resource Management
- Savings Plans
- Subscription Management
- Azure
- Videos
section_names:
- azure
primary_section: azure
---
John Savill provides an in-depth walkthrough of Azure's shared capacity reservations, outlining new scenarios for efficiently using reserved VM capacity across subscriptions.<!--excerpt_end-->

{% youtube vxBpbNCLANM %}

# Understanding Shared Capacity Reservations in Azure

John Savill's Technical Training presents an in-depth look at Azure's shared capacity reservation feature. This guide covers not only the basics of capacity reservation but also new sharing abilities, compatibility, and steps to implement these features in your Azure environments.

## What Are Shared Capacity Reservations?

- **Capacity Reservations** in Azure allow organizations to reserve compute capacity in specific regions and availability zones, ensuring resources are available when needed.
- The new **Sharing Ability** extends these reservations across multiple subscriptions and resource groups, enabling greater flexibility and more efficient capacity utilization.

## Key Topics Covered

- **Capacity Reservation Refresher**: Recap of how reservations work in Azure, distinction between quotas and capacity guarantees.
- **SLA-Backed Guarantees**: Azure provides service-level agreements guaranteeing reserved capacity.
- **Moving Running VMs**: How to migrate existing virtual machines into reserved capacity groups for better predictability.
- **RI and Savings Plan Compatibility**: Discussion of Reserved Instances and Savings Plans compatibility with capacity reservations.
- **Common Issues and Gotchas**: Current limitations (such as the absence of portal support at the time of recording), permission requirements, and how to avoid pitfalls.
- **Sharing Reservations**: Technical details on how reservation sharing is mapped across subscriptions and the scenarios this enables for larger Azure customers.

## Practical Scenarios Unlocked

- Use reserved capacities more flexibly within an organization
- Map capacity reservations across multiple Azure subscriptions
- Improve budget and resource forecasting
- Maintain compliance with organizational boundaries while maximizing resource availability

## Implementation and Documentation Links

- [Azure Capacity Reservation Sharing documentation](https://learn.microsoft.com/azure/virtual-machines/capacity-reservation-group-share)
- [Microsoft SLA documentation](https://www.microsoft.com/licensing/docs/view/Service-Level-Agreements-SLA-for-Online-Services?lang=1)

## Summary

Azure's shared capacity reservation feature is a significant enhancement for resource and subscription management. It enables organizations to take full advantage of capacity guarantees, share resources efficiently, and prepare for scaling requirements without over-provisioning.

For further exploration, refer to the official whiteboard diagrams and video chapters shared by John Savill.
