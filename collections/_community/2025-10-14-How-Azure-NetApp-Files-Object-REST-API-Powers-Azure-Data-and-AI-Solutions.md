---
layout: "post"
title: "How Azure NetApp Files Object REST API Powers Azure Data and AI Solutions"
description: "This detailed article explores the Azure NetApp Files Object REST API, a newly released capability enabling direct, secure, and real-time integration between enterprise data and Azure’s analytics, AI, and machine learning services. Learn how S3-compatible access unlocks new use cases in Azure Databricks, Microsoft Fabric, OneLake, Power BI, and more, all while maintaining compliance and boosting operational efficiency. The piece covers architecture, implementation, and numerous practical scenarios across industries for streamlining data workflows and accelerating business insights."
author: "GeertVanTeylingen"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-architecture-blog/how-azure-netapp-files-object-rest-api-powers-azure-and-isv-data/ba-p/4459545"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-10-14 15:00:00 +00:00
permalink: "/community/2025-10-14-How-Azure-NetApp-Files-Object-REST-API-Powers-Azure-Data-and-AI-Solutions.html"
categories: ["AI", "Azure", "ML"]
tags: ["AI", "Azure", "Azure AI", "Azure Databricks", "Azure NetApp Files", "Azure OpenAI", "Community", "Compliance", "Data Lake", "Data Science", "Data Security", "Enterprise Analytics", "Microsoft Fabric", "ML", "Object REST API", "OneLake", "Power BI", "Real Time Data Integration", "S3 Compatibility"]
tags_normalized: ["ai", "azure", "azure ai", "azure databricks", "azure netapp files", "azure openai", "community", "compliance", "data lake", "data science", "data security", "enterprise analytics", "microsoft fabric", "ml", "object rest api", "onelake", "power bi", "real time data integration", "s3 compatibility"]
---

GeertVanTeylingen, along with co-authors Sean Luce, Asutosh Panda, and Thomas Willingham, introduces the Azure NetApp Files Object REST API—a robust way to unlock direct data access for advanced analytics, AI, and machine learning in Azure environments.<!--excerpt_end-->

# How Azure NetApp Files Object REST API Powers Azure Data and AI Solutions

## Abstract

The Azure NetApp Files Object REST API lets enterprises expose unstructured file data via an S3-compatible API, enabling direct, secure integration with advanced Azure analytics and AI services. This means organizations can connect applications like Azure Databricks, Azure Machine Learning, Microsoft Fabric, and Power BI to their data—eliminating the need for costly data copies or migrations and ensuring compliance and operational efficiency.

## Introduction

Enterprises increasingly rely on seamless data access to unlock value from analytics and AI. The Azure NetApp Files Object REST API provides real-time, direct connectivity between data stored on Azure NetApp Files and core Azure services, supporting workloads like AI, ML, and analytics with efficiency and flexibility.

## Key Features and Integration

- **S3-Compatible Access**: Access volumes using familiar S3 protocols (reading/writing data), supporting a broad ecosystem of analytics, ML, and AI tools.
- **Direct Connectivity**: Azure Databricks, Fabric, OneLake, and Azure AI/ML services can access data directly, reducing data movement overhead.
- **Multi-Protocol Support**: Access the same data from S3, NFS, or SMB as needed, supporting a wide range of enterprise tools and applications.
- **Security & Compliance**: Enterprise data remains in-place, within Azure NetApp Files' security context, aiding GDPR, HIPAA, and other compliance needs.

## How It Works

1. **Data Exposure via Object REST API**: Unstructured files in Azure NetApp Files are exposed via an S3-compatible REST API.
2. **Integration with Fabric and OneLake**: Azure NetApp Files volumes are virtualized into OneLake, Microsoft's unified data lake inside Microsoft Fabric, enabling all analytics workloads to access the data as tables or lakehouse files.
3. **Direct AI/ML Data Flows**: Azure OpenAI, AI Foundry, and Azure Databricks can train or serve models directly on NetApp Files datasets, without manual transfers.
4. **Analytics Visualization**: Power BI can visualize live data via OneLake/Fabric integrations.

## Use Cases

- **Manufacturing**: Accelerate engineering searches through Azure AI Search, using AI agents to improve precision and speed.
- **Healthcare Analytics**: Real-time healthcare analytics with Azure Databricks Lakehouse powered by NetApp Files data.
- **EDA Workflows**: Boost simulation and debug cycles for electronic design automation.
- **Financial Services**: Use NVIDIA Enterprise AI for rapid, accurate financial document analysis.
- **Industry Extensions**: Supports media editing, genomics research, legal compliance, retail analytics, and energy/geospatial use cases.

## Example Architectures

Data remains on Azure NetApp Files, accessible via the Object REST API (as S3), NFS, or SMB. Microsoft Fabric’s OneLake offers unified access across Azure services, streamlining ingestion, transformation, and reporting within security and compliance boundaries.

## Security and Compliance

- Keeps sensitive data within Azure’s trusted boundaries.
- Maintains compliance for regulated industries without extra data movement.
- Supports enterprise-grade access controls and auditing.

## Learn More and Get Started

- [What's new in Azure NetApp Files](https://learn.microsoft.com/azure/azure-netapp-files/whats-new#october-2025)
- [Understand Azure NetApp Files object REST API access](https://learn.microsoft.com/azure/azure-netapp-files/object-rest-api-introduction)
- [Configure object REST API access in Azure NetApp Files](https://learn.microsoft.com/azure/azure-netapp-files/object-rest-api-access-configure)
- [Connect Azure Databricks to an Azure NetApp Files object REST API-enabled volume](https://learn.microsoft.com/azure/azure-netapp-files/object-rest-api-databricks)
- [Connect OneLake to an Azure NetApp Files volume using object REST API](https://learn.microsoft.com/azure/azure-netapp-files/object-rest-api-onelake)
- [Connect an S3 browser to an Azure NetApp Files object REST API-enabled volume](https://learn.microsoft.com/azure/azure-netapp-files/object-rest-api-browser)

---
Authors: GeertVanTeylingen, Sean Luce, Asutosh Panda, Thomas Willingham

## Summary

Azure NetApp Files Object REST API is a critical new building block for Azure-based analytics, AI, and data science. It simplifies integration, optimizes productivity, and strengthens compliance—making it a preferred solution for modern, data-driven organizations.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-architecture-blog/how-azure-netapp-files-object-rest-api-powers-azure-and-isv-data/ba-p/4459545)
