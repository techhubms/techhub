---
layout: post
title: File Share Access for SYSTEM User with PowerShell Across Multiple Devices
author: LordLoss01
canonical_url: https://www.reddit.com/r/AZURE/comments/1mhnybo/file_share_that_the_system_user_can_access/
viewing_mode: external
feed_name: Reddit Azure
feed_url: https://www.reddit.com/r/azure/.rss
date: 2025-08-04 19:57:43 +00:00
permalink: /azure/community/File-Share-Access-for-SYSTEM-User-with-PowerShell-Across-Multiple-Devices
tags:
- Automation
- Domain
- File Permissions
- File Share
- Network
- PowerShell
- Scheduled Tasks
- SYSTEM User
- Windows
section_names:
- azure
- security
---
LordLoss01 seeks advice on creating a file share that allows Windows SYSTEM user access for PowerShell-automated data transfer across thousands of devices.<!--excerpt_end-->

## File Share That the System User Can Access

**Author: LordLoss01**

### Scenario

The author needs to set up a file sharing solution usable by the Windows SYSTEM account for automated uploads of very small files (less than 1 KB) from approximately 3000 devices. The uploads are to be triggered both at system boot and user logon via scheduled PowerShell tasks. The devices may be domain-joined or connected via home networks.

### Requirements

- **Accessible by SYSTEM User:** The share must support connections initiated by the built-in SYSTEM account on Windows.
- **Triggered by Scheduled Task:** PowerShell scripts, running as SYSTEM, will handle the file transfer.
- **Multiple Network Environments:** Devices may be connected to either Active Directory domains or standalone home networks.
- **High Device Count, Small Payloads:** Around 3000 devices sending minimal data.

### Typical Approaches

1. **Traditional Windows File Share (SMB):**
   - Might work well for domain-joined environments where Kerberos authentication is possible.
   - Extra configuration needed for SYSTEM to authenticate when not domain-joined.
   - Access via IP or DNS name—permissions must explicitly allow SYSTEM or another suitable account.

2. **Azure File Shares:**
   - Azure Files provides SMB access and supports integration with Azure Active Directory Domain Services for authentication.
   - For non-domain or hybrid environments, shared access signatures (SAS) or storage account keys could be used to authenticate uploads via REST API or PowerShell scripts.
   - Possible to script uploads using `AzCopy`, PowerShell's `Set-AzStorageFileContent`, or direct REST calls.

3. **Home Network or Standalone Devices:**
   - System accounts typically cannot use personal credentials, so relying on domain authentication is not feasible.
   - Using cloud-based solutions like OneDrive or Azure Blob Storage with explicit access tokens might be a workaround.

### Notes

- **SYSTEM Account Limitations:** SYSTEM usually cannot access network shares requiring user-based authentication unless computer accounts are properly trusted.
- **Recommended for Hybrid:** Azure Files with SAS tokens or storage keys. Store required credentials or tokens locally in a secure manner accessible by SYSTEM or within the scheduled task’s run context.
- **Security Considerations:** Carefully handle credentials or tokens, tightening share permissions to minimize risk.

### Example PowerShell Snippet Using Azure Files

```powershell
# Example: Upload file to Azure File Share using storage account key

$storageAccountName = "<YourStorageAccount>"
$storageAccountKey = "<YourAccountKey>"
$shareName = "<YourShare>"
$localFilePath = "C:\path\to\file.txt"
$destFilePath = "file.txt"

# Install module if needed

# Install-Module -Name Az.Storage

$ctx = New-AzStorageContext -StorageAccountName $storageAccountName -StorageAccountKey $storageAccountKey
Set-AzStorageFileContent -Context $ctx -ShareName $shareName -Source $localFilePath -Path $destFilePath
```

### Summary

Selecting a file share setup that works for both domain and non-domain devices (where SYSTEM user must operate) points toward cloud-based solutions like Azure Files with permission model adjustments or token-based authentication for uploads.

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mhnybo/file_share_that_the_system_user_can_access/)
