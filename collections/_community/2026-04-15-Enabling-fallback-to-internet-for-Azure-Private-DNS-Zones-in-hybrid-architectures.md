---
tags:
- AKS
- Az Network Private DNS
- Azure
- Azure CLI
- Azure Container Registry
- Azure DNS
- Azure Firewall
- Azure Key Vault
- Azure Private DNS Zones
- Azure Private Endpoint
- Azure Private Link
- Azure SQL Managed Instance
- Azure Storage
- Community
- Cross Subscription
- Cross Tenant
- ExpressRoute
- Hybrid Networking
- Multi Region Disaster Recovery
- Network Security Group (nsg)
- Nslookup
- NXDOMAIN
- NxDomainRedirect
- Resolutionpolicy
- Service Endpoint Policies
- User Defined Routes (udr)
- Virtual Network Link
- VNet DNS Resolution
- VPN
author: kirankumar_manchiwar04
feed_name: Microsoft Tech Community
title: Enabling fallback to internet for Azure Private DNS Zones in hybrid architectures
primary_section: azure
date: 2026-04-15 15:57:15 +00:00
section_names:
- azure
external_url: https://techcommunity.microsoft.com/t5/azure-networking-blog/enabling-fallback-to-internet-for-azure-private-dns-zones-in/ba-p/4511131
---

kirankumar_manchiwar04 explains how Azure Private DNS Zones can return NXDOMAIN for missing Private Link records in hybrid and multi-region setups, and how enabling the NxDomainRedirect (“fallback to internet”) resolution policy on a VNet link lets Azure retry via public recursive DNS to keep applications connecting.<!--excerpt_end-->

# Enabling fallback to internet for Azure Private DNS Zones in hybrid architectures

Azure Private DNS Zones enable name resolution for Azure PaaS services integrated with Private Endpoints by redirecting service FQDNs to a private DNS zone such as `privatelink.database.windows.net`. In Private Link–enabled environments, DNS queries from Azure Virtual Networks are resolved using the linked private DNS zone.

A common failure mode is when the required DNS record is missing (for example in multiregion, cross-subscription, or hybrid connectivity scenarios). In that case, the resolver can return an `NXDOMAIN` response, which can lead to application connectivity failures.

Azure provides a native DNS resolution policy called `NxDomainRedirect` (shown in the portal as **Enable fallback to internet**). It allows unresolved queries (that result in authoritative `NXDOMAIN` from the Private Link zone scope) to be retried using Azure’s public recursive DNS resolvers.

This configuration is applied at the **virtual network link** level of the private DNS zone.

## Introduction

Azure Private Endpoint enables secure connectivity to Azure PaaS services such as:

- Azure SQL Managed Instance
- Azure Container Registry
- Azure Key Vault
- Azure Storage Account

When Private Endpoint is enabled for a service, Azure DNS changes the name resolution path using **CNAME redirection**.

Example:

- `myserver.database.windows.net`
- → `myserver.privatelink.database.windows.net`
- → Private IP

Azure Private DNS Zones are then used to resolve the Private Endpoint FQDN within the VNet.

## The DNS limitation in Private Link environments

This can introduce a critical DNS limitation in scenarios like:

- Hybrid cloud architectures (AWS → Azure SQL MI)
- Multiregion deployments (DR region access)
- Cross-tenant / cross-subscription access
- Multi-VNet isolated networks

If the Private DNS zone does not contain a corresponding record, Azure DNS returns:

- `NXDOMAIN` (NonExistent Domain)

When a DNS resolver receives a negative response (`NXDOMAIN`), it sends no DNS response to the DNS client and the query fails.

This can result in:

- Application connectivity failure
- Database connection timeout
- AKS pod DNS resolution errors
- DR failover application outage

## Problem statement

In traditional Private Endpoint DNS resolution:

1. A DNS query is sent from the application.
2. Azure DNS checks the linked Private DNS Zone.
3. If no matching record exists, `NXDOMAIN` is returned.

Azure does not retry resolution using public DNS by default, so:

- Public endpoint resolution never occurs
- The DNS query fails permanently
- The application cannot connect

## Microsoft native solution: NxDomainRedirect (fallback to internet)

Azure introduced a DNS resolution policy:

- `resolutionPolicy = NxDomainRedirect`

This enables public recursion via Azure’s recursive resolver fleet when an authoritative `NXDOMAIN` response is received for a Private Link zone.

When enabled:

- Azure DNS retries the query
- Public endpoint resolution occurs
- Application connectivity continues
- No custom DNS forwarder is required

Where it’s configured:

- Private DNS Zone → **Virtual network links**
- Enable at the **virtual network link level** with the `NxDomainRedirect` setting

In the Azure portal this appears as:

- **Enable fallback to internet**

## How it works

### Without fallback

- Application → Azure DNS
- → Private DNS Zone
- → Record missing
- → `NXDOMAIN` returned
- → Connection failure

### With fallback enabled

- Application → Azure DNS
- → Private DNS Zone
- → Record missing
- → `NXDOMAIN` returned
- → Azure recursive resolver
- → Public DNS resolution
- → Public endpoint IP returned
- → Connection successful

Azure recursive resolver retries the query using the public endpoint QNAME each time `NXDOMAIN` is received from the private zone scope.

## Real world use case: AWS app connecting to Azure SQL Managed Instance

You are running:

- SQL MI in Azure
- Private Endpoint enabled
- Private DNS Zone: `privatelink.database.windows.net`

An AWS application tries to connect to:

- `my-mi.database.windows.net`

If a DR region DNS record is not available:

- Without fallback: DNS query → `NXDOMAIN` → app failure
- With fallback enabled: DNS query → retry public DNS → connection success

## Step-by-step configuration

## Method 1 – Azure portal

1. Go to **Private DNS Zones**.
2. Select your Private Link DNS Zone (example: `privatelink.database.windows.net`).
3. Select **Virtual network links**.
4. Open your linked VNet.
5. Enable **Enable fallback to internet**.
6. Click **Save**.

## Method 2 – Azure CLI

Configure fallback policy using:

```bash
az network private-dns link vnet update \
  --resource-group RG-Network \
  --zone-name privatelink.database.windows.net \
  --name VNET-Link \
  --resolution-policy NxDomainRedirect
```

## Validation steps

Run from an Azure VM:

```bash
nslookup my-mi.database.windows.net
```

Expected:

- Private IP (if available)
- Public IP (if fallback triggered)

## Security considerations

Enabling fallback to internet:

- Does not expose data
- Only impacts DNS resolution
- Network traffic is still governed by:
  - NSG
  - Azure Firewall
  - UDR
  - Service Endpoint Policies

Fallback only triggers on `NXDOMAIN` and does not change network-level firewall controls.

## When should you enable this?

Recommended in:

- Hybrid AWS → Azure connectivity
- Multiregion DR deployments
- AKS accessing Private Endpoint services
- Cross-tenant connectivity
- Private Link + VPN / ExpressRoute scenarios

## Conclusion

Fallback to internet using `NxDomainRedirect` provides:

- Seamless hybrid connectivity
- Reduced DNS complexity
- No custom forwarders
- Improved application resilience

It simplifies DNS resolution for modern Private Endpoint–enabled architectures.

[Read the entire article](https://techcommunity.microsoft.com/t5/azure-networking-blog/enabling-fallback-to-internet-for-azure-private-dns-zones-in/ba-p/4511131)

