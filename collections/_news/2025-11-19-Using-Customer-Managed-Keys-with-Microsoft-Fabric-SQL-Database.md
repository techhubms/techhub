---
layout: "post"
title: "Using Customer-Managed Keys with Microsoft Fabric SQL Database"
description: "This news post introduces customer-managed keys (CMK) for Microsoft Fabric SQL Database, explaining how organizations can leverage Azure Key Vault to control data encryption, enhance compliance, and manage access within Fabric workspaces. The article covers CMK setup, integration with Transparent Data Encryption, and verification queries for successful encryption. User testimonials highlight real-world benefits and ease of implementation, emphasizing both security improvements and architectural advances."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/announcing-public-preview-customer-managed-keys-in-fabric-sql-database/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-11-19 08:00:00 +00:00
permalink: "/news/2025-11-19-Using-Customer-Managed-Keys-with-Microsoft-Fabric-SQL-Database.html"
categories: ["Azure", "ML", "Security"]
tags: ["Access Management", "AI", "AI Projects", "Asymmetric Key", "Auditability", "Azure", "Azure Key Vault", "Compliance", "Customer Managed Keys", "Data Governance", "Data Security", "DEK", "Encryption", "Encryption Query", "Fabric Integration", "Microsoft Fabric", "ML", "News", "Regulatory Standards", "Secure Development", "Security", "SQL Database", "Transparent Data Encryption", "Workspaces"]
tags_normalized: ["access management", "ai", "ai projects", "asymmetric key", "auditability", "azure", "azure key vault", "compliance", "customer managed keys", "data governance", "data security", "dek", "encryption", "encryption query", "fabric integration", "microsoft fabric", "ml", "news", "regulatory standards", "secure development", "security", "sql database", "transparent data encryption", "workspaces"]
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
