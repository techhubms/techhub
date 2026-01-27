---
external_url: https://devops.com/the-most-destructive-cloud-cost-pitfall-discounts-before-optimization/
title: 'Avoiding Cloud Cost Traps: Optimize Workloads Before Negotiating Discounts'
author: Andrew Hillier
feed_name: DevOps Blog
date: 2025-11-06 14:43:11 +00:00
tags:
- Autoscaling
- Cloud
- Cloud Cost
- Cloud Discounts
- Cloud Economics
- Cloud Infrastructure
- Cloud Optimization
- Containerization
- Contributed Content
- CPU Usage
- DevOps Practices
- Financial Commitment
- FinOps
- Infrastructure Management
- Kubernetes
- Memory Management
- Reserved Capacity
- Resource Allocation
- Right Sizing
- Social Facebook
- Social LinkedIn
- Social X
- Workload Analysis
section_names:
- devops
primary_section: devops
---
Andrew Hillier reveals why securing cloud discounts before optimizing workloads can lock in inefficiency, especially in Kubernetes environments, and shares actionable collaboration strategies for engineering and finance teams.<!--excerpt_end-->

# Avoiding Cloud Cost Traps: Optimize Workloads Before Negotiating Discounts

**Author: Andrew Hillier**

Organizations often try to cut cloud expenses by securing discount programs first—committing to reserved capacity or enterprise agreements to lower their rates. Although this appears to deliver savings, doing so before workload optimization can create long-term inefficiencies and waste.

## Why Discount-First Approaches Cause Problems

Discounts are attractive because they’re simple for finance teams to negotiate, without requiring deep technical input from engineering. But if workloads—such as Kubernetes clusters—are not right-sized, discounts lock in oversized infrastructure. For example:

- **Kubernetes containers** often over-request CPU and memory out of caution, leading clusters to appear full but remain underutilized.
- **AI jobs** may reserve expensive GPU resources that sit idle, driving up costs.
- Lower unit costs sound positive, but if the number of units is inflated, total spending remains unnecessarily high.

## Real-World Proof: Kubernetes Cost Management

Surveys show Kubernetes is a major driver of cloud spend, but most organizations struggle to optimize their clusters. Inflated resource requests prompt autoscaling, increasing server counts and spend without matching value. Developers frequently ask for more computing power than required, making clusters inefficient.

## Collaboration Is Key to Optimization

Optimizing cloud workloads demands cooperation between finance, engineering, and application teams. Finance may negotiate deals, but only engineers have the technical evidence to adjust configurations wisely. Siloed teams often settle for quick wins from discounts, ignoring operational waste.

The [FinOps Foundation’s State of FinOps 2025 report](https://www.usu.com/en/blog/state-of-finops-2025#:~:text=50%25%20say%20workload%20optimization/waste,unit%20economics%20%28+5%20places%29) highlights that workload optimization and waste reduction remain the top challenge in cloud cost management.

## Best Practices: Optimize Before Committing

To avoid locking in inefficiency:

- **Track actual resource usage**: Monitor real CPU, memory, and GPU consumption—not just provisioned resources.
- **Analyze true requirements**: Align Kubernetes requests and limits with actual utilization.
- **Negotiate discounts only after optimization**: Base commitments on proven resource needs, favoring flexible plans that support evolving workloads.
- **Automate optimization**: Use automation in container environments to adjust thousands of pods efficiently.
- **Foster cross-team collaboration**: Engineers understand workload behavior, finance manages contracts—combine evidence to guide decisions.
- **Adjust commitments as needed**: Use instruments that support conversion or flexible discounts to match optimized usage.

Discounts should amplify efficiency, not compensate for technical debt. By right-sizing first, organizations set a foundation for sustainable growth rather than perpetual overspending.

## Additional Resources & Links

- [KubeCon + CloudNativeCon North America 2025 Registration](https://events.linuxfoundation.org/kubecon-cloudnativecon-north-america/register/)
- [DevOps.com Article](https://devops.com/the-most-destructive-cloud-cost-pitfall-discounts-before-optimization/)

---

*Cloud economics is about operational discipline—not just procurement. Sustainable cost optimization requires technical and financial alignment before making cloud commitments.*

This post appeared first on "DevOps Blog". [Read the entire article here](https://devops.com/the-most-destructive-cloud-cost-pitfall-discounts-before-optimization/)
