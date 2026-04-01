---
tags:
- Access Control
- Audit Logging
- Azure
- Azure Key Vault
- CMK
- Compliance
- Customer Managed Keys
- Data At REST Encryption
- Database Encryption Key
- DEK
- Fabric SQL Database
- Key Rotation
- Microsoft Fabric
- ML
- News
- Security
- SQL
- Sys.dm Database Encryption Keys
- TDE
- Transparent Data Encryption
section_names:
- azure
- ml
- security
external_url: https://blog.fabric.microsoft.com/en-US/blog/customer-managed-keys-cmk-in-fabric-sql-database-generally-available/
author: Microsoft Fabric Blog
primary_section: ml
feed_name: Microsoft Fabric Blog
title: Customer-managed keys (CMK) in Fabric SQL Database (Generally Available)
date: 2026-03-26 11:00:00 +00:00
---

Microsoft Fabric Blog announces general availability of customer-managed keys (CMK) for Fabric SQL Database, explaining how Azure Key Vault-backed keys work with Transparent Data Encryption and how to verify encryption status via a DMV query.<!--excerpt_end-->

## Customer-managed keys (CMK) in Fabric SQL Database (Generally Available)

*If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at all of our FabCon and SQLCon announcements across both Fabric and our database offerings:* https://aka.ms/FabCon-SQLCon-2026-news

Customer-managed keys (CMK) in **Fabric SQL Database** is now generally available, letting organizations control encryption keys for data stored in Fabric workspaces.

## Why customer-managed keys matter

Microsoft Fabric encrypts all data-at-rest using **Microsoft-managed keys** by default. For stricter governance and regulatory requirements, **CMK** adds more control by allowing you to use your own **Azure Key Vault** keys to encrypt SQL database data in Fabric workspaces.

With CMK, you get:

- **Key ownership and rotation control**
- **Granular access management**
- **Auditability of key usage**
- **Compliance** with industry-specific encryption standards

## Seamless integration with Transparent Data Encryption (TDE)

Once CMK is configured for a Fabric workspace, **Transparent Data Encryption (TDE)** is automatically enabled for all SQL databases in that workspace (including **tempdb**). This provides:

- Real-time encryption/decryption of **data, backups, and transaction logs**
- Page-level encryption using a symmetric **Database Encryption Key (DEK)**
- DEK protection via the **customer-managed asymmetric key** stored in **Azure Key Vault**

![Diagram illustrating encryption process for Azure SQL data protected by TDE. It shows TDE protector CMK encrypting Database Encryption Key (DEK), which decrypts SQL database in Fabric that stores data in encrypted Azure storage, with arrows indicating encryption and decryption flow.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/diagram-illustrating-encryption-process-for-azure.png)

*Figure: Encryption process for Azure SQL data protected by TDE.*

No manual steps are required after configuration: encryption starts automatically and applies to both existing and newly created databases in the workspace.

## Get started

Follow the Microsoft Learn guidance to enable CMK for a Fabric workspace:

- [Customer-managed keys for Fabric workspaces](https://learn.microsoft.com/fabric/security/workspace-customer-managed-keys)

## Query to verify successful CMK encryption

After CMK is enabled for the workspace, Fabric automatically encrypts:

1. Existing SQL databases in that workspace
2. New SQL databases created in that workspace going forward

To confirm that a specific database is encrypted, run:

```sql
SELECT DB_NAME(database_id) as DatabaseName, *
FROM sys.dm_database_encryption_keys
WHERE database_id <> 2
```

Interpretation:

- A database is encrypted if `encryption_state_desc` shows **"ENCRYPTED"** (or **"ENCRYPTION_IN_PROGRESS"** during encryption).
- `encryptor_type` should be **ASYMMETRIC_KEY**.
- If the database is not encrypted, it will not appear in this DMV.

![Screenshot of query result set of the SYS.DM_DATABASE_ENCRYPTION_KEYS DMV. Notable entries include AES algorithm with a 256-bit key, highlighted terms "ASYMMETRIC," "ENCRYPTED," and "COMPLETE," indicating successful encryption processes.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-query-result-set-of-the-sys-dm_datab.png)

*Figure: Result set of the query to verify if your database is successfully encrypted.*

## Learn more

- [Data encryption in SQL database](https://learn.microsoft.com/fabric/database/sql/encryption)
- [Customer-managed keys for Fabric workspaces](https://learn.microsoft.com/fabric/security/workspace-customer-managed-keys)
- [How to encrypt Fabric SQL Database with Customer Managed Keys (video)](https://youtu.be/1ffSH5g1t-Y?si=_pyHAqby4GQ-6ztK)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/customer-managed-keys-cmk-in-fabric-sql-database-generally-available/)

