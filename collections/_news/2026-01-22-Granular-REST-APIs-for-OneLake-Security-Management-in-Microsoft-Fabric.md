---
external_url: https://blog.fabric.microsoft.com/en-US/blog/granular-apis-for-onelake-security-preview/
title: Granular REST APIs for OneLake Security Management in Microsoft Fabric
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2026-01-22 09:00:00 +00:00
tags:
- API Integration
- Authorization
- CI/CD
- Cloud Security
- Governance
- Microsoft Entra
- Microsoft Fabric
- OneLake
- REST API
- Role Management
- Security Automation
- Security Policy
- Azure
- DevOps
- Security
- News
section_names:
- azure
- devops
- security
primary_section: azure
---
Microsoft Fabric Blog introduces granular REST APIs for OneLake security role management, empowering developers and platform teams with precise, automation-friendly controls for security and governance.<!--excerpt_end-->

# Granular REST APIs for OneLake Security Management in Microsoft Fabric

Microsoft Fabric has expanded OneLake's security capabilities by introducing new granular REST API endpoints for role management. These enhancements give developers and platform engineers greater precision and flexibility when managing security policies programmatically.

## Key Features

- **Granular Role APIs**: In addition to the existing batch role API, Fabric now offers discrete Create, Get, and Delete APIs for managing individual roles.
- **Programmatic Security**: Developers can automate individual security changes—for example, create or update a single role definition, retrieve existing roles, or delete obsolete roles—without needing to handle the entire collection each time.
- **Automation in DevOps**: The granular APIs facilitate incremental security updates, allowing changes to be integrated into CI/CD pipelines, reviewed, and rolled back at a granular level, mirroring standard software development practices.
- **Fine-Grained Permissions**: Roles can now express precise permissions, such as granting scoped access to specific tables or storage paths, and can include Microsoft Entra (formerly Azure AD) principals.
- **Support for Validation and Auditing**: The new Get API enables targeted policy inspection for auditing, validation, or drift detection, without pulling the full configuration.

## Developer and Platform Impact

- **Composability**: The new APIs are well-suited for SaaS partners, enterprises practicing 'policy as code', and internal platform teams building governance tools.
- **Lifecycle Management**: With discrete endpoints for create/update, retrieve, and delete, it is now easier to maintain clean and secure configurations over time, especially in dynamic environments.
- **Backward Compatibility**: The batch role API remains available for bulk operations, ensuring existing automation continues to work.
- **Integration-Friendly**: The APIs are designed for open, interoperable, and developer-first scenarios, making them a strong fit for automation and governance ecosystems.

## Resources

- [Microsoft Fabric documentation](https://learn.microsoft.com/en-us/fabric/)
- [OneLake security API reference](https://learn.microsoft.com/rest/api/fabric/core/onelake-data-access-security)

---

These improvements underscore Microsoft Fabric's commitment to comprehensive, automation-ready security for cloud-scale data platforms.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/granular-apis-for-onelake-security-preview/)
