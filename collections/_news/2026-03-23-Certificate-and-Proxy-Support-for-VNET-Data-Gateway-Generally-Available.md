---
author: Microsoft Fabric Blog
primary_section: azure
feed_name: Microsoft Fabric Blog
section_names:
- azure
- security
title: Certificate and Proxy Support for VNET Data Gateway (Generally Available)
tags:
- Azure
- Certificate Based Authentication
- Certificate Lifecycle
- Certificate Rotation
- Compliance
- Corporate PKI
- Enterprise Security
- Microsoft Fabric
- Microsoft Learn
- Network Egress
- Network Monitoring
- News
- Outbound Internet Restrictions
- PKI
- Proxy Configuration
- Security
- Traffic Inspection
- Virtual Network Data Gateway
- VNet Data Gateway
- X.509 Certificates
external_url: https://blog.fabric.microsoft.com/en-US/blog/certificate-and-proxy-support-for-vnet-data-gateway-generally-available/
date: 2026-03-23 11:00:00 +00:00
---

Microsoft Fabric Blog announces GA support for certificate-based authentication and proxy routing in VNET Data Gateway, aimed at regulated and enterprise environments needing stronger identity assurance and controlled network egress.<!--excerpt_end-->

# Certificate and Proxy Support for VNET Data Gateway (Generally Available)

*If you haven’t already, check out Arun Ulag’s blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at FabCon and SQLCon announcements across both Fabric and database offerings:* https://aka.ms/FabCon-SQLCon-2026-news

**Certificate and Proxy Support for VNET Data Gateway** is now generally available, adding stronger security controls and more network flexibility for enterprise and regulated environments.

These capabilities are meant to help organizations align gateway connectivity with corporate security standards and network policies. With certificate-based authentication and proxy configuration support, VNET Data Gateway deployments can operate in more restricted, controlled, and compliance-driven infrastructures.

## Certificate-based authentication (Generally Available)

Organizations can now use **trusted, enterprise-issued certificates** to validate gateway communication. This enables security teams to:

- Use corporate PKI-issued certificates
- Enforce stronger identity validation for gateway connectivity
- Align with internal security and compliance requirements
- Reduce reliance on less controlled authentication approaches
- Standardize certificate lifecycle and rotation practices

Certificate-based authentication helps ensure gateway communication is verified using organization-approved trust chains.

## Proxy configuration support (Generally Available)

VNET Data Gateway now supports **proxy-based network routing**, enabling connectivity in environments where direct outbound internet access is restricted.

With proxy support, organizations can:

- Route gateway traffic through **corporate-controlled proxy servers**
- Comply with strict outbound network policies
- Operate in locked-down enterprise network environments
- Centralize traffic inspection and monitoring
- Meet regulatory and internal network control requirements

This expands where and how VNET Data Gateway can be deployed.

![Screenshot of certificate and proxy settings for a Virtual Network Data Gateway](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-certificate-and-proxy-settings-for-a-3.png)

*Figure: Configure certificate authentication and proxy for a Virtual Network Data Gateway.*

## Why this matters for enterprises

Many enterprise and regulated environments require both **strong identity assurance** and **controlled network egress paths**. With certificate and proxy support generally available, VNET Data Gateway can better meet these requirements by:

- Strengthening connection security
- Supporting compliant network architectures
- Increasing deployment flexibility
- Reducing exceptions needed from security teams
- Enabling smoother adoption in regulated industries

These enhancements are positioned to make VNET Data Gateway more production-ready for security-sensitive workloads.

## Getting started

Administrators can enable certificate-based authentication and configure proxy settings as part of their VNET Data Gateway deployment and network setup.

- Coordinate with security and network teams to select appropriate certificates and proxy routing policies.
- Documentation: [Manage virtual network (VNet) data gateways | Microsoft Learn](https://learn.microsoft.com/data-integration/vnet/manage-data-gateways?toc=%2Ffabric%2Fdata-factory%2Ftoc.json%22%20%5Cl%20%22certificate--proxy-support-preview)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/certificate-and-proxy-support-for-vnet-data-gateway-generally-available/)

