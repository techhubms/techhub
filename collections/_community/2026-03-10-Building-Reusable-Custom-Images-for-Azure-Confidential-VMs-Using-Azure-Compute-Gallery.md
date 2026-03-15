---
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/building-reusable-custom-images-for-azure-confidential-vms-using/ba-p/4500880
title: Building Reusable Custom Images for Azure Confidential VMs Using Azure Compute Gallery
author: PramodPalukuru
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-03-10 12:09:24 +00:00
tags:
- Azure
- Azure Compute Gallery
- Azure Confidential VM
- BitLocker
- Community
- Custom Images
- Customer Managed Keys
- Defender For Endpoint
- Disk Encryption Set
- Enterprise Deployment
- Golden Image
- Image Generalization
- Platform Managed Keys
- Policy Compliance
- PowerShell
- Secure Boot
- Security
- Security Baseline
- Sysprep
- Trusted Launch
- Vtpm
- Windows Server
section_names:
- azure
- security
---
PramodPalukuru demonstrates how to create hardened, reusable custom images for Azure Confidential Virtual Machines, guiding you through secure image creation, compliance with PMK and CMK, and enterprise deployment using Azure Compute Gallery.<!--excerpt_end-->

# Building Reusable Custom Images for Azure Confidential VMs Using Azure Compute Gallery

## Overview

Azure Confidential Virtual Machines (CVMs) deliver hardware-based protection for sensitive workloads by encrypting data in use with AMD SEV-SNP technology. Enterprises require the ability to create hardened images, standardize VM baselines, support both Platform Managed Keys (PMK) and Customer Managed Keys (CMK), and replicate images across regions.

This guide details a complete, production-ready workflow for building reusable custom images for Confidential VMs, using tools like PowerShell (Az module), Azure Portal, Disk Encryption Sets, and Azure Compute Gallery.

## Key Design Principles

1. **The Same Image Supports PMK and CMK:**
    - The encryption model is not embedded in the image; encryption happens during VM deployment through disk configuration.
    - Build one golden image and deploy it with PMK or CMK depending on compliance needs, simplifying lifecycle management.

2. **Confidential VM Images Require Source VHD:**
    - Azure Compute Gallery mandates the use of a Source VHD for Confidential Security Type support. This is essential when publishing images for Confidential VMs.

## Security Stack Breakdown

| Protection Area      | Technology                              |
|---------------------|-----------------------------------------|
| Data in Use         | AMD SEV-SNP                             |
| Boot Integrity      | Secure Boot + vTPM                      |
| Image Lifecycle     | Azure Compute Gallery                    |
| Disk Encryption     | Platform Managed Keys or Customer Managed Keys |
| Compliance Control  | Disk Encryption Set (CMK)                |

# Implementation Steps

## Step 1 – Deploy a Base Windows Confidential VM

- Use Gen2 image and confidential SKUs (e.g., DCasv5/ECasv5)
- Set SecurityType to ConfidentialVM
- Enable Secure Boot and vTPM
- Turn on Confidential OS Encryption

**PowerShell example:**

```powershell
$rg = "rg-cvm-gi-pr-sbx-01"
$location = "NorthEurope"
$vmName = "cvmwingiprsbx01"
New-AzResourceGroup -Name $rg -Location $location
$cred = Get-Credential
$vmConfig = New-AzVMConfig -VMName $vmName -VMSize "Standard_DC2as_v5" -SecurityType "ConfidentialVM"
$vmConfig = Set-AzVMOperatingSystem -VM $vmConfig -Windows -ComputerName $vmName -Credential $cred
$vmConfig = Set-AzVMSourceImage -VM $vmConfig -PublisherName "MicrosoftWindowsServer" -Offer "WindowsServer" -Skus "2022-datacenter-azure-edition" -Version "latest"
$vmConfig = Set-AzVMOSDisk -VM $vmConfig -CreateOption FromImage -SecurityEncryptionType "ConfidentialVM_DiskEncryptedWithPlatformKey"
New-AzVM -ResourceGroupName $rg -Location $location -VM $vmConfig
```

## Step 2 – Harden and Customize the OS

- Install monitoring and security agents (e.g., Defender for Endpoint)
- Apply CIS baseline and remove unnecessary services
- Add application dependencies as needed

## Step 3 – Generalize the Windows Confidential VM

- Remove Panther folder: `rd /s /q C:\Windows\Panther`
- Run Sysprep: `sysprep.exe /generalize /shutdown`
- If Sysprep fails due to BitLocker:
    - Check BitLocker status: `manage-bde -status`
    - Disable BitLocker: `manage-bde -off C:` and wait for 100% decryption
    - Reboot and run Sysprep again
- Mark the VM as generalized in Azure:

```powershell
Stop-AzVM -Name $vmName -ResourceGroupName $rg -Force
Set-AzVM -Name $vmName -ResourceGroupName $rg -Generalized
```

## Step 4 – Export OS Disk as VHD

- Generate SAS URL for the OS Disk of the VM
- Copy OS disk to a storage account as a VHD file
- Check copy status with `Get-AzStorageBlobCopyState`

## Step 5 – Create Azure Compute Gallery and Image Version

1. Create Azure Compute Gallery:

```powershell
$galleryName = "cvmImageGallery"
New-AzGallery -GalleryName $galleryName -ResourceGroupName $rg -Location $location -Description "Confidential VM Image Gallery"
```

1. Create Image Definition specifying Gen2 and security type as TrustedLaunchAndConfidentialVmSupported.
2. Publish a gallery image version from the VHD (use Azure Portal for this step). Enable regional replication as required.

**Why Azure Compute Gallery:** Enables versioning, cross-region replication, enterprise lifecycle management, and is recommended for production.

## Step 6 – Deploy Confidential VM from Gallery Image

- **Using PMK (default):** Deploy without specifying a Disk Encryption Set; Azure uses Platform Managed Keys.
- **Using CMK:** Create a Disk Encryption Set, link to Key Vault or Managed HSM, and attach DES at deployment.

**Sample (PMK):**

```powershell
$imageId = (Get-AzGalleryImageVersion -GalleryName $galleryName -GalleryImageDefinitionName $imageDefName -ResourceGroupName $rg -Name "1.0.0").Id
$vmConfig = New-AzVMConfig -VMName "cvmwingiprsbx02" -VMSize "Standard_DC2as_v5" -SecurityType "ConfidentialVM"
$vmConfig = Set-AzVMOSDisk -VM $vmConfig -CreateOption FromImage -SecurityEncryptionType "ConfidentialVM_DiskEncryptedWithPlatformKey"
$vmConfig = Set-AzVMSourceImage -VM $vmConfig -Id $imageId
$vmConfig = Set-AzVMOperatingSystem -VM $vmConfig -Windows -ComputerName "cvmwingiprsbx02" -Credential (Get-Credential)
New-AzVM -ResourceGroupName $rg -Location $location -VM $vmConfig
```

**Sample (CMK):**

```powershell
$vmConfig = Set-AzVMOSDisk -VM $vmConfig -CreateOption FromImage -SecurityEncryptionType "ConfidentialVM_DiskEncryptedWithCustomerKey" -DiskEncryptionSetId $des.Id
```

- Validate security with:

```powershell
Get-AzVM -Name "cvmwingiprsbx02" -ResourceGroupName $rg | Select SecurityProfile
Get-AzDisk -ResourceGroupName $rg
```

# Architectural Summary

- Confidential VM security is independent of the chosen disk encryption model
- Encryption choice is applied at deployment, not image creation
- One golden image supports multiple compliance scenarios (PMK or CMK)
- Source VHD is required for Confidential VM gallery publishing
- Azure Compute Gallery streamlines versioning, replication, and management

## PMK vs CMK Decision Matrix

| Scenario                       | Recommended Model |
|--------------------------------|------------------|
| Standard enterprise workloads  | PMK              |
| Financial services/regulated   | CMK              |
| BYOK requirement               | CMK              |
| Simplicity prioritized         | PMK              |

## Enterprise Recommendations

- Use Azure Compute Gallery for all Confidential VM builds
- Implement semantic versioning (e.g., 1.0.0, 1.0.1)
- Automate image pipeline with Azure Image Builder
- Enforce Confidential VM usage via Azure Policy
- Enable Guest Attestation
- Integrate Defender for Cloud for advanced monitoring

# Final Thoughts

Creating custom images for Azure Confidential VMs gives organizations both security and efficiency. By following these best practices—versioning, security hardening, and leveraging platform features—enterprises can deploy consistent, compliant, and secure confidential workloads at scale.

*Author: PramodPalukuru*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/building-reusable-custom-images-for-azure-confidential-vms-using/ba-p/4500880)
