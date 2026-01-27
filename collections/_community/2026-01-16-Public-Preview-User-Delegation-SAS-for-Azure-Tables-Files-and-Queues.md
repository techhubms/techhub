---
external_url: https://techcommunity.microsoft.com/t5/azure-storage-blog/announcing-public-preview-of-user-delegation-sas-for-azure/ba-p/4485693
title: 'Public Preview: User Delegation SAS for Azure Tables, Files, and Queues'
author: ellievail
feed_name: Microsoft Tech Community
date: 2026-01-16 13:54:12 +00:00
tags:
- Access Control
- Azure Files
- Azure Queues
- Azure Storage
- Azure Tables
- CLI
- Delegated Access
- Entra ID
- Identity
- PowerShell
- RBAC
- REST API
- SAS Token
- SDK
- User Delegation SAS
section_names:
- azure
- security
primary_section: azure
---
ellievail announces the public preview of user delegation SAS for Azure Tables, Files, and Queues, highlighting new security features for delegated access via Entra ID and Azure RBAC.<!--excerpt_end-->

# Public Preview: User Delegation SAS for Azure Tables, Files, and Queues

Azure Storage now supports user delegation (UD) shared access signatures (SAS) for Azure Tables, Azure Files, and Azure Queues in public preview. Already available for Azure Blobs, this enhancement extends secure, identity-based delegated access across more Azure Storage services.

## What is User Delegation SAS?

User delegation SAS ties access tokens specifically to a user's identity in Microsoft Entra ID. This improves security by ensuring that tokens can only be used within the delegated scope of the creator's access rights. Delegation is now possible for more granular levels, including tables, table entities, queues, queue messages, file shares, and individual files.

- **Enhanced Security**: SAS tokens are linked to Entra ID accounts and Azure role-based access control (RBAC), reducing risk from overprivileged accounts.
- **Scoped Permissions**: Lower-privileged users can delegate only subsets of their access to clients.

## Supported Services and Interfaces

- **Now in public preview**: Azure Tables, Azure Files, and Azure Queues
- **Previously available**: Azure Blobs
- **Interfaces**:
  - REST APIs (all three services)
  - SDKs, PowerShell, CLI (Files and Queues; Tables via REST)

## Steps to Get Started

All general-purpose v2 storage accounts can use user delegation SAS, with no separate account setting required. Follow these steps:

1. **Assign Roles**: Ensure you have the Storage Data Contributor and Storage Delegator RBAC roles (specific to the service).
2. **Get User Delegation Key**: Retrieve the key via REST API [(instructions)](https://learn.microsoft.com/en-us/rest/api/storageservices/create-user-delegation-sas#request-the-user-delegation-key).
3. **Create SAS Token**: Construct the user delegation SAS token [(instructions)](https://learn.microsoft.com/en-us/rest/api/storageservices/create-user-delegation-sas#construct-a-user-delegation-sas). Note, permissions vary per service.
4. **Share SAS Token**: Distribute as needed—tokens can be passed automatically in applications or stored in systems like Azure Key Vault.

## Pricing and Availability

- No extra cost beyond standard storage transaction pricing ([Azure Storage Pricing](https://azure.microsoft.com/pricing/details/storage/blobs/)).
- Public preview is available in all Azure regions.

## Feedback and Support

- [Feedback form](https://forms.office.com/Pages/DesignPageV2.aspx?origin=NeoPortalPage&subpage=design&id=v4j5cvGGr0GRqy180BHbR9iuLyDgXDNIkMaAAVSMpJxUOVdMS0tXU1ZVV1hRQzlLM1hYTVZYTzEwVy4u)
- [Microsoft support request](https://portal.azure.com/#blade/Microsoft_Azure_Support/HelpAndSupportBlade/newsupportrequest)

---

For detailed implementation instructions, see the [official documentation](https://learn.microsoft.com/en-us/rest/api/storageservices/create-user-delegation-sas#construct-a-user-delegation-sas) for creating a user delegation SAS.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-storage-blog/announcing-public-preview-of-user-delegation-sas-for-azure/ba-p/4485693)
