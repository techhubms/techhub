---
external_url: https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-copy-data-across-tenants-using-copy-job-in-fabric-data-factory/
title: 'Step-by-Step: Using Copy Job to Move Data Across Tenants in Fabric Data Factory'
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-10-20 12:00:00 +00:00
tags:
- Access Control
- Authentication
- Azure Data Lake Gen2
- Bulk Copy
- Change Data Capture
- Cloud Data Movement
- Copy Job
- Data Factory
- Data Ingestion
- Data Warehouse
- ETL
- Incremental Copy
- Microsoft Entra ID
- Microsoft Fabric
- Role Assignment
- Service Principal
- Table Mapping
- Tenant Transfer
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog presents a practical tutorial for moving data across Azure tenants using Copy job in Fabric Data Factory. Authored by the Microsoft Fabric Blog team, this walkthrough emphasizes secure authentication, configuration, and transfer into a Fabric Data Warehouse.<!--excerpt_end-->

# Step-by-Step: Using Copy Job to Move Data Across Tenants in Fabric Data Factory

**Author:** Microsoft Fabric Blog

## Overview

Copy job in Microsoft Fabric Data Factory streamlines the process of moving data across clouds, on-premises systems, and Azure services. This guide focuses on migrating data securely between different Azure tenants using service principal authentication and highlights scenarios like bulk copy, incremental copy, and change data capture (CDC).

## Scenario

- **Tenant A:** Owns Fabric Data Warehouse and will execute the Copy job.
- **Tenant B:** Hosts the Azure Data Lake Gen2 source account.

The task: Move data from Azure Data Lake Gen2 (Tenant B) into a Fabric Data Warehouse (Tenant A), using a service principal for secure authentication.

## Prerequisites

- **Azure Data Lake Gen2 account** in Tenant B with a registered service principal.
- **Service principal credentials:** Application (client) ID, Directory (tenant) ID, and a client secret.
- **Proper role assignment** for the service principal (e.g., Storage Blob Data Contributor).

## Setting up Azure Data Lake Gen2 with Service Principal

1. Sign into the Azure Portal with Tenant B credentials.
2. Create a Storage Account and add a data container.
3. Register a new application in Azure AD (Microsoft Entra ID) for a service principal.
4. Record the Application (client) ID and Directory (tenant) ID.
5. Generate and securely store a client secret.
6. Assign appropriate RBAC roles (e.g., Storage Blob Data Contributor) to the service principal for the Data Lake account.

## Initiating the Copy Job in Fabric

1. Sign into Microsoft Fabric (app.powerbi.com) using Tenant A credentials.
2. In your Fabric workspace, create a new Copy job.
3. Choose Azure Data Lake Gen2 as the **source**, and select **service principal** as the authentication method.
4. Enter the Tenant ID, Client ID, and Client Secret obtained from setup.
5. Select files to copy from the Data Lake container.
6. Choose the **destination** as Fabric Data Warehouse (Tenant A).
7. Optionally, configure table or column mapping.
8. Select full or incremental copy mode as desired.
9. Review and run the job. Monitor progress in the job panel.
10. After a successful run, validate that data appears in the target Fabric Data Warehouse.

## Key Features and Options

- **Bulk and incremental copy**: Supports one-time and ongoing data sync.
- **CDC replication:** Facilitates change-based data movement.
- **Authentication:** Uses service principals for secure, cross-tenant access.
- **Role-based access control:** Granular permission management.
- **Table/column mapping:** Customize destination schemas.

## Additional Resources

- [What is Copy job in Data Factory – Microsoft Fabric | Microsoft Learn](https://learn.microsoft.com/en-us/fabric/data-factory/what-is-copy-job)
- [Register a Microsoft Entra app and create a service principal](https://learn.microsoft.com/en-us/entra/identity-platform/howto-create-service-principal-portal?utm_source=chatgpt.com)
- [Access storage using a service principal & Microsoft Entra ID](https://learn.microsoft.com/en-us/azure/databricks/connect/storage/aad-storage-service-principal?utm_source=chatgpt.com)
- [Microsoft Fabric documentation](https://aka.ms/FabricBlog/docs)
- [Fabric Community – Copy job discussions](https://community.fabric.microsoft.com/t5/Copy-job/bd-p/db_copyjob)

## Summary

The Copy job capability in Microsoft Fabric Data Factory is a practical tool for data professionals tasked with transferring datasets across Azure tenants. With thorough authentication, configuration, and clear support for multiple workflows (full, incremental, CDC), it enables robust, secure data migration into Fabric Data Warehouse environments.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-copy-data-across-tenants-using-copy-job-in-fabric-data-factory/)
