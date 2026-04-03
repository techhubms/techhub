---
title: Announcing general availability of Network Security Perimeter for Azure Service Bus
primary_section: azure
author: EldertGrootenboer
external_url: https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/announcing-general-availability-of-network-security-perimeter/ba-p/4508179
section_names:
- azure
- security
date: 2026-04-03 16:31:50 +00:00
tags:
- Access Logs
- Audit Logging
- Azure
- Azure Key Vault
- Azure PaaS
- Azure Portal
- Azure Service Bus
- CMK
- Community
- Compliance
- Customer Managed Keys
- Defense in Depth
- FQDN
- Inbound Access Rules
- IP Firewall Rules
- Network Security Perimeter
- NSP
- Outbound Access Rules
- Private Endpoints
- Private Link
- Public Network Access
- Security
- VNet Service Endpoints
feed_name: Microsoft Tech Community
---

EldertGrootenboer announces the general availability of Network Security Perimeter (NSP) support for Azure Service Bus, explaining how it complements existing network controls and how to roll it out safely using transition and enforced modes.<!--excerpt_end-->

# Announcing general availability of Network Security Perimeter for Azure Service Bus

Today we are excited to announce the general availability of [Network Security Perimeter](https://learn.microsoft.com/azure/private-link/network-security-perimeter-concepts) (NSP) support for Azure Service Bus.

Network Security Perimeter lets you define a logical network boundary around your Service Bus namespaces and other Azure PaaS resources to:

- Restrict public network access
- Enable secure communication between services within the perimeter

This builds on existing [network security options for Service Bus](https://learn.microsoft.com/azure/service-bus-messaging/network-security):

- IP firewall rules
- VNet service endpoints
- Private endpoints

NSP adds centralized, perimeter-level control over which resources can communicate with each other.

## How Network Security Perimeter fits with existing network security

Service Bus already offers several ways to control network access:

- **IP firewall rules** restrict access to specific IPv4 addresses.
- **VNet service endpoints** and **private endpoints** route Service Bus traffic over the Microsoft backbone network, avoiding the public internet.

These options are configured at the **individual namespace** level.

**Network Security Perimeter** takes a perimeter-first approach:

- You create a perimeter and associate PaaS resources with it.
- By default, resources *inside* the perimeter can communicate with each other.
- Public access from *outside* the perimeter is denied by default.
- You define explicit **inbound** and **outbound** access rules for traffic that must cross the perimeter boundary.

This allows you to manage a Service Bus namespace, the Azure Key Vault it uses for customer-managed keys, and other related resources under a consistent set of network rules.

### NSP and private endpoints (defense in depth)

NSP is described as complementary to private endpoints:

- **Private endpoints** secure traffic between your virtual network and Service Bus.
- **Network Security Perimeter** secures the *public endpoint* of Service Bus.

Used together, they provide defense-in-depth for messaging infrastructure.

## Concepts

Network Security Perimeter uses **profiles** and **access rules**:

- A **profile** is a collection of access rules applied to associated resources.
- You can use different profiles in the same perimeter to apply different rule sets to different groups of resources.

### Access modes

- **Transition mode** (default when first associating a resource):
  - Logs access attempts
  - Does not enforce restrictions
  - Intended to help you understand existing traffic patterns before enforcing rules
- **Enforced mode**:
  - Denies all traffic from outside the perimeter by default
  - Allows traffic only when an explicit access rule permits it

## Access rules

Access rules control traffic that crosses the perimeter boundary:

- **Inbound rules**: allow traffic from specific IP address ranges or Azure subscriptions to reach your Service Bus namespace.
- **Outbound rules**: allow your Service Bus namespace to communicate with external resources identified by fully qualified domain names (FQDNs).

Within the perimeter, **PaaS-to-PaaS communication is allowed by default** without additional rules.

## Supported scenarios

NSP for Service Bus supports:

- **Customer-managed keys (CMK)**:
  - Service Bus namespaces using CMK must communicate with **Azure Key Vault**.
  - Placing both Service Bus and Key Vault in the same perimeter secures this communication without extra network configuration.
- **Diagnostic logging**:
  - NSP provides access logs recording every allowed or denied connection attempt.
  - These logs support audit and compliance needs by showing what accessed the namespace and from where.

## Getting started (Azure portal)

You can associate a Service Bus namespace with a Network Security Perimeter from the Azure portal:

1. On your Service Bus namespace page, select **Networking** under **Settings**.
2. Select the **Public access** tab.
3. In the **Network security perimeter** section, select **Associate**.
4. In the **Select network security perimeter** dialog, search for and select the perimeter to associate.
5. Select a profile to associate with the namespace.
6. Select **Associate** to complete the association.

Recommendation: start in **transition mode** to understand current traffic patterns, then move to **enforced mode** after configuring access rules.

More information: [Network security perimeter for Azure Service Bus](https://learn.microsoft.com/azure/service-bus-messaging/network-security-perimeter)


[Read the entire article](https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/announcing-general-availability-of-network-security-perimeter/ba-p/4508179)

