---
section_names:
- azure
- security
primary_section: azure
date: 2026-04-21 07:46:00 +00:00
title: Leveraging Azure Resource Graph Queries for Azure Storage Configuration
external_url: https://techcommunity.microsoft.com/t5/azure-paas-blog/leveraging-azure-resource-graph-queries-for-azure-storage/ba-p/4509184
tags:
- Accesstier
- Allowblobpublicaccess
- Azure
- Azure Data Lake Storage Gen2
- Azure Portal
- Azure Resource Graph
- Azure Storage
- Cloud Governance
- Community
- Firewall Rules
- Hierarchical Namespace
- KQL
- Kusto Query Language
- Microsoft.storage/storageaccounts
- Minimumtlsversion
- Networkacls
- NFS 3.0
- Publicnetworkaccess
- Resource Graph Explorer
- Security
- SFTP
- Storage Accounts
- Subscription Inventory
- TLS
feed_name: Microsoft Tech Community
author: jainsourabh
---

jainsourabh shares practical Azure Resource Graph (KQL) queries you can run in the Azure Portal to audit Azure Storage account settings across subscriptions, including SFTP, HNS (ADLS Gen2), minimum TLS version, public blob access, NFS 3.0, default access tier, and network exposure.<!--excerpt_end-->

# Leveraging Azure Resource Graph Queries for Azure Storage Configuration

## Scenario

Teams often need a quick, reliable way to check which **Azure Storage** features are enabled across subscriptions (for example **SFTP**, **Hierarchical Namespace (HNS)**, or **default access tiers**).

While you can do this with **PowerShell**, **Azure CLI**, or **REST APIs**, those approaches can be time-consuming due to module setup, frequent updates, and script maintenance.

A faster option is **Azure Resource Graph Explorer**, which lets you query storage account configurations at scale using **Kusto Query Language (KQL)** directly in the Azure Portal—no scripts to maintain.

## Azure Resource Graph Explorer

Azure Resource Graph Explorer lets you run KQL queries from the **Azure Portal** to inspect resource configurations across subscriptions.

All queries below:

- Use the **Resources** table
- Filter on resource type `microsoft.storage/storageaccounts`
- Retrieve configuration properties from the `Microsoft.Storage/storageAccounts` resource schema

## How to open Azure Resource Graph Explorer (quick steps)

1. Sign in to the **Azure Portal**
2. In the global search bar, search for **Resource Graph Explorer**
3. Open **Resource Graph Explorer**
4. Paste a KQL query and select **Run query**

## Queries to analyse Azure Storage account configurations

### 1. Storage accounts with SFTP enabled

Find all storage accounts that have Secure File Transfer Protocol (SFTP) turned on:

```kusto
Resources
| where type =~ "microsoft.storage/storageaccounts"
| where properties.isSftpEnabled == true
| project name, resourceGroup, location
```

Find all storage accounts with SFTP enabled in a specific subscription:

```kusto
Resources
| where type =~ "microsoft.storage/storageaccounts" and subscriptionId =~ "XXXXXXXXXXXXXXXXXXXX"
| where properties.isSftpEnabled == true
| project name, resourceGroup, location
```

**Explanation:** `properties.isSftpEnabled` is a boolean. When it’s `true`, SFTP is enabled. The query returns the account name, resource group, and location.

### 2. Minimum TLS version per storage account

List each storage account alongside its configured minimum TLS version:

```kusto
Resources
| where type =~ "microsoft.storage/storageaccounts"
| project StorageAccount = name, resourceGroup, location,
         MinimumTLS = properties.minimumTlsVersion
```

**Explanation:** `properties.minimumTlsVersion` is a string that sets the minimum TLS protocol version allowed for incoming requests.

### 3. Storage accounts with Hierarchical Namespace (HNS) enabled

Find all storage accounts that have Hierarchical Namespace enabled (Azure Data Lake Storage Gen2):

```kusto
Resources
| where type =~ "microsoft.storage/storageaccounts"
| where properties.isHnsEnabled == true
| project name, resourceGroup, location
```

**Explanation:** `properties.isHnsEnabled` indicates whether HNS is enabled.

### 4. Storage accounts that do NOT allow public blob access

Identify storage accounts where anonymous public read access to blobs is disallowed:

```kusto
Resources
| where type =~ "microsoft.storage/storageaccounts"
| where properties.allowBlobPublicAccess == false
| project name, resourceGroup, location
```

**Explanation:** `properties.allowBlobPublicAccess` controls whether anonymous public read access to blob data is permitted at the account level.

### 5. Storage accounts with NFS 3.0 support enabled

Find all storage accounts that have NFS 3.0 protocol support turned on:

```kusto
Resources
| where type =~ "microsoft.storage/storageaccounts"
| where properties.isNfsV3Enabled == true
| project name, resourceGroup, location
```

**Explanation:** `properties.isNfsV3Enabled` is described in the schema as “NFS 3.0 protocol support enabled if set to true”. NFS 3.0 support lets Linux clients mount Azure Blob Storage via NFS, which can be useful for high-performance computing and large-scale analytics workloads.

### 6. Storage accounts with default access tier

Find all storage accounts and check their default access tier (Hot / Cool):

```kusto
Resources
| where type =~ "microsoft.storage/storageaccounts"
| extend defaultAccessTier = tostring(properties.accessTier)
| project name, resourceGroup, location, kind, sku.name, defaultAccessTier
```

**Explanation:** `properties.accessTier` indicates the default access tier configured for the storage account (for supported account kinds).

### 7. Storage accounts open to all network traffic (no firewall restrictions)

Find storage accounts accessible from any network without virtual network or IP-based firewall rules:

```kusto
Resources
| where type =~ "microsoft.storage/storageaccounts"
| where (properties.publicNetworkAccess == "Enabled"
        or isnull(properties.publicNetworkAccess))
    and properties.networkAcls.defaultAction == "Allow"
| project name, resourceGroup, location
```

**Explanation:** This helps identify storage accounts that are fully open to public network access (no firewall/network restrictions), which can be a risk in audits or compliance reviews.

## Reference

- [Overview of Azure Resource Graph - Azure Resource Graph | Microsoft Learn](https://learn.microsoft.com/en-us/azure/governance/resource-graph/overview)
- [Quickstart: Run Resource Graph query using Azure portal - Azure Resource Graph | Microsoft Learn](https://learn.microsoft.com/en-us/azure/governance/resource-graph/first-query-portal)
- [Microsoft.Storage/storageAccounts - Bicep, ARM template & Terraform AzAPI reference | Microsoft Learn](https://learn.microsoft.com/en-us/azure/templates/microsoft.storage/storageaccounts?pivots=deployment-language-terraform)

## Note

Although this post focuses on **Azure Storage**, the same approach can be used for other Azure resource types by querying their respective resource schemas via **Azure Resource Graph**.


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-paas-blog/leveraging-azure-resource-graph-queries-for-azure-storage/ba-p/4509184)

