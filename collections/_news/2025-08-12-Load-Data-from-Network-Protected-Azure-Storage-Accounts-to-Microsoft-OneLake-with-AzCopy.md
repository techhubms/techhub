---
external_url: https://blog.fabric.microsoft.com/en-US/blog/load-data-from-network-protected-azure-storage-accounts-to-microsoft-onelake-with-azcopy/
title: Load Data from Network-Protected Azure Storage Accounts to Microsoft OneLake with AzCopy
author: Microsoft Fabric Blog
viewing_mode: external
feed_name: Microsoft Fabric Blog
date: 2025-08-12 11:30:00 +00:00
tags:
- ARM Template
- AzCopy
- Azure Data Engineering
- Azure Storage
- Cloud Data Integration
- Cloud Security
- Data Movement
- Data Pipeline
- Fabric
- Firewall
- Managed Identity
- Microsoft OneLake
- OneLake Shortcut
- Resource Instance Rule
- Trusted Workspace Access
section_names:
- azure
---
Microsoft Fabric Blog explains how to use AzCopy for transferring data from firewall-enabled Azure Storage accounts to Microsoft OneLake, highlighting secure access setup and performance best practices.<!--excerpt_end-->

# Load Data from Network-Protected Azure Storage Accounts to Microsoft OneLake with AzCopy

AzCopy is a powerful command-line tool designed for copying large datasets between Azure Storage and Microsoft OneLake. This guide demonstrates how to use AzCopy to transfer data securely from firewall-enabled Azure Storage accounts directly into OneLake, leveraging recent improvements for trusted workspace access.

## Key Advantages of AzCopy

- **Performance**: Supports high-throughput, large-scale data movement
- **Security**: Now compatible with firewall-protected storage using trusted workspace access
- **Ease of Use**: Simplifies complex data transfer scenarios with straightforward commands

## What is Trusted Workspace Access?

Trusted workspace access enables secure communication between Microsoft Fabric workspaces and firewall-enabled Azure Storage accounts. By registering a workspace managed identity and creating a corresponding resource instance rule on the storage account, Fabric services (including OneLake shortcuts, pipelines, DW copy, semantic models, and AzCopy operations) can directly access protected storage.

### Setup Steps

1. **Create a Workspace Managed Identity**
   - Configure the identity in workspace settings.
   - [More information](https://blog.fabric.microsoft.com/en-us/blog/load-data-from-network-protected-azure-storage-accounts-to-microsoft-onelake-with-azcopy/)
2. **Add a Resource Instance Rule to Azure Storage**
   - Grant the workspace identity explicit access via the firewall’s resource instance rules.
   - Sample ARM templates and configuration guidance are available in Fabric documentation.
3. **Copy Data Using AzCopy**
   - Reference your managed identity and set trusted suffixes in your command:

   ```bash
   azcopy copy \
   "https://contosostorage.dfs.core.windows.net/sales/customers.csv" \
   "http://onelake.dfs.fabric.microsoft.com/contosoWorkspace/contosoLakehouse.Lakehouse/Files/sales/customers.csv" \
   --trusted-microsoft-suffixes "fabric.microsoft.com"
   ```

## Further Learning

- [Get started with AzCopy](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azcopy-v10?tabs=dnf)
- [Copy data with OneLake and AzCopy](https://blog.fabric.microsoft.com/en-us/blog/load-data-from-network-protected-azure-storage-accounts-to-microsoft-onelake-with-azcopy/)
- [Use trusted workspace access](https://blog.fabric.microsoft.com/en-us/blog/load-data-from-network-protected-azure-storage-accounts-to-microsoft-onelake-with-azcopy/)

By following these steps, organizations can ensure efficient, secure, and compliant data transfer workflows between Azure Storage and OneLake for analytics and data warehousing needs.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/load-data-from-network-protected-azure-storage-accounts-to-microsoft-onelake-with-azcopy/)
