---
layout: "post"
title: "Azure Managed Grafana 12: New Authentication, Faster Log Analysis, Enhanced Monitoring"
description: "Azure Managed Grafana 12 introduces current-user Entra authentication, improved query building for Azure Monitor logs, seamless Prometheus metrics exploration, and enhanced database dashboards for Azure PostgreSQL and SQL. This update empowers teams with finer-grained access control, faster insights from monitoring data, and streamlined database observability in Azure deployments."
author: "aayodeji"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-observability-blog/introducing-azure-managed-grafana-12/ba-p/4500673"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-03-13 16:54:19 +00:00
permalink: "/2026-03-13-Azure-Managed-Grafana-12-New-Authentication-Faster-Log-Analysis-Enhanced-Monitoring.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Azure", "Azure Data Explorer", "Azure DevOps", "Azure Managed Grafana", "Azure Monitor", "Azure SQL", "Community", "Database Monitoring", "DevOps", "Entra ID Authentication", "Grafana 12", "Kusto Query Language", "Log Analytics", "Logging", "Managed Identities", "Metrics", "Monitoring Dashboards", "Observability", "OpenTelemetry", "PostgreSQL", "Prometheus", "Security", "SQL Managed Instance"]
tags_normalized: ["azure", "azure data explorer", "azure devops", "azure managed grafana", "azure monitor", "azure sql", "community", "database monitoring", "devops", "entra id authentication", "grafana 12", "kusto query language", "log analytics", "logging", "managed identities", "metrics", "monitoring dashboards", "observability", "opentelemetry", "postgresql", "prometheus", "security", "sql managed instance"]
---

aayodeji covers the major upgrades in Azure Managed Grafana 12, highlighting new Entra authentication, faster Azure Monitor log query workflows, Prometheus metric improvements, and streamlined database monitoring for Azure environments.<!--excerpt_end-->

# Azure Managed Grafana 12: What's New

Azure Managed Grafana 12 is now available, bringing a suite of new features to enhance observability, security, and analytics for teams monitoring Azure-based environments.

## Key Features in Grafana 12

- **Current-User Entra Authentication**: You can now configure Entra ID (Azure Active Directory) authentication so queries to supported Azure data sources use the permissions of the currently signed-in user. This enables least-privilege access and simplifies compliance, while keeping options open for Managed Identities and Service Principals where needed. Supported data sources include:
    - Azure Monitor
    - Azure Data Explorer
    - Azure Monitor Managed Service for Prometheus

- **Faster Log Analysis**: Upgrades to Azure Monitor logs integration introduce a new query builder, improved visualization, and enhanced Explore experience. Users can now build and refine logs queries with a click-to-build interface—no need to handwrite Kusto Query Language (KQL). Grafana Explore now supports up to 30,000 records per query for rapid analysis, quicker searches, and smoother navigation of large datasets.

- **Prometheus Query Enhancements**: Prometheus metrics users benefit from improved drill-down functionality, prefix/suffix filtering, group-by label support, and new features for OpenTelemetry and native histogram data. OTel mode streamlines complex label joins for OTLP metrics, providing a more accessible and powerful troubleshooting experience.

- **New Database Monitoring Dashboards**: Updated pre-built dashboards for Azure Database for PostgreSQL, Azure SQL Databases, and SQL Managed Instance let teams quickly achieve visibility into key database health and usage metrics. These dashboards accelerate monitoring setup and help users extract actionable insights with less manual configuration.

## Getting Started

To use Grafana 12, create a new Azure Managed Grafana instance or upgrade your current one via the Azure portal. Enable current-user Entra authentication on supported data sources for stricter access control. Try out the new Azure Monitor logs query builder in the Explore workspace, and leverage new database dashboards for immediate insights into Azure-native database performance.

For more details and upgrading instructions, refer to Microsoft's official documentation: [Upgrade Azure Managed Grafana to Grafana 12](https://learn.microsoft.com/en-us/azure/managed-grafana/how-to-upgrade-grafana-12?tabs=azure-portal).

---

**About the Author:**

aayodeji is a Microsoft community contributor focusing on Azure cloud services, observability, and security best practices.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/introducing-azure-managed-grafana-12/ba-p/4500673)
