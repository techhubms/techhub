---
layout: post
title: Auditing Features for Fabric SQL Database (Preview)
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/announcing-public-preview-auditing-for-fabric-sql-database/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-11-21 08:00:00 +00:00
permalink: /ml/news/Auditing-Features-for-Fabric-SQL-Database-Preview
tags:
- Access Control
- Audit Logs
- Auditing
- Compliance
- Data Governance
- Fabric SQL Database
- HIPAA
- Microsoft Fabric
- OneLake
- Retention Policy
- Role Based Access
- SOX
- T SQL
section_names:
- azure
- ml
- security
---
Microsoft Fabric Blog details new auditing features for Fabric SQL databases. This article shows how organizations can strengthen security, meet compliance, and gain operational insights using audit logs stored in OneLake.<!--excerpt_end-->

# Auditing Features for Fabric SQL Database (Preview)

The Microsoft Fabric Blog introduces the public preview of auditing for Fabric SQL databases—a key capability for organizations needing enhanced security, compliance, and operational transparency in their data environments.

## Why Auditing Matters

Auditing is fundamental for data governance. With Fabric SQL Database auditing, you can:

- **Track and log every database activity:** Know who accessed data, when, and in what way.
- **Support compliance requirements:** Meet industry regulations such as HIPAA and SOX.
- **Enable robust threat detection:** Spot unauthorized activities and enable forensic investigations.

## Key Auditing Features

- **Flexible Configuration:**
  - Choose from auditing all actions, specific scenarios (e.g., permission changes, login attempts, data reads/writes), or custom action groups and predicate filters.
- **Seamless Log Access in OneLake:**
  - Audit logs are stored securely in OneLake, and can be accessed via T-SQL or OneLake Explorer.
- **Role-Based Access Control:**
  - Both Fabric workspace roles and SQL permissions control who can configure and view audit logs.
- **Custom Retention Policies:**
  - Tailor log retention according to organizational needs.

## How Auditing Works

Audit logs are written to a secure, read-only OneLake folder, referenced by workspace and artifact IDs. Even if databases move between logical servers, log consistency is maintained. Query logs using the T-SQL function:

```sql
select event_time, action_id, statement, file_name, application_name
from sys.fn_get_audit_file_v2('https://onelake.blob.fabric.microsoft.com/<<workspace id>>/<<artifact id>>/Audit/sqldbauditlogs/', default, default, default, default)
```

Access is tightly controlled at both the workspace and SQL database levels to ensure only authorized users can view or manage audit data.

## Common Use Cases

- **Compliance Monitoring:** Validate audit trails for regulatory frameworks.
- **Security Investigations:** Track key events like permission changes and failed login attempts.
- **Operational Insights:** Focus auditing on specific operations or test retention settings.
- **Role-Based Visibility:** Ensure audit data is accessible only to appropriate roles.

## Getting Started

Enable auditing via the 'Manage SQL Auditing' blade in the Fabric Portal:

- Select your auditing scenario
- Set desired retention
- Optionally define custom filters
- Manage everything in an approachable interface

### Additional Resources

- [Auditing for Azure SQL Database and Azure Synapse Analytics](https://learn.microsoft.com/azure/azure-sql/database/auditing-overview?view=azuresql)

## Visual Overview

![Audit Log Storage in OneLake](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/11/audit-1-300x249.png)

---

For more guidance and details, consult official Microsoft documentation and stay tuned for updates as auditing for Fabric SQL Database evolves.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/announcing-public-preview-auditing-for-fabric-sql-database/)
