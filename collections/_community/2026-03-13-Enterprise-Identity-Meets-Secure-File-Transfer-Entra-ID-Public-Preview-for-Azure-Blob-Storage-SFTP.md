---
layout: "post"
title: "Enterprise Identity Meets Secure File Transfer: Entra ID Public Preview for Azure Blob Storage SFTP"
description: "This announcement introduces the public preview of Microsoft Entra ID-based access for Azure Blob Storage SFTP. It explains how organizations can eliminate the need for local SFTP user management by leveraging enterprise identity strategies. The post details security, compliance, onboarding, and collaboration improvements, with examples from real-world sectors like finance and healthcare, and provides steps to get started."
author: "JeevanManoj"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-storage-blog/enterprise-identity-meets-secure-file-transfer-entra-id-public/ba-p/4501937"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-03-13 10:37:25 +00:00
permalink: "/2026-03-13-Enterprise-Identity-Meets-Secure-File-Transfer-Entra-ID-Public-Preview-for-Azure-Blob-Storage-SFTP.html"
categories: ["Azure", "Security"]
tags: ["ABAC", "Access Control", "ACL", "Authentication", "Azure", "Azure Active Directory", "Azure Blob Storage", "Cloud Storage", "Community", "Conditional Access", "Data Protection", "Enterprise Security", "External Identities", "Identity Management", "Microsoft Entra ID", "Onboarding", "Privilege Management", "RBAC", "Security", "SFTP"]
tags_normalized: ["abac", "access control", "acl", "authentication", "azure", "azure active directory", "azure blob storage", "cloud storage", "community", "conditional access", "data protection", "enterprise security", "external identities", "identity management", "microsoft entra id", "onboarding", "privilege management", "rbac", "security", "sftp"]
---

JeevanManoj presents an in-depth guide to the public preview of Entra ID-based access for Azure Blob Storage SFTP, highlighting enhanced enterprise identity management and security.<!--excerpt_end-->

# Enterprise Identity Meets Secure File Transfer: Entra ID Public Preview for Azure Blob Storage SFTP

Microsoft has announced the public preview of Entra ID-based access for [Azure Blob Storage SFTP](https://learn.microsoft.com/azure/storage/blobs/secure-file-transfer-protocol-support-entra-id-based-access?). This update allows organizations to connect securely to Azure Blob Storage with Microsoft Entra ID (formerly Azure Active Directory), including support for guest users through Entra External Identities.

## Why Move Beyond Local SFTP Users?

Previously, Azure Blob Storage SFTP required using 'local users'—credentials managed individually for each user. This meant:

- Manual account provisioning and offboarding
- Management of passwords or SSH keys
- Disconnected identity silos
- Insecure ad-hoc external access for partners or vendors

## Entra ID-Based Access: Key Benefits

- **Eliminates local SFTP account management**: Users authenticate with existing corporate credentials through Entra ID. No additional password or SSH key distribution.
- **Enterprise-grade security**: Supports Multi-Factor Authentication (MFA), Conditional Access, Identity Protection, and Privileged Identity Management (PIM).
- **Unified Access Governance**: Integrates Azure RBAC, ABAC, and ACL for granular permissions, using a single unified permission model across SFTP, REST API, and Azure CLI.
- **Faster Onboarding**: New SFTP users (internal or external) can be onboarded within minutes, with streamlined lifecycle and audit processes.
- **External Collaboration**: Partners and vendors can authenticate using Entra External Identities, with full audit trails and Conditional Access policies.

## Real-World Scenarios

- **Financial Services**: Merchants deliver daily files via SFTP, authenticating through Entra ID B2B; MFA and granular access enforced.
- **Healthcare**: Hospitals share sensitive documents externally with audit and Compliance (HIPAA) via Entra ID.
- **Media & Entertainment**: Freelancers and agencies transfer files with time-bound, automatically revoked access—no local credential setups.
- **Manufacturing**: Suppliers receive secure, policy-driven access using a unified identity system.

## How It Works

1. **Authentication**: Users log in with their corporate Entra ID credentials.
2. **Certificate Issuance**: A short-lived SSH certificate is granted, validated by the service.
3. **Access Control**: Permissions are enforced via Azure RBAC/ABAC/ACL frameworks.
4. **Revocation**: Access is instantly revoked as soon as identity changes are made in Entra ID, including for external users.

## Getting Started

- Register for the public preview: [Documentation](https://learn.microsoft.com/azure/storage/blobs/secure-file-transfer-protocol-support-entra-id-based-access?#connecting-to-azure-blob-storage-with-microsoft-entra-ids).
- Test in non-production environments and provide feedback to shape future releases.

*Local user accounts remain supported, but Microsoft recommends Entra ID-based access for greater security and easier management.*

**Questions or feedback?** Contact the team at blobsftp@microsoft.com.

---
*JeevanManoj, Microsoft (Published Mar 13, 2026)*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-storage-blog/enterprise-identity-meets-secure-file-transfer-entra-id-public/ba-p/4501937)
