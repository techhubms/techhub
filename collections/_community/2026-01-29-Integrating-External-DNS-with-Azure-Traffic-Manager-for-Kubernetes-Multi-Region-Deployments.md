---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/exploring-traffic-manager-integration-for-external-dns/ba-p/4485690
title: Integrating External DNS with Azure Traffic Manager for Kubernetes Multi-Region Deployments
author: samcogan
primary_section: dotnet
feed_name: Microsoft Tech Community
date: 2026-01-29 08:43:34 +00:00
tags:
- AKS
- Annotation Driven Configuration
- Azure
- Azure DNS
- Azure Traffic Manager
- Blue Green Deployment
- Cloud Native
- Community
- DevOps
- Disaster Recovery
- DNS Automation
- External DNS
- Global Load Balancing
- Go SDK
- Health Checks
- Kubernetes
- Multi Region Deployment
- Network Architecture
- Webhook Provider
- Weighted Routing
- .NET
section_names:
- azure
- dotnet
- devops
---
Sam Cogan presents a practical proof-of-concept for automating Azure Traffic Manager configuration using External DNS and Kubernetes, enabling multi-region DNS-based load balancing and streamlined deployment workflows.<!--excerpt_end-->

# Integrating External DNS with Azure Traffic Manager for Kubernetes Multi-Region Deployments

Author: Sam Cogan

## Overview

When deploying externally accessible applications on Kubernetes (AKS), managing DNS records is fundamental. Rather than handling DNS records manually, [External DNS](https://github.com/kubernetes-sigs/external-dns) can automate this process, especially for Azure DNS. However, to enable global traffic distribution and advanced routing across multiple clusters, this guide explores automating [Azure Traffic Manager](https://learn.microsoft.com/azure/traffic-manager) with a custom External DNS provider.

## Challenge

While External DNS works well for standard DNS zones, it does not natively support Azure Traffic Manager, which is a DNS-based global load balancer. Traffic Manager enables scenarios such as:

- **Geographic Routing:** Send users to the nearest endpoint
- **Weighted Distribution:** Distribute traffic by percentage
- **Priority Failover:** Disaster recovery with primary/backup
- **Performance-Based Routing:** Route to lowest latency endpoint

## Proof of Concept: Webhook Provider for Traffic Manager

This article details the creation of a Webhook provider for External DNS, capable of:

- Watching Kubernetes Services with Traffic Manager annotations
- Automatically creating/managing Traffic Manager profiles and endpoints
- Syncing state between Kubernetes clusters and Azure
- Handling duplicate creations, state, and configuration via annotations
- Operating alongside the regular Azure DNS provider

> **Important:** This is a proof of concept, not a production-ready solution. See the [GitHub project](https://github.com/sam-cogan/external-dns-traffic-manager) and [readme](https://github.com/sam-cogan/external-dns-traffic-manager/blob/main/README.md) for full deployment steps.

## Example: Multi-Region Weighted Routing

**Scenario:** Deploy an app across East US and West US with 70% traffic to East US, 30% to West US.

1. **Deploy Your App**: Standard Kubernetes `Service` type LoadBalancer with special annotations.
2. **Apply Traffic Manager Annotations:**
   - Enable integration using `external-dns.alpha.kubernetes.io/webhook-traffic-manager-enabled: "true"`
   - Specify resource group, profile name, endpoints, weights, and health check settings in Service annotations.
   - Example (YAML snippet shown in article)
3. **Automatic Resource Creation:**
   - Azure Load Balancer assigns public IPs
   - External DNS (Azure provider) creates A records for each region
   - External DNS (Webhook provider) creates Traffic Manager profile + endpoints
   - Webhook synchronizes state, creates a CNAME alias
4. **Live Traffic Management:**
   - Dynamically update weights by adjusting annotations (`kubectl annotate ...`).
   - Enables canary, blue-green, and geo-routing strategies.
5. **Failover:**
   - Traffic Manager health checks handle automatic failover if one region becomes unhealthy.

## Technical Implementation

- Webhook implements External DNS webhook provider protocol:
  1. **Capability Negotiation**
  2. **Endpoint Adjustment & Filtering**
  3. **State Synchronization**
  4. **Apply Changes via Azure Go SDK**
- In-memory state (PoC limitation)
- Managed via standard annotations and GitOps friendly workflows

## Use Cases

- Multi-Region High Availability (no extra load balancers)
- Blue-Green Deployments, Canary Releases
- Geo-Distributed Routing
- Disaster Recovery with minimal manual intervention
- Cost Optimization by adjusting traffic flow

## Limitations & Considerations

- Proof of concept; not production supported
- In-memory state—no persistence
- Basic error handling and observability
- Requires manual cleanup on provider switch
- Future improvements could include metric-based routing, advanced health checks, additional endpoint types, deeper GitOps/Helm integration

## Resources

- [GitHub: external-dns-traffic-manager](https://github.com/sam-cogan/external-dns-traffic-manager)
- [External DNS Docs](https://kubernetes-sigs.github.io/external-dns/)
- [Azure Traffic Manager](https://learn.microsoft.com/azure/traffic-manager)
- [Webhook Provider Tutorial](https://github.com/kubernetes-sigs/external-dns/blob/master/docs/tutorials/webhook-provider.md)

## Conclusion

Bringing Azure Traffic Manager under annotation-driven External DNS control opens up robust DNS automation and multi-region Kubernetes deployment options. While this solution is for experimentation and learning, it demonstrates a flexible pattern for integrating global load balancing with standard Kubernetes operations.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/exploring-traffic-manager-integration-for-external-dns/ba-p/4485690)
