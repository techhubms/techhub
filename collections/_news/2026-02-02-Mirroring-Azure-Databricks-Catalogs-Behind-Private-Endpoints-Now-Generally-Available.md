---
external_url: https://blog.fabric.microsoft.com/en-US/blog/mirroring-azure-databricks-catalogs-from-azure-databricks-workspaces-behind-private-endpoints-generally-available/
title: Mirroring Azure Databricks Catalogs Behind Private Endpoints Now Generally Available
author: Microsoft Fabric Blog
primary_section: ml
feed_name: Microsoft Fabric Blog
date: 2026-02-02 09:00:00 +00:00
tags:
- Analytics
- Azure
- Azure Databricks
- Cloud Networking
- Data Governance
- Data Mirroring
- Enterprise Security
- Microsoft Fabric
- ML
- Network Isolation
- News
- Private Endpoints
- Regulatory Compliance
- SaaS Integration
- Unity Catalog
- Virtual Network
- VNet Data Gateway
- Machine Learning
section_names:
- azure
- ml
---
Microsoft Fabric Blog announces that organizations can now mirror Azure Databricks catalogs from workspaces secured behind private endpoints. This update, authored by the Microsoft Fabric Blog team, enables secure, enterprise-grade data integration for analytics in regulated Azure environments.<!--excerpt_end-->

# Mirroring Azure Databricks Catalogs from Private Workspaces Now Generally Available

Enterprises are increasingly adopting restricted network topologies for analytics, keeping Azure Databricks workspaces behind private endpoints to meet enterprise and regulatory standards. Historically, this blocked integration with Microsoft Fabric’s Mirrored Databricks Catalog due to its reliance on public network connectivity.

Microsoft Fabric now fully supports [mirroring Azure Databricks catalogs](https://learn.microsoft.com/fabric/mirroring/azure-databricks) from private endpoint-only Databricks workspaces—this capability is generally available. It leverages the **Virtual Network (VNet) data gateway** to create secure, private connections from Fabric to Azure Databricks, without public IP exposure.

## Why Is This Important?

- Customers no longer face a trade-off between strong network isolation and a unified analytics platform.
- Organizations using Azure Databricks in secured VNet environments can directly mirror Unity Catalog metadata and data into Fabric.

## Key Benefits

- **Secure, Private Connectivity**: Data and metadata move between Fabric and Databricks through private IPs on the Azure backbone, with no data traversing the public internet.
- **Enterprise Compliance & Protection**: Supports regulatory and enterprise security needs with private endpoint-only integration.
- **Simplified IT Networking**: Uses standard Azure building blocks—private endpoints, managed identities, VNet data gateways—avoiding custom networking or proxies.
- **Consistent Fabric Experience**: Aligns Mirrored Azure Databricks Catalog with other Fabric workload connectivity models, simplifying governance.

## Technical Architecture

The integration is facilitated by deploying the VNet data gateway within a customer-controlled virtual network. This gateway routes Fabric’s requests to Databricks via a private endpoint, ensuring traffic remains within Azure’s secure infrastructure.

> ![Architecture diagram](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/01/image-16.png)
> _Mirrored Azure Databricks Catalog connecting to a VNET-injected Databricks workspace using a private endpoint_

## Getting Started

Organizations with Databricks workspaces behind private endpoints can begin mirroring catalogs in Fabric right away. Official setup instructions:

- [Mirroring Azure Databricks catalogs in Fabric](https://learn.microsoft.com/fabric/mirroring/azure-databricks-private-endpoint)

## Additional Resources

- [Mirroring Azure Databricks Unity Catalog overview](https://learn.microsoft.com/fabric/mirroring/azure-databricks)
- [Databricks private connectivity setup](https://learn.microsoft.com/azure/databricks/security/network/front-end/front-end-private-connect)
- [VNet data gateway creation](https://learn.microsoft.com/data-integration/vnet/create-data-gateways)

This release helps organizations unify their analytics landscape with best-in-class network security using Azure-native constructs.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/mirroring-azure-databricks-catalogs-from-azure-databricks-workspaces-behind-private-endpoints-generally-available/)
