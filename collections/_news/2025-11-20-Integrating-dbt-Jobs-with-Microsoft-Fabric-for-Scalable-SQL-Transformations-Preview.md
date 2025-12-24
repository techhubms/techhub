---
layout: "post"
title: "Integrating dbt Jobs with Microsoft Fabric for Scalable SQL Transformations (Preview)"
description: "This announcement details the preview release of dbt Job integration within Microsoft Fabric. Learn how Fabric enables dbt users to natively author, orchestrate, and manage SQL-based data transformations with enterprise-grade governance, CI/CD, and security. The guide covers main features, configuration steps, and the roadmap for future enhancements, including dbt Fusion integration."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/dbt-job-in-microsoft-fabric-ship-trustworthy-sql-models-faster-preview/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-11-20 12:30:00 +00:00
permalink: "/news/2025-11-20-Integrating-dbt-Jobs-with-Microsoft-Fabric-for-Scalable-SQL-Transformations-Preview.html"
categories: ["Azure", "ML"]
tags: ["Admin Portal", "Authentication", "Azure", "CI/CD", "Data Engineering", "Data Factory", "Data Transformation", "Dbt", "Distributed Architecture", "Enterprise Security", "Entra ID", "Governance", "Microsoft Fabric", "ML", "Monitoring", "News", "Operationalization", "Scheduling", "SQL Models"]
tags_normalized: ["admin portal", "authentication", "azure", "cislashcd", "data engineering", "data factory", "data transformation", "dbt", "distributed architecture", "enterprise security", "entra id", "governance", "microsoft fabric", "ml", "monitoring", "news", "operationalization", "scheduling", "sql models"]
---

Microsoft Fabric Blog introduces the preview of dbt Job integration, enabling data engineers to create, orchestrate, and secure SQL-based transformations directly within Fabric's Data Factory.<!--excerpt_end-->

# Integrating dbt Jobs with Microsoft Fabric for Scalable SQL Transformations (Preview)

Microsoft Fabric now supports **dbt Jobs** natively inside Fabric's Data Factory. This preview feature allows data engineers and analysts to bring their existing dbt workflows into the Microsoft ecosystem, leveraging SQL-based data transformation with robust security and operational tools.

## Key Features

- **Native dbt Authoring**: Create, manage, and maintain dbt projects directly within Fabric Data Factory, eliminating the need for local CLI tools or custom dependencies.
- **Governed & Secure**: Integrates with Entra ID authentication and supports workspace roles and row-level, column-level, and object-level security (RLS/CLS/OLS) for enterprise-grade compliance and governance.
- **Unified Experience**: Manage all data pipeline activities—including dbt transformations—in a single, secure interface alongside Fabric’s low-code and code-first tools.
- **Integrated Testing & Documentation**: Built-in testing and model documentation makes it easier to validate and audit your pipelines.
- **Scalable Collaboration**: Leverage modular, version-controlled, and reusable SQL models for effective teamwork across analytics and engineering.
- **Operational Excellence**: Schedule, monitor, and automate dbt job execution using Fabric's distributed, serverless runtime.

## Collaborative Roadmap

Microsoft is closely collaborating with dbt Labs to bring **dbt Fusion** capabilities to Microsoft Fabric, aiming for greater developer performance and features targeted to launch in 2026.

## Getting Started

1. **Enable dbt Jobs Preview**:
    - Access the **Fabric Admin Portal** via Settings.
    - Under **Tenant Settings**, expand **Users can create dbt job items**.
    - Toggle to 'Enabled' and apply across your organization.
    - Preview will gradually roll out to all regions by December 2025.
2. **Create and Manage dbt Jobs**:
    - Once enabled, dbt jobs can be authored and scheduled directly from Fabric workspaces.

For more visual guidance, reference the provided screenshots and GIF demos on setup and job creation.

## Why dbt in Fabric?

- **SQL-native**: Use familiar SQL syntax for transformations.
- **Built-in Version Control & Testing**: Improve trust and manage risk in analytics projects.
- **No Extra Tooling**: All dependencies are managed in the Fabric cloud environment.
- **Secure by Default**: Every operation inherits enterprise governance settings by design.

## Resources

- [dbt job overview](https://aka.ms/dbt-job-intro)

---

By embedding dbt natively in Microsoft Fabric, data teams can streamline workflow, ensure compliance, and rapidly ship trustworthy data models at enterprise scale.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/dbt-job-in-microsoft-fabric-ship-trustworthy-sql-models-faster-preview/)
