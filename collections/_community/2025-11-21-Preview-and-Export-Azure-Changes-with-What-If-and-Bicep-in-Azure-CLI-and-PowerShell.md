---
layout: "post"
title: "Preview and Export Azure Changes with 'What If' and Bicep in Azure CLI & PowerShell"
description: "This article introduces two new Azure features: 'What if' and 'Export Bicep' in Azure CLI and PowerShell. These tools allow users to preview the effects of their commands and export those commands as Bicep templates, enabling safer, more efficient infrastructure management and adoption of Infrastructure as Code practices, all currently available in private preview."
author: "stevenbucher"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-tools-blog/gaining-confidence-with-az-cli-and-az-powershell-introducing/ba-p/4472147"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-21 18:39:06 +00:00
permalink: "/community/2025-11-21-Preview-and-Export-Azure-Changes-with-What-If-and-Bicep-in-Azure-CLI-and-PowerShell.html"
categories: ["AI", "Azure", "DevOps"]
tags: ["AI", "AI Translation", "Azure", "Azure CLI", "Azure Networking", "Azure PowerShell", "Azure Storage Account", "Azure VM", "Bicep Templates", "Change Preview", "Cloud Automation", "Community", "DevOps", "DryRun", "Export Bicep", "IaC", "Private Preview", "Resource Deployment", "What If"]
tags_normalized: ["ai", "ai translation", "azure", "azure cli", "azure networking", "azure powershell", "azure storage account", "azure vm", "bicep templates", "change preview", "cloud automation", "community", "devops", "dryrun", "export bicep", "iac", "private preview", "resource deployment", "what if"]
---

Steven Bucher discusses Azure CLI and PowerShell's new 'What if' and 'Export Bicep' features, helping practitioners safely preview command impacts and generate reusable Bicep templates.<!--excerpt_end-->

# Preview and Export Azure Changes with 'What If' and Bicep in Azure CLI & PowerShell

Azure practitioners often hesitate before running commands, worrying about unintended changes to resources. To address these concerns, Microsoft has introduced two key features in Azure CLI and PowerShell: **What if** and **Export Bicep**. These tools empower users to preview resource modifications and generate reusable Bicep templates—all before executing changes.

## Why These Features Matter

- **Reduce risk**: Prevent accidental deletions or unwanted resource changes.
- **Increase transparency**: See exactly what will change in your environment.
- **Adopt Infrastructure as Code (IaC)**: Instantly convert CLI/PowerShell commands into Bicep templates.
- **Boost productivity**: Validate scripts without trial-and-error deployments.

## How It Works

### What if Command Preview

- **Azure CLI**: Add `--what-if` to preview the effects of a command.

  ```shell
  az storage account create --name "mystorageaccount" --resource-group "myResourceGroup" --location "eastus" --what-if
  ```

- **Azure PowerShell**: Use `-DryRun` to see operation results.

  ```powershell
  New-AzVirtualNetwork -Name MyVNET -ResourceGroupName MyResourceGroup -Location eastus -AddressPrefix "10.0.0.0/16" -DryRun
  ```

### Exporting Commands to Bicep

Use `--export-bicep` with `--what-if` to auto-generate a Bicep template representing your CLI/PowerShell command. The template is saved to `~/.azure/whatif`. AI services translate your command to Bicep, and the system automatically previews the Bicep execution, showing expected resource changes before any action is taken.

## Getting Started with Private Preview

These features are available in **private preview**:

1. Visit [aka.ms/PreviewSignupPSCLI](http://aka.ms/PreviewSignupPSCLI) to request access.
2. Approved users receive download instructions for the preview packages.

### Supported Commands (Private Preview)

**Azure CLI:**

- az vm create / update
- az storage account/container/share create
- az network vnet create / update
- az storage account network-rule add
- az vm disk attach / detach
- az vm nic remove

**Azure PowerShell:**

- New-AzVM / Update-AzVM
- New-AzStorageAccount
- New-AzRmStorageShare / New-AzRmStorageContainer
- New-AzVirtualNetwork / Set-AzVirtualNetwork
- Add-AzStorageAccountNetworkRule

## Next Steps

- Sign up for the private preview.
- Install preview packages as instructed.
- Start using `--what-if`, `-DryRun`, and `--export-bicep` to safely manage Azure changes and accelerate your Infrastructure as Code journey.
- Share feedback at [aka.ms/PreviewFeedbackWhatIf](https://aka.ms/PreviewFeedbackWhatIf).

---

*Article by Steven Bucher, PM for Azure Client Tools, published Nov 21, 2025.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-tools-blog/gaining-confidence-with-az-cli-and-az-powershell-introducing/ba-p/4472147)
