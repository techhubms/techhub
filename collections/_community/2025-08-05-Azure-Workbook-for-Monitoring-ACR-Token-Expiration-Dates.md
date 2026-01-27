---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/azure-workbook-for-acr-tokens-and-their-expiration-dates/ba-p/4438249
title: Azure Workbook for Monitoring ACR Token Expiration Dates
author: Jamesdld23
feed_name: Microsoft Tech Community
date: 2025-08-05 04:30:00 +00:00
tags:
- ACR
- Authentication
- Automation
- Azure Container Registry
- Azure Monitor Workbook
- Azure REST API
- Bash Scripting
- PowerShell
- Resource Graph
- Token Expiration
- Workbook Deployment
section_names:
- azure
- devops
- security
primary_section: azure
---
Jamesdld23 explains how to monitor Azure Container Registry token expiration dates through Azure REST API, Azure Workbooks, and automation techniques.<!--excerpt_end-->

## Introduction

This guide by Jamesdld23 explores how to monitor [Azure Container Registry (ACR)] tokens and their expiration dates, leveraging Azure REST APIs and Azure Monitor Workbooks. The process covers authentication, querying for token details, visualizing results, and automating deployment using scripts and templates.

## Monitoring ACR Tokens and Expiration

### Overview

- Use the Azure REST API to list ACR tokens and retrieve their credentials and expiration dates.
- Visualize and interact with this data in Azure Monitor Workbooks for ongoing operations visibility.

### Manual API Workflow

1. **Authenticate and obtain an Azure access token**:
   You need Azure AD service principal credentials to authenticate.
2. **List ACR tokens**:
   Call the REST API endpoint to enumerate all tokens for a registry.
3. **Retrieve credentials and expiration dates**:
   Parse the API response to find password creation and expiry information for each token.

#### Example Bash Script

```bash
# !/bin/bash

# Azure AD application (service principal) credentials

CLIENT_ID=""
CLIENT_SECRET=""
TENANT_ID=""

# Azure subscription and resource details

SUBSCRIPTION_ID=""
RESOURCE_GROUP=""
REGISTRY_NAME=""

# Authenticate and obtain the access token

ACCESS_TOKEN=$(curl -s -X POST -H "Content-Type: application/x-www-form-urlencoded" \
-d "grant_type=client_credentials&client_id=${CLIENT_ID}&client_secret=${CLIENT_SECRET}&scope=https://management.azure.com/.default" \
"https://login.microsoftonline.com/${TENANT_ID}/oauth2/v2.0/token" | jq -r .access_token)

# List ACR tokens and credentials

curl -s -X GET -H "Authorization: Bearer ${ACCESS_TOKEN}" -H "Content-Type: application/json" \
"https://management.azure.com/subscriptions/${SUBSCRIPTION_ID}/resourceGroups/${RESOURCE_GROUP}/providers/Microsoft.ContainerRegistry/registries/${REGISTRY_NAME}/tokens?api-version=2023-01-01-preview" | jq .
```

## Visualizing with Azure Monitor Workbook

Azure Workbooks can serve as a dashboard to query and display token information visually and interactively across your tenant's container registries.

### Workbook Setup Steps

1. **Create a new workbook** in Azure Monitor ([link](https://portal.azure.com/#view/Microsoft_Azure_Monitoring/AzureMonitoringBrowseBlade/~/workbooks)).
2. **Add a Data Query using Azure Resource Graph**:
   Use this query to list all your Azure Container Registries:

   ```kusto
   resources
   | where type == "microsoft.containerregistry/registries"
   | project id, name, resourceGroup, location, skuName=sku.name
   ```

3. **Configure Export Parameters** in Advanced Settings:
    - Export field: `id`, Parameter name: `containerRegistryId`, Type: Resource picker
    - Export field: `name`, Parameter name: `containerRegistryName`, Type: Resource picker
4. **Add a secondary Data Query using Azure Resource Manager**:
    - Path: `{containerRegistryId}/tokens?api-version=2023-01-01-preview`
    - Select a registry to make this dynamic.
5. **Format the API result as a table**:
    - JSON Path Table: `$.value[*]`
    - Columns to show:
      - `tokenName` via `$.name`
      - `password1_creationTime` and `password1_expiry` via `$.properties.credentials.passwords[?(@.name=='password1')].creationTime` and `.expiry`
      - `password2_creationTime` and `password2_expiry` via `$.properties.credentials.passwords[?(@.name=='password2')].creationTime` and `.expiry`
6. **Customize chart titles and finalize settings** as needed.
7. **Save the workbook** for future use.

## Deploying as Code

You can automate the deployment of this workbook using Azure Resource Manager (ARM) templates and PowerShell. Example deployment variables and command:

```powershell
# Variables

$AzureRmSubscriptionName = "Azure subscription 1"
$RgName = "monResourceGroup"
$workbookDisplayName = "dmo acr tokens"
$workbookSourceId = "Azure Monitor"
$workbookType = "workbook"
$templateUri = "https://raw.githubusercontent.com/JamesDLD/AzureRm-Template/master/Create-AzWorkbookAcrTokens/template.json"

# Connectivity

Connect-AzAccount # (if not using Cloud Shell)
$AzureRmContext = Get-AzSubscription -SubscriptionName $AzureRmSubscriptionName | Set-AzContext -ErrorAction Stop
Select-AzSubscription -Name $AzureRmSubscriptionName -Context $AzureRmContext -Force -ErrorAction Stop

# Deployment

Write-Host "Deploying : $workbookType-$workbookDisplayName in the resource group : $RgName" -ForegroundColor Cyan
New-AzResourceGroupDeployment -Name $(("$workbookType-$workbookDisplayName").replace(' ', '')) -ResourceGroupName $RgName `
-TemplateUri $TemplateUri `
-workbookDisplayName $workbookDisplayName `
-Confirm -ErrorAction Stop
```

A ready-made ARM template is available in the [JamesDLD/AzureRm-Template repository](https://github.com/JamesDLD/AzureRm-Template/blob/master/Create-AzWorkbookAcrTokens/README.md).

## Conclusion

Azure Monitor Workbooks and the REST API give you flexibility to track token credentials and expiration for your Azure Container Registries. Automating this with scripts and templates accelerates visibility and governance over container access security.

---

For more details and updates, refer to Jamesdld23's GitHub repository.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/azure-workbook-for-acr-tokens-and-their-expiration-dates/ba-p/4438249)
