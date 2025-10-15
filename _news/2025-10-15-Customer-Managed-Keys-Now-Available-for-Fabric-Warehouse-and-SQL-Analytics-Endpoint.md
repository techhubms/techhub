---
layout: "post"
title: "Customer-Managed Keys Now Available for Fabric Warehouse and SQL Analytics Endpoint"
description: "This update introduces Customer-Managed Keys (CMK) for Fabric Warehouse and SQL Analytics Endpoint, enabling organizations to control and manage encryption keys themselves. By leveraging Azure Key Vault integration, enterprises gain enhanced data security, compliance alignment, and governance over critical metadata and operational artifacts within Fabric workloads."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/bringing-customer-managed-keys-to-fabric-warehouse-and-sql-analytics-endpoint/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-10-15 09:00:00 +00:00
permalink: "/2025-10-15-Customer-Managed-Keys-Now-Available-for-Fabric-Warehouse-and-SQL-Analytics-Endpoint.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Azure Key Vault", "BYOK", "Compliance", "Customer Managed Keys", "Data Protection", "Database Security", "Governance", "Microsoft Fabric", "News", "Security", "SPN", "SQL Analytics Endpoint", "Warehouse Encryption"]
tags_normalized: ["azure", "azure key vault", "byok", "compliance", "customer managed keys", "data protection", "database security", "governance", "microsoft fabric", "news", "security", "spn", "sql analytics endpoint", "warehouse encryption"]
---

Microsoft Fabric Blog details the introduction of Customer-Managed Keys for Fabric Warehouse and SQL Analytics Endpoint, empowering organizations to enhance security and compliance through Azure Key Vault integration.<!--excerpt_end-->

# Bringing Customer-Managed Keys to Fabric Warehouse and SQL Analytics Endpoint

With the latest update, organizations can now gain increased control and assurance over the encryption of their most critical analytical workloads within Microsoft Fabric.

## Why Customer-Managed Keys Matter

- **Enhanced Control:** Move beyond Microsoft-managed keys and enable your organization to control and manage encryption keys for compliance and governance.
- **Bring Your Own Key (BYOK):** Store and manage your own encryption keys in [Azure Key Vault](https://learn.microsoft.com/azure/key-vault/general/about-keys-secrets-certificates) and apply them to Fabric Warehouse and SQL Analytics Endpoint.
- **Stronger Security:** Enforce lifecycle operations like key rotation and revocation to meet regulatory requirements.
- **Compliance Alignment:** Address internal and industry-specific compliance needs for data protection and auditability.

## Scope of Encryption

- **Preview Features:** Currently, Customer-Managed Keys encrypt metadata, such as SQL objects (tables, views, stored procedures, and functions).
- **Planned Enhancements:** Future updates will expand encryption to cover saved queries and further operational artifacts.
- **Continuous Protection:** All Fabric data at rest, including OneLake storage, continues to be encrypted. CMK allows you to select which keys are used for this data.

## Organizational Benefits

- **Protection Beyond Raw Data:** Not just the data but also the logical structure and metadata are protected under your management.
- **Granular Lifecycle Operations:** You can rotate, disable, or revoke keys as needed, with Fabric respecting these changes across workloads.
- **Consistent Enterprise Governance:** Align Fabric security controls with those in other Azure services (e.g., Storage, Synapse, Key Vault).

## How to Enable CMK

1. Create or select an encryption key in Azure Key Vault.
2. Create a Service Principal Name (SPN) to facilitate communication between Fabric and Azure Key Vault.
3. Enable CMK within your Fabric Workspace's security settings.
4. Monitor configuration progress directly in the workspace security interface.
5. Upon successful encryption, check the CMK status (should read “Active”).

## Transparency and Control

By bringing CMK into Fabric Warehouse and SQL Analytics Endpoint, you are empowered to decide which keys protect your organization's metadata, determine rotation schedules, and maintain full control over your cryptographic governance. Integration with Azure Key Vault ensures you govern every aspect of the key lifecycle, enhancing trust and compliance.

## Learn More

- [About keys, secrets, and certificates in Azure Key Vault](https://learn.microsoft.com/azure/key-vault/general/about-keys-secrets-certificates)
- [Customer-managed keys in Fabric Workspaces](https://learn.microsoft.com/fabric/security/workspace-customer-managed-keys)
- [Customer-managed keys for Azure Storage](https://learn.microsoft.com/azure/storage/common/customer-managed-keys-overview)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/bringing-customer-managed-keys-to-fabric-warehouse-and-sql-analytics-endpoint/)
