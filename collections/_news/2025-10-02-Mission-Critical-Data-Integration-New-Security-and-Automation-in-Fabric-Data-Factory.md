---
external_url: https://blog.fabric.microsoft.com/en-US/blog/mission-critical-data-integration-whats-new-in-fabric-data-factory/
title: 'Mission-Critical Data Integration: New Security and Automation in Fabric Data Factory'
author: Microsoft Fabric Blog
viewing_mode: external
feed_name: Microsoft Fabric Blog
date: 2025-10-02 17:00:00 +00:00
tags:
- Azure Key Vault
- Compliance
- Data Factory
- Data Integration
- Data Orchestration
- Data Security
- Enterprise Data
- Hybrid Cloud
- Microsoft Fabric
- Network Isolation
- PowerShell Automation
- Private Link
- REST API
- VNet Gateway
- Workspace Identity Authentication
section_names:
- azure
- ml
- security
---
Microsoft Fabric Blog outlines recent mission-critical enhancements to Fabric Data Factory, focusing on security, compliance, and automation for enterprise data integration, with details curated by the Microsoft Fabric Blog team.<!--excerpt_end-->

# Mission-Critical Data Integration: What’s New in Fabric Data Factory

## Why Mission-Critical Features Matter

Organizations operating at enterprise scale require a data integration platform that is resilient, secure, and designed for high-demand workloads. Fabric Data Factory addresses these requirements by enabling unified data orchestration, stringent protection, and seamless hybrid and cloud integration.

## Key Security, Isolation, and Compliance Enhancements

### Workspace Identity Authentication for Connectors (Generally Available)

- Allows secure and seamless data access using managed service principals assigned to each Fabric workspace (excluding My Workspace).
- Enhances credential management and reduces administrative overhead.
- [More info](https://blog.fabric.microsoft.com/blog/announcing-support-for-workspace-identity-authentication-in-new-fabric-connectors-and-for-dataflow-gen2/)

### Workspace Private Link Support (Preview)

- Enables secure, isolated dataflows, pipelines, and copy jobs via Private Link.
- Supports compliance mandates and network isolation via VNet data gateways.
- [Learn more](https://blog.fabric.microsoft.com/blog/fabric-workspace-level-private-link-preview/)

### Azure Key Vault Integration (Generally Available)

- Centralizes secret and credential management for data source connections.
- Minimizes risk of credential sprawl and supports on-premises gateway connections, with VNet gateway support planned.
- [Read details](https://blog.fabric.microsoft.com/en-US/blog/authenticate-to-fabric-data-connections-using-azure-key-vault-stored-secrets-preview/)

### VNet Gateway Support for Pipelines and Copy Jobs

- Ensures all data movements use secure, isolated network routes, rather than exposing data to public networks.
- Strengthens compliance and data boundary management.
- [Further reading](https://blog.fabric.microsoft.com/blog/26862/)

### Connections and Gateways API (Generally Available)

- Offers programmatic management for provisioning and updating connections and gateways via REST API, with support for service principals.
- Facilitates automation and consistent policy enforcement across enterprise workspaces.
- [API announcement](https://blog.fabric.microsoft.com/blog/announcing-the-availability-of-rest-apis-for-connections-and-gateways-in-microsoft-fabric/)

### VNet Gateway Lifecycle Controls

- Enables administrators to manage gateway uptime by starting, stopping, and restarting as required.
- Assists with optimizing resource use and supporting maintenance operations.

### PowerShell Automation for Gateway Management (Preview)

- Provides automation for deployment and ownership transfers using PowerShell modules, improving disaster recovery and scaling operations.
- [PowerShell details](https://learn.microsoft.com/powershell/module/datagateway/?view=datagateway-ps)

### Upcoming Features

- **Snowflake Key-Pair Authentication**: Enables secure, key-based authentication (coming soon).
- **Outbound Access Protection for Data Factory**: Allows organizations to control and monitor outbound data flows, supporting further compliance and network security (coming soon).

## Impact for Enterprises

These enhancements mean Fabric Data Factory is well-suited for organizations in highly regulated fields, such as finance, government, and healthcare, looking to meet strict security and compliance standards in their data operations.

For additional announcements and updates, refer to the [Data Factory announcement blog](https://blog.fabric.microsoft.com/blog/26911/) from Fabric Conference EU 2025.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/mission-critical-data-integration-whats-new-in-fabric-data-factory/)
