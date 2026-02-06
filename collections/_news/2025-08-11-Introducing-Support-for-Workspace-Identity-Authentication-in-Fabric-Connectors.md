---
external_url: https://blog.fabric.microsoft.com/en-US/blog/announcing-support-for-workspace-identity-authentication-in-new-fabric-connectors-and-for-dataflow-gen2/
title: Introducing Support for Workspace Identity Authentication in Fabric Connectors
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-08-11 10:00:00 +00:00
tags:
- Authentication
- Azure Active Directory
- Azure Blob Storage
- Azure Data Lake Storage
- Azure Synapse Analytics
- Credential Management
- Data Governance
- Data Pipeline
- Data Security
- Dataflows Gen2
- Fabric Connectors
- Identity Management
- Microsoft Entra ID
- Microsoft Fabric
- Role Based Access Control
- Secure Data Access
- Semantic Models
- Service Principal
- Workspace Identity
- Azure
- ML
- Security
- News
- Machine Learning
section_names:
- azure
- ml
- security
primary_section: ml
---
The Microsoft Fabric Blog team, including co-author Meenal Srivastva, announces expanded workspace identity authentication support in Microsoft Fabric connectors, detailing security improvements and implementation steps.<!--excerpt_end-->

# Introducing Support for Workspace Identity Authentication in Fabric Connectors

*Co-author: Meenal Srivastva*

Managing data source access securely is critical for organizations using Microsoft Fabric. The introduction of workspace identity authentication provides teams with a seamless and secure way to manage credentials and data access across their enterprise environments.

## What is Workspace Identity in Microsoft Fabric?

- **Workspace Identity** is an automatically managed service principal associated with most Fabric workspaces (excluding My Workspaces).
- When created, Fabric generates a service principal in Microsoft Entra ID (formerly Azure Active Directory), streamlining identity management for secure, trusted access to firewall-enabled storage accounts.
- This approach eliminates the need for storing individual credentials and offers fine-grained access control and comprehensive auditing.

## Recent Security Change: Role Assignment

As of July 27, 2025, new workspace identities will no longer be assigned the default contributor role automatically. Admins must explicitly assign roles as needed. Existing workspace identities will also lose their default contributor assignments. This change enhances the principle of least privilege and strengthens organizational security.

## Expanded Support: Where Can You Use Workspace Identity Authentication?

Workspace identity authentication is now available for new connectors and features in Microsoft Fabric, including:

- **Data Pipeline**
- **Copy Job**
- **Semantic Models**
- **Dataflows Gen2 (CI/CD)**

This support allows centralized access control across a range of connectors, reducing credential sprawl.

### Supported Connectors (Table Excerpt)

- **Azure Analysis Services** (Semantic Models)
- **Azure Blob Storage** (Copy Job, Data Pipeline, Dataflows Gen2)
- **Azure Cosmos DB (SQL API)** (Copy Job, Data Pipeline, Dataflows Gen2)
- **Azure Data Explorer (Kusto)** (Copy Job, Data Pipeline, Dataflows Gen2)
- **Azure Data Lake Storage Gen2** (Copy Job, Data Pipeline, Dataflows Gen2)
- **Azure Synapse Analytics** (Copy Job, Data Pipeline, Dataflows Gen2)
- **Azure Tables, Dataverse, SharePoint, SQL Server** (All supported)
- Others: Dynamics 365, Dynamics AX, Dynamics CRM, Viva Insights, Web (each varies by Fabric item)

For the full, up-to-date connector support matrix, see the [Fabric documentation](https://learn.microsoft.com/fabric/security/workspace-identity).

## Walkthrough: Using Workspace Identity to Connect Dataflow Gen2 to Azure Blob Storage

### Step 1: Create Workspace Identity

1. In your workspace (not "My Workspace"), go to settings.
2. Select the "Workspace identity" tab.
3. Click "+ Workspace identity" to create.
4. Alternatively, use the [Workspaces – Provision Identity REST API](https://learn.microsoft.com/rest/api/fabric/core/workspaces/provision-identity?tabs=HTTP).

### Step 2: Assign Storage Account Permissions

1. Open the Azure portal and go to your Blob storage account.
2. Find the **Access control (IAM)** section and select **Role assignments**.
3. Click **Add**, choose an appropriate role (e.g., Storage Blob Data Reader), and assign it to the workspace identity.
4. Finish by clicking **Review + assign**.

### Step 3: Bind the Connection in Dataflows Gen2

1. Create a Dataflow Gen2 in Fabric. Enable Git integration and deployment pipelines as needed.
2. Use the "Get Data" experience ([documentation](https://learn.microsoft.com/power-query/connectors/azure-blob-storage#connect-to-azure-blob-storage-from-power-query-online)) or configure via "Manage Connections and Gateways," choosing workspace identity authentication.
3. Reference the connection in Get Data and select Workspace Identity as the authentication method.

![Workspace Identity auth selection in Dataflow Gen2](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/08/Screenshot-2025-08-04-at-2.01.12 PM-1024x579.png)

## What's Next?

- More connectors and features will integrate workspace identity authentication.
- Review the latest updates in the [authentication documentation](https://learn.microsoft.com/fabric/security/workspace-identity-authenticate).
- Provide feedback or suggest improvements via [Fabric Ideas](https://ideas.fabric.microsoft.com/).

---

**References:**  

- [Workspace identity in Microsoft Fabric](https://learn.microsoft.com/fabric/security/workspace-identity)  
- [Fabric Blog Announcement](https://blog.fabric.microsoft.com/blog/fabric-workspace-identity-removing-default-contributor-access-for-workspace-identity?ft=All)

*Contributors: Meenal Srivastva, Microsoft Fabric Blog Team*

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/announcing-support-for-workspace-identity-authentication-in-new-fabric-connectors-and-for-dataflow-gen2/)
