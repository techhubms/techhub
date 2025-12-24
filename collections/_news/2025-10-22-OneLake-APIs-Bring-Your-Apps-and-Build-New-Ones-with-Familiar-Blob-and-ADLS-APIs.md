---
layout: "post"
title: "OneLake APIs: Bring Your Apps and Build New Ones with Familiar Blob and ADLS APIs"
description: "This article introduces OneLake, the unified data lake for Microsoft Fabric, highlighting its support for Azure Data Lake Storage and Blob Storage APIs. It explains how developers and data engineers can migrate workloads, leverage familiar tools, and use standard endpoints without code rewrites. Real-world compatibility and migration strategies are covered with examples."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/onelake-apis-bring-your-apps-and-build-new-ones-with-familiar-blob-and-adls-apis/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-10-22 09:00:00 +00:00
permalink: "/news/2025-10-22-OneLake-APIs-Bring-Your-Apps-and-Build-New-Ones-with-Familiar-Blob-and-ADLS-APIs.html"
categories: ["Azure", "ML"]
tags: ["API Compatibility", "Azure", "Azure Blob Storage", "Azure Data Lake Storage", "Blob API", "C#", "Cloud File Systems", "Data Engineering", "Data Lake", "Data Migration", "Delta Tables", "Developer Tools", "DF API", "Microsoft Fabric", "ML", "News", "On Premises Mirroring", "OneLake"]
tags_normalized: ["api compatibility", "azure", "azure blob storage", "azure data lake storage", "blob api", "csharp", "cloud file systems", "data engineering", "data lake", "data migration", "delta tables", "developer tools", "df api", "microsoft fabric", "ml", "news", "on premises mirroring", "onelake"]
---

Microsoft Fabric Blog explains how OneLake empowers developers and data engineers to integrate existing applications with Microsoft Fabric using familiar Blob and ADLS APIs, simplifying migration and supporting modern analytics.<!--excerpt_end-->

# OneLake APIs: Bring Your Apps and Build New Ones with Familiar Blob and ADLS APIs

**Author:** Microsoft Fabric Blog

## Overview

OneLake is the unified data lake for Microsoft Fabric, designed to streamline data management and boost analytics capabilities across organizations. Its core advantage is seamless compatibility with prevalent Azure Data Lake Storage (ADLS) and Blob Storage APIs, enabling organizations to migrate workloads and modernize analytics without rewriting code.

## Key Features

- **Seamless Migration:** Move existing workloads and applications to OneLake using the same ADLS or Blob Storage APIs, eliminating the need for costly rewrites or changes to integrations.
- **Familiar Tools Support:** Popular tools and SDKs like Azure Storage Explorer remain compatible; scripts and workflows built for ADLS and Blob Storage work out of the box.
- **Unified Data Management:** OneLake unifies data silos, providing a standardized foundation for AI, ML, and analytics initiatives in Microsoft Fabric.

## Using OneLake APIs

Applications can interact with OneLake using standard REST endpoints:

```
https://onelake.dfs.fabric.microsoft.com/<workspace>/<item>.<itemtype>/<path>/<fileName>
https://onelake.blob.fabric.microsoft.com/<workspace>/<item>.<itemtype>/<path>/<fileName>
```

- These endpoints are drop-in replacements for existing services.
- Compatible with DFS or Blob protocols, enabling immediate integration.

## Why It Matters

Organizations have significant investments in cloud data platforms, and migrations can be resource-intensive. With OneLake’s API compatibility:

- Existing tools and codebases work without significant modification
- Focus remains on building new features, not on re-architecting old solutions
- Developers save time and reduce operational disruption

## Practical Example: C# Integration

The [OneLake APIs in Action guide](https://learn.microsoft.com/fabric/onelake/onelake-apis-in-action) offers real-world samples, such as leveraging the Azure Blob Storage and Distributed File System (DFS) libraries in C# with OneLake endpoints. Key demos include:

- Authenticating and accessing data using familiar APIs
- Performance best practices for cloud file systems
- Using [mirroring](https://learn.microsoft.com/fabric/database/mirrored-database/open-mirroring-landing-zone-format) to replicate on-premises data into OneLake Delta tables

## Who Should Use This

- **Developers:** Integrate OneLake into .NET apps using known storage SDKs
- **Data Engineers:** Migrate and manage organizational analytics workloads quickly
- **Organizations:** Unify data, streamline analytics, and support AI/ML initiatives without starting over

## Resources

- [Official OneLake Product Overview](https://blog.fabric.microsoft.com/blog/onelake-your-foundation-for-an-ai-ready-data-estate?ft=All)
- [OneLake Access API Documentation](https://learn.microsoft.com/fabric/onelake/onelake-access-api)
- [OneLake APIs in Action Guide](https://learn.microsoft.com/fabric/onelake/onelake-apis-in-action)
- [Azure Data Lake Storage Gen2](https://learn.microsoft.com/rest/api/storageservices/data-lake-storage-gen2)
- [Blob Storage REST API](https://learn.microsoft.com/rest/api/storageservices/blob-service-rest-api)
- [Azure Storage Explorer](https://azure.microsoft.com/products/storage/storage-explorer/)
- [DFS Libraries for .NET](https://learn.microsoft.com/azure/storage/blobs/data-lake-storage-directory-file-acl-dotnet)

---

This content offers clear action steps for technical readers wanting to use Microsoft Fabric OneLake’s APIs in their existing solutions. It’s tailored for professional developers and data engineers migrating analytics and data workloads to the Microsoft ecosystem.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/onelake-apis-bring-your-apps-and-build-new-ones-with-familiar-blob-and-adls-apis/)
