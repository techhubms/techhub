---
title: Private DNS and Hub–Spoke Networking for Enterprise AI Workloads on Azure
author: deepthihr
feed_name: Microsoft Tech Community
section_names:
- ai
- azure
- security
primary_section: ai
tags:
- .internal Domain
- 168.63.129.16
- AI
- Azure
- Azure Active Directory
- Azure AI Search
- Azure Cognitive Services
- Azure Container Apps
- Azure Firewall
- Azure Key Vault
- Azure Monitor
- Azure OpenAI
- Azure Private DNS
- Azure Recursive Resolver
- Azure Service Tags
- Azure Storage
- Azure Virtual Network
- Community
- Conditional Forwarders
- Custom DNS Servers
- DNS Resolution
- Hub And Spoke Networking
- Internal Load Balancer
- Microsoft Entra ID
- Network Security Groups (nsg)
- Network Virtual Appliance (nva)
- Private DNS Zone Links
- Private Endpoints
- Security
- Terraform
- User Defined Routes (udr)
date: 2026-04-06 07:56:39 +00:00
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/private-dns-and-hub-spoke-networking-for-enterprise-ai-workloads/ba-p/4508835
---

deepthihr walks through a real production incident running a private, enterprise AI platform on Azure, showing how DNS and private networking gaps (custom DNS, Private Endpoints, and Azure Container Apps internal ingress) caused intermittent failures—and the concrete fixes that stabilized the environment.<!--excerpt_end-->

# Private DNS and Hub–Spoke Networking for Enterprise AI Workloads on Azure

## Introduction

As organizations deploy enterprise AI platforms on Azure, security requirements increasingly drive the adoption of private-first architectures:

- Private networking only
- Centralized firewalls or NVAs
- Hub–and–spoke virtual network architectures
- Private Endpoints for all PaaS services

While these patterns are well understood individually, their interaction often exposes hidden failure modes—especially around DNS and name resolution.

During a recent production deployment of a private, enterprise-grade AI workload on Azure, several issues surfaced that initially looked like platform instability. After investigation, the root cause was gaps in network and DNS design.

This post provides a technical walkthrough of the problem, root causes, resolution steps, and lessons learned as a reusable blueprint for running AI workloads reliably in private Azure environments.

## Problem statement

The platform was deployed with:

- Hub-and-spoke topology
- Custom DNS servers running in the hub
- Firewall / NVA enforcing strict egress controls
- AI, data, and platform services exposed through Private Endpoints
- Azure Container Apps using internal load balancer mode
- Centralized monitoring, secrets, and identity services

Even though infrastructure deployment succeeded, production issues were non-deterministic:

- Container Apps intermittently failing to start or scale
- AI platform endpoints becoming unreachable from workload subnets
- Authentication and secret access failures
- DNS resolution working in some environments but failing in others
- Terraform deployments stalling or failing unexpectedly

Because symptoms varied across subnets and environments, finding the root cause was not straightforward.

## Root cause analysis: DNS resolution in a private Azure environment

After end-to-end isolation, the issue was not AI services, authentication, or application logic. The core problem was DNS.

### 1) Custom DNS servers were not Azure-aware

The hub DNS servers could resolve:

- Corporate domains
- On-premises records

But they could not resolve Azure platform names or Private Endpoint FQDNs by default.

Azure relies on an internal recursive resolver at `168.63.129.16` that must be explicitly integrated when you use custom DNS.

### 2) Missing conditional forwarders for private DNS zones

Many Azure services rely on private DNS zones such as:

- `privatelink.cognitiveservices.azure.com`
- `privatelink.openai.azure.com`
- `privatelink.vaultcore.azure.net`
- `privatelink.search.windows.net`
- `privatelink.blob.core.windows.net`

Without conditional forwarders to Azure’s internal DNS:

- Queries failed silently, or
- Resolved to public endpoints that were blocked by firewall rules

### 3) Azure Container Apps internal DNS requirements were overlooked

When Azure Container Apps are deployed with internal ingress:

```hcl
internal_load_balancer_enabled = true
```

Azure does not automatically create supporting DNS records.

The environment generates:

- A default domain
- `.internal` subdomains for internal FQDNs

If you don’t explicitly create:

- A private DNS zone matching the default domain
- Wildcard records: `*`, `@`, and `*.internal`

Then internal service-to-service communication fails.

### 4) Private DNS zones were not consistently linked

Even when zones existed, they were:

- Spread across multiple subscriptions
- Linked to some VNets but not others
- Missing links to DNS server VNets or shared services VNets

So resolution depended on where a lookup originated, leading to “works here, fails there” behavior.

## Resolution (no application changes required)

Stability was achieved through architectural corrections.

### Step 1: Make custom DNS Azure-aware

On all custom DNS servers (or NVAs acting as DNS proxies):

- Configure conditional forwarders for Azure private DNS zones
- Forward those queries to `168.63.129.16`

This resolver is mandatory for Private Endpoint resolution.

### Step 2: Centralize and link private DNS zones

Adopt a centralized private DNS model:

- Host all private DNS zones in a shared subscription
- Link zones to:
  - Hub VNet
  - All spoke VNets
  - DNS server VNet
  - Operational / virtual desktop VNets (as needed)

This makes name resolution consistent regardless of workload placement.

### Step 3: Explicitly handle Container Apps DNS

For Container Apps using internal ingress:

- Create a private DNS zone matching the environment’s default domain
- Add records:
  - `*` wildcard record
  - `@` apex record
  - `*.internal` wildcard record
- Point the records to the Container Apps Environment static IP
- If using custom DNS, add a conditional forwarder for the default domain

This resolves internal connectivity issues for service-to-service calls.

### Step 4: Align routing, NSGs, and service tags

Align firewall, NSG, and route table rules to:

- Allow DNS traffic (`TCP/UDP 53`)
- Allow required Azure service tags, such as:
  - `AzureCloud`
  - `CognitiveServices`
  - `AzureActiveDirectory`
  - `Storage`
  - `AzureMonitor`
- Ensure some subnets (for example, Container Apps and Application Gateway) keep direct internet access where required by Azure platform services

## Key learnings

1. **DNS is a Tier-0 dependency for AI platforms**
   - Many “service issues” are actually DNS failures.

2. **Private Endpoints require Azure DNS integration**
   - If you use custom DNS and Private Endpoints, forwarding to `168.63.129.16` is non-negotiable.

3. **Container Apps internal ingress has hidden DNS requirements**
   - Internal environments need manually created zones and `.internal` records.

4. **Centralized DNS prevents environment drift**
   - Subscription-local DNS zones often lead to inconsistent behavior.

5. **Validate networking first, then the platform**
   - Before escalating to service teams, validate DNS resolution, routing, and Private Endpoint connectivity.

## Quick production validation checklist

Before go-live:

1. Private FQDNs resolve to private IPs from all required VNets
2. UDR/NSG rules allow required Azure service traffic
3. Managed identities can access all dependent resources
4. AI portal user workflows succeed (evaluations, agents, etc.)
5. `terraform plan` shows only intended changes

## Conclusion

Running private, enterprise-grade AI workloads on Azure is achievable, but requires intentional DNS and networking design.

Key actions:

- Make custom DNS Azure-aware
- Centralize private DNS zones
- Explicitly handle Container Apps DNS
- Align routing and firewall rules

Done correctly, an unstable environment becomes a repeatable, production-ready platform pattern.


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/private-dns-and-hub-spoke-networking-for-enterprise-ai-workloads/ba-p/4508835)

