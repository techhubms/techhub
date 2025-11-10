---
layout: "post"
title: "Migrate or Modernize Applications Using Azure Migrate: New Features and Application-Aware Approaches"
description: "This article by Shikher offers an in-depth exploration of Azure Migrate's latest capabilities for application-aware cloud migration. It covers holistic planning, dependency mapping, code-level integration (including GitHub Copilot assessment), security insights, wave planning, and unified tooling for migrating and modernizing multi-tiered applications to Microsoft Azure. The guide targets architects, IT admins, and developers aiming for predictable and secure cloud adoption."
author: "Shikher"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-migration-and/migrate-or-modernize-your-applications-using-azure-migrate/ba-p/4468587"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-10 16:03:15 +00:00
permalink: "/2025-11-10-Migrate-or-Modernize-Applications-Using-Azure-Migrate-New-Features-and-Application-Aware-Approaches.html"
categories: ["AI", "Azure", "Coding", "DevOps", "GitHub Copilot", "Security"]
tags: [".NET Migration", "AI", "AKS", "App Service", "Application Assessment", "Application Modernization", "Azure", "Azure Database Migration Service", "Azure Migrate", "Azure Security", "Azure SQL Database", "Azure VM", "CAST Highlight", "Cloud Migration", "Coding", "Community", "Dependency Mapping", "DevOps", "DevOps Tools", "GitHub Copilot", "GitHub Copilot Assessment", "Microsoft Defender For Cloud", "Refactor", "Rehost", "Replatform", "Security", "Wave Planning"]
tags_normalized: ["dotnet migration", "ai", "aks", "app service", "application assessment", "application modernization", "azure", "azure database migration service", "azure migrate", "azure security", "azure sql database", "azure vm", "cast highlight", "cloud migration", "coding", "community", "dependency mapping", "devops", "devops tools", "github copilot", "github copilot assessment", "microsoft defender for cloud", "refactor", "rehost", "replatform", "security", "wave planning"]
---

Shikher provides a comprehensive overview of Azure Migrate's new application-focused features, showing how architects and developers can leverage code insight tools like GitHub Copilot to drive secure, efficient modernization and migration projects to Azure.<!--excerpt_end-->

# Enabling Migration or Modernization of Your Multi-Tiered Applications Using Azure Migrate

## Introduction

Enterprises aiming to leverage cloud benefits—such as enhanced security, flexibility, and cost-efficiency—require a comprehensive approach to migration and modernization. Azure Migrate has evolved to offer application-centric capabilities, helping organizations plan, assess, and execute migrations holistically, focusing on applications rather than just individual servers.

## Key New Features in Azure Migrate

- **Multi-Server Dependency Mapping**: Visualizes interactions between servers across a datacenter. This helps teams identify application boundaries, manage dependencies, and plan migrations without missing critical components.
- **Software and Security Insights**: Offers built-in detection of outdated software and unpatched vulnerabilities. The platform provides actionable recommendations, like leveraging Microsoft Defender for Cloud and Azure Update Manager, enabling IT and security teams to address issues proactively.
- **Application Definition & Import**: Apps can be logically grouped and managed as first-class entities in migration projects, simplifying project organization and analysis.
- **ROI Analysis**: Delivers tools for business case development, enabling estimation of costs, projected savings, and evaluating options like Rehost, Replatform, or Refactor.
- **Holistic Application Assessments**: Azure Migrate assesses all infrastructure, data, and web tiers of an application, generating a comprehensive migration plan. This includes strategies, readiness checks, target services, SKUs, costs, and appropriate migration tools.
- **Code Insight Integration**: Application assessments can now accommodate code-level analysis via GitHub Copilot assessment and CAST Highlight. These tools help uncover technical debt and identify code changes required for modernization, with insights feeding directly into migration recommendations.
- **Wave Planning & End-to-End Execution**: Migrations can be sequenced and managed in phases (waves), supported by tooling for Azure VMs, databases, and web apps. This approach enables collaboration between architects, developers, and IT admins within a single platform.

## Deep Dive into Capabilities

### Multi-Server Dependency Mapping and Application Definition

Azure Migrate's agentless analysis maps server connections, visualizes datacenter topologies, and helps define applications by grouping relevant workloads. Users can refine groupings through import/export and assign metadata such as criticality and complexity.

### Proactive Software and Security Insights

The platform flags outdated OS or software and suggests upgrades or migration paths. Security insights integrate with Defender for Cloud and Azure Update Manager for vulnerability remediation, letting teams mitigate risks as part of the migration strategy.

### Business Case and ROI for Modernization

Azure Migrate's Business Case Generator evaluates modernization value, applying spend analysis across strategies (Rehost, Replatform, Refactor) and providing cost projections.

### Holistic Application Assessments

Assessments cover servers, databases, and web apps together, offering unified migration recommendations. Cloud architects can prioritize applications, select optimal migration approaches, and minimize execution surprises.

### Code-Level Insights with GitHub Copilot Assessment and CAST Highlight

Migration readiness is increasingly tied to application source code. By integrating assessment reports from GitHub Copilot and CAST Highlight, Azure Migrate incorporates actionable insights—such as required modernizations or technical debt—into its plans. This enables collaborative and realistic migration execution between infrastructure and development teams.

### Wave Planning and Integrated Migration Execution

Large migrations benefit from sequenced phase execution. Wave Planning enables batch migrations, integrated tool launches (e.g., Server Migration, Database Migration Service), and unified tracking across all migration activities within Azure Migrate's interface.

## Getting Started

All discussed features are available (preview as of November 7, 2025). Users can begin by creating Azure Migrate projects and leveraging the new application-aware experience to manage their migration from assessment to execution.

## Supported Platforms and Workloads

Azure Migrate supports discovery and assessment for workloads such as Windows and Linux Server, SQL Server, PostgreSQL, .NET on IIS, and Java on Tomcat—across VMware, bare-metal, AWS, GCP, and more. Assessment tooling supports Azure VMs, AVS, SQL Database/MI, App Service (code/containers), AKS, and others.

---

**Author:** Shikher  
**Source:** Azure Migration and Modernization Blog  
**Updated:** Nov 10, 2025

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-migration-and/migrate-or-modernize-your-applications-using-azure-migrate/ba-p/4468587)
