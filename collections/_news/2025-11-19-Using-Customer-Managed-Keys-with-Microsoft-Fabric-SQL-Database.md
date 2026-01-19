---
external_url: https://blog.fabric.microsoft.com/en-US/blog/announcing-public-preview-customer-managed-keys-in-fabric-sql-database/
title: Using Customer-Managed Keys with Microsoft Fabric SQL Database
author: Microsoft Fabric Blog
viewing_mode: external
feed_name: Microsoft Fabric Blog
date: 2025-11-19 08:00:00 +00:00
tags:
- Access Management
- AI
- AI Projects
- Asymmetric Key
- Auditability
- Azure Key Vault
- Compliance
- Customer Managed Keys
- Data Governance
- Data Security
- DEK
- Encryption
- Encryption Query
- Fabric Integration
- Microsoft Fabric
- Regulatory Standards
- Secure Development
- SQL Database
- Transparent Data Encryption
- Workspaces
section_names:
- azure
- ml
- security
---
The Microsoft Fabric Blog highlights how customer-managed keys in Fabric SQL Database empower organizations to enhance data security and compliance, with insights from Data and AI Practice Leads.<!--excerpt_end-->

# Using Customer-Managed Keys with Microsoft Fabric SQL Database

Microsoft Fabric has rolled out Customer-Managed Keys (CMK) in SQL Database, giving organizations direct control over encryption for data-at-rest. Previously, all Fabric SQL data was encrypted with Microsoft-managed keys. CMK now lets organizations use their own Azure Key Vault keys for encryption, supporting data governance and regulatory requirements.

## Key Features

- **Key Ownership and Rotation**: Organizations can manage their own keys, rotate them as needed, and customize access policies.
- **Granular Access Management & Auditability**: Full control over who accesses encryption keys and detailed tracking of key usage.
- **Transparent Data Encryption (TDE)**: Automatically enabled when CMK is configured in a Fabric workspace. This covers:
   - Real-time encryption of databases, backups, and transaction logs.
   - Page-level encryption using a symmetric DEK.
   - DEK protected by customer-managed asymmetric key from Azure Key Vault.
- **Automatic Activation**: No manual steps required—encryption applies to existing/new databases in the Fabric workspace.

## What Customers Are Saying

- *Ivan van Rooyen – Data and AI Practice Lead:* "Features such as customer-managed keys support clients with high security and regulatory standards. The setup is straightforward."
- *Vikram Hodachalli – Architect:* "CMK in Fabric SQL Database empowered us to securely develop AI project notebooks and data flows, plus gave insights for future innovation."

## Getting Started

- Follow Microsoft’s guide for [Customer-managed keys for Fabric workspaces](https://learn.microsoft.com/en-us/fabric/security/workspace-customer-managed-keys).

## Verify CMK Encryption

To confirm successful encryption of SQL databases:

```sql
SELECT DB_NAME(database_id) as DatabaseName, *
FROM sys.dm_database_encryption_keys
WHERE database_id <> 2
```

A database is encrypted if `encryption_state_desc` displays "ENCRYPTED" or "ENCRYPTION_IN_PROGRESS" with `ASYMMETRIC_KEY` as `encryptor_type`.

## Further Resources

- [Data encryption in SQL database](https://learn.microsoft.com/fabric/database/sql/encryption)
- [Customer-managed keys for Fabric workspaces](https://learn.microsoft.com/fabric/security/workspace-customer-managed-keys)

## Summary

Customer-managed keys in Microsoft Fabric SQL Database offer cloud-first organizations new ways to control and audit data encryption, blending compliance features and deep integration with Azure Key Vault. This solution especially benefits scenarios involving sensitive AI/data projects or regulated industries.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/announcing-public-preview-customer-managed-keys-in-fabric-sql-database/)
