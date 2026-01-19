---
external_url: https://techcommunity.microsoft.com/t5/azure-confidential-computing/securing-confidential-vm-backups-with-azure-recovery-services/ba-p/4458965
title: Securing Azure Confidential VM Backups with Recovery Services Vault and Private Endpoints
author: PramodPalukuru
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-11-29 08:26:10 +00:00
tags:
- Attestation
- Azure Confidential VM
- Azure Monitor
- Backup Extension
- Bicep
- CMK Support
- Confidential Computing
- Customer Managed Keys
- CVM Encryption
- DNS Zone
- Enhanced Backup Policy
- Key Vault
- Managed HSM
- Network Isolation
- PowerShell
- Private Endpoints
- RBAC
- Recovery Services Vault
- REST API
- Terraform
- Zone Redundant Storage
section_names:
- azure
- devops
- security
---
PramodPalukuru details how to secure backups for Azure Confidential VMs using Recovery Services Vault with private endpoints, highlighting security, configuration steps, and best practices for cloud professionals.<!--excerpt_end-->

# Securing Azure Confidential VM Backups with Recovery Services Vault and Private Endpoints

Backing up sensitive workloads running on Azure Confidential VMs (CVMs) requires robust isolation and encryption. This article walks through the technical steps and considerations for deploying secure backups using Recovery Services Vault (RSV) tied to private endpoints, ensuring backup traffic remains inside Microsoft's trusted backbone rather than the public internet.

## What Are Confidential VMs?

- Use hardware-based Trusted Execution Environments (TEEs) like AMD SEV-SNP and Intel TDX to secure workloads
- Support Confidential OS Encryption for OS disk

## Why Combine RSV and Private Endpoints?

- By configuring private endpoints, all backup and restore traffic avoids public networks
- Enhances isolation required for regulated and sensitive business workloads

## Prerequisites

- Azure Subscription with Owner/Contributor permission
- Confidential VM on supported SKUs ([SKU list](https://learn.microsoft.com/en-us/azure/confidential-computing/virtual-machine-options#sizes))
- Recovery Services Vault (RSV) in same or peered region
- Virtual Network/subnet with enough private IP capacity (/25–/27 recommended)
- Private DNS Zones linked: privatelink.backup.windowsazure.com, privatelink.blob.core.windows.net, privatelink.queue.core.windows.net, privatelink.table.core.windows.net
- Enhanced 3-blob backup configuration for latest SKUs
- Azure Backup Private Preview feature (enabled by Product Team)
- Up-to-date Backup extension installed on VM

## Step-by-Step Backup Configuration

1. **Enable Private Preview Feature**
   - Coordinate with Microsoft support/Product Team to add Azure Backup for CVMs Private Preview to your subscription
2. **Create Recovery Services Vault** in your chosen region
3. **Add Private Endpoint**
   - RSV → Networking → Private Endpoint connections
   - Pick VNet/subnet (ensure size: /25–/27 for sufficient IPs)
   - Link required DNS zones
4. **Enable Backup on CVM**
   - VM → Backup → Select RSV & Enhanced Policy
   - Trigger initial backup

## Key Technical Considerations

- Only Enhanced Backup Policies are supported for CVMs with confidential OS disk encryption and customer-managed keys (CMK)
- Deploy RSV as Zone-Redundant (ZRS) for cross-zone restores
- CMK support for backup/restore only available in Private Preview with Product Team enablement
- For non-portal automation (PowerShell/CLI/REST), additional permissions on Key Vault or Managed HSM are required:
  - Key Vault: Assign Get, List, Backup permissions
  - Managed HSM: Assign Managed HSM Crypto User role or custom roles for key read/backup actions
- Assign roles to Backup Management Service

## Restore Methods and Limitations

- Restore to Original location: subscription, resource group, network unchanged
- Restore to Alternate location: zone, vNet, or resource group may differ; ZRS must be used, snapshot restores unsupported across zones
- Disk-level restore: recover specific managed disks, manual VM recreation needed, OS disk replacement on existing VM not supported
- Point-in-Time Restore (via Enhanced Policies)

**Restore Caveats:**

- CMK restores require current Key Vault access/permissions
- Private DNS issues may block recovery operations
- Features may be limited by region or preview program status

## Security Features

- **Network Isolation:** All backup/restore communication over private IPs
- **End-to-End Encryption:** Data encrypted at rest and in transit, optionally by CMK
- **RBAC:** Restricts backup/restore actions to authorized identities
- **Managed Identities:** Simplifies authentication and enhances security

## Known Issues

- DNS misconfigurations are a frequent cause of backup/restore failure
- Extension version compatibility is critical—use the latest Azure Backup extension
- Regional availability for features may be limited
- Private Preview features require manual enablement
- Some latency may occur due to attestation/encryption processes

## Best Practices

- Test backup/restore scenarios on a schedule
- Plan subnet IPs carefully to avoid resource exhaustion
- Use Zone-Redundant RSV for resiliency
- Always apply Enhanced Policies for compliant, point-in-time recovery
- Automate networking and resource setup with Bicep or Terraform
- Monitor health using Azure Monitor and Backup Center
- Engage Microsoft Product Team early in your deployment cycle

## Final Thoughts

This approach combines confidential computing protections for VMs with a secure, auditable backup strategy leveraging Recovery Services Vault and private endpoints. Adhering to best practices for DNS, permissions, and policy configurations ensures reliable recovery and maximum data protection for regulated Azure cloud workloads.

**Author:** PramodPalukuru

_Last updated Nov 29, 2025, Version 1.0_

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-confidential-computing/securing-confidential-vm-backups-with-azure-recovery-services/ba-p/4458965)
