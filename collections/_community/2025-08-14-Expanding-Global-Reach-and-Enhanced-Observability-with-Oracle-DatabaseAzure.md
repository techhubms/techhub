---
external_url: https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/expanding-global-reach-and-enhancing-observability-with-oracle/ba-p/4443650
title: Expanding Global Reach and Enhanced Observability with Oracle Database@Azure
author: bhbandam
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-08-14 22:42:37 +00:00
tags:
- Azure Monitor
- Azure Regions
- Azure Security Center
- Cloud Scalability
- Compliance
- Exadata
- Germany North
- Hybrid Cloud
- Kusto Query Language
- Log Analytics
- Microsoft Sentinel
- Observability
- Operational Monitoring
- Oracle Database@Azure
- Security Information And Event Management
section_names:
- azure
- security
---
bhbandam, co-authoring with Jeni Mattson, outlines new global regions and advanced monitoring capabilities for Oracle Database@Azure, showcasing how Azure Monitor and Microsoft Sentinel enhance operational visibility and security for enterprise deployments.<!--excerpt_end-->

# Expanding Global Reach and Enhanced Observability with Oracle Database@Azure

**Authors:** bhbandam, Jeni Mattson (Principal Product Manager, Exadata and Database Cloud Product Management)

## Introduction

Microsoft and Oracle have announced expanded regional availability and new observability features for Oracle Database@Azure. The platform is now available in 23 Azure regions worldwide, with Germany North as the latest addition. This helps customers meet their data residency needs, improve latency, and strengthen application resiliency across diverse geographies.

## Regional Expansion

- **Oracle Database@Azure** now operates in 23 Azure regions, the widest reach among hyperscalers.
- Recent expansion includes Germany North.
- Supported regions: Australia East, Australia Southeast, Brazil South, Canada Central, Central India, Central US, East US, East US 2, France Central, Germany North, Germany West Central, Italy North, Japan East, Japan West, North Europe, Southeast Asia, South Central US, Sweden Central, UK South, UK West, West US, West US2, and West US3.
- Plans for expansion to 10 more regions by end of 2025 enhance global scalability and business continuity.

## Enhanced Observability and Security

Expanded capabilities include:

- **Integration with Azure Monitor and Microsoft Sentinel:** Organizations can now send Oracle Exadata VM Cluster, Infrastructure, and Data Guard logs directly to Azure native monitoring and security platforms.
- **Consolidated Log Management:** Centralized operational and database logs for improved visibility into health, performance, and user activities.
- **Advanced Security Features:** Enhanced threat detection and compliance monitoring are enabled via Microsoft Sentinel and Azure Security Center, providing a robust SIEM and SOAR solution.
- **Dashboards and Querying:** Deep analysis and custom dashboards are supported through Log Analytics and Kusto Query Language.
- **Compliance and Auditing Support:** Log retention and change/access tracking help organizations sustain compliance using Azure policy frameworks.

### Supported Log Types

- Exadata VM Cluster Life Cycle Management Logs
- Exadata Database Logs
- Exadata Infrastructure Logs
- Exadata Data Guard Logs

## Getting Started

- Enable the observability feature in the Azure Portal.
- Configure settings and integrate with relevant partner solutions.
- Onboard logs to Microsoft Sentinel and customize analysis using Log Analytics workspaces.

**Resources:**

- [Detailed setup guide](https://learn.microsoft.com/en-us/azure/oracle/oracle-db/oracle-exadata-database-dedicated-infrastructure-logs?branch=main&branchFallbackFrom=pr-en-us-304189)
- [Azure Monitor Logs Documentation](https://learn.microsoft.com/en-us/azure/azure-monitor/logs/data-platform-logs)
- [Log Analytics Workspace Overview](https://learn.microsoft.com/en-us/azure/azure-monitor/logs/log-analytics-workspace-overview)
- [Onboard to Microsoft Sentinel](https://learn.microsoft.com/en-us/azure/sentinel/quickstart-onboard?tabs=defender-portal)

## Conclusion

The expanded reach and enhanced observability in Oracle Database@Azure allow enterprises to unify monitoring, strengthen security postures, and simplify compliance. By leveraging Azure-native services, organizations gain deeper insight into their cloud environments and can proactively manage operations across complex hybrid architectures.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/oracle-on-azure-blog/expanding-global-reach-and-enhancing-observability-with-oracle/ba-p/4443650)
