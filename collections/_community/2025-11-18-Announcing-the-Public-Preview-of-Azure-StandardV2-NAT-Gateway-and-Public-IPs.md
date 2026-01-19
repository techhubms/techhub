---
external_url: https://techcommunity.microsoft.com/t5/azure-networking-blog/announcing-the-public-preview-of-standardv2-nat-gateway-and/ba-p/4458292
title: Announcing the Public Preview of Azure StandardV2 NAT Gateway and Public IPs
author: aimeelittleton
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-11-18 17:30:00 +00:00
tags:
- ARM Template
- Azure NAT Gateway
- Bicep
- CLI
- Cloud Networking
- Flow Logs
- High Availability
- IPv6
- Network Security
- Outbound Connectivity
- PowerShell
- Public IP
- Resiliency
- StandardV2
- Virtual Network
- Zone Redundancy
section_names:
- azure
---
Aimee Littleton introduces the public preview of Azure StandardV2 NAT Gateway and StandardV2 public IPs, detailing new features like zone redundancy, enhanced throughput, and flow logs for increased resiliency and visibility.<!--excerpt_end-->

# Announcing the Public Preview of Azure StandardV2 NAT Gateway and Public IPs

**Author: Aimee Littleton**

In today's fast-evolving digital landscape, it's crucial for organizations to deliver resilient, always-available cloud-native experiences. The evolution of the Azure NAT Gateway—now in StandardV2 SKU—brings significant improvements for outbound connectivity, network resiliency, and performance in Azure virtual networks.

## Why Zone Resiliency Matters

Cloud applications must remain available even in the event of an availability zone failure. The StandardV2 NAT Gateway is zone redundant by default, operating across multiple zones to ensure ongoing outbound connectivity even if a zone goes down. For example, if one zone experiences an outage, new outbound connections can use other healthy zones, maintaining uptime and business continuity. [Learn more about zone redundancy.](https://learn.microsoft.com/en-us/azure/nat-gateway/nat-availability-zones#standardv2-sku-nat-gateway-zone-redundant)

## Key Features of StandardV2 NAT Gateway

- **Zone Redundancy**: Default operation across multiple availability zones in supported regions. Provides seamless failover for outbound network traffic.
- **High Performance**: Supports up to 100 Gbps throughput and 10 million packets per second, making it suitable for demanding and latency-sensitive workloads. [Performance details.](https://learn.microsoft.com/en-us/azure/nat-gateway/nat-gateway-resource#performance)
- **Dual-Stack Support (IPv4/IPv6)**: Addresses modern networking needs and regulatory requirements, each resource supporting up to 16 IPv4 and 16 IPv6 StandardV2 public IP addresses or prefixes. [Learn more about IPv6 support.](https://learn.microsoft.com/en-us/azure/nat-gateway/nat-sku#ipv6-support)
- **Flow Logs**: Capture detailed IP-level traffic for troubleshooting, identifying top talkers, compliance, and security auditing. [Enabling flow logs.](https://aka.ms/natflowlogs)
- **New Public IP SKU**: StandardV2 public IP addresses and prefixes, currently in public preview and required for StandardV2 NAT Gateway deployments. [IP deployment guide.](https://learn.microsoft.com/en-us/azure/virtual-network/ip-services/create-public-ip-portal#create-a-standardv2-sku-public-ip-address)

## Deployment Guidance

StandardV2 NAT Gateway and StandardV2 Public IPs can be provisioned using ARM templates, Bicep, PowerShell, or CLI. Portal and Terraform support will be available soon.

**Deployment Links:**

- [Quickstart: Create StandardV2 NAT Gateway](https://learn.microsoft.com/en-us/azure/nat-gateway/quickstart-create-nat-gateway-v2)
- [Deploy StandardV2 public IPs](https://learn.microsoft.com/en-us/azure/virtual-network/ip-services/create-public-ip-portal?tabs=option-1-create-public-ip-standardv2)
- [Known Limitations](https://learn.microsoft.com/en-us/azure/nat-gateway/nat-sku#known-limitations)

## When to Adopt StandardV2 NAT Gateway

- If you require enhanced availability across Azure regions with multiple zones
- For applications demanding high or consistent outbound throughput
- When regulatory or operational needs call for IPv6 support
- If your organization values detailed outbound traffic observability

## Additional Resources

- [StandardV2 NAT Gateway Documentation](https://learn.microsoft.com/en-us/azure/nat-gateway/nat-overview#standardv2-nat-gateway)
- [Public IP SKUs](https://learn.microsoft.com/en-us/azure/virtual-network/ip-services/public-ip-addresses#sku)
- [Pricing Details](https://azure.microsoft.com/en-us/pricing/details/azure-nat-gateway/)

## Conclusion

Azure's StandardV2 NAT Gateway provides a future-ready solution for organizations needing scalable, reliable, and observable outbound internet connectivity. By leveraging zone redundancy, dual-stack networking, and enhanced traffic visibility, you can help ensure the high availability and compliance of your cloud-native workloads.

---
*Updated: Nov 14, 2025 (Version 1.0)*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-networking-blog/announcing-the-public-preview-of-standardv2-nat-gateway-and/ba-p/4458292)
