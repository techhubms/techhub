---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/capacity-planning-with-azure-container-apps-workload-profiles/ba-p/4477085
title: Capacity Planning with Azure Container Apps Workload Profiles
author: nesubram
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-12-11 05:23:42 +00:00
tags:
- Autoscaling
- Azure Container Apps
- Capacity Planning
- Cloud Infrastructure
- High Availability
- Kubernetes
- Microsoft Azure
- Node Resources
- Replica Placement
- Resource Modeling
- Scaling Strategy
- VM SKU
- Workload Profiles
section_names:
- azure
---
nesubram guides you through best practices for capacity planning in Azure Container Apps, clarifying key concepts like node resources, replica scheduling, and practical autoscaling strategies.<!--excerpt_end-->

# Capacity Planning with Azure Container Apps Workload Profiles

Azure Container Apps (ACA) makes it easier to run containers without managing Kubernetes directly. However, understanding how to plan for capacity is essential to control costs, ensure reliability, and avoid resource shortages.

## Workload Profiles in ACA

ACA currently supports three main workload profiles:

- **Consumption:**
  - Scales to zero when idle for cost savings
  - Platform chooses the node size
  - You pay per replica's execution time
- **Dedicated:**
  - You select the VM SKU (e.g., D4 = 4 vCPU, 16 GiB RAM per node)
  - Pricing is per node
- **Flex (Preview):**
  - Offers dedicated isolation with consumption-style billing flexibility

Each profile determines node-level resources (e.g., D4 implies 4 vCPU, 16 GiB RAM per node).

## How Replicas Consume Node Resources

ACA applications run as containers in managed Kubernetes pods. Remember:

- **Node** = a VM with a fixed set of resources (CPU/RAM)
- **Replica** = a pod scheduled onto a node, sharing node resources with other replicas

**Packing Example:**

- Node SKU: D4 (4 vCPU, 16 GiB RAM)
- Each replica requests: 1 vCPU, 2 GiB RAM
- 5 replicas needed → Total need: 5 vCPU, 10 GiB RAM
- ACA fits 4 replicas on Node 1, then launches a second node for the 5th replica

## When ACA Adds Nodes

ACA automatically adds nodes:

- If new replicas can't fit on existing node(s) due to CPU/memory limits
- When resource requests exceed what's available

ACA follows Kubernetes scheduling—nodes are added when pods can't be scheduled.

## Practical Sizing Strategy

1. **Estimate peak workload** and size each replica (CPU/memory)
2. **Pick the right profile/SKU** (e.g., D4)
3. **Calculate packing:** node capacity ÷ replica request = max replicas per node
4. Add **20% headroom** for unexpected increases
5. Configure autoscaling:
   - Minimum replicas for high availability
   - Maximum replicas for burst handling
   - Minimum/maximum node counts for cost management

## Common Misconceptions

- **Myth:** Replicas have isolated CPU/RAM automatically.
  - **Reality:** All replicas on a node share its resources up to configured limits.
- **Myth:** ACA scales nodes based on overall CPU time used.
  - **Reality:** ACA adds nodes only when replicas can’t be scheduled due to resource constraints. Replica count is driven by application load and scale triggers

## Key Takeaways

- Understand how replicas pack onto nodes before setting scaling limits
- Plan for zero-downtime upgrades (temporarily double your replica count)
- Monitor autoscaler behavior—defaults might not fit your exact workload

**Resources:**

- [Ask questions on Azure forums](https://social.msdn.microsoft.com/forums/azure/en-US/home?forum=windowsazurewebsitespreview)
- [Provide ACA product feedback](https://feedback.azure.com/forums/169385-web-apps)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/capacity-planning-with-azure-container-apps-workload-profiles/ba-p/4477085)
