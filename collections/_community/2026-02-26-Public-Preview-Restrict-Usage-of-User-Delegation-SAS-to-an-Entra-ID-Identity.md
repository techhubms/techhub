---
external_url: https://techcommunity.microsoft.com/t5/azure-storage-blog/public-preview-restrict-usage-of-user-delegation-sas-to-an-entra/ba-p/4497196
title: 'Public Preview: Restrict Usage of User Delegation SAS to an Entra ID Identity'
author: ellievail
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-26 15:15:30 +00:00
tags:
- Access Control
- Azure
- Azure Blob Storage
- Azure CLI
- Azure Security
- Azure Storage
- Community
- Identity And Access Management
- Microsoft Entra ID
- PowerShell
- RBAC
- REST API
- SAS
- SDK
- Security
- Shared Access Signature
- Storage Accounts
- User Delegation SAS
section_names:
- azure
- security
---
ellievail details the public preview of user-bound user delegation SAS for Azure Storage, showing how to tightly control access by tying SAS tokens to individual Entra ID identities.<!--excerpt_end-->

# Public Preview: Restrict Usage of User Delegation SAS to an Entra ID Identity

**Author:** ellievail

Shared access signatures (SAS) have long provided a way to grant time-limited, scoped access to Azure Storage resources without requiring the exposure of storage account keys. Over time, security has been improved, moving from account key-based SAS to user delegation (UD) SAS, which are protected by Microsoft Entra ID (formerly known as Azure Active Directory).

## What’s New: User-Bound User Delegation SAS Preview

Microsoft is introducing a public preview for *user-bound user delegation SAS*. This extension ensures a SAS token can only be used by a specified Entra ID identity, reducing the risk of unintended or unauthorized access. Unlike traditional SAS or even standard UD SAS, user-bound SAS ties the token's privileges concretely to a set Entra ID security principal.

### Key Security Enhancements

- User-bound SAS extends user delegation SAS, restricting SAS usage to a specific identity.
- Delegators specify the Entra identity of the end user; the end user must authenticate using Entra ID when accessing the resource.
- Supports use within the same tenant or across tenants (with the allowCrossTenantDelegationSas setting enabled).
- SAS tokens are traceable to the delegating identity, and validity periods are strictly limited (up to 7 days).
- No additional cost—follows standard Azure Storage transaction pricing.

## How It Works

1. **Assign appropriate RBAC roles**: The user generating the user-bound SAS must have Storage Data Contributor and Storage Delegator roles for the relevant Azure service (Blob, Table, Files, or Queue).
2. **Generate a user delegation key**: Request a user delegation key from Azure Storage, tied to the delegator’s Entra ID identity.
3. **Gather end user details**: Obtain the OAuth object ID and tenant ID from the end user.
4. **Create the user-bound SAS token**: Use the same steps as standard UD SAS, but specify the intended Entra ID and tenant in the token parameters.
5. **Share the SAS token**: Deliver the SAS to the application or user who requires access (recommend using secure means, such as Azure Key Vault).

> See detailed step-by-step guides for each platform:
> - [REST API](https://learn.microsoft.com/rest/api/storageservices/get-user-delegation-key)
> - [.NET SDK](https://learn.microsoft.com/azure/storage/blobs/storage-blob-user-delegation-sas-create-dotnet?tabs=packages-dotnetcli%2Ccontainer)
> - [PowerShell](https://learn.microsoft.com/azure/storage/blobs/storage-blob-user-delegation-sas-create-powershell)
> - [Azure CLI](https://learn.microsoft.com/azure/storage/blobs/storage-blob-user-delegation-sas-create-cli)

## Tenant and Cross-Tenant Scenarios

- *Same tenant*: No special configuration needed.
- *Different tenant*: Enable the `allowCrossTenantDelegationSas` on the storage account if delegating access across tenants.

## Supported Storage Account Types

- Available for all GPv2 storage accounts in public Azure regions during this preview.

## Pricing and Availability

- This feature is in preview across all public Azure regions.
- No extra costs; standard storage transaction fees apply ([Azure Storage Pricing](https://azure.microsoft.com/pricing/details/storage/blobs/)).

## Feedback and Support

- Provide feedback through the [feedback form](https://forms.office.com/Pages/DesignPageV2.aspx?origin=NeoPortalPage&subpage=design&id=v4j5cvGGr0GRqy180BHbR9iuLyDgXDNIkMaAAVSMpJxUOTNNNDFGTVZIUTdRU0w0Qjg0QjdTSlNSNy4u).
- For support, open a request in the [Azure Portal](https://portal.azure.com/#blade/Microsoft_Azure_Support/HelpAndSupportBlade/newsupportrequest).

---

*Published Feb 26, 2026.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-storage-blog/public-preview-restrict-usage-of-user-delegation-sas-to-an-entra/ba-p/4497196)
