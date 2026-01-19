---
layout: post
title: Previewing OneLake Table APIs for Microsoft Fabric
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/now-in-preview-onelake-table-apis/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-10-13 07:00:00 +00:00
permalink: /ml/news/Previewing-OneLake-Table-APIs-for-Microsoft-Fabric
tags:
- Analytics
- Apache Iceberg
- Cloud Data
- Data Engineering
- Data Integration
- Data Lake
- Delta Lake
- Iceberg REST Catalog
- Microsoft Fabric
- Microsoft OneLake
- OneLake Table APIs
- Open Table Formats
- Programmatic Data Access
- REST API
section_names:
- azure
- ml
---
Microsoft Fabric Blog introduces the OneLake Table APIs preview, allowing developers and data engineers to programmatically manage and integrate tables in Microsoft OneLake using open standards like Apache Iceberg.<!--excerpt_end-->

# OneLake Table APIs (Preview) for Microsoft Fabric

Microsoft OneLake offers a unified data lake for organizations, integrated directly into Microsoft Fabric. With the preview of the OneLake Table APIs, developers and data engineers can now:

- Programmatically list and fetch details about tables and schemas in OneLake
- Integrate with open-source analytics ecosystems using familiar APIs
- Build custom services and applications that interact directly with OneLake tables

## OneLake Table APIs Overview

OneLake aims to simplify data management and eliminate silos by providing a secure, open foundation for analytics workloads. The new Table API endpoint enables seamless integration of data into open analytics workflows, with a focus on connectivity and ease of management.

### Main Features

- List and retrieve metadata for tables and schemas
- REST API support for streamlined automation and integration
- Compatibility with Apache Iceberg REST Catalog (IRC) specification

## Integration with Apache Iceberg REST Catalog

Initially, OneLake Table APIs support the [Iceberg REST Catalog (IRC) specification](https://iceberg.apache.org/rest-catalog-spec/), enabling:

- Standards-based management of Iceberg tables
- Reuse of existing Iceberg-compatible tools and workflows (e.g., Snowflake, DuckDB, PyIceberg)

Planned future updates will expand support to Delta Lake operations.

## Getting Started

To work with OneLake Table APIs:

1. Refer to the [OneLake table APIs for Iceberg documentation](https://aka.ms/OneLakeIrcDocs)
2. Use provided examples to:
   - Set up an Iceberg REST Catalog client
   - Connect to OneLake APIs
   - Create and list tables programmatically

These guides address a variety of tools and libraries, helping you quickly integrate OneLake into your analytics or data engineering workflows.

## Importance and Roadmap

By supporting open standards (Apache Iceberg today, Delta Lake soon), OneLake facilitates interoperable, flexible analytics and machine learning solutions. The APIs empower you to:

- Build analytics pipelines
- Enable machine learning workflows
- Create custom data-driven applications

Further API enhancements and broader ecosystem compatibility are planned, extending integration opportunities for Microsoft Fabric users.

## Feedback & Next Steps

Microsoft invites feedback on the Table APIs to help shape ongoing development. Try the new capabilities and share your experiences via [Microsoft Fabric Ideas](https://ideas.fabric.microsoft.com/).

For full documentation and practical examples, see: [Use Iceberg tables with OneLake](https://aka.ms/OneLakeIrcDocs).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/now-in-preview-onelake-table-apis/)
