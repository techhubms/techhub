---
layout: "post"
title: "Metadata Security Protocol (MSP) General Availability Secures Azure VM Metadata"
description: "This announcement details the General Availability of Azure's Metadata Security Protocol (MSP)—the first major cloud-native solution to introduce robust authentication and authorization at the platform layer for virtual machines. MSP implements a default-closed security design for Azure's Instance Metadata Service (IMDS) and WireServer, using trusted delegates, HMAC signatures, and process-level RBAC to dramatically decrease metadata-related attack surface. The post explains how MSP mitigates SSRF attacks, enforces fine-grained access control inside VMs, and aligns with zero-trust principles. Practical onboarding steps are outlined for Azure customers."
author: "Amjad_Shaik"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-compute-blog/introducing-metadata-security-protocol-msp-elevating-platform/ba-p/4471204"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-19 19:36:55 +00:00
permalink: "/2025-11-19-Metadata-Security-Protocol-MSP-General-Availability-Secures-Azure-VM-Metadata.html"
categories: ["Azure", "Security"]
tags: ["Allowlisting", "Audit Mode", "Authentication", "Authorization", "Azure", "Azure Virtual Machines", "Cloud Security", "Community", "Ebpf", "Guest Proxy Agent", "HMAC", "IMDS", "Instance Metadata Service", "Metadata Security Protocol", "MSP", "RBAC", "Role Based Access Control", "Security", "Security Best Practices", "Server Side Request Forgery", "SSRF", "WireServer", "Zero Trust"]
tags_normalized: ["allowlisting", "audit mode", "authentication", "authorization", "azure", "azure virtual machines", "cloud security", "community", "ebpf", "guest proxy agent", "hmac", "imds", "instance metadata service", "metadata security protocol", "msp", "rbac", "role based access control", "security", "security best practices", "server side request forgery", "ssrf", "wireserver", "zero trust"]
---

Amjad_Shaik introduces Azure's Metadata Security Protocol (MSP) for Virtual Machines, describing its security enhancements, attack mitigation strategies, and step-by-step guidance for platform deployment.<!--excerpt_end-->

# Metadata Security Protocol (MSP): General Availability for Azure Virtual Machines

## Overview

Azure has announced the General Availability of Metadata Security Protocol (MSP), an industry-first protocol designed to secure the platform layer in Azure Virtual Machines. MSP adds robust authentication and authorization (AuthN/AuthZ) controls to metadata service endpoints, including the Instance Metadata Service (IMDS) and WireServer, providing defense beyond traditional boundaries.

## Key Features and Protections

- **Strong Authentication**: Every IMDS and WireServer request is authenticated using trusted delegates and HMAC signatures, ensuring only verified processes can access sensitive metadata inside VMs.
- **Default-Closed Security Model**: Metadata endpoints are locked down by default. Only approved applications and users can interact with IMDS, enforcing strict allowlisting.
- **Guest Proxy Agent (GPA)**: GPA leverages eBPF to verify sources of metadata requests and applies Role-Based Access Control (RBAC) at the process level for granular security.
- **Attack Mitigation**:
  - **SSRF**: MSP helps curb server-side request forgery by authenticating IMDS calls and denying unauthorized access to tokens/configuration.
  - **Nested Virtualization Bypasses**: Protects against multi-tenancy and trust boundary attacks in virtualized/cloud setups.
  - **Implicit VM Trust Reduction**: Adds deep defense mechanisms within applications, beyond just network isolation.
- **Fine-Grained Controls**: Advanced configuration enables restriction of IMDS access to specific processes/users, reducing attack surface.

## Benefits to Azure Customers

- **Defense-In-Depth**: Additional layer protecting against metadata and identity token attacks, even from misconfigured or compromised processes.
- **Granular Access**: Define applications and users allowed access to metadata—no implicit trust, supporting zero-trust strategies.
- **Auditability and Easy Onboarding**: Start MSP in audit mode to monitor usage, create allowlists with legitimate app data, then enforce controls to lock down access.

## Getting Started with MSP

Follow these steps to onboard MSP on your Azure VMs:

1. **Enable MSP in Audit Mode**: Monitor which processes attempt to access IMDS.
2. **Create an Allowlist**: Use logs to determine legitimate applications and users, then build your allowlist.
3. **Switch to Enforcement**: Move to enforcement mode to restrict access strictly to the allowlist and block unauthorized attempts.

For full instructions, see [the MSP documentation on Microsoft Learn](https://learn.microsoft.com/en-us/azure/virtual-machines/metadata-security-protocol/overview).

## References

- [Metadata Security Protocol Overview](https://learn.microsoft.com/en-us/azure/virtual-machines/metadata-security-protocol/overview)
- [Instance Metadata Service (IMDS)](https://learn.microsoft.com/en-us/azure/virtual-machines/instance-metadata-service?tabs=windows)
- [WireServer](https://learn.microsoft.com/en-us/azure/virtual-network/what-is-ip-address-168-63-129-16?tabs=windows)
- [Guest Proxy Agent (GPA) on GitHub](https://github.com/Azure/GuestProxyAgent)

*Author: Amjad_Shaik*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/introducing-metadata-security-protocol-msp-elevating-platform/ba-p/4471204)
