---
external_url: https://blog.fabric.microsoft.com/en-US/blog/31267/
title: Optimizing Permissions with OneLake Security ReadWrite Access
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-12-08 11:00:00 +00:00
tags:
- Access Control
- Collaboration
- Data Architecture
- Data Governance
- Data Security
- Enterprise Data
- Granular Permissions
- Lakehouse
- Microsoft Fabric
- OneLake
- ReadWrite Permissions
- Role Based Security
- Schema Level Access
- Storage Solutions
- Azure
- Security
- News
section_names:
- azure
- security
primary_section: azure
---
Microsoft Fabric Blog outlines how role-based ReadWrite permissions in OneLake enable secure, collaborative data governance for organizations with varied access needs.<!--excerpt_end-->

# Optimizing Permissions with OneLake Security ReadWrite Access

Data teams often struggle to find the right mix between open collaboration and safeguarding sensitive information. As organizations grow, data naturally splits across multiple domains, each with its own sensitivity requirements. Some teams need access to everything, some only to a curated subset. Historically, this meant splitting storage or risking overexposure.

## Addressing Access Challenges in Microsoft Fabric

At Microsoft, Fabric's data and insights platform had faced these same hurdles—managing access across a single Lakehouse used for diverse business scenarios, some involving confidential datasets. Previously, trade-offs were common: either fragment the storage or over-expose data just to keep processes agile.

With the introduction of **ReadWrite permissions** (now in public preview), this balance can be achieved with more flexibility and security. The new model is based on real-world implementation at Microsoft, and this guide shows the resulting architecture and security benefits.

## Consolidated Access Model: Three Broad Security Groups

- **Contributor:** Engineers with full read/write access to all Lakehouse data.
- **Reader:** Business users, after access reviews, who can only read 'cooked' data.
- **Restricted:** Engineers who can read and write only a defined subset of data and cannot access or see sensitive schemas.

This setup allows organizations to centralize their data but strictly control who can see—and modify—specific parts, using human-friendly security groups as access boundaries.

## Example: Production Storage Architecture

The Unified Lakehouse brings together various data solutions for the business. Multiple schemas represent the data lifecycle:

- **Curate:** Staged incoming data
- **Dataprod:** Cooked, production-ready dimensions and facts
- **Tabular:** Output for semantic modeling
- **Sensitive\_ schemas:** Hold confidential information, shielded from most users

The diagrams referenced in the article (see original post for images) illustrate:

- How 'Restricted' roles receive ReadWrite access solely to non-sensitive schemas (Curate, Dataprod, Tabular)
- How overall architecture keeps sensitive data isolated
- How access can be aligned with specific business workflows

## Why This Matters

This approach streamlines collaboration and security. Teams no longer need to maintain fragmented storage or risk sensitive data leaks. Role-based, granular access at the schema level makes data governance more predictable and collaboration scalable.

## Next Steps and Guidance

- Review your organization's current Lakehouse access patterns
- Map users to role-based security groups aligned with their responsibilities
- Leverage schema-level permissions to minimize risk while enabling agility
- For more, see the [Table and folder security in OneLake documentation](https://learn.microsoft.com/en-us/fabric/onelake/security/table-folder-security)

By adopting these principles in Microsoft Fabric and OneLake, data teams can securely support the needs of growing businesses, striking the right compromise between collaboration and control.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/31267/)
