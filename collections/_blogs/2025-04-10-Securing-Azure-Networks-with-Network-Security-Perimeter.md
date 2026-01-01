---
layout: "post"
title: "Securing Azure Networks with Network Security Perimeter"
description: "This blog post by Jere Haavisto explains Microsoft's new Network Security Perimeter (NSP) for Azure, describing its advantages over traditional network security approaches like NSGs, Private Endpoints, and virtual networks. The article provides demonstrations securing PaaS resources, managing inbound and outbound rules, and details supported Azure services."
author: "jere.haavisto@zure.com (Jere Haavisto)"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://zure.com/blog/securing-azure-networks-with-network-security-perimeter"
viewing_mode: "external"
feed_name: "Zure Data & AI Blog"
feed_url: "https://zure.com/blog/rss.xml"
date: 2025-04-10 04:40:00 +00:00
permalink: "/2025-04-10-Securing-Azure-Networks-with-Network-Security-Perimeter.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Azure Key Vault", "Azure Network Security Perimeter", "Azure SQL", "Azure Storage", "Blog", "Blogs", "Cloud Security", "Cosmos DB", "Event Hubs", "Inbound Rules", "Network Security", "Network Security Groups", "NSP", "Outbound Rules", "PaaS Security", "Private Endpoints", "Security", "Security Posture", "Virtual Networks"]
tags_normalized: ["azure", "azure key vault", "azure network security perimeter", "azure sql", "azure storage", "blog", "blogs", "cloud security", "cosmos db", "event hubs", "inbound rules", "network security", "network security groups", "nsp", "outbound rules", "paas security", "private endpoints", "security", "security posture", "virtual networks"]
---

Jere Haavisto presents a hands-on guide to enhancing Azure network security using the Network Security Perimeter, with practical demos, supported resource lists, and configuration tips.<!--excerpt_end-->

# Securing Azure Networks with Network Security Perimeter

**Author:** Jere Haavisto

In modern Azure environments, it's common to find platform resources like Storage Accounts, Key Vaults, and SQL Servers exposed to the internet due to the complexity of configuring secure network boundaries. Traditionally, this required Virtual Networks, Network Security Groups (NSGs), and Private Endpoints for every resource—a cumbersome and error-prone process.

## Introducing Network Security Perimeter (NSP)

Microsoft introduced Network Security Perimeter (NSP) in November 2024. NSP allows you to define a single set of security rules for multiple PaaS resources, simplifying the configuration and reducing the risk of misconfiguration.

**Supported Azure resources (as of March 2025):**

- Azure Monitor
- Azure AI Search
- Cosmos DB
- Event Hubs
- Key Vault
- SQL Database
- Storage Accounts

Network Security Perimeter is generally available across public cloud regions but currently restricts log storage to certain US regions.

## How NSP Works

- Each NSP can contain up to 200 profiles and associate with 1000 resources.
- Each profile can have up to 200 inbound and outbound rule elements.
- Profiles operate in two modes:
  - Learning (default): Reports current access patterns
  - Enforced: Denies all traffic not explicitly allowed
- Inbound and outbound rules can be specified by IP range or Azure subscription.

## Demonstrations

### Key Vault Security Demo

- Several NSP-supported resources are set to default (all networks allowed).
- A VM with a managed identity has unrestricted access to Key Vault secrets.
- By creating an NSP and associating resources, but without setting rules, access is blocked once mode is set to 'Enforced.'
- Adding an inbound rule (by subscription) allows the VM to regain access to Key Vault.

### Storage Account Security Demo

- After setting up inbound rules for the relevant subscriptions or IPs, access is verified and works as expected.
- Attempting access from an unauthorized IP is correctly denied.

## Management and Limitations

- NSP is in Public Preview and currently free, but pricing may apply at GA.
- Outbound rules further secure data egress by allowing only specified FQDNs.
- NSP streamlines security management, particularly for large, complex environments.

## Conclusion

Network Security Perimeter significantly improves Azure security, making it easier to lock down critical PaaS resources. While it's not a "silver bullet," NSP makes network-level access control practical and less error-prone.

*For assistance evaluating or implementing secure Azure networking, contact the author or the Zure team.*

This post appeared first on "Zure Data & AI Blog". [Read the entire article here](https://zure.com/blog/securing-azure-networks-with-network-security-perimeter)
