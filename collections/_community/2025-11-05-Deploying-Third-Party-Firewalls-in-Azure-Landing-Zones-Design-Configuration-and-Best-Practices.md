---
external_url: https://techcommunity.microsoft.com/t5/azure-networking-blog/deploying-third-party-firewalls-in-azure-landing-zones-design/ba-p/4458972
title: 'Deploying Third-Party Firewalls in Azure Landing Zones: Design, Configuration, and Best Practices'
author: PramodPalukuru
feed_name: Microsoft Tech Community
date: 2025-11-05 06:11:28 +00:00
tags:
- Active Active Deployment
- Active Passive Deployment
- Azure Firewall
- Azure Load Balancer
- Azure Monitor
- Azure Sentinel
- BYOL
- Check Point
- Fortinet
- High Availability
- Hub And Spoke Architecture
- IDS/IPS
- Intrusion Detection
- Landing Zone
- NAT Rules
- Network Security
- Palo Alto
- Third Party Firewall
- User Defined Routes
- Virtual Network
- Zero Trust
- Azure
- Security
- Community
section_names:
- azure
- security
primary_section: azure
---
PramodPalukuru delivers a comprehensive overview of deploying third-party firewalls in Azure Landing Zones, sharing actionable design, configuration, and security best practices for enterprise-scale cloud environments.<!--excerpt_end-->

# Deploying Third-Party Firewalls in Azure Landing Zones: Design, Configuration, and Best Practices

_Authored by PramodPalukuru_

## Introduction

As enterprises migrate and scale workloads in Microsoft Azure, robust network security becomes foundational. While Azure Firewall provides a native and managed solution, many organizations opt for familiar third-party appliances from vendors like Palo Alto, Fortinet, and Check Point to meet advanced needs and maintain operational consistency with on-premises security architectures. This guide walks through the architectural patterns, configuration strategies, and proven practices for successful integration of these solutions in Azure Landing Zones.

## 1. Overview: Landing Zone Architecture & Firewall Roles

- **Azure Landing Zone**: Microsoft's modular, enterprise-scale scaffold for secure, governed cloud adoption across multiple subscriptions and regions.
- **Hub-and-Spoke Topology:**
  - **Hub** centralizes shared services (DNS, VPN/ExpressRoute, Azure Firewall or third-party appliances, Bastion, monitoring).
  - **Spokes** house workloads (e.g., web apps, data platforms) in isolated VNets, connected via the hub for policy consistency.
- **Traffic Flow:**
  - _North-South_: Internet ↔ Azure workloads, routed through the external firewall.
  - _East-West_: Intra-cloud communications, routed through an internal firewall for segmentation and threat prevention.

### Why Use Firewalls Above NSGs & Route Tables?

- Enable deep packet inspection (DPI), application-level filtering, intrusion detection/prevention (IDS/IPS), and centralized policy controls.
- Reduce blast radius, enforce least privilege, and consistently meet compliance across hybrid/multi-cloud environments.

## 2. Choosing Third-Party Firewalls Over Azure Firewall

Organizations gravitate towards third-party appliances for:

- **Advanced security features**: Deep packet/application inspection, SSL decryption, and threat intelligence.
- **Vendor familiarity & existing management tools**: Use of Panorama, FortiManager, or SmartConsole across on-premises and cloud.
- **Industry certifications**: Certain sectors (finance, healthcare) demand certified solutions.
- **Hybrid/multi-cloud alignment**: Unified security layer across Azure, on-prem, and other clouds.
- **Customization**: Greater control of OS, routing, integrations, and patch cycles.
- **Licensing flexibility**: BYOL options via Azure Marketplace images.

_Azure Firewall_ remains a strong option for:

- Simpler use cases.
- Teams preferring managed PaaS and minimal infrastructure overhead.

## 3. Deployment Models

### IaaS-Based Appliance Deployments

- Deployable as Azure VMs via Marketplace or ARM/Bicep templates.
- Integrate with Azure VNets, peering, Azure Monitor, and Sentinel.
- **Active-Active:**
  - Multiple firewalls balanced using Azure Load Balancer.
  - High throughput, resilience, and near-zero downtime.
  - Policy/state synchronization required (BGP, UDRs).
- **Active-Passive:**
  - Primary firewall handles traffic, secondary stands by.
  - IP reassignment managed via Azure service principals on failover (some downtime anticipated).
- **Multiple NICs per firewall instance:**
  - Untrust/Public (Internet facing)
  - Trust/Internal (private traffic)
  - Management (admin traffic)
  - HA NIC (for stateful sync and failover)

## 4. Configuration Considerations

### Routing

- **User-Defined Routes (UDRs):** Ensure outbound/inbound flows always traverse firewalls, using load balancer IP where needed.
- **Symmetric Routing:** Prevent firewall connection drops via path symmetry; BGP with Azure Route Server can greatly help.

### Policies

- **NAT (DNAT/SNAT)**: Secure public app publishing and private identity masking.
- **Security Rules:** Granular allow/deny for all traffic directions, enforcing least privilege.
- **Segmentation:** Separate workloads, environments, and tenants for compliance.
- **Application-Aware Controls:** Layer 7 inspection, user-level rules (Azure AD, LDAP integration).

### Load Balancers

- **ILB:** Controls east-west inspection.
- **ELB:** Manages north-south flows; critical for Active-Active scaling.
- **Health Probes:** Auto-bypass unhealthy firewall nodes.

### Identity Integration & Management

- **Azure Service Principals:** Automate failover IP reassignment for seamless passive–>active switch.
- **RBAC:** Use Azure RBAC for firewall config permissions.
- **SIEM Integration:** Forward logs to Azure Monitor, Sentinel, or other SIEMs.

### Licensing

- **Pay-As-You-Go:** VM + license bundled via Marketplace (good for pilot/short-term).
- **BYOL:** Reuse enterprise agreements; cost-effective for scale.
- **Hybrid Licenses:** Support license mobility from on-premises.

## 5. Operational Challenges

- **Misconfiguration**: Incorrect UDRs/NAT can break connectivity or bypass inspection.
- **Asymmetric Routing**: Topology errors cause stateful firewall packet drops.
- **Performance Bottlenecks**: VM SKU limits can throttle throughput; proactive scaling and monitoring are essential.
- **Failover Downtime**: Even short failovers in Active-Passive deployments can impact service levels.
- **Backup/Recovery**: Azure Backup unsupported; export configurations manually to external storage.
- **Azure Session Limit Cap**: 250,000 concurrent connections per firewall VM—solution is horizontal scaling and careful monitoring.

## 6. Best Practices

- Deploy across Availability Zones for resilience.
- Prefer Active-Active for critical workloads.
- Isolate interfaces (trust, untrust, HA, management) onto dedicated subnets.
- Begin with a deny-all baseline, permit only explicit traffic.
- Use standardized naming and tagging for all network/security resources.
- Validate traffic flows end-to-end using Azure Network Watcher and firewall/vendor logs.
- Monitor health/scalability; enable autoscaling if possible.
- Schedule regular firmware and signature updates with automation where possible.

## Conclusion

Third-party firewalls offer the advanced controls, operational consistency, and flexibility required for secure enterprise-scale Azure adoption. By following robust architectural guidance and best practices—especially for hub-and-spoke network topologies, redundant deployments, and careful configuration—organizations can deliver a secure, reliable foundation for their cloud workloads.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-networking-blog/deploying-third-party-firewalls-in-azure-landing-zones-design/ba-p/4458972)
