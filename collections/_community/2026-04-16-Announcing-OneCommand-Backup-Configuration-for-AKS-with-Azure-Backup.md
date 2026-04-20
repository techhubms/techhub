---
tags:
- AKS
- Automation
- Az AKS
- Az Dataprotection
- Az Kubernetes Extension
- Azure
- Azure Backup
- Azure CLI
- Backup Extension
- Backup Instance
- Backup Policy
- Backup Vault
- CI/CD Pipelines
- Community
- Cross Region DR
- DevOps
- Disaster Recovery
- Infrastructure Automation
- Kubernetes
- Persistent Volumes
- RBAC
- Stateful Workloads
- Storage Account
- Trusted Access
section_names:
- azure
- devops
title: Announcing One‑Command Backup Configuration for AKS with Azure Backup
date: 2026-04-16 07:50:59 +00:00
external_url: https://techcommunity.microsoft.com/t5/azure-governance-and-management/announcing-one-command-backup-configuration-for-aks-with-azure/ba-p/4511852
feed_name: Microsoft Tech Community
author: shobhitgarg
primary_section: azure
---

shobhitgarg describes a new Azure CLI experience that enables Azure Backup for Azure Kubernetes Service (AKS) with a single command, reducing the previous multi-step onboarding (extensions, vaults, policies, trusted access) into an automation-friendly workflow for platform teams.<!--excerpt_end-->

# Announcing One‑Command Backup Configuration for AKS with Azure Backup

Running production workloads on **Azure Kubernetes Service (AKS)** is increasingly common for platform teams building cloud-native apps. As AKS clusters host more **stateful workloads** using **persistent volumes**, data protection and recovery become critical.

This post introduces an alternate **simplified CLI-based experience** to configure **AKS backups using Azure Backup with a single command**.

## The challenge with AKS backup onboarding today

Previously, enabling backup for an AKS cluster via Azure CLI meant coordinating multiple CLI domains:

- `az aks`
- `az k8s-extension`
- `az dataprotection`

A typical “vaulted backup” setup required coordinating items like:

- Extension installation
- Storage account provisioning
- Backup vault creation
- Policy configuration
- Trusted access setup
- Backup instance initialization

In practice, this could mean **8 lifecycle steps across 15+ CLI commands**, which is painful when managing many clusters through automation or CI/CD.

## A simpler way: configure backup in one CLI command

You can now enable full-cluster backup for AKS with a single Azure CLI command:

```sh
az dataprotection enable-backup trigger \
  --datasource-type AzureKubernetesService \
  --datasource-id <cluster-arm-id> \
  --backup-strategy <strategy> \
  --backup-configuration-file @config.json
```

Behind the scenes, the command orchestrates the workflow by automatically performing these steps:

- Validate AKS cluster existence and running state
- Create or identify region-specific backup resource group
- Check if Backup Extension is already installed on the cluster
- Install Backup Extension (if not present)
- Create or reuse Storage Account for backup data
- Create or reuse Backup Vault
- Create or reuse Backup Policy
- Enable Trusted Access between vault and cluster
- Initialize and create Backup Instance

The goal is to remove the need to manually orchestrate resources across different CLI “surfaces”.

## Backup strategy presets

You can pick predefined strategies:

| Strategy | Op Store Retention | Vault Store Retention | Use Case |
| --- | --- | --- | --- |
| Week (default) | 7 days | — | Dev/Test |
| Month | 30 days | — | Production |
| DisasterRecovery | 7 days | 90 days | Cross-region DR |
| Custom | User-defined | User-defined | BYO Vault & Policy |

Example:

```sh
az dataprotection enable-backup trigger \
  --datasource-type AzureKubernetesService \
  --datasource-id <cluster-arm-id> \
  --backup-strategy DisasterRecovery
```

## Backup configuration JSON (advanced customization)

Optionally, provide a configuration JSON file to:

- Use existing vaults or policies
- Bring your own storage account
- Apply enterprise tags
- Use custom backup resource groups

### Supported parameters

| Parameter | When Required | Description |
| --- | --- | --- |
| `backupVaultId` | Custom strategy | Use existing vault |
| `backupPolicyId` | Custom strategy | Use existing policy |
| `storageAccountResourceId` | Optional | Use existing storage account |
| `blobContainerName` | Optional | Custom container |
| `backupResourceGroupId` | Optional | Use existing resource group |
| `tags` | Optional | Apply to created resources |

## Built-in validations for reliability

Before enabling backup, the CLI validates:

- Cluster existence
- Running state
- Backup compatibility
- Required RBAC permissions
- Resource availability (if provided)

## Faster time-to-protection for AKS workloads

Collapsing a multi-step setup into a single command is intended to make backup onboarding:

- More automation-friendly
- More consistent across environments
- Less error-prone than manual orchestration
- Faster to roll out across large AKS estates

## What’s next

The single-command AKS backup enablement is described as part of a broader effort to make Azure Backup more automation-friendly for cloud-native/platform workloads, with plans to extend similar CLI experiences to other Azure Backup-supported workloads.

For details, see: how to configure AKS backup using a single CLI command: https://learn.microsoft.com/en-us/azure/backup/azure-kubernetes-service-cluster-backup-using-cli#configure-backup-using-a-single-azure-cli-command


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-governance-and-management/announcing-one-command-backup-configuration-for-aks-with-azure/ba-p/4511852)

