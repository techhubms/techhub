---
layout: "post"
title: "Decoupling Semantic Models from Mirrored Artifacts in Microsoft Fabric"
description: "Microsoft Fabric is evolving its semantic models by decoupling them from Mirrored artifacts. This change increases flexibility and transparency in data modeling, allowing customers to independently define, version, and manage semantic models on mirrored data. The update enables tailored analytics, improved access, and better organizational agility."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/25332/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-08-05 11:30:00 +00:00
permalink: "/2025-08-05-Decoupling-Semantic-Models-from-Mirrored-Artifacts-in-Microsoft-Fabric.html"
categories: ["Azure", "ML"]
tags: ["Azure", "Business Intelligence", "Data Analytics", "Data Architecture", "Data Modeling", "Data Science", "Microsoft Fabric", "Mirrored Artifacts", "ML", "News", "Semantic Models", "SQL", "Versioning"]
tags_normalized: ["azure", "business intelligence", "data analytics", "data architecture", "data modeling", "data science", "microsoft fabric", "mirrored artifacts", "ml", "news", "semantic models", "sql", "versioning"]
---

This news update from the Microsoft Fabric Blog details a significant shift in how semantic models interact with Mirrored artifacts. The author explains the advantages and phased implementation of decoupling, providing improved flexibility and control for customers.<!--excerpt_end-->

# Decoupling Semantic Models from Mirrored Artifacts in Microsoft Fabric

## Overview

Semantic models within Microsoft Fabric are evolving to work more seamlessly with Mirrored artifacts. This new approach is designed to give users greater flexibility, control, and transparency when handling mirrored data.

## Rationale for Decoupling

Historically, the creation of Mirrored artifacts involved the automatic pairing of a semantic model. While this coupling was convenient for initial setup, it limited the customization and ongoing management of business logic, as well as the synchronization of changes between data storage and its semantic definitions.

To address the diverse needs of customers, Microsoft Fabric is moving toward a decoupled model. This empowers users to define, version, and manage semantic models independently, enabling sophisticated governance over business definitions.

## How the Decoupling Process Works

The transition to decoupled semantic models is structured in two phases:

**Phase 1:**

- New Mirrored artifacts are no longer created with a default, coupled semantic model.
- When creating a new artifact, users can design and manage their semantic model utilizing the tools and frameworks best suited for their analytics needs.
- This phase has already launched.

**Phase 2:**

- Existing Mirrored artifacts will have their default semantic models decoupled.
- Further details for this phase will be released soon.

![Screenshot illustrating the decoupling process](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/07/Screenshot-2025-07-23-142007-300x156.png)

## Impact on Customers

Decoupling semantic models from Mirrored artifacts introduces several key advantages:

- **Layer Separation:** There is now a clear separation between raw data storage and the semantic, business definitions applied to that data. This allows storage and logic to evolve independently for greater flexibility.

- **Customization:** Organizations can build bespoke semantic models that match diverse business units, analytics requirements, or unique use cases, supporting tailored reporting and interpretations.

- **Parallel Models:** Multiple semantic models can be layered atop the same dataset, enabling distinct reporting, BI, and data science needs to be met simultaneously without interference.

- **Raw Data Access:** Users have full access to raw mirrored data, making it possible to query with SQL, BI tools, or data science platforms and leverage complex query features (joins, window functions, custom aggregations).

- **Robust Versioning:** The decoupled approach supports version control for semantic models, making it easy to iterate, develop, and test business logic without impacting others or causing version conflicts with the dataset.

## Conclusion

This update represents a step forward in making semantic models within Microsoft Fabric more open, flexible, and responsive to changing organizational needs. Customers gain the tools and autonomy to innovate and adapt their data strategy more effectively.

For more information, read the [Sunsetting Default Semantic Models – Microsoft Fabric](https://blog.fabric.microsoft.com/en-us/blog/sunsetting-default-semantic-models-microsoft-fabric/) blog post.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/25332/)
