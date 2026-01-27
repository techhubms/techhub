---
layout: "post"
title: "Granular REST APIs for OneLake Security Management in Microsoft Fabric"
description: "This article details new enhancements to Microsoft Fabric's OneLake security through granular REST APIs for role management. Developers and platform teams can now programmatically create, retrieve, update, and delete security roles, enabling fine-grained automation and improved CI/CD integration for data platform governance and security."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/granular-apis-for-onelake-security-preview/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2026-01-22 09:00:00 +00:00
permalink: "/2026-01-22-Granular-REST-APIs-for-OneLake-Security-Management-in-Microsoft-Fabric.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["API Integration", "Authorization", "Azure", "CI/CD", "Cloud Security", "DevOps", "Governance", "Microsoft Entra", "Microsoft Fabric", "News", "OneLake", "REST API", "Role Management", "Security", "Security Automation", "Security Policy"]
tags_normalized: ["api integration", "authorization", "azure", "cislashcd", "cloud security", "devops", "governance", "microsoft entra", "microsoft fabric", "news", "onelake", "rest api", "role management", "security", "security automation", "security policy"]
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
