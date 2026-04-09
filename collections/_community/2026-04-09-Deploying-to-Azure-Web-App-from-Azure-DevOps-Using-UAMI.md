---
section_names:
- azure
- devops
- security
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/deploying-to-azure-web-app-from-azure-devops-using-uami/ba-p/4509800
title: Deploying to Azure Web App from Azure DevOps Using UAMI
author: theringe
primary_section: azure
date: 2026-04-09 05:53:29 +00:00
tags:
- Access Audit Logs
- Access Control (iam)
- AppServiceAuditLogs
- ARM Service Connection
- Azure
- Azure App Service
- Azure DevOps
- Azure Pipelines
- Azure Resource Manager
- Azure Web Apps
- Community
- Deployment Logs
- DevOps
- Diagnostic Settings
- Managed Identity
- Microsoft Entra ID
- RBAC
- Role Based Access Control
- Security
- Service Connections
- User Assigned Managed Identity
- Website Contributor Role
feed_name: Microsoft Tech Community
---

theringe walks through deploying to Azure App Service from Azure DevOps using a user-assigned managed identity (UAMI), including the Azure DevOps service connection setup, required RBAC permissions, and how to validate the deployment identity via AppServiceAuditLogs.<!--excerpt_end-->

# Deploying to Azure Web App from Azure DevOps Using UAMI

This article explains how to deploy code to an **Azure Web App (Azure App Service)** using a newer **Azure DevOps** capability: creating an **ARM (Azure Resource Manager) service connection** that authenticates via a **User Assigned Managed Identity (UAMI)** instead of a service principal.

## Table of contents

1. UAMI Configuration
2. App Configuration
3. Azure DevOps Configuration
4. Logs

## UAMI Configuration

1. Create a **User Assigned Managed Identity**.
2. No extra configuration is required at creation time.
3. Note the identity details (especially the **Object ID**), as it will be referenced later.

## App Configuration

On an existing **Azure Web App**:

1. Enable **Diagnostic Settings**.
2. Configure it to retain relevant logs (for example **Access Audit Logs**). These are used later to validate what identity initiated the deployment.

Next, configure permissions:

1. Go to **Access Control (IAM)**.
2. Assign the previously created **User Assigned Managed Identity** the **Website Contributor** role.

## Azure DevOps Configuration

1. In Azure DevOps, go to:
   - **Project Settings → Service Connections**
2. Create a new **ARM (Azure Resource Manager) connection**.

During creation:

- Select the corresponding **User Assigned Managed Identity**.
- Grant appropriate permissions at the **Resource Group** level.

While setting this up, you will be prompted to sign in again with your own account. The article notes that this authentication shows up later in the deployment/audit logs.

When using this connection in a pipeline with a deployment template, you should see **additional steps in the deployment process** compared to traditional **service principal** authentication.

## Logs

A few minutes after the deployment completes, related records appear in logs.

In the **AppServiceAuditLogs** table:

- The **deployment initiator** shows as the **UAMI Object ID**.
- The **Source** shows as **Azure (DevOps)**.

The interpretation given is:

- The **UAMI** is the identity recorded as initiating the deployment.
- The deployment is run by **Azure DevOps**, and the identity is **authorized under the user context** used during setup.


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/deploying-to-azure-web-app-from-azure-devops-using-uami/ba-p/4509800)

