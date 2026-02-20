---
external_url: https://techcommunity.microsoft.com/t5/finops-blog/what-s-new-in-finops-toolkit-13-january-2026/ba-p/4493090
title: 'What’s New in FinOps Toolkit 13: January 2026 Feature Updates'
author: Michael_Flanakin
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-09 09:21:43 +00:00
tags:
- Azure
- Azure Optimization Engine
- Bicep
- Community
- Community Engagement
- Compression
- Cost Management
- Data Lake Storage
- DevOps
- Enterprise Compliance
- Export Automation
- FinOps Toolkit
- Key Vault
- Parquet
- Power BI
- PowerShell
- RBAC
- Security
section_names:
- azure
- devops
- security
---
Michael Flanakin summarizes the latest enhancements in FinOps Toolkit 13, covering Azure-focused improvements in cost management, security, export automation, and extensibility for cloud practitioners and DevOps professionals.<!--excerpt_end-->

# What’s New in FinOps Toolkit 13: January 2026 Feature Updates

**Author:** Michael Flanakin

## Overview

FinOps Toolkit 13 delivers a range of improvements for cloud efficiency practitioners, DevOps teams, and anyone seeking to maximize the value of Microsoft Azure. This release strengthens stability and usability across core FinOps Hubs, boosts Power BI and Azure Optimization Engine integration, and introduces new export automation options and compliance controls to meet enterprise cloud requirements.

## Highlights

### Improved Documentation and Troubleshooting

- Expanded documentation now covers new scenarios such as Data Lake Storage connectivity beyond Power BI, Azure scope configuration for cross-cloud data ingestion, and step-by-step guidance for removing private networking to optimize costs.
- Provides updated troubleshooting guides for Data Explorer ingestion issues, enhanced export requirements clarifying dependencies in Power BI reports, and newly documented fields for FOCUS export conversion.

### Power BI Reports and Workbooks

- Bugs resolved around tag expansion (including special characters), accuracy for unattached disk counts, and scope calculations for usage reports.
- Improvements in SQL Managed Instance core reporting via AHB workbook updates and reservation workbook reliability.

### Azure Optimization Engine Advancements

- Default database backup redundancy is now set to LRS for improved cost control, aligning with best practices for non-paired Azure regions.
- More accurate underutilized disk recommendations for Premium SSD V2 and better instance size flexibility handling.

### Enhanced Security and Compliance

- Introduction of an optional `enablePurgeProtection` parameter for Key Vault, addressing enterprise compliance while retaining flexibility during development.
- Role-based access has been tightened by replacing the 'User Access Administrator' with the more restrictive 'RBAC Administrator' role, specifically scoped to the Managed Exports app.

### Export Automation With Parquet Support

- PowerShell users can leverage the new `-Format` parameter in `New-FinOpsCostExport` to create columnar parquet exports (with optional snappy or gzip compression) for faster queries in Azure Data Explorer, Synapse, and Fabric.
- Export automation now enables easier scaling and more efficient storage, especially in complex Azure environments.

### Community Features

- Regularly hosted FinOps Toolkit Office Hours – an open forum for getting advice, reporting problems, and sharing feedback with both Microsoft engineers and other practitioners.
- Features and documentation expansions driven by community suggestions (e.g., thanks to GitHub contributor gorkomikus for adding Parquet export support).

### Architecture and Extensibility

- Bicep templates have been refactored into modular app components to enable more flexible deployments and custom extensions in the future.
- Extensibility remains a key focus, empowering organizations to deploy only what they need and integrate with existing infrastructures.

## Looking Forward

- Enhancements planned across AI-based automation for cost management, richer recommendation engines in FinOps hubs, and continued integration of Azure Optimization Engine.
- Introduction of premium services for guided toolkit deployment, customization, and scaling securely in the enterprise.

## Get Involved

- Join the [FinOps Toolkit Office Hours](https://aka.ms/ftk/officehours)
- Explore documentation: [FinOps Toolkit Docs](https://aka.ms/ftk/docs?WT.mc_id=finops-260210-micflan)
- Contribute tips or report issues: [Community Discussion](https://aka.ms/ftk/discuss?WT.mc_id=finops-260210-micflan)

---

*Updated: February 9, 2026 — Version 1.0*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/finops-blog/what-s-new-in-finops-toolkit-13-january-2026/ba-p/4493090)
