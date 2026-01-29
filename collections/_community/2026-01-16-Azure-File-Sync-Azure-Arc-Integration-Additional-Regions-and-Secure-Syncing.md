---
external_url: https://techcommunity.microsoft.com/t5/azure-storage-blog/azure-file-sync-azure-arc-integration-additional-regions-and/ba-p/4486050
title: 'Azure File Sync: Azure Arc Integration, Additional Regions, and Secure Syncing'
author: grace_kim
feed_name: Microsoft Tech Community
date: 2026-01-16 18:02:58 +00:00
tags:
- Authentication
- Azure Arc
- Azure File Sync
- Azure Files
- CLI
- Cloud Storage
- Enterprise Storage
- File Server Modernization
- Hybrid Cloud
- Hybrid File Services
- Managed Identities
- Microsoft Entra ID
- PowerShell
- Regional Availability
- Secure Access
- Windows Server
- Azure
- DevOps
- Security
- Community
section_names:
- azure
- devops
- security
primary_section: azure
---
grace_kim details new Azure File Sync capabilities, focusing on Azure Arc integration, regional expansion, and secure onboarding with managed identities—key for IT and DevOps teams modernizing enterprise file services.<!--excerpt_end-->

# Azure File Sync: Azure Arc Integration, Additional Regions, and Secure Syncing

As enterprises accelerate their journeys to the cloud, securely modernizing file data without disrupting daily operations becomes a core challenge. Azure Files and Azure File Sync now allow IT and DevOps teams to bridge on-premises Windows File Servers with Azure's flexibility and scale, introducing features that enhance hybrid deployment, security, and geographic reach.

## Highlights

- **Azure File Sync now available in four new regions**: Italy North, New Zealand North, Poland Central, and Spain Central, bringing file data closer to users and aiding compliance with regional mandates.
- **Integrated identity-driven authentication**: Leverages Microsoft Entra ID (formerly Azure Active Directory) via Managed Identities to streamline authentication and improve security, eliminating reliance on passwords and helping automate credential management.
- **Simplified onboarding with Azure Arc**: The Azure File Sync agent can now be quickly deployed on Azure Arc connected servers using Azure Arc extensions, PowerShell, or CLI. This streamlines installation and upgrades while enforcing security and compliance.
- **Cost-effective scaling**: Starting January 2026, File Sync incurs no per-server cost for customers using Windows Server Software Assurance, Azure Arc, and File Sync agent v22+, making large-scale hybrid file deployments more economical.

## Simplified Deployment with Azure Arc Extension

Customers can easily add the Azure File Sync agent to Azure Arc-managed servers with just a few clicks in the portal or by automating with PowerShell/CLI. The Azure Arc extension ensures:

- Trusted, predictable installation and upgrade experiences
- Built-in security controls
- Reduced operational complexity in hybrid scenarios

## Expanded Regional Availability

Azure File Sync's expansion to four new global regions gives administrators more flexibility to:

- Align storage strategy with regional regulatory and performance needs
- Modernize remote offices, branch sites, or government workloads with data residency controls
- Strategically locate hybrid storage close to end users for improved performance

## Secure by Default with Managed Identities

Support for managed identities (handled by Microsoft Entra ID) ensures secure, end-to-end authentication between File Sync Servers, the Storage Sync Service, and Azure Files. Benefits include:

- Removal of direct storage account key management
- Automated key and certificate rotations
- Reduced security risk and simplified secret management
- Support extended to Azure Files SMB scenarios

## Cost Model Update

From January 2026 onward, Azure File Sync will not charge per-server fees for qualifying customers with Windows Server Software Assurance, using Azure Arc and File Sync agent v22 or higher. This drives down costs and simplifies planning as file services are scaled out.

## Getting Started

To deploy or expand your Azure File Sync footprint:

- Review the [Azure Arc integration documentation](https://learn.microsoft.com/en-us/azure/storage/file-sync/file-sync-extension?tabs=azure-portal)
- Check [regional availability](https://azure.microsoft.com/en-us/explore/global-infrastructure/products-by-region/table)
- Learn about [managed identities for file sync](https://learn.microsoft.com/en-us/azure/storage/file-sync/file-sync-managed-identities?tabs=azure-portal)

For additional information or technical questions, Microsoft invites IT professionals to reach out to the Azure Files team at [azurefiles@microsoft.com](mailto:azurefiles@microsoft.com).

_Updated Jan 16, 2026._

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-storage-blog/azure-file-sync-azure-arc-integration-additional-regions-and/ba-p/4486050)
