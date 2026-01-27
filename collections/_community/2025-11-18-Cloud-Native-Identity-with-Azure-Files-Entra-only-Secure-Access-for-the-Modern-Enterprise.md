---
external_url: https://techcommunity.microsoft.com/t5/azure-storage-blog/cloud-native-identity-with-azure-files-entra-only-secure-access/ba-p/4469778
title: 'Cloud Native Identity with Azure Files: Entra-only Secure Access for the Modern Enterprise'
author: Priyanka-Gangal
feed_name: Microsoft Tech Community
date: 2025-11-18 17:41:59 +00:00
tags:
- Active Directory
- Admin Roles
- Azure Files
- Cloud Security
- Cloud Storage
- FSLogix
- Hybrid Identity
- Identity Management
- Microsoft Entra ID
- Operational Overhead
- RBAC
- Remote Work
- SMB Authentication
- Thin Client
- Virtual Desktop Infrastructure
- Zero Trust
section_names:
- azure
- security
primary_section: azure
---
Priyanka-Gangal introduces the public preview of Entra-only identities for Azure Files, highlighting how cloud-native authentication streamlines secure access, reduces complexity, and modernizes enterprise storage infrastructure.<!--excerpt_end-->

# Cloud Native Identity with Azure Files: Entra-only Secure Access for the Modern Enterprise

Microsoft has announced the public preview of native Entra-only identities support for Azure Files SMB. This feature enables organizations to use identities managed entirely in the cloud (via Microsoft Entra ID, formerly Azure AD) for accessing Azure Files, eliminating dependencies on on-premises Active Directory or hybrid setups.

## Key Benefits

- **Enhanced Secure Access with Admin Roles**: Admin scenarios for SMB shares now use RBAC permissions instead of storage account keys, providing secure and manageable access control. [More info](https://learn.microsoft.com/en-us/azure/storage/files/storage-files-identity-configure-file-level-permissions#mount-the-file-share-with-admin-level-access).
- **Modern Identity Management**: Directly manage file and directory permissions in the Azure portal, streamlining configuration and centralizing identity management.
- **Reduced Operational Overhead**: There's no longer a need for VPNs, complex hybrid identity configurations, GPO maintenance, or Active Directory health checks.
- **Elimination of On-Premises Infrastructure**: Organizations can retire domain controllers and grant cloud-only identities seamless access to Azure Files.
- **Remote Login for Thin Clients**: Remote workers on thin clients only require internet access, removing the necessity for network connectivity to AD servers.

## Modernizing Real World Architectures

### Virtual Desktop Workloads (VDI) with Entra-only Identities

Azure Files underpins VDI solutions like Azure Virtual Desktop (AVD), offering scalable, secure file shares for persistent user profiles and session continuity. Integration with FSLogix enables roaming user profiles. Native Entra Kerberos with cloud-only identity lets AVD session hosts authenticate and access file shares without on-premises AD, aligning with Zero Trust principles and simplifying cloud-only deployments.

> “Entra-only identities access with Azure Files will transform how we deliver virtual desktop solutions. By removing the need for on-premises domain controllers, we've simplified deployments and strengthened security for our customers. This cloud-native approach aligns perfectly with Zero Trust principles, enabling us to provide a seamless, secure VDI experience while reducing operational complexity.” — Jacques Theron, Cloud Solutions Architect, Netsurit

### Thin Clients for Remote Collaboration

Industries such as oil & gas can use Entra-only identities to enable secure file-sharing over simple internet connectivity, avoiding thick clients and domain controller setups at remote sites. This supports global, decentralized teams in real-time collaboration, improves agility, reduces IT costs, and fosters a cloud-based identity architecture.

> “Entra-only identities support with Azure Files transforms SLB’s Petrel workflows by removing dependencies on on-premises domain controllers, simplifying identity management and storage infrastructure for globally distributed teams working on complex exploration and reservoir characterization. This cloud-native architecture allows customers to access SMB shares in an easy and secure manner without complex VPN or hybrid infrastructure setups.” – Swapnil Daga, Storage Architect for Tenant Infrastructure, SLB.

### Information Worker Productivity

Large distributed workforces benefit from seamless, secure SMB access without domain controller dependencies. Entra ID authentication ensures consistent file access in any location, speeding onboarding and reducing helpdesk load. Policy-driven governance supports productivity and user mobility.

## Getting Started

Organizations can begin using Entra-only identity support with Azure Files at no additional cost. Step-by-step documentation is available [here](https://learn.microsoft.com/en-us/azure/storage/files/storage-files-identity-auth-hybrid-identities-enable?tabs=azure-portal%2Cintune). For questions, contact the Microsoft Azure Files team at <azurefiles@microsoft.com>.

---

*Updated Nov 17, 2025 by Priyanka-Gangal*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-storage-blog/cloud-native-identity-with-azure-files-entra-only-secure-access/ba-p/4469778)
