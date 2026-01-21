---
external_url: https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/azure-data-factory-sql-managed-instance-and-ssis-implementation/ba-p/4459525
title: Enterprise-Scale Data Integration with Azure Data Factory, SQL Managed Instance, and SSIS
author: PeterLo
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-10-06 21:02:48 +00:00
tags:
- Azure Data Factory
- Azure SQL Managed Instance
- Cloud Solution Architecture
- Data Engineering
- Data Integration
- Data Pipelines
- Enterprise Edition
- Microsoft Entra ID
- Microsoft Fabric
- Power BI
- Self Hosted Integration Runtime
- SQL Server Integration Services
- SSIS
- Virtual Network
section_names:
- azure
- ml
---
PeterLo presents a thorough enterprise data integration guide using Azure Data Factory, SQL Managed Instance, and SSIS, guiding engineering teams through secure cloud deployment and end-to-end pipeline setup.<!--excerpt_end-->

# Enterprise-Scale Data Integration with Azure Data Factory, SQL Managed Instance, and SSIS

*Author: PeterLo*

This step-by-step guide is designed for cloud solution architects and data engineering teams implementing enterprise data solutions on Azure. It covers the integration of Azure Data Factory (ADF) with Azure SQL Managed Instance (SQLMI), the configuration and deployment of SQL Server Integration Services (SSIS) on Azure, and end-to-end data pipeline integration with Microsoft Fabric (Power BI). All instructions reference official Microsoft Learn documentation for further details.

## 1. Enabling the Azure Data Factory Instance

- **Create an Azure Data Factory**: [Get started with Azure Data Factory](https://learn.microsoft.com/en-us/azure/data-factory/quickstart-create-data-factory)
- **Permissions**: Assign **Contributor, Owner, or Administrator** roles on your subscription/resource group. [Role requirements](https://learn.microsoft.com/en-us/azure/data-factory/concepts-roles-permissions#permissions-to-create-data-factory-instances)

## 2. Granting Initial Access to the Data Engineering Team

- **Set ADF Permissions**: [How to set permissions in ADF?](https://learn.microsoft.com/en-us/answers/questions/2120792/how-to-set-permissions-in-adf)
- **Assign Data Factory Contributor Role**: This allows your team to author pipelines and manage connections. [Data Factory Contributor details](https://learn.microsoft.com/en-us/azure/data-factory/concepts-roles-permissions#permissions-to-create-and-manage-resources-within-data-factory)

## 3. Connecting Azure SQL Managed Instance with ADF

- **Networking Options**
  - Use managed virtual networks with managed private endpoints. [More info](https://learn.microsoft.com/en-us/azure/data-factory/managed-virtual-network-private-endpoint#managed-private-endpoints)
  - For hybrid scenarios, use a [self-hosted integration runtime](https://learn.microsoft.com/en-us/azure/data-factory/create-self-hosted-integration-runtime?tabs=data-factory).
- **Data Movement**
  - [Copy and transform data in Azure SQL Managed Instance](https://learn.microsoft.com/en-us/azure/data-factory/connector-azure-sql-managed-instance?tabs=data-factory#prerequisites) using ADF pipelines.

## 4. Creating and Configuring SSIS Runtime

- **Prerequisites**
  - [Create an Azure-SSIS integration runtime](https://learn.microsoft.com/en-us/azure/data-factory/create-azure-ssis-integration-runtime#prerequisites)
- **Provisioning**
  - [Provision the Azure-SSIS integration runtime](https://learn.microsoft.com/en-us/azure/data-factory/tutorial-deploy-ssis-packages-azure)
  - Choose [Enterprise Edition](https://learn.microsoft.com/en-us/azure/data-factory/how-to-configure-azure-ssis-ir-enterprise-edition) for advanced features
  - [Enable Microsoft Entra authentication](https://learn.microsoft.com/en-us/azure/data-factory/enable-aad-authentication-azure-ssis-ir#enable-microsoft-entra-authentication-on-azure-sql-database) for secure access
  - Join the runtime to a virtual network; create a subnet in the SQLMI VNET as required

## 5. Running SSIS Packages in ADF

- **Deploy and Run**
  - [Deploy and run SSIS packages in Azure](https://learn.microsoft.com/en-us/sql/integration-services/lift-shift/ssis-azure-lift-shift-ssis-packages-overview?view=sql-server-ver17)
  - [Migrate on-premises SSIS workloads](https://learn.microsoft.com/en-us/azure/data-factory/scenario-ssis-migration-overview#four-storage-types-for-ssis-packages) to Azure Data Factory SSIS

## 6. Sending Data to Microsoft Fabric (Power BI)

- **Ingest Data**
  - Use [ADF Copy activity to ingest data into Fabric (Power BI)](https://learn.microsoft.com/en-us/azure/data-factory/how-to-ingest-data-into-fabric-from-azure-data-factory)

## 7. Using ADF in Fabric for Mature, Deployment-Ready Workloads

- **ADF Integration in Fabric**
  - [Mount Azure Data Factory items in Microsoft Fabric](https://learn.microsoft.com/en-us/fabric/data-factory/migrate-pipelines-azure-data-factory-item) for production workloads

## References

All steps and best practices are sourced from Microsoft Learn:

- Azure Data Factory documentation: <https://learn.microsoft.com/en-us/azure/data-factory/>
- SQL Managed Instance: <https://learn.microsoft.com/en-us/azure/azure-sql/managed-instance/>
- SSIS on Azure: <https://learn.microsoft.com/en-us/azure/data-factory/ssis-overview>
- Microsoft Fabric integration: <https://learn.microsoft.com/en-us/fabric/data-factory/>

---
*Published by PeterLo, October 6, 2025*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/azure-data-factory-sql-managed-instance-and-ssis-implementation/ba-p/4459525)
