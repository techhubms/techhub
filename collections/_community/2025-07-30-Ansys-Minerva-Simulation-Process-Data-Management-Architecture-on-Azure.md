---
external_url: https://techcommunity.microsoft.com/t5/azure-high-performance-computing/ansys-minerva-simulation-process-data-management-architecture-on/ba-p/4432098
title: Ansys Minerva Simulation & Process Data Management Architecture on Azure
author: Sunita_AZ0708
feed_name: Microsoft Tech Community
date: 2025-07-30 21:17:02 +00:00
tags:
- Ansys Minerva
- Backup Strategy
- Cloud Architecture
- Ddv5
- High Availability
- Infrastructure
- Simulation Data Management
- SQL Server
- Virtual Machines
section_names:
- azure
- security
primary_section: azure
---
Sunita_AZ0708 examines the technical architecture and deployment strategies of Ansys Minerva’s Simulation and Process Data Management platform on Azure, addressing reliability, security, backup, and VM recommendations.<!--excerpt_end-->

## In-Depth Overview: Ansys Minerva on Azure

### Introduction

Ansys Minerva’s Simulation & Process Data Management (SPDM) solution leverages a robust and scalable architecture on Microsoft Azure. This summary covers its distributed architecture, workflow, security, backup best practices, and specific Azure resource recommendations essential for organizations adopting Minerva in the cloud.

### Architecture Overview

- **Four Distributed Tiers**:
  - **Client Tier**: User-facing, web-based access.
  - **Web Tier**: IIS application servers, handling external requests.
  - **Enterprise Tier**: Central core server, running main business components (Minerva core, agents, vault, metadata extraction, and license servers).
  - **Resource Tier**: Storage and database infrastructure.
- **Network Design**: All tiers use individual VMs on a single Azure Virtual Network (VNet), and proximity placement groups minimize latency for critical enterprise components.
- **Dev/Test Environments**: Isolated VNets enable development and testing without impacting production workflows.

### Workflow & Authentication

1. **User Access**: SPDM users connect via an HTTPS endpoint (public URL).
2. **Single Sign-On**: Azure Entra ID with SAML configuration supports SSO, managing authentication securely.
3. **Traffic Security**: Azure Firewall and Application Gateway filter and route traffic, leveraging Microsoft’s threat intelligence.
4. **Web Tier**:
    - IIS serves as the application server.
    - Web Application Firewall (WAF) monitors exploits and supports features like sticky sessions.
    - Consistent VM configuration and recommended sizing—potential use of HPC VMs for performance needs.
5. **Enterprise Tier**:
    - Manages licensing, core business logic, metadata extraction, and file storage (Vault).
    - Distributed architecture supports scaling as needed.
    - Data Vaults can be distributed and can interact with HPC clusters.
6. **Database & Storage**:
    - Uses SQL Server or Oracle on IaaS with recommended practices for high availability (Always On groups, availability sets).
    - Azure Files Premium or Azure NetApp Files handle file storage.
7. **On-premises Integration**: Support and admin teams gain secured RDP access to VMs via Azure VPN and Bastion.

### Reliability & Scalability Practices

- Deploy multiple VMs in Web and Enterprise tiers for load balancing and fault tolerance.
- Leverage Application Gateway for distributing requests across web servers.
- Use proximity placement groups and availability sets/zones to maximize uptime.

### Data Protection & Backup Strategies

- Configure database and volume backups via Azure Backup and native utilities. Employ regular testing and coordination between database and volume backups.
- Azure Files snapshots or Azure NetApp Files provide point-in-time recovery.
- Always On availability groups increase database resilience.
- Recommended to set backup frequency according to business needs.

### Security Considerations

- Employ Azure’s built-in network security, WAF, and traffic filtering.
- Secure authentication with Azure Entra ID (SAML-based SSO).
- Consult Azure Security documentation for best practice frameworks.

### Virtual Machine Sizing and Recommendations

- Specific Azure VM SKUs recommended for each server role, e.g.,
  - Core server: Standard_F16s_v2
  - Agent server: Standard_F8s_v2
  - License server: Standard_D4d_v5
  - Extraction server: Standard_F8s_v2
  - Database: Standard E32-16ds v4
  - Volume server: Standard_L32s_v3

### Conclusion

By following this recommended architecture and best practices, organizations can ensure high availability, reliability, security, and performance for Ansys Minerva SPDM deployments on Azure. The article emphasizes the importance of robust backup strategies, tiered design, and security measures tailored to enterprise needs.

This post appeared first on Microsoft Tech Community. [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/ansys-minerva-simulation-process-data-management-architecture-on/ba-p/4432098)
