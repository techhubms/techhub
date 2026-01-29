---
external_url: https://dellenny.com/azure-security-basics-network-security-groups-firewalls-and-defender-for-cloud/
title: 'Azure Security Basics: Network Security Groups, Firewalls, and Defender for Cloud'
author: Dellenny
feed_name: Dellenny's Blog
date: 2025-11-05 09:26:10 +00:00
tags:
- Azure Firewall
- Azure Monitor
- Cloud Architecture
- Cloud Security
- Defense in Depth
- Firewall Policies
- Incident Response
- Just in Time Access
- Log Analytics
- Microsoft Defender For Cloud
- Network Security Group
- NSG
- Security Auditing
- Security Posture Management
- Security Recommendations
- Threat Intelligence
- Virtual Networks
- Zero Trust
- Azure
- Security
- Blogs
section_names:
- azure
- security
primary_section: azure
---
Dellenny provides a practitioner-focused walkthrough on securing Azure networks using Network Security Groups, Azure Firewall, and Defender for Cloud, outlining practical strategies and best practices for cloud security.<!--excerpt_end-->

# Azure Security Basics: Network Security Groups, Firewalls, and Defender for Cloud

**Author:** Dellenny

Securing your Microsoft Azure environment is crucial for protecting workloads and data. This article explains the three core pillars of Azure network security: Network Security Groups (NSGs), Azure Firewall, and Microsoft Defender for Cloud. It includes actionable guidelines, architecture advice, and setup checklists for a solid security foundation.

## 1. Network Security Groups (NSGs)

- **Definition:** NSGs filter inbound and outbound traffic to Azure resources at the subnet or network-interface level using prioritized rules.
- **How They Work:**
  - Operate at OSI Layer 3 and 4 (network and transport)
  - Allow/deny traffic based on source, destination, protocol, port range, and direction
  - Default rules include allowing virtual network traffic and denying all other inbound traffic
  - Attach to subnets or NICs for fine-grained control
- **Best Practices:**
  - Segment networks (management, frontend, data)
  - Use tight restrictions, avoiding overly permissive rules (e.g., avoid 'Any/Any/Internet')
  - Leverage service tags and application security groups for simplified management
  - Rely on NSGs as the first line of defense, not as the only layer
- **Limitations:**
  - No application-layer (Layer 7) inspection
  - No deep packet inspection or built-in threat intelligence

## 2. Azure Firewall

- **Role:** Provides enterprise-grade, stateful, managed firewall capabilities.
- **Key Features:**
  - Stateful inspection of both internal (east-west) and external (north-south) traffic
  - Threat intelligence-based filtering using Microsoft feeds
  - Rules based on FQDN, with support for SNAT/DNAT
  - Premium SKU expands to intrusion detection, TLS inspection, and more
  - Centralized logging to Azure Monitor or Sentinel
- **Usage Scenarios:**
  - Centralized security gateway for large or hybrid cloud environments
  - Need for application-layer filtering and unified security policy enforcement
  - Environments with multiple VNETs or hub-and-spoke topologies
- **Comparison with NSG:**
  - NSG: local, subnet- or NIC-level filtering
  - Azure Firewall: central, deep packet inspection and global policy enforcement
  - Combine both for a defense-in-depth approach

## 3. Microsoft Defender for Cloud

- **Purpose:** Unified CSPM (Cloud Security Posture Management) and workload protection for Azure
- **Functionality:**
  - Continuous monitoring and analysis of resource security configurations
  - Security recommendations and alerts for misconfigurations (e.g., open ports, risky NSG rules)
  - Automated remediation guidance and integration with SOC tooling (Sentinel, SIEM)
- **Best Practices:**
  - Enable at environment setup (free tier provides valuable coverage)
  - Regularly review the Networking Recommendations dashboard
  - Cross-check Firewall and NSG setup against recommendations
  - Combine with Azure Monitor/Log Analytics for full observability

## 4. Layered Security Approach

- **Step 1:** Baseline network filtering with NSGs
- **Step 2:** Advanced inspection and centralized control using Azure Firewall
- **Step 3:** Ongoing assessment and incident response with Defender for Cloud
- **Example Architecture:** Hub-and-spoke VNETs utilize centralized Firewall in the hub, with NSGs in spokes, and Defender for Cloud monitoring the entire environment
- **Benefits:**
  - Attack surface reduction
  - Multi-layered defense
  - Real-time security posture visibility

## 5. Quick Setup Checklist

- Create/review NSGs, enable 'deny all inbound' by default
- Attach NSGs to subnets and/or NICs as appropriate
- Deploy Azure Firewall (Standard or Premium) for central gateway functionality
- Enable Microsoft Defender for Cloud across subscriptions
- Review security recommendations regularly
- Use JIT VM access for management ports
- Log all changes in Azure Monitor or Sentinel
- Regularly reassess security configurations as threats evolve

By combining these Microsoft Azure security tools, organizations can achieve robust, sustainable defenses against both external and internal cloud threats, supporting better compliance and business continuity.

This post appeared first on "Dellenny's Blog". [Read the entire article here](https://dellenny.com/azure-security-basics-network-security-groups-firewalls-and-defender-for-cloud/)
